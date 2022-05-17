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
            Task<int[]> arrayCreateTask = new Task<int[]>(() =>
            {
                int[] array = ArrayCreate(10);
                ArrayPrint(array);
                return array;
            });

            Task<int[]> arrayMultiplyTask = arrayCreateTask.ContinueWith(arr =>
            {
                int[] array = ArrayMultiply(arr.Result);
                ArrayPrint(array);
                return array;
            });

            Task<int[]> arraySortTask = arrayMultiplyTask.ContinueWith(arr =>
            {
                int[] array = ArraySort(arr.Result);
                ArrayPrint(array);
                return array;
            });

            Task<int> getAverageTask = arraySortTask.ContinueWith(arr =>
            {
                int average = GetAverage(arr.Result);
                Console.WriteLine(average);
                return average;
            });

            arrayCreateTask.Start();
            await arrayCreateTask;
            await arrayMultiplyTask;
            await arraySortTask;
            await getAverageTask;
        }

        private static int[] ArrayCreate(int arraySize)
        {
            int[] array = new int[arraySize];

            for (int i = 0; i < arraySize; i++)
            {
                array[i] = _random.Next(0, 100);
            }

            return array;
        }

        private static int[] ArrayMultiply(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] *= _random.Next(1, 11);
            }

            return array;
        }

        private static int[] ArraySort(int[] array)
        {
            Array.Sort(array);

            return array;
        }

        private static int GetAverage(int[] array)
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
