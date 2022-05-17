using System;
using System.Threading.Tasks;

namespace EPAM.Multithreading.TaskArray
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Task<int>[] tasks = CreateTaskArray(100, 1000);

            await Task.WhenAll(tasks);
        }

        private static Task<int>[] CreateTaskArray(int tasksCount, int iterationCount)
        {
            Task<int>[] tasks = new Task<int>[tasksCount];

            for (int i = 0; i < tasksCount; i++)
            {
                tasks[i] = IterateTask(i, iterationCount);
            }

            return tasks;
        }

        private static async Task<int> IterateTask(int taskInd, int iteartionCount)
        {
            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < iteartionCount; i++)
                {
                    Console.WriteLine($"(Task #{taskInd} - {{{i}}})");
                }
            });

            return 0;
        }
    }
}
