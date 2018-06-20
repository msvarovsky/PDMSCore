﻿using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Data.SqlClient;

namespace PDMSCore.BusinessObjects
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<Panel> PanelList { get; set; }
        public Menu SideMenu { get; set; }

        public Project()
        {
            PanelList = new List<Panel>();
            SideMenu = new Menu();
        }
        public bool Create()
        {
            // jdi do DB a vytvor novy projekt.
            ID = (Int32)DateTime.Now.Ticks;
            return true;
        }
        public void CreateNew()
        {
            // jdi do DB a vytvor novy projekt.
            //ID = (Int32)DateTime.Now.Ticks;
            //ID = DateTime.Now.Hour * 100000000 + DateTime.Now.Minute * 1000000 + DateTime.Now.Second * 10000 + DateTime.Now.Millisecond * 100;
            ID = DateTime.Now.Millisecond * 100;

            SideMenu.GetRandomMenu();
            List<Field> fields = new List<Field>();

            fields.Add(new LabelTextBoxField(ID++, "Project name", "", "...", "Give your project a name."));
            fields.Add(new LabelTextAreaField(ID++, "Project description:"));
            fields.Add(Field.NewLine());

            LabelDropDownField dd = new LabelDropDownField(ID++, "Project type:");
            dd.Dropdown.Add(new DropDownOption(1.ToString(), "Novy"));
            dd.Dropdown.Add(new DropDownOption(2.ToString(), "Stary"));
            dd.Dropdown.Add(new DropDownOption(3.ToString(), "Refresh"));
            fields.Add(dd);

            LabelDropDownField dd2 = new LabelDropDownField(ID++, "pt:");
            dd2.Dropdown.Add(new DropDownOption(1.ToString(), "Novy"));
            dd2.Dropdown.Add(new DropDownOption(2.ToString(), "Stary"));
            dd2.Dropdown.Add(new DropDownOption(3.ToString(), "Refresh"));
            fields.Add(dd2);

            fields.Add(new LabelDatePickerField(ID++, "Creation date:", DateTime.Now));

            fields.Add(new LabelSelectableTextBoxField(ID++, "Choose user:", "...", "sp_AllUsers"));


            Panel panel = new Panel(1, "New project:", 2);
            panel.GenerateRandomPanelMenuItems(5);
            panel.Content = fields;
            PanelList.Add(panel);
        }

        public bool SaveFromHtml(IFormCollection fc)
        {
            StringValues sv = new StringValues();

            if (!fc.TryGetValue("PanelId", out sv))
                return false;

            Panel p = GetPanel(sv.ToString());
            return p.Save(fc);
        }

        private Panel GetPanel(string id)
        {
            int iId;
            if (Int32.TryParse(id, out iId))
            {
                for (int i = 0; i < PanelList.Count; i++)
                {
                    if (PanelList[i].id == iId)
                        return PanelList[i];
                }
            }
            return null;
        }

        public void AddPanel(Panel newPanel)
        {
            PanelList.Add(newPanel);
        }

        public void GetFieldTemplate()
        {
            //GetRandom
        }

        //private bool AuthUser()
        //{

        //}

        //public static void GetProject(int ProjectID)
        //{

        //    bool ret = false;
        //    using (SqlConnection con = new SqlConnection(
        //                                   "user id=Martin;" +
        //                                   "password=6835001;" +
        //                                   "server=192.168.2.101;" +
        //                                   "database=test; " +
        //                                   "connection timeout=10"
        //                                   ))
        //    {
        //        SqlCommand sql = new SqlCommand("AuthUser", con);
        //        sql.CommandType = CommandType.StoredProcedure;
        //        sql.Parameters.Add(new SqlParameter("@username", tbName.Text));
        //        sql.Parameters.Add(new SqlParameter("@pass", tbPass.Text));
        //        try
        //        {
        //            con.Open();
        //            int r = (int)sql.ExecuteScalar();
        //            if (r == 0)
        //            {   //  Not OK
        //                ret = false;
        //            }
        //            else
        //            {   //  OK
        //                ret = true;
        //            }
        //        }
        //        catch (Exception eee)
        //        {
        //            eee = null;
        //        }
        //    }
        //    return ret;

        //}

        public void GetRandom()
        {
            SideMenu.GetRandomMenu();

            int id = 1;
            long randonName = DateTime.Now.Ticks;

            List<Field> fields = new List<Field>();

            //TextBoxField tb = new TextBoxField((randonName++).ToString(), "Pocet projektu", "5");
            
            fields.Add(new LabelField("Normalni text"));
            fields.Add(Field.NewLine());
            fields.Add(new LabelField("Bold text",true));
            fields.Add(Field.NewLine());

            fields.Add(LabelTextBoxField.GetRandom(id++));
            fields.Add(LabelTextAreaField.GetRandom(id++));
            fields.Add(Field.NewLine());

            fields.Add(LabelRBCBControl<LabelRadioButtonField>.GetRandom((id++).ToString(),3));
            fields.Add(LabelRBCBControl<LabelCheckBoxField>.GetRandom((id++).ToString(), 4));
            fields.Add(LabelDropDownField.GetRandom(id++,4));
            fields.Add(LabelDatePickerField.GetRandom(id++));

            fields.Add(LabelFileUploadField.GetRandom());
            fields.Add(LabelFileUploadField.GetRandom(true));

            fields.Add(LabelFileDownloadField.GetRandom());


            /*fields.Add(LabelCheckBoxesControl.GetRandom(2));
            fields.Add(LabelRadioButtonsControl.GetRandom(2));*/


            //fields.Add(new Field { Label = "Pocet projektu", tagName = 1, Type = FieldType.Indicator, Value = "5" });
            //fields.Add(new Field { Label = "Pocet otevrenych projektu", tagName = 2, Type = FieldType.Indicator, Value = "5" });

            Panel panel = new Panel(1,"GetRandom",1);
            //panel.Content = fields;
            fields = new List<Field>();
            fields.Add(LabelTextBoxField.GetRandom(id++));
            panel.GenerateRandomPanelMenuItems(5);
            panel.Content = fields;
            //ToShow.Add(panel);

            Panel panel2 = new Panel(2, "Grid", 1);
            List<Field> fields2 = new List<Field>();
            fields2.Add(LabelTextBoxField.GetRandom(id++));
            fields2.Add(LabelDataGridField.GetRandom("Grid"));

            panel2.Content = fields2;
            panel2.GenerateRandomPanelMenuItems(2);


            PanelList.Add(panel);
            PanelList.Add(panel2);
        }

        public TagBuilder AsyncPanelList()
        {
            AsyncPanel ap = new AsyncPanel();

            return ap.HtmlText();

        }

        public void LoadProjectFromDB()
        {
            bool ret = false;
            string cd = Directory.GetCurrentDirectory();
            string l = "PDMSCore";
            int a = cd.IndexOf(l);
            string AttachDbFilename = cd.Substring(0, a + l.Length);
            AttachDbFilename = AttachDbFilename + "\\PDMSCore\\wwwroot\\TestDB\\System.mdf;";

            using (SqlConnection con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                "AttachDbFilename=" + AttachDbFilename +
                "Connect Timeout=30;" +
                "User Id=martin;" +
                "Password=martin;")
                )
            {
                con.Open();

                //LoadProjectInfo(id);
                //LoadMenu(id, page) ???
                //LoadPage(id, page)




                //Label = LoadPanelInfo(con, 1, "en");
                Content = LoadPanelContent(con, 1, "en");
                //  LoadMenu
            }
            return ret;
        }

    }
}