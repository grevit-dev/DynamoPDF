
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Utilities;
using Dynamo.Models;

namespace DynamoPDF
{
    /// <summary>
    /// PDF Parser for Dynamo
    /// </summary>
    public static class Read
    {
        /// <summary>
        /// Read PDF annotations into Dynamo
        /// </summary>
        /// <param name="filepath">PDF filepath</param>
        /// <param name="page">Document Page number (optional)</param>
        /// <returns>Annotation Objects</returns>
        [MultiReturn(new string[]{ "Annotations", "Rectangle" })]
        public static Dictionary<string,object> GetAnnotationsFromPDF(string filepath, int page = 1)
        {
            if (!System.IO.File.Exists(filepath))
                throw new Exception(Properties.Resources.FileNotFoundError);

            List<Content.Annotation> elements = new List<Content.Annotation>();

            Rectangle pagesize = null;

            // Open a new memory stream
            using (var ms = new System.IO.MemoryStream())
            {
                // Create a new pdf reader and get the first page
                PdfReader myPdfReader = new PdfReader(filepath);

                if (page < 1 || page > myPdfReader.NumberOfPages)
                    throw new Exception(Properties.Resources.WrongPageNumber);

                PdfDictionary pageDict = myPdfReader.GetPageN(page);
                PdfArray annotArray = pageDict.GetAsArray(PdfName.ANNOTS);
                pagesize = myPdfReader.GetPageSizeWithRotation(page).ToDSRectangle();

                if (annotArray == null) throw new Exception(Properties.Resources.NoAnnotations);

                // Walk through the annotation element array
                for (int i = 0; i < annotArray.Size; i++)
                {
                    // Get the elements type and subject to filter by
                    PdfDictionary annotationElement = annotArray.GetAsDict(i);
                    PdfName subtype = annotationElement.GetAsName(PdfName.SUBTYPE);
                    if (subtype != null)
                    {
                        if (subtype == PdfName.LINE)
                        {
                            elements.Add(annotationElement.ToLine());
                        }
                        else if (subtype == PdfName.POLYGON)
                        {
                            elements.Add(annotationElement.ToPolyCurve( true));
                        }
                        else if (subtype == PdfName.POLYLINE)
                        {
                            elements.Add(annotationElement.ToPolyCurve( false));
                        }
                        else if (subtype == PdfName.SQUARE)
                        {
                            elements.Add(annotationElement.ToRectangle());
                        }
                        else if (subtype == PdfName.FREETEXT)
                        {
                            elements.Add(annotationElement.ToRectangle());
                        }
                        else if (subtype == PdfName.CIRCLE)
                        {
                            elements.Add(annotationElement.ToCircle());
                        }
                    }
                }

                string content = PdfTextExtractor.GetTextFromPage(myPdfReader, page, new SimpleTextExtractionStrategy());
            }

            return new Dictionary<string,object>()
            {
                {"Annotations", elements},
                {"Rectangle", pagesize}
            };
        }

        /// <summary>
        /// Get Text from PDF page
        /// </summary>
        /// <param name="filepath">PDF Filepath</param>
        /// <param name="page">Document Page number (optional)</param>
        /// <returns>Page Content as Text</returns>
        public static string GetPDFPageAsText(string filepath, int page = 1)
        {
            if (!System.IO.File.Exists(filepath))
                throw new Exception(Properties.Resources.FileNotFoundError);

            string content = string.Empty;

            // Open a new memory stream
            using (var ms = new System.IO.MemoryStream())
            {
                // Create a new pdf reader and get the first page
                PdfReader myPdfReader = new PdfReader(filepath);

                if (page < 1 || page > myPdfReader.NumberOfPages)
                    throw new Exception(Properties.Resources.WrongPageNumber);
                
                string nonformatcontent = PdfTextExtractor.GetTextFromPage(myPdfReader, page, new SimpleTextExtractionStrategy());
                content = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(nonformatcontent)));
            }

            return content;
        }

        /// <summary>
        /// Get all Text from PDF
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetEntirePDFAsText(string filepath)
        {
            if (!System.IO.File.Exists(filepath))
                throw new Exception(Properties.Resources.FileNotFoundError);

            string content = string.Empty;

            // Open a new memory stream
            using (var ms = new System.IO.MemoryStream())
            {
                // Create a new pdf reader and get the first page
                PdfReader myPdfReader = new PdfReader(filepath);

                for (int i = 1; i <= myPdfReader.NumberOfPages; i++)
                {
                    string nonformatcontent = PdfTextExtractor.GetTextFromPage(myPdfReader, i, new SimpleTextExtractionStrategy());
                    string pagebreak = (i > 1) ? "\n" : "";

                    content += pagebreak + Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(nonformatcontent)));
                }
            }

            return content;
        }


    }

}

	
