using PDMSCore.BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Text;
using PDMSCore.DataManipulation;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
            
            p.GetRandom();



            return View(p);
        }

        [HttpGet]
        public ActionResult ShowProject()
        {
            int PanelID = 1;

            //p = Project.GetProject(PanelID);
            p = new Project();

            p.GetRandom();
            return View(p);
        }

        [HttpPost]
        public ActionResult ShowProject(IFormCollection fc)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var FieldId in fc.Keys)
            {
                sb.AppendLine(FieldId + ": " + fc[FieldId].ToString());
            }

            //p = Project.GetProject(PanelID);
            p = new Project();

            p.GetRandom();
            return View(p);
        }




        public JsonResult Ajax(string prefix, string id)
        {
            List<JsonItem> results = new List<JsonItem>();

            int i = 0;

            results.Add(new JsonItem(i++.ToString(), "Value-" + i));
            results.Add(new JsonItem(i++.ToString(), "Value-" + i));
            results.Add(new JsonItem(i++.ToString(), "Value-" + i));

            dynamic product = new JObject();
            product.ProductName = "Elbow Grease";
            product.Enabled = true;
            product.Price = 4.90m;
            product.StockCount = 9000;
            product.StockValue = 44100;
            product.Tags = new JArray("Real", "OnSale");
            Console.WriteLine(product.ToString());

            string r = JsonConvert.SerializeObject(results, Formatting.Indented);

            //string r = ToJson<List<JsonItem>>(results, Encoding.UTF8);
            //return Json(results);
            //return Json(r.Trim());
            //return Json("{ \"ID \" : \"Name\"");
            //  "[{"ID":"idecko1","Value":"hodnota1"},{"ID":"idecko2","Value":"hodnota2"}]"
            //return Json("[{\"ID\":\"idecko1\",\"Value\":\"hodnota1\"},{\"ID\":\"idecko2\",\"Value\":\"hodnota2\"}]");

            //{ label: "Choice1", value: "value1" }
            //  "[{"label":"idecko1","value":"hodnota1"},{"label":"idecko2","value":"hodnota2"}]"

            return Json("{\"label\":\"idecko1\",\"value\":\"hodnota1\"},{\"label\":\"idecko2\",\"value\":\"hodnota2\"}");
        }

        public static string ToJson<T>(/* this */ T value, Encoding encoding)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream, encoding))
                {
                    serializer.WriteObject(writer, value);
                }

                return encoding.GetString(stream.ToArray());
            }
        }

        [HttpPost]
        public ActionResult ShowAll(IFormCollection fc)
        {
            

            //Project.SaveFromHtml(fc);

            

            return RedirectToAction("Index");
        }
    }
}