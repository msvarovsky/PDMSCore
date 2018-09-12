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
            string[] al = s.Split('&');

            for (int i = 0; i < al.Length; i++)
            {
                dec = HttpUtility.UrlDecode(al[i]);
                string[] tra = dec.Split('=');
                ret.Add(tra[0], tra[1]);
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



        public static TagBuilder ModalDialog(string ID, string Title, string descr, TagBuilder content, bool visible )
        {
            /*
<div id = "@Model.ID-AddModal" class="Modal" style="display: none;">
    <div class="ModalBody" id="@Model.ID-Add">
        <div class="ModalTitle">Title</div>
        <div class="ModalDescription"> Modal Descr. ...</div>
        <hr />
        <div class="ModalContent">
            <p>...</p>
        </div>
        <div class="ModalBottom">
            <input type = "button" class="CancelBtn" value="Cancel" />
            <input type = "button" class="OkBtn" value="Tlacitko OK" />
        </div>
    </div>
</div>
            */

            TagBuilder tb = new TagBuilder("div");
            tb.Attributes.Add("id", ID + "-Add");
            tb.AddCssClass("ModalBody");

            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("<div class=\"ModalTitle\">" + Title + "</div>");
            sb.AppendLine("<div class=\"ModalDescription\">" + descr + "</div>");
            sb.AppendLine("<hr />");
            TagBuilder tbModalContent = new TagBuilder("div");
            tbModalContent.AddCssClass("ModalContent");

            tbModalContent.InnerHtml.AppendHtml(content);
            sb.AppendLine(WebStuffHelper.GetString(tbModalContent));

            sb.AppendLine("<div class=\"ModalBottom\">");
            sb.AppendLine("<input type = \"button\" class=\"CancelBtn\" value=\"Cancel\" />");
            sb.AppendLine("<input type = \"button\" class=\"OkBtn\" value=\"Tlacitko OK\" />");
            sb.AppendLine("</div>");
            //sb.AppendLine("</div>");

            tb.InnerHtml.AppendHtml(sb.ToString());



            //TagBuilder tb = new TagBuilder("div");
            //tb.Attributes.Add("id", ID + "-AddModal");
            //tb.AddCssClass("Modal");

            //if (visible)
            //    tb.Attributes.Add("style", "display:block");
            //else
            //    tb.Attributes.Add("style", "display:none");


            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("<div class=\"ModalBody\" id=\"" + ID + "-Add\">");
            //sb.AppendLine("<div class=\"ModalTitle\">" + Title + "</div>");
            //sb.AppendLine("<div class=\"ModalDescription\">" + descr + "</div>");
            //sb.AppendLine("<hr />");
            //TagBuilder tbModalContent = new TagBuilder("div");
            //tbModalContent.AddCssClass("ModalContent");

            //tbModalContent.InnerHtml.AppendHtml(content);
            //sb.AppendLine(WebStuffHelper.GetString(tbModalContent));

            //sb.AppendLine("<div class=\"ModalBottom\">");
            //sb.AppendLine("<input type = \"button\" class=\"CancelBtn\" value=\"Cancel\" />");
            //sb.AppendLine("<input type = \"button\" class=\"OkBtn\" value=\"Tlacitko OK\" />");
            //sb.AppendLine("</div>");
            //sb.AppendLine("</div>");

            //tb.InnerHtml.AppendHtml(sb.ToString());

            return tb;
        }

    }
}
