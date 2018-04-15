
using Microsoft.AspNetCore.Html;

namespace PDMSCore.DataManipulation
{
    /*
    public class AsyncLoader
    {
        //https://srramalho.wordpress.com/2017/08/27/asp-net-mvc-loading-partial-views-asynchronously/

            
        public static HtmlString Render(string Controler, string Action, string PlaceHolder)
        {
            return AsyncLoader.Render(Controler, Action, PlaceHolder, null);
        }

        public static HtmlString Render(string Controler, string Action, string PlaceHolder, dynamic Model)
        {
            var model = ObjectToJason(Model);

            string html = " $(document).ready(function(){ { window.async.getFromController(\"{ Controler}/{ Action}\", \"{ PlaceHolder}\", { model}); }});";

            return new HtmlString(html);
        }

        public static HtmlString Action(string Controler, string Action, string PlaceHolder, string Link)
        {
           return AsyncLoader.Action(Controler, Action, PlaceHolder, Link, null);
        }

        public static HtmlString Action(string Controler, string Action, string PlaceHolder, string Link, dynamic Model)
        {
           var model = ObjectToJason(Model);
            var html = $@” $(‘#{Link}‘).click(function(){{ window.async.getFromController(‘/{Controler}/{Action}‘, ‘{PlaceHolder}‘, {model}); }}); “;
        return HtmlString.Create(html);

        }

        public static HtmlString Post(string Controler, string Action, string PlaceHolder)
        {
            var html = $@” window.async.postToController(‘/{ Controler}/{ Action}‘, ‘{ PlaceHolder}‘); “;
            return HtmlString.Create(html);
        }

    }

    private JsonSerializerSettings CreateSerializerSettings()

    {

        var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings();

        serializerSettings.MaxDepth = 5;

        serializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

        serializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;

        serializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

        serializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Reuse;

        serializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;

        return serializerSettings;

    }

    private string ObjectToJason(object obj)
    {
        string json = null;
        if (obj == null)
            return json;
        try
        {
            json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings());
        }
        catch
        {
            //log this
        }
        return json;
    }

    */
}
