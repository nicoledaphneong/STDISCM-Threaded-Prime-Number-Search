using System;
using System.Collections.Generic;
using System.Threading;

public class ImmediateLinear : IPrimeNumberSearcher
{
    public List<int> SearchPrimes(int start, int end, int numberOfThreads)
    {
        List<int> primes = new List<int>();
        for (int number = start; number <= end; number++)
        {
            bool isPrime = true;
            Console.WriteLine($"Number: {number}");

            int divisorRange = number / numberOfThreads;
            List<Thread> threads = new List<Thread>();
            List<bool> results = new List<bool>(new bool[numberOfThreads]);

            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadId = i + 1; // Custom thread ID starts from 1
                int startDivisor = i * divisorRange + 1;
                int endDivisor = (i == numberOfThreads - 1) ? number : (i + 1) * divisorRange;

                Thread thread = new Thread(() =>
                {
                    for (int divisor = startDivisor; divisor <= endDivisor; divisor++)
                    {
                        Console.WriteLine($"Thread ID: {threadId}\t|\t Time: {DateTime.Now.ToString("HH:mm:ss.fff")}\t|\tDivisor: {divisor}\t|\t");
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

            Console.WriteLine($"Prime: {isPrime}\n");

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
        return result;
    }
}
