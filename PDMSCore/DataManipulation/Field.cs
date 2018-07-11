﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Collections;
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

    public struct WebTagAttributes
    {
        public Boolean ReadOnly { get; private set; }
        public string CheckBoxGroupName { get; private set; }

        public WebTagAttributes(Boolean readOnly, string checkBoxGroupName)
        {
            ReadOnly = readOnly;
            CheckBoxGroupName = checkBoxGroupName;
        }
    }

    public interface IHtmlTag
    {
        TagBuilder HtmlText();
    }

    public abstract class Field
    {
        public string NameId { get; set; }
        public int IntId { get; set; }
        //public FieldType Type { get; set; }

        public string Tag { get; set; }

        public abstract TagBuilder HtmlText();
        public abstract string GetValue();

        public static Field NewLine()
        {
            return new NewLine();
        }
    }
    public abstract class MultiOption<T>: Field
    {
        public int OtherRef { get; set; }
        public string[] SelectedValues { get; set; }
        public List<T> Options { get; set; }
        public GroupControlType GCType { get; set; }

        public MultiOption()
        {
            Options = new List<T>();
        }

        public void AddRelevantItems(List<TempMultiSelectItem> AllMultiSelectItem)
        {
            for (int i = 0; i < AllMultiSelectItem.Count; i++)
            {
                if (AllMultiSelectItem[i].OtherRef == IntId)
                {
                    bool bChecked = (ExistsInSelectedValuesArrays(AllMultiSelectItem[i].MultiSelectItemID.ToString()) ? true : false);

                    if (GCType == GroupControlType.DropDownListBoxes)
                    {   //DropDownOption
                        T aa = (T)Activator.CreateInstance(typeof(T), AllMultiSelectItem[i].MultiSelectItemID.ToString(), AllMultiSelectItem[i].StringValue);
                        Options.Add(aa);
                    }
                    else
                    {
                        T aa = (T)Activator.CreateInstance(typeof(T), "name-TODO", AllMultiSelectItem[i].StringValue, AllMultiSelectItem[i].MultiSelectItemID.ToString(), bChecked, false);
                        Options.Add(aa);
                    }
                }
            }
        }

        private bool ExistsInSelectedValuesArrays(string h)
        {
            for (int i = 0; i < SelectedValues.Length; i++)
                if (SelectedValues[i] == h)
                    return true;
            return false;
        }
    }

    public class NewLine : Field
    {
        public static Field GetRandom()
        {
            return null;
        }

        public override string GetValue()
        {
            return "";
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("/br");
            tb.TagRenderMode = TagRenderMode.SelfClosing;
            return tb;
        }
    }
    public class HiddenField : Field
    {
        public string Value { get; set; }
        public HiddenField(string NameId)
        {
            this.NameId = NameId;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "hidden");
            tb.Attributes.Add("name", NameId);
            tb.Attributes.Add("id", NameId);
            return tb;
        }

        public override string GetValue()
        {
            return Value;
        }
    }

    public class LabelDataGridField : Field
    {
        public LabelField label { get; set; }
        public DataGridField table { get; set; }

        public LabelDataGridField(string LabelText, DataGridField table)
        {
            label = new LabelField(LabelText, true);
            this.table = table;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(table.HtmlText());
            return tb;
        }

        public static Field GetRandom(string id)
        {
            LabelDataGridField n = new LabelDataGridField("Grid", DataGridField.GetRandom());
            return n;
        }

        public override string GetValue()
        {
            return "";
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

        public override string GetValue()
        {
            return HtmlLabel;
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
            tb.AddCssClass("ControlOfLabelControlDuo");
            tb.Attributes.Add("name", NameId);
            tb.Attributes.Add("id", NameId);
            tb.Attributes.Add("rows", Rows.ToString());
            tb.Attributes.Add("placeholder", Placeholder);
            tb.InnerHtml.AppendHtml(WebUtility.HtmlEncode(Text));
            return tb;
        }

        public override string GetValue()
        {
            return Text;
        }
    }
    public class LabelTextAreaField : Field
    {   //  <textarea rows="4" cols="50" placeholder="Describe yourself here...">Value</textarea>
        public LabelField label { get; set; }
        public TextAreaField textArea { get; set; }


        public LabelTextAreaField(int Id, string LabelText, string Text="", string Placeholder = "...", int Rows = 4)
        {
            
            label = new LabelField(LabelText, true);
            textArea = new TextAreaField(Id.ToString(), Text, Placeholder, Rows);
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(textArea.HtmlText());

            return tb;
        }

        public static Field GetRandom(int id)
        {
            LabelTextAreaField a = new LabelTextAreaField(id, "TextArea label", null, "holder", 4);
            return a;
        }

        public override string GetValue()
        {
            return textArea.GetValue();
        }
    }

    public class TextBoxField : Field
    {
        public string Text { get; set; }
        public string PlaceHolder { get; set; }
        public string ToolTip { get; set; }
        public string Name { get; set; }
        public string OnClick { get; set; }
        public bool ReadOnly { get; set; }
        private string CSS { get; set; }

        public TextBoxField(string NameId, string CSS, string Text="", string PlaceHolder = "", string ToolTip="")
        {
            this.Name = NameId;
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
            this.ToolTip = ToolTip;
            this.Tag = "input";
            this.CSS = CSS;
            this.ReadOnly = false;
            OnClick = "";
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder(this.Tag);

            if (CSS == "") 
                tb.AddCssClass("ControlOfLabelControlDuo");
            else
                tb.AddCssClass(CSS);

            if (ReadOnly)
                tb.Attributes.Add("readonly","");

            if (OnClick!="")
                tb.Attributes.Add("onclick", OnClick);

            tb.Attributes.Add("name", this.Name);
            tb.Attributes.Add("id", this.Name);
            tb.Attributes.Add("type", "text");
            if (ToolTip != "") 
                tb.Attributes.Add("title", ToolTip);
            if (PlaceHolder != "")
                tb.Attributes.Add("placeholder", PlaceHolder);
            return tb;
        }

        public override string GetValue()
        {
            return Text;
        }
    }
    public class LabelTextBoxField : Field
    {
        private LabelField label;
        private TextBoxField txtb;

        public LabelTextBoxField(int Id, string LabelText, string Text, string Placeholder="...", string ToolTip="")
        {
            label = new LabelField(LabelText,true);
            txtb = new TextBoxField(Id.ToString(), "ControlOfLabelControlDuo", Text, Placeholder, ToolTip);
        }

        public static Field GetRandom(int Id)
        {
            LabelTextBoxField a = new LabelTextBoxField(Id,Id+":labelText","","placeholder","tooltip");
            return a;
        }

        public override string GetValue()
        {
            return txtb.GetValue();
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

    public class LabelSelectableTextBoxField : Field
    {
        private LabelField label;
        private TextBoxField txtb;
        private HiddenField hiddenField;
        public string DataGridSP;

        public LabelSelectableTextBoxField(int Id, string LabelText, string Text, string dataGridSP)
        {
            label = new LabelField(LabelText, true);
            txtb = new TextBoxField(Id.ToString(), "ControlOfLabelControlDuo", Text);

            //function OpenModal(dialogID,      tagIDOfReturnedID,  tagIDOfReturnedLabel)
                   //  OpenModal('myModal1',    '74100',            'SelectedIDOf74100')
            txtb.OnClick = "OpenModal(\'ErarniModal\',\'SelectedIDOf" + Id + "\', \'" + Id + "\',\'ModalDataGrid\',\'sp_Users\')";
            txtb.ReadOnly = true;
            hiddenField = new HiddenField("SelectedIDOf" + Id);

            DataGridSP = dataGridSP;
        }

        public override string GetValue()
        {
            return txtb.GetValue();
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");
            tb.AddCssClass("LabelControlDuo");
            tb.InnerHtml.AppendHtml(label.HtmlText());
            tb.InnerHtml.AppendHtml(txtb.HtmlText());
            tb.InnerHtml.AppendHtml(hiddenField.HtmlText());
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

        public override string GetValue()
        {
            return Name;
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

        public override string GetValue()
        {
            return "";
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

        public override string GetValue()
        {
            return Name;
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

        public override string GetValue()
        {
            return fu.GetValue();
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

        public override string GetValue()
        {
            return Value.Value.ToShortDateString();
        }
    }
    public class LabelDatePickerField : Field
    {   //    <input type="date" name="bday">
        private LabelField Label;
        private DatePickerField DatePicker;

        public LabelDatePickerField(int Id, string HtmlLabel, DateTime? Value = null, DateTime? MinDate = null, DateTime? MaxDate = null)
        {
            this.Label = new LabelField(HtmlLabel, true);
            this.DatePicker = new DatePickerField(Id.ToString(), Value, MinDate, MaxDate);
        }

        public static Field GetRandom(int Id, DateTime? DefaultValue = null)
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

        public override string GetValue()
        {
            return DatePicker.GetValue();
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
            //this.NameId ="DD" + ValueId.ToString();
            NameId = ValueId;
            this.Label = Label;
            this.Enabled = Enabled;
        }

        public DropDownOption(string Name, string Label)
        {
            NameId = Name;
            this.Label = Label;
            this.Enabled = true;
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

        public override string GetValue()
        {
            return Label;
        }
    }
    public class DropDownField : MultiOption<DropDownOption>
    {
        //List<DropDownOption> Options { get; set; }
        public int Size { get; set; }
        private List<string> Classes { get; set; }
        public string jsOnInputFunction { get; set; }

        public DropDownField(string Id, int VisibleRows = 1)
        {
            //Options = new List<DropDownOption>();
            this.NameId = Id;
            this.Size = VisibleRows;
            Classes = new List<string>();
            Classes.Add("ControlOfLabelControlDuo");
            jsOnInputFunction = "";
        }

        public void Add(DropDownOption toBeAdded)
        {
            Options.Add(toBeAdded);
        }

        public void AddClass(string newClass)
        {
            Classes.Add(newClass);
        }

        public static DropDownField GetRandom(int id, int count)
        {
            DropDownField n = new DropDownField(id+":DD", count - 1);
            for (int i = 0; i < count; i++)
            {
                DropDownOption no = new DropDownOption(i.ToString(), "DD" + i.ToString(), i % 2 == 0);
                n.Add(no);
            }
            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tbDropDown = new TagBuilder("select");
            for (int i = 0; i < Classes.Count; i++)
                tbDropDown.AddCssClass(Classes[i]);

            if (jsOnInputFunction!="")
                tbDropDown.Attributes.Add("oninput", jsOnInputFunction);

            tbDropDown.Attributes.Add("name", NameId);
            if (Size > 1)
                tbDropDown.Attributes.Add("size", Size.ToString());

            for (int i = 0; i < Options.Count; i++)
            {
                TagBuilder t = Options[i].HtmlText();
                tbDropDown.InnerHtml.AppendHtml(t);
            }
            return tbDropDown;
        }

        public override string GetValue()
        {
            return "DropDownField,GetValue: TODO";
        }
    }
    //public class LabelDropDownField : MultiOption<DropDownOption>
    public class LabelDropDownField : Field
    {
        public LabelField Label { get; set; }
        public DropDownField Dropdown { get; set; }
        
        public LabelDropDownField(int Id, string label, int VisibleRows = 1)
        {
            //GCType = GroupControlType.DropDownListBoxes;
            IntId = Id;
            Label = new LabelField(label, true);
            Dropdown = new DropDownField(Id.ToString(), VisibleRows);
        }

        public static Field GetRandom(int id, int count)
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

        public override string GetValue()
        {
            return Dropdown.GetValue();
        }
    }

    public class CheckBoxField : Field
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
        public bool Disabled { get; set; }

        public CheckBoxField(string GroupName, string HtmlLabel, string IndividualValueId, bool Checked = false, bool Disabled = false)
        {
            this.Name = GroupName;
            this.Id = IndividualValueId;
            this.Value = IndividualValueId;
            this.Checked = Checked;
            this.Disabled = Disabled;
        }
        public CheckBoxField(string IndividualValueId, string HtmlLabel, bool Checked, WebTagAttributes wta)
        {
            this.Name = wta.CheckBoxGroupName;
            this.Id = IndividualValueId;
            this.Value = IndividualValueId;
            this.Checked = Checked;
            this.Disabled = wta.ReadOnly;
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

            tbLabelCheckBox.InnerHtml.AppendHtml(tbCheckBox);
            return tbLabelCheckBox;
        }

        public override string GetValue()
        {
            return Checked ? "1" : "-1";
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

        public override string GetValue()
        {
            return Value;
        }
    }
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

        public override string GetValue()
        {
            return Value;
        }
    }
    public enum GroupControlType
    {
        RadioButtons,
        CheckBoxes,
        DropDownListBoxes
    };
    public class LabelRBCBControl<T> : MultiOption<T>
    {
        public LabelField Label { get; set; }

        public LabelRBCBControl(string id, string label)
        {
            this.Label = new LabelField(label, true);
            Options = new List<T>();
            GCType = (typeof(T) == typeof(LabelCheckBoxField)) ? GroupControlType.CheckBoxes : GroupControlType.RadioButtons;
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
            else if (GCType == GroupControlType.RadioButtons)
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

        public override string GetValue()
        {
            return "";
        }
    }

    

}