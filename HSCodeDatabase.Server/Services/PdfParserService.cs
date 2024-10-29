using HSCodeDatabase.DataAccess.Entities;
using Newtonsoft.Json;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace HSCodeDatabase.Server.Services;

public class PdfParser
{
    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "TNV.pdf");
    public List<Root> RootTables { get; set; } = new();
    public List<Category> CategoryTables { get; set; } = new();
    public List<SubCategory> SubCategoryTables { get; set; } = new();
    public List<Product> ProductTables { get; set; } = new();

    public PdfParser()
    {
        ParsePdf();
    }

    public void ParsePdf()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string pdfPath = @"C:\Users\user\source\repos\ConsoleApp1\ConsoleApp1\input.pdf";
        string outputFilePath = @"C:\Users\user\source\repos\ConsoleApp1\ConsoleApp1\output.txt";

        // Set console output encoding to UTF-8 to display Russian characters
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            using (PdfDocument pdf = PdfDocument.Open(FilePath))
            using (StreamWriter writer = new StreamWriter(outputFilePath, false, System.Text.Encoding.UTF8))
            {
                // Iterate over each page in the PDF
                foreach (Page page in pdf.GetPages().Skip(21))
                {
                    // Extract the text content from the page
                    string pageText = page.Text;

                    // Split the content into lines and process each line
                    string[] lines = pageText.Split('\n');
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line); // Write the line to the output file
                        Console.WriteLine(line); // Display the line in the console
                    }
                }
            }

            Console.WriteLine("PDF content has been successfully written to the output file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        //using (var pdf = PdfDocument.Open(FilePath))
        //{
        //    foreach (var page in pdf.GetPages().Skip(302))
        //    {

        //        string pageText = page.Text;
        //        pageText = pageText.Replace("\"", "");
        //        Console.Clear();
        //        Console.WriteLine(pageText);
        //        Console.ReadKey();

        //        // Split lines and extract relevant data
        //        //var aa = pageText.Split('\n');
        //        //foreach (var line in aa)
        //        //{
        //        //    ParseLine(line.Trim());
        //        //} 
        //    }
        //}
    }


    private void ProcessWords(Page page)
    {
        Console.WriteLine("Words on the page (with positions):");
        foreach (var word in page.GetWords())
        {
            Console.WriteLine($"Word: {word.Text}, Position: {word.BoundingBox}");
        }
    }

    //private void ProcessTextBlocks(Page page)
    //{

       

    //    Console.WriteLine("Extracted text blocks:");

    //    foreach (var block in page.TextBlocks)
    //    {
    //        Console.WriteLine("Block:");
    //        Console.WriteLine(block.Text); // Outputs the full block of text
    //        Console.WriteLine(new string('-', 30)); // Block separator
    //    }
    //}

   

  
}
