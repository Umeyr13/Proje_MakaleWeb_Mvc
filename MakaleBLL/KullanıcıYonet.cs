using Makale_Entities;
using Makale_Entities.ViewModel;
using MakaleDataAccessLayer;
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
        public MakaleBLLSonuc<Kullanici> KullanıcıBul(RegisterModel model)
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

            return sonuc;
        }


    }
}
