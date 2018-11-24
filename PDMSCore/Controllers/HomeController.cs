using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDMSCore.BusinessObjects;
using PDMSCore.DataManipulation;
using PDMSCore.Models;
using System.Web;

namespace PDMSCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return this.RedirectToAction("NewProject", "Project", new { NavID = 21 });
            //return this.RedirectToAction("Labels", "Configuration");

            //return this.RedirectToAction("CreateNewProject", "Project");
            ////return this.RedirectToAction("ShowProject", "Project");
            //Project pr = new Project(null);

            GeneralSessionInfo gsi = new GeneralSessionInfo(1, 1, "en");
            ViewData["gsi"] = JsonConvert.SerializeObject(gsi);

            Menu navigation = new Menu();
            navigation.LoadNavigation(gsi);

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
