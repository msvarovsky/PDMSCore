using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.BusinessObjects;

namespace PDMSCore.DataManipulation
{
    public class MenuItem
    {
        private int NavID { get; set; }
        public MenuHeading MIHeading { get; set; }
        private List<MenuItem> SubMenu { get; set; }
        public int Level { get; set; }

        public MenuItem(int ID, NavFromDB nav, int level)
        {
            Level = level;
            NavItemFromDB ni = nav.GetNavID(ID);
            ni.Udane = true;

            Init(ID, ni.Label, ni.Url);
            for (int i = 0; i < ni.ChildrenNavIDs.Count; i++)
            {
                int ChildID = -1;
                if (Int32.TryParse(ni.ChildrenNavIDs[i], out ChildID))
                {
                    MenuItem nmi = new MenuItem(ChildID, nav, level+1);
                    AddMenuItem(nmi);
                }
            }

        }
        public MenuItem(int ID, string Label, string Url)
        {
            Init(ID, Label, Url);
        }
        private void Init(int ID, string Label, string Url)
        {
            NavID = ID;
            MIHeading = new MenuHeading(Label, Url);
            SubMenu = new List<MenuItem>();
        }

        public MenuItem GetSubMenu(int index)
        {
            if (index >= SubMenu.Count)
                return null;
            return SubMenu[index];
        }

        private bool Find(string Url, string Label, ref List<int> p)
        {
            if (MIHeading.Url == Url && (Label == null || MIHeading.Label == Label)  )
                return true;
            else
            {
                for (int i = 0; i < SubMenu.Count; i++)
                {
                    if (SubMenu[i].Find(Url, Label, ref p))
                    {
                        p.Add(i);
                        return true;
                    }
                }
                return false;
            }
        }
        private void Select(List<int> MenuPath, int index)
        {
            if (index >= 0)
                SubMenu[MenuPath[index]].Select(MenuPath, index - 1);

            if (index == -1)
                MIHeading.Selected = true;
            if (SubMenu.Count > 0)
                MIHeading.Expanded = true;
        }
        public void Select(string Url, string Label=null)
        {
            List<int> MenuPath = new List<int>();
            Find(Url, Label, ref MenuPath);
            Select(MenuPath, MenuPath.Count-1);
        }

        public void UnselectAll()
        {
            this.MIHeading.Selected = false;
            this.MIHeading.Expanded = false;
            for (int i = 0; i < SubMenu.Count; i++)
                SubMenu[i].UnselectAll();
        }

        public MenuItem GetLastSubMenu()
        {
            return SubMenu[SubMenu.Count - 1];
        }

        public void AddMenuItem(MenuItem md)
        {
            MIHeading.Empty = false;
            md.Level = Level + 1;
            SubMenu.Add(md);
        }

        private TagBuilder HtmlTextItems(TagBuilder ParentTag)
        {
            TagBuilder tb;
            if (ParentTag != null)
                tb = ParentTag;
            else
            {
                tb = new TagBuilder("div");
                tb.AddCssClass("MenuContent");
                tb.Attributes.Add("display", MIHeading.Expanded ? "block": "none");
            }

            for (int i = 0; i < SubMenu.Count; i++)
                SubMenu[i].HtmlText(tb);

            return tb;
        }

        public TagBuilder HtmlText(TagBuilder tb = null)
        {
            if (Level == 0)
            {
                tb = new TagBuilder("aside");
                tb.AddCssClass("Accordion");
                HtmlTextItems(tb);
            }
            else
            {
                tb.InnerHtml.AppendHtml(MIHeading.HtmlText(Level));
                tb.InnerHtml.AppendHtml(HtmlTextItems(null));
            }
            return tb;
        }

        //public static string GetString(IHtmlContent content)
        //{
        //    var writer = new System.IO.StringWriter();
        //    content.WriteTo(writer, HtmlEncoder.Default);
        //    return writer.ToString();
        //}
    }

