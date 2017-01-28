using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Pagebreak
    /// </summary>
    public class PageBreak : IPDFContent
    {
        /// <summary>
        /// Create a new Pagebreak
        /// </summary>
        public PageBreak()
        {

        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {
            return null;
        }
    }
}
