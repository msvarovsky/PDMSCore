using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;

namespace PDMSCore.DataManipulation
{
    public enum RowType
    {
        Header,
        Data
    }
    public enum ColumnType
    {
        Label,
        Text,
        CheckBox
    }

    public class DataGridField2 : Field
    {
        //public int ID { get; set; }
        public int nVisibleRows { get; set; }
        public string FocusControlID { get; set; }
        private List<TableRow2> Data;
        private string[] HeaderLabels;
        private int[] MinColumnWidtg;

        public DataGridField2():base("TODO","GridTable","table", null)
        {
            HeaderLabels = null;
            Data = new List<TableRow2>();
            nVisibleRows = 5;
        }

        public static DataGridField2 GetTestData(int ID)
        {
            DataGridField2 d = new DataGridField2();
            //d.ID = ID;
            d.SetHeaderLabels("Jmeno", "Prijmeni", "Aktivni");

            TableRow2 tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Martin-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Svarovsky"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr, 1);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Martin-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("SpatnePrijmeni"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Cecile-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Svarovska"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Jitka-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Svarovska"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Astrid-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Svarovska"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Lubos-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Svarovsky"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Dominique-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Champagne"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Nadine-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Champagne"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Thomas-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("Champagne"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            tr = new TableRow2();
            tr.AddColumnCell(new LabelField("Noemie-" + DateTime.Now.Second));
            tr.AddColumnCell(new LabelField("JeNeSaisPas"));
            tr.AddColumnCell(new CheckBoxField("", "", "", false, false));
            d.AddDataRow(tr);

            /*d.AddDataRow(tr.MakeCopy());
            d.AddDataRow(tr.MakeCopy());*/

            return d;
        }

        public void SetHeaderLabels(params string[] a)
        {
            HeaderLabels = a;
            FocusControlID = "filter-" + HeaderLabels[0];
            MinColumnWidtg = new int[a.Length];

            for (int i = 0; i < a.Length; i++)
                MinColumnWidtg[i] = HeaderLabels[i].Length * 5;
        }

        public bool AddDataRow(TableRow2 tr, int? id = null)
        {
            if (HeaderLabels == null)
                throw new Exception("Table header labels are not defined.");

            tr.ID = (id == null) ? Data.Count+1 : (int)id;

            Data.Add(tr);
            return true;
        }

        private string[] ToLowerCase(string[] f)
        {
            string[] ret = new string[f.Length];

            for (int i = 0; i < f.Length; i++)
            {
                if (f[i] == null)
                    continue;
                ret[i] = f[i].ToLower();
            }


            return ret;
        }

        public void ApplyFilters(string[] filters, FilteringType ft )
        {
            if (filters.Length == 0)
                return;
            filters = ToLowerCase(filters);

            for (int r = 0; r < Data.Count; r++)
            {
                if (!Data[r].DoesRowComplyWithFilters(filters, ft))
                {
                    Data.RemoveAt(r);
                    r--;
                }
            }
        }

        public TagBuilder HtmlTextHeaderRow()
        {
            TableRow2 tr = Data[0];

            TagBuilder tbTr = new TagBuilder("tr");

            TagBuilder tbID = new TagBuilder("th");
            tbID.Attributes.Add("style", "display:none");
            tbTr.InnerHtml.AppendHtml(tbID);

            for (int i = 0; i < tr.Cells.Count; i++)
            {
                TagBuilder tbTh = new TagBuilder("th");
                TagBuilder tbDiv = new TagBuilder("div");
                tbDiv.AddCssClass("HeadCell");
                TagBuilder tbHl = new TagBuilder("HeaderLabel");
                TagBuilder tbHs = new TagBuilder("HeaderSearch");

                string colID;
                if (i > HeaderLabels.Length)
                    colID = "{Not defined}";
                else
                    colID = HeaderLabels[i];

                tbHl.InnerHtml.AppendHtml(colID);

                if (tr.Cells[i].GetType() == typeof(CheckBoxField))
                {
                    DropDownListBox ddf = new DropDownListBox("todo","filter-ddf-1", 1);
                    ddf.jsOnInputFunction = "OnDataGridFilterChange()";
                    ddf.Add("", "(All)");
                    ddf.Add("1", "Yes");
                    ddf.Add("-1", "No");
                    ddf.Classes.Add("filter");

                    tbHs.InnerHtml.AppendHtml(ddf.BuildHtmlTag());
                }
                else
                {
                    TagBuilder tbInput = new TagBuilder("input");
                    tbInput.Attributes.Add("type", "text");
                    tbInput.Attributes.Add("placeholder", "...");
                    tbInput.Attributes.Add("id", "filter-" + colID);
                    tbInput.Attributes.Add("oninput", "OnDataGridFilterChange(\'filter-" + colID + "\')");
                    tbInput.AddCssClass("filter");

                    tbHs.InnerHtml.AppendHtml(tbInput);
                }
                tbDiv.InnerHtml.AppendHtml(tbHl);
                tbDiv.InnerHtml.AppendHtml(tbHs);
                tbTh.InnerHtml.AppendHtml(tbDiv);

                tbTr.InnerHtml.AppendHtml(tbTh);
            }
            return tbTr;
        }

