using PDMSCore.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PDMSCore.DataManipulation;
using System;

namespace PDMSCore.Controllers
{
    public class AutoCompleteSuggestion
    {
        public int Id { get; set; }
        public string Suggestion {get;set; }  
    }  

    public class ProjectController : Controller
    {
        Project p;
        
        [HttpGet]
        public ActionResult Index()
        {
            //TempData.Add("a", "b");
            //p = new Project();
            return View();
            //return this.RedirectToAction("ShowProject", "Project");

        }

        [HttpGet]
        public ActionResult CreateNewProject()
        {
            p = new Project(1);
            p.CreateNew();
            ViewData["panelOwnerID"] = "ID projektu";
            return View("ShowProject",p);
        }
        [HttpPost]
        public ActionResult CreateNewProject(IFormCollection fc)
        {
            Project p = new Project(1);
            //p.SaveFromHtml(fc);
            return View();
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ShowProject(IFormCollection fc)
        {
            //  User, RetailerID, languageID
            GeneralSessionInfo gsi = new GeneralSessionInfo(HttpContext);
            gsi = new GeneralSessionInfo(1, 1, "en");

            Project pr = new Project(1);
            pr.SavePage(gsi, 1, fc); // asi bych ProjectID a PageID mel dodat ze stranky jako hidden fields.

            //p.Page.SideMenu.Select(HttpContext.Session.GetString("OpenMenu"));

            //return View(p);
            return this.RedirectToAction("ShowProject", "Project");
        }

        [HttpGet]
        public ActionResult GetProject(string Param1, string Param2)
        {
            return ShowProject(Param1);
        }
        [HttpGet]
        //public ActionResult ShowProject()
        public ActionResult ShowProject(string ProjectID = null, string Other = null)
        {
            ViewData["panelOwnerID"] = "ID projektu";
            Project pr = new Project(1);
            pr.LoadProjectFromDB(new GeneralSessionInfo(1, 1, "en"), 1);

            if (ProjectID==null)
                return View(pr);
            else
                return PartialView("ShowProjectPartial", pr);
        }

        /// <summary>
        /// Uschova do cookie link kliknuteho menu .
        /// UPDATE 28.8.2018: Asi to nebudu potrebovat. A to proto, ze tedka nacitam content jako ajax volani, 
        ///                 takze se navigace nerefreshuje a tudis nemusim ukladat kliknute menu/nav item.
        /// </summary>
        /// <param name="href"></param>
        [HttpPost]
        public void OpenMenuUpdate(string href)
        {
            if (href == null)
                href = "";
            HttpContext.Session.SetString("OpenMenu", href);
        }


        /*
        [HttpPost]
        public JsonResult AjaxAutoComplete(string prefix, string id)
        {
            //  http://www.c-sharpcorner.com/blogs/how-to-create-autocomplete-textbox-in-asp-net-mvc-5
            List<AutoCompleteSuggestion> objGameList = new List<AutoCompleteSuggestion>()
            {
                new AutoCompleteSuggestion { Id = 1, Suggestion = "Leden" },
                new AutoCompleteSuggestion { Id = 2, Suggestion = "Unor" },
                new AutoCompleteSuggestion { Id = 3, Suggestion = "Brezen" },
                new AutoCompleteSuggestion { Id = 4, Suggestion = "Duben" },
                new AutoCompleteSuggestion { Id = 5, Suggestion = "Kveten" },
                new AutoCompleteSuggestion { Id = 6, Suggestion = "Cerven" },
                new AutoCompleteSuggestion { Id = 7, Suggestion = "Cervenec" },
                new AutoCompleteSuggestion { Id = 8, Suggestion = "Srpen" },
                new AutoCompleteSuggestion { Id = 9, Suggestion = "Zari" },
                new AutoCompleteSuggestion { Id = 10, Suggestion = "Rijen" },
                new AutoCompleteSuggestion { Id = 11, Suggestion = "Listopad" },
                new AutoCompleteSuggestion { Id = 12, Suggestion = "Prosinec" }
            };

            //var result = (from a in objGameList where a.Suggestion.ToLower().StartsWith(prefix.ToLower()) select new { a.Suggestion });
            var result = (from a in objGameList where a.Suggestion.ToLower().Contains(prefix.ToLower()) select new { a.Suggestion });

            //return Json(objGameList);
            return Json(result);
        }*/
    }
}