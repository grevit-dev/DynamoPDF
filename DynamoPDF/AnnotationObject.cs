using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using iTextSharp.text.pdf;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;

namespace DynamoPDF
{
    /// <summary>
    /// Annotation Object
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class AnnotationObject
    {
        /// <summary>
        /// Date Created
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public DateTime Created;

        /// <summary>
        /// Date Updated
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public DateTime Updated;

        /// <summary>
        /// Contents
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public string Contents;

        /// <summary>
        /// Author
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public string Author;

        /// <summary>
        /// Dynamo Geometry
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public Geometry Geometry;

        /// <summary>
        /// Annotation Object
        /// </summary>
        /// <param name="created"></param>
        /// <param name="updated"></param>
        /// <param name="contents"></param>
        /// <param name="author"></param>
        /// <param name="geometry"></param>
        [IsVisibleInDynamoLibrary(false)]
        public AnnotationObject(DateTime created, DateTime updated, string contents, string author,Geometry geometry)
        {
            this.Created = created;
            this.Updated = updated;
            this.Contents = contents;
            this.Author = author;
            this.Geometry = geometry;
        }

        /// <summary>
        /// Annotation Object
        /// </summary>
        /// <param name="annotation"></param>
        /// <param name="geometry"></param>
        [IsVisibleInDynamoLibrary(false)]
        public AnnotationObject(PdfDictionary annotation, Geometry geometry)
        {
            this.Geometry = geometry;

            PdfString createdString = annotation.GetAsString(PdfName.CREATIONDATE);
            this.Created = (createdString == null) ? DateTime.MinValue : createdString.ToString().ToDateTime();

            PdfString updatedString = annotation.GetAsString(PdfName.M);
            this.Updated = (updatedString == null) ? DateTime.MinValue : updatedString.ToString().ToDateTime();

            PdfString contents = annotation.GetAsString(PdfName.CONTENTS);
            this.Contents = (contents == null) ? string.Empty : contents.ToString();

            PdfString author = annotation.GetAsString(PdfName.T);
            this.Author = (author == null)? string.Empty : author.ToString();
        }
    }
}
