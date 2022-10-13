using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intimex.Utility
{
    /// <summary>
    /// 自動映射實體類
    /// </summary>
    public static class AutoMapper
    {
        /// <summary>
        /// 浅克隆
        /// </summary>
        /// <param name="source">源</param>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <returns>目标类型</returns>
        public static TDestination Clone<TDestination>(this object source)
            where TDestination : new()
        {
            TDestination dest = new TDestination();
            dest.GetType().GetProperties().ForEach(p =>
            {
                p.SetValue(dest, source.GetType().GetProperty(p.Name)?.GetValue(source));
            });
            return dest;
        }

        /// <summary>
        /// 把T1集合克隆到T2集合
        /// </summary>
        /// <typeparam name="T1">源集合</typeparam>
        /// <typeparam name="T2">目標集合</typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T2> CloneList<T1, T2>(this IEnumerable<T1> list) where T1 : new() where T2 : new()
        {
            var newList = new List<T2>();
            T2 t = new T2();
            list.ForEach(f =>
            {
                t = f.Clone<T2>();
                newList.Add(t);
            });
            return newList;
        }

        /// <summary>
        /// 深克隆
        /// </summary>
        /// <param name="source">源</param>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <returns>目标类型</returns>
        public static TDestination DeepClone<TDestination>(this object source)
            where TDestination : new()
        {
            return JsonConvert.DeserializeObject<TDestination>(JsonConvert.SerializeObject(source));
        }

        /// <summary>
        /// 遍历IEnumerable
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="action">回调方法</param>
        /// <typeparam name="T"></typeparam>
        public static void ForEach<T>(this IEnumerable<T> objs, Action<T> action)
        {
            foreach (var o in objs)
            {
                action(o);
            }
        }
    }
}
