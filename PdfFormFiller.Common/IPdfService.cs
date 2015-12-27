using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormFiller.Common
{
  /// <summary>
  /// Pdf Service
  /// </summary>
  public interface IPdfService
  {
    /// <summary>
    /// Fills the specified in stream.
    /// </summary>
    /// <param name="inStream">The in stream.</param>
    /// <param name="fields">The fields.</param>
    /// <returns></returns>
    byte[] Fill(Stream inStream, IDictionary<string, string> fields);

    /// <summary>
    /// Gets the form field names.
    /// </summary>
    /// <param name="inStream">The in stream.</param>
    /// <returns></returns>
    IDictionary<string, string> GetFormFieldNames(Stream inStream);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="item">The item.</param>  
    string GetValue(AcroFields.Item item);
    /// <summary>
    /// Generates the file.
    /// </summary>
    /// <param name="inFile">The in file.</param>
    /// <param name="jsonFile">The json file.</param>
    /// <param name="outFile">The out file.</param>
    void GenerateFile(string inFile, string jsonFile, string outFile = null);

    /// <summary>
    /// Downloads the URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    Stream DownloadUrl(string url);
  }
}
