using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.DataManipulation
{
    public class ModalDialog
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Field> Fields { get; set; }
        public string OkButtonLabel { get; set; }
        public string CancelButtonLabel { get; set; }
        public string TagIDOfReturnedID { get; set; }
        public string TagIDOfReturnedLabel { get; set; }
        public string ModalDialogID { get; set; }

        public ModalDialog(string ID)
        {
            Init(ID);
        }
        public ModalDialog(string ID, string LangID, string title, string Descr = "")
        {
            Title = title;
            Description = Descr;
            Init(ID);
        }
        private void Init(string id)
        {
            this.ID = id;
            Fields = new List<Field>();
            OkButtonLabel = "Ok";
            CancelButtonLabel = "Cancel";
        }

        public void AddField(Field f)
        {
            Fields.Add(f);
        }

        public TagBuilder BuildDialogContent()
        {
            TagBuilder tb = new TagBuilder("div");  //  Might need to replace by "<form id="NewDialog-noIDneeded" method="post">"
            for (int f = 0; f < Fields.Count; f++)
            {
                tb.InnerHtml.AppendHtml(((IHtmlElement)Fields[f]).BuildHtmlTag());
            }
            return tb;
        }

    }
}
