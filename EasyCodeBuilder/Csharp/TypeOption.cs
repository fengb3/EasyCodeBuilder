using System;
using System.Collections.Generic;

// using Fengb3.EasyCodeBuilder.Csharp.Abstraction;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 类型选项
/// </summary>
public class TypeOption : CodeOption
{
    /// <summary>
    /// 类型种类
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// 类
        /// </summary>
        Class,
        /// <summary>
        /// 结构体
        /// </summary>
        Struct,
        /// <summary>
        /// 接口
        /// </summary>
        Interface,
        /// <summary>
        /// 枚举
        /// </summary>
        Enum,
        /// <summary>
        /// 记录
        /// </summary>
        Record
    }

    /// <summary>
    /// 类型种类
    /// </summary>
    public Type TypeKind { get; set; } = Type.Class;

    /// <summary>
    /// 关键字列表
    /// </summary>
    public ICollection<string> Keywords { get; set; } = new HashSet<string>();

    /// <summary>
    /// 基类列表
    /// </summary>
    public ICollection<string> BaseTypes { get; set; } = new HashSet<string>();

    /// <summary>
    /// 类型名称
    /// </summary>
    public string Name { get; set; } = "UnnamedType";

    /// <inheritdoc />
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords    = string.Join(" ", CsharpKeywordOrdering.OrderForType(Keywords));
        var typeKeyword = TypeKind.ToString().ToLower();
        if (BaseTypes.Count > 0)
        {
            Name += " : " + string.Join(", ", BaseTypes);
        }

        cb.CodeBlock(OnChildren, $"{keywords} {typeKeyword} {Name}".Trim());
        return cb;
    }

    /// <summary>
    /// 关键字配置器
    /// </summary>
    public KeywordOptionConfigurator<TypeOption> KeywordConfigurator => new(this);

    /// <summary>
    /// 公共访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<TypeOption> Public => KeywordConfigurator.Public;

    /// <summary>
    /// 内部访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<TypeOption> Internal => KeywordConfigurator.Internal;

    /// <summary>
    /// 私有访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<TypeOption> Private => KeywordConfigurator.Private;
    
    /// <summary>
    /// 静态修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<TypeOption> Static => KeywordConfigurator.Static;
}

/// <summary>
/// extensions for TypeOption
/// </summary>
public static class TypeOptionExtensions
{
    /// <summary>
    /// 设置类型种类
    /// </summary>
    /// <param name="type">类型选项</param>
    /// <param name="typeKind">类型种类</param>
    /// <returns>类型选项</returns>
    public static TypeOption WithTypeKind(this TypeOption type, TypeOption.Type typeKind)
    {
        type.TypeKind = typeKind;
        return type;
    }

    /// <summary>
    /// set name of type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static TypeOption WithName(this TypeOption type, string name)
    {
        type.Name = name;
        return type;
    }

    /// <summary>
    /// add keywords into type, this can be public, private, abstract, sealed, partial, etc. duplicates are ignored
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keywords"></param>
    /// <returns></returns>
    public static TypeOption WithKeywords(this TypeOption type, params string[] keywords)
    {
        foreach (var keyword in keywords)
            type.Keywords.Add(keyword);
        return type;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keyword"></param>
    /// <returns></returns>
    public static TypeOption WithKeyword(this TypeOption type, string keyword)
    {
        type.Keywords.Add(keyword);
        return type;
    }

    /// <summary>
    /// add base types or interface to inherit or implement, duplicates are ignored
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseTypes"></param>
    /// <returns></returns>
    public static TypeOption Inherit(this TypeOption type, params string[] baseTypes)
    {
        foreach (var baseType in baseTypes)
            type.BaseTypes.Add(baseType);
        return type;
    }

    /// <summary>
    /// 添加构造函数
    /// </summary>
    /// <param name="type">类型选项</param>
    /// <param name="configure">构造函数配置委托</param>
    /// <returns>类型选项</returns>
    public static TypeOption Constructor(this TypeOption type, Action<ConstructorOption> configure)
    {
        configure += ctor => ctor.WithName(type.Name);
        return type.AddChild(configure);
    }

    /// <summary>
    ///  add method into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption Method(this TypeOption type, Func<MethodOption, MethodOption> configure)
        => type.Method(m => { configure(m); });


    /// <summary>
    ///  add method into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption Method(this TypeOption type, Action<MethodOption> configure)
        => type.AddChild(configure);

    /// <summary>
    /// add auto property into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption AutoProperty(this TypeOption type, Func<AutoPropertyOption, AutoPropertyOption> configure)
        => type.AutoProperty(p => { configure(p); });
    
    /// <summary>
    /// add auto property into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption AutoProperty(this TypeOption type, Action<AutoPropertyOption> configure)
        => type.AddChild(configure);

    /// <summary>
    /// add field into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption Field(this TypeOption type, Func<FieldOption, FieldOption> configure)
        => type.Field(f => { configure(f); });

    /// <summary>
    /// add field into type
    /// </summary>
    /// <param name="type"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption Field(this TypeOption type, Action<FieldOption> configure)
        => type.AddChild(configure);
}