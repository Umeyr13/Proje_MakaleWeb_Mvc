using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    [Table("Begeni")]
    public class Begeni // base class dan miras almıyoruz. beğeni var yada yoktur. tarihi önemli değil
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Kullanici Kullanici { get; set; }
        public virtual Makale Makale { get; set; }
    }
}
