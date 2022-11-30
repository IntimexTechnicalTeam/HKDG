using Microsoft.Extensions.Configuration.Json;

namespace HKDG.WebSite
{
    public class Resources
    {
        public static string ToMultiLang(string key, Language? language=null)
        {
            if (key.IsEmpty()) return "";

            var context = Globals.Services.GetService<IHttpContextAccessor>();
            var access_token = context?.HttpContext?.Request.Cookies["access_token"] ?? "";
            var user = RedisHelper.HGet<CurrentUser>($"{CacheKey.CurrentUser}", access_token) ?? null;
   
            if (language == null) language = user?.Language ?? Language.C;

            var providers = ((IConfigurationRoot)Globals.Configuration).Providers;
            var jsonProvider =providers.Where(x => x.GetType() == typeof(JsonConfigurationProvider)).Select(item=> (JsonConfigurationProvider)item)
                                        ?.FirstOrDefault(x=>x.Source.Path == $"Config/Resource_{language}.json");

           if (jsonProvider ==null) return key;

            jsonProvider.TryGet(key, out string value);
            return value;
        }

        
    }
}
