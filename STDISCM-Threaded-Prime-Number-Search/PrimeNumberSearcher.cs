using System;
using System.Collections.Generic;

public class PrimeNumberSearcher
{
    private int _start;
    private int _end;
    private int _customThreadId;

    public PrimeNumberSearcher(int start, int end, int customThreadId)
    {
        _start = start;
        _end = end;
        _customThreadId = customThreadId;
    }

    public List<int> SearchPrimes()
    {
        List<int> primes = new List<int>();
        for (int i = _start; i <= _end; i++)
        {
            if (IsPrime(i))
            {
                primes.Add(i);
                // Print each prime immediately
                Console.WriteLine($"Thread ID: {_customThreadId}\t|\t Time: {DateTime.Now.ToString("HH:mm:ss.fff")}\t|\t{i}\t|\t Thread Range: ({_start} - {_end})");
                Thread.Sleep(1000);
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
