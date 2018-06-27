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
        public int userID, retailerID, languageID;

        public GeneralSessionInfo()
        {

        }
        public GeneralSessionInfo(HttpContext s)
        {
            userID = Int32.Parse(s.Session.GetString("UserID"));
            retailerID = Int32.Parse(s.Session.GetString("RetailerID"));
            languageID = Int32.Parse(s.Session.GetString("LanguageID"));
        }
        public GeneralSessionInfo(int userID, int retailerID, int languageID)
        {
            this.userID = userID;
            this.retailerID = retailerID;
            this.languageID = languageID;
        }
    }


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

        //  User, RetailerID, languageID,     ProjectID, PageID
        public bool LoadProjectFromDB(GeneralSessionInfo gsi, int ProjectID, int PageID)
        {
            bool ret = false;
            string cd = Directory.GetCurrentDirectory();
            string l = "PDMSCore";
            int a = cd.IndexOf(l);
            string AttachDbFilename = cd.Substring(0, a + l.Length) + "\\PDMSCore\\wwwroot\\TestDB\\System.mdf;";

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

                //LoadPageInfo - P Menu, navigation??
                LoadPageContent(gsi, con, ProjectID, PageID);
                //GeneralSessionInfo gsi = new GeneralSessionInfo() { userID = 1, retailerID = 1, languageID = 1 };

                //Label = LoadPanelInfo(con, 1, "en");
                //Content = LoadPanelContent(con, 1, "en");
                //  LoadMenu
            }
            return ret;
        }

        private bool LoadPageContent(GeneralSessionInfo gsi, SqlConnection con, int ProjectID, int PageID)
        {

            /// exec GetPageFields 1,1,1,'en'
            //List<Object> dbRow = new List<object>();
            object[] dbRow;

            List<TempMultiSelectItem> AllMultiSelectItem = new List<TempMultiSelectItem>();
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPageFields", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("RetailerID", gsi.retailerID));
            sql.Parameters.Add(new SqlParameter("ProjectID", ProjectID));
            sql.Parameters.Add(new SqlParameter("PageID", PageID));
            sql.Parameters.Add(new SqlParameter("@LanguageID", gsi.languageID));

            //  FieldID	FieldType	Label	StringValue	IntValue	DateValue	FileValue	OtherRef	MultiSelectItemID	PredecessorFieldID
            try
            {
                using (SqlDataReader sdr = sql.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        int PanelID;
                        dbRow = new Object[sdr.FieldCount];
                        sdr.GetValues(dbRow);
                        int? nPanelID = sdr.IsDBNull(0) ? (int?)null : sdr.GetInt32(0);
                        if (nPanelID == null)
                            continue;
                        PanelID = (int)nPanelID;

                        Panel p = PanelExists(PanelID);
                        if (p == null)
                        {
                            p.LoadDBRow(dbRow);
                        }
                        else
                        {
                            p = new Panel(PanelID, "TODO:Panel label", 1);    //  TODO: Panel label + Panel xSize 
                            p.LoadDBRow(dbRow);
                            PanelList.Add(p);

                        }
                    }
                }
            }
            catch (Exception eee)
            {
                ret.Add(new LabelTextAreaField(1, "Exception in LoadPanelContent(..)", eee.ToString()));
            }

            AssignMultiSelectItemsToControls(ret, AllMultiSelectItem);


            return ret;
        }

        private Panel PanelExists(int PanelID)
        {
            for (int i = 0; i < PanelList.Count; i++)
            {
                if (PanelList[i].id == PanelID)
                    return PanelList[i];
            }
            return null;
        }

    }
}