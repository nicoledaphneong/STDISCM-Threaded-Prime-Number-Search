using System;
using System.Collections.Generic;
using System.Threading;

public class ImmediateStraight : IPrimeNumberSearcher
{
    public List<int> SearchPrimes(int start, int end, int customThreadId)
    {
        List<int> primes = new List<int>();
        for (int i = start; i <= end; i++)
        {
            if (IsPrime(i))
            {
                primes.Add(i);
                // Print each prime immediately with the custom thread ID
                Console.WriteLine($"Thread ID: {customThreadId}\t|\t Time: {DateTime.Now.ToString("HH:mm:ss.fff")}\t|\t{i}\t|\t Thread Range: ({start} - {end})");
            }
        }

        return primes;
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
