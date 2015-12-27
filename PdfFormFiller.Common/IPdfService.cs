using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

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
    byte[] FillForm(Stream inStream, IDictionary<string, string> fields);
                                                                         
    /// <summary>
    /// Gets the form fields.
    /// </summary>
    /// <param name="inStream">The in stream.</param>
    /// <returns></returns>
    IDictionary<string, PdfField> GetFormFields(Stream inStream);

    /// <summary>
    /// Generates the file.
    /// </summary>
    /// <param name="inFile">The in file.</param>
    /// <param name="jsonFile">The json file.</param>
    /// <param name="outFile">The out file.</param>
    void GenerateFile(string inFile, string jsonFile, string outFile);

    /// <summary>
    /// Downloads the URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    [ExcludeFromCodeCoverage]
    Stream DownloadUrl(string url);
  }
}
