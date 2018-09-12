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
using static PDMSCore.DataManipulation.WebStuffHelper;

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


        public ActionResult Labels()                                        //  Index
        {
            ViewData["gsi"] = JsonConvert.SerializeObject(new GeneralSessionInfo(1, 1, "en"));
            return View(GetLabels());
        }

        [HttpPost]
        public ActionResult Labels(IFormCollection fc, string tra="")       //  Save
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
                string NormalID = ComplexID.Substring(0, s);

                Labels l = GetLabels(NormalID);
                l.Save(fc);
            }
            return this.RedirectToAction("Labels");
        }


        public ContentResult AjaxSave(string formcontent)
        {
            Dictionary<string, string> decoded = DecodeJsonFormData(formcontent);

            if (decoded.Count > 0)
            {
                KeyValuePair<string, string> FirstKVP = decoded.First();

                int s = FirstKVP.Key.IndexOf("-");
                string NormalID = FirstKVP.Key.Substring(0, s);

                Labels l = GetLabels(NormalID);
                l.Save(decoded);
            }


            return Content("<div>Ahoj</div>");
        }


        public ContentResult AddLabel(string DataGridID)                                          //  AddLabel
        {
            Labels l = new Labels(DataGridID);
            return Content(WebStuffHelper.GetString(l.AddLabelDialogHtml()));
        }



        private Labels GetLabels(string ID="")
        {
            Labels l = new Labels(ID).LoadLabelsFromDB();
            l.DataGrid.CallingControllerAndAction = "Configuration/RefreshData";
            l.DataGrid.CallingControllerAndActionData = "Labels";
            l.DataGrid.CallingController = "Configuration";
            l.DataGrid.ControllerAddAction = "AddLabel";

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