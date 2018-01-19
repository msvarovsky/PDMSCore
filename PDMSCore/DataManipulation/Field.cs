using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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

        public abstract string HtmlText();
    }

    public class LabelField//: IHtmlTag
    {
        public string Text { get; set; }
        public string ToolTip { get; set; }

        public LabelField(string text)
        {
            Text = text;
        }

        public string HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");

            TagBuilder InnerTb = new TagBuilder("div");
            InnerTb.AddCssClass("label");
            InnerTb.InnerHtml.AppendHtml(Text);

            tb.InnerHtml.AppendHtml(InnerTb.ToString());

            return tb.ToString();
        }
    }

    public class TextBoxField : Field//, IHtmlTag
    {
        public LabelField Label { get; set; }

        public string Text { get; set; }
        public string TextHolder { get; set; }
        public string ToolTip { get; set; }


        public TextBoxField(string TagName, string LabelText, string TextBoxText)
        {
            tagName = TagName;
            Label = new LabelField(LabelText);
            Text = TextBoxText;
        }

        public override string HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");

            TagBuilder tb3 = new TagBuilder("input");
            tb3.Attributes.Add("type", "text");
            tb3.Attributes.Add("name", tagName);
            tb3.AddCssClass("CustomTextBox");

            TagBuilder tb2 = new TagBuilder("div");
            tb2.AddCssClass("control");
            tb2.InnerHtml.AppendHtml(tb3.ToString());

            tb.InnerHtml.AppendHtml(Label.HtmlText() + tb2.ToString());

            return tb.ToString();
        }
    }
   
    public class OneCheckBox//: IHtmlTag
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public string HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "checkbox");
            tb.Attributes.Add("name", Name);
            tb.Attributes.Add("value", Text);

            if (Checked)
                tb.Attributes.Add("checked", "checked");
            if(Disabled)
                tb.Attributes.Add("disabled", "disabled");

            return tb.ToString();
        }
    }
    public class CheckBoxField : Field//, IHtmlTag
    {
        public LabelField label { get; set; }
        List<OneCheckBox> CheckBoxes { get; set; }

        public void Add(string text, bool bChecked, bool bDisabled)
        {
            CheckBoxes.Add(new OneCheckBox { Name = tagName, Text = text, Checked = bChecked, Disabled = bDisabled});
        }

        public override string HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");

            TagBuilder tbControl = new TagBuilder("div");
            tbControl.AddCssClass("control");

            TagBuilder tbForm = new TagBuilder("form");
            tbForm.Attributes.Add("action","#");
            tbForm.Attributes.Add("method", "get");


            string htmlCheckBoxes = "";
            for (int i = 0; i < CheckBoxes.Count; i++)
                htmlCheckBoxes += CheckBoxes[i].HtmlText();

            tbForm.InnerHtml.AppendHtml(htmlCheckBoxes);
            tbControl.InnerHtml.AppendHtml(tbForm.ToString());


            tb.InnerHtml.AppendHtml(label.HtmlText() + tbControl.ToString());
            return tb.ToString();
        }
    }


}