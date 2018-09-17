using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace PDMSCore.DataManipulation
{
    public static class WebStuffHelper
    {
        public static KeyValuePair<string,string> CreateJSParameter(string JSEvent, string JSFunction, params string[] JSFunctionParameters )
        {
            string val = JSFunction + "(";
            for (int i = 0; i < JSFunctionParameters.Length; i++)
            {
                if (JSFunctionParameters[i].ToString() == "this")
                    val += HttpUtility.JavaScriptStringEncode("this");
                else
                    val += "'" + HttpUtility.JavaScriptStringEncode(JSFunctionParameters[i].ToString()) + "'";

                if (i < JSFunctionParameters.Length-1)
                    val += ",";
            }
            val += ")";
            return new KeyValuePair<string, string>(JSEvent, val);
        }

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            string ret = writer.ToString();
            return ret.Trim();
        }

        public static Dictionary<string, string> DecodeJsonFormData(string s)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string dec;
            if (s == null)
                return ret;

            string[] al = s.Split('&');

            for (int i = 0; i < al.Length; i++)
            {
                dec = HttpUtility.UrlDecode(al[i]);
                string[] tra = dec.Split('=');
                ret.Add(tra[0], tra[1]);
            }

            return ret;
        }
        public static Dictionary<string, string> GetRidOfParentID(Dictionary<string, string> s)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string first = "";
            int off = -1;
            if (s.Count > 1)
            {
                first = s.First().Key;
                off = first.IndexOf('-')+1;
            }

            foreach (var item in s.Keys)
            {
                string Key = item.ToString();
                string Val = s[item];

                Key = item.ToString().Substring(off);
                ret.Add(Key, Val);

            }
            return ret;
        }

        public class DBTableUpdateTrio
        {
            public int IDOfRowToBeUpdated { get; set; }
            public KeyValuePair<string, string> newValue;   //  TODO: Later, accomodate for more file types. (Date, ...)

            public DBTableUpdateTrio(int RowID, KeyValuePair<string, string> newValue)
            {
                IDOfRowToBeUpdated = RowID;
                this.newValue = newValue;
            }
        }



        //public static TagBuilder ModalDialog(string ID, string Title, string descr, TagBuilder content, bool visible )
        //{
        //    TagBuilder tb = new TagBuilder("div");
        //    tb.Attributes.Add("id", ID + "-Add");
        //    tb.AddCssClass("ModalBody");

        //    StringBuilder sb = new StringBuilder();
            
        //    sb.AppendLine("<div class=\"ModalTitle\">" + Title + "</div>");
        //    sb.AppendLine("<div class=\"ModalDescription\">" + descr + "</div>");
        //    sb.AppendLine("<hr />");
        //    TagBuilder tbModalContent = new TagBuilder("div");
        //    tbModalContent.AddCssClass("ModalContent");

        //    tbModalContent.InnerHtml.AppendHtml(content);
        //    sb.AppendLine(WebStuffHelper.GetString(tbModalContent));

        //    sb.AppendLine("<div class=\"ModalBottom\">");
        //    sb.AppendLine("<input type = \"button\" class=\"CancelBtn\" value=\"Cancel\" />");
        //    sb.AppendLine("<input type = \"button\" class=\"OkBtn\" value=\"Tlacitko OK\" />");
        //    sb.AppendLine("</div>");

        //    tb.InnerHtml.AppendHtml(sb.ToString());

        //    return tb;
        //}

    }
}
