using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DynamoPDF.Tests
{
    [TestClass]
    public class ContentTests
    {
        [TestMethod]
        public void ColorsAreValid()
        {
            var color = DynamoPDF.Content.Colors.Green;

            Assert.AreEqual(0, color.Red);
            Assert.AreEqual(255, color.Green);
            Assert.AreEqual(0, color.Blue);

            color = DynamoPDF.Content.Colors.Red;

            Assert.AreEqual(255, color.Red);
            Assert.AreEqual(0, color.Green);
            Assert.AreEqual(0, color.Blue);

            color = DynamoPDF.Content.Colors.Blue;

            Assert.AreEqual(0, color.Red);
            Assert.AreEqual(0, color.Green);
            Assert.AreEqual(255, color.Blue);

            color = DynamoPDF.Content.Colors.Black;

            Assert.AreEqual(0, color.Red);
            Assert.AreEqual(0, color.Green);
            Assert.AreEqual(0, color.Blue);

            color = DynamoPDF.Content.Colors.White;

            Assert.AreEqual(255, color.Red);
            Assert.AreEqual(255, color.Green);
            Assert.AreEqual(255, color.Blue);
        }

        [TestMethod]
        public void FontIsValid()
        {
            DynamoPDF.Content.Font f = new Content.Font(DynamoPDF.Content.Colors.Black, "HELVETICA", 12);
            var pdfFont = f.ToPDFFont();
            Assert.AreEqual(0, pdfFont.Color.R);
            Assert.AreEqual(0, pdfFont.Color.G);
            Assert.AreEqual(0, pdfFont.Color.B);
            Assert.AreEqual(12, pdfFont.Size);
            Assert.AreEqual("HELVETICA", pdfFont.Familyname.ToUpper());
        }

        [TestMethod]
        public void PhraseIsValid()
        {
            var font = new Content.Font(DynamoPDF.Content.Colors.Black, "HELVETICA", 12);
            DynamoPDF.Content.Phrase p = new Content.Phrase("Hello World", 0, font);
            var pdf = p.ToPDF();

            Assert.AreEqual("Hello World", pdf.Chunks[0].Content);
            Assert.AreEqual("HELVETICA", pdf.Chunks[0].Font.Familyname.ToUpper());
            Assert.AreEqual(12, pdf.Chunks[0].Font.Size);
        }

        [TestMethod]
        public void ParagraphIsValid()
        {
            var font = new Content.Font(DynamoPDF.Content.Colors.Black, "HELVETICA", 12);
            DynamoPDF.Content.Phrase p = new Content.Phrase("Hello World", 0, font);
            DynamoPDF.Content.Paragraph pg = new Content.Paragraph(p, 10, 10);
            var pdf = pg.ToPDF();

            Assert.AreEqual("Hello World", pdf.Chunks[0].Chunks[0].Content);
            Assert.AreEqual("HELVETICA", pdf.Chunks[0].Chunks[0].Font.Familyname.ToUpper());
            Assert.AreEqual(12, pdf.Chunks[0].Chunks[0].Font.Size);
        }

        [TestMethod]
        public void PageBreakIsValid()
        {
            var pb = new Content.PageBreak();
            Assert.IsNotNull(pb);
        }

        [TestMethod]
        public void ListIsValid()
        {
            var font = new Content.Font(DynamoPDF.Content.Colors.Black, "HELVETICA", 12);
            DynamoPDF.Content.Phrase p = new Content.Phrase("Hello World", 0, font);
            DynamoPDF.Content.ListItem pg = new Content.ListItem(p);
            var list = new Content.List(true, false, new object[] { pg });
            var pdf = pg.ToPDF();

            Assert.AreEqual("Hello World", pdf.Chunks[0].Chunks[0].Content);
            Assert.AreEqual("HELVETICA", pdf.Chunks[0].Chunks[0].Font.Familyname.ToUpper());
            Assert.AreEqual(12, pdf.Chunks[0].Chunks[0].Font.Size);
        }

        //[TestMethod]
        public void RectPageIsValid()
        {
            var p = Content.Page.PageByWidthHeight(500,700, DynamoPDF.Content.Colors.White);
            Autodesk.DesignScript.Geometry.Rectangle rect = p.Values.Last() as Autodesk.DesignScript.Geometry.Rectangle;
            Content.Page page = p.Values.First() as Content.Page;

            Assert.AreEqual(500, rect.Width);
            Assert.AreEqual(700, rect.Height);


            var pdf = page.ToPDF();

            Assert.AreEqual(500, pdf.Width);
            Assert.AreEqual(700, pdf.Height);
            Assert.AreEqual(255, pdf.BackgroundColor.R);
            Assert.AreEqual(255, pdf.BackgroundColor.G);
            Assert.AreEqual(255, pdf.BackgroundColor.B);

        }

        //[TestMethod]
        public void DefaultPageIsValid()
        {
            var p = Content.Page.PageBySize("A4", DynamoPDF.Content.Colors.White);
            Autodesk.DesignScript.Geometry.Rectangle rect = p.Values.Last() as Autodesk.DesignScript.Geometry.Rectangle;
            Content.Page page = p.Values.First() as Content.Page;

            Assert.AreEqual(210, rect.Width);
            Assert.AreEqual(297, rect.Height);


            var pdf = page.ToPDF();

            Assert.AreEqual(210, pdf.Width);
            Assert.AreEqual(297, pdf.Height);
            Assert.AreEqual(255, pdf.BackgroundColor.R);
            Assert.AreEqual(255, pdf.BackgroundColor.G);
            Assert.AreEqual(255, pdf.BackgroundColor.B);

        }


    }
}
