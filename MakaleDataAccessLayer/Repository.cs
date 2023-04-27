using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            return db.SaveChanges(); // neden direk save ettik
        }
    }
}
