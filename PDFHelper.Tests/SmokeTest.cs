using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFHelper.Tests
{
    [TestClass]
    public class SmokeTest
    {
        static string path = @"C:\test.pdf";

        public SmokeTest ()
        {
           if (!File.Exists(path))
           {
               PDFHelper.WriteTextToPDF("This is the test", path);
           }
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
        public void WriteTextToPDF()
        {
            PDFHelper helper = new PDFHelper(path);
            bool exist = File.Exists(path);
            Assert.AreNotEqual(false, exist);
        }

        [TestMethod]
        public void GetBytesFromFile()
        {
            Assert.AreNotEqual(null, PDFHelper.GetBytesFromFile(path));
        }

        [TestMethod]
        public void CombineMultiplePDFs()
        {
            string secondPDF = @"C:\test2.pdf";
            PDFHelper.WriteTextToPDF("This is the test 2", secondPDF);

            string [] filePaths = { path, secondPDF};
            string combPath = @"C:\combine.pdf";
            Assert.AreNotEqual(null, PDFHelper.CombineMultiplePDFs(filePaths,combPath));

            File.Delete(secondPDF);
            File.Delete(combPath);
        }

        [TestMethod]
        public void CountPages()
        {
            PDFHelper helper = new PDFHelper(path);
            Assert.AreNotEqual(null, helper.CountPages());
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            File.Delete(path);
        }

    }
}
