using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ColorChecker {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        MyColor currentColor;   //現在設定している色情報

        public MainWindow() {
            InitializeComponent();
            DataContext = GetColorList();
        }

        /// <summary>
        /// すべての色を取得するメソッド
        /// </summary>
        /// <returns></returns>
        private MyColor[] GetColorList() {
            return typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(i => new MyColor() { Color = (Color)i.GetValue(null), Name = i.Name }).ToArray();
        }

        //すべてのスライダーから呼ばれるイベントハンドラ
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            //colorAreaの色（背景色）は、スライダーで指定したRGBの色を表示する
            var myColor = new MyColor { Color = Color.FromRgb((byte)rSlider.Value, (byte)gSlider.Value, (byte)bSlider.Value),
                Name = string.Empty };
            currentColor.Color = myColor.Color;
            currentColor.Name = ((MyColor[])DataContext).Where(c => c.Color.Equals(currentColor.Color))
                                                                    .Select(x => x.Name).FirstOrDefault();
            int i;
            for ( i = 0; i < ((MyColor[])DataContext).Length; i++) {
                if (((MyColor[])DataContext)[i].Color.Equals(currentColor.Color)) {
                    currentColor.Name = ((MyColor[])DataContext)[i].Name;
                    break; //色データが見つかった
                }
            }
            colorSelectComboBox.SelectedIndex = i < ((MyColor[])DataContext).Length ? i : -1;
            colorArea.Background = new SolidColorBrush(myColor.Color);
        }

        private void stockList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            colorArea.Background = new SolidColorBrush(((MyColor)stockList.Items[stockList.SelectedIndex]).Color);
            setSliderValue(((MyColor)stockList.Items[stockList.SelectedIndex]).Color);
        }

        private void stockButton_Click(object sender, RoutedEventArgs e) {

            //既に登録されている場合は登録しない
            if (!stockList.Items.Contains((MyColor)currentColor)) {
                stockList.Items.Insert(0, currentColor);
            } else {
                MessageBox.Show("既に登録済みです！", "ColorChecker", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //コンボボックスから色を選択
        private void colorSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (((ComboBox)sender).SelectedIndex == -1) return;

            var comboSelectMyColor = (MyColor)((ComboBox)sender).SelectedItem;
            setSliderValue(comboSelectMyColor.Color);   //スライダーを設定

            currentColor.Name = comboSelectMyColor.Name;
        }

        //各スライダーの値を設定する
        private void setSliderValue(Color color) {
            rSlider.Value = color.R;
            gSlider.Value = color.G;
            bSlider.Value = color.B;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            colorSelectComboBox.SelectedIndex = 7;
        }
    }
}
