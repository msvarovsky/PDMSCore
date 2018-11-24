using Microsoft.AspNetCore.Mvc.Rendering;
using PDMSCore.DataManipulation;
using System.Collections.Generic;

namespace PDMSCore.BusinessObjects
{
    public class ButtonMenu
    {
        public string DataGridID { get; set; }
        public string DataGridParentControllerAndAction { get; set; }

        private List<ButtonMenuItem> items;

        public ButtonMenu(string ID, string ParentControllerAndAction)
        {
            DataGridID = ID;
            DataGridParentControllerAndAction = ParentControllerAndAction;
            items = new List<ButtonMenuItem>();
        }

        public void AddMenu(ButtonMenuItem item)
        {
            item.UpdateID(DataGridID);
            items.Add(item);
        }

        public ModalDialog GetModalDialog(string LookingFor)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].WebName.ToLower() == LookingFor.ToLower())
                    return items[i].GetModalDialogObject();
            }
            return null;
        }

        public bool Exists(string LookingFor)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].WebName == LookingFor)
                    return true;
            }
            return false;
        }

        public TagBuilder BuildHtmlMenuButtons()
        {
            TagBuilder tb = new TagBuilder("span");

            if (items.Count == 0)
                tb.InnerHtml.AppendHtml("Zatim nic");
            else
            {
                for (int i = 0; i < items.Count; i++)
                {
                    tb.InnerHtml.AppendHtml(items[i].BuildHtmlMenuButton(DataGridID));
                }
            }

            return tb;
        }
        public TagBuilder BuildHtmlMenuDialogs()
        {
            TagBuilder tb = new TagBuilder("div");
            for (int i = 0; i < items.Count; i++)
            {
                tb.InnerHtml.AppendHtml(items[i].BuildHtmlMenuDialog(DataGridID, DataGridParentControllerAndAction));
            }

            return tb;
        }
    }

    public abstract class ButtonMenuItem
    {
        abstract public TagBuilder BuildHtmlMenuButton(string ID);
        abstract public TagBuilder BuildHtmlMenuDialog(string ID, string ParentControllerAndAction);
        abstract public ModalDialog GetModalDialogObject();
        abstract public void UpdateID(string id);
        virtual public string WebName { get; set; }
        virtual public string ID { get; set; }
    }

    public class BMAddItem : ButtonMenuItem
    {
        public ModalDialog AddRowDialog { get; set; }

        public BMAddItem(string DialogTitle, string DialogDescription)
        {
            Init();
            AddRowDialog = new ModalDialog(ID + "-" + WebName, "no need", DialogTitle, DialogDescription);
        }
        public BMAddItem()
        {
            Init();
            AddRowDialog = new ModalDialog(ID + "-" + WebName);
        }
        private void Init()
        {
            WebName = "AddMenu";
        }

        public override TagBuilder BuildHtmlMenuButton(string ID)
        {
            // <button type = "button" onclick = "AddRowModal(100)" >< i class="fa fa-plus"></i> Add</button>
            //< button type = "button" title = "New label" >< img src = "~/images/Plus2-24x24.png" alt = "Save" /></ button >


            TagBuilder tbButton = new TagBuilder("button");
            tbButton.Attributes.Add("type", "button");
            tbButton.Attributes.Add("onclick", "AddRowModal(100)");

            TagBuilder tbImg = new TagBuilder("img");
            tbImg.Attributes.Add("src", "/images/Plus2-24x24.png");
            tbImg.Attributes.Add("alt", "Add");
            tbButton.InnerHtml.AppendHtml(tbImg);

            //TagBuilder tbI = new TagBuilder("i");
            //tbI.AddCssClass("fa fa-plus");
            //tbButton.InnerHtml.AppendHtml(tbI);

            return tbButton;
        }
        public override TagBuilder BuildHtmlMenuDialog(string ID, string ParentControllerAndAction)
        {
            AddRowDialog = new ModalDialog(ID + "-AddRowModal", "", "Titulek");
            AddRowDialog.ParentControllerAndAction = ParentControllerAndAction;
            AddRowDialog.OnOk = "ReloadDataGridAndScrollToBottom()";
            return AddRowDialog.BuildDialogContent();
        }

        public override ModalDialog GetModalDialogObject()
        {
            return AddRowDialog;
        }

        public override void UpdateID(string id)
        {
            ID = id;
            AddRowDialog.ID = id + "-" + WebName;
        }
    }

    public class BMSaveMenuItem : ButtonMenuItem
    {
        public BMSaveMenuItem()
        {
            WebName = "SaveMenu";
        }

        public override TagBuilder BuildHtmlMenuButton(string ID)
        {
            //< button type = "button" title = "New label" >< img src = "~/images/Plus2-24x24.png" alt = "Save" /></ button >

            TagBuilder tb = new TagBuilder("span");

            TagBuilder tbButtonSave = new TagBuilder("button");
            tbButtonSave.Attributes.Add("id", ID + "-SaveMenuBtn");
            tbButtonSave.Attributes.Add("title", "Save");


            TagBuilder tbImgSave = new TagBuilder("img");
            tbImgSave.Attributes.Add("src", "/images/Save-24x24.png");
            tbImgSave.Attributes.Add("alt", "Save");
            tbButtonSave.InnerHtml.AppendHtml(tbImgSave);


            //<button type = "button" id="@Model.ID-SavedMenuBtn" style="width:100px; background-color: limegreen; border-color: limegreen; display: none;"><i class="fa fa-save"></i> Saved</button>
            TagBuilder tbButtonSaved = new TagBuilder("button");
            tbButtonSaved.Attributes.Add("type", "button");
            tbButtonSaved.Attributes.Add("id", ID + "-SavedMenuBtn");
            tbButtonSaved.Attributes.Add("style", "display: none;");

            TagBuilder tbImgSaved = new TagBuilder("img");
            tbImgSaved.Attributes.Add("src", "/images/Saved-24x24.png");
            tbImgSaved.Attributes.Add("alt", "Save");
            tbButtonSaved.InnerHtml.AppendHtml(tbImgSaved);


            tb.InnerHtml.AppendHtml(tbButtonSave);
            tb.InnerHtml.AppendHtml(tbButtonSaved);

            return tb;
        }
        public override TagBuilder BuildHtmlMenuDialog(string ID, string ParentControllerAndAction)
        {
            return null;
        }

        public override ModalDialog GetModalDialogObject()
        {
            return null;
        }

        public override void UpdateID(string id)
        {
            ID = id;
        }
    }


    
}
