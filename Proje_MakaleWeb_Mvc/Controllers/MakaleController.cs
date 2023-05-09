using System;
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
            ViewBag.KategoriListesi=new SelectList(KatYonet.Listele(),"Id","Baslik");
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
                ViewBag.KategoriListesi = new SelectList(KatYonet.Listele(), "Id", "Baslik", makale.Kategori.Id);
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
            ViewBag.KategoriListesi = new SelectList(KatYonet.Listele(), "Id", "Baslik",makale.Kategori.Id);
     
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
            ViewBag.KategoriListesi = new SelectList(KatYonet.Listele(), "Id", "Baslik");

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
   
    }
}
