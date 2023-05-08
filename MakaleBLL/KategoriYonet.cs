using Makale_Entities;
using MakaleDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KategoriYonet
    {
        MakaleBLLSonuc<Kategori> sonuc = new MakaleBLLSonuc<Kategori> ();
        Repository<Kategori> rep_kat = new Repository<Kategori>();
        public List<Kategori> Listele()
        {
            return rep_kat.Liste();
        }

        public Kategori KategoriBul(int id) 
        {
                return rep_kat.Find(x=>x.Id==id);
        }

        public MakaleBLLSonuc<Kategori> KategoriEkle(Kategori model)
        {
            sonuc.nesne = rep_kat.Find(x=>x.Baslik == model.Baslik);
            if (sonuc.nesne != null)
            {
                sonuc.hatalar.Add("Kategori zaten mevcut");
             
            }
            else
            {
                if (rep_kat.Insert(model)<1)
                {
                    sonuc.hatalar.Add("Kategori Kaydedilemedi");
                }  
                
              
            }

            return sonuc;
            
        }

        public MakaleBLLSonuc<Kategori> KategoriUpdate(Kategori model)
        {
            sonuc.nesne =rep_kat.Find(x=> x.Id == model.Id); //güncellenecek kaegori bilgisi
            
            Kategori kategori = rep_kat.Find(x=>x.Id != model.Id && x.Baslik == model.Baslik); // Farklı id de aynı başlık varmı? Yoksa değiştirsin 

            if (sonuc.nesne !=null && kategori == null) //db de kayıtlı ve aynı başlıkta başka yok ise değiştir.
            {
                sonuc.nesne.Baslik = model.Baslik;
                sonuc.nesne.Aciklama = model.Aciklama;
                if(rep_kat.Update(sonuc.nesne) <1)
                {
                    sonuc.hatalar.Add("Güncelleme başarısız oldu!");
                }

            }
            else
            {
                if (kategori !=null)
                {
                    sonuc.hatalar.Add("Aynı başlıkta kategori mevcut!");
                }
                else
                {
                    sonuc.hatalar.Add("Kategori Bulunamadı!");

                }
            }

            return sonuc;
        }

        public MakaleBLLSonuc<Kategori> KategoriSil(int id)
        {
            Kategori kategori = rep_kat.Find(x => x.Id == id);
            // sonuc.nesne = rep_kat.Find(x => x.Id ==id);
            // rep_kat.Delete(sonuc.nesne); // Normalde silme kodu bu ama silmez eğer Katagorinin bağlı olduğu makale vs. varsa silmeye sql db izin vermez. Brkaç farklı yoluvar

            //  1-    Makalelerini silebiliriz bunu silmek için
            Repository<Makale> rep_makale = new Repository<Makale>();
            
            foreach (Makale item in kategori.Makaleler.ToList())
            {
                //burada da yine aynı mantık makalenin yorum ve beğenisi olduğu için silmeye izin vermez.
                //rep_makale.Delete(item);

                //Makalenin yorumlarını sil
                Repository<Yorum> rep_yorum = new Repository<Yorum>();  
                foreach (Yorum yorum in item.Yorumlar.ToList())
                {
                    rep_yorum.Delete(yorum);
                }

                //Makalenin beğenilerinin sil

                Repository<Begeni> rep_begeni = new Repository<Begeni>();

                foreach (Begeni begeni in item.Begeniler.ToList())
                {
                    rep_begeni.Delete(begeni);
                }

                rep_makale.Delete(item);

            }
                rep_kat.Delete(kategori);
            return sonuc;
           
        }
    }
}
