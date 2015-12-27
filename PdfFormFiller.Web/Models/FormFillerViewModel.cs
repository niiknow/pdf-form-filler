using PdfFormFiller.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfFormFiller.Web.Models
{
  /// <summary>
  /// View Model
  /// </summary>
  public class FormFillerViewModel
  {
    /// <summary>
    /// Gets or sets the fields.
    /// </summary>
    /// <value>
    /// The fields.
    /// </value>
    public IDictionary<string, PdfField> Fields { get; set; }

    /// <summary>
    /// Gets or sets the fields json.
    /// </summary>
    /// <value>
    /// The fields json.
    /// </value>
    public string FieldsJson { get; set; }
  }
}