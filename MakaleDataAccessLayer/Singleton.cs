using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDataAccessLayer
{
    public class Singleton
    {
        public static DatabaseContext db;
        public Singleton()
        {
            if (db== null)
            {
                db = new DatabaseContext();
            }
        }
    }
}
