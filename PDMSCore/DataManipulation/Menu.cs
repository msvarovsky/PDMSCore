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

        public MenuItem()
        {
            Level = 0;

            MIHeading = new MenuHeading();
            SubMenu = new List<MenuItem>();
            
        }

        public MenuItem GetSubMenu(int index)
        {
            if (index >= SubMenu.Count)
                return null;
            return SubMenu[index];
        }

        public MenuItem GetLastSubMenu()
        {
            return SubMenu[SubMenu.Count - 1];
        }

        public void AddMenuItem(MenuItem md)
        {
            md.Level = Level + 1;
            SubMenu.Add(md);
        }

        private TagBuilder HtmlTextItems(TagBuilder ParentTag)
        {
            TagBuilder tb;
            if (ParentTag != null)
            {
                tb = ParentTag;
            }
            else
            {
                tb = new TagBuilder("div");
                tb.AddCssClass("MenuContent");
            }

            for (int i = 0; i < SubMenu.Count; i++)
                tb.InnerHtml.AppendHtml(SubMenu[i].HtmlText());

            return tb;
        }
        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb;

            if (Level == 0)
            {
                tb = new TagBuilder("aside");
                tb.AddCssClass("Accordion");

                TagBuilder tbMenuItems = HtmlTextItems(tb);
                tb.InnerHtml.AppendHtml(tbMenuItems);
            }
            else
            {
                TagBuilder tbMenuItems = HtmlTextItems(null);
                tb.InnerHtml.AppendHtml(MIHeading.HtmlText(Level));
                tb.InnerHtml.AppendHtml(tbMenuItems);
            }
            return tb;
        }
    }

    public class MenuHeading
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public bool Expanded { get; set; }
        public bool Empty { get; set; }
        public bool Selected { get; set; }

        public MenuHeading()
        {
        }
        public MenuHeading(string Label, string Url, bool Expanded, bool Empty, bool Selected)
        {
            this.Label = Label;
            this.Url = Url;
            this.Expanded = Expanded;
            this.Empty = Empty;
            this.Selected = Selected;
        }

        private TagBuilder TagDivChevronArea()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("MIChevronArea");

            TagBuilder tbMIChevron = new TagBuilder("div");
            tbMIChevron.AddCssClass("MIChevron");
            tbMIChevron.AddCssClass(Expanded ? "MIChevronExpanded" : "");

            tb.InnerHtml.AppendHtml(tbMIChevron);
            return tb;
        }

        public TagBuilder HtmlText(int Level)
        {
            TagBuilder tbOuter = new TagBuilder("div");
            tbOuter.AddCssClass("MenuItemL" + Level.ToString());
            tbOuter.AddCssClass(Expanded ? "MIExpanded" : "");
            tbOuter.AddCssClass(Empty ? "MIEmpty" : "");
            tbOuter.AddCssClass(Selected? "MISelected" : "");

            TagBuilder tbA = new TagBuilder("a");
            tbA.Attributes.Add("href", Url);
            tbA.InnerHtml.AppendHtml(Label);

            TagBuilder tbDivChevronArea = TagDivChevronArea();

            tbOuter.InnerHtml.AppendHtml(tbA);
            tbOuter.InnerHtml.AppendHtml(tbDivChevronArea);
            return tbOuter;
        }
    }


    public class Menu
    {
        /*
            1   1.1     1.1.1
                        1.1.2
                1.2     1.2.1
        */

        MenuItem root { get; set; }

        public Menu()
        {
            root = new MenuItem();
            Test();
        }


        public TagBuilder HtmlText()
        {
            return root.HtmlText();
        }



        public void Test()
        {
            root.AddMenuItem(new MenuItem());                               //1
            root.GetLastSubMenu().AddMenuItem(new MenuItem());                  //1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem());     //1.1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem());     //1.1.2

            root.GetLastSubMenu().AddMenuItem(new MenuItem());                  //1.2
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem());     //1.2.1


            //string aaa = root.HtmlText();

        }

      
    }
}
