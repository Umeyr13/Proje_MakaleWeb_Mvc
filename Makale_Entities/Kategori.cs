using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;



namespace Makale_Entities
{

    [Table("Kategori")]
    public class Kategori:BaseClass
    {
        [Required,StringLength(50),DisplayName("Kategori")]      
        public string Baslik { get; set; }

        [StringLength(150),DisplayName("Açıklama")]
        public string Aciklama { get; set; }

        public virtual List<Makale> Makaleler { get; set; }//0013

        public Kategori() //0013 de hata alamamak için Ctor da Makalelerin bir nesne örneğini aldık.
        {
            Makaleler = new List<Makale>();
        }
    }
}
