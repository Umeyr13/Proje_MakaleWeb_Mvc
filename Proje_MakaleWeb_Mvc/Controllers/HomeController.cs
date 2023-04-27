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
        MakaleYonet mYonet = new MakaleYonet();
        KategoriYonet kYonet = new KategoriYonet();
        public ActionResult Index()
        {
            Test test = new Test();
            // Test class ında metot yazmadık. Ctor u na yazdık kodları. Burada new ile örnekleyince zaten oradaki ctor çalışıcak.
            
            // test.EkleTest();
            //test.UpdateTest();
            // test.DeleteTest();
            // test.YorumTest();
            
           
            return View(mYonet.Listele());

        }

        public PartialViewResult kategoriPartial()//Örnek olarak oluşturuldu kullanmıyoruz. Model ile de olduğunu gördük
        {
            KategoriYonet kYonet = new KategoriYonet();
            List<Kategori> liste = kYonet.Listele();
            return PartialView("_PartialPageKat2", liste);//model gönderdik 
        }

        public ActionResult SecilenKategori(int? id)//bu aydi rastgele yazılmadı RouteConfig.cs de "id" url i olduğu için
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Kategori secKat = kYonet.KategoriBul(id.Value);//null olmasına izin verdiğimiz için direk "id" yi değilde id.value yazmamızı istedi
            return View("Index",secKat.Makaleler);
        }

        public ActionResult EnBegenilenler()
        {
            return View();
        }

        public ActionResult Sonyazilar()
        {
            return View();
        }

        public ActionResult Hakkımızda() //sayfası olacak
        {
            return View();
        }

    }
}