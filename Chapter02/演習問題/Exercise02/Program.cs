namespace Exercise02 {
    internal class Program {
        static void Main(string[] args) {

            PrintFeetToMeterList(1, 10);
        }
        // インチからメートルへの対応表を出力
        static void PrintFeetToMeterList(int start, int end) {
            for (int feet = start; feet <= end; feet++) {
                double meter = InchConverter.ToMeter(feet);
                Console.WriteLine($"{feet}inch = {meter:0.0000}m");
            }
        }
    }
}
