using PDMSCore.BusinessObjects;
using Microsoft.AspNetCore.Mvc;


namespace PDMSCore.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project
        public ActionResult Index()
        {   //  Show all projects

            Project p = new Project();
            p.GetRandom();
            //ViewBag.Panels = p.GetRandom();


            return View(p);
        }
    }
}