﻿using Microsoft.AspNetCore.Hosting.Server;
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

    //public interface IHtmlTag
    //{
    //    TagBuilder HtmlText();
    //}
    public abstract class Field
    {
        public string NameId { get; set; }
        //public FieldType Type { get; set; }

        public string Tag { get; set; }

        public abstract TagBuilder HtmlText();

        public static Field NewLine()
        {
            return new NewLine();
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


    public class TableCell: Field
    {
        ColumnType type;
        public string HtmlLabel { get; set; }
        private Field CellField { get; set; }

        public TableCell(ColumnType Type, Field f)
        {
            this.type = Type;
            this.CellField = f;
            this.Tag = "div";
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder(this.Tag);

            if (type == ColumnType.Header)
            {
                tb.AddCssClass("HeadCell");

                TagBuilder tbHeaderLabel = new TagBuilder("HeaderLabel");
                tbHeaderLabel.InnerHtml.AppendHtml(HtmlLabel);

                TagBuilder tbHeaderSearch = new TagBuilder("HeaderSearch");
                TextBoxField t = new TextBoxField("TODO-NameId", "", "...");
                tbHeaderSearch.InnerHtml.AppendHtml(t.HtmlText());

                tb.InnerHtml.AppendHtml(tbHeaderLabel);
                tb.InnerHtml.AppendHtml(tbHeaderSearch);
            }
            else if (type == ColumnType.Data)
            {
                tb.AddCssClass("Cell");
                tb.InnerHtml.AppendHtml(CellField.HtmlText());
            }
            else
                throw new NotImplementedException();

            return tb;
        }
    }

    public enum ColumnType
    {
        Header,
        Data
    }
    public class TableColumn : Field
    {
        public TableCell Cell;

        public TableColumn(ColumnType ColumnType, TableCell Cell = null)
        {
            this.Cell = Cell;
            if (ColumnType== ColumnType.Data)
                this.Tag = "td";
            else if (ColumnType == ColumnType.Header)
                this.Tag = "th";
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder(this.Tag);
            if (Cell != null)
                tb.InnerHtml.AppendHtml(this.Cell.HtmlText());
            else
                tb.InnerHtml.AppendHtml("Not defined.");
            return tb;
        }
    }

    public class TableRow : Field
    {
        private List<TableColumn> Columns;
        private ColumnType type;

        public TableRow(ColumnType type)
        {
            Columns = new List<TableColumn>();
            this.Tag = "tr";
            this.type = type;
        }

        public void AddColumn(TableColumn NewColumn)
        {
            Columns.Add(new TableColumn(this.type, NewColumn.Cell));
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder(this.Tag);
            for (int i = 0; i < Columns.Count; i++)
                tb.InnerHtml.AppendHtml(Columns[i].HtmlText());
            return tb;
        }
    }

    public class TableField : Field
    {
        private List<TableRow> Rows;

        public TableField()
        {
            Rows = new List<TableRow>();
        }

        public void AddHeader(TableRow NewRow)
        {
            NewRow = new TableRow();
            NewRow.AddColumn()

        }
        public void AddRow()
        {

        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("table");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(textArea.HtmlText());

            return tb;
        }
    }

    public class GridViewField : Field
    {   //  <textarea rows="4" cols="50" placeholder="Describe yourself here...">Value</textarea>

        public GridViewField(string Id, string LabelText, string Text, string Placeholder = "...", int Rows = 4)
        {
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(textArea.HtmlText());

            return tb;
        }

        public static Field GetRandom(string id)
        {
            LabelTextAreaField a = new LabelTextAreaField(id, "TextArea label", null, "holder", 4);
            return a;
        }
    }



    public class LabelField : Field
    {
        public string HtmlLabel { get; set; }
        public string For { get; set; }

        public string ToolTip { get; set; }
        public string PlaceHolder { get; set; }
        public bool Bold { get; set; }

        public LabelField(string HtmlLabel, bool Bold = false, string For = null)
        {
            this.Bold = Bold;
            this.HtmlLabel = HtmlLabel;
            this.For = For;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("label");
            tb.AddCssClass(Bold ? "BoldLabelField" : "LabelField");
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
    public class TextAreaField : Field
    {
        public int Rows { get; set; }
        public string Placeholder { get; set; }
        public string Text { get; set; }

        public TextAreaField(string NameId, string Text, string Placeholder, int Rows)
        {
            this.NameId = NameId;
            this.Text = Text;
            this.Placeholder = Placeholder;
            this.Rows = Rows;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("textarea");
            tb.AddCssClass("TextAreaField");
            tb.Attributes.Add("name", NameId);
            tb.Attributes.Add("rows", Rows.ToString());
            tb.Attributes.Add("placeholder", Placeholder);
            tb.InnerHtml.AppendHtml(WebUtility.HtmlEncode(Text));
            return tb;
        }
    }
    public class LabelTextAreaField : Field
    {   //  <textarea rows="4" cols="50" placeholder="Describe yourself here...">Value</textarea>
        public LabelField label { get; set; }
        public TextAreaField textArea { get; set; }

        public LabelTextAreaField(string Id, string LabelText, string Text, string Placeholder = "...", int Rows = 4)
        {
            label = new LabelField(LabelText, true);
            textArea = new TextAreaField(Id, Text, Placeholder, Rows);
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(textArea.HtmlText());

            return tb;
        }

        public static Field GetRandom(string id)
        {
            LabelTextAreaField a = new LabelTextAreaField(id,"TextArea label", null, "holder", 4);
            return a;
        }
    }

    public class TextBoxField : Field
    {
        public string Text { get; set; }
        public string PlaceHolder { get; set; }
        public string ToolTip { get; set; }
        public string Name { get; set; }

        public TextBoxField(string NameId, string Text, string PlaceHolder = "", string ToolTip="")
        {
            this.Name = NameId;
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
            this.ToolTip = ToolTip;
            this.Tag = "input";
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder(this.Tag);
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("name", this.Name);
            tb.Attributes.Add("type", "text");
            tb.Attributes.Add("title", ToolTip);
            tb.Attributes.Add("placeholder", PlaceHolder);
            return tb;
        }
    }
    public class LabelTextBoxField : Field
    {
        private LabelField label;
        private TextBoxField txtb;

        public LabelTextBoxField(string Id, string LabelText, string Text, string Placeholder="...", string ToolTip="")
        {
            label = new LabelField(LabelText,true);
            txtb = new TextBoxField(Id, Text, Placeholder, ToolTip);
        }

        public static Field GetRandom(string Id)
        {
            LabelTextBoxField a = new LabelTextBoxField(Id,Id+":labelText","","placeholder","tooltip");
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

    public class FileUploadField : Field
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public bool MultipleFile { get; set; }

        public FileUploadField(string NameId, bool MultipleFile = false, string ToolTip = "")
        {
            this.Name = NameId;
            this.ToolTip = ToolTip;
            this.MultipleFile = MultipleFile;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("name", this.Name);
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("title", ToolTip);
            if (MultipleFile)
                tb.Attributes.Add("multiple", "");
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

        public static Field GetRandom(bool MultipleUpload=false)
        {
            LabelFileUploadField a = new LabelFileUploadField("fu",new FileUploadField("fuu", MultipleUpload,"Tooltip..."));
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

    public class FileDownloadField : Field
    {
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public string Path { get; set; }

        public FileDownloadField(string NameId, string Path, string ToolTip = "")
        {
            this.Name = NameId;
            this.ToolTip = ToolTip;
            this.Path = Path;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("a");
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("href", this.Path);
            tb.Attributes.Add("download","");
            if (ToolTip=="")
                tb.Attributes.Add("title", this.ToolTip);

            string aa = System.IO.Path.GetFileName(this.Path);

            tb.InnerHtml.AppendHtml(aa);
            return tb;
        }
    }
    public class LabelFileDownloadField : Field
    {
        private LabelField label;
        private FileDownloadField fu;

        public LabelFileDownloadField(string HtmlText, FileDownloadField f)
        {
            label = new LabelField(HtmlText, true);
            fu = f;
        }

        public static Field GetRandom(bool MultipleUpload = false)
        {
            LabelFileDownloadField a = new LabelFileDownloadField("fu", new FileDownloadField("fuu", "/images/banner1.svg", "Tooltip..."));
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

    public class DatePickerField : Field
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

        public override TagBuilder HtmlText()
        {   
            TagBuilder tb = new TagBuilder("input");
            tb.AddCssClass("ControlOfLabelControlDuo");
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

        public LabelDatePickerField(string Id, string HtmlLabel, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null)
        {
            this.Label = new LabelField(HtmlLabel, true);
            this.DatePicker = new DatePickerField(Id, Value, MinDate, MaxDate);
        }

        public static Field GetRandom(string Id, DateTime? DefaultValue = null)
        {
            DefaultValue = (DefaultValue == null ? DateTime.Now : DefaultValue);
            LabelDatePickerField a = new LabelDatePickerField(Id,"DP Label",DefaultValue );
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

    //public class DropDownOption : IHtmlTag
    public class DropDownOption : Field
    {
        //public string Value { get; set; }
        public string Label { get; set; }
        public bool Enabled { get; set; }

        public DropDownOption(string ValueId, string Label, bool Enabled = true)
        {
            this.NameId = ValueId;
            //this.Value = ValueId;
            this.Label = Label;
            this.Enabled = Enabled;
        }

        public override TagBuilder HtmlText()
        {   //  <option label="England" value="England"></option> 
            TagBuilder tb = new TagBuilder("option");
            //tb.Attributes.Add("value", Value);
            tb.Attributes.Add("value", NameId);
            if (!Enabled)
                tb.Attributes.Add("disabled", "");
            tb.InnerHtml.AppendHtml(Label);
            return tb;
        }
    }
    public class DropDownField : Field
    {
        List<DropDownOption> Options { get; set; }
        public int Size { get; set; }

        public DropDownField(string Id, int VisibleRows = 1)
        {
            Options = new List<DropDownOption>();
            this.NameId = Id;
            this.Size = VisibleRows;
        }

        public void Add(DropDownOption toBeAdded)
        {
            Options.Add(toBeAdded);
        }

        public static DropDownField GetRandom(string id, int count)
        {
            DropDownField n = new DropDownField(id+":DD", count - 1);
            for (int i = 0; i < count; i++)
            {
                DropDownOption no = new DropDownOption("DD" + i.ToString(), "DD" + i.ToString(), i % 2 == 0);
                n.Add(no);
            }
            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tbDropDown = new TagBuilder("select");
            tbDropDown.AddCssClass("DropDownOptions");

            tbDropDown.Attributes.Add("name", NameId);
            if (Size > 1)
                tbDropDown.Attributes.Add("size", Size.ToString());

            /*if (!Enabled)
                tbDropDown.Attributes.Add("disabled", "");*/

            for (int i = 0; i < Options.Count; i++)
            {
                TagBuilder t = Options[i].HtmlText();
                tbDropDown.InnerHtml.AppendHtml(t);
            }
            return tbDropDown;
        }
    }
    public class LabelDropDownField : Field
    {
        public LabelField Label { get; set; }
        public DropDownField Dropdown { get; set; }

        public LabelDropDownField(string Id, string label, int VisibleRows = 1)
        {
            Label = new LabelField(label, true);
            Dropdown = new DropDownField(Id, VisibleRows);
        }

        public static Field GetRandom(string id, int count)
        {
            LabelDropDownField n = new LabelDropDownField(id, id + ":DDLabel");
            n.Dropdown = DropDownField.GetRandom(id, count);
            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
                
            tb.InnerHtml.AppendHtml(Label.HtmlText());
            tb.InnerHtml.AppendHtml(Dropdown.HtmlText());
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
            this.Label = new LabelField(Label, false, IndividualValueId);
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

        public LabelRBCBControl(string id, string label)
        {
            this.Label = new LabelField(label, true);
            Options = new List<T>();
            GCType = (typeof(T) == typeof(LabelCheckBoxField)) ? GroupControlType.CheckBoxes : GroupControlType.RadioBoxes;
        }

        public void Add(T toBeAdded)
        {
            Options.Add(toBeAdded);
        }

        public static Field GetRandom(string id, int count)
        {
            LabelRBCBControl<T> n = new LabelRBCBControl<T>(id, "control global label");
            for (int i = 0; i < count; i++)
            {
                string Name = typeof(T).ToString() + DateTime.Now.ToLongTimeString();
                //          string Name, string Label, string IndividualValueId, bool Checked = false, bool Disabled = false
                T aa = (T)Activator.CreateInstance(typeof(T), Name, typeof(T).ToString() + i.ToString(), typeof(T).ToString() + i.ToString(), false, false);
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

            //TagBuilder tbForm = new TagBuilder("form");
            //tbForm.Attributes.Add("action", "#");
            //tbForm.Attributes.Add("method", "get");

            for (int i = 0; i < Options.Count; i++)
            {
                TagBuilder t = (TagBuilder) Options[i].GetType().GetMethod("HtmlText").Invoke(Options[i], null);
                //tbForm.InnerHtml.AppendHtml(t);
                tbControl.InnerHtml.AppendHtml(t);
            }

            //tbControl.InnerHtml.AppendHtml(tbForm);

            tb.InnerHtml.AppendHtml(Label.HtmlText());
            tb.InnerHtml.AppendHtml(tbControl);
            return tb;
        }
    }


}