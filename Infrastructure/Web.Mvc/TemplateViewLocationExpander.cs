using HKDG.Resources;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.Framework;

namespace NETCoreViewLocationExpander.ViewLocationExtend
{
    /// <summary>
    /// 视图默认路径扩展
    /// </summary>
    public class TemplateViewLocationExpander : IViewLocationExpander
    {
        string[] views =  { };
        public TemplateViewLocationExpander(Func< string[]> action) {

            views = action.Invoke();
        }

        /// <summary>
        /// 扩展视图默认路径（PS：并非每次FindView都会执行该方法）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="viewLocations"></param>
        /// <returns></returns>
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var template = context.Values["template"] ?? TemplateEnum.Desktop.ToString();

            //string[] views = {
            //        "/Views/Desktop/{1}/{0}.cshtml",
            //        "/Views/Mobile/{1}/{0}.cshtml"
            //    };            
            return views.Union(viewLocations);

            //if (template == TemplateEnum.WeChatArea.ToString())
            //{
            //    string[] weChatAreaViewLocationFormats = {
            //        "/Areas/{2}/WeChatViews/{1}/{0}.cshtml",
            //        "/Areas/{2}/WeChatViews/Shared/{0}.cshtml",
            //        "/WeChatViews/Shared/{0}.cshtml"
            //    };
            //    //weChatAreaViewLocationFormats值在前--优先查找weChatAreaViewLocationFormats（即优先查找移动端目录）
            //    return weChatAreaViewLocationFormats.Union(viewLocations);
            //}
            //else if (template == TemplateEnum.WeChat.ToString())
            //{
            //    string[] weChatViewLocationFormats = {
            //        "/WeChatViews/{1}/{0}.cshtml",
            //        "/WeChatViews/Shared/{0}.cshtml"
            //    };
            //    //weChatViewLocationFormats值在前--优先查找weChatViewLocationFormats（即优先查找移动端目录）
            //    return weChatViewLocationFormats.Union(viewLocations);
            //}
            // return viewLocations;
        }

        /// <summary>
        /// 往ViewLocationExpanderContext.Values里面添加键值对（PS：每次FindView都会执行该方法）
        /// </summary>
        /// <param name="context"></param>
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var userAgent = context.ActionContext.HttpContext.Request.Headers["User-Agent"].ToString();
            var isMobile =ToolUtil.IsMobile(userAgent);
            var template = TemplateEnum.Desktop.ToString();
            if (isMobile)
            {
                //区域名称
                var areaName = context.AreaName ?? "";
               
                //控制器名称
                var controllerName = context.ControllerName ?? "";
                
                if (!string.IsNullOrEmpty(areaName) && !string.IsNullOrEmpty(controllerName)) //访问的是区域
                {
                    //template = TemplateEnum.WeChatArea.ToString();
                }
                else
                {
                    //template = TemplateEnum.WeChat.ToString();
                    template = TemplateEnum.Mobile.ToString();
                }
            }

            context.Values["template"] = template; //context.Values会参与ViewLookupCache缓存Key（cacheKey）的生成
        }
    }

    /// <summary>
    /// 模板枚举
    /// </summary>
    public enum TemplateEnum
    {
        //Default = 1,
        //WeChat = 2,
        //WeChatArea = 3

        Desktop = 1,
        Mobile = 2,
        
    }
}