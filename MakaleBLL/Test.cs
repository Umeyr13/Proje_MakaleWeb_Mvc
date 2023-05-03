using Makale_Entities;
using MakaleDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Makale_Entities;

namespace MakaleBLL // busines Logic İş Katmanı
{
    public class Test
    {
      Repository<Kullanici> rep_kul = new Repository<Kullanici>();

        public Test()
        {
          DatabaseContext db = new DatabaseContext();   
          db.Kullanicilar.ToList(); // Data base oluşturmayı tetikler ama "18" deki daha iyi
          // db.Database.CreateIfNotExists();
          List<Kullanici> kullanicilar = rep_kul.Liste();
            List<Kullanici> adminler = rep_kul.Liste(x => x.Admin == true);
          
        }
        public void EkleTest()
        {
            rep_kul.Insert(new Kullanici() { Ad = "test", Soyad = "test", Admin = false, aktif = true, AktifGuid = Guid.NewGuid(), Email = "test", KayitTarihi = DateTime.Now, DegistirmeTarihi = DateTime.Now, DegistirenKullanici = "elif", KullaniciAdi = "test", Sifre = "test" });
        }

        public void UpdateTest() 
        {  
            Kullanici kullanici = rep_kul.Find(x => x.KullaniciAdi == "test");
            if (kullanici != null)
            {
                kullanici.Ad = "BuYeniİsim";
                kullanici.Soyad= "YeniSoyad";
                rep_kul.Update(kullanici);
            }
        }

        public void DeleteTest()
        {
            Kullanici kullanici = rep_kul.Find(x => x.KullaniciAdi == "test");
            if (kullanici!=null)
            {
                rep_kul.Delete(kullanici);
            }
        }

        public void YorumTest()
        {
            Repository<Makale> rep_mak = new Repository<Makale>();
            Repository<Yorum> rep_yorum = new Repository<Yorum>();

            Makale m = rep_mak.Find(x => x.Id == 1);//Makaleyi bulduk
            Kullanici k = rep_kul.Find(x => x.Id ==1);// yorumu yapacak kullanıcı
            Yorum yorum = new Yorum() 
            {
                Text = "Deneme yorumu",
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now,
                DegistirenKullanici = "elif",
                Kullanici = k,//yorumu yapan kullanıcı
                Makale = m // makale bilgisi
            };

            rep_yorum.Insert(yorum);
        }

    }
}
