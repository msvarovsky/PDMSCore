using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PDMSCore.DataManipulation
{
    public class Panel
    {
        public int id { get; set; }
        public string Size { get; set; }
        public string Label { get; set; }
        public List<Field> Content { get; set; }
        public PanelMenu menu { get; set; }

        public Panel(int id, string Label, int xSize)
        {
            this.id = id;
            this.Label = Label;

            xSize = (xSize > 2) ? 2 : xSize;
            xSize = (xSize < 1) ? 1 : xSize;
            this.Size = "x" + xSize;

            this.Content = new List<Field>();
            menu = new PanelMenu(id);
        }


        public bool Load()
        {
            bool ret = false;
            string cd = Directory.GetCurrentDirectory();
            string l = "PDMSCore";
            int a = cd.IndexOf(l);
            string AttachDbFilename = cd.Substring(0, a + l.Length);
            AttachDbFilename = AttachDbFilename + "\\PDMSCore\\wwwroot\\TestDB\\System.mdf;";

            using (SqlConnection con = new SqlConnection(
                "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                //"AttachDbFilename=C:\\!Martin\\Osobni\\Programovani\\GitHubRepo\\PDMSCore\\PDMSCore\\wwwroot\\TestDB\\System.mdf;" + 
                "AttachDbFilename=" + AttachDbFilename +
                "Connect Timeout=30;" +
                "User Id=martin;" +
                "Password=martin;")
                )
            {
                con.Open();

                //LoadTest(con);

                Label = LoadPanelInfo(con, 1, "en");
                Content = LoadPanelContent(con, 1, "en");
                //  LoadMenu
            }
            return ret;
        }
        private void LoadTest(SqlConnection con)
        {
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("SELECT CompanyID FROM Panels", con);

            try
            {
                using (SqlDataReader sdr = sql.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        //string aaa  = sdr["CompanyID"];
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private string LoadPanelInfo(SqlConnection con, int CompanyID, string LanguageID)
        {
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPanelFromID", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("PanelID", this.id));
            sql.Parameters.Add(new SqlParameter("CompanyID", CompanyID));
            sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));

            try
            {
                using (SqlDataReader sdr = sql.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        return sdr.GetString(2).Trim();
                    }
                }
            }
            catch (Exception)
            {
                return "Exception in LoadPanelInfo(CompanyID = " + CompanyID + ", Lang = " + LanguageID + ")";
            }
            return "Something went wrong at LoadPanelInfo(CompanyID = " + CompanyID + ", Lang = " + LanguageID + ")";
        }

        private List<Field> LoadPanelContent(SqlConnection con, int CompanyID, string LanguageID)
        {
            List<Field> ret = new List<Field>();
            SqlCommand sql = new SqlCommand("GetPanelFields", con);
            sql.CommandType = CommandType.StoredProcedure;
            sql.Parameters.Add(new SqlParameter("PanelID", this.id));
            sql.Parameters.Add(new SqlParameter("CompanyID", CompanyID));
            sql.Parameters.Add(new SqlParameter("LanguageID", LanguageID));

            try
            {
                using (SqlDataReader sdr = sql.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        Field f = null;
                        int FieldID = sdr.GetInt32(0);
                        string FieldType = sdr.GetString(1).Trim();
                        string Label = sdr.GetString(2).Trim();

                        string test = sdr.GetString(3).Trim();

                        switch (FieldType)
                        {
                            case "tx":
                                string StringValue = (sdr.IsDBNull(3) ? "" : sdr.GetString(3).Trim());
                                f = new LabelTextBoxField(FieldID, Label, StringValue);
                                break;
                            case "rb":
                                int IntValue = (sdr.IsDBNull(4) ? -1 : sdr.GetInt32(3));

                                //f = new LabelRadioButtonField LabelTextBoxField(FieldID, Label, StringValue);
                                f = new LabelTextBoxField(FieldID, Label, "RadioBox:TODO");
                                break;

                            default:
                                break;
                        }
                        ret.Add(f);
                    }
                }
            }
            catch (Exception eee)
            {
                ret.Add(new LabelTextAreaField(1,"Exception in LoadPanelContent(..)",eee.ToString()));
            }
            return ret;
        }

        public bool Save(IFormCollection fc)
        {
            foreach (var FieldId in fc.Keys)
            {
                if (FieldId == "PanelId")
                    continue;

                string FieldValue = fc[FieldId];
            }
            return false;
        }

        public void AddFields(Field newField)
        {
            this.Content.Add(newField);
        }

        public void GenerateRandomPanelMenuItems(int count)
        {
            string PanelID = id.ToString();

            menu.items.Add(new PanelMenuItem(true, "Refresh", "refresh", PanelID));
            menu.items.Add(new PanelMenuItem(true, "Save", "save", PanelID));

            for (int i = 2; i < count; i++)
                menu.items.Add(new PanelMenuItem(false, i.ToString(), "www" + i + ".cz", PanelID));
        }

    }

  

    public class PanelMenuItem
    {/*     <a href="#">Link 1-</a>
                <a href="#">Link 3-</a>     */

        public bool IsActionAsync { get; set; }
        public string ItemText { get; set; }
        public string ItemLinkOrAsyncAction { get; set; }
        private string PanelMenuId { get; set; }

        public PanelMenuItem(bool Async, string Text, string LinkOrAsyncAction, string PanelMenuId)
        {
            IsActionAsync = Async;
            ItemText = Text;
            ItemLinkOrAsyncAction = LinkOrAsyncAction;
            this.PanelMenuId = PanelMenuId;
        }

        public TagBuilder HtmlText(string PanelOwnerID, string PanelID)
        {
            TagBuilder tb = new TagBuilder("a");
            if (IsActionAsync)
                //tb.Attributes.Add("onclick", "onPanelMenuItemClick('" + PanelID + "','"+ ItemLinkOrAsyncAction + "')");
                tb.Attributes.Add("onclick", "onPanelMenuItemClick('" + PanelOwnerID + "','" + PanelID + "','" + ItemLinkOrAsyncAction + "')");
            else
                tb.Attributes.Add("href", ItemLinkOrAsyncAction);

            tb.InnerHtml.AppendHtml(ItemText);
            return tb;
        }
    }

    public class PanelMenu
    {   
        /*  <div class="dd-menu">
                <i class="fa fa-gear dd-menu-btn" onclick="onPanelMenuClick()" aria-hidden="true"></i>
                <div id="myDropdown" class="dd-menu-content">
                </div>
            </div>          */

        public List<PanelMenuItem> items { get; set; }
        private string PanelId { get; set; }

        public PanelMenu(int id)
        {
            //PanelMenuId = "PanelMenu" + id;
            PanelId = id.ToString();

            items = new List<PanelMenuItem>();
        }

        public void AddMenuItem(PanelMenuItem pmi)
        {
            items.Add(pmi);
        }

        public TagBuilder HtmlText(object PanelOwner)
        {
            string PanelOwnerID = (string)PanelOwner;
            TagBuilder tbDiv = new TagBuilder("div");
            tbDiv.AddCssClass("dd-menu");

            TagBuilder tbI = new TagBuilder("i");
            tbI.AddCssClass("fa fa-gear dd-menu-btn");
            tbI.Attributes.Add("onclick", "onPanelMenuClick('" + HttpUtility.JavaScriptStringEncode(PanelId) + "')");
            tbI.Attributes.Add("aria-hidden", "true");
            tbI.RenderSelfClosingTag();

            TagBuilder tbDivContent = new TagBuilder("div");
            tbDivContent.Attributes.Add("id", PanelId);
            tbDivContent.AddCssClass("dd-menu-content");

            for (int i = 0; i < items.Count; i++)
            {
                tbDivContent.InnerHtml.AppendHtml(items[i].HtmlText(PanelOwnerID, PanelId));
            }

            tbDiv.InnerHtml.AppendHtml(tbI);
            tbDiv.InnerHtml.AppendHtml(tbDivContent);

            return tbDiv;
        }
    }
}