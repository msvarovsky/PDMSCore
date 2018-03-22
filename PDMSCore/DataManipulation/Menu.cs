using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PDMSCore.DataManipulation
{
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

    public class MenuItem
    {
        public MenuHeading MIHeading { get; set; }
        public MenuContent MIContent { get; set; }

        public MenuItem()
        {
            MIHeading = new MenuHeading();
            MIContent = new MenuContent();
        }
        public MenuItem(MenuHeading mh, MenuContent mc)
        {
            MIHeading = mh;
            MIContent = mc;
        }

        public void AddMenuItem(MenuItem md)
        {
            MIContent.AddMenuItem(md);
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

        public List<MenuItem> MenuItems { get; set; }
        public MenuContent(int Level=-1)
        {
            ID = -1;
            Label = "not defined";
            Url = "not defined";
            MenuItems = new List<MenuItem>();

            /*MIHeading = new MenuHeading("",Level,);
            MIContent = new List<MenuContent>();*/
        }

        public void AddMenuItem(MenuItem md)
        {
            MenuItems.Add(md);
        }

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

        public List<MenuItem> MIContent { get; set; }

        public Menu()
        {
            MIContent = new List<MenuItem>();

            MIContent.Add(new MenuItem(new MenuHeading(), new MenuContent(1)));                                    //1
            MIContent[0].AddMenuItem(new MenuItem(new MenuHeading(), new MenuContent(2)));                           //1.1
            MIContent[0].MIContent.MenuItems[0].AddMenuItem(new MenuItem(new MenuHeading(), new MenuContent(3)));      //1.1.1
            MIContent[0].MIContent.MenuItems[0].AddMenuItem(new MenuItem(new MenuHeading(), new MenuContent(3)));      //1.1.2

            MIContent[0].AddMenuItem(new MenuItem(new MenuHeading(), new MenuContent(2)));                           //1.2
            MIContent[0].MIContent.MenuItems[1].AddMenuItem(new MenuItem(new MenuHeading(), new MenuContent(3)));      //1.2.1


        }
    }
}
