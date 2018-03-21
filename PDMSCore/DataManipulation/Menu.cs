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





            tbContent.InnerHtml.AppendHtml(HtmlLabel);
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

        //private List<MenuItem> SubMenus { get; set; }

        public MenuHeading MIHeading { get; set; }
        public List<MenuContent> MIContent { get; set; }


        public MenuContent(int Level)
        {
            ID = -1;
            Label = "not defined";
            Url = "not defined";
            MIHeading = new MenuHeading("",Level,);
            MIContent = new List<MenuContent>();
        }

        public void AddSubMenu(MenuContent content)
        {
            MIContent.Add(content);
        }



        public TagBuilder HtmlText()
        {
            MIHeading.HtmlText();
            return null;
        }
    }

    public class Menu
    {

    }
}
