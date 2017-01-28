using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Chapter
    /// </summary>
    public class Chapter : IPDFContent
    {
        /// <summary>
        /// Paragraph
        /// </summary>
        public Paragraph Paragraph;

        /// <summary>
        /// Number
        /// </summary>
        public int Number;

        /// <summary>
        /// Create a new Chapter
        /// </summary>
        /// <param name="paragraph"></param>
        /// <param name="number"></param>
        public Chapter(Paragraph paragraph, int number) { this.Paragraph = paragraph; this.Number = number; }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {

            iTextSharp.text.Chapter pg = new iTextSharp.text.Chapter((iTextSharp.text.Paragraph)Paragraph.ToPDF(), Number);

            return pg;

        }
    }
}
