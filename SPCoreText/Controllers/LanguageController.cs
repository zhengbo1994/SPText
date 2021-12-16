using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.Controllers
{
    public class LanguageController : Controller
    {
        [HttpGet]
        public IActionResult Change(string language, string url)
        {
            //Response.Cookies.Delete(".AspNetCore.Culture");
            //Response.Cookies.Append(".AspNetCore.Culture", language);

            //string str;
            //Request.Cookies.TryGetValue(CookieRequestCultureProvider.DefaultCookieName, out str);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(url);
        }
    }
}
