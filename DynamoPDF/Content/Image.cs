using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF Image
    /// </summary>
    public class Image : IPDFContent
    {
        private string Path;
        private double Width;
        private double Height;

        /// <summary>
        /// Create a new image in PDF from path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Image (string path, double width = 0, double height = 0)
        {

                Path = path;
                Width = width;
                Height = height;

        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {
            if (System.IO.File.Exists(Path))
            {
                
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(Path);

                if (Height > 0)
                    pic.ScaleAbsoluteHeight((float)Height);
                if (Width > 0)
                    pic.ScaleAbsoluteWidth((float)Width);

                return pic;
            }
            else return new iTextSharp.text.Rectangle((float)Width, (float)Height);

        }
    }
}
