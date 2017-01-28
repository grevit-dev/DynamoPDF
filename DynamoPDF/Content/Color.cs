using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// Static Colors and Converters
    /// </summary>
    public static class Colors
    {

        /// <summary>
        /// Convert to PDF Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static iTextSharp.text.BaseColor ToPDFColor(this DSCore.Color color)
        {
            return new iTextSharp.text.BaseColor(color.Red, color.Green, color.Blue, color.Alpha);
        }

        /// <summary>
        /// Convert to DS Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static DSCore.Color ToDSColor(this iTextSharp.text.BaseColor color)
        {
            return DSCore.Color.ByARGB(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// White for Pages
        /// </summary>
        public static DSCore.Color White
        {
            get { return DSCore.Color.ByARGB(255, 255, 255, 255); }
        }

        /// <summary>
        /// Black for Text
        /// </summary>
        public static DSCore.Color Black
        {
            get { return DSCore.Color.ByARGB(255, 0, 0, 0); }
        }

        /// <summary>
        /// DarkGray
        /// </summary>
        public static DSCore.Color DarkGray
        {
            get { return DSCore.Color.ByARGB(255, 50, 50, 50); }
        }

        /// <summary>
        /// LightGray
        /// </summary>
        public static DSCore.Color LightGray
        {
            get { return DSCore.Color.ByARGB(255, 200, 200, 200); }
        }


        /// <summary>
        /// Red
        /// </summary>
        public static DSCore.Color Red
        {
            get { return DSCore.Color.ByARGB(255, 255, 0, 0); }
        }

        /// <summary>
        /// Blue
        /// </summary>
        public static DSCore.Color Blue
        {
            get { return DSCore.Color.ByARGB(255, 0, 0, 255); }
        }

        /// <summary>
        /// Green
        /// </summary>
        public static DSCore.Color Green
        {
            get { return DSCore.Color.ByARGB(255, 0, 255, 0); }
        }
    }
}