    public class MenuHeading
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public bool Expanded { get; set; }
        public bool Empty { get; set; }
        public bool Selected { get; set; }

        public MenuHeading(string Label, string Url)
        {
            this.Label = Label;
            this.Url = Url;

            this.Expanded = false;
            this.Empty = true;
            this.Selected = false;
        }

        private TagBuilder TagDivChevronArea()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("MIChevronArea");
            tb.Attributes.Add(WebStuffHelper.CreateJSParameter("onclick", "OnNavItemClick", "this", "MICA", Url, ""));

            TagBuilder tbMIChevron = new TagBuilder("div");
            tbMIChevron.AddCssClass("MIChevron");
            tbMIChevron.Attributes.Add(WebStuffHelper.CreateJSParameter("onclick", "OnNavItemClick", "this", "MIC", Url, ""));
            AddCssClass(tbMIChevron, Expanded, "MIChevronExpanded");


            tb.InnerHtml.AppendHtml(tbMIChevron);
            return tb;
        }

        private void AddCssClass(TagBuilder tb, bool Condition, string ClassString)
        {
            if (Condition)
                tb.AddCssClass(ClassString);
        }

        public TagBuilder HtmlText(int Level)
        {
            TagBuilder tbOuter = new TagBuilder("div");
            
            /* MenuItemLx musi byt pred MIExpanded. Aby se toho docililo, tak sa do tbOuter musi pridat az po MIExpanded.*/
            AddCssClass(tbOuter, Selected, "MISelected");
            AddCssClass(tbOuter, Empty, "MIEmpty");
            AddCssClass(tbOuter, Expanded, "MIExpanded");

            //
            //tbOuter.Attributes.Add(WebStuffHelper.CreateJSParameter("onclick", "ReloadPageContent", "Project/ShowProject", "dataDoMVC"));
            tbOuter.Attributes.Add(WebStuffHelper.CreateJSParameter("onclick", "OnNavItemClick", "this", "MIL", Url, ""));

            /*tbOuter.Attributes.Add("onclick", "onNavigationItemClick('" +   HttpUtility.JavaScriptStringEncode("ProjectController/ShowProject") +
                                                                            HttpUtility.JavaScriptStringEncode("ahoj") + "')");*/
            //
            tbOuter.AddCssClass("MenuItemL" + Level.ToString());

            //TagBuilder tbA = new TagBuilder("a");
            TagBuilder tbA = new TagBuilder("div");
            //tbA.Attributes.Add("href", Url);
            tbA.AddCssClass("MenuItemText");
            tbA.InnerHtml.AppendHtml(Label);
            tbA.Attributes.Add(WebStuffHelper.CreateJSParameter("onclick", "OnNavItemClick", "this", "MIT", Url, ""));


            tbOuter.InnerHtml.AppendHtml(tbA);
            if (!Empty)
                tbOuter.InnerHtml.AppendHtml(TagDivChevronArea());
            return tbOuter;
        }
    }

    public class NavItemFromDB
    {
        public bool Udane { get; set; }
        public int NavID { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public int ParentNavID { get; set; }
        public List<string> ChildrenNavIDs { get; set; }
        public byte[] Icon { get; set; }

        public NavItemFromDB()
        {
            Udane = false;
            ChildrenNavIDs = new List<string>();
        }

        public void SetChildrenNavIDs(string par)
        {
            if (par == "")
            {
                ChildrenNavIDs = new List<string>();
                return;
            }
            else
            {
                par = par.Replace(")(", "|");
                par = par.Trim('(').Trim(')');

                string[] a = par.Trim().Split('|', StringSplitOptions.RemoveEmptyEntries);
                ChildrenNavIDs = new List<string>(a);
            }
        }

    }
    public class NavFromDB
    {
        public List<NavItemFromDB> navs { get; set; }

        public NavFromDB()
        {
            navs = new List<NavItemFromDB>();
        }

        public NavItemFromDB GetParentOf(int c)
        {
            return null;
        }

        public NavItemFromDB GetNavID(int id)
        {
            for (int i = 0; i < navs.Count; i++)
            {
                if (navs[i].Udane)
                    continue;

                if (navs[i].NavID == id)
                    return navs[i];
            }
            return null;
        }
    }
    

    public class Menu
    {   /*  1   1.1     1.1.1
                        1.1.2
                1.2     1.2.1        */
        public int NavID { get; set; }
        private MenuItem root { get; set; }

        public Menu()
        {
            root = new MenuItem(-1, "root","root");
        }

        public TagBuilder HtmlText()
        {
            //return new TagBuilder("testtag");
            return root.HtmlText();
        }

        public void GetRandomMenu()
        {
            root.AddMenuItem(new MenuItem(1,"1","#1"));                                   //1
            root.GetLastSubMenu().AddMenuItem(new MenuItem(11,"1.1","#1.1"));                      //1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem(111,"1.1.1", "#1.1.1"));     //1.1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem(112,"1.1.2", "#1.1.2"));     //1.1.2

            root.GetLastSubMenu().AddMenuItem(new MenuItem(12,"1.2", "#1.2"));                     //1.2
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem(121, "1.2.1", "#1.2.1"));     //1.2.1

            root.AddMenuItem(new MenuItem(2, "2", "#2"));                                   //2
            root.GetLastSubMenu().AddMenuItem(new MenuItem(21, "2.1", "#2.1"));                      //1.1
            root.GetLastSubMenu().AddMenuItem(new MenuItem(22, "2.2", "#2.2"));                      //1.1

            //root.Select("#1.1.2", "1.1.2");
            root.Select("#1.1.2");
        }

        public void Select(string path)
        {
            if (path == null)
                return;
            root.UnselectAll();
            root.Select(path);
        }

        public bool LoadNavigation(GeneralSessionInfo gsi)
        {
            int RootNavigationItemID = 6;
            NavID = RootNavigationItemID;

            using (SqlConnection con = new SqlConnection(DBUtil.GetSqlConnectionString()))
            {
                con.Open();

                SqlDataAdapter sqlDataAdapter;
                DataSet dataSet = new DataSet();

                List<Field> ret = new List<Field>();
                SqlCommand sql = new SqlCommand("GetNavigation", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.Add(new SqlParameter("NavID", RootNavigationItemID));
                sql.Parameters.Add(new SqlParameter("LanguageID", gsi.languageID));
                sql.Parameters.Add(new SqlParameter("UserID", gsi.userID));

                try
                {
                    sqlDataAdapter = new SqlDataAdapter(sql);
                    sqlDataAdapter.Fill(dataSet);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                        ProcessNavigation(dataSet.Tables[0]);
                    else
                    {
                        //Page.GenerateUnknownPageInfo();
                        Console.WriteLine("No matching records found.");
                    }
                }
                catch (Exception eee)
                {
                    ret.Add(new LabelTextAreaField("1", "Exception in LoadNavigation(..)", eee.ToString()));
                }
                return false;
            }
        }

        public void ProcessNavigation(DataTable dt)
        {
            NavFromDB nav = new NavFromDB();

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                NavItemFromDB ni = new NavItemFromDB();
                ni.NavID = DBUtil.GetInt(dt.Rows[r],0);
                ni.Label = DBUtil.GetString(dt.Rows[r], 1);
                ni.Url = DBUtil.GetString(dt.Rows[r], 2);
                ni.ParentNavID = DBUtil.GetInt(dt.Rows[r], 3);
                ni.Icon = Encoding.UTF8.GetBytes(DBUtil.GetString(dt.Rows[r], 4));
                ni.SetChildrenNavIDs(DBUtil.GetString(dt.Rows[r], 5));
                nav.navs.Add(ni);
            }

            root = new MenuItem(NavID, nav, 0);    // Reconstruct Navigation
            root.Select("ShowProject");
        }
    }
}
