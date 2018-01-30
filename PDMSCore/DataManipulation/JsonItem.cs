using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDMSCore.DataManipulation
{
    public class JsonItem
    {
        public string ID { get; set; }
        public string Value { get; set; }

        public JsonItem(string ID, string Value)
        {
            this.ID = ID;
            this.Value = Value;
        }
    }
}
