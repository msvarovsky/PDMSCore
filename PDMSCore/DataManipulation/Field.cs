using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Net;

namespace PDMSCore.DataManipulation
{
    public enum FieldType
    {
        Label,
        TextBoxNoLabel,
        TextBox,
        DropDownListBox,
        DatePicker,
        CheckBox,
        RadioButton,
        FileUpload,
        FileDownload,
        Indicator
    }

  /*  public interface IHtmlTag
    {
        string HtmlText();
    }   */

    public abstract class Field
    {
        public string tagName { get; set; }
        public FieldType Type { get; set; }
        //public string CssClasses { get; set; }
        protected StringWriter writer;

        public abstract TagBuilder HtmlText();

        public static Field GetRandom()
        {
            return new LabelField("LabelField" + DateTime.Now.ToString());
        }

        public static Field NewLine()
        {
            return new NewLine();
        }

    }

    public class LabelTextAreaField : Field
    {
        /*
            <textarea rows="4" cols="50" placeholder="Describe yourself here..."></textarea>
            <textarea rows="4" cols="50" placeholder="Describe yourself here...">Value</textarea>
        */
        public string LabelText { get; set; }
        public string Text { get; set; }
        public string Placeholder { get; set; }
        public string Rows { get; set; }

        public LabelTextAreaField(string LabelText, string Text, string Placeholder="...", string Rows = "4")
        {
            this.LabelText = LabelText;
            this.Text = Text;
            this.Placeholder = Placeholder;
            this.Rows = Rows;
        }

        public override TagBuilder HtmlText()
        {
            LabelField lf = new LabelField(LabelText);

            TagBuilder tbInner = new TagBuilder("textarea");
            tbInner.Attributes.Add("rows", Rows);
            tbInner.AddCssClass("TextAreaField");
            tbInner.Attributes.Add("placeholder", Placeholder);
            tbInner.InnerHtml.AppendHtml(WebUtility.HtmlEncode(Text));

            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(lf.HtmlText());
            tb.InnerHtml.AppendHtml(tbInner);

            return tb;
        }

        public new static LabelTextAreaField GetRandom()
        {
            LabelTextAreaField a = new LabelTextAreaField("TextArea label", null,"holder", "4");
            return a;
        }
    }

    public class NewLine : Field
    {
        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("/br");
            tb.TagRenderMode = TagRenderMode.SelfClosing;
            return tb;
        }
    }

    public class LabelTextBoxField : Field
    {
        private LabelField l;
        private TextBoxField txtb;

        public LabelTextBoxField()
        {
            l = new LabelField("LabelText");
            txtb = new TextBoxField("", "Sem neco napis.");
        }

        public new static LabelTextBoxField GetRandom()
        {
            LabelTextBoxField a = new LabelTextBoxField();
            a.l = new LabelField("Random label text");
            a.txtb = new TextBoxField("TextBox text", "place holder");
            return a;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(l.HtmlText());
            tb.InnerHtml.AppendHtml(txtb.HtmlText());
            return tb;
        }
    }
    public class LabelField : Field
    {
        public string Text { get; set; }
        public string ToolTip { get; set; }
        public string PlaceHolder { get; set; }

        public LabelField(string Text)
        {
            this.Text = Text;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("label");
            tb.AddCssClass("labelField");
            tb.InnerHtml.AppendHtml(Text);
            return tb;
        }
    }

    public class TextBoxField : Field
    {
        public string Text { get; set; }
        public string PlaceHolder { get; set; }
        public string ToolTip { get; set; }

        public TextBoxField(string Text, string PlaceHolder = "")
        {
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
        }

        //public override IHtmlContent HtmlText()
        //{
        //    TagBuilder tb = new TagBuilder("div");
        //    tb.AddCssClass("LabelControlDuo");

        //    TagBuilder tb3 = new TagBuilder("input");
        //    tb3.Attributes.Add("type", "text");
        //    tb3.Attributes.Add("name", tagName);
        //    tb3.AddCssClass("CustomTextBox");

        //    TagBuilder tb2 = new TagBuilder("div");
        //    tb2.AddCssClass("control");
        //    tb2.InnerHtml.AppendHtml(tb3.ToString());

        //    tb.InnerHtml.AppendHtml(Label.HtmlText() + tb2.ToString());
        //    return tb;
        //}
        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "text");
            tb.AddCssClass("CustomTextBox");
            tb.Attributes.Add("placeholder", PlaceHolder);

            return tb;
        }
    }

    public class OneCheckBox : Field
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "checkbox");
            tb.Attributes.Add("name", Name);
            tb.Attributes.Add("value", Text);

            if (Checked)
                tb.Attributes.Add("checked", "checked");
            if (Disabled)
                tb.Attributes.Add("disabled", "disabled");

            return tb;
        }
    }

    public class CheckBoxField : Field
    {
        public LabelField label { get; set; }
        List<OneCheckBox> CheckBoxes { get; set; }

        public void Add(string text, bool bChecked, bool bDisabled)
        {
            CheckBoxes.Add(new OneCheckBox { Name = tagName, Text = text, Checked = bChecked, Disabled = bDisabled });
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");

            TagBuilder tbControl = new TagBuilder("div");
            tbControl.AddCssClass("control");

            TagBuilder tbForm = new TagBuilder("form");
            tbForm.Attributes.Add("action", "#");
            tbForm.Attributes.Add("method", "get");

            string htmlCheckBoxes = "";
            for (int i = 0; i < CheckBoxes.Count; i++)
                htmlCheckBoxes += CheckBoxes[i].HtmlText();

            tbForm.InnerHtml.AppendHtml(htmlCheckBoxes);
            tbControl.InnerHtml.AppendHtml(tbForm.ToString());

            tb.InnerHtml.AppendHtml(label.HtmlText() + tbControl.ToString());
            return tb;
        }
    }

    
}