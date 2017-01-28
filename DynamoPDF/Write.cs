using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;

namespace DynamoPDF
{
    public static class Write
    {

        /// <summary>
        /// Add Annotation Objects to an existing PDF
        /// </summary>
        /// <param name="filename">Existing PDF</param>
        /// <param name="targetpath">New filepath to write to</param>
        /// <param name="annotations"></param>
        /// <param name="page">Page to modify</param>
        public static void AddAnnotationsToPDF(string filename, string targetpath, IEnumerable<Content.Annotation> annotations, int page = 1)
        {

            using (var ms = new System.IO.MemoryStream())
            {
                PdfReader myPdfReader = new PdfReader(filename);
                PdfStamper stamper = new PdfStamper(myPdfReader, ms, '\0', true);

                foreach (var annotation in annotations)
                {
                    annotation.StampAnnotation(stamper, page, 1);
                }

                stamper.Close();
                myPdfReader.Close();

                System.IO.File.WriteAllBytes(targetpath, ms.ToArray());
            }

        }

        /// <summary>
        /// Create a new PDF document from contents
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        /// <param name="page"></param>
        /// <param name="marginLeft"></param>
        /// <param name="marginTop"></param>
        /// <param name="marginRight"></param>
        /// <param name="marginBottom"></param>
        public static void CreateNewPDF(string path, IEnumerable<object> contents, Content.Page page, double marginLeft = 5, double marginTop = 10, double marginRight = 5, double marginBottom = 10)
        {

            FileStream fs = new System.IO.FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(page.ToPDF());
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.SetMargins((float)marginLeft, (float)marginRight, (float)marginTop, (float)marginBottom);

            doc.Open();

            foreach (object content in contents)
            {
                var t = content.GetType();

                if (content.GetType().GetInterfaces().Contains(typeof(Content.IPDFContent)))
                {
                    if (content.GetType() == typeof(Content.PageBreak))
                    {
                        doc.NewPage();
                    }
                    else
                    {
                        Content.IPDFContent co = content as Content.IPDFContent;
                        doc.Add(co.ToPDF());
                    }
                }
                else
                {
                    if (content.GetType() == typeof(Geometries.PDFGeometry))
                    {
                        Geometries.PDFGeometry l = (Geometries.PDFGeometry)content;
                        l.ToPDF(writer);
                    }
                    else
                    {
                        doc.Add(new iTextSharp.text.Paragraph(content.ToString()));
                    }
                }

            }


            doc.Close();

        }






    }
}
