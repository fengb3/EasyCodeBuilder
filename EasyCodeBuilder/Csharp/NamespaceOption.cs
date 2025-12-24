using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 命名空间选项
/// </summary>
public class NamespaceOption : CodeOption
{
    /// <summary>
    /// 命名空间名称
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"namespace {Name}");
    }

    /// <summary>
    /// 访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<NamespaceOption> Has => new(this);

    /// <summary>
    /// 公共访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<NamespaceOption> Public => Has.Public;

    /// <summary>
    /// 内部访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<NamespaceOption> Internal => Has.Internal;

    /// <summary>
    /// 私有访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<NamespaceOption> Private => Has.Private;
}

/// <summary>
/// extensions for NamespaceOption
/// </summary>
public static class NameSpaceOptionsExtensions
{
    /// <summary>
    /// set name of namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static NamespaceOption WithName(this NamespaceOption ns, string name)
    {
        ns.Name = name;
        return ns;
    }

    /// <summary>
    /// add type into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Type(this NamespaceOption ns, Action<TypeOption> configure) =>
        ns.AddChild(configure);

    /// <summary>
    /// add class into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Class(this NamespaceOption ns, Action<TypeOption> configure) =>
        ns.AddChild(
            (TypeOption type) =>
            {
                type.TypeKind = TypeOption.Type.Class;
                configure(type);
            }
        );

    /// <summary>
    /// add struct into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Struct(this NamespaceOption ns, Action<TypeOption> configure) =>
        ns.AddChild(
            (TypeOption type) =>
            {
                type.TypeKind = TypeOption.Type.Struct;
                configure(type);
            }
        );

    /// <summary>
    /// add enum into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Enum(this NamespaceOption ns, Action<TypeOption> configure) =>
        ns.AddChild(
            (TypeOption type) =>
            {
                type.TypeKind = TypeOption.Type.Enum;
                configure(type);
            }
        );
}
