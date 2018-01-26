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

        public Project()
        {
            ToShow = new List<Panel>();
        }
        public bool Create()
        {
            // jdi do DB a vytvor novy projekt.
            Id = (Int32)DateTime.Now.Ticks;
            return true;
        }

        public void AddPanel(Panel newPanel)
        {
            ToShow.Add(newPanel);
        }

        public void GetRandom()
        {
            int id = 1;
            long randonName = DateTime.Now.Ticks;

            List<Field> fields = new List<Field>();

            //TextBoxField tb = new TextBoxField((randonName++).ToString(), "Pocet projektu", "5");
            
            fields.Add(new LabelField("Normalni text"));
            fields.Add(Field.NewLine());
            fields.Add(new LabelField("Bold text",true));
            fields.Add(Field.NewLine());

            fields.Add(LabelTextBoxField.GetRandom((id++).ToString()));
            fields.Add(LabelTextAreaField.GetRandom((id++).ToString()));
            fields.Add(Field.NewLine());

            fields.Add(LabelRBCBControl<LabelRadioButtonField>.GetRandom((id++).ToString(),3));
            fields.Add(LabelRBCBControl<LabelCheckBoxField>.GetRandom((id++).ToString(), 4));
            fields.Add(LabelDropDownField.GetRandom((id++).ToString(),4));
            fields.Add(LabelDatePickerField.GetRandom((id++).ToString()));

            fields.Add(LabelFileUploadField.GetRandom());
            fields.Add(LabelFileUploadField.GetRandom(true));

            fields.Add(LabelFileDownloadField.GetRandom());


            /*fields.Add(LabelCheckBoxesControl.GetRandom(2));
            fields.Add(LabelRadioButtonsControl.GetRandom(2));*/


            //fields.Add(new Field { Label = "Pocet projektu", tagName = 1, Type = FieldType.Indicator, Value = "5" });
            //fields.Add(new Field { Label = "Pocet otevrenych projektu", tagName = 2, Type = FieldType.Indicator, Value = "5" });

            Panel panel = new Panel(1,"GetRandom",1);
            panel.Content = fields;

            ToShow.Add(panel);
            ToShow.Add(panel);
        }


    }
}