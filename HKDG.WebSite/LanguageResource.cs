using Microsoft.Extensions.Configuration.Json;
using System.Collections.Generic;
using System.Configuration;

namespace HKDG.WebSite
{
    public class Resources
    {
        public static string ToMultiLang(string key, Language? language)
        {
            if (key.IsEmpty()) return "";

            if (language ==null ) return key;

            var providers = ((IConfigurationRoot)Globals.Configuration).Providers;
            var jsonProvider =providers.Where(x => x.GetType() == typeof(JsonConfigurationProvider)).Select(item=> (JsonConfigurationProvider)item)
                                        ?.FirstOrDefault(x=>x.Source.Path == $"Config/Resource_{language}.json");

           if (jsonProvider ==null) return key;

            jsonProvider.TryGet(key, out string value);
            return value;
        }

        
    }
}
