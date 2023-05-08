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

namespace Proje_MakaleWeb_Mvc.Controllers
{
    public class KullaniciController : Controller
    {
        KullanıcıYonet KulY = new KullanıcıYonet();
        MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
        

        // GET: Kullanici
        public ActionResult Index()
        {
            return View(KulY.KUllaniciListesi());
        }

        // GET: Kullanici/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc  = KulY.KullanıcıBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // GET: Kullanici/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kullanici/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                KulY.KullanıcıKaydet(kullanici);              
                return RedirectToAction("Index");
            }

            return View(kullanici);
        }

        // GET: Kullanici/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sonuc = KulY.KullanıcıBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // POST: Kullanici/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                 KulY.KullaniciUpdate(kullanici);
                return RedirectToAction("Index");
                //foreach ile validation a hatalar eklenmeli.
            }
            return View(kullanici);
        }

        // GET: Kullanici/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                sonuc = KulY.KullanıcıBul(id.Value);
            if (sonuc.nesne == null)
            {
                return HttpNotFound();
            }
            return View(sonuc.nesne);
        }

        // POST: Kullanici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sonuc  = KulY.KullaniciSil(id);
       
            return RedirectToAction("Index");
        }

    }
}
