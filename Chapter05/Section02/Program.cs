namespace Section02 {
    internal class Program {
        static void Main(string[] args) {

            var appVer1 = new AppVersion(5, 1);
            var appVer2 = new AppVersion(5, 1);

            Console.WriteLine(appVer1);

        }
    }

    //プライマリーコンストラクタを使ったクラス定義
    public record AppVersion(int Major, int Minor, int Build = 0, int Revision = 0);



}
