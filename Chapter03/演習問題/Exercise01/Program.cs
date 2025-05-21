﻿
namespace Exercise01 {
    internal class Program {
        static void Main(string[] args) {
            var numbers = new List<int> { 12, 87, 94, 14, 53, 20, 40, 35, 76, 91, 31, 17, 48 };

            // 3.1.1
            Exercise1(numbers);
            Console.WriteLine("-----");

            // 3.1.2
            Exercise2(numbers);
            Console.WriteLine("-----");

            // 3.1.3
            Exercise3(numbers);
            Console.WriteLine("-----");

            // 3.1.4
            Exercise4(numbers);
        }

        private static void Exercise1(List<int> numbers) {
            var exist = numbers.Exists(n => n % 8 == 0 || n % 9 == 0);
            if (exist)
                Console.WriteLine("存在しています");
            else
                Console.WriteLine("存在していません");
        }

        private static void Exercise2(List<int> numbers) {
            numbers.ForEach(n => Console.WriteLine(n / 2.0));

            //foreach (var n in numbers) {
            //    Console.WriteLine(n / 2.0);
            //}
        }

        private static void Exercise3(List<int> numbers) {
            numbers.Where(n => n > 50).ToList().ForEach(n=>Console.WriteLine(n));

            //foreach (var num in numbers.Where(n => n > 50)) {
            //    Console.WriteLine(num);
            //}
        }

        private static void Exercise4(List<int> numbers) {
            numbers.Select(n => n * 2).ToList().ForEach(Console.WriteLine);
        }
    }
}
