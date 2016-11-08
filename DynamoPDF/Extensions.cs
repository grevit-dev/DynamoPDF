using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;

namespace DynamoPDF
{
    /// <summary>
    /// Extensions for PDF parsing
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public static class Extensions
    {
        /// <summary>
        /// Parse PDF double value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static double ToDouble(this PdfObject obj, double scale = 0)
        { 
            double value = 0;
            if (double.TryParse(obj.ToString(), out value))
                return value *= scale;
            else
                return 0;
        }

        /// <summary>
        /// Parse PDF Line
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static Line ToLine(this PdfDictionary annotation, double scale)
        {
            PdfArray data = annotation.GetAsArray(PdfName.L);
            Point start = Point.ByCoordinates(data[0].ToDouble(scale), data[1].ToDouble(scale));
            Point end = Point.ByCoordinates(data[2].ToDouble(scale), data[3].ToDouble(scale));
            return Line.ByStartPointEndPoint(start, end);
        }

        /// <summary>
        /// Parse PDF Polyline to PolyCurve
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static PolyCurve ToPolyCurve(this PdfDictionary annotation, double scale, bool close)
        {
            PdfArray data = annotation.GetAsArray(PdfName.VERTICES);

            List<Point> points = new List<Point>();
            for (int j = 0; j < data.Size - 1; j++)
            {
                points.Add(Point.ByCoordinates(data[j].ToDouble(scale), data[j + 1].ToDouble(scale)));
            }

            return PolyCurve.ByPoints(points, close);
        }
    }

}
