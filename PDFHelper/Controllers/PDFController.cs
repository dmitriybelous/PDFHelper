using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PDFHelper.Controllers
{
    public class PDFController : Controller
    {
        // GET: PDF
        public ActionResult Index()
        {
            PDFHelper pdf = new PDFHelper(@"C:\merge.pdf");

            List<string> list = new List<string>();
            list.Add(@"C:\merge.pdf");
            list.Add(@"C:\test.pdf");
            MemoryStream ms = PDFHelper.CombineMultiplePDFs(list);
            PDFHelper.DownloadAsPDF(ms);
            //pdf.AddBlankPage(@"C:\blank.pdf");
            //pdf.Burst();
            //pdf.Download();
            //PDFHelper.CreatePDF("Create pdf test", @"C:\test2.pdf");

            return View();
        }
    }
}