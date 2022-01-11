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
            int result = await CalculateAsync();
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
            if (!int.TryParse(Console.ReadLine(), out int inputNumber))
            {
                Console.WriteLine("Wrong input, try again");
                InputNumber();
            }

            return inputNumber;
        }

        private static async Task<int> InputNumberAsync()
        {
            int inputNumber = 0;

            await Task.Factory.StartNew(() =>
            {
                if (!int.TryParse(Console.ReadLine(), out inputNumber))
                {
                    throw new InvalidCastException("Wrong input format, try again");
                }
                s_cts.Cancel();
            });

            return inputNumber;
        }

        private static async Task<int> CalculateAsync()
        {
            int inputNumber = InputNumber();
            Task<int> newNumberTask = InputNumberAsync();
            int sum = 0;

            try
            {
                sum = 0;
                
                Task<int> sumTask = GetSum(inputNumber, s_cts.Token);
                Task<int>[] tasks = new Task<int>[] { sumTask, newNumberTask };

                await Task.WhenAny(tasks);

                sum = sumTask.Result;

                return sum;
            }

            catch (AggregateException e)
            {
                Console.WriteLine("Calculation was canceled");

                s_cts = new CancellationTokenSource();

                if (newNumberTask.Result > 0)
                {
                    inputNumber = newNumberTask.Result;
                }

                Task<int> sumTask = GetSum(inputNumber, s_cts.Token);
                newNumberTask = InputNumberAsync();

                Task<int>[] tasks = new Task<int>[] { sumTask, newNumberTask };

                await Task.WhenAny(tasks);

                sum = sumTask.Result;

                return sum;
            }
        }
    }
}
