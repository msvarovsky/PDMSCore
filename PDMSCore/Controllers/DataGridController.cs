using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDMSCore.DataManipulation;

namespace PDMSCore.Controllers
{
    public class DataGridController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult DataGridPartialView(string aa)
        {
            DataGridField2 d = new DataGridField2();

            return PartialView("ModalPartialView", d);
        }
    }
}