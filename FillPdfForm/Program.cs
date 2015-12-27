namespace FillPdfForm
{
  /// <summary>
  /// Main entry point.
  /// </summary>
  class Program
  {
    /// <summary>
    /// Mains the specified arguments.
    /// </summary>
    /// <param name="args">The arguments.</param>
    static void Main(string[] args)
    {
      string inFile = args[0];
      string jsonFile = args[1];
      string outFile = args.Length > 2 ? args[2] : inFile.Replace(".", "-out.");
      var service = new PdfFormFiller.Common.PdfService();
      service.GenerateFile(inFile, jsonFile, outFile);
    }
  }
}
