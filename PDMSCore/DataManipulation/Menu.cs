using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace PDMSCore.DataManipulation
{
    public class MenuItem
    {
        public MenuHeading MIHeading { get; set; }
        private List<MenuItem> SubMenu { get; set; }
        public int Level { get; set; }

        public MenuItem(string Label, string Url)
        {
            Level = 0;

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
                //tb.InnerHtml.AppendHtml();
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

            TagBuilder tbMIChevron = new TagBuilder("div");
            tbMIChevron.AddCssClass("MIChevron");
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
            tbOuter.AddCssClass("MenuItemL" + Level.ToString());

            TagBuilder tbA = new TagBuilder("a");
            tbA.Attributes.Add("href", Url);
            tbA.AddCssClass("MenuItemText");
            tbA.InnerHtml.AppendHtml(Label);


            tbOuter.InnerHtml.AppendHtml(tbA);
            if (!Empty)
                tbOuter.InnerHtml.AppendHtml(TagDivChevronArea());
            return tbOuter;
        }
    }

    public class Menu
    {   /*  1   1.1     1.1.1
                        1.1.2
                1.2     1.2.1        */

        private MenuItem root { get; set; }

        public Menu()
        {
            root = new MenuItem("root","root");
        }

        public TagBuilder HtmlText()
        {
            //return new TagBuilder("testtag");
            return root.HtmlText();
        }

        public void GetRandomMenu()
        {
            root.AddMenuItem(new MenuItem("1","#1"));                                   //1
            root.GetLastSubMenu().AddMenuItem(new MenuItem("1.1","#1.1"));                      //1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem("1.1.1", "#1.1.1"));     //1.1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem("1.1.2", "#1.1.2"));     //1.1.2

            root.GetLastSubMenu().AddMenuItem(new MenuItem("1.2", "#1.2"));                     //1.2
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem("1.2.1", "#1.2.1"));     //1.2.1

            root.AddMenuItem(new MenuItem("2", "#2"));                                   //2
            root.GetLastSubMenu().AddMenuItem(new MenuItem("2.1", "#2.1"));                      //1.1
            root.GetLastSubMenu().AddMenuItem(new MenuItem("2.2", "#2.2"));                      //1.1

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
      
    }
}
