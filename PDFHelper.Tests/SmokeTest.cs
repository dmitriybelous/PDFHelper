using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFHelper.Tests
{
    [TestClass]
    public class SmokeTest
    {
        static string path = @"C:\test.pdf";

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            if (!File.Exists(path))
            {
                PDFHelper.CreatePDF("This is the test", path);
            }
        }

        public SmokeTest ()
        {

        }

        [TestMethod]
        public void PDFHelperInit()
        {
            PDFHelper helper = new PDFHelper(path);
            Assert.AreNotEqual(null, helper);
        }

        [TestMethod]
        public void GetFieldNames()
        {
            PDFHelper helper = new PDFHelper(path);
            Assert.AreNotEqual(null, helper.GetFieldNames());
        }

        [TestMethod]
        public void CreatePDF()
        {
            PDFHelper helper = new PDFHelper(path);
            bool exist = File.Exists(path);
            Assert.AreNotEqual(false, exist);
        }

        [TestMethod]
        public void ConvertToBytes()
        {
            Assert.AreNotEqual(null, PDFHelper.ConvertToBytes(path));
        }

        [TestMethod]
        public void ConvertPDFToMemory()
        {
            PDFHelper helper = new PDFHelper(path);
            Assert.AreNotEqual(null, helper.ConvertPDFToMemory());
        }

        [TestMethod]
        public void CombineMultiplePDFs()
        {
            string secondPDF = @"C:\test2.pdf";
            PDFHelper.CreatePDF("This is the test 2", secondPDF);

            List<string> filePaths = new List<string>();
            filePaths.Add(path);
            filePaths.Add(secondPDF);
            string combPath = @"C:\combine.pdf";
            Assert.AreNotEqual(null, PDFHelper.CombineMultiplePDFs(filePaths,combPath));

            File.Delete(secondPDF);
            File.Delete(combPath);
        }

        [TestMethod]
        public void CombinePDFsInMemory()
        {
            string secondPDF = @"C:\test2.pdf";
            PDFHelper.CreatePDF("This is the test 2", secondPDF);

            //string [] filePaths = { path, secondPDF};
            List<string> filePaths = new List<string>();
            filePaths.Add(path);
            filePaths.Add(secondPDF);
            string combPath = @"C:\combine.pdf";
            Assert.AreNotEqual(null, PDFHelper.CombineMultiplePDFs(filePaths));

            File.Delete(secondPDF);
            File.Delete(combPath);
        }

        [TestMethod]
        public void CountPages()
        {
            PDFHelper helper = new PDFHelper(path);
            Assert.AreNotEqual(null, helper.CountPages());
        }

        [TestMethod]
        public void ExtractText()
        {
            PDFHelper helper = new PDFHelper(path);
            string text = helper.ExtractText(1);
            Assert.AreNotEqual(null, text);
        }

        [TestMethod]
        public void Burst()
        {
            PDFHelper helper = new PDFHelper(path);
            List<string> paths = helper.Burst();
            Assert.AreNotEqual(0, paths.Count);

            foreach (var pdf in paths)
            {
                  File.Delete(pdf);
            }
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            File.Delete(path);
        }

    }
}
