﻿using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using PDMSCore.DataManipulation;

namespace PDMSCore.Controllers
{
    public class DataGridController : Controller
    {
        
        public ActionResult DataGridPartial(string DataGridID)
        {
            DataGridField2 d = new DataGridField2();
            d.ID = Int32.Parse(DataGridID);
            d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");

            TableRow2 tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Jmeno"));
            tr.AddColumnCell(new LabelField("Prijmeni"));
            tr.AddColumnCell(new CheckBoxField("", "", true, new WebTagAttributes(true, "")));

            d.AddDataRow(tr, 1);
            d.AddDataRow(tr.MakeCopy());
            d.AddDataRow(tr.MakeCopy());

            return PartialView("DataGridPartial", d);
        }

        public ActionResult GetDataGridContent(string DataGridID)
        {
            DataGridField2 d = new DataGridField2();
            d.ID = Int32.Parse(DataGridID);
            d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");

            TableRow2 tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Jmeno-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Prijmeni"));
            tr.AddColumnCell(new CheckBoxField("", "", true, new WebTagAttributes(true, "")));

            d.AddDataRow(tr, 1);
            d.AddDataRow(tr.MakeCopy());
            d.AddDataRow(tr.MakeCopy());

            //return PartialView("DataGridPartialContent", d);
            return Content(GetString(d.HtmlTextContent()));
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}