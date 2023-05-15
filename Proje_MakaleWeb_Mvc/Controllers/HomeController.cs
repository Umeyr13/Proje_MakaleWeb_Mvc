using MakaleBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Makale_Entities;
using Makale_Entities.ViewModel;
using MakaleCommon;
using Proje_MakaleWeb_Mvc.Models;
using System.Data.Entity;
using System.Reflection;

namespace Proje_MakaleWeb_Mvc.Controllers
{
    public class HomeController : Controller
    {
        MakaleYonet mYonet = new MakaleYonet();
        KategoriYonet KatYonet = new KategoriYonet();
        KullanıcıYonet KulYonet = new KullanıcıYonet();
        KullanıcıYonet ky = new KullanıcıYonet();
        BegeniYonet BegeniYonet = new BegeniYonet();
        public ActionResult Index()
        {
            Test test = new Test();
            // Test class ında metot yazmadık. Ctor u na yazdık kodları. Burada new ile örnekleyince zaten oradaki ctor çalışıcak.
            
            // test.EkleTest();
            //test.UpdateTest();
            // test.DeleteTest();
            // test.YorumTest();

            return View(mYonet.Listele().Where(x => x.Taslak == false).ToList());

        }

        public PartialViewResult kategoriPartial()//Örnek olarak oluşturuldu kullanmıyoruz. Model ile de olduğunu gördük
        {
          
            List<Kategori> liste = KatYonet.Listele();
            return PartialView("_PartialPageKat2", liste);//model gönderdik 
        }

        public ActionResult SecilenKategori(int? id)//bu aydi rastgele yazılmadı RouteConfig.cs de "id" url i olduğu için
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            Kategori secKat = KatYonet.KategoriBul(id.Value);//null olmasına izin verdiğimiz için direk "id" yi değilde id.value yazmamızı istedi
            return View("Index",secKat.Makaleler.Where(x=>x.Taslak==false).ToList());
        }

        public ActionResult EnBegenilenler()// Index e  modeli değiştirip göndericez
        {
            return View("Index",mYonet.Listele().Where(x=>x.Taslak==false).OrderByDescending(x =>x.BegeniSayisi).ToList());//büyükten küçüğe sıralamış olduk. Sınır vermedik top 15 veya top 5 gibi ek filtre eklenebilir
        }

