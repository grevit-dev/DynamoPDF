using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DynamoPDF.Tests
{
    [TestClass]
    public class GeometryTests
    {


        [TestMethod]
        public void LineIsValid()
        {

            var a = Autodesk.DesignScript.Geometry.Point.ByCoordinates(0, 0, 0);
            var b = Autodesk.DesignScript.Geometry.Point.ByCoordinates(100, 0, 0);
            var line = Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(a, b);

            var settings = DynamoPDF.Geometries.PrintSettings.PrintStroke(2, DynamoPDF.Content.Colors.Black);

            var pdfgeo = new DynamoPDF.Geometries.PDFGeometry(line, settings);

            DynamoPDF.Write.CreateNewPDF("test.pdf", new object[] { pdfgeo }, (DynamoPDF.Content.Page)DynamoPDF.Content.Page.PageBySize("A4", DynamoPDF.Content.Colors.White).Values.First());

        }




    }
}
