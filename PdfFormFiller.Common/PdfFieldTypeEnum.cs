using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFormFiller.Common
{
  public enum PdfFieldTypeEnum
  {
    /// <summary>
    /// The none
    /// </summary>
    None = 0,

    /// <summary>
    /// The button
    /// </summary>
    Button = 1,

    /// <summary>
    /// The CheckBox
    /// </summary>
    CheckBox = 2,

    /// <summary>
    /// The RadioButton
    /// </summary>
    RadioButton = 3,

    /// <summary>
    /// The text field
    /// </summary>
    TextField = 4,

    /// <summary>
    /// The ListBox
    /// </summary>
    ListBox = 5,

    /// <summary>
    /// The ComboBox
    /// </summary>
    ComboBox = 6,

    /// <summary>
    /// The signature
    /// </summary>
    Signature = 7
  }
}
