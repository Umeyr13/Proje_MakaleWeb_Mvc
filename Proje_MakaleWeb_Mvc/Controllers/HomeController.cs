using MakaleBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proje_MakaleWeb_Mvc.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Test test = new Test(); 
            // Test class ında metot yazmadık. Ctor u na yazdık kodları. Burada new ile örnekleyince zaten oradaki ctor çalışıcak.
            return View();
        }
    }
}