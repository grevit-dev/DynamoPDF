using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// IPDF Content Interface
    /// </summary>
    [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
    public interface IPDFContent
    {
        /// <summary>
        /// Default ToPDF Method
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        iTextSharp.text.IElement ToPDF();
    }
}
