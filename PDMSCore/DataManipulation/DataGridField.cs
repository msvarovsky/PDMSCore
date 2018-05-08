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
        public int ID { get; set; }
        private List<TableRow2> Data;
        private string[] HeaderLabels;

        public DataGridField2()
        {
            HeaderLabels = null;
            Data = new List<TableRow2>();
        }

        private static DataGridField2 GetTestData()
        {
            DataGridField2 r = new DataGridField2();


            return r;
        }

        public void SetHeaderLabels(params string[] a)
        {
            HeaderLabels = a;
        }

        public bool AddDataRow(TableRow2 tr, int? id = null)
        {
            if (HeaderLabels == null)
                throw new Exception("Table header labels are not defined.");

            tr.ID = (id == null) ? Data.Count+1 : (int)id;

            Data.Add(tr);
            return true;
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

                //if (i > HeaderLabels.Length)
                //    tbHl.InnerHtml.AppendHtml("{Not defined}");
                //else
                //    tbHl.InnerHtml.AppendHtml(HeaderLabels[i]);

                string colID;
                if (i > HeaderLabels.Length)
                    colID = "{Not defined}";
                else
                    colID = HeaderLabels[i];

                tbHl.InnerHtml.AppendHtml(colID);

                if (tr.Cells[i].GetType() == typeof(CheckBoxField))
                {
                    DropDownField ddf = new DropDownField("filter-ddf-1", 1);
                    ddf.Add(new DropDownOption("-", "(All)"));
                    ddf.Add(new DropDownOption("y", "Yes"));
                    ddf.Add(new DropDownOption("n", "No"));
                    ddf.AddClass("filter");

                    tbHs.InnerHtml.AppendHtml(ddf.HtmlText());
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
            tbTableBody.Attributes.Add("id", "DataGridContent");

            for (int i = 0; i < Data.Count; i++)
                tbTableBody.InnerHtml.AppendHtml(Data[i].HtmlText());

            return tbTableBody;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("table");
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
    }

    public class TableRow2: IHtmlTag
    {
        public List<Field> Cells { get; set; }
        public int ID { get; set; }

        public TableRow2()
        {
            Cells = new List<Field>();
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

                tbDiv.InnerHtml.AppendHtml(Cells[i].HtmlText());
                tbTd.InnerHtml.AppendHtml(tbDiv);
                tbTr.InnerHtml.AppendHtml(tbTd);
            }
            return tbTr;
        }

        public TableRow2 MakeCopy()
        {
            TableRow2 n = new TableRow2();
            n.ID = this.ID;
            Field[] fs = this.Cells.ToArray();

            n.Cells = new List<Field>();
            for (int i = 0; i < fs.Length; i++)
                n.Cells.Add(fs[i]);

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

    public class DataGridField : Field
    {
        private TableRow HeaderRow;
        private List<TableRow> DataRow;

        public DataGridField()
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

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("table");
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
                tbHeaderSearch.InnerHtml.AppendHtml(t.HtmlText());

                tb.InnerHtml.AppendHtml(tbHeaderLabel);
                tb.InnerHtml.AppendHtml(tbHeaderSearch);
            }
            else if (type == RowType.Data)
            {
                tb.AddCssClass("Cell");
                tb.InnerHtml.AppendHtml(CellField.HtmlText());
            }
            else
                throw new NotImplementedException();

            return tb;
        }
    }
    
}
