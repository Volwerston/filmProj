using System;
using System.Collections.Generic;
using System.Linq;
using FilmDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using FilmDatabase.Filters;

namespace FilmDatabase.Controllers
{
    [Culture]
    public class NavigationController : Controller
    {
        IFilmRepository repo;
        public NavigationController(FilmRepository fr)
        {
            repo = fr;
            //     TempData["Pressed"] = "false";

        }
        public NavigationController(IFilmRepository context)
        {
            repo = context;
            //    TempData["Pressed"] = "false";
        }
        //
        // GET: /Navigation/
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            List<string> categories = repo.Categories.Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
            return PartialView(categories);
        }

        public PartialViewResult Search(string search)
        {
            ViewBag.Search = search;
            return PartialView();
        }
        public ActionResult ShowGenres(bool pressed)
        {
            if (!pressed)
            {
                TempData["Pressed"] = "true";

            }
            else
            {
                TempData["Pressed"] = "false";
            }
            TempData.Keep("Pressed");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.Path.Value;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }

            Response.Cookies.Append("lang", lang);
            return Redirect(returnUrl);
        }


    }
}