using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Print the current working directory
        // Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");

        // Navigate to the root directory
        string currentDirectory = Directory.GetCurrentDirectory();
        string projectRoot = Directory.GetParent(currentDirectory).Parent.Parent.FullName;

        // Define the relative path to the config file
        string configFilePath = Path.Combine(projectRoot, "config.txt");

        // Read configuration settings from config.txt
        var config = File.ReadAllLines(configFilePath);
        int numberOfThreads = 0;
        int upperLimit = 0;

        foreach (var line in config)
        {
            var parts = line.Split('=');
            if (parts[0] == "NumberOfThreads")
            {
                numberOfThreads = int.Parse(parts[1]);
            }
            else if (parts[0] == "UpperLimit")
            {
                upperLimit = int.Parse(parts[1]);
            }
        }

        Console.WriteLine($"Number of Threads: {numberOfThreads}");
        Console.WriteLine($"Upper Limit: {upperLimit}");

        // Calculate the range for each thread
        int range = upperLimit / numberOfThreads;
        Thread[] threads = new Thread[numberOfThreads];
        List<int> allPrimes = new List<int>();

        for (int i = 0; i < numberOfThreads; i++)
        {
            int start = i * range + 1;
            int end = (i == numberOfThreads - 1) ? upperLimit : (i + 1) * range;

            PrimeNumberSearcher searcher = new PrimeNumberSearcher(start, end, i + 1);
            threads[i] = new Thread(() =>
            {
                var primes = searcher.SearchPrimes();
                lock (allPrimes)
                {
                    allPrimes.AddRange(primes);
                }
            });
            threads[i].Start();
        }

        // Wait for all threads to complete
        foreach (var thread in threads)
        {
            thread.Join();
        }

        // Display all found primes at the end
        allPrimes.Sort();
        Console.WriteLine("All found primes:");
        Console.WriteLine(string.Join(", ", allPrimes));
    }
}
