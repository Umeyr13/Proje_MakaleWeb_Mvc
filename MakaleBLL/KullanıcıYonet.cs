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

        public MakaleBLLSonuc<Kullanici> KullanıcıKaydet(RegisterModel model)
        {
            MakaleBLLSonuc<Kullanici> sonuc = new MakaleBLLSonuc<Kullanici>();
            Kullanici k =rep_kul.Find(x => x.KullaniciAdi == model.KullaniciAdi || x.Email==model.email);
            sonuc.nesne = k;
            if (sonuc.nesne!=null)
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
                    sonuc.nesne = rep_kul.Find(x =>x.KullaniciAdi == model.KullaniciAdi || x.Email==model.email );
                    //Eğer İşlem gerçekleşirse sonuç nesnesi boşkalmasın diye kullanıcıyı içine attık.

                    //Aktivasyon mail i atabiliriz.
                    
                   string siteURL = ConfigHelper.Get<string>("SiteRootUri");

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
            return sonuc;

        }


    }
}
