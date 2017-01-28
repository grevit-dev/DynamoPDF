using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// List Item for PDF Lists
    /// </summary>
    public class ListItem : IPDFContent
    {
        private Phrase Phrase;

        /// <summary>
        /// Create List Item for PDF Lists
        /// </summary>
        /// <param name="phrase"></param>
        public ListItem(Phrase phrase)
        {

            Phrase = phrase;

        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {
            iTextSharp.text.ListItem content = new iTextSharp.text.ListItem((iTextSharp.text.Phrase)Phrase.ToPDF());
            return content;
        }
    }
}
