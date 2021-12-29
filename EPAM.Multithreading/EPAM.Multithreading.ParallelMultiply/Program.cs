using System;
using System.Threading.Tasks;

namespace EPAM.Multithreading.ParallelMultiply
{
    internal class Program
    {
        private static Random _random = new Random();

        static void Main(string[] args)
        {
            MatrixInitialize(out int[,] matrix1, 5, 5);
            MatrixInitialize(out int[,] matrix2, 5, 5);

            PrintMatrix(matrix1);
            Console.WriteLine("---------");

            PrintMatrix(matrix2);
            Console.WriteLine("---------");

            int[,] multiplyResult = ParallelMatrixMultiply(matrix1, matrix2);

            PrintMatrix(multiplyResult);
            Console.WriteLine("---------");
        }

        private static void MatrixInitialize(out int[,] matrix, int matrixWidth, int matrixHeight)
        {
            matrix = new int[matrixWidth, matrixHeight];

            for (int i = 0; i < matrixWidth; i++)
            {
                for (int j = 0; j < matrixHeight; j++)
                {
                    matrix[i, j] = _random.Next(0, 10);
                }
            }
        }

        private static int[,] ParallelMatrixMultiply(int[,] matrix1, int[,] matrix2)
        {
            int[,] result = new int[matrix1.GetLength(0), matrix1.GetLength(1)];

            Parallel.For(0, matrix1.GetLength(0), i =>
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    result[i,j] = matrix1[i,j] * matrix2[i,j];
                }
            });

            return result;
        }

        private static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i,j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
