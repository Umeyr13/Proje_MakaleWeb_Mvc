using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities.ViewModel
{
    public class LoginModel //View için bir giriş modeli. Diğer kullanıcı modelini kullanırsak orada zorunlu alanları da isterdi biz sadece kullanıcı adı ve şifreyi istiyoruz
    {
        [DisplayName("Kullanıcı Adı"),StringLength(30),Required(ErrorMessage ="{0} alanı boş geçilemez")]
        public string KullaniciAdi {get; set;}

        [DisplayName("Şifre"),Required(ErrorMessage ="{0} alanı boş geçilemez")]
        public string Sifre { get; set; }

    }
}
