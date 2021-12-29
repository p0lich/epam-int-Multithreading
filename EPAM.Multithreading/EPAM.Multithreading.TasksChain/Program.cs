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

            Task<int[]> createArray = ArrayInitialize(array);
            Task<int[]> multiplyArray = MulitplyArray(array, 5);
            Task<int[]> sortArray = SortArray(array);
            Task<int> getAverage = GetAverage(array);

            Task<int[]>[] tasks = new Task<int[]>[] { createArray, multiplyArray, sortArray};

            Task[] processingTasks = tasks.Select(async t =>
            {
                var result = await t;
                TaskPrint(t);
            }).ToArray();

            await Task.WhenAll(processingTasks);
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

        private static void TaskPrint(Task<int[]> array)
        {
            Console.WriteLine("---------------------------------------");

            for (int i = 0; i < array.Result.Length; i++)
            {
                Console.Write(array.Result[i] + " ");
            }

            Console.WriteLine("\n---------------------------------------");
        }
    }
}
