using Makale_Entities;
using MakaleDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class BegeniYonet
    {
        Repository<Begeni> rep_Begen = new Repository<Begeni>();
        public IQueryable<Begeni> ListQuery()
        {
            return rep_Begen.ListQueryable();
        }
        public List<Begeni> Liste()
        {
            List<Begeni> liste = rep_Begen.Liste();
            return liste;
        }
        public Begeni BegeniBul(int Makid, int KulId)
        {
            return rep_Begen.Find(x =>x.Makale.Id == Makid && x.Kullanici.Id == KulId);

        }
        public int BegeniEkle(Begeni begen)
        {
            return rep_Begen.Insert(begen);
        }

        public int BegeniSil(Begeni begen)
        {
            return rep_Begen.Delete(begen);

        }
    }
}
