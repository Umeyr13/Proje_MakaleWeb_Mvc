using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MakaleDataAccessLayer
{
    //Database Yoksa oluştur
    public class VeriTabaniOlusturucu : CreateDatabaseIfNotExists<DatabaseContext>
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
                ,
                ProfilResimDosyaAdi= "user_1.jpg"

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
            context.SaveChanges();

            List<Kullanici> kullanicilar = context.Kullanicilar.ToList(); //Database den bütün kullanıcı verilerini alıyoruz. Aşağıda içinden kullanıcı alıp kullanıyoruz.

            for (int i = 0; i < 5; i++)
            {
                //Kategori Ekle

                Kategori kategori = new Kategori()
                {
                    Baslik = FakeData.PlaceData.GetStreetName()
                    ,Aciklama=FakeData.PlaceData.GetAddress()
                    ,KayitTarihi= DateTime.Now
                    ,DegistirmeTarihi = DateTime.Now
                    ,DegistirenKullanici = admin.KullaniciAdi
                    
                };
                context.Kategoriler.Add(kategori);

                for (int j = 0; j < 6; j++)
                {
                    Kullanici rasgelekullanici= kullanicilar[FakeData.NumberData.GetNumber(0, 5)];//rasgele bir tane kullanıcı aldık
                    

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
                        DegistirenKullanici = rasgelekullanici.KullaniciAdi //makaleyi değiştiren 

                        ,Kullanici = rasgelekullanici //makaleyi yazan

                    };
                    
                    //Kategoriye makale ekle
                    kategori.Makaleler.Add(makale);//(0013)Bu satırda hata alırdık."Nesne Örneği alınmamış hatası"
                    // almamak için Katagori.cs e bak


                    //Yorum Ekle

                    for (int z = 0; z < 3; z++)
                    {
                        Kullanici rasgelekullanici2 = kullanicilar[FakeData.NumberData.GetNumber(0, 5)]; //rasgele başka kullanıcı. yorum yapan kullanıcı seçmek için aldık
                        Yorum yorum = new Yorum()
                        {
                            Text = FakeData.TextData.GetSentence()
                            ,KayitTarihi = DateTime.Now.AddDays(-2)
                            ,DegistirmeTarihi = DateTime.Now
                            ,DegistirenKullanici=rasgelekullanici2.KullaniciAdi
                            , Kullanici = rasgelekullanici2
                        };

                        //yorum.Makale = makale;
                        //context.Yorumlar.Add(yorum);

                        //Yorumu Makaleye ekle
                        makale.Yorumlar.Add(yorum);
                    }


                    
                    //Begeni Ekleme - Kim beğendi

                    for (int x = 0; x < makale.BegeniSayisi; x++)
                    {
                        Kullanici rasgelekullanici3 = kullanicilar[FakeData.NumberData.GetNumber(0, 5)]; //rasgele başka kullanıcı. Beğenen kullanıcı seçmek için aldık
                        Begeni begen = new Begeni() 
                        {
                            Kullanici = rasgelekullanici3
                        };
                        makale.Begeniler.Add(begen);

                    }


                }


            }

            context.SaveChanges();




        }

    }
}
