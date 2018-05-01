using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.DataManipulation
{
    public class ModalDialog
    {
        public string Title{ get; set; }
        public string Description { get; set; }
        public List<Field> Fields { get; set; }
        public string OkButtonLabel { get; set; }
        public string CancelButtonLabel { get; set; }
        public string ReturnFieldID { get; set; }


        public ModalDialog(string LangID, string title, string Descr="")
        {
            Title = title;
            Description = Descr == "" ? "When you are defining a string you must start and end with a single or double quote" : Descr;
            Fields = new List<Field>();


            OkButtonLabel = "Ok";
            CancelButtonLabel = "Cancel";
            
        }

        public void AddField(Field f)
        {
            Fields.Add(f);
        }

    }
}
