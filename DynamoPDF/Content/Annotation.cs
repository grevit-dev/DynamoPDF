using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using iTextSharp.text.pdf;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;

namespace DynamoPDF.Content
{
    /// <summary>
    /// Annotation Object
    /// </summary>
    public class Annotation
    {
        /// <summary>
        /// Date Created
        /// </summary>
        private DateTime Created;

        /// <summary>
        /// Date Updated
        /// </summary>
        private DateTime Updated;

        /// <summary>
        /// Contents
        /// </summary>
        private string Contents;

        /// <summary>
        /// Author
        /// </summary>
        private string Author;

        /// <summary>
        /// Dynamo Geometry
        /// </summary>
        private Geometry Geometry;

        /// <summary>
        /// Subject
        /// </summary>
        private string Subject;

        /// <summary>
        /// Color
        /// </summary>
        private DSCore.Color Color;

        /// <summary>
        /// Get Annotation Color
        /// </summary>
        public DSCore.Color GetColor { get { return this.Color; } }

        /// <summary>
        /// Get Subject
        /// </summary>
        public string GetSubject { get { return this.Subject; } }

        /// <summary>
        /// Get Author
        /// </summary>
        public string GetAuthor { get { return this.Author; } }

        /// <summary>
        /// Get Contents
        /// </summary>
        public string GetContents { get { return this.Contents; } }

        /// <summary>
        /// Get Updated Date
        /// </summary>
        public DateTime GetUpdated { get { return this.Updated; } }

        /// <summary>
        /// Get Created Date
        /// </summary>
        public DateTime GetCreated { get { return this.Created; } }

        /// <summary>
        /// Get Geometry
        /// </summary>
        public Geometry GetGeometry { get { return this.Geometry; } }

        /// <summary>
        /// Annotation Object
        /// </summary>
        /// <param name="created"></param>
        /// <param name="updated"></param>
        /// <param name="contents"></param>
        /// <param name="author"></param>
        /// <param name="geometry"></param>
        public Annotation(string contents, string author, string subject,Geometry geometry, DSCore.Color color)
        {
            this.Created = DateTime.Now;
            this.Updated = DateTime.Now;
            this.Contents = contents;
            this.Author = author;
            this.Geometry = geometry;
            this.Color = color;
            this.Subject = subject;
        }

        /// <summary>
        /// Annotation Object
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="geometry"></param>
        [IsVisibleInDynamoLibrary(false)]
        public Annotation(PdfDictionary annotation, Geometry geometry)
        {
            this.Geometry = geometry;

            PdfString createdString = annotation.GetAsString(PdfName.CREATIONDATE);
            this.Created = (createdString == null) ? DateTime.MinValue : createdString.ToString().ToDateTime();

            PdfString updatedString = annotation.GetAsString(PdfName.M);
            this.Updated = (updatedString == null) ? DateTime.MinValue : updatedString.ToString().ToDateTime();

            PdfString contents = annotation.GetAsString(PdfName.CONTENTS);
            this.Contents = (contents == null) ? string.Empty : contents.ToString();

            PdfString author = annotation.GetAsString(PdfName.T);
            this.Author = (author == null) ? string.Empty : author.ToString();

            PdfString subject = annotation.GetAsString(PdfName.SUBJECT);
            this.Subject = (subject == null) ? string.Empty : subject.ToString();



            PdfArray color = annotation.GetAsArray(PdfName.C);
            if (color != null && color.Size == 3)
            {
                var pdfcolor = new iTextSharp.text.BaseColor(color[0].ToFloat(), color[1].ToFloat(), color[2].ToFloat());
                this.Color = pdfcolor.ToDSColor();
            }


            if (this.Color == null)
            {

                PdfString ds = annotation.GetAsString(PdfName.DS);
                //font: Helvetica 12pt; text-align:left; margin:3pt; line-height:13.8pt; color:#000000
                if (ds != null)
                {
                    if (ds.ToString().Contains(';'))
                    {
                        string[] data = ds.ToString().Split(';');
                        Dictionary<string, string> datadict = new Dictionary<string, string>();

                        foreach (string d in data)
                        {
                            if (d.Contains(':'))
                            {
                                string[] vp = d.Split(':');
                                string key = vp[0].Replace(" ", "");
                                string val = vp[1].Replace(" ", "");

                                if (!datadict.ContainsKey(key))
                                    datadict.Add(key, val);
                            }
                        }

                        if (datadict.ContainsKey("color"))
                        {
                            var syscolor = System.Drawing.ColorTranslator.FromHtml(datadict["color"].ToUpper());

                            if (syscolor != null)
                                this.Color = DSCore.Color.ByARGB(syscolor.A, syscolor.R, syscolor.G, syscolor.B);
                        }
                    }
                }

                if (this.Color == null)
                    this.Color = DSCore.Color.ByARGB(255, 255, 0, 0);
            }

                

        }

        /// <summary>
        /// Stamp Annotation to PDF
        /// </summary>
        /// <param name="stamper"></param>
        /// <param name="page"></param>
        /// <param name="scale"></param>
        [IsVisibleInDynamoLibrary(false)]
        public void StampAnnotation(PdfStamper stamper, int page, double scale)
        {
            PdfAnnotation annotation = null;

            if (Geometry == null)
            {
                throw new Exception(Properties.Resources.NoGeometry);
            }
            else if (Geometry.GetType() == typeof(Autodesk.DesignScript.Geometry.Line))
            {
                var line = Geometry as Autodesk.DesignScript.Geometry.Line;
                annotation = line.ToPDFLine(Contents, stamper.Writer);
            }
            else if (Geometry.GetType() == typeof(Autodesk.DesignScript.Geometry.Polygon))
            {
                var polygon = Geometry as Autodesk.DesignScript.Geometry.Polygon;
                annotation = polygon.ToPDFPolygon(Contents, stamper.Writer);
            }
            else if (Geometry.GetType() == typeof(Autodesk.DesignScript.Geometry.PolyCurve))
            {
                var polycurve = Geometry as Autodesk.DesignScript.Geometry.PolyCurve;
                annotation = polycurve.ToPDFPolygon(Contents, stamper.Writer);
            }
            else if (Geometry.GetType() == typeof(Autodesk.DesignScript.Geometry.Circle))
            {
                var circle = Geometry as Autodesk.DesignScript.Geometry.Circle;
                annotation = circle.ToPDFCircle(Contents, stamper.Writer);
            }
            else
            {
                throw new Exception(Properties.Resources.NotSupported);
            }

            annotation.Put(PdfName.T, new PdfString(Author));
            annotation.Put(PdfName.M, new PdfDate(DateTime.Now));
            annotation.Put(PdfName.CREATIONDATE, new PdfDate(DateTime.Now));
            annotation.Put(PdfName.CONTENTS, new PdfString(Contents));

            float[] floatdata = { Convert.ToSingle(Color.Red), Convert.ToSingle(Color.Green), Convert.ToSingle(Color.Blue)};
            annotation.Put(PdfName.C, new PdfArray(floatdata));

            stamper.AddAnnotation(annotation, page);
        }
    }
}
