using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Page
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Background Color
        /// </summary>
        private DSCore.Color Color;

        /// <summary>
        /// Page Rectangle
        /// </summary>
        private iTextSharp.text.Rectangle Rectangle;


        private Page(string pagesize, DSCore.Color color)
        {
            Rectangle = new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.GetRectangle(pagesize));
            Color = color;
        }

        private Page(double width, double height, DSCore.Color color)
        {
            Rectangle = new iTextSharp.text.Rectangle((float)width, (float)height);
            Color = color;
        }

        /// <summary>
        /// Create a new Page by width and height
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [MultiReturn(new string[] { "Page", "Rectangle" })]
        public static Dictionary<string, object> PageByWidthHeight(double width, double height, DSCore.Color color)
        {
            Page p = new Page(width,height, color);
            return new Dictionary<string, object>(){
             { "Page", p },
             { "Rectangle", p.Rectangle.ToDSRectangle() }
            };
        }

        /// <summary>
        /// Create a new Page by standard sizes like A4, A5, etc.
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [MultiReturn(new string[] { "Page", "Rectangle" })]
        public static Dictionary<string, object> PageBySize(string pagesize, DSCore.Color color)
        {
            Page p = new Page(pagesize, color);
            return new Dictionary<string, object>(){
             { "Page", p },
             { "Rectangle", p.Rectangle.ToDSRectangle() }
            };
        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.Rectangle ToPDF()
        {
            iTextSharp.text.Rectangle PDFPage = Rectangle;
            PDFPage.BackgroundColor = Color.ToPDFColor();
            return PDFPage;
        }
    }
}
