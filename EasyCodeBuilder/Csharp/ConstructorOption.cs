using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 构造函数选项
/// </summary>
public class ConstructorOption : CodeOption
{
    /// <summary>
    /// 关键字列表
    /// </summary>
    public ICollection<string> Keywords { get; } = new HashSet<string>();
    
    /// <summary>
    /// 构造函数名称
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// 参数列表
    /// </summary>
    public ICollection<string> Parameters { get; } = new HashSet<string>();


    /// <inheritdoc />
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords   = string.Join(" ", CsharpKeywordOrdering.OrderForMember(Keywords));
        var parameters = string.Join(", ", Parameters);

        cb.CodeBlock(OnChildren, $"{keywords} {Name}({parameters})".Trim());
        return cb;
    }
}

/// <summary>
/// extensions for ConstructorOption
/// </summary>
public static class ConstructorOptionExtensions
{
    /// <summary>
    /// add keyword into constructor, this can be public, private, protected, internal, static, etc. duplicates are ignored
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static ConstructorOption WithKeyword(this ConstructorOption constructor, string keyword)
    {
        constructor.Keywords.Add(keyword);
        return constructor;
    }
    
    /// <summary>
    /// set name of constructor
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static ConstructorOption WithName(this ConstructorOption constructor, string name)
    {
        constructor.Name = name;
        return constructor;
    }
    
    /// <summary>
    /// add parameter into constructor, duplicates are ignored
    /// </summary>
    /// <param name="constructor"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public static ConstructorOption WithParameter(this ConstructorOption constructor, string parameter)
    {
        constructor.Parameters.Add(parameter);
        return constructor;
    }
}