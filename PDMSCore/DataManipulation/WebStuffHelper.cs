using System;
using System.Collections.Generic;
using System.Linq;
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
        

    }
}
