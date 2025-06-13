
namespace Exercise01 {
    internal class Program {
        static void Main(string[] args) {
            var text = "Cozy lummox gives smart squid who asks for job pen";

            Exercise1(text);
            Console.WriteLine();

            Exercise2(text);

        }

        private static void Exercise1(string text) {
            var dict = new Dictionary<Char, int>();

            foreach (var uc in text.ToUpper()) {
                if ('A' <= uc && uc <= 'Z') {
                    if (dict.ContainsKey(uc))
                        dict[uc]++;     //  登録済み：valueをインクリメント
                    else
                        dict[uc] = 1;   //  未登録：valueに1を設定
                }
            }
            //⑥すべての文字が読み終わったら、アルファベット順に並び替えて出力
            foreach (var item in dict.OrderBy(x => x.Key)) {
                Console.WriteLine("{0}:{1}", item.Key, item.Value);
            }
        }

        private static void Exercise2(string text) {
            var dict = new SortedDictionary<Char, int>();

            foreach (var uc in text.ToUpper()) {
                if ('A' <= uc && uc <= 'Z') {
                    if (dict.ContainsKey(uc))
                        dict[uc]++;     //  登録済み：valueをインクリメント
                    else
                        dict[uc] = 1;   //  未登録：valueに1を設定
                }
            }
            //⑥すべての文字が読み終わったら出力
            foreach (var item in dict) {
                Console.WriteLine("{0}:{1}", item.Key, item.Value);
            }
        }
    }
}
