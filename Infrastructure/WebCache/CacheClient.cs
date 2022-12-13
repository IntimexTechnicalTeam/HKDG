using CSRedis;
using System;
using System.Threading.Tasks;
using Web.Framework;

namespace WebCache
{
    public static  class CacheClient
    {
       
        /// <summary>
        /// 缓存壳，获取Hash数据时如果缓存没有，则先执行委托获取到数据到再写入到缓存中，最后返回Hash数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="getDataAsync"></param>
        /// <returns></returns>
        public static async Task<T> CacheShellAsync<T>( string key, string field, Func<Task<T>> getDataAsync)
        {
           
            var cacheValue = await RedisHelper.HGetAsync<T>(key, field);
            if (cacheValue == null)
            {
                cacheValue = await getDataAsync();
                await RedisHelper.HSetAsync(key, field, cacheValue);
            }

            return cacheValue;
        }

        /// <summary>
        /// 缓存壳，获取Hash数据时如果缓存没有，则先执行委托获取到数据到再写入到缓存中，最后返回Hash数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="getDataAsync"></param>
        /// <param name="timeOutSecond"></param>
        /// <returns></returns>
        public static async Task<T> CacheShellAsync<T>(string key, string field,  Func<Task<T>> getDataAsync, int timeOutSecond = 3600)
        {
            string cacheSign = $"{key}_sign";                                                   //使用sign来防缓存雪崩
            var sign = await RedisHelper.GetAsync(cacheSign);
            if (sign != null)                           //未过期，直接返回。
            {
                var cacheValue = await RedisHelper.HGetAsync<T>(key, field);
                if (cacheValue ==null)  cacheValue = await getDataAsync();
                await RedisHelper.HSetAsync(key, field, cacheValue);               //不用管cacheValue是不是空值，防止缓存穿透
                return cacheValue;
            }
            else
            {
                var cacheValue = await getDataAsync();
                await RedisHelper.SetAsync(cacheSign, "Just a key sign", timeOutSecond);      //设置缓存标签和过期时间       
                await RedisHelper.HSetAsync(key, field, cacheValue);                                        //不用管cacheValue是不是空值,直接放在缓存中,防止缓存穿透
                return cacheValue;
            }
        }


        /// <summary>
        ///  缓存壳，获取string数据时如果缓存没有，则先执行委托获取到数据到再写入到缓存中，最后返回string数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        ///<param name="getDataAsync"></param>
        /// <param name="timeOutSecond">过期时间（秒），默认3600</param>
        /// <returns></returns>
        public static async Task<T> CacheShellAsync<T>(string key, Func<Task<T>> getDataAsync, int timeOutSecond = 3600)
        {
            var cacheValue = await RedisHelper.GetAsync<T>(key);
            if (cacheValue == null)
            {
                cacheValue = await getDataAsync();
                if (cacheValue != null) await RedisHelper.SetAsync(key,cacheValue, timeOutSecond);
            }

            return cacheValue;
        }

    }
}
