using PDMSCore.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PDMSCore.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        [HttpGet]
        public ActionResult Index()
        {   //  Show all projects

            //this.RedirectToAction("ShowAll");

            Project p = new Project();
            p.GetRandom();

            //DataManipulation.Panel panel = new DataManipulation.Panel(1, "Prvni", 1);
            //panel.AddFields(new DataManipulation.LabelTextAreaField("labeltext", "", "neco napis", 5));
            //p.AddPanel(panel);
            //p.AddPanel(panel);

            //ViewBag.Panels = p.GetRandom();
            return View(p);
        }

        [HttpGet]
        public ActionResult ShowAll()
        {
            Project p = new Project();
            p.GetRandom();
            return View(p);
        }

        [HttpPost]
        //public ActionResult ShowAll(Microsoft.AspNetCore.Http.IFormCollection fc)
        public ActionResult ShowAll(Microsoft.AspNetCore.Http.IFormCollection fc)
        {
            foreach (var key in fc.Keys)
            {
                var value = fc[key];
            }

            return RedirectToAction("Index");
        }
    }
}