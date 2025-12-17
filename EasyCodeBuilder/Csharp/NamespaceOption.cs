using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class NamespaceOption : CodeOption
{
    public string Name { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"namespace {Name}");
    }
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
    public static NamespaceOption Type(this NamespaceOption ns, Action<TypeOption> configure)
         => ns.AddConfiguredChild(configure);
    

    /// <summary>
    /// add class into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Class(this NamespaceOption ns, Action<TypeOption> configure)
        => ns.AddConfiguredChild((TypeOption type) => {
            type.TypeKind = TypeOption.Type.Class;
            configure(type);
        });


    /// <summary>
    /// add struct into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Struct(this NamespaceOption ns, Action<TypeOption> configure)
        => ns.AddConfiguredChild((TypeOption type) => {
            type.TypeKind = TypeOption.Type.Struct;
            configure(type);
        });

    /// <summary>
    /// add enum into namespace
    /// </summary>
    /// <param name="ns"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static NamespaceOption Enum(this NamespaceOption ns, Action<TypeOption> configure)
        => ns.AddConfiguredChild((TypeOption type) => {
            type.TypeKind = TypeOption.Type.Enum;
            configure(type);
        });
}