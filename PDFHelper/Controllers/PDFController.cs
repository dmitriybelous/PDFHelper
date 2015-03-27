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
            //PDFHelper pdf = new PDFHelper("your pdf path.pdf");

            return View();
        }
    }
}