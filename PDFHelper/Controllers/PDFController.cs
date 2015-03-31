using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PDFHelper.Controllers
{
    public class PDFController : Controller
    {
        // GET: PDF
        public ActionResult Index()
        {
            PDFHelper pdf = new PDFHelper(@"C:\test.pdf");
            //pdf.Download();
            //PDFHelper.CreatePDF("Create pdf test", @"C:\test2.pdf");

            return View();
        }
    }
}