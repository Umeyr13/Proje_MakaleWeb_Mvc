using Makale_Entities;
using MakaleBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Proje_MakaleWeb_Mvc.Models
{
    public class CacheHelper
    {

        /*Eskiden yaptığımız yöntem*/
        /*
            //Bu ön bellektir. Bir süresi, adı, değeri var. Tarih verilebilir, zamanlayıcı verilebilir. Sonra silinir. Silinme önceliği belirtilir. cache silindiğinde bir şey yaptırılabilir.
            HttpContext.Cache.Add("Ad_string_key", "Elif_object_value", null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 0, 10), System.Web.Caching.CacheItemPriority.Normal/*Öncelik normal*///, CacheSilindigiZaman/*Bir delege dir.*/);

        //HttpContext.Cache.Add("Ad_string_key", "Elif_object_value", null, new DateTime(2023,04,12,10,10,20) , System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null /*cach in süresi bitince bunu


        //Nuget packages ile Microsoft.AspNet.WebHelpers 
        
        public static List<Kategori> KategoriCache()
        {
            var kategoriler = WebCache.Get("kat-cache");
            if (kategoriler == null) // ilk çağırdığımızda boş olduğu için buraya girer.
            {
                KategoriYonet ky = new KategoriYonet();
                kategoriler = ky.Listele();
                WebCache.Set("kat-cache",kategoriler,20/*dakika*/,true/*süre bitince bi 20 dk daha uzat*/);
            }

            return kategoriler;
        }

        public static void CacheTemizle()
        {
            WebCache.Remove("kat-cache");
        }
    }

}