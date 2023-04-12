using System;
using System.Collections.Generic;
using System.Threading;

namespace Practical_5
{
    class Program
    {

        static Queue<int> sharedQueue = new Queue<int>();
        static object lockObject = new object();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Thread producerThread = new Thread(Producer);
            Thread consumerThread = new Thread(Consumer);

            producerThread.Start();
            consumerThread.Start();

            producerThread.Join();
            consumerThread.Join();
        }

        static void Producer()
        {
            Random random = new Random();

            for (int i = 0; i < 15; i++)
            {
                int number = random.Next(50);
                lock (lockObject)
                {
                    sharedQueue.Enqueue(number);
                    Console.WriteLine($"Producer produced {number}");
                }
                Thread.Sleep(300);
            }
        }

        static void Consumer()
        {
            while (true)
            {
                lock (lockObject)
                {
                    if (sharedQueue.Count > 0)
                    {
                        int number = sharedQueue.Dequeue();
                        Console.WriteLine($"Consumer consumed {number}");
                    }
                }
                Thread.Sleep(300);
            }
        }
    }
}
