using System;
using System.Collections.Generic;

public class ImmediateLinear : IPrimeNumberSearcher
{
    public List<int> SearchPrimes(int start, int end, int customThreadId = 0)
    {
        List<int> primes = new List<int>();
        for (int number = start; number <= end; number++)
        {
            bool isPrime = true;
            Console.WriteLine($"Number: {number}");

            for (int divisor = 1; divisor <= number; divisor++)
            {
                Console.WriteLine($"Divisor: {divisor}");
                if (number % divisor == 0 && divisor != 1 && divisor != number)
                {
                    isPrime = false;
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
        result[0] = SearchPrimes(2, upperLimit);
        return result;
    }
}
