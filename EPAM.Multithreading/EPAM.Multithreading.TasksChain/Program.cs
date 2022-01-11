using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EPAM.Multithreading.TasksChain
{
    internal class Program
    {
        private static Random _random = new Random();

        static async Task Main(string[] args)
        {
            int[] array = new int[10];

            var chainTask = Task.Run(() => 
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = _random.Next(0, 100);
                    Console.Write(array[i] + " ");
                }

                Console.WriteLine();
            });

            await chainTask.ContinueWith(_ => 
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] *= _random.Next(1, 11);
                    Console.Write(array[i] + " ");
                }

                Console.WriteLine();
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            await chainTask.ContinueWith(_ =>
            {
                Array.Sort(array);
                for (int i = 0; i < array.Length; i++)
                {
                    Console.Write(array[i] + " ");
                }
                
                Console.WriteLine();
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            await chainTask.ContinueWith(_ =>
            {
                int average = 0;
                for (int i = 0;i < array.Length; i++)
                {
                    average += array[i];
                }

                average /= array.Length;
                Console.WriteLine(average);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            //Task<int[]> createArray = ArrayInitialize(array);
            //Task<int[]> multiplyArray = MulitplyArray(array, 5);
            //Task<int[]> sortArray = SortArray(array);

            //Task<int> getAverage = GetAverage(array);

            //Task<int[]>[] tasks = new Task<int[]>[] { createArray, multiplyArray, sortArray };

            //Task[] processingTasks = tasks.Select(async t =>
            //{
            //    var result = await t;
            //    ArrayPrint(t.Result);
            //}).ToArray();

            //var result = await Task.WhenAll(tasks);

            //for (int i = 0; i < result.Length; i++)
            //{
            //    ArrayPrint(result[i]);
            //}
        }

        private static async Task<int[]> ArrayInitialize(int[] array)
        {
            await Task.Factory.StartNew(() => {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = _random.Next(0, 100);
                }
            });

            return array;
        }

        private static async Task<int[]> MulitplyArray(int[] array, int value)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] *= value;
                }
            });

            return array;
        }

        private static async Task<int[]> SortArray(int[] array)
        {
            await Task.Factory.StartNew(() => { Array.Sort(array); });
            
            return array;
        }

        private static async Task<int> GetAverage(int[] array)
        {
            int average = 0;

            for (int i = 0; i < array.Length; i++)
            {
                average += array[i];
            }

            return average / array.Length;
        }

        private static void ArrayPrint(int[] array)
        {
            Console.WriteLine("---------------------------------------");

            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }

            Console.WriteLine("\n---------------------------------------");
        }
    }
}
