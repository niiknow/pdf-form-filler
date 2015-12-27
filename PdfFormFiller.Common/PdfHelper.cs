using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    public byte[] Fill(Stream inStream, IDictionary<string, string> fields)
    {
      var outStream = new MemoryStream();
      var fieldsToFill = fields.ToDictionary(k => k.Key.Replace("$", "."), k => k.Value);
      var ftf = new Dictionary<string, string>(fieldsToFill, StringComparer.InvariantCultureIgnoreCase);
      var reader = new PdfReader(inStream);     
      var stamper = new PdfStamper(reader, outStream);
      var formFields = stamper.AcroFields;

      foreach (var fieldName in fieldsToFill.Keys)
        formFields.SetField(fieldName, fieldsToFill[fieldName]);

      stamper.FormFlattening = true;
      stamper.Close();
      reader.Close();
      return outStream.ToArray();
    }

    /// <summary>
    /// Generates the file.
    /// </summary>
    /// <param name="inFile">The in file.</param>
    /// <param name="jsonFile"></param>
    /// <param name="outFile">The out file.</param>
    public void GenerateFile(string inFile, string jsonFile, string outFile = null)
    {                                           
      if (string.IsNullOrEmpty(outFile))
      {
        outFile = inFile.Replace(".", "-out.");
      }

      var jsonText = System.IO.File.ReadAllText(jsonFile);                           
      var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);
      using (var inStream = new FileStream(inFile, FileMode.Open))
      {
        var result = this.Fill(inStream, data);
        System.IO.File.WriteAllBytes(outFile, result);
      }
    }

    /// <summary>
    /// Gets the form field names.
    /// </summary>
    /// <param name="inStream">The in stream.</param>
    /// <returns></returns>
    public IDictionary<string, string> GetFormFieldNames(Stream inStream)
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
          var f = form.GetField(x.Key);
          if (!string.IsNullOrWhiteSpace(f) && f != x.Key)
          {
            return f;
          }
          return null;
        }, StringComparer.InvariantCultureIgnoreCase);
      reader.Close();
      return new SortedDictionary<string, string>(result, StringComparer.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public string GetValue(AcroFields.Item item)
    {
      var valueDict = item.GetValue(0);
      var appearanceDict = valueDict.GetAsDict(PdfName.AP);

      if (appearanceDict != null)
      {
        var normalAppearances = appearanceDict.GetAsDict(PdfName.N);
        // /D is for the "down" appearances.

        // if there are normal appearances, one key will be "Off", and the other
        // will be the export value... there should only be two.
        if (normalAppearances != null)
        {
          foreach (var curKey in normalAppearances.Keys)
            if (!PdfName.OFF.Equals(curKey))
              return curKey.ToString().Substring(1); // string will have a leading '/' character, so remove it!
        }
      }

      // if that doesn't work, there might be an /AS key, whose value is a name with 
      // the export value, again with a leading '/', so remove it!
      var curVal = valueDict.GetAsName(PdfName.AS);                             
      return (curVal != null) ? curVal.ToString().Substring(1) : string.Empty;
    }  
  }
}
