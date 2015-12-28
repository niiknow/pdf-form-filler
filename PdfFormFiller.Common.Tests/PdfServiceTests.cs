using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PdfFormFiller.Common.Tests
{
  /// <summary>
  /// Container for all PdfService unit tests.
  /// </summary>
  [TestClass]
  public class PdfServiceTests
  {
    /// <summary>
    /// Gets the form fields test.
    /// </summary>
    [TestMethod]
    public void GetFormFieldsTest()
    {
      IDictionary<string, PdfField> fields;

      using (var pdfStream = Assembly.GetAssembly(typeof(PdfService)).GetManifestResourceStream("PdfFormFiller.Common.Resources.fw9.pdf"))
      {
        var filler = new PdfService();

        fields = filler.GetFormFields(pdfStream);
      }

      Assert.IsNotNull(fields);
      Assert.IsTrue(fields.Count > 0);
      Assert.IsTrue(fields.ContainsKey("topmostSubform[0].Page1[0].Address[0].f1_7[0]"));
    }

    /// <summary>
    /// Fills the test.
    /// </summary>
    [TestMethod]
    public void FillFormTest()
    {
      var fieldsToFill = new Dictionary<string, string>();
      fieldsToFill["topmostSubform[0].Page1[0].Address[0].f1_7[0]"] = "Hello";

      IDictionary<string, PdfField> fields;

      using (var pdfStream = Assembly.GetAssembly(typeof(PdfService)).GetManifestResourceStream("PdfFormFiller.Common.Resources.fw9.pdf"))
      {
        var filler = new PdfService();

        var result = filler.FillForm(pdfStream, fieldsToFill);     
        fields = filler.GetFormFields(result);
      }

      Assert.IsNotNull(fields);
      Assert.IsTrue(fields.Count > 0);
      Assert.IsTrue(fields.ContainsKey("topmostSubform[0].Page1[0].Address[0].f1_7[0]"));
      Assert.AreEqual(0, string.CompareOrdinal(fields["topmostSubform[0].Page1[0].Address[0].f1_7[0]"].Value, fieldsToFill["topmostSubform[0].Page1[0].Address[0].f1_7[0]"]));
    }

    /// <summary>
    /// Generates the file test.
    /// </summary>
    [TestMethod]
    public void GenerateFileTest()
    {
      var fieldsToFill = new Dictionary<string, string>();
      fieldsToFill["topmostSubform[0].Page1[0].Address[0].f1_7[0]"] = "Hello";

      IDictionary<string, PdfField> fields;
      var inFile = Path.GetTempFileName();
      var outFile = Path.GetTempFileName();
      var jsonFile = Path.GetTempFileName();

      using (var pdfStream = Assembly.GetAssembly(typeof(PdfService)).GetManifestResourceStream("PdfFormFiller.Common.Resources.fw9.pdf"))
      {
        using (var fileStream = File.Create(inFile))
        {
          pdfStream.CopyTo(fileStream);
        }
      }

      File.WriteAllText(jsonFile, JsonConvert.SerializeObject(fieldsToFill));
      var filler = new PdfService();
      filler.GenerateFile(inFile, jsonFile, outFile);

      using (var pdfStream = File.OpenRead(outFile))
      {
        fields = filler.GetFormFields(pdfStream);
      }
               
      Assert.IsNotNull(fields);
      Assert.IsTrue(fields.Count > 0);
      Assert.IsTrue(fields.ContainsKey("topmostSubform[0].Page1[0].Address[0].f1_7[0]"));
      Assert.AreEqual(0, string.CompareOrdinal(fields["topmostSubform[0].Page1[0].Address[0].f1_7[0]"].Value, fieldsToFill["topmostSubform[0].Page1[0].Address[0].f1_7[0]"]));
    }
  }
}
