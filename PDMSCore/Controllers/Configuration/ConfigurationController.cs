using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDMSCore.BusinessObjects;
using PDMSCore.Models;

namespace PDMSCore.Controllers
{
    public class ConfigurationController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["gsi"] = JsonConvert.SerializeObject(new GeneralSessionInfo(1, 1, "en"));

            return View();
        }


        [HttpGet]
        public ActionResult Pages()
        {
            ViewData["gsi"] = JsonConvert.SerializeObject(new GeneralSessionInfo(1, 1, "en"));


            return View();
        }

        public ActionResult Labels()
        {
            ViewData["gsi"] = JsonConvert.SerializeObject(new GeneralSessionInfo(1, 1, "en"));

            Labels l = new Labels();
            l.LoadLabelsFromDB();
            
            return View(l);
        }
    }
    
}