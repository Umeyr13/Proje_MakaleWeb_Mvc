using Makale_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proje_MakaleWeb_Mvc.Models
{
    public static class SessionsUser
    {
        public static Kullanici login 
        {

            get 
            {
                if (HttpContext.Current.Session["Login"] !=null)
                {
                    return HttpContext.Current.Session["Login"] as Kullanici;
                }
                return null;
                
            }

            set 
            {
                HttpContext.Current.Session["Login"] = value; //gelen değeri sessiona attık
                
                
            }
              
        }
    }
}