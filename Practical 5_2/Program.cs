using System;
using System.Threading;

namespace Practical_5_2
{
    class Program
    {
        static Semaphore semaphore = new Semaphore(2, 2);
        static object lockObject = new object();
        static int currentGreenLight = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Thread[] threads = new Thread[4];

            for (int i = 0; i < threads.Length; i++)
            {
                int index = i;
                threads[i] = new Thread(() => TrafficLight(index));
                threads[i].Start();
            }

            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(Car);
                thread.Start();
            }
        }

        static void TrafficLight(int index)
        {
            while (true)
            {
                lock (lockObject)
                {
                    if (index == currentGreenLight)
                    {
                        Console.WriteLine($"Світлофор {index} зелений");
                        currentGreenLight = (currentGreenLight + 1) % 4;
                        Monitor.PulseAll(lockObject);
                    }
                    else
                    {
                        Console.WriteLine($"Світлофор {index} червоний");
                        Monitor.Wait(lockObject);
                    }
                }

                Thread.Sleep(3000);
            }
        }

        static void Car()
        {
            Console.WriteLine("Автомобіль наближається до перехрестя");

            semaphore.WaitOne();
            Console.WriteLine("Автомобіль виїхав на перехрестя");

            Thread.Sleep(2000);

            Console.WriteLine("Ліве перехрестя автомобіля");
            semaphore.Release();
        }
    }
}
