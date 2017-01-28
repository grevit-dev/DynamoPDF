using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF
{
    /// <summary>
    /// PDF Coordinates
    /// </summary>
    [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
    public class PDFCoords
    {
        /// <summary>
        /// X
        /// </summary>
        public float X;

        /// <summary>
        /// Y
        /// </summary>
        public float Y;

        /// <summary>
        /// PDF Coords
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public PDFCoords(double x, double y)
        {
            this.X = (float)x;
            this.Y = (float)y;
        }

    }
}
