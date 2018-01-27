using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PDMSCore.DataManipulation
{
    public class Panel
    {
        public int id { get; set; }
        public string Size { get; set; }
        public string Label { get; set; }
        public List<Field> Content { get; set; }

        public Panel(int id, string Label, int xSize)
        {
            //
            this.id = id;
            this.Label = Label;

            xSize = (xSize > 2) ? 2 : xSize;
            xSize = (xSize < 1) ? 1 : xSize;
            this.Size = "x" + xSize;

            this.Content = new List<Field>();
        }

        public bool Save(IFormCollection fc)
        {
            foreach (var FieldId in fc.Keys)
            {
                if (FieldId == "PanelId")
                    continue;

                string FieldValue = fc[FieldId];




            }
            return false;
        }

        public void AddFields(Field newField)
        {
            this.Content.Add(newField);
        }
    }
}