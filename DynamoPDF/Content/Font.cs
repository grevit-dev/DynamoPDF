using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Font
    /// </summary>
    public class Font
    {

        private double FontSize;
        private string Style;
        private string FontFamily;
        private DSCore.Color Color;

        /// <summary>
        /// Create a new Font
        /// </summary>
        /// <param name="color"></param>
        /// <param name="fontfamily"></param>
        /// <param name="size"></param>
        /// <param name="style"></param>
        public Font(DSCore.Color color, string fontfamily = "HELVETICA", double size = 1.2, string style = "")
        {

            FontSize = size;
            Style = style;
            FontFamily = fontfamily;
            Color = color;

        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.Font ToPDFFont()
        {
            iTextSharp.text.Font.FontFamily family = iTextSharp.text.Font.FontFamily.HELVETICA;
            Enum.TryParse<iTextSharp.text.Font.FontFamily>(FontFamily, out family);

            iTextSharp.text.Font content = new iTextSharp.text.Font(family, (float)FontSize, iTextSharp.text.Font.GetStyleValue(Style), Color.ToPDFColor());

            return content;

        }
    }
}
