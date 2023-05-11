using Makale_Entities;
using MakaleBLL;
using Proje_MakaleWeb_Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Proje_MakaleWeb_Mvc.Controllers
{
    public class YorumController : Controller
    {
        // GET: Yorum
      
        public ActionResult YorumGoster(int? id) // id ismi root config den geldi
        {
            if (id ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MakaleYonet MakYonet = new MakaleYonet();
            Makale makale = MakYonet.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
           return PartialView("_PartialPageYorumlar",makale.Yorumlar);// tıklayan makalenin id sine göre yorumları gönderiyoruz
        }
      

        public ActionResult YorumGuncelle(int? id, string text) 
        {
            if  (id ==null)
            {
                return new HttpStatusCodeResult (HttpStatusCode.BadRequest);
            }

            YorumYonet yorumYonet = new YorumYonet();
            Yorum yorum = yorumYonet.YorumBul(id.Value);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            yorum.Text = text;
            if (yorumYonet.YorumUpdate(yorum)>0)
            {
                return Json(new {hata=false},JsonRequestBehavior.AllowGet);
            }

            return Json(new {hata = true}, JsonRequestBehavior.AllowGet);

        }

        public ActionResult YorumSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            YorumYonet yorumYonet = new YorumYonet();
            Yorum yorum = yorumYonet.YorumBul(id.Value);
            if (yorum == null)
            {
                return HttpNotFound();
            }
           
            if (yorumYonet.yorumSil(yorum) > 0)
            {                      //sonuc.hata gibi buraya eklediklerimiz sonuc un içine gider....
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult YorumEkle(Yorum yorum, int? id )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            YorumYonet yorumYonet = new YorumYonet();
            MakaleYonet makaleyonet = new MakaleYonet();
            Makale makale = makaleyonet.MakaleBul(id.Value);
            if (yorum == null)
            {
                return HttpNotFound();
            }

            yorum.Makale = makale;
            yorum.Kullanici= SessionsUser.login;

            if (yorumYonet.YorumEkle(yorum) > 0)
            {                      //sonuc.hata gibi buraya eklediklerimiz sonuc un içine gider....
                return Json(new { hata = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { hata = true }, JsonRequestBehavior.AllowGet);

        }
    }
}