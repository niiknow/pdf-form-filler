using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    /// Indexes the specified URL.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult Index(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
      {                                                          
        throw new HttpException("The query string 'url' parameter is required.");
      }

      var filler = new PdfFormFiller.Common.PdfService();
      var fields = filler.GetFormFieldNames(filler.DownloadUrl(url));
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

      var filler = new PdfFormFiller.Common.PdfService();
      var fields = filler.GetFormFieldNames(filler.DownloadUrl(url));
      var data = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
      foreach(var f in fields)
      {
        data.Add(f.Key, "string");
      }

      var model = new PdfFormFiller.Web.Models.FormFillerViewModel()
      {
        Fields = fields,
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

      var filler = new PdfFormFiller.Common.PdfService();   
      var result = filler.Fill(filler.DownloadUrl(url), formData);
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
