using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;

namespace PDMSCore.BusinessObjects
{
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

        public static DataGridField GetRandom()
        {
            int ColumnCount = 3;
            DataGridField n = new DataGridField();

            n.SetHeaderRow(TableRow.GetRandomHeaderRow(ColumnCount));
            for (int i = 0; i < 5; i++)
                n.AddDataRow(TableRow.GetRandomDataRow(ColumnCount));

            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder("table");
            tb.AddCssClass("GridTable");
            tb.InnerHtml.AppendHtml(HeaderRow.HtmlText());

            for (int i = 0; i < DataRow.Count; i++)
                tb.InnerHtml.AppendHtml(DataRow[i].HtmlText());

            return tb;
        }
    }

    public class TableRow : Field
    {
        private List<TableColumn> Columns;
        private ColumnType type;

        public TableRow()
        {
            Columns = new List<TableColumn>();
            this.Tag = "tr";
        }
        public TableRow(ColumnType type)
        {
            Columns = new List<TableColumn>();
            this.Tag = "tr";
            this.type = type;
        }

        public void AddColumn(TableColumn NewColumn)
        {
            Columns.Add(NewColumn);
        }

        public static TableRow GetRandomHeaderRow(int count)
        {
            TableRow n = new TableRow(ColumnType.Header);
            for (int i = 0; i < count; i++)
            {
                TableColumn t = new TableColumn(ColumnType.Header, new TableCell(ColumnType.Header, new LabelField("NameID-" + i), "NameID-" + i));
                n.AddColumn(t);
            }
            return n;
        }
        public static TableRow GetRandomDataRow(int count)
        {
            TableRow n = new TableRow();
            for (int i = 0; i < count; i++)
            {
                TableColumn t = new TableColumn(ColumnType.Data, new TableCell(ColumnType.Data, new TextBoxField("NameID-" + i, "TableTextBoxData", "NameID-" + i), "NameID-" + i));
                n.AddColumn(t);
            }
            return n;
        }

        public override TagBuilder HtmlText()
        {
            TagBuilder tb = new TagBuilder(this.Tag);
            for (int i = 0; i < Columns.Count; i++)
                tb.InnerHtml.AppendHtml(Columns[i].HtmlText());
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

        public TableColumn(ColumnType type, TableCell Cell = null)
        {
            this.Cell = Cell;
            if (type == ColumnType.Data)
                this.Tag = "td";
            else if (type == ColumnType.Header)
                this.Tag = "th";
        }

        public void SetType(ColumnType type)
        {

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

    public class TableCell : Field
    {
        ColumnType type;
        public string HtmlLabel { get; set; }
        private Field CellField { get; set; }

        public TableCell(ColumnType Type, Field f, string HeaderText = null)
        {
            HtmlLabel = HeaderText;
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
                TextBoxField t = new TextBoxField("TODO-NameId", "TableTextBoxHeader AutoComplete", "", "...");
                tbHeaderSearch.AddCssClass("HeaderSrchCl");
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
    
}