        public TagBuilder HtmlTextTableBody()
        {
            TagBuilder tbTableBody = new TagBuilder("tbody");
            //tbTableBody.Attributes.Add("id", "DataGridContent");
            tbTableBody.Attributes.Add("id", "dg-" + this.ID + "-b");

            for (int i = 0; i < Data.Count; i++)
                tbTableBody.InnerHtml.AppendHtml(Data[i].HtmlText());

            return tbTableBody;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.Attributes.Add("id", "GridTable");
            tb.AddCssClass("GridTable");

            tb.InnerHtml.AppendHtml(HtmlTextHeaderRow());

            TagBuilder tbDiv = new TagBuilder("div");
            tbDiv.Attributes.Add("id", "GridTableContent");

            for (int i = 0; i < Data.Count; i++)
                tbDiv.InnerHtml.AppendHtml(Data[i].HtmlText());

            tb.InnerHtml.AppendHtml(tbDiv);

            return tb;
        }

        //public override string GetValue()
        //{
        //    return "";
        //}

        public string GetPresentableStringFromID(string id)
        {
            int iid;
            if (Int32.TryParse(id, out iid))
            {
                TableRow2 tr2 = Data[iid - 1];
                return tr2.Cells[0].GetValue() + " " + tr2.Cells[1].GetValue();
            }
            return null;
        }
    }

    public enum FilteringType
    {
        StartWith,
        Contain,
        ContainWithAsterisks
    }

    public class TableRow2: IHtmlTag
    {
        public List<Field> Cells { get; set; }
        public int ID { get; set; }

        public TableRow2()
        {
            Cells = new List<Field>();
        }

        public bool DoesRowComplyWithFilters(string[] filter, FilteringType ft)
        {
            if (ft == FilteringType.StartWith)
            {
                for (int c = 0; c < Cells.Count; c++)
                {
                    if (filter[c] == null || filter[c] == "")
                        continue;
                    else
                    {
                        if (Cells[c].GetValue().ToLower().IndexOf(filter[c]) != 0)
                            return false;
                    }
                }
            }
            return true;
        }

        public void AddColumnCell(Field f)
        {
            Cells.Add(f);
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tbTr = new TagBuilder("tr");

            TagBuilder tbID = new TagBuilder("td");
            tbID.Attributes.Add("style", "display:none");
            tbID.InnerHtml.AppendHtml(ID.ToString());

            tbTr.InnerHtml.AppendHtml(tbID);


            for (int i = 0; i < Cells.Count; i++)
            {
                TagBuilder tbTd = new TagBuilder("td");
                TagBuilder tbDiv = new TagBuilder("div");
                tbDiv.AddCssClass("Cell");

                tbDiv.InnerHtml.AppendHtml(((IHtmlElement)Cells[i]).BuildHtmlTag());
                tbTd.InnerHtml.AppendHtml(tbDiv);
                tbTr.InnerHtml.AppendHtml(tbTd);
            }
            return tbTr;
        }

        //public TableRow2 MakeCopy()
        //{
        //    TableRow2 n = new TableRow2();
        //    n.ID = this.ID;
        //    Field[] fs = this.Cells.ToArray();

        //    n.Cells = new List<IHtmlElement>();
        //    for (int i = 0; i < fs.Length; i++)
        //        n.Cells.Add(fs[i]);

        //    return n;
        //}

        public TableRow2 MakeCopy()
        {
            TableRow2 n = new TableRow2();
            n.ID = this.ID;
            n.Cells = new List<Field>();

            for (int i = 0; i < this.Cells.Count; i++)
                n.Cells.Add(this.Cells[i]);

            return n;
        }
    }
    

    //public class TableColumns
    //{
    //    public List<OneColumnInfo> column;
    //    //ColumnType type, Boolean ro = false)

    //    public TableColumns()
    //    {
    //        column = new List<OneColumnInfo>();
    //    }

    //    public void Add(ColumnType type, Boolean readOnly)
    //    {
    //        column.Add(new OneColumnInfo(type, readOnly));

    //    }
    //}
    //public struct OneColumnInfo
    //{
    //    public ColumnType Type { get; private set; }
    //    public Boolean ReadOnly { get; private set; }

