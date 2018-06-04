using System;
using Microsoft.AspNetCore.Mvc;
using PDMSCore.DataManipulation;

namespace PDMSCore.Controllers
{
    public class PanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PanelMenuItemClick(string PanelOwnerID, string PanelMenuID, string PanelMenuItemID)
        {
            int MenuItemCode = int.Parse(PanelMenuItemID);
            if (MenuItemCode == 0)    // Refresh
            {
                Panel p = new Panel(5, "Label:" + DateTime.Now.ToLongTimeString(), 1);
                p.GenerateRandomPanelMenuItems(5);
                return PartialView("PartialPanel", p);
            }
            else if (MenuItemCode == 1) //  Save
            {
                System.Threading.Thread.Sleep(100);
                return PartialView();
            }
            else
                return null;
        }
        
        [HttpGet]
        public ActionResult ReturnFromModalFieldUpdate(string FieldIDToLookUp)
        {
            DataGridField2 dgf = DataGridField2.GetTestData(1);
            string ret = dgf.GetPresentableStringFromID(FieldIDToLookUp);
            return this.Json(ret);
        }

        
    }
}