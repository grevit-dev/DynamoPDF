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
        /// Convert to DS Rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static Autodesk.DesignScript.Geometry.Rectangle ToDSRectangle(this iTextSharp.text.Rectangle rectangle)
        {

            var p1 = Autodesk.DesignScript.Geometry.Point.ByCoordinates(0, 0, 0);
            var p2 = Autodesk.DesignScript.Geometry.Point.ByCoordinates(0, rectangle.Height, 0);
            var p3 = Autodesk.DesignScript.Geometry.Point.ByCoordinates(rectangle.Width, rectangle.Height, 0);
            var p4 = Autodesk.DesignScript.Geometry.Point.ByCoordinates(rectangle.Width, 0, 0);

            return Autodesk.DesignScript.Geometry.Rectangle.ByCornerPoints(p1, p2, p3, p4);

        }

        /// <summary>
        /// Parse PDF double value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static double ToDouble(this PdfObject obj)
        { 
            double value = 0;
            if (double.TryParse(obj.ToString(), out value))
                return value;
            else
                return 0;
        }

        public static float ToFloat(this PdfObject obj)
        {
            float value = 0f;
            if (float.TryParse(obj.ToString(), out value))
                return value;
            else
                return 0f;
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
        public static Content.Annotation ToLine(this PdfDictionary annotation)
        {
            PdfArray data = annotation.GetAsArray(PdfName.L);
            if (data == null) return null;

            Point start = Point.ByCoordinates(data[0].ToDouble(), data[1].ToDouble());
            Point end = Point.ByCoordinates(data[2].ToDouble(), data[3].ToDouble());
            return new Content.Annotation(annotation, Line.ByStartPointEndPoint(start, end));
        }

        /// <summary>
        /// Parse PDF Polyline to PolyCurve
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static Content.Annotation ToPolyCurve(this PdfDictionary annotation, bool close)
        {
            PdfArray data = annotation.GetAsArray(PdfName.VERTICES);
            if (data == null) return null;

            List<Point> points = new List<Point>();
            for (int j = 0; j < data.Size - 1; j=j+2)
            {
                points.Add(Point.ByCoordinates(data[j].ToDouble(), data[j + 1].ToDouble()));
            }

            if (points.First().IsAlmostEqualTo(points.Last()))
                points.RemoveAt(points.Count - 1);

            return new Content.Annotation(annotation, PolyCurve.ByPoints(points, close));
        }

        /// <summary>
        /// Parse PDF Rectangle to Rectangle
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static Content.Annotation ToRectangle(this PdfDictionary annotation)
        {
            PdfArray data = annotation.GetAsArray(PdfName.RECT);
            if (data == null) return null;

            List<Point> points = new List<Point>();
            points.Add(Point.ByCoordinates(data[0].ToDouble(), data[1].ToDouble()));
            points.Add(Point.ByCoordinates(data[0].ToDouble(), data[3].ToDouble()));
            points.Add(Point.ByCoordinates(data[2].ToDouble(), data[3].ToDouble()));
            points.Add(Point.ByCoordinates(data[2].ToDouble(), data[1].ToDouble()));

            return new Content.Annotation(annotation, Rectangle.ByCornerPoints(points));
        }

        /// <summary>
        /// Convert to Circle
        /// </summary>
        /// <param name="annotation"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static Content.Annotation ToCircle(this PdfDictionary annotation)
        {
            PdfArray data = annotation.GetAsArray(PdfName.RECT);
            if (data == null) return null;


            var min = Point.ByCoordinates(data[0].ToDouble(), data[1].ToDouble());
            var max = Point.ByCoordinates(data[2].ToDouble(), data[3].ToDouble());

            var center = Point.ByCoordinates(min.X + ((max.X - min.X) / 2), min.Y + ((max.Y - min.Y) / 2));
            var radius = (max.X - min.X) / 2;
            return new Content.Annotation(annotation, Circle.ByCenterPointRadius(center, radius));
        }

    }

}
