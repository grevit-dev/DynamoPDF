using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Autodesk.DesignScript.Geometry;

namespace DynamoPDF
{
    [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
    public static class ToPDF
    {


        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static PdfAnnotation ToPDFFreeText(this string content, PdfWriter writer, float x, float y)
        {
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(x, y);
            var app = new PdfContentByte(writer);
            var anno = PdfAnnotation.CreateFreeText(writer, rect, content, app);
            return anno;
        }

        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static PDFCoords ToPDFCoords(this Point point)
        {
            return new PDFCoords(point.X, point.Y);
        }

        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static PdfAnnotation ToPDFLine(this Autodesk.DesignScript.Geometry.Line line, string content, PdfWriter writer)
        {
            var start = line.StartPoint.ToPDFCoords();
            var end = line.EndPoint.ToPDFCoords();

            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(start.X, start.Y);

            var app = new PdfContentByte(writer);
            var anno = PdfAnnotation.CreateLine(writer, rect, content, start.X, start.Y, end.X, end.Y);
            return anno;
        }

        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static PdfAnnotation ToPDFPolygon(this Autodesk.DesignScript.Geometry.PolyCurve polycurve, string content, PdfWriter writer)
        {
            List<float> points = new List<float>();
            foreach (var curve in polycurve.Curves())
            {
                PDFCoords coords = curve.StartPoint.ToPDFCoords();
                points.Add(coords.X);
                points.Add(coords.Y);
            }

            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(0, 0);

            var app = new PdfContentByte(writer);
            var anno = PdfAnnotation.CreatePolygonPolyline(writer, rect, content, false, new PdfArray(points.ToArray()));
            return anno;
        }

        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static PdfAnnotation ToPDFPolygon(this Autodesk.DesignScript.Geometry.Polygon polygon, string content, PdfWriter writer)
        {
            List<float> points = new List<float>();
            foreach (var pt in polygon.Points)
            {
                PDFCoords coords = pt.ToPDFCoords();
                points.Add(coords.X);
                points.Add(coords.Y);
            }

            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(0, 0);

            var app = new PdfContentByte(writer);
            var anno = PdfAnnotation.CreatePolygonPolyline(writer, rect, content, true, new PdfArray(points.ToArray()));
            return anno;
        }

        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public static PdfAnnotation ToPDFCircle(this Autodesk.DesignScript.Geometry.Circle circle, string content, PdfWriter writer)
        {
            
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(
                (float)circle.BoundingBox.MinPoint.X, (float)circle.BoundingBox.MinPoint.Y,
                (float)circle.BoundingBox.MaxPoint.X, (float)circle.BoundingBox.MaxPoint.Y
              );

            var app = new PdfContentByte(writer);
            var anno = PdfAnnotation.CreateSquareCircle(writer, rect, content, false);
            return anno;
        }
    }


}
