namespace FillPdfForm
{
  using System;
  using System.Diagnostics.CodeAnalysis;

  /// <summary>
  /// Main entry point.
  /// </summary>
  class Program
  {
    /// <summary>
    /// Mains the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    [ExcludeFromCodeCoverage]
    static void Main(string[] args)
    {
      if (args.Length < 2)
      {
        ShowUsage();
        return;
      }

      string inFile = args[0];
      string jsonFile = args[1];
      string outFile = args.Length > 2 ? args[2] : inFile.Replace(".", "-out.");
      var service = new PdfFormFiller.Common.PdfService();
      service.GenerateFile(inFile, jsonFile, outFile);
    }

    /// <summary>
    /// Shows the usage.
    /// </summary>
    [ExcludeFromCodeCoverage]
    static void ShowUsage()
    {
      Console.WriteLine();
      Console.WriteLine("The syntax of this command is: ");
      Console.WriteLine("");
      Console.WriteLine("FillPdfForm.exe <originalFilePdf> <inputFileJson> <outputFilePdf>");
      Console.WriteLine("");
      Console.WriteLine("  originalFilePdf.: Full path to the original PDF file.");                 
      Console.WriteLine("");
      Console.WriteLine("  inputFileJson...: Full path to form data in json format (input.json) file.");        
      Console.WriteLine("");
      Console.WriteLine("  outputFilePdf...: Full path to result file");
      Console.WriteLine("                 [Default Value: originalFile-out.pdf]");
      Console.WriteLine("");                                                      
      Console.WriteLine("");
      Console.WriteLine("Example:");
      Console.WriteLine("");
      Console.WriteLine("  FillPdfForm.exe c:\\work\\originaFile.pdf c:\\work\\formData.json");
      Console.WriteLine("");
      Console.WriteLine("");
    }
  }
}
