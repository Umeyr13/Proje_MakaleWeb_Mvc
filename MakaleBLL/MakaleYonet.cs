﻿using Makale_Entities;
using MakaleDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class MakaleYonet
    {
        Repository<Makale> rep_makale = new Repository<Makale>();
        MakaleBLLSonuc<Makale> sonuc = new MakaleBLLSonuc<Makale>();
        public List<Makale> Listele()
        {
            return rep_makale.Liste();
        }

        public Makale MakaleBul(int id)
        {
            return rep_makale.Find(x=>x.Id == id);
        }

        public MakaleBLLSonuc<Makale> MakaleEkle(Makale makale)
        {
            sonuc.nesne = rep_makale.Find(x=> x.Baslik == makale.Baslik && x.Kategori.Id == makale.Kategori.Id);
            if (sonuc.nesne != null)
            {
                sonuc.hatalar.Add("Bu makale Kayıtlı");
            }
            else
            {
                if (rep_makale.Insert(makale)<1)
                {
                    sonuc.hatalar.Add("Makale Eklenemedi");
                }
            }
            return sonuc;
        }

        public MakaleBLLSonuc<Makale> MakaleSil(int id)
        {
            sonuc.nesne = rep_makale.Find(x => x.Id == id);
            if (sonuc.nesne != null)
            {
                if (rep_makale.Delete(sonuc.nesne)<1) // makalenin yorumu varsa silmez kategorideki gibi yapılabilir. şimdilik böyle kalsın
                {
                    sonuc.hatalar.Add("Makale Silinemedi");
                }
            }
            else
            {
                sonuc.hatalar.Add("Makale Bulunamadı");
            }
            return sonuc;
        }

        public MakaleBLLSonuc<Makale> MakaleUpdate(Makale makale)
        {
            Makale nesne = rep_makale.Find( x=> x.Baslik == makale.Baslik && x.Kategori.Id == makale.Kategori.Id && x.Id != makale.Id);

            if( nesne != null)
            {
                sonuc.hatalar.Add("Bu makale Kayıtlı");
            }
            else
            {
                sonuc.nesne = rep_makale.Find(x => x.Id == makale.Id);
                sonuc.nesne.Kategori = makale.Kategori;
                sonuc.nesne.Baslik = makale.Baslik;
                sonuc.nesne.Icerik = makale.Icerik;
                sonuc.nesne.Taslak = makale.Taslak;
                if(rep_makale.Update(sonuc.nesne)<1)
                {
                    sonuc.hatalar.Add("Makale Güncellenemedi");
                }
            }
            return sonuc;
        }
    }
}
