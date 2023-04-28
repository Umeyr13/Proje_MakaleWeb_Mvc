using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities.ViewModel
{
    public class RegisterModel
    {
        [DisplayName("Kullanıcı Adı"),StringLength(30), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string KullaniciAdi { get; set; }

        [DisplayName("Şifre"),StringLength(20), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Sifre { get; set; }

        [DisplayName("E-Posta"),StringLength(50),EmailAddress(ErrorMessage ="Girilen {0} adresi geçerli değil."), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string email { get; set; }

        [DisplayName("Şifre(Tekrar)"), StringLength(20),Compare(nameof(Sifre),ErrorMessage ="Şifreler Uyuşmuyor"), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Sifre2 { get; set; }

    }
}
