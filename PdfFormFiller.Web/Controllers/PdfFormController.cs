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
  public class PdfFormController : Controller
  {                    
    [HttpGet]
    public ActionResult Index(string url)
    {
      var filler = new PdfFormFiller.Common.PdfService();
      var fields = filler.GetFormFieldNames(this.DownloadPdf(url));
      var model = new PdfFormFiller.Web.Models.FormFillerViewModel()
      {
        Fields = fields
      };

      return View(model);
    }

    [HttpGet]
    public ActionResult Fill(string url)
    {
      var filler = new PdfFormFiller.Common.PdfService();
      var fields = filler.GetFormFieldNames(this.DownloadPdf(url));
      var model = new PdfFormFiller.Web.Models.FormFillerViewModel()
      {
        Fields = fields
      };

      return View(model);
    }

    [HttpPost]
    public void Fill(string url, IDictionary<string, string> formData)
    {
      var filler = new PdfFormFiller.Common.PdfService();   
      var result = filler.Fill(this.DownloadPdf(url), formData);
      var response = this.Response;
      var cd = new System.Net.Mime.ContentDisposition
      {
        FileName = "result.pdf",                       
        Inline = false,
      };
      response.AppendHeader("Content-Disposition", cd.ToString());
                                    
      response.ContentType = "application/pdf";
      response.BinaryWrite(result);
      response.End();   
      //return new ByteArrayContent(result);  
    }

    public System.IO.Stream DownloadPdf(string url)
    {
      Uri pdfUrl;
      System.IO.Stream result = null;
      if (Uri.TryCreate(url, UriKind.Absolute, out pdfUrl))
      {           
        using (var httpClient = new HttpClient())
        {
          var GetAsynctask = httpClient.GetAsync(pdfUrl, HttpCompletionOption.ResponseHeadersRead);
          var response = GetAsynctask.ConfigureAwait(false).GetAwaiter().GetResult();
          if (response.IsSuccessStatusCode)
          {
            if (response.Content != null)
            {
              var readAsStreamTask = response.Content.ReadAsStreamAsync();
              result = readAsStreamTask.ConfigureAwait(false).GetAwaiter().GetResult();
            }
          }
        }
      }

      if (result == null)
      {

        if (!Uri.TryCreate(url, UriKind.Absolute, out pdfUrl))
        {
          throw new HttpException((int)HttpStatusCode.BadRequest, "The URL was not a valid, absolute URI: " + url);
        }

      }
      return result;
    }
  }
}
