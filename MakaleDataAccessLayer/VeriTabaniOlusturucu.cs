using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MakaleDataAccessLayer
{
    //Database Yoksa oluştur
    internal class VeriTabaniOlusturucu : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            // 1 Admin kullanıcı
            Kullanici admin = new Kullanici()
            {
                Ad = "Umeyr"
                ,
                Soyad = "Gülçimen"
                ,
                Email = "umeyr@gmail"
                ,
                Admin = true
                ,
                aktif = true
                ,
                KullaniciAdi = "admin"
                ,
                Sifre = "123"
                ,
                AktifGuid = Guid.NewGuid()
                ,
                KayitTarihi = DateTime.Now
                ,
                DegistirmeTarihi = DateTime.Now.AddMinutes(5)
                ,
                DegistirenKullanici = "Umeyr"

            };

            context.Kullanicilar.Add(admin);

            //5 adet yeni kullanıcı
            for (int i = 1; i < 6; i++)
            {
                Kullanici k = new Kullanici()

                {
                    Ad = FakeData.NameData.GetFirstName()
                    ,Soyad = FakeData.NameData.GetSurname()
                    ,Email = FakeData.NetworkData.GetEmail()
                    ,Admin = false                   
                    ,aktif = true
                    ,KullaniciAdi = $"user{i}" // "user" + i demenin başka yolu
                    ,Sifre = "123"
                    ,AktifGuid = Guid.NewGuid()
                    ,KayitTarihi = DateTime.Now.AddDays(-3)
                    ,DegistirmeTarihi = DateTime.Now.AddMinutes(5)
                    ,DegistirenKullanici = $"user{i}"

                };

                context.Kullanicilar.Add(k);

            }

            

            for (int i = 0; i < 5; i++)
            {
                //Kategori Ekle

                Kategori kategori = new Kategori()
                {
                    Baslik = FakeData.PlaceData.GetStreetName()
                    ,Aciklama=FakeData.PlaceData.GetAddress()
                    ,KayitTarihi= DateTime.Now
                    ,DegistirmeTarihi = DateTime.Now
                    ,DegistirenKullanici = "Umeyr"
                    
                };
                context.Kategoriler.Add(kategori);

                for (int j = 0; j < 6; j++)
                {
                    // Makale Ekle

                    Makale makale = new Makale()
                    {
                        Baslik = FakeData.TextData.GetAlphabetical(5)
                        ,
                        Icerik = FakeData.TextData.GetSentences(2)
                        ,
                        Taslak = false
                        ,
                        BegeniSayisi = FakeData.NumberData.GetNumber(1, 6)
                        ,
                        KayitTarihi = DateTime.Now.AddDays(-1)
                        ,
                        DegistirmeTarihi = DateTime.Now
                        ,
                        DegistirenKullanici = admin.KullaniciAdi

                    };
                    
                    //Kategoriye makale ekle
                    kategori.Makaleler.Add(makale);//(0013)Bu satırda hata alırdık."Nesne Örneği alınmamış hatası"
                    // almamak için Katagori.cs e bak


                    //Yorum Ekle

                    for (int z = 0; z < 3; z++)
                    {
                        Yorum yorum = new Yorum()
                        {
                            Text = FakeData.TextData.GetSentence()
                            ,KayitTarihi = DateTime.Now.AddDays(-2)
                            ,DegistirmeTarihi = DateTime.Now
                            ,DegistirenKullanici=admin.KullaniciAdi
                        };

                        //yorum.Makale = makale;
                        //context.Yorumlar.Add(yorum);

                        //Yorumu Makaleye ekle
                        makale.Yorumlar.Add(yorum);
                    }


                    List<Kullanici> kullanicilar = context.Kullanicilar.ToList();
                    //Begeni Ekleme - Kim beğendi

                    for (int x = 0; x < makale.BegeniSayisi; x++)
                    {

                        Begeni begen = new Begeni() 
                        {
                            Kullanici = kullanicilar[x]
                        };
                        makale.Begeniler.Add(begen);

                    }


                }


            }

            context.SaveChanges();




        }

    }
}
