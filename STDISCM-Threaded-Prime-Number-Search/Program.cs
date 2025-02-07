using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        string projectRoot = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;

        if (projectRoot == null)
        {
            Console.WriteLine("Error: Unable to determine the project root directory.");
            return;
        }

        // Define the relative path to the config file
        string configFilePath = Path.Combine(projectRoot, "config.txt");

        // Default configuration settings
        int numberOfThreads = 4;
        int upperLimit = 1000;
        string printing = "immediate";
        string divisionScheme = "straight";

        // Read configuration settings from config.txt
        try
        {
            var config = File.ReadAllLines(configFilePath);
            foreach (var line in config)
            {
                var parts = line.Split('=');
                if (parts[0] == "NumberOfThreads")
                {
                    if (int.TryParse(parts[1], out int parsedThreads) && parsedThreads >= 1)
                    {
                        numberOfThreads = parsedThreads;
                    }
                    else
                    {
                        Console.WriteLine("Number of threads is invalid. Using default: 4");
                    }
                }
                else if (parts[0] == "UpperLimit")
                {
                    if (int.TryParse(parts[1], out int parsedLimit) && parsedLimit >= 0)
                    {
                        upperLimit = parsedLimit;
                    }
                    else
                    {
                        Console.WriteLine("Upper limit is invalid. Using default: 1000");
                    }
                }
                else if (parts[0] == "Printing")
                {
                    if (parts[1].Equals("immediate", StringComparison.OrdinalIgnoreCase) || parts[1].Equals("wait", StringComparison.OrdinalIgnoreCase))
                    {
                        printing = parts[1].ToLower();
                    }
                    else
                    {
                        Console.WriteLine("Printing mode is invalid. Using default: immediate");
                    }
                }
                else if (parts[0] == "DivisionScheme")
                {
                    if (parts[1].Equals("straight", StringComparison.OrdinalIgnoreCase) || parts[1].Equals("linear", StringComparison.OrdinalIgnoreCase))
                    {
                        divisionScheme = parts[1].ToLower();
                    }
                    else
                    {
                        Console.WriteLine("Division scheme is invalid. Using default: straight");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading config file: {ex.Message}");
            Console.WriteLine("Using default configuration settings.");
        }

        Console.WriteLine($"Number of Threads: {numberOfThreads}");
        Console.WriteLine($"Upper Limit: {upperLimit}");
        Console.WriteLine($"Printing: {printing}");
        Console.WriteLine($"Division Scheme: {divisionScheme}");

        // Determine the prime checker implementation based on the config
        IPrimeNumberSearcher searcherImplementation;
        if (printing == "immediate" && divisionScheme == "straight")
        {
            searcherImplementation = new ImmediateStraight();
        }
        else if (printing == "wait" && divisionScheme == "straight")
        {
            searcherImplementation = new WaitStraight();
        }
        else
        {
            // Default to ImmediateStraight if the input is not recognized
            searcherImplementation = new ImmediateStraight();
        }

        // Start the stopwatch
        Stopwatch stopwatch = Stopwatch.StartNew();
        DateTime startTime = DateTime.Now;
        Console.WriteLine($"Start Time: {startTime.ToString("HH:mm:ss.fff")}");
        Console.WriteLine("Searching...");

        // Start the prime search
        var primesPerThread = searcherImplementation.StartPrimeSearch(numberOfThreads, upperLimit);

        // Stop the stopwatch
        stopwatch.Stop();
        DateTime endTime = DateTime.Now;

        // Display end time, and total time taken
        Console.WriteLine($"End Time: {endTime.ToString("HH:mm:ss.fff")}");
        Console.WriteLine($"Total Time Taken: {stopwatch.ElapsedMilliseconds} ms");

        // Display the count of primes found per thread
        for (int i = 0; i < numberOfThreads; i++)
        {
            Console.WriteLine($"Thread {i + 1} found {primesPerThread[i].Count} primes.");
        }

        // Display the total count of primes found
        int totalPrimes = 0;
        foreach (var primes in primesPerThread)
        {
            totalPrimes += primes.Count;
        }
        Console.WriteLine($"Total primes found: {totalPrimes}");

        // Print all prime logs after all threads have completed
        if (searcherImplementation is WaitStraight)
        {
            if (totalPrimes < 1000)
            {
                WaitStraight.PrintAllPrimeLogs();
            }
            else
            {
                Console.Write("The number of primes found is quite large. Display primes found per thread? (Y/n): ");
                string userInput = Console.ReadLine();

                if (userInput != null && userInput.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    WaitStraight.PrintAllPrimeLogs();
                }
            }
        }

        // Ask the user if they want to display the list of all found primes
        if (totalPrimes < 1000)
        {
            DisplayAllPrimes(primesPerThread);
        }
        else
        {
            Console.Write("The list of all primes is quite large. Display list of all found primes? (Y/n): ");
            string userInput = Console.ReadLine();

            if (userInput != null && userInput.Equals("Y", StringComparison.OrdinalIgnoreCase))
            {
                DisplayAllPrimes(primesPerThread);
            }
        }

        void DisplayAllPrimes(List<int>[] primesPerThread)
        {
            // Display all found primes at the end
            List<int> allPrimes = new List<int>();
            foreach (var primes in primesPerThread)
            {
                allPrimes.AddRange(primes);
            }
            allPrimes.Sort();
            Console.WriteLine("All found primes:");
            Console.WriteLine(string.Join(", ", allPrimes));
        }
    }
}
