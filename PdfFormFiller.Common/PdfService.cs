using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace PdfFormFiller.Common
{
  /// <summary>
  /// Pdf Service
  /// </summary>
  /// <seealso cref="PdfFormFiller.Common.IPdfService" />
  public class PdfService : IPdfService
  {
    /// <summary>
    /// Fills the specified in stream.
    /// </summary>
    /// <param name="inStream">The in stream.</param>
    /// <param name="fields">The fields.</param>
    /// <returns></returns>
    public MemoryStream FillForm(Stream inStream, IDictionary<string, string> fields)
    {
      var outStream = new MemoryStream();
      var fieldsToFill = fields.ToDictionary(k => k.Key.Replace("$", "."), k => k.Value);
      var ftf = new Dictionary<string, string>(fieldsToFill, StringComparer.InvariantCultureIgnoreCase);
      var reader = new PdfReader(inStream);     
      var stamper = new PdfStamper(reader, outStream);
      var formFields = stamper.AcroFields;

      foreach (var fieldName in fieldsToFill.Keys)
        formFields.SetField(fieldName, fieldsToFill[fieldName]);
                                        
      stamper.Close();
      reader.Close();
      return outStream;
    }

    /// <summary>
    /// Generates the file.
    /// </summary>
    /// <param name="inFile">The in file.</param>
    /// <param name="jsonFile"></param>
    /// <param name="outFile">The out file.</param>          
    public void GenerateFile(string inFile, string jsonFile, string outFile)
    {                                             
      var jsonText = System.IO.File.ReadAllText(jsonFile);                           
      var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
      using (var inStream = new FileStream(inFile, FileMode.Open))
      {
        var result = this.FillForm(inStream, data);
        System.IO.File.WriteAllBytes(outFile, result.ToArray());
      }
    }                              

    /// <summary>
    /// Gets the form fields.
    /// </summary>
    /// <param name="inStream">The in stream.</param>
    /// <returns></returns>
    public IDictionary<string, PdfField> GetFormFields(Stream inStream)
    {
      var reader = new PdfReader(inStream);
      var form = reader.AcroFields;
      var keys = new List<string>();
      foreach (var f in form.Fields.Keys)
      {
        keys.Add(f.ToString());
      }

      var result = keys
        .GroupBy(x => x, StringComparer.InvariantCultureIgnoreCase)
        .ToDictionary(x => x.Key, x =>
        {
          return new PdfField()
          {
            Name = x.Key,           
            FieldTypeId = form.GetFieldType(x.Key),
            Value = form.GetField(x.Key) ,
            AppearanceStates = form.GetAppearanceStates(x.Key)
          };
        }, StringComparer.InvariantCultureIgnoreCase);
      reader.Close();
      return new SortedDictionary<string, PdfField>(result, StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Downloads the PDF.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    /// <exception cref="ApplicationException">The URL was not a valid, absolute URI:  + url</exception>    
    [ExcludeFromCodeCoverage]
    public MemoryStream DownloadUrl(string url)
    {
      Uri pdfUrl;                     
      if (Uri.TryCreate(url, UriKind.Absolute, out pdfUrl))
      {
        byte[] data;
        using (var wc = new WebClient())
        {
          data = wc.DownloadData(url); 
        }       
                                
        return new MemoryStream(data);
      }

      throw new ApplicationException("The URL was not a valid, absolute URI: " + url);
    }
  }
}
