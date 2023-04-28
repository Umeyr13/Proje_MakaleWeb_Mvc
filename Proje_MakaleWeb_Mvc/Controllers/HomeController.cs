using MakaleBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Makale_Entities;
using Makale_Entities.ViewModel;

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

        public ActionResult EnBegenilenler()// Index e  modeli değiştirip göndericez
        {
            return View("Index",mYonet.Listele().OrderByDescending(x =>x.BegeniSayisi).ToList());//büyükten küçüğe sıralamış olduk. Sınır vermedik top 15 veya top 5 gibi ek filtre eklenebilir
        }

        public ActionResult Sonyazilar()
        {
            return View("Index",mYonet.Listele().OrderByDescending(x => x.DegistirmeTarihi).ToList());
        }

        public ActionResult Hakkımızda() //sayfası olacak
        {
            return View();
        }

        public ActionResult Giris()
        {
            return View() ;
        }

        [HttpPost]
        public ActionResult Giris(LoginModel model)
        {
            return View();
        }

        public ActionResult Kayıt()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Kayıt(RegisterModel model)
        {
            //Kullanıcı adı ve email varmı kontrolü
            //kayıt işlemi varmı
            //aktivasyon mail i gönderilecek

            KullanıcıYonet ky = new KullanıcıYonet();

            if (ModelState.IsValid)
            {
                MakaleBLLSonuc<Kullanici> sonuc = ky.KullanıcıBul(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x=>ModelState.AddModelError("",x));
                    return View(model);
                }
                else
                {
                    //database e kaydet
                    return RedirectToAction("Giris");
                }
            }
            return View(model);

            //if (ModelState.IsValid)//tüm datalar uygun olarak geldi mi? taşma eksik data gibi şeyler var mı
            //{
            //    Kullanici kullanici = ky.KullanıcıBul(model); //RegisterModel tipinde veri gitti
            //    if (kullanici == null)
            //    {
            //        ModelState.AddModelError("","Bu kullanıcı adı yada email kayıtlı");
            //        return View(model);
            //    }
            //    else
            //    {
            //        //database e kaydet
            //        return RedirectToAction("Giris");
            //    }

            }
           

        }


}
