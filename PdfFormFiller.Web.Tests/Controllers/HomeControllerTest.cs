using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PdfFormFiller.Web;
using PdfFormFiller.Web.Controllers;

namespace PdfFormFiller.Web.Tests.Controllers
{
  /// <summary>
  /// Home Controller tests container.
  /// </summary>
  [TestClass]
  public class HomeControllerTest
  {
    /// <summary>
    /// Indexes this instance.
    /// </summary>
    [TestMethod]
    public void Index()
    {
      // Arrange
      HomeController controller = new HomeController();

      // Act
      ViewResult result = controller.Index() as ViewResult;

      // Assert
      Assert.IsNotNull(result);
      Assert.AreEqual("Home Page", result.ViewBag.Title);
    }
  }
}
