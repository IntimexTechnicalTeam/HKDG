﻿namespace HKDG.Admin.Controllers
{
    [LanguageResource]
    public class ContentController : Controller
    {       
        public ContentResult LanguageScript()
        {
            System.Type stringType = typeof(string);
            PropertyInfo[] scripts = typeof(HKDG.Resources.Javascript).GetProperties();
            System.Collections.Hashtable array = new System.Collections.Hashtable();
            foreach (var p in scripts)
            {
                if (p.PropertyType == stringType)
                    array.Add(p.Name, p.GetValue(p, null));
            }

            string script = JsonUtil.ToJson(array);
            script = "var Resources =" + script + ";";

            ContentResult Content = new ContentResult();
            Content.Content = script;
            Content.ContentType = "application/x-javascript; charset=utf-8";
            return Content;
        }

    }
}
