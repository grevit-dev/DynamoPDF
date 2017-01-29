using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Dyn = Autodesk.DesignScript.Geometry;
using DynamoPDF.Content;

namespace DynamoPDF.Geometries
{
    /// <summary>
    /// PDF Geometry
    /// </summary>
    public class PDFGeometry
    {
        private Dyn.Geometry Geometry;
        private PrintSettings Settings;

        /// <summary>
        /// Create PDF Geometry from Dynamo Geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="settings"></param>
        public PDFGeometry(Dyn.Geometry geometry, PrintSettings settings)
        {
            this.Geometry = geometry;
            this.Settings = settings;
        }

        /// <summary>
        /// Convert DS Geometry to PDF
        /// </summary>
        /// <param name="w"></param>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public void ToPDF(iTextSharp.text.pdf.PdfWriter w)
        {
            PdfContentByte cb = w.DirectContent;
            cb.SetLineWidth((float)Settings.Thickness);
            if (Settings.Fill != null)
                cb.SetColorFill(Settings.Fill.ToPDFColor());

            if (Settings.Stroke != null)
                cb.SetColorStroke(Settings.Stroke.ToPDFColor());

            if (Geometry.GetType() == typeof(Dyn.Arc))
            {
                Dyn.Arc arc = Geometry as Dyn.Arc;
                cb.MoveTo(arc.StartPoint.X, arc.EndPoint.Y);
                cb.CurveTo(arc.PointAtParameter(0.5).X, arc.PointAtParameter(0.5).Y, arc.EndPoint.X, arc.EndPoint.Y);
            }
            else if (Geometry.GetType() == typeof(Dyn.Line))
            {
                Dyn.Line line = Geometry as Dyn.Line;
                cb.MoveTo(line.StartPoint.X, line.StartPoint.Y);
                cb.LineTo(line.EndPoint.X, line.EndPoint.Y);
            }
            else if (Geometry.GetType() == typeof(Dyn.Circle))
            {
                Dyn.Circle circle = Geometry as Dyn.Circle;
                cb.Circle(circle.CenterPoint.X, circle.CenterPoint.Y, circle.Radius);
            }
            else if (Geometry.GetType() == typeof(Dyn.Ellipse))
            {
                Dyn.Ellipse ellipse = Geometry as Dyn.Ellipse;
                cb.Ellipse(ellipse.StartPoint.X, ellipse.StartPoint.Y, ellipse.EndPoint.X, ellipse.EndPoint.Y);
            }
            else if (Geometry.GetType() == typeof(Dyn.Rectangle))
            {
                Dyn.Rectangle rect = Geometry as Dyn.Rectangle;
                cb.Rectangle(rect.Center().X, rect.Center().Y, rect.Width, rect.Height);
            }
            else if (Geometry.GetType() == typeof(Dyn.Polygon))
            {
                Dyn.Polygon p = Geometry as Dyn.Polygon;
                foreach (var curve in p.Curves())
                    CurveToPDF(curve, cb);
            }
            else if (Geometry.GetType() == typeof(Dyn.PolyCurve))
            {
                Dyn.PolyCurve pc = Geometry as Dyn.PolyCurve;
                foreach (var curve in pc.Curves())
                    CurveToPDF(curve, cb);
            }
            else if (Geometry.GetType() == typeof(Dyn.NurbsCurve))
            {
                Dyn.NurbsCurve nc = Geometry as Dyn.NurbsCurve;
                    
                foreach (var linearc in nc.ApproximateWithArcAndLineSegments())
                    CurveToPDF(linearc, cb);
            }
            else if (Geometry.GetType() == typeof(Dyn.Curve))
            {
                Dyn.Curve curve = Geometry as Dyn.Curve;
                CurveToPDF(curve,cb);
            }
            else {
                throw new Exception(Properties.Resources.NotSupported);
            }

            if (Settings.Fill != null && Settings.Stroke != null)
            {
                cb.FillStroke();
            }
            else
            {
                if (Settings.Stroke != null)
                    cb.Stroke();
                if (Settings.Fill != null)
                    cb.Fill();
            }
        }


        /// <summary>
        /// Convert curve to PDF
        /// </summary>
        /// <param name="curve"></param>
        /// <param name="cb"></param>
        private void CurveToPDF(Dyn.Curve curve, PdfContentByte cb)
        {
            if (curve.GetType() == typeof(Dyn.Line))
            {
                Dyn.Line line = Geometry as Dyn.Line;
                cb.MoveTo(line.StartPoint.X, line.StartPoint.Y);
                cb.LineTo(line.EndPoint.X, line.EndPoint.Y);
            }
            else if (Geometry.GetType() == typeof(Dyn.Arc))
            {
                Dyn.Arc arc = Geometry as Dyn.Arc;
                cb.MoveTo(arc.StartPoint.X, arc.EndPoint.Y);
                cb.CurveTo(arc.PointAtParameter(0.5).X, arc.PointAtParameter(0.5).Y, arc.EndPoint.X, arc.EndPoint.Y);
            }
            else
            {
                cb.MoveTo(curve.StartPoint.X, curve.StartPoint.Y);
                cb.LineTo(curve.EndPoint.X, curve.EndPoint.Y);
            }
        }

    }
}
