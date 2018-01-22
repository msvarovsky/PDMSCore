using PDMSCore.DataManipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSCore.BusinessObjects
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Panel> ToShow { get; set; }

        public bool Create()
        {
            // jdi do DB a vytvor novy projekt.

            Id = (Int32) DateTime.Now.Ticks;

            return true;
        }

        public List<Panel> GetRandom()
        {
            long randonName = DateTime.Now.Ticks;

            List<Field> fields = new List<Field>();

            //TextBoxField tb = new TextBoxField((randonName++).ToString(), "Pocet projektu", "5");

            
            fields.Add(new LabelField("Normalni text"));
            fields.Add(Field.NewLine());
            fields.Add(new LabelField("Bold text",true));
            fields.Add(Field.NewLine());

            fields.Add(LabelTextBoxField.GetRandom());
            fields.Add(LabelTextAreaField.GetRandom());
            fields.Add(Field.NewLine());

            fields.Add(LabelRBCBControl<LabelRadioButtonField>.GetRandom(5));
            fields.Add(LabelRBCBControl<LabelCheckBoxField>.GetRandom(10));


            /*fields.Add(LabelCheckBoxesControl.GetRandom(2));
            fields.Add(LabelRadioButtonsControl.GetRandom(2));*/


            //fields.Add(new Field { Label = "Pocet projektu", tagName = 1, Type = FieldType.Indicator, Value = "5" });
            //fields.Add(new Field { Label = "Pocet otevrenych projektu", tagName = 2, Type = FieldType.Indicator, Value = "5" });

            Panel panel = new Panel();
            panel.Content = fields;
            panel.id = 1;
            panel.Label = "GetRandom";
            panel.Size = "x1";

            List<Panel> panels = new List<Panel>();
            panels.Add(panel);
            panels.Add(panel);
            ToShow = panels;

            return panels;
        }


    }
}