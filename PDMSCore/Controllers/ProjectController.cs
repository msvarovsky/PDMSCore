using PDMSCore.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

        // GET: Project
        [HttpGet]
        public ActionResult Index()
        {   //  Show all projects

            //this.RedirectToAction("ShowAll");
            TempData.Add("a", "b");

            p = new Project();
            

            //DataManipulation.Panel panel = new DataManipulation.Panel(1, "Prvni", 1);
            //panel.AddFields(new DataManipulation.LabelTextAreaField("labeltext", "", "neco napis", 5));
            //p.AddPanel(panel);
            //p.AddPanel(panel);

            //ViewBag.Panels = p.GetRandom();
            return View(p);
        }

        [HttpGet]
        public ActionResult CreateNewProject()
        {
            Project p = new Project();
            p.GetRandom();
            return View(p);
        }

        [HttpGet]
        public ActionResult ShowProject()
        {
            //int PanelID = 1;

            //p = Project.GetProject(PanelID);
            p = new Project();
            ViewData["panelOwnerID"] = "ID projektu";

            p.GetRandom();
            return View(p);
        }

        [HttpPost]
        public ActionResult ShowProject(IFormCollection fc)
        {
            //p = Project.GetProject(PanelID);
            p = new Project();
            p.GetRandom();
            p.SideMenu.Select(HttpContext.Session.GetString("OpenMenu"));

            return View(p);
        }

        /// <summary>
        /// Uschova do cookie link kliknuteho menu 
        /// </summary>
        /// <param name="href"></param>
        [HttpPost]
        public void OpenMenuUpdate(string href)
        {
            HttpContext.Session.SetString("OpenMenu", href);
        }


        //public ActionResult DataGridPartial(string DialogID, string ReturnFieldID)
        //{
        //    DataGridField2 d = new DataGridField2();
        //    return PartialView("ModalPartial", d);
        //}


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