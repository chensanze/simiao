using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShiMiao.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
#if DEBUG
#else
            throw new Exception("ex");
#endif
            return View();
        }
        public ActionResult QRCode()
        {
            return View();
        }
    }
}