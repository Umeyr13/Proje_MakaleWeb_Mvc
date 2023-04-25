﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale_Entities
{
    public class BaseClass
    {
        //Bilgiler her tabloda olacak. Buraya taşıdık ondan dolayı

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime KayitTarihi { get; set; }

        [Required]
        public DateTime DegistirmeTarihi { get; set; }

        [Required,StringLength(30)]
        public string DegistirenKullanici { get; set; }
    }
}