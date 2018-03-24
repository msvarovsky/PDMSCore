using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PDMSCore.DataManipulation
{
    public class MenuItem
    {
        public MenuHeading MIHeading { get; set; }
        public MenuContent MIContent { get; set; }
        public int Level { get; set; }

        public MenuItem()
        {
            MIHeading = new MenuHeading();
            MIContent = new MenuContent();
            Level = 0;
        }
        public MenuItem(MenuHeading mh, MenuContent mc)
        {
            MIHeading = mh;
            MIContent = mc;
        }

        public MenuItem GetSubMenu(int index)
        {
            return MIContent.GetMenuItem(index);
        }
        public MenuItem GetLastSubMenu()
        {
            return MIContent.GetLastMenuItem();
        }




        public void AddMenuItem(MenuItem md)
        {
            md.Level = Level + 1;
            MIContent.AddMenuItem(md);
        }

    }

    public class MenuHeading
    {
        public string Label { get; set; }
        public int Level { get; set; }
        public string Url { get; set; }
        public bool Expanded { get; set; }
        public bool Empty { get; set; }
        public bool Selected { get; set; }

        public MenuHeading()
        {
        }
        public MenuHeading(string Label, int Level, string Url, bool Expanded, bool Empty, bool Selected)
        {
            this.Label = Label;
            this.Level = Level;
            this.Url = Url;
            this.Expanded = Expanded;
            this.Empty = Empty;
            this.Selected = Selected;
        }

        public TagBuilder TagA(string Classes, string InnerHtml)
        {
            TagBuilder tb = new TagBuilder("a");
            tb.AddCssClass(Classes);
            tb.Attributes.Add("href", Url);
            tb.InnerHtml.AppendHtml(InnerHtml);
            return tb;
        }

        public TagBuilder TagDiv(string Classes, string InnerHtml)
        {
            TagBuilder tb = new TagBuilder("a");
            tb.AddCssClass(Classes);
            tb.InnerHtml.AppendHtml(InnerHtml);
            return tb;
        }

        public TagBuilder TagDivChevronArea()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("MIChevronArea");

            TagBuilder tbMIChevron = new TagBuilder("div");
            tbMIChevron.AddCssClass("MIChevron");
            tbMIChevron.AddCssClass(Expanded ? "MIChevronExpanded" : "");

            tb.InnerHtml.AppendHtml(tbMIChevron);
            return tb;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tbContent = new TagBuilder("div");

            tbContent.AddCssClass(Expanded ? "MIExpanded" : "");
            tbContent.AddCssClass(Empty ? "MIEmpty" : "");
            tbContent.AddCssClass(Selected? "MISelected" : "");

            TagBuilder tbA = TagA("MenuItemText", Label);
            TagBuilder tbDivChevronArea = TagDivChevronArea();

            TagBuilder tbDivMIChevron = TagDiv("MIChevron", "");

            return tbContent;
        }
    }
    
    public class MenuContent
    {
        public int ID { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public bool Expanded { get; set; }
        public bool Empty { get; set; }
        public bool Selected { get; set; }

        private int Level { get; set; }

        private List<MenuItem> MenuItems { get; set; }
        public MenuContent(int Level = 0)
        {
            ID = -1;
            Label = "not defined";
            Url = "not defined";
            MenuItems = new List<MenuItem>();

            /*MIHeading = new MenuHeading("",Level,);
            MIContent = new List<MenuContent>();*/
        }

        public MenuItem GetMenuItem(int index)
        {
            return MenuItems[index];
        }

        public MenuItem GetLastMenuItem()
        {
            return MenuItems[MenuItems.Count-1];
        }

        public void AddMenuItem(MenuItem md)
        {
            md.Level = this.Level + 1;

            MenuItems.Add(md);
        }
    /*
        public void AddMenuItem(int index, MenuItem md, int level)
        {
            md.Level = level;
            MenuItems.Add(md);
        }*/

        public TagBuilder HtmlText()
        {
            return null;
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

        public void Test()
        {
            root.AddMenuItem(new MenuItem());                               //1
            root.GetLastSubMenu().AddMenuItem(new MenuItem());                  //1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem());     //1.1.1
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem());     //1.1.2

            root.GetLastSubMenu().AddMenuItem(new MenuItem());                  //1.2
            root.GetLastSubMenu().GetLastSubMenu().AddMenuItem(new MenuItem());     //1.2.1
        }

      
    }
}
