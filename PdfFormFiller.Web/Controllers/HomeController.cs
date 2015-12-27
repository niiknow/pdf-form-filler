using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfFormFiller.Web.Controllers
{
  /// <summary>
  /// Home
  /// </summary>
  /// <seealso cref="System.Web.Mvc.Controller" />
  public class HomeController : Controller
  {
    /// <summary>
    /// Landing page.
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {
      ViewBag.Title = "Home Page";

      return View();
    }
  }
}
