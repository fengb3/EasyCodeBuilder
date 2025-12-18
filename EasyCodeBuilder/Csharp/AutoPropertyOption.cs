using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 自动属性选项
/// </summary>
public class AutoPropertyOption : CodeOption
{
    /// <summary>
    /// 关键字列表
    /// </summary>
    public ICollection<string> Keywords { get; set; } = [];

    /// <summary>
    /// 属性类型
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// 属性名称
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords = string.Join(" ", Keywords);
        cb.AppendLine($"{keywords} {Type} {Name} {{ get; set; }}");
        return cb;
    }
}

/// <summary>
/// 自动属性选项扩展方法
/// </summary>
public static class AutoPropertyOptionExtensions
{
    /// <summary>
    /// 添加关键字
    /// </summary>
    /// <param name="prop">自动属性选项</param>
    /// <param name="keyword">关键字</param>
    /// <returns>自动属性选项</returns>
    public static AutoPropertyOption WithKeyword(this AutoPropertyOption prop, string keyword)
    {
        prop.Keywords.Add(keyword);
        return prop;
    }

    /// <summary>
    /// 设置类型
    /// </summary>
    /// <param name="prop">自动属性选项</param>
    /// <param name="type">类型</param>
    /// <returns>自动属性选项</returns>
    public static AutoPropertyOption WithType(this AutoPropertyOption prop, string type)
    {
        prop.Type = type;
        return prop;
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="prop">自动属性选项</param>
    /// <param name="name">名称</param>
    /// <returns>自动属性选项</returns>
    public static AutoPropertyOption WithName(this AutoPropertyOption prop, string name)
    {
        prop.Name = name;
        return prop;
    }
}