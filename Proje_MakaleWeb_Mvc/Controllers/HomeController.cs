using MakaleBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Makale_Entities;

namespace Proje_MakaleWeb_Mvc.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Test test = new Test();
            // Test class ında metot yazmadık. Ctor u na yazdık kodları. Burada new ile örnekleyince zaten oradaki ctor çalışıcak.
            
            // test.EkleTest();
            //test.UpdateTest();
            // test.DeleteTest();
            // test.YorumTest();
            MakaleYonet mYonet = new MakaleYonet();
           
            return View(mYonet.Listele());

        }

        public PartialViewResult kategoriPartial()
        {
            KategoriYonet kYonet = new KategoriYonet();
            List<Kategori> liste = kYonet.Listele();
            return PartialView("_PartialPageKat2", liste);//model gönderdik 
        }
    }
}