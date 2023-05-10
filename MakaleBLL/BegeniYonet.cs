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
    }
}
