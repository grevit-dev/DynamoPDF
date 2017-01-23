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
        /// PDFString Date to DateTime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static DateTime ToDateTime(this string str)
        {
            return PdfDate.Decode(str);
        }

        /// <summary>
        /// Parse PDF Line
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static AnnotationObject ToLine(this PdfDictionary annotation, double scale)
        {
            PdfArray data = annotation.GetAsArray(PdfName.L);
            Point start = Point.ByCoordinates(data[0].ToDouble(scale), data[1].ToDouble(scale));
            Point end = Point.ByCoordinates(data[2].ToDouble(scale), data[3].ToDouble(scale));
            return new AnnotationObject(annotation, Line.ByStartPointEndPoint(start, end));
        }

        /// <summary>
        /// Parse PDF Polyline to PolyCurve
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static AnnotationObject ToPolyCurve(this PdfDictionary annotation, double scale, bool close)
        {
            PdfArray data = annotation.GetAsArray(PdfName.VERTICES);

            List<Point> points = new List<Point>();
            for (int j = 0; j < data.Size - 1; j=j+2)
            {
                points.Add(Point.ByCoordinates(data[j].ToDouble(scale), data[j + 1].ToDouble(scale)));
            }

            if (points.First().IsAlmostEqualTo(points.Last()))
                points.RemoveAt(points.Count - 1);

            return new AnnotationObject(annotation, PolyCurve.ByPoints(points, close));
        }

        /// <summary>
        /// Parse PDF Rectangle to Rectangle
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static AnnotationObject ToRectangle(this PdfDictionary annotation, double scale)
        {
            PdfArray data = annotation.GetAsArray(PdfName.RECT);

            List<Point> points = new List<Point>();
            points.Add(Point.ByCoordinates(data[0].ToDouble(scale), data[1].ToDouble(scale)));
            points.Add(Point.ByCoordinates(data[0].ToDouble(scale), data[3].ToDouble(scale)));
            points.Add(Point.ByCoordinates(data[2].ToDouble(scale), data[3].ToDouble(scale)));
            points.Add(Point.ByCoordinates(data[2].ToDouble(scale), data[1].ToDouble(scale)));

            return new AnnotationObject(annotation, Rectangle.ByCornerPoints(points));
        }

    }

}