        public ActionResult Sonyazilar()
        {
            return View("Index",mYonet.Listele().Where(x => x.Taslak == false).OrderByDescending(x => x.DegistirmeTarihi).ToList());
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
            if (ModelState.IsValid) // Girilen bilgiler tamam ise.
            {
                MakaleBLLSonuc<Kullanici> sonuc = KulYonet.LoginKontrol(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("",x));// her bir x hatasını sonuc.hatalara ekle
                    return View(model);
                }

              //  BegeniYonet.ListQuery<Kullanici>().Where(x => x.)
                SessionsUser.login = sonuc.nesne;//bulduğu kullanıcıyı kayıt altına almış olduk

                Uygulama.login = sonuc.nesne.KullaniciAdi;
                SessionsUser.begenilenler = BegeniYonet.ListQuery().Include("Kullanici").Include("Makale").Where(x => x.Kullanici.Id == SessionsUser.login.Id).Select(x => x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(x => x.DegistirmeTarihi).ToList(); //deneme kod
                //  BegeniYonet.ListQuery<Kullanici>().Where(x => x.)
               // ViewBag.begeni = view bag ile begendi bilgisini göndermeye çalışıcam ************
                return RedirectToAction("Index");
            }
            return View(model);
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


            if (ModelState.IsValid)
            {
                MakaleBLLSonuc<Kullanici> sonuc = ky.KullanıcıKaydet(model);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x=>ModelState.AddModelError("",x));
                    return View(model);
                }
                else
                {
                    //database e kaydet
                    return RedirectToAction("KayıtBasarili");
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

        public ActionResult Cikis()
        {
           // Session["login"] = null;
           Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult KayıtBasarili ()
        {
            return View();
        }

        public ActionResult HesapAktiflestir(Guid id)//Appstart/RouteConfig de yazan url: "{controller}/{action}/{id}",//burada id yazdığı için id aldık home controller da
        {
            MakaleBLLSonuc<Kullanici> sonuc = KulYonet.ActivateUser(id);
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View();
        }

        public ActionResult Error()
        {
            List<string> errors = new List<string>();

            if (TempData["hatalar"] != null)
            {
                ViewBag.hatalar = TempData["hatalar"]; 

            }
            else
            {
                ViewBag.hatalar = errors;
            }
            return View();
        }

        public ActionResult ProfilGoster()
        {
            //Kullanici kullanici = Session["Login"] as Kullanici;
           MakaleBLLSonuc<Kullanici> sonuc = KulYonet.KullanıcıBul(SessionsUser.login.Id);
            if (sonuc.hatalar.Count>0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");

            }
            return View(sonuc.nesne);
        }

        public ActionResult ProfilDegistir()
        {
           //Kullanici nesne = Session["Login"] as Kullanici;
            MakaleBLLSonuc<Kullanici> sonuc = KulYonet.KullanıcıBul(SessionsUser.login.Id);
            if (sonuc.hatalar.Count >0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View(sonuc.nesne);
        }

        [HttpPost]
        public ActionResult ProfilDegistir(Kullanici kullanici, HttpPostedFileBase profilresmidegeldi)
        {
            ModelState.Remove("DegistirenKullanici"); // aşağıda hata vermesin diye bunu model state den sildik. Artık bu değer boş iken de model isValid oldu
            if (ModelState.IsValid)//Değiştiren kullanıcıyı burada vermediğimiz için kesin hata verir idi.(Değiştiren kullanıcı boş geçilemez hatası)
            {

                if (profilresmidegeldi != null && (profilresmidegeldi.ContentType=="image/jpg" || profilresmidegeldi.ContentType =="image/png" || profilresmidegeldi.ContentType=="image/jpeg" ) )
                {
                    string dosya = $"user_{kullanici.Id}.{profilresmidegeldi.ContentType.Split('/')[1]}";// "/" işaretine göre parçala 1. indextekini getir dedik.

                    profilresmidegeldi.SaveAs ( Server.MapPath($"~/resim/{dosya}") ); // girilen resmi uygulama klasörüne kaydettik
                    kullanici.ProfilResimDosyaAdi = dosya;//resmin ismini o anki kullanıcıya tanımladık
                }

                Uygulama.login = kullanici.KullaniciAdi;//yeni gelen kullanıcı adını değiştiren kullanıcı adı olarak atadık
                MakaleBLLSonuc<Kullanici> sonuc =  KulYonet.KullaniciUpdate(kullanici);

                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("",x));
                    return View(kullanici);
                }

                //profldeğişti ise 
                SessionsUser.login=sonuc.nesne; //güncel kullanıcı bilgisi
                //Uygulama.login = sonuc.nesne.KullaniciAdi; Bu burada olursa değiştiren kullanıcı ismi eski kullanıcı ismi olarak ayarlanır. Yukarıda olması daha mantıklı geldi.
                return RedirectToAction("ProfilGoster");
            }
            else
            {
                return View(kullanici);

            }

        }

        public ActionResult ProfilSil()
        {
            //Kullanici kullanici = Session["Login"] as Kullanici;

           MakaleBLLSonuc<Kullanici> sonuc = KulYonet.KullaniciSil(SessionsUser.login.Id);
            if (sonuc.hatalar.Count>1)
            {
                //profil sil in kendi View i olamdığı için error sayfasına yönlendiriyoruz
                TempData["hatalar"] =sonuc.hatalar;
                return RedirectToAction("Error");
            }
            Session.Clear();//Bellekten de sildik kullanıcıyı
            return RedirectToAction("Index");
        }

        public ActionResult Begendiklerim()
        {
            //var query = BegeniYonet.ListQuery().Include("Kullanici").Include("Makale").Where(x =>x.Kullanici.Id == SessionsUser.login.Id).Select(x => x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(x=>x.DegistirmeTarihi);//bu sordudan makaleyi almak istiyorum bunlara bir de kategori ve makaleleri de ekle
           // Inner Join yapmanın başka bir yolu Include
            //SessionsUser.begenilenler = query.ToList();// devam edicem yukarıdaki kod session user set e taşınıcak gibi
            var begenilenler = SessionsUser.begenilenler;
            return View("Index",begenilenler.ToList());
            
        }
    }

    


}
