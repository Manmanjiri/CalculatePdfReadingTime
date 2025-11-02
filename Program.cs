using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string pdfFilePath = @"path\to\your\document.pdf";  // Provide the path to your PDF file

        // Start the timer when opening the PDF
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Open the PDF file (raw byte read here as a placeholder)
        bool isFileProcessedSuccessfully = OpenPdfFile(pdfFilePath);

        // Stop the timer when closing the PDF
        stopwatch.Stop();

        if (isFileProcessedSuccessfully)
        {
            Console.WriteLine($"Time taken to open and close the PDF file: {stopwatch.ElapsedMilliseconds} ms");
        }
        else
        {
            Console.WriteLine("There was an error processing the PDF file.");
        }
    }

    static bool OpenPdfFile(string filePath)
    {
        try
        {
            // Check if the file exists before attempting to open it
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return false;
            }

            // Open the PDF file using FileStream
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Simulate some work being done on the file, such as reading the contents
                byte[] fileContent = new byte[fs.Length];
                fs.Read(fileContent, 0, (int)fs.Length);

                // You could potentially process the content here if needed (but we're just timing open/close)
            }

            // If we reach this point, the file was opened and closed successfully
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening file: {ex.Message}");
            return false;
        }
    }
}
