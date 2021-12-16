using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPCoreText.App
{
    public class LanguageApp
    {
        public static string GetCulture()
        {
            string selectedCulture = "en-US";

            HttpContext.Current.Request.Cookies.TryGetValue(CookieRequestCultureProvider.DefaultCookieName, out selectedCulture);

            if (string.IsNullOrEmpty(selectedCulture))
            {
                selectedCulture = "en-US";
                HttpContext.Current.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US")),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
            }
            else if (selectedCulture != "en-US")
            {
                selectedCulture = selectedCulture.Substring(selectedCulture.IndexOf("=") + 1, 5);
            }
            return selectedCulture;
        }
        public static string GetAreaStrByLanguage(string area, string returnType)
        {
            string returnAreaName = area;
            switch (returnType.ToLower())
            {
                case "en-us":
                    returnAreaName = area;
                    break;
                case "zh-cn":
                    if (area.Contains("Singapore"))
                    {
                        returnAreaName = "新加坡";
                    }
                    else if (area.Contains("Malaysia"))
                    {
                        returnAreaName = "马来西亚";
                    }
                    break;
                case "zh-hk":
                    if (area.Contains("Singapore"))
                    {
                        returnAreaName = "新加坡";
                    }
                    else if (area.Contains("Malaysia"))
                    {
                        returnAreaName = "馬來西亞";
                    }
                    break;
                default:
                    returnAreaName = area;
                    break;
            }
            return returnAreaName;
        }
    }
}
