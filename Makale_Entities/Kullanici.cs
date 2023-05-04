﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    [Table("Kullanici")]
    public class Kullanici:BaseClass
    {
        [StringLength(30), DisplayName("Ad: ")]
        public string Ad { get; set; }

        [StringLength(30), DisplayName("Soyad: ")]
        public string Soyad { get; set; }

        [Required,StringLength(30),DisplayName("Kullanıcı Adı: ")]
        public string KullaniciAdi { get; set; }

        [Required, StringLength(50),DisplayName("E-Posta: ")]
        public string Email { get; set; }

        [Required, StringLength(20),DisplayName("Şifre: ")]
        public string Sifre { get; set; }

        [StringLength(30)]
        public string ProfilResimDosyaAdi { get; set; }

        public bool aktif { get; set; }

        [Required]
        public Guid AktifGuid { get; set; }
        public bool Admin { get; set; }

        public virtual List<Makale> Makaleler { get; set; }
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual List<Begeni> Begeniler { get; set; }
    }
}
