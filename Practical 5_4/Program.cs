using System;
using System.Threading;

namespace Practical_5_4
{
    class Program
    {
        static Semaphore semaphore;
        static object lockObject = new object();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            int[,] matrixA = { { 1, 2, 3 }, { 4, 5, 6 } };
            int[,] matrixB = { { 7, 8 }, { 9, 10 }, { 11, 12 } };

            int[,] resultMatrix = MultiplyMatrix(matrixA, matrixB);

            Console.WriteLine("Матриця результатів:");
            PrintMatrix(resultMatrix);
        }

        static int[,] MultiplyMatrix(int[,] matrixA, int[,] matrixB)
        {
            int rowsA = matrixA.GetLength(0);
            int columnsA = matrixA.GetLength(1);
            int rowsB = matrixB.GetLength(0);
            int columnsB = matrixB.GetLength(1);

            if (columnsA != rowsB)
            {
                throw new ArgumentException("Матриці не можна перемножувати");
            }

            int[,] resultMatrix = new int[rowsA, columnsB];

            semaphore = new Semaphore(Environment.ProcessorCount, Environment.ProcessorCount);

            for (int i = 0; i < rowsA; i++)
            {
                Monitor.Enter(lockObject);

                int currentIndex = i;

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    semaphore.WaitOne();

                    for (int j = 0; j < columnsB; j++)
                    {
                        int sum = 0;

                        for (int k = 0; k < columnsA; k++)
                        {
                            sum += matrixA[currentIndex, k] * matrixB[k, j];
                        }

                        resultMatrix[currentIndex, j] = sum;
                    }

                    semaphore.Release();
                });

                Monitor.Exit(lockObject);
            }

            return resultMatrix;
        }

        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int columns = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
