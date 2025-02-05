using System;
using System.IO;

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
    }
}
