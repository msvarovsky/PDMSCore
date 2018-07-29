﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PDMSCore.DataManipulation;

namespace PDMSCore.Controllers
{
    public class ModalDialogController : Controller
    {
        public ActionResult NewModal(string DialogID, string TagIDOfReturnedID, string TagIDOfReturnedLabel, string ContentReference)
        {
            ModalDialog md = new ModalDialog("en", "Test title");
            md.Fields = GetFieldsFromDB(ContentReference);
            md.TagIDOfReturnedID = TagIDOfReturnedID;
            md.TagIDOfReturnedLabel = TagIDOfReturnedLabel;
            md.ModalDialogID = DialogID;

            return PartialView("NewModal", md);
        }

        public List<Field> GetFieldsFromDB(string sp)
        {
            int id = 0;
            List<Field> r = new List<Field>();
            if (sp == "sp_Users")
            {
                r.Add(DataGridField2.GetTestData(0));
            }
            else
            {
                r.Add(new LabelTextBoxField(id++, "Testovaci LabelField", ""));
                r.Add(new LabelTextBoxField(id++, "Testovaci LabelField 2", ""));
                r.Add(new LabelTextBoxField(id++, "Testovaci LabelField 3", ""));
                r.Add(DataGridField2.GetTestData(id++));
            }
            return r;
        }

        public ActionResult ModalPartial(string DialogID, string TagIDOfReturnedID, string TagIDOfReturnedLabel, string DialogType, string ContentReference)
        {
            if (DialogType == "ModalDataGrid")
            {
                ModalDialog md = new ModalDialog("en", "Test title");
                md.TagIDOfReturnedID = TagIDOfReturnedID;
                md.TagIDOfReturnedLabel = TagIDOfReturnedLabel;
                md.ModalDialogID = DialogID;
                md.Fields = GetFieldsFromDB(ContentReference);
                return PartialView("NewPartial", md);
            }
            else
            {
                ModalDialog md = new ModalDialog("en", "Test title");
                DataGridField2 d = new DataGridField2();
                d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");

                TableRow2 tr = new TableRow2();
                tr.AddColumnCell(new LabelField("Jmeno"));
                tr.AddColumnCell(new LabelField("Prijmeni"));
                //tr.AddColumnCell(new CheckBoxField("", "", true, new WebTagAttributes(true, "")));
                tr.AddColumnCell(new CheckBoxField("", "", "", false, false));

                d.AddDataRow(tr, 1);
                d.AddDataRow(tr.MakeCopy());
                d.AddDataRow(tr.MakeCopy());

                md.AddField(d);
                md.TagIDOfReturnedID = TagIDOfReturnedID;
                md.TagIDOfReturnedLabel = TagIDOfReturnedLabel;
                md.ModalDialogID = DialogID;

                //md.ReturnLabelFieldID = ReturnLabelFieldID;
                //md.AddField(new LabelTextBoxField("testTextBoxDield", "Label", "already in"));
                return PartialView("ModalPartial", md);
            }
        }
    }
}