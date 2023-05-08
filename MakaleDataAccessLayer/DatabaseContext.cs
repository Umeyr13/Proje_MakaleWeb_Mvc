using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDataAccessLayer
{
    public class DatabaseContext:DbContext
    {
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Makale> Makaleler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Begeni> Begeniler { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new VeriTabaniOlusturucu()); //buradan veri  tabanı sınıfını tetikleyip orada da veri tabanını oluşmasını bekliyoruz.
        }

        //Katagori silmenin düer yolu

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        ////database oluşurken cascade özelliğini açıyoruz...
        //{ 
        //    //kategorinin makalelerini de sil ki kategoriyi silebilesin
        //    modelBuilder.Entity<Kategori>().HasMany(k => k.Makaleler).WithRequired(x => x.Kategori).WillCascadeOnDelete(true);

        //   // Eğer adam bu kategoriyi silmeye çalışırsa bilki  kategorinin makalelerinin yorumları var. Kategoriyi silmek için makaleleri ve yorumları silmek istiyeceksin evet yorumları silebilirsin.
        //    modelBuilder.Entity<Makale>().HasMany(m => m.Yorumlar).WithRequired(x =>x.Makale).WillCascadeOnDelete(true);
        //    // ve evet silmek istediğin kategorilerin makalelerinin begenilerini de silebilirsin
        //    modelBuilder.Entity<Makale>().HasMany(m => m.Begeniler).WithRequired(x => x.Makale).WillCascadeOnDelete(true);
        //}
    }
}
