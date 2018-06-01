using System.Collections.Generic;
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

        //private bool AuthUser()
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

            menu.items.Add(new PanelMenuItem(true, "Refresh", "0", PanelID));
            menu.items.Add(new PanelMenuItem(true, "Save", "1", PanelID));

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