using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class WaitLinear : IPrimeNumberSearcher
{
    private static readonly object lockObject = new object();
    private static List<string> logMessages = new List<string>();

    public List<int> SearchPrimes(int start, int end, int numberOfThreads)
    {
        List<int> primes = new List<int>();
        for (int number = start; number <= end; number++)
        {
            bool isPrime = true;
            lock (lockObject)
            {
                logMessages.Add($"Number: {number}");
            }

            int divisorRange = Math.Max(1, (number - 1) / numberOfThreads);
            List<Thread> threads = new List<Thread>();
            List<bool> results = new List<bool>(new bool[numberOfThreads]);

            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadId = i + 1; // Custom thread ID starts from 1
                int startDivisor = i * divisorRange + 1;
                int endDivisor = (i == numberOfThreads - 1) ? number - 1 : (i + 1) * divisorRange;

                // Adjust the range if there are more threads than divisors
                if (startDivisor > number - 1)
                {
                    startDivisor = number - 1;
                }
                if (endDivisor > number - 1)
                {
                    endDivisor = number - 1;
                }

                Thread thread = new Thread(() =>
                {
                    for (int divisor = startDivisor; divisor <= endDivisor; divisor++)
                    {
                        lock (lockObject)
                        {
                            logMessages.Add($"Thread ID: {threadId}\t|\t Time: {DateTime.Now.ToString("HH:mm:ss.fff")}\t|\tDivisor: {divisor}\t|");
                        }
                        if (number % divisor == 0 && divisor != 1 && divisor != number)
                        {
                            results[threadId - 1] = true;
                        }
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            foreach (var result in results)
            {
                if (result)
                {
                    isPrime = false;
                    break;
                }
            }

            lock (lockObject)
            {
                logMessages.Add($"Prime: {isPrime}\n");
            }

            if (isPrime)
            {
                primes.Add(number);
            }
        }
        return primes;
    }

    public List<int>[] StartPrimeSearch(int numberOfThreads, int upperLimit)
    {
        List<int>[] result = new List<int>[1];
        result[0] = SearchPrimes(2, upperLimit, numberOfThreads);

        // Print all log messages after all primes are found
        lock (lockObject)
        {
            foreach (var message in logMessages)
            {
                Console.WriteLine(message);
            }
        }

        return result;
    }
}
