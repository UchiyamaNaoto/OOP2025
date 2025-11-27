using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace TenkiApp {
    public partial class MainWindow : Window {
        private static readonly HttpClient httpClient = new HttpClient();

        // テーマ状態（false = ライト, true = ダーク）
        private bool _isDark = false;

        public MainWindow() {
            InitializeComponent();
            StartClock();

            // 起動時にライトテーマ適用
            ApplyLightTheme();

            // User-Agent を設定（現在地API対策）
            if (!httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("TenkiApp/1.0")) {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
            }

            // 起動時は現在地で取得
            _ = LoadWeatherForCurrentLocationAsync();
        }

        // ==============================
        // テーマ切り替え
        // ==============================

        private void ThemeButton_Click(object sender, RoutedEventArgs e) {
            _isDark = !_isDark;

            if (_isDark) {
                ApplyDarkTheme();
                ThemeButton.Content = "ライトテーマ";
            } else {
                ApplyLightTheme();
                ThemeButton.Content = "ダークテーマ";
            }
        }

        /// <summary>
        /// テーマ用ブラシを一括設定
        /// （XAML の AccentBrush / BackgroundBrush / CardBrush / MainTextBrush / SubTextBrush に対応）
        /// </summary>
        private void SetTheme(
            string accentColor,
            string backgroundColor,
            string cardColor,
            string mainTextColor,
            string subTextColor) {

            Resources["AccentBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(accentColor));
            Resources["BackgroundBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(backgroundColor));
            Resources["CardBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(cardColor));
            Resources["MainTextBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(mainTextColor));
            Resources["SubTextBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(subTextColor));
        }

        // ライトテーマ
        private void ApplyLightTheme() {
            SetTheme(
                accentColor: "#4FC3F7",   // 青
                backgroundColor: "#FFFFFF",   // 白
                cardColor: "#FDFDFD",
                mainTextColor: "#222222",
                subTextColor: "#777777"
            );
        }

        // ダークテーマ
        private void ApplyDarkTheme() {
            SetTheme(
                accentColor: "#BB86FC",   // 紫
                backgroundColor: "#121212",   // ほぼ黒
                cardColor: "#1E1E1E",
                mainTextColor: "#EEEEEE",
                subTextColor: "#BBBBBB"
            );
        }

        // ==============================
        // 時計表示
        // ==============================
        private void StartClock() {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (_, __) => {
                TimeText.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            };
            timer.Start();
        }

        // ==============================
        // UIイベント
        // ==============================
        private async void SearchButton_Click(object sender, RoutedEventArgs e) {
            await LoadWeatherByCityAsync(CityTextBox.Text);
        }

        private async void QuickCity_Click(object sender, RoutedEventArgs e) {
            if (sender is Button btn && btn.Tag is string city) {
                CityTextBox.Text = city;
                await LoadWeatherByCityAsync(city);
            }
        }

        private async void CurrentLocationButton_Click(object sender, RoutedEventArgs e) {
            await LoadWeatherForCurrentLocationAsync();
        }

        // ==============================
        // メイン処理：都市名から天気取得
        // ==============================
        private async Task LoadWeatherByCityAsync(string cityName) {
            if (string.IsNullOrWhiteSpace(cityName)) {
                StatusText.Text = "都市名を入力してください。";
                return;
            }

            try {
                StatusText.Text = $"「{cityName}」の位置情報取得中...";
                var (lat, lon, resolvedName, timezone) = await GeocodeAsync(cityName);

                StatusText.Text = "天気情報を取得中...";
                var forecast = await GetForecastAsync(lat, lon, timezone);

                UpdateCurrentWeatherUi(resolvedName, forecast);
                UpdateHourlyUi(forecast);

                StatusText.Text = $"更新完了：{resolvedName}";
            }
            catch (Exception ex) {
                StatusText.Text = $"エラー：{ex.Message}";
            }
        }

        // ==============================
        // 現在地から天気取得
        // ==============================
        private async Task LoadWeatherForCurrentLocationAsync() {
            try {
                StatusText.Text = "現在地から天気を取得中...";

                var (lat, lon, locationName, timezone) = await GetCurrentLocationByIpAsync();

                CityTextBox.Text = locationName;

                var forecast = await GetForecastAsync(lat, lon, timezone);

                UpdateCurrentWeatherUi(locationName, forecast);
                UpdateHourlyUi(forecast);

                StatusText.Text = $"現在地の天気を表示中：{locationName}";
            }
            catch (Exception ex) {
                StatusText.Text = $"現在地取得エラー：{ex.Message}";
            }
        }

        private async Task<(double lat, double lon, string displayName, string timezone)> GetCurrentLocationByIpAsync() {
            var url = "https://ipapi.co/json/";

            HttpResponseMessage resp;
            try {
                resp = await httpClient.GetAsync(url);
            }
            catch (Exception ex) {
                throw new Exception("現在地APIに接続できませんでした：" + ex.Message);
            }

            if (!resp.IsSuccessStatusCode) {
                throw new Exception($"現在地APIエラー: {(int)resp.StatusCode} {resp.ReasonPhrase}");
            }

            var json = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<IpLocationResponse>(json, options);
            if (data == null || string.IsNullOrWhiteSpace(data.city)) {
                throw new Exception("現在地の解析に失敗しました。");
            }

            string name = data.city;
            if (!string.IsNullOrEmpty(data.country_name)) {
                name += $"（{data.country_name}）";
            }

            return (data.latitude, data.longitude, name, data.timezone);
        }

        // ==============================
        // UI更新（現在の天気）
        // ==============================
        private void UpdateCurrentWeatherUi(string locationName, ForecastResponse forecast) {
            if (forecast.current == null) {
                CurrentConditionText.Text = "現在の天気データ無し";
                return;
            }

            LocationText.Text = locationName;
            TimezoneText.Text = string.IsNullOrEmpty(forecast.timezone) ? "" : $"（{forecast.timezone}）";

            CurrentTempText.Text = $"{forecast.current.temperature_2m:F1} ℃";

            var (icon, textJa) = WeatherCodeToIconAndText(forecast.current.weather_code);
            CurrentIconText.Text = icon;
            CurrentConditionText.Text = textJa;

            CurrentTimeDetailText.Text = forecast.current.time;
            CurrentCodeText.Text = $"weather_code: {forecast.current.weather_code}";

            SummaryText.Text = "Open-Meteo 現在値";
        }

        // ==============================
        // UI更新（時間別）
        // ==============================
        private void UpdateHourlyUi(ForecastResponse forecast) {
            if (forecast.hourly == null ||
                forecast.hourly.time == null ||
                forecast.hourly.temperature_2m == null ||
                forecast.hourly.weather_code == null) {
                HourlyList.ItemsSource = null;
                return;
            }

            var items = new List<HourlyForecastItem>();
            int count = Math.Min(10, forecast.hourly.time.Count);

            for (int i = 0; i < count; i++) {
                if (!DateTime.TryParse(forecast.hourly.time[i], out var t)) continue;

                var (icon, _) = WeatherCodeToIconAndText(forecast.hourly.weather_code[i]);

                items.Add(new HourlyForecastItem {
                    Time = t.ToString("HH時"),
                    Temperature = $"{forecast.hourly.temperature_2m[i]:F1} ℃",
                    Icon = icon
                });
            }

            HourlyList.ItemsSource = items;
        }

        // ==============================
        // Open-Meteo Geocoding API
        // ==============================
        private async Task<(double lat, double lon, string displayName, string timezone)> GeocodeAsync(string cityName) {
            var url =
                "https://geocoding-api.open-meteo.com/v1/search" +
                $"?name={Uri.EscapeDataString(cityName)}" +
                "&count=1&language=ja&format=json";

            using var resp = await httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var data = JsonSerializer.Deserialize<GeocodingResponse>(json, options);
            if (data?.results == null || data.results.Count == 0) {
                throw new Exception("場所が見つかりませんでした。");
            }

            var r = data.results[0];
            string name = r.name;
            if (!string.IsNullOrEmpty(r.country)) {
                name += $"（{r.country}）";
            }

            return (r.latitude, r.longitude, name, r.timezone);
        }

        // ==============================
        // Open-Meteo Forecast API
        // ==============================
        private async Task<ForecastResponse> GetForecastAsync(double lat, double lon, string timezone) {
            var url =
                "https://api.open-meteo.com/v1/forecast" +
                $"?latitude={lat}&longitude={lon}" +
                "&current=temperature_2m,weather_code" +
                "&hourly=temperature_2m,weather_code" +
                "&forecast_days=1" +
                "&timezone=auto";

            using var resp = await httpClient.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<ForecastResponse>(json, options);

            if (data == null) {
                throw new Exception("天気情報の解析に失敗しました。");
            }
            return data;
        }

        // ==============================
        // weather_code → アイコン＆日本語
        // ==============================
        private (string Icon, string TextJa) WeatherCodeToIconAndText(int code) {
            return code switch {
                0 => ("☀", "快晴"),
                1 => ("🌤", "ほぼ晴れ"),
                2 => ("⛅", "晴れ時々くもり"),
                3 => ("☁", "くもり"),
                45 or 48 => ("🌫", "霧"),
                51 or 53 or 55 => ("🌦", "霧雨"),
                56 or 57 => ("🌧", "氷の霧雨"),
                61 or 63 or 65 => ("🌧", "雨"),
                66 or 67 => ("🌧", "氷雨"),
                71 or 73 or 75 or 77 => ("🌨", "雪"),
                80 or 81 or 82 => ("🌧", "にわか雨"),
                85 or 86 => ("🌨", "にわか雪"),
                95 or 96 or 99 => ("⛈", "雷雨"),
                _ => ("❓", $"不明（{code}）")
            };
        }
    }

    // ==== モデルクラス群 ====

    public class GeocodingResponse {
        public List<GeocodingResult> results { get; set; } = new();
    }

    public class GeocodingResult {
        public string name { get; set; } = "";
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string country { get; set; } = "";
        public string timezone { get; set; } = "";
    }

    public class ForecastResponse {
        public string timezone { get; set; } = "";
        public CurrentWeather? current { get; set; }
        public HourlyWeather? hourly { get; set; }
    }

    public class CurrentWeather {
        public string time { get; set; } = "";
        public double temperature_2m { get; set; }
        public int weather_code { get; set; }
    }

    public class HourlyWeather {
        public List<string> time { get; set; } = new();
        public List<double> temperature_2m { get; set; } = new();
        public List<int> weather_code { get; set; } = new();
    }

    public class HourlyForecastItem {
        public string Time { get; set; } = "";
        public string Temperature { get; set; } = "";
        public string Icon { get; set; } = "";
    }

    public class IpLocationResponse {
        public string ip { get; set; } = "";
        public string city { get; set; } = "";
        public string region { get; set; } = "";
        public string country_name { get; set; } = "";
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string timezone { get; set; } = "";
    }
}
