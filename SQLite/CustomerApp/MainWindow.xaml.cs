using CustomerApp.Data;
using Microsoft.Win32;
using SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerApp {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        OpenFileDialog ofd;

        private List<Customer> _customer = new List<Customer>();
        public MainWindow() {
            InitializeComponent();
            ReadDatabase();
            CustomerListView.ItemsSource = _customer;
        }
        
        private void ReadDatabase() {
            using (var connection = new SQLiteConnection(App.databasePath)) {
                connection.CreateTable<Customer>();
                _customer = connection.Table<Customer>().ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            ofd = new OpenFileDialog();
            if (ofd.ShowDialog() ?? false) {
                PictureImageBox.Source = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            Customer customer = new Customer();

            //customer.Picture = File.ReadAllBytes(ofd.FileName);

            customer.Picture = ImageSourceToByteArray(PictureImageBox.Source);

            using (var connection = new SQLiteConnection(App.databasePath)) {
                connection.CreateTable<Customer>();
                connection.Insert(customer);
            }
            ReadDatabase();
            CustomerListView.ItemsSource = _customer;
        }

        public static byte[] ImageSourceToByteArray(ImageSource imageSource) {
            if (imageSource == null) {
                return null;
            }

            byte[] byteArray = null;
            // MemoryStreamを作成
            using (var stream = new MemoryStream()) {
                // PngEncoderを作成
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
                // MemoryStreamにエンコードを保存
                encoder.Save(stream);
                // MemoryStreamの内容をbyte配列として取得
                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (CustomerListView.SelectedIndex != -1) {
                if (_customer[CustomerListView.SelectedIndex].Picture != null) {
                    PictureImageBox.Source = byteToBitmap(_customer[CustomerListView.SelectedIndex].Picture);
                } else {
                    PictureImageBox.Source = null;
                }
            }
        }

        public static BitmapImage byteToBitmap(byte[] bytes) {
            var result = new BitmapImage();

            using (var stream = new MemoryStream(bytes)) {
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.CreateOptions = BitmapCreateOptions.None;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();    // 非UIスレッドから作成する場合、Freezeしないとメモリリークするため注意
            }
            return result;
        }
    }
}