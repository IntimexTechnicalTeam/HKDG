

namespace Web.Framework
{
    /// <summary>
    /// 获取程序集
    /// </summary>
    public class RuntimeHelper
    {
        /// <summary>
        /// 获取程序集集合
        /// </summary>
        /// <returns></returns>
        public static Assembly[] Discovery()
        {
            var deps = DependencyContext.Default.RuntimeLibraries.Where(t => !t.Serviceable && t.Type != "package");////排除所有的系统程序集、Nuget下载包
            var list = deps.SelectMany(t => t.GetDefaultAssemblyNames(DependencyContext.Default)).Select(Assembly.Load).ToArray();
            //var result= DependencyContext.Default.RuntimeLibraries.SelectMany(t => t.GetDefaultAssemblyNames(DependencyContext.Default)).Select(Assembly.Load).ToArray();
            return list;
        }
        /// <summary>
        /// 获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies()
        {
            //return DependencyContext.Default.RuntimeLibraries.SelectMany(t => t.GetDefaultAssemblyNames(DependencyContext.Default)).Select(Assembly.Load).ToArray();
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
            //排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception ex)
                {
                    // ignored
                    //throw ex;
                }
            }
            return list;
        }

        /// <summary>
        /// 查找某个程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
        }
        /// <summary>
        /// 查询所有的程序集类型（排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包）
        /// </summary>
        /// <returns></returns>
        public static IList<Type> GetAllTypes()
        {
            var list = new List<Type>();
            foreach (var assembly in GetAllAssemblies())
            {
                var typeInfos = assembly.DefinedTypes;
                foreach (var typeInfo in typeInfos)
                {
                    list.Add(typeInfo.AsType());
                }
            }
            return list;
        }

        /// <summary>
        /// 查询某个类型的程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static IList<Type> GetTypesByAssembly(string assemblyName)
        {
            var list = new List<Type>();
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            var typeInfos = assembly.DefinedTypes;
            foreach (var typeInfo in typeInfos)
            {
                list.Add(typeInfo.AsType());
            }
            return list;
        }
        public static Type GetImplementType(string typeName, Type baseInterfaceType)
        {
            return GetAllTypes().FirstOrDefault(t =>
            {
                if (t.Name == typeName &&
                    t.GetTypeInfo().GetInterfaces().Any(b => b.Name == baseInterfaceType.Name))
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
                }
                return false;
            });
        }


    }

    public static class ReflectionUtil
    {
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="obj">反射对象</param>
        /// <param name="name">属性名</param>
        /// <param name="value">值</param>
        public static string SetProperty<T>(this T obj, string name, object value) where T : class
        {
            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.PropertyOrField(parameter, name);
            var before = Expression.Lambda(property, parameter).Compile().DynamicInvoke(obj);
            if (value.Equals(before))
            {
                return value.ToString();
            }

            if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeof(T).GetProperty(name)?.SetValue(obj, value);
            }
            //else if (property.Type == typeof(Guid))
            //{
            //    var assign = Expression.Assign(property, Expression.Constant(new Guid(value.ToString())));
            //    Expression.Lambda(assign, parameter).Compile().DynamicInvoke(obj);
            //}
            //else if (property.Type == typeof(bool))
            //{
            //    var assign = Expression.Assign(property, Expression.Constant(bool.Parse(value.ToString())));
            //    Expression.Lambda(assign, parameter).Compile().DynamicInvoke(obj);
            //}
            else
            {
                var assign = Expression.Assign(property, Expression.Constant(value));
                Expression.Lambda(assign, parameter).Compile().DynamicInvoke(obj);
            }

            return JsonUtil.ToJson(before);
        }
    }
}
