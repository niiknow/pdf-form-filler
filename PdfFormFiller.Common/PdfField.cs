using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormFiller.Common
{
  /// <summary>
  /// Represent a Pdf Field
  /// </summary>
  public class PdfField
  {
    /// <summary>
    /// Gets or sets the field type identifier.
    /// </summary>
    /// <value>
    /// The field type identifier.
    /// </value>
    public int FieldTypeId { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the original.
    /// </summary>
    /// <value>
    /// The name of the original.
    /// </value>
    public string OriginalName { get; set; }

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public string Value { get; set; }
  }
}
