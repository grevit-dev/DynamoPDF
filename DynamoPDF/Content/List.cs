using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoPDF.Content
{
    /// <summary>
    /// PDF List
    /// </summary>
    public class List : IPDFContent
    {
        private bool Numbered;
        private bool Lettered;
        private IEnumerable<object> Items;

        /// <summary>
        /// Create a new PDF List from items
        /// </summary>
        /// <param name="numbered"></param>
        /// <param name="lettered"></param>
        /// <param name="items"></param>
        public List(bool numbered, bool lettered, IEnumerable<object> items)
        {

            Numbered = numbered;
                Lettered = lettered;
                Items = items;

        }

        /// <summary>
        /// Convert to PDF Content
        /// </summary>
        /// <returns></returns>
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public iTextSharp.text.IElement ToPDF()
        {

            iTextSharp.text.List content = new iTextSharp.text.List(Numbered,Lettered);

            foreach (object item in Items)
            {
                if (content.GetType().GetInterfaces().Contains(typeof(Content.IPDFContent)))
                {
                    IPDFContent cont = item as IPDFContent;
                    content.Add(cont.ToPDF());
                }
                else
                {
                    content.Add(item.ToString());
                }
            }

            return content;
        }
    }
}
