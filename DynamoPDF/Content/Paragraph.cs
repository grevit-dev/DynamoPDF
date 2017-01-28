using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Paragraph
    /// </summary>
    public class Paragraph : IPDFContent
    {
 
        private Phrase Phrase;
        private double LeftIndent;
        private double Rightindent;

        /// <summary>
        /// Create a new PDF Paragraph from a phrase
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="leftIndent"></param>
        /// <param name="rightindent"></param>
        public  Paragraph(Phrase phrase, double leftIndent = 0, double rightindent = 0)
        {

            Phrase = phrase;
                LeftIndent = leftIndent;
                Rightindent = rightindent;
            
        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {

            iTextSharp.text.Paragraph pg = new iTextSharp.text.Paragraph((iTextSharp.text.Phrase)Phrase.ToPDF());
            pg.IndentationLeft = (float)LeftIndent;
            pg.IndentationRight = (float)Rightindent;

            return pg;

        }
    }
}
