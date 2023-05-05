using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MakaleCommon;

namespace MakaleDataAccessLayer
{
    public class Repository<T> : Singleton, IRepository<T> where T : class
    {

        private DbSet<T> _dbSet;
        public Repository()
        {
            _dbSet = db.Set<T>(); 
        }
        public int Delete(T nesne)
        {
            _dbSet.Remove( nesne);
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> kosul)
        {
            return _dbSet.FirstOrDefault(kosul);
        }

        public int Insert(T nesne)
        {
            _dbSet.Add(nesne);
            if (nesne is BaseClass)//begeni sınıfı base den kalıtılmadı bu if blogu olmasa patlar çünkü begenide bu field lar yok
            {
                BaseClass obj = nesne as BaseClass;//Baseclass tipinde çünkü gelen kullanıcı mı makale mi belli değil ama hepsinde baseclass var
                DateTime tarih = DateTime.Now; //tarih hepsinde ikisinde olsun diye
                obj.KayitTarihi = tarih;
                obj.DegistirmeTarihi = tarih;
                obj.DegistirenKullanici = Uygulama.login;


            }
            return db.SaveChanges();
        }

        public List<T> Liste()
        {
            return _dbSet.ToList();
        }

        public List<T> Liste(Expression<Func<T, bool>> kosul)
        {
            return _dbSet.Where(kosul).ToList();
        }

        public int Update(T nesne)
        {
            if (nesne is BaseClass)
            {
                BaseClass obj = nesne as BaseClass;              
                obj.DegistirmeTarihi = DateTime.Now;
                obj.DegistirenKullanici = Uygulama.login;

            }
            return db.SaveChanges();
        }
    }
}