    //    public OneColumnInfo(ColumnType type, Boolean readOnly)
    //    {
    //        Type = type;
    //        ReadOnly = readOnly;
    //    }
    //}

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class DataGridField : Field, IHtmlElement
    {
        private TableRow HeaderRow;
        private List<TableRow> DataRow;

        public DataGridField():base("todo","GridTable","table", null)
        {
            DataRow = new List<TableRow>();
        }

        public void SetHeaderRow(TableRow NewRow)
        {
            HeaderRow = NewRow;
        }
        public void AddDataRow(TableRow NewRow)
        {
            DataRow.Add(NewRow);
        }

        public static DataGridField GetRandom(string type="")
        {
            DataGridField n = new DataGridField();
            if (type == "")
            {
                int ColumnCount = 3;
                n.SetHeaderRow(TableRow.GetRandomHeaderRow(ColumnCount));
                for (int i = 0; i < 5; i++)
                    n.AddDataRow(TableRow.GetRandomDataRow(ColumnCount));
            }
            else if (type == "users")
            {
                int ColumnCount = 3;
                n.SetHeaderRow(TableRow.GetRandomHeaderRow(ColumnCount));
                for (int i = 0; i < 5; i++)
                    n.AddDataRow(TableRow.GetRandomDataRow(ColumnCount));

            }

            return n;
        }

        public TagBuilder BuildHtmlTag()
        {
            TagBuilder tb = base.BuildBaseHtmlTag();
            tb.Attributes.Add("id", "GridTable");
            tb.AddCssClass("GridTable");
            tb.InnerHtml.AppendHtml(HeaderRow.HtmlText());

            for (int i = 0; i < DataRow.Count; i++)
                tb.InnerHtml.AppendHtml(DataRow[i].HtmlText());

            return tb;
        }

    }

    public class TableRow : IHtmlTag
    {
        private List<TableColumn> Columns;

        public TableRow()
        {
            Columns = new List<TableColumn>();
        }

        public void AddColumn(TableColumn NewColumn)
        {
            Columns.Add(NewColumn);
        }

        public static TableRow GetRandomHeaderRow(int count)
        {
            TableRow n = new TableRow();
            for (int i = 0; i < count; i++)
            {
                TableColumn t = new TableColumn(RowType.Header, new TableCell(RowType.Header, new LabelField("NameID-" + i), "NameID-" + i));
                n.AddColumn(t);
            }
            return n;
        }
        public static TableRow GetRandomDataRow(int count)
        {
            TableRow n = new TableRow();
            for (int i = 0; i < count; i++)
            {
                TableColumn t = new TableColumn(RowType.Data, new TableCell(RowType.Data, new TextBoxField("NameID-" + i, "TableTextBoxData", "NameID-" + i), "NameID-" + i));
                n.AddColumn(t);
            }
            return n;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("tr");
            for (int i = 0; i < Columns.Count; i++)
                tb.InnerHtml.AppendHtml(Columns[i].HtmlText());
            return tb;
        }
    }

    public class TableColumn : IHtmlTag
    {
        public TableCell Cell;
        private RowType TableRowType;

        public TableColumn(RowType type, TableCell Cell = null)
        {
            TableRowType = type;
            this.Cell = Cell;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb;
            if (TableRowType == RowType.Header)
                tb = new TagBuilder("th");
            else if (TableRowType == RowType.Data)
                tb = new TagBuilder("td");
            else
                return null;


            if (Cell != null)
                tb.InnerHtml.AppendHtml(this.Cell.HtmlText());
            else
                tb.InnerHtml.AppendHtml("Not defined.");
            return tb;
        }
    }

    public class TableCell : IHtmlTag
    {
        RowType type;
        public string HtmlLabel { get; set; }
        private Field CellField { get; set; }

        public TableCell(RowType Type, Field f, string HeaderText = null)
        {
            HtmlLabel = HeaderText;
            this.type = Type;
            this.CellField = f;
        }

        public TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("div");

            if (type == RowType.Header)
            {
                tb.AddCssClass("HeadCell");

                TagBuilder tbHeaderLabel = new TagBuilder("HeaderLabel");
                tbHeaderLabel.InnerHtml.AppendHtml(HtmlLabel);

                TagBuilder tbHeaderSearch = new TagBuilder("HeaderSearch");
                TextBoxField t = new TextBoxField("TODO-NameId", "TableTextBoxHeader AutoComplete", "", "...");
                tbHeaderSearch.AddCssClass("HeaderSrchCl");
                tbHeaderSearch.InnerHtml.AppendHtml(t.BuildHtmlTag());

                tb.InnerHtml.AppendHtml(tbHeaderLabel);
                tb.InnerHtml.AppendHtml(tbHeaderSearch);
            }
            else if (type == RowType.Data)
            {
                tb.AddCssClass("Cell");
                tb.InnerHtml.AppendHtml(CellField.BuildBaseHtmlTag());
            }
            else
                throw new NotImplementedException();

            return tb;
        }
    }
    
}
