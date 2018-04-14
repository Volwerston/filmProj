using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;

namespace FilmDatabase.Filters
{
    public class CultureAttribute : ActionFilterAttribute, IActionFilter
    {
       



        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
          
        }




        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cultureName = null;
            // Получаем куки из контекста, которые могут содержать установленную культуру
            var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cultureCookie != null)
                cultureName = cultureCookie;
            else
                cultureName = "ru";

            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(cultureName))
            {
                cultureName = "ru";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}