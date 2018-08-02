using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace PDMSCore.BusinessObjects
{
    public class GeneralSessionInfo
    {
        //public int UserID { get; set; }
        //public int RetailerID { get; set; }
        //public int UserID { get; set; }

        public int userID, retailerID;
        public string languageID;

        public GeneralSessionInfo()
        {
            languageID = "en";

        }
        public GeneralSessionInfo(HttpContext s)
        {
            Int32.TryParse(s.Session.GetString("UserID"), out userID);
            Int32.TryParse(s.Session.GetString("RetailerID"), out retailerID);

            //userID = Int32.Parse(user);
            //retailerID = Int32.Parse(s.Session.GetString("RetailerID"));
            languageID = s.Session.GetString("LanguageID");
        }
        public GeneralSessionInfo(int userID, int retailerID, string languageID)
        {
            this.userID = userID;
            this.retailerID = retailerID;
            this.languageID = languageID;
        }
    }

    public class FieldValueUpdateInfo
    {
        public SqlConnection con { get; set; }
        public int RetailerID { get; set; }
        public int ProjectID { get; set; }
        public string FieldID { get; set; }

        public FieldValueUpdateInfo(SqlConnection con, int RetailerID, int ProjectID, string FieldID = null)
        {
            this.con = con;
            this.RetailerID = RetailerID;
            this.ProjectID = ProjectID;
            this.FieldID = FieldID;
        }
    }

    public class Project
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        //public List<Panel> PanelList { get; set; }
        public Page Page { get; set; }
        public PageMenu PageMenu{ get; set; }
        //public Menu SideMenu { get; set; }

        public Project(int ProjectID)
        {
            this.ProjectID = ProjectID;
            //PanelList = new List<Panel>();
            //SideMenu = new Menu();
            Page = new Page();
            PageMenu = new PageMenu();

        }
        public bool Create()
        {
            // jdi do DB a vytvor novy projekt.
            ProjectID = (Int32)DateTime.Now.Ticks;
            return true;
        }
        public void CreateNew()
        {
            // jdi do DB a vytvor novy projekt.
            //ID = (Int32)DateTime.Now.Ticks;
            //ID = DateTime.Now.Hour * 100000000 + DateTime.Now.Minute * 1000000 + DateTime.Now.Second * 10000 + DateTime.Now.Millisecond * 100;
            ProjectID = DateTime.Now.Millisecond * 100;

            Page.SideMenu.GetRandomMenu();
            List<Field> fields = new List<Field>();

            fields.Add(new LabelTextBoxField("1", ProjectID++, "Project name", "", "...", "Give your project a name."));
            fields.Add(new LabelTextAreaField("1", ProjectID++.ToString(), "Project description:"));
            //fields.Add(Field.NewLine());
            fields.Add(new NewLine());

            LabelDropDownListBox dd = new LabelDropDownListBox("1",ProjectID++.ToString(), "Project type:");
            dd.DropDown.Add("1", "Novy");
            dd.DropDown.Add("2", "Stary",true);
            dd.DropDown.Add("3", "Refresh");
            fields.Add(dd);

            LabelDropDownListBox dd2 = new LabelDropDownListBox("1", ProjectID++.ToString(), "pt:");
            dd2.DropDown.Add("1", "Novy");
            dd2.DropDown.Add("2", "Stary", true);
            dd2.DropDown.Add("3", "Refresh");
            fields.Add(dd2);


            fields.Add(new LabelDatePickerField("1", ProjectID++, "Creation date:", DateTime.Now));

            fields.Add(new LabelSelectableTextBoxField(ProjectID++, "Choose user:", "...", "sp_AllUsers"));


            Panel panel = new Panel(1, "New project:", 2);
            panel.GenerateRandomPanelMenuItems(5);
            panel.Fields = fields;

            Page.Panels.PanelList.Add(panel);
            //PanelList.Add(panel);
        }

        //public bool SaveFromHtml(IFormCollection fc)
        //{
        //    StringValues sv = new StringValues();

        //    if (!fc.TryGetValue("PanelId", out sv))
        //        return false;

        //    Panel p = GetPanel(sv.ToString());
        //    return p.Save(fc);
        //}
        //private Panel GetPanel(string id)
        //{
        //    int iId;
        //    if (Int32.TryParse(id, out iId))
        //    {
        //        for (int i = 0; i < PanelList.Count; i++)
        //        {
        //            if (PanelList[i].id == iId)
        //                return PanelList[i];
        //        }
        //    }
        //    return null;
        //}


        public void GetFieldTemplate()
        {
            //GetRandom
        }

        public void GetRandom()
        {
            Page.SideMenu.GetRandomMenu();

            int id = 1;
            long randonName = DateTime.Now.Ticks;

            List<Field> fields = new List<Field>();

            //TextBoxField tb = new TextBoxField((randonName++).ToString(), "Pocet projektu", "5");

            fields.Add(new LabelField("Normalni text"));
            fields.Add(new NewLine());
            fields.Add(new LabelField("Bold text", true));
            fields.Add(new NewLine());

            fields.Add(LabelTextBoxField.GetRandom(id++));
            fields.Add(LabelTextAreaField.GetRandom(id++));
            fields.Add(new NewLine());

            fields.Add(LabelRadioButtonFields.GetRandom((id++).ToString(), 3));
            fields.Add(LabelRadioButtonFields.GetRandom((id++).ToString(), 4));

            fields.Add(LabelDropDownListBox.GetRandom(id++.ToString(), 4));
            fields.Add(LabelDatePickerField.GetRandom(id++));

            fields.Add(LabelFileUploadField.GetRandom());
            fields.Add(LabelFileUploadField.GetRandom(true));

            fields.Add(LabelFileDownloadField.GetRandom());


            /*fields.Add(LabelCheckBoxesControl.GetRandom(2));
            fields.Add(LabelRadioButtonsControl.GetRandom(2));*/


            //fields.Add(new Field { Label = "Pocet projektu", tagName = 1, Type = FieldType.Indicator, Value = "5" });
            //fields.Add(new Field { Label = "Pocet otevrenych projektu", tagName = 2, Type = FieldType.Indicator, Value = "5" });

            Panel panel = new Panel(1, "GetRandom", 1);
            //panel.Content = fields;
            fields = new List<Field>();
            fields.Add(LabelTextBoxField.GetRandom(id++));
            panel.GenerateRandomPanelMenuItems(5);
            panel.Fields = fields;
            //ToShow.Add(panel);

            Panel panel2 = new Panel(2, "Grid", 1);
            List<Field> fields2 = new List<Field>();
            fields2.Add(LabelTextBoxField.GetRandom(id++));
            fields2.Add(LabelDataGridField.GetRandom("Grid"));

            panel2.Fields = fields2;
            panel2.GenerateRandomPanelMenuItems(2);


            //PanelList.Add(panel);
            //PanelList.Add(panel2);
        }

        public TagBuilder AsyncPanelList()
        {
            AsyncPanel ap = new AsyncPanel();

            return ap.HtmlText();

        }

        private string GetSqlConnectionString()
        {
            string cd = Directory.GetCurrentDirectory();
            string l = "PDMSCore";
            int a = cd.IndexOf(l);
            string AttachDbFilename = cd.Substring(0, a + l.Length) + "\\PDMSCore\\wwwroot\\TestDB\\System.mdf;";

            return "Data Source=(LocalDB)\\MSSQLLocalDB;" + "AttachDbFilename=" + AttachDbFilename +
                "Connect Timeout=30;" + "User Id=martin;" + "Password=martin;";
        }

        public bool LoadProjectFromDB(GeneralSessionInfo gsi, int PageID)
        {
            using (SqlConnection con = new SqlConnection(GetSqlConnectionString()))
            {
                con.Open();

                LoadPageInfo(gsi, con, ProjectID, PageID);
                LoadPanelsInfo(gsi, con, ProjectID, PageID);
                LoadPageContent(gsi, con, ProjectID, PageID);
            }
            return false;
        }

        private bool LoadPageInfo(GeneralSessionInfo gsi, SqlConnection con, int ProjectID, int PageID)
        {
            SqlDataAdapter sqlDataAdapter;
            DataSet dataSet = new DataSet();

            List<TempMultiSelectItem> AllMultiSelectItem = new List<TempMultiSelectItem>();
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPageInfo", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("RetailerID", gsi.retailerID));
            sql.Parameters.Add(new SqlParameter("PageID", PageID));
            sql.Parameters.Add(new SqlParameter("LanguageID", gsi.languageID));

            try
            {
                sqlDataAdapter = new SqlDataAdapter(sql);
                sqlDataAdapter.Fill(dataSet);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    Page.ProcessPageInfo(dataSet.Tables[0]);
                else
                {
                    Page.GenerateUnknownPageInfo();
                    Console.WriteLine("No matching records found.");
                }
            }
            catch (Exception eee)
            {
                ret.Add(new LabelTextAreaField("1", "Exception in LoadPageInfo(..)", eee.ToString()));
            }
            return false;
        }
        private bool LoadPanelsInfo(GeneralSessionInfo gsi, SqlConnection con, int ProjectID, int PageID)
        {
            SqlDataAdapter sqlDataAdapter;
            DataSet dataSet = new DataSet();

            List<TempMultiSelectItem> AllMultiSelectItem = new List<TempMultiSelectItem>();
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPanelsInfo", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("RetailerID", gsi.retailerID));
            sql.Parameters.Add(new SqlParameter("PageID", PageID));
            sql.Parameters.Add(new SqlParameter("LanguageID", gsi.languageID));

            try
            {
                sqlDataAdapter = new SqlDataAdapter(sql);
                sqlDataAdapter.Fill(dataSet);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    Page.ProcessPanelsInfo(dataSet.Tables[0]);
                else
                {
                    Page.GenerateUnknownPageInfo();
                    Console.WriteLine("No matching records found.");
                }
            }
            catch (Exception eee)
            {
                ret.Add(new LabelTextAreaField("1", "Exception in LoadPanelsInfo(..)", eee.ToString()));
            }
            return false;
        }
        private bool LoadPageContent(GeneralSessionInfo gsi, SqlConnection con, int ProjectID, int PageID)
        {
            SqlDataAdapter sqlDataAdapter;
            DataSet dataSet = new DataSet();

            List<TempMultiSelectItem> AllMultiSelectItem = new List<TempMultiSelectItem>();
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPageFields", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("RetailerID", gsi.retailerID));
            sql.Parameters.Add(new SqlParameter("ProjectID", ProjectID));
            sql.Parameters.Add(new SqlParameter("PageID", PageID));
            sql.Parameters.Add(new SqlParameter("LanguageID", gsi.languageID));

            try
            {
                //  http://www.dotnetfunda.com/articles/show/1716/multiple-resultsets-in-sql-server-and-handling-them-in-csharp-part-ii

                sqlDataAdapter = new SqlDataAdapter(sql);
                sqlDataAdapter.Fill(dataSet);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0 )
                { 
                    Page.Panels.ProcessFieldsInfo(dataSet.Tables[0]);
                }
                else
                {
                    Page.GenerateUnknownPageInfo();
                    Console.WriteLine("No matching records found.");
                }
                //if (dataSet.Tables.Count > 0)
                //{
                //    if (dataSet.Tables[0].Rows.Count > 0)
                //        Page.ProcessPageInfo(dataSet.Tables[0]);
                //    if (dataSet.Tables[1].Rows.Count > 0)
                //        Page.ProcessPanelsInfo(dataSet.Tables[1]);
                //    if (dataSet.Tables[2].Rows.Count > 0)
                //        Page.Panels.ProcessFieldsInfo(dataSet.Tables[2]);
                //}
            }
            catch (Exception eee)
            {
                ret.Add(new LabelTextAreaField("1", "Exception in LoadPanelContent(..)", eee.ToString()));
            }

            return false;
        }

        public bool SavePage(GeneralSessionInfo gsi, int PageID, IFormCollection fc)
        {
            Project OldProject = new Project(ProjectID);
            OldProject.LoadProjectFromDB(gsi, PageID);
            using (SqlConnection con = new SqlConnection(GetSqlConnectionString()))
            {
                con.Open();
                FieldValueUpdateInfo UpdateInfo = new FieldValueUpdateInfo(con, gsi.retailerID, ProjectID);
                OldProject.Page.Panels.SavePanels(fc, UpdateInfo);
            }

            return false;
        }

    }
    
}