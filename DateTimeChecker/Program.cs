using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        string directoryPath = @""; // Switch with proper directory

        // Get all files in the directory
        string[] files = Directory.GetFiles(directoryPath);
        Console.WriteLine($"Found {files.Length} files in directory: {directoryPath}");

        // Dictionary to group files by their timestamp
        var groupedFiles = new Dictionary<string, List<string>>();

        // Match timestamp
        string pattern = @"_(\d{12})(?:\.\w+)?$";
        Regex regex = new Regex(pattern);

        foreach (string file in files)
        {
            // Match the file name with the regex
            Match match = regex.Match(Path.GetFileName(file));
            if (match.Success)
            {
                // Extract the timestamp
                string timestamp = match.Groups[1].Value;

                // Add the file to the corresponding timestamp group
                if (!groupedFiles.ContainsKey(timestamp))
                {
                    groupedFiles[timestamp] = new List<string>();
                }
                groupedFiles[timestamp].Add(file);
            }
            else
            {
                Console.WriteLine($"File {Path.GetFileName(file)} does not match the expected pattern.");
            }
        }

        // Move files into folders
        foreach (var group in groupedFiles)
        {
            string timestamp = group.Key;
            List<string> filesWithSameTimestamp = group.Value;

            // Create a new folder for this timestamp
            string folderPath = Path.Combine(directoryPath, timestamp);
            Directory.CreateDirectory(folderPath);
            Console.WriteLine($"Created directory: {folderPath}");

            // Move each file to the new folder
            foreach (string file in filesWithSameTimestamp)
            {
                string destinationPath = Path.Combine(folderPath, Path.GetFileName(file));
                File.Move(file, destinationPath);
                Console.WriteLine($"Moved file {file} to {destinationPath}");
            }
        }

        Console.WriteLine("Files moved.");
    }
}
