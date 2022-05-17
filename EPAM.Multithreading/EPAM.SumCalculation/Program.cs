using System;
using System.Threading;
using System.Threading.Tasks;

namespace EPAM.SumCalculation
{
    internal class Program
    {
        private static CancellationTokenSource s_cts = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            int inputNumber = InputNumber();
            int result = await CalculateAsync(inputNumber);
            Console.WriteLine(result);
        }

        private static async Task<int> GetSum(int N, CancellationToken cancellationToken)
        {
            int sum = 0;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= N; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Current step: {i}");
                    sum += i;
                    
                    cancellationToken.ThrowIfCancellationRequested();
                }
            });

            return sum;
        }

        private static int InputNumber()
        {
            int inputNumber = 0;

            while (!int.TryParse(Console.ReadLine(), out inputNumber))
            {
                Console.WriteLine("Wrong input, try again");
            }

            return inputNumber;
        }

        private static async Task<int> InputNumberAsync()
        {
            int inputNumber = 0;

            await Task.Factory.StartNew(() =>
            {
                while (!int.TryParse(Console.ReadLine(), out inputNumber))
                {
                    Console.WriteLine("Wrong input. Try again");
                }
            });

            s_cts.Cancel();
            return inputNumber;
        }

        private static async Task<int> CalculateAsync(int inputNumber)
        {
            Task<int> newNumberTask = InputNumberAsync();

            try
            {              
                Task<int> sumTask = GetSum(inputNumber, s_cts.Token);
                Task<int>[] tasks = new Task<int>[] { sumTask, newNumberTask };

                await Task.WhenAny(tasks);

                int sum = sumTask.Result;

                return sum;
            }

            catch (AggregateException e)
            {
                Console.WriteLine("Calculation was canceled");

                if (newNumberTask.Result <= 0)
                {
                    return 0;
                }

                s_cts = new CancellationTokenSource();

                return CalculateAsync(newNumberTask.Result).Result;
            }
        }
    }
}
