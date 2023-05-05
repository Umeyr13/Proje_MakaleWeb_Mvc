using Makale_Entities;
using Makale_Entities.ViewModel;
using MakaleCommon;
using MakaleDataAccessLayer;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class KullanıcıYonet
    {
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();

        public MakaleBLLSonuc<Kullanici> ActivateUser(Guid id)
        {
            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
           sonuc.nesne =  rep_kul.Find(x => x.AktifGuid == id);
            if (sonuc.nesne !=null)
            {
                if (sonuc.nesne.aktif)
                {
                    sonuc.hatalar.Add("Kullanıcı daha önce aktifleştirilmiştir");
                }
                else
                {
                    sonuc.nesne.aktif=true;
                    rep_kul.Update(sonuc.nesne);
                }
            }
            else
            {
                sonuc.hatalar.Add("Aktifleştirilecek kullanıcı bulunamadı");
            }
            return sonuc;
        }

            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
        public MakaleBLLSonuc<Kullanici> KullaniciUpdate(Kullanici model)
        {
            #region Hatalı Kod Çözüldü
            /* Buradaki */
            //Kullanici temp = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email == model.Email);
            //if (temp != null && temp.Id != model.Id)
            //{
            //    if (temp.Email == model.Email)
            //    {
            //        sonuc.hatalar.Add("Bu Mail adresi daha önce kaydedilmiş..");
            //    }
            //    if (temp.KullaniciAdi == model.KullaniciAdi)
            //    {
            //        sonuc.hatalar.Add("Kullanıcı adı daha önce kaydedilmiş..");
            //    }

            //}
            #endregion

            sonuc = KullanıcıKontrol(model);
            if (sonuc.hatalar.Count>0)
            {
                sonuc.nesne = model;
                return sonuc;
            }
            else
            {
                sonuc.nesne = rep_kul.Find(x => x.Id == model.Id); // x i bul data base den. x ne al x de bu == model.id
                sonuc.nesne.Ad = model.Ad;
                sonuc.nesne.Soyad = model.Soyad;
                sonuc.nesne.Email = model.Email;
                sonuc.nesne.KullaniciAdi = model.KullaniciAdi;
                sonuc.nesne.Sifre = model.Sifre;
                sonuc.nesne.ProfilResimDosyaAdi = model.ProfilResimDosyaAdi;
                // sonuc.nesne.DegistirmeTarihi Repository class ında bu veriler zaten güncellenecek...
                if (rep_kul.Update(sonuc.nesne) < 1) // db.SaveChanges gittiği yerde var.
                {
                    sonuc.hatalar.Add("Profil bilgileri güncellenemedi");
                }
            }

            return sonuc;
        }

        private MakaleBLLSonuc<Kullanici> KullanıcıKontrol(Kullanici model)
        {
          

                Kullanici k1 = rep_kul.Find(x=>x.Email == model.Email);
                Kullanici k2 = rep_kul.Find(x=>x.KullaniciAdi == model.KullaniciAdi);

            if (k1 != null && k1.Id != model.Id)
            {             
                    sonuc.hatalar.Add("Bu Mail adresi daha önce kaydedilmiş..");             
            }

            if (k2 != null && k2.Id != model.Id)
            {
                    sonuc.hatalar.Add("Kullanıcı adı daha önce kaydedilmiş..");
            }

            sonuc.nesne = model;
            return sonuc;
        }

        public MakaleBLLSonuc<Kullanici> KullanıcıBul(int id)
        {
            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
            sonuc.nesne = rep_kul.Find(x=>x.Id== id);

            if (sonuc.nesne==null)
            {
                sonuc.hatalar.Add("Kullanıcı bulunamadı");
            }

            return sonuc;
        }

        public MakaleBLLSonuc<Kullanici> KullanıcıKaydet(RegisterModel model)
        {
            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
            Kullanici k =rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email==model.email);
            sonuc.nesne = k;
            if (sonuc.nesne!=null )
            {
                if (sonuc.nesne.KullaniciAdi== model.KullaniciAdi)
                {
                    sonuc.hatalar.Add("Bu kullanıcı adı sisteme kayıtlı");
                }
                else if (sonuc.nesne.Email == model.email)
                {
                    sonuc.hatalar.Add("Bu Email sistemde kayıtlı");

                }
            }
            else // eğer database de aynı kullanıcı yoksa buraya gelir.
            {
               int islemsonuc = rep_kul.Insert(new Kullanici()
                {
                    KullaniciAdi = model.KullaniciAdi
                   ,Email = model.email
                   ,Sifre = model.Sifre
                   ,AktifGuid = Guid.NewGuid()
                   //Bunları Repository/Insert metot u na taşıyabiliriz...
                   //,KayitTarihi = DateTime.Now
                   //,DegistirmeTarihi = DateTime.Now
                   //,DegistirenKullanici ="system" 
                });
                //burada iken aslında sonuc nesnesine birşey atılmamış oldu.

                if (islemsonuc >0) //Insert de hata yoksa mail göner
                {
                    sonuc.nesne = rep_kul.Find(x =>x.KullaniciAdi == model.KullaniciAdi && x.Email==model.email );
                    //Eğer İşlem gerçekleşirse sonuç nesnesi boşkalmasın diye kullanıcıyı içine attık.

                    //Aktivasyon mail i atabiliriz.
                    
                   string siteURL = ConfigHelper.Gettir<string>("SiteRootUri");

                    string aktiveUrl = $"{siteURL}/Home/HesapAktiflestir/{sonuc.nesne.AktifGuid}";
                    string body = $"Merhaba {sonuc.nesne.Ad} {sonuc.nesne.Soyad} <br /> Hesabınızı aktifleştirmek için " +
                        $"<a href='{aktiveUrl}' target='_blank' > Tıklayınız.. </a>";

                    MailHelper.SendMail(body,sonuc.nesne.Email,"Hesap Aktifleştirme");
                }
            
            }

            return sonuc;//else den gelirse boş nesne gelirdi.
        }

        public MakaleBLLSonuc<Kullanici> LoginKontrol(LoginModel model)
        {
            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
            sonuc.nesne = rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi && x.Sifre==model.Sifre);

            if (sonuc.nesne ==null)
            {
                sonuc.hatalar.Add("Hatalı kullanıcı adı veya şifre");
                
            }
            else
            {
                if (!sonuc.nesne.aktif)
                {
                    sonuc.hatalar.Add("Aktifleştirilmedi ");
                }
            }
 
            return sonuc;//else den gelirse boş nesne gelirdi.
        }




    }
}
