using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfFormFiller.Web.Models
{
  public class FormFillerViewModel
  {
    public IDictionary<string, string> Fields { get; set; }
  }
}