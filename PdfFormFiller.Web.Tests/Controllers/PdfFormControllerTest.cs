using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfFormFiller.Web;
using PdfFormFiller.Web.Controllers;
using PdfFormFiller.Common;
using System.Web;
using Moq;
using System.Web.Routing;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PdfFormFiller.Web.Tests.Controllers
{
  /// <summary>
  /// Home Controller tests container.
  /// </summary>
  [TestClass]
  public class PdfFormControllerTest
  {
    /// <summary>
    /// Gets the controller context.
    /// </summary>
    /// <returns></returns>
    private ControllerContext GetControllerContext()
    {
      var request = new Mock<HttpRequestBase>();       
      var mockHttpContext = new Mock<HttpContextBase>();             
      var controllerContext = new ControllerContext(mockHttpContext.Object
      , new RouteData(), new Mock<ControllerBase>().Object);    
      var response = new Mock<HttpResponseBase>();

      mockHttpContext.SetupGet(c => c.Response)
             .Returns(response.Object);

      return controllerContext;
    }

    /// <summary>
    /// Tests the fill get return model that help generate dynamic form.
    /// </summary>
    [TestMethod]
    public void TestFillGetReturnModelThatHelpGenerateDynamicForm()
    {
      IDictionary<string, PdfField> myList = new Dictionary<string, PdfField>();
      var pdfService = new Mock<IPdfService>();
      pdfService.Setup(p => p.DownloadUrl(It.IsAny<string>())).Returns(new MemoryStream());                        

      pdfService.Setup(p => p.GetFormFields(It.IsAny<Stream>())).Returns(myList);
      using (var controller = new PdfFormController(pdfService.Object))
      {                         
        controller.ControllerContext = this.GetControllerContext();
        controller.Fill("https://www.irs.gov/pub/irs-pdf/fw9.pdf");
      }

      pdfService.VerifyAll();
    }

    /// <summary>
    /// Tests the fill post dictionary return result PDF.
    /// </summary>
    [TestMethod]
    public void TestFillPostDictionaryReturnResultPdf()
    {                                                                                                                           
      IDictionary<string, PdfField> myList = new Dictionary<string, PdfField>();
      var pdfService = new Mock<IPdfService>();
      pdfService.Setup(p => p.DownloadUrl(It.IsAny<string>())).Returns(new MemoryStream());
      pdfService.Setup(p => p.FillForm(It.IsAny<Stream>(), It.IsAny<IDictionary<string, string>>())).Returns(new MemoryStream());
                                                                                                                 
      using (var controller = new PdfFormController(pdfService.Object))
      {
        controller.ControllerContext = this.GetControllerContext();
        controller.Fill("https://www.irs.gov/pub/irs-pdf/fw9.pdf", new Dictionary<string, string>());
      }

      pdfService.VerifyAll();
    }

    /// <summary>
    /// Tests the fill post with json flatten and return result PDF.
    /// </summary>
    [TestMethod]
    public void TestFillPostWithJsonFlattenAndReturnResultPdf()
    {
      var dummyItem = new
      {
        topmostSubform = new List<dynamic>()
        {
          new { Page1 = new List<dynamic>()
            {
              new { Address = new List<dynamic>()
                {
                  new { f1_7 = new List<string>()
                    {
                      "Hello"
                    }
                  }
                }
              }
            }
          }
        }
      };

      var text = JsonConvert.SerializeObject(dummyItem);
      JObject param = JObject.Parse(text);
                                                                                                                                
      IDictionary<string, PdfField> myList = new Dictionary<string, PdfField>();
      var pdfService = new Mock<IPdfService>();
      pdfService.Setup(p => p.DownloadUrl(It.IsAny<string>())).Returns(new MemoryStream());
      pdfService.Setup(p => p.FillForm(It.IsAny<Stream>(), It.IsAny<IDictionary<string, string>>())).Returns(new MemoryStream());

      using (var controller = new PdfFormController(pdfService.Object))
      {
        controller.ControllerContext = this.GetControllerContext();
        controller.FillWithJson("https://www.irs.gov/pub/irs-pdf/fw9.pdf", param);
      }

      pdfService.VerifyAll();
    }
  }
}
