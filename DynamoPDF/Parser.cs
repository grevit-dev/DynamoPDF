
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
        /// Read PDF annotations and return their geometries
        /// </summary>
        /// <param name="filepath">PDF filepath</param>
        /// <param name="scale">Conversion factor (scaling)</param>
        /// <returns>Geometry collection</returns>
        public static IEnumerable<Geometry> GetGeometries(string filepath, double scale = 0)
        {
            if (!System.IO.File.Exists(filepath))
                throw new Exception(Properties.Resources.FileNotFoundError);


            List<Geometry> geometries = new List<Geometry>();

            // Open a new memory stream
            using (var ms = new System.IO.MemoryStream())
            {
                // Create a new pdf reader and get the first page
                PdfReader myPdfReader = new PdfReader(filepath);
                PdfDictionary pageDict = myPdfReader.GetPageN(1);
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
                            geometries.Add(annotationElement.ToLine(scale));
                        }
                        else if (subject == PdfName.POLYGON)
                        {
                            geometries.Add(annotationElement.ToPolyCurve(scale, true));
                        }
                        else if (subject == PdfName.POLYLINE)
                        {
                            geometries.Add(annotationElement.ToPolyCurve(scale, false));
                        }
                    }
                }
            }

            return geometries;
        }
    }

}
