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

    public interface IHtmlTag
    {
        TagBuilder HtmlText();
    }
    public abstract class Field
    {
        public FieldType Type { get; set; }
        //public string CssClasses { get; set; }

        public abstract TagBuilder HtmlText();

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
            LabelField lf = new LabelField(LabelText,true);

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

        public static Field GetRandom()
        {
            LabelTextAreaField a = new LabelTextAreaField("TextArea label", null, "holder", "4");
            return a;
        }
    }

    public class NewLine : Field
    {
        public static Field GetRandom()
        {
            return null;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("/br");
            tb.TagRenderMode = TagRenderMode.SelfClosing;
            return tb;
        }
    }

    public class LabelTextBoxField : Field
    {
        private LabelField label;
        private TextBoxField txtb;

        public LabelTextBoxField()
        {
            label = new LabelField("LabelText");
            txtb = new TextBoxField("", "Sem neco napis.");
        }

        public static Field GetRandom()
        {
            LabelTextBoxField a = new LabelTextBoxField();
            a.label = new LabelField("Random label text",true);
            a.txtb = new TextBoxField("TextBox text", "place holder");
            return a;
        }
        
        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(txtb.HtmlText());
            return tb;
        }
    }
    public class LabelField : Field
    {
        public string HtmlLabel { get; set; }
        public string For { get; set; }

        public string ToolTip { get; set; }
        public string PlaceHolder { get; set; }
        public bool Bold { get; set; }

        public LabelField(string HtmlLabel, bool Bold = false, string For=null)
        {
            this.Bold = Bold;
            this.HtmlLabel = HtmlLabel;
            this.For = For;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("label");
            tb.AddCssClass(Bold? "BoldLabelField" : "LabelField");
            if (this.For != null)
                tb.Attributes.Add("for", this.For);
            tb.InnerHtml.AppendHtml(HtmlLabel);
            return tb;
        }

        public static Field GetRandom()
        {
            return new LabelField("LabelField");
        }
    }
    public class TextBoxField : Field
    {
        public string Text { get; set; }
        public string PlaceHolder { get; set; }
        public string ToolTip { get; set; }

        public TextBoxField(string Text, string PlaceHolder = "", string ToolTip="")
        {
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
            this.ToolTip = ToolTip;
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
            tb.AddCssClass("CustomTextBox");
            tb.Attributes.Add("type", "text");
            tb.Attributes.Add("title", ToolTip);
            tb.Attributes.Add("placeholder", PlaceHolder);
            return tb;
        }
    }

    public class FileUploadField : Field
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }

        public FileUploadField(string NameId, string ToolTip = "")
        {
            this.Name = NameId;
            this.ToolTip = ToolTip;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.AddCssClass("CustomTextBox");
            tb.Attributes.Add("name", this.Name);
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("title", ToolTip);
            return tb;
        }
    }
    public class LabelFileUploadField : Field
    {
        private LabelField label;
        private FileUploadField fu;

        public LabelFileUploadField(string HtmlText, FileUploadField f)
        {
            label = new LabelField(HtmlText,true);
            fu = f;
        }

        public static Field GetRandom()
        {
            LabelFileUploadField a = new LabelFileUploadField("fu",new FileUploadField("fuu"));
            return a;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(fu.HtmlText());
            return tb;
        }
    }

    public class DatePickerField : IHtmlTag
    {
        //  <input type="date" name="bday" value="2013-01-08"> disabled>
        public string Name { get; set; }
        public DateTime? Value { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool Enabled { get; set; }

        public DatePickerField(string Id, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null, bool Enabled = true)
        {
            this.Name = Id;
            this.Value = Value;
            this.MinDate = MinDate;
            this.MaxDate = MaxDate;
            this.Enabled = Enabled;
        }
        //public static Field GetRandom()
        //{
        //    //DatePickerField n = new DatePickerField("DP Label", DateTime.Now, DateTime.Now, DateTime.Now + TimeSpan.FromDays(20));
        //    DatePickerField n = new DatePickerField("DP Label");
        //    return n;
        //}
        public TagBuilder HtmlText()
        {   
            TagBuilder tb = new TagBuilder("input");
            tb.AddCssClass("CustomDatePicker");
            tb.Attributes.Add("type", "date");
            tb.Attributes.Add("name", this.Name);

            if (this.Value != null)
                tb.Attributes.Add("value", this.Value.Value.ToString("yyyy-MM-dd"));
            if (this.MinDate != null)
                tb.Attributes.Add("min", this.MinDate.Value.ToString("yyyy-MM-dd"));
            if (this.MaxDate!= null)
                tb.Attributes.Add("max", this.MaxDate.Value.ToString("yyyy-MM-dd"));

            if (!Enabled)
                tb.Attributes.Add("disabled", "");
            return tb;
        }
    }
    public class LabelDatePickerField : Field
    {   //    <input type="date" name="bday">
        private LabelField Label;
        private DatePickerField DatePicker;

        public LabelDatePickerField(string HtmlLabel, DatePickerField date)
        {
            this.Label = new LabelField(HtmlLabel,true);
            this.DatePicker = date;
        }

        public static Field GetRandom(DateTime? DefaultValue = null)
        {
            DatePickerField d = new DatePickerField("DP Label",DefaultValue);
            LabelDatePickerField a = new LabelDatePickerField("DP Label", d);
            return a;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(Label.HtmlText());
            tb.InnerHtml.AppendHtml(DatePicker.HtmlText());
            return tb;
        }
    }

    public class DropDownOption : IHtmlTag
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public bool Enabled { get; set; }

        public DropDownOption(string ValueId, string Label, bool Enabled = true)
        {
            this.Value = ValueId;
            this.Label = Label;
            this.Enabled = Enabled;
        }

        public TagBuilder HtmlText()
        {   //  <option label="England" value="England"></option>             TagBuilder tb = new TagBuilder("option");
            tb.Attributes.Add("value", Value);
            if (!Enabled)
                tb.Attributes.Add("disabled", "");
            tb.InnerHtml.AppendHtml(Label);
            return tb;
        }
    }
    public class LabelDropDownField : Field
    {
        public LabelField Label { get; set; }
        List<DropDownOption> Options { get; set; }
        public int Size { get; set; }
        public bool Enabled { get; set; }

        public LabelDropDownField(string label, int VisibleRows = 1, bool Enabled = true)
        {
            this.Label = new LabelField(label, true);
            Options = new List<DropDownOption>();
            Size = VisibleRows;
            this.Enabled = Enabled;
        }

        public void Add(DropDownOption toBeAdded)
        {
            Options.Add(toBeAdded);
        }

        public static Field GetRandom(int count)
        {
            LabelDropDownField n = new LabelDropDownField("DropDown ListBox", count - 1, true);
            for (int i = 0; i < count; i++)
            {
                DropDownOption no = new DropDownOption("DD" + i.ToString(), "DD" + i.ToString(), i % 2 == 0);
                n.Add(no);
            }
            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
                TagBuilder tbDropDown = new TagBuilder("select");
                tbDropDown.AddCssClass("DropDownOptions");
                if (Size > 1)
                    tbDropDown.Attributes.Add("size", Size.ToString());
                if (!Enabled)
                    tbDropDown.Attributes.Add("disabled", "");

                    for (int i = 0; i < Options.Count; i++)
                    {
                        TagBuilder t = Options[i].HtmlText();
                        tbDropDown.InnerHtml.AppendHtml(t);
                    }
            tb.InnerHtml.AppendHtml(Label.HtmlText());
            tb.InnerHtml.AppendHtml(tbDropDown);
            return tb;
        }
    }

    public class LabelCheckBoxField : Field
    {
        public LabelField Label { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public LabelCheckBoxField(string GroupName, string HtmlLabel, string IndividualValueId, bool Checked = false, bool Disabled = false)
        {
            this.Name = GroupName;
            this.Id = IndividualValueId;
            this.Value = IndividualValueId;
            this.Label = new LabelField(HtmlLabel, false, IndividualValueId);
            this.Checked = Checked;
            this.Disabled = Disabled;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tbLabelCheckBox = new TagBuilder("div");

                TagBuilder tbCheckBox = new TagBuilder("input");
                tbCheckBox.Attributes.Add("type", "checkbox");
                tbCheckBox.Attributes.Add("name", Name);
                tbCheckBox.Attributes.Add("id", Id);
                tbCheckBox.Attributes.Add("value", Value);
                //tbCheckBox.Attributes.Add("value", Label);
                if (Checked)
                    tbCheckBox.Attributes.Add("checked", "checked");
                if (Disabled)
                    tbCheckBox.Attributes.Add("disabled", "disabled");

                TagBuilder tbLabel = Label.HtmlText();

            tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
            tbLabelCheckBox.InnerHtml.AppendHtml(Label.HtmlText());
            return tbLabelCheckBox;
        }

        public static Field GetRandom()
        {
            throw new NotImplementedException();
        }
    }
    //public class LabelCheckBoxesControl : Field
    //{
    //    public LabelField Label { get; set; }
    //    public string Name { get; set; }

    //    List<LabelCheckBoxField> CheckBoxes { get; set; }

    //    public LabelCheckBoxesControl(string Id, string HtmlLabel)
    //    {
    //        this.Name = Id;
    //        this.Label = new LabelField(HtmlLabel, true);
    //        CheckBoxes = new List<LabelCheckBoxField>();
    //    }

    //    //public void Add(string HtmlLabel, string ValueId, bool bChecked, bool bDisabled)
    //    public void Add(LabelCheckBoxField lcbf)
    //    {
    //        LabelCheckBoxField cb = lcbf;
    //        CheckBoxes.Add(cb);
    //    }

    //    public static Field GetRandom(int count)
    //    {
    //        string GroupName = "lcbc" + DateTime.Now.ToShortDateString();
    //        LabelCheckBoxesControl c = new LabelCheckBoxesControl(GroupName,"Checkbox global label");
    //        for (int i = 0; i < count; i++)
    //        {
    //            LabelCheckBoxField cb = new LabelCheckBoxField(GroupName, "LCBC global label", GroupName + i.ToString(), i % 2 == 0, false);
    //            c.CheckBoxes.Add(cb);
    //        }
    //        return c;
    //    }

    //    public override TagBuilder HtmlText()
    //    {
    //        TagBuilder tb = new TagBuilder("div");
    //        tb.AddCssClass("LabelControlDuo");

    //            /*TagBuilder tbControlLabel = new TagBuilder("div");
    //            tbControlLabel.InnerHtml.AppendHtml(label.HtmlText());*/

    //            TagBuilder tbControl = new TagBuilder("div");
    //            tbControl.AddCssClass("GroupOfCheckBoxes");

    //                TagBuilder tbForm = new TagBuilder("form");
    //                tbForm.Attributes.Add("action", "#");
    //                tbForm.Attributes.Add("method", "get");

    //                //TagBuilder tbCheckboxes = new TagBuilder("div");
    //                for (int i = 0; i < CheckBoxes.Count; i++)
    //                    tbForm.InnerHtml.AppendHtml(CheckBoxes[i].HtmlText());
                    
    //                //tbForm.InnerHtml.AppendHtml(tbCheckboxes);

    //            tbControl.InnerHtml.AppendHtml(tbForm);

    //        tb.InnerHtml.AppendHtml(Label.HtmlText());
    //        tb.InnerHtml.AppendHtml(tbControl);
    //        return tb;
    //    }
    //}
    public class LabelRadioButtonField : Field
    {
        public LabelField Label { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public LabelRadioButtonField(string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false)
        {
            this.Name = Name;
            this.Id = IndividualValueId;
            this.Value = IndividualValueId;
            this.Label = new LabelField(Label, false, Name);
            this.Checked = Checked;
            this.Disabled = Disabled;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tbLabelCheckBox = new TagBuilder("div");

            TagBuilder tbCheckBox = new TagBuilder("input");
            tbCheckBox.Attributes.Add("type", "radio");
            tbCheckBox.Attributes.Add("name", Name);
            tbCheckBox.Attributes.Add("id", Id);
            tbCheckBox.Attributes.Add("value", Value);
            //tbCheckBox.Attributes.Add("value", Label);
            if (Checked)
                tbCheckBox.Attributes.Add("checked", "checked");
            if (Disabled)
                tbCheckBox.Attributes.Add("disabled", "disabled");

            TagBuilder tbLabel = Label.HtmlText();

            tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
            tbLabelCheckBox.InnerHtml.AppendHtml(Label.HtmlText());
            return tbLabelCheckBox;
        }

        public static Field GetRandom()
        {
            throw new NotImplementedException();
        }
    }
    //public class LabelRadioButtonsControl : Field
    //{
    //    public LabelField label { get; set; }
    //    List<LabelRadioButtonField> RadioButtons { get; set; }

    //    public LabelRadioButtonsControl(string label)
    //    {
    //        this.label = new LabelField(label, true);
    //        RadioButtons = new List<LabelRadioButtonField>();
    //    }

    //    public void Add(string text, bool bChecked, bool bDisabled)
    //    {
    //        LabelRadioButtonField cb = new LabelRadioButtonField(tagName, text,"error", bChecked, bDisabled);
    //        RadioButtons.Add(cb);
    //    }

    //    public static Field GetRandom(int count)
    //    {
    //        LabelRadioButtonsControl c = new LabelRadioButtonsControl("Radiobuttons global label");
    //        for (int i = 0; i < count; i++)
    //        {
    //            LabelRadioButtonField cb = new LabelRadioButtonField("cb" + i, "Label cb" + i, "Label cb" + i, i % 2 == 0, false);
    //            c.RadioButtons.Add(cb);
    //        }
    //        return c;
    //    }

    //    public override TagBuilder HtmlText()
    //    {
    //        TagBuilder tb = new TagBuilder("div");
    //        tb.AddCssClass("LabelControlDuo");

    //        /*TagBuilder tbControlLabel = new TagBuilder("div");
    //        tbControlLabel.InnerHtml.AppendHtml(label.HtmlText());*/

    //        TagBuilder tbControl = new TagBuilder("div");
    //        tbControl.AddCssClass("GroupOfCheckBoxes");

    //        TagBuilder tbForm = new TagBuilder("form");
    //        tbForm.Attributes.Add("action", "#");
    //        tbForm.Attributes.Add("method", "get");

    //        //TagBuilder tbCheckboxes = new TagBuilder("div");
    //        for (int i = 0; i < RadioButtons.Count; i++)
    //            tbForm.InnerHtml.AppendHtml(RadioButtons[i].HtmlText());
            
    //        //tbForm.InnerHtml.AppendHtml(tbCheckboxes);

    //        tbControl.InnerHtml.AppendHtml(tbForm);

    //        tb.InnerHtml.AppendHtml(label.HtmlText());
    //        tb.InnerHtml.AppendHtml(tbControl);
    //        return tb;
    //    }
    //}
    enum GroupControlType
    {
        RadioBoxes,
        CheckBoxes
    };
    public class LabelRBCBControl<T> : Field
    {
        public LabelField Label { get; set; }
        List<T> Options { get; set; }
        private GroupControlType GCType { get; set; }

        public LabelRBCBControl(string label)
        {
            this.Label = new LabelField(label, true);
            Options = new List<T>();
            GCType = (typeof(T) == typeof(LabelCheckBoxField)) ? GroupControlType.CheckBoxes : GroupControlType.RadioBoxes;
        }

        public void Add(T toBeAdded)
        {
            Options.Add(toBeAdded);
        }

        public static Field GetRandom(int count)
        {
            LabelRBCBControl<T> n = new LabelRBCBControl<T>("control global label");
            for (int i = 0; i < count; i++)
            {
                string Name = typeof(T).ToString() + DateTime.Now.ToLongTimeString();
                //          string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false
                T aa = (T)Activator.CreateInstance(typeof(T), Name, Name + i.ToString(), Name + i.ToString(), false, false);
                n.Options.Add(aa);
            }
            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");

            TagBuilder tbControl = new TagBuilder("div");

            if (GCType == GroupControlType.CheckBoxes)
                tbControl.AddCssClass("GroupOfCheckBoxes");
            else if (GCType == GroupControlType.RadioBoxes)
                tbControl.AddCssClass("GroupOfRadioButtons");
            else
                return new TagBuilder("error");

            TagBuilder tbForm = new TagBuilder("form");
            tbForm.Attributes.Add("action", "#");
            tbForm.Attributes.Add("method", "get");

            for (int i = 0; i < Options.Count; i++)
            {
                TagBuilder t = (TagBuilder) Options[i].GetType().GetMethod("HtmlText").Invoke(Options[i], null);
                tbForm.InnerHtml.AppendHtml(t);
            }

            tbControl.InnerHtml.AppendHtml(tbForm);

            tb.InnerHtml.AppendHtml(Label.HtmlText());
            tb.InnerHtml.AppendHtml(tbControl);
            return tb;
        }
    }


}