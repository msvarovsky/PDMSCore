using PDMSCore.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PDMSCore.Controllers
{
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
            p.
            p.GetRandom();



            return View(p);
        }

        [HttpGet]
        public ActionResult ShowAll()
        {
            p = new Project();
            p.GetRandom();



            return View(p);
        }

        [HttpPost]
        public ActionResult ShowAll(IFormCollection fc)
        {
            

            Project.SaveFromHtml(fc);

            

            return RedirectToAction("Index");
        }
    }
}