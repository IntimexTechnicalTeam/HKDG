namespace Web.Framework
{
    /// <summary>
    /// 隐藏字段特性，设置入参某个字段隐藏，设置后swagger将不显示此字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HiddenFieldAttribute : Attribute
    {
    }

    /// <summary>
    /// 隐藏特性，在方法或Class设置后，swagger将不显示此方法或此Controller所有方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenAttribute : Attribute
    {
    }
}
