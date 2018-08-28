using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PDMSCore.BusinessObjects;
using PDMSCore.DataManipulation;
using PDMSCore.Models;

namespace PDMSCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return this.RedirectToAction("CreateNewProject", "Project");
            ////return this.RedirectToAction("ShowProject", "Project");
            //Project pr = new Project(null);

            Menu navigation = new Menu();
            navigation.LoadNavigation(new GeneralSessionInfo(1, 1, "en"));

            
            return View(navigation);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
