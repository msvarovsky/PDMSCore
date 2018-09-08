using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDMSCore.BusinessObjects;
using PDMSCore.DataManipulation;
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

        [HttpPost]
        public ActionResult Labels(IFormCollection fc)
        {
            string ComplexID="";
            if (fc.Keys.Count > 0)
            {
                foreach (string key in fc.Keys)
                {
                    ComplexID = key;
                    break;
                }
                int s = ComplexID.IndexOf("-");
                string NormalID = ComplexID.Substring(2, s - 2);

                Labels l = GetLabels(NormalID);
                l.Save(fc);
            }

          
            return this.RedirectToAction("Labels");
        }


        public ActionResult Labels()
        {
            ViewData["gsi"] = JsonConvert.SerializeObject(new GeneralSessionInfo(1, 1, "en"));
            return View(GetLabels());
        }

        private Labels GetLabels(string ID="")
        {
            Labels l = new Labels(ID).LoadLabelsFromDB();
            l.DataGrid.CallingControllerAndAction = "Configuration/RefreshData";
            l.DataGrid.CallingControllerAndActionData = "Labels";
            return l;
        }

        public ContentResult RefreshData(string DataGridID, string ContentID, string[] FilterValues)
        {
            Labels l = GetLabels(DataGridID);
            //l.DataGrid.ID = DataGridID;
            l.DataGrid.ApplyFilters(FilterValues, FilteringType.StartWith);
            
            return Content(WebStuffHelper.GetString(l.DataGrid.HtmlTextTableBody()));
        }
    }
}