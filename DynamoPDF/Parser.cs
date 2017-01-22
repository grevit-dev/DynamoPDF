
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Dynamo.Utilities;
using Dynamo.Models;

namespace DynamoPDF
{
    /// <summary>
    /// PDF Parser for Dynamo
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Read PDF annotations into Dynamo
        /// </summary>
        /// <param name="filepath">PDF filepath</param>
        /// <param name="scale">Scaling factor (optional)</param>
        /// <param name="page">Document Page (optional)</param>
        /// <returns>Annotation Objects</returns>
        public static IEnumerable<AnnotationObject> GetAnnotationsFromPDF(string filepath, double scale = 1, int page = 1)
        {
            if (!System.IO.File.Exists(filepath))
                throw new Exception(Properties.Resources.FileNotFoundError);

            List<AnnotationObject> elements = new List<AnnotationObject>();

            // Open a new memory stream
            using (var ms = new System.IO.MemoryStream())
            {
                // Create a new pdf reader and get the first page
                PdfReader myPdfReader = new PdfReader(filepath);

                if (page < 1 || page > myPdfReader.NumberOfPages)
                    throw new Exception(Properties.Resources.WrongPageNumber);

                PdfDictionary pageDict = myPdfReader.GetPageN(page);
                PdfArray annotArray = pageDict.GetAsArray(PdfName.ANNOTS);

                // Walk through the annotation element array
                for (int i = 0; i < annotArray.Size; i++)
                {
                    // Get the elements type and subject to filter by
                    PdfDictionary annotationElement = annotArray.GetAsDict(i);
                    PdfName subject = annotationElement.GetAsName(PdfName.SUBTYPE);
                    if (subject != null)
                    {
                        if (subject == PdfName.LINE)
                        {
                            elements.Add(annotationElement.ToLine(scale));
                        }
                        else if (subject == PdfName.POLYGON)
                        {
                            elements.Add(annotationElement.ToPolyCurve(scale, true));
                        }
                        else if (subject == PdfName.POLYLINE)
                        {
                            elements.Add(annotationElement.ToPolyCurve(scale, false));
                        }
                        else if (subject == PdfName.SQUARE)
                        {
                            elements.Add(annotationElement.ToRectangle(scale));
                        }
                    }
                }
            }

            return elements;
        }

        /// <summary>
        /// Get Annotation Object's content (Geometry, Author, etc)
        /// </summary>
        /// <param name="annotation">PDF Annotation Object</param>
        [MultiReturn(new[] { "Author", "Contents", "Modified", "Created", "Geometry" })]
        public static Dictionary<string, object> GetAnnotation(AnnotationObject annotation)
        {
            return new Dictionary<string, object>()
            {
                {"Author", annotation.Author},
                {"Contents", annotation.Contents},
                {"Modified", annotation.Updated},
                {"Created", annotation.Created},
                {"Geometry", annotation.Geometry}
            };
        }
    }

}
