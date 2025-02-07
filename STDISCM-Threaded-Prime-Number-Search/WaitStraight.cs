using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class WaitStraight : IPrimeNumberSearcher
{
    private static readonly object lockObject = new object();
    private static Dictionary<int, List<int>> primesByThread = new Dictionary<int, List<int>>();
    private static List<(int Prime, string Log)> allPrimeLogs = new List<(int, string)>();

    public List<int> SearchPrimes(int start, int end, int customThreadId)
    {
        List<int> primes = new List<int>();
        DateTime foundTime = DateTime.Now;

        for (int i = start; i <= end; i++)
        {
            if (IsPrime(i))
            {
                primes.Add(i);
                // Record the time when the prime is found
                lock (lockObject)
                {
                    if (!primesByThread.ContainsKey(customThreadId))
                    {
                        primesByThread[customThreadId] = new List<int>();
                    }
                    primesByThread[customThreadId].Add(i);
                    allPrimeLogs.Add((i, $"Thread ID: {customThreadId}\t|\t Time: {foundTime.ToString("HH:mm:ss.fff")}\t|\t{i}\t|\t Thread Range: ({start} - {end})"));
                }
            }
        }

        return primes;
    }

    public List<int>[] StartPrimeSearch(int numberOfThreads, int upperLimit)
    {
        // Calculate the range for each thread
        int range = upperLimit / numberOfThreads;
        Thread[] threads = new Thread[numberOfThreads];
        List<int>[] primesPerThread = new List<int>[numberOfThreads];

        for (int i = 0; i < numberOfThreads; i++)
        {
            int start = i * range + 1;
            int end = (i == numberOfThreads - 1) ? upperLimit : (i + 1) * range;
            int threadIndex = i;
            int customThreadId = i + 1;

            primesPerThread[i] = new List<int>();

            threads[i] = new Thread(() =>
            {
                var primes = SearchPrimes(start, end, customThreadId);
                lock (primesPerThread[threadIndex])
                {
                    primesPerThread[threadIndex].AddRange(primes);
                }
            });
            threads[i].Start();
        }

        // Wait for all threads to complete
        foreach (var thread in threads)
        {
            thread.Join();
        }

        return primesPerThread;
    }

    public static void PrintAllPrimeLogs()
    {
        lock (lockObject)
        {
            foreach (var log in allPrimeLogs.OrderBy(log => log.Prime))
            {
                Console.WriteLine(log.Log);
            }
        }
    }

    public static void PrintPrimesByThread()
    {
        lock (lockObject)
        {
            foreach (var threadId in primesByThread.Keys.OrderBy(id => id))
            {
                Console.WriteLine($"Thread {threadId} primes: {string.Join(", ", primesByThread[threadId].OrderBy(p => p))}");
            }
        }
    }

    private bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        for (int i = 3; i <= Math.Sqrt(number); i += 2)
        {
            if (number % i == 0) return false;
        }

        return true;
    }
}
