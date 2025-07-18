namespace Test01 {
    public class ScoreCounter {
        private IEnumerable<Student> _score;

        // コンストラクタ
        public ScoreCounter(string filePath) {

            _score = ReadScore(filePath);
        }

        //メソッドの概要： 
        private static IEnumerable<Student> ReadScore(string filePath) {
            var sales = new List<Student>();
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines) {
                var items = line.Split(',');
                var sale = new Student {
                    Name = items[0],
                    Subject = items[1],
                    Score = int.Parse(items[2])
                };
                sales.Add(sale);
            }
            return sales;
        }

        //メソッドの概要： 
        public IDictionary<string, int> GetPerStudentScore() {
            var dict = new Dictionary<string, int>();
            foreach (var score in _score) {
                if (dict.ContainsKey(score.Name))
                    dict[score.Name] += score.Score;
                else
                    dict[score.Name] = score.Score;
            }
            return dict;
        }
    }
}
