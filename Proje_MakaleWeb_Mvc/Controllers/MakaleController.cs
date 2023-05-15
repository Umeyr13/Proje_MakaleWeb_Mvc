﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Makale_Entities;
using MakaleBLL;
using Proje_MakaleWeb_Mvc.Models;

namespace Proje_MakaleWeb_Mvc.Controllers
{
    public class MakaleController : Controller
    {
       MakaleBLLSonuc<Makale> sonuc = new MakaleBLLSonuc<Makale> ();
        MakaleYonet MakYonet = new MakaleYonet ();
        KategoriYonet KatYonet = new KategoriYonet ();
        BegeniYonet By = new BegeniYonet();
        // GET: Makale
        public ActionResult Index()
        {
            //Kullanici kullanici = Session["login"] as Kullanici;
            return View(MakYonet.Listele().Where(x=>x.Kullanici.Id==SessionsUser.login.Id) );
        }

        // GET: Makale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = MakYonet.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: Makale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriListesi=new SelectList(/*KatYonet.Listele*/ CacheHelper.KategoriCache(),"Id","Baslik");
            return View();
        }

        // POST: Makale/Create
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            //Makale tablosunda kategori id si yok. biz id bilgisini taaa base class tan alıyoruz oraya kategori tablosu üzerinden gidiyor giderken sadece id yi aldığımız için zorunlu diğer alanlardan hata alıyoruz. almamak için modelstate den sildik
            if (ModelState.IsValid)
            {
                makale.Kullanici =SessionsUser.login;//Makaleyi yazan kişi
                makale.Kategori =KatYonet.KategoriBul(makale.Kategori.Id);
                ViewBag.KategoriListesi = new SelectList(/*KatYonet.Listele*/CacheHelper.KategoriCache(), "Id", "Baslik", makale.Kategori.Id);
                MakaleBLLSonuc<Makale> sonuc = MakYonet.MakaleEkle(makale);
                if (sonuc.hatalar.Count>0)
                {
                    sonuc.hatalar.ForEach(x=> ModelState.AddModelError("",x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            
            return View(makale);
        }

        // GET: Makale/Edit/5
        public ActionResult Edit(int? id)
        {
            Makale makale =MakYonet.MakaleBul(id.Value);
            ViewBag.KategoriListesi = new SelectList(/*KatYonet.Listele()*/ CacheHelper.KategoriCache() , "Id", "Baslik",makale.Kategori.Id);
     
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (makale == null)
            {
                return HttpNotFound();
            }
          
            return View(makale);
        }

        // POST: Makale/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Makale makale)
        {
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ViewBag.KategoriListesi = new SelectList(/*KatYonet.Listele()*/ CacheHelper.KategoriCache(), "Id", "Baslik");

            if (ModelState.IsValid)
            {
                makale.Kategori = KatYonet.KategoriBul(makale.Kategori.Id);
                MakaleBLLSonuc<Makale> sonuc = MakYonet.MakaleUpdate(makale);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                
                return RedirectToAction("Index");
            }

            return View(makale);
        }

        // GET: Makale/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = MakYonet.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: Makale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MakYonet.MakaleSil(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult MakaleGetir(int[] makaleidleri)// O an sayfada olan makalelerin Id leri 
        {
            List<int> Mliste = null;
          
            if (SessionsUser.login!= null && makaleidleri !=null) //ilk kadegori oluştuğunda makale id leri boş olduğu için makaleid leri boş değilse diye ifade ekledik
            {
                Mliste = By.Liste().Where(x => x.Kullanici.Id == SessionsUser.login.Id && makaleidleri.Contains(x.Makale.Id)).Select(x=>x.Makale.Id).ToList(); // burada kulllanıcı id si ve makale is si bu olanların makale id sini getirr. Yani login olan kullanıcının begendiği makaleler
                //Mliste = By.ListQuery().Include("Kullanıcı").Include("Makale").Where(x => x.Kullanici.Id == SessionsUser.login.Id).Select(x => x.Makale).Include(k => k.Kategori).ToList();
            }

            //makaleidleri = 1,3,5,7
            //select * from begeni where kullanıcıid = 5 ve makaleid in (1,2,3)
            //Liste = 1 ve 3 tür.
            return Json(new {liste = Mliste});
        }

        [HttpPost]
        public ActionResult MakaleBegen(int makaleid, bool begeni)//buradak değer ajax data daki isimler ile aynı olmalı
        {
            int sonuc =0;
           Begeni like = By.BegeniBul(makaleid,SessionsUser.login.Id);
            Makale makale = MakYonet.MakaleBul(makaleid);
            if (like!=null && begeni==false)
            {
               sonuc= By.BegeniSil(like);
            }
            else if (like ==null && begeni == true)
            {
              sonuc =  By.BegeniEkle(new Begeni { Kullanici = SessionsUser.login, Makale = makale });
                
            }
            if (sonuc>0)
            {   
                if (begeni) { makale.BegeniSayisi++; }
                else { makale.BegeniSayisi--; }
                return Json(new {hata = false, begenisayisi=makale.BegeniSayisi });
            }
            else
            {
                return Json(new {hata = true, begenisayisi = makale.BegeniSayisi }); 
            }
        }

        public ActionResult MakaleGoster(int? id) //Route config de "id" yazdığı için ismi "id" oldu
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = MakYonet.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialPageMakaleGoster", makale);;
        }

    }
}
