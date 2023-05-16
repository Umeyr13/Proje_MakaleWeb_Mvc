using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.UI;
using Proje_MakaleWeb_Mvc.Models;

namespace Proje_MakaleWeb_Mvc.Filter
{
    public class Auth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            
                if (SessionsUser.login == null)
               filterContext.Result = new RedirectResult("/Home/YetkisizErisim");
        }
    }
}
