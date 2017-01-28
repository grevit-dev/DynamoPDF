using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Phrase
    /// </summary>
    public class Phrase : IPDFContent
    {
        private double Leading;
        private string Content;
        private Font Font;

        /// <summary>
        /// Create a new Phrase from content
        /// </summary>
        /// <param name="content"></param>
        /// <param name="leading"></param>
        /// <param name="font"></param>
        public  Phrase(string content, double leading, Font font)
        {

                Leading = leading;
                Font = font;
                Content = content;

        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {
            iTextSharp.text.Phrase content = new iTextSharp.text.Phrase((float)Leading, Content, Font.ToPDFFont());
            return content;
        }
    }
}
