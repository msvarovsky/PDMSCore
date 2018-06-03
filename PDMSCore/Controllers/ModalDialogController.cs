using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDMSCore.DataManipulation;

namespace PDMSCore.Controllers
{
    public class ModalDialogController : Controller
    {
        public ActionResult ModalPartial(string DialogID, string ReturnFieldID)
        {
            ModalDialog md = new ModalDialog("en", "Test title");

            DataGridField dgf = DataGridField.GetRandom();

            DataGridField2 d = new DataGridField2();
            d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");

            TableRow2 tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Jmeno"));
            tr.AddColumnCell(new LabelField("Prijmeni"));
            tr.AddColumnCell(new CheckBoxField("", "", true, new WebTagAttributes(true, "")));

            d.AddDataRow(tr, 1);
            d.AddDataRow(tr.MakeCopy());
            d.AddDataRow(tr.MakeCopy());


            md.AddField(d);
            md.ReturnFieldID = ReturnFieldID;
            md.ModalDialogID = DialogID;
            //md.AddField(new LabelTextBoxField("testTextBoxDield", "Label", "already in"));


            return PartialView("ModalPartial", md);
        }
    }
}