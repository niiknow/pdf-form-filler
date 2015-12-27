using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfFormFiller.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PdfFormFiller.Web.Controllers
{
  /// <summary>
  /// PDF Form Handler
  /// </summary>
  /// <seealso cref="System.Web.Mvc.Controller" />
  public class PdfFormController : Controller
  {
    /// <summary>
    /// The PDF service
    /// </summary>
    private readonly IPdfService pdfService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfFormController"/> class.
    /// </summary>
    /// <param name="pdfService">The PDF service.</param>
    public PdfFormController(IPdfService pdfService)
    {
      this.pdfService = pdfService;
    }

    /// <summary>
    /// Indexes the specified URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    /// <exception cref="System.Web.HttpException">The query string 'url' parameter is required.</exception>
    [HttpGet]
    public ActionResult Index(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
      {                                                          
        throw new HttpException("The query string 'url' parameter is required.");
      }
                                                            
      var fields = this.pdfService.GetFormFieldNames(this.pdfService.DownloadUrl(url));
      var model = new PdfFormFiller.Web.Models.FormFillerViewModel()
      {
        Fields = fields
      };

      return View(model);
    }

    /// <summary>
    /// Fills the specified URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Fill(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
      {
        throw new HttpException("The query string 'url' parameter is required.");
      }
      var inStream1 = this.pdfService.DownloadUrl(url);
      var fields = this.pdfService.GetFormFields(inStream1);
      var data = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
      var myFields = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase); 
      foreach(var f in fields)
      {
        data.Add(f.Key, ((PdfFieldTypeEnum)f.Value.FieldTypeId).ToString());
        myFields.Add(f.Key, f.Value.OriginalName);
      }

      var model = new PdfFormFiller.Web.Models.FormFillerViewModel()
      {
        Fields = myFields,
        FieldsJson = JsonConvert.SerializeObject(data, Formatting.Indented)
      };

      return View(model);
    }

    /// <summary>
    /// Fills the specified URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="formData">The form data.</param>
    [HttpPost]
    public void Fill(string url, IDictionary<string, string> formData)
    {
      if (string.IsNullOrWhiteSpace(url))
      {
        throw new HttpException("The query string 'url' parameter is required.");
      }
  
      var result = this.pdfService.Fill(this.pdfService.DownloadUrl(url), formData);
      var fileName = System.IO.Path.GetFileName(url);
      var response = this.Response;
      var cd = new System.Net.Mime.ContentDisposition
      {
        FileName = fileName,                       
        Inline = true,
      };
      response.AppendHeader("Content-Disposition", cd.ToString());
                                    
      response.ContentType = "application/pdf";
      response.BinaryWrite(result);
      response.End();    
    }

    /// <summary>
    /// Fills the json.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="jsonData">The json data.</param>
    [HttpPost]
    public void FillWithJson(string url, JObject formData)
    {
      if (string.IsNullOrWhiteSpace(url))
      {
        throw new HttpException("The query string 'url' parameter is required.");
      }

      var jobject = formData;

      var jsonData = jobject.Descendants()
        .Where(j => j.Children().Count() == 0)
        .Aggregate(
          new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase),
          (props, jtoken) =>
          {
            props.Add(jtoken.Path, jtoken.ToString());
            return props;
          });

      this.Fill(url, jsonData);
    }     
  }
}
