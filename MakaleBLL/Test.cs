using Makale_Entities;
using MakaleDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL // Logic İş Katmanı
{
    public class Test
    {

        public Test()
        {
         DatabaseContext db = new DatabaseContext();   
            
            db.Kullanicilar.ToList();// Data base oluşturmayı tetikler ama "satır 18" deki daha iyi
            
            //db.Database.CreateIfNotExists();
        }

    }
}
