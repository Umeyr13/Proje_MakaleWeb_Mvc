using Makale_Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proje_MakaleWeb_Mvc.Models;

namespace Proje_MakaleWeb_Mvc.Filter
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionsUser.login != null && SessionsUser.login.Admin == false)
             filterContext.Result = new RedirectResult("/Home/YetkisizErisim");
        }
    }
}