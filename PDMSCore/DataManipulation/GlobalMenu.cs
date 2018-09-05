using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PDMSCore.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace PDMSCore.DataManipulation
{
    public class GlobalMenuItem
    {
        public string Label { get; set; }
        public int ID { get; set; }
        public List<GlobalMenuItem> Children { get; set; }
        public int Level { get; set; }
        public string Url { get; set; }

        public GlobalMenuItem(int id, string label, int level, string url, List<GlobalMenuItem> l = null)
        {
            ID = id;
            Label = label;
            Level = level;
            Url = url;
            if (l == null)
                Children = new List<GlobalMenuItem>();
            else
                Children = l;
        }

        public void Populate(NavFromDB nav, List<string> children)
        {
            for (int i = 0; i < children.Count; i++)
            {
                int ChildID;
                if (Int32.TryParse(children[i], out ChildID))
                {
                    NavItemFromDB ni = nav.GetNavID(ChildID);
                    if (ni != null)
                    {
                        GlobalMenuItem ChildItem = new GlobalMenuItem(ChildID, ni.Label, this.Level + 1, ni.Url);
                        ChildItem.Populate(nav, ni.ChildrenNavIDs);
                        Children.Add(ChildItem);
                    }
                }
            }
        }

        public TagBuilder HtmlText()
        {
            if (Level == 0)
            {
                TagBuilder tbUL = new TagBuilder("ul");
                tbUL.Attributes.Add("id", "GlobalMenu");
                for (int i = 0; i < Children.Count; i++)
                {
                    tbUL.InnerHtml.AppendHtml(Children[i].HtmlText());
                }
                return tbUL;
            }
            else
            {
                TagBuilder tbLI = new TagBuilder("li");
                tbLI.Attributes.Add("id", ID.ToString());
                tbLI.Attributes.Add(WebStuffHelper.CreateJSParameter("onclick", "OnGlobalMenuItemClick", "this", "LI", Url, "dalsi data"));

                TagBuilder tbDiv = new TagBuilder("div");
                TagBuilder tbLabelDiv = new TagBuilder("div");
                tbLabelDiv.InnerHtml.AppendHtml(Label);

                tbDiv.InnerHtml.AppendHtml(tbLabelDiv);
                if (Children.Count > 0)
                {
                    TagBuilder tbUL = new TagBuilder("ul");
                    if (Level == 1)
                        tbUL.AddCssClass("sub-d");

                    for (int i = 0; i < Children.Count; i++)
                        tbUL.InnerHtml.AppendHtml(Children[i].HtmlText());

                    tbDiv.InnerHtml.AppendHtml(tbUL);
                }
                tbLI.InnerHtml.AppendHtml(tbDiv);
                return tbLI;
            }
        }
    }

    public static class GlobalMenu
    {
        public static GlobalMenuItem LoadFromDB(GeneralSessionInfo gsi)
        {
            using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
            {
                con.Open();
                SqlDataAdapter sqlDataAdapter;
                DataSet dataSet = new DataSet();

                SqlCommand sql = new SqlCommand("GetGlobalMenu", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add(new SqlParameter("RetailerID", gsi.retailerID));
                sql.Parameters.Add(new SqlParameter("UserID", gsi.userID));
                sql.Parameters.Add(new SqlParameter("LanguageID", gsi.languageID));

                try
                {
                    sqlDataAdapter = new SqlDataAdapter(sql);
                    sqlDataAdapter.Fill(dataSet);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        return ProcessMenu(dataSet.Tables[0]);
                    else
                    {
                        Console.WriteLine("No matching records found.");
                    }
                }
                catch { }
            }
            return null;
        }
        private static GlobalMenuItem ProcessMenu(DataTable dt)
        {
            NavFromDB nav = new NavFromDB();

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                NavItemFromDB ni = new NavItemFromDB();
                ni.NavID = DBUtil.GetInt(dt.Rows[r], 0);
                ni.Label = DBUtil.GetString(dt.Rows[r], 1);
                ni.Icon = Encoding.UTF8.GetBytes(DBUtil.GetString(dt.Rows[r], 2));
                ni.SetChildrenNavIDs(DBUtil.GetString(dt.Rows[r], 3));
                ni.Type = DBUtil.GetString(dt.Rows[r], 4);
                ni.Url = DBUtil.GetString(dt.Rows[r], 5);
                nav.navs.Add(ni);
            }

            GlobalMenuItem root = new GlobalMenuItem(0, "", 0, "");
            root.Populate(nav, nav.GetRootChildren("gm-root"));
            return root;
        }

        public static TagBuilder RenderMenu(Object gsis)
        {
            if (gsis == null)
            {
                TagBuilder tb = new TagBuilder("span");
                tb.InnerHtml.AppendHtml("Global menu not defined.");
                return tb;
            }
            else
            {
                GeneralSessionInfo gsi = JsonConvert.DeserializeObject<GeneralSessionInfo>((string)gsis);
                return RenderMenu(gsi);
            }
        }
        public static TagBuilder RenderMenu(GeneralSessionInfo gsi)
        {
            GlobalMenuItem root = LoadFromDB(gsi);
            if (root == null)
            {
                TagBuilder tb = new TagBuilder("span");
                tb.InnerHtml.AppendHtml("Global menu not defined.");
                return tb;
            }
            else
                return root.HtmlText();
        }


    }
}
