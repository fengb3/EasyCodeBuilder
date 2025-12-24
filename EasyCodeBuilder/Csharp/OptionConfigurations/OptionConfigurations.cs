using System;

namespace Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;

/// <summary>
/// 关键字配置器扩展方法
/// </summary>
public static class KeywordConfiguratorExtensions
{
    #region private methods

    private static void AddType(
        KeywordOptionConfigurator<NamespaceOption> configurator,
        TypeOption.Type typeKind,
        Action<TypeOption> configure
    )
    {
        configurator.Parent.AddChild<NamespaceOption, TypeOption>(to =>
        {
            to.TypeKind = typeKind;
            // ApplyKeywords(configurator, keyword => to.WithKeyword(keyword));
            configurator.Configure(keyword => to.WithKeyword(keyword));
            configure(to);
        });
    }

    private static void AddMethod(
        KeywordOptionConfigurator<TypeOption> configurator,
        Action<MethodOption> configure
    )
    {
        configurator.Parent.AddChild<TypeOption, MethodOption>(mo =>
        {
            // ApplyKeywords(configurator, keyword => mo.WithKeyword(keyword));
            configurator.Configure(keyword => mo.WithKeyword(keyword));
            configure(mo);
        });
    }
    #endregion

    #region Add type to namespace

    /// <summary>
    /// 添加类
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">类选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Class(
        this KeywordOptionConfigurator<NamespaceOption> configurator,
        Func<TypeOption, TypeOption> configure
    )
    {
        AddType(configurator, TypeOption.Type.Class, to => configure(to));
        return configurator.Parent;
    }

    /// <summary>
    /// 添加类
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">类选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Class(
        this KeywordOptionConfigurator<NamespaceOption> configurator,
        Action<TypeOption> configure
    )
    {
        AddType(configurator, TypeOption.Type.Class, configure);
        return configurator.Parent;
    }

    /// <summary>
    /// 添加结构体
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">结构体选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Struct(
        this KeywordOptionConfigurator<NamespaceOption> configurator,
        Func<TypeOption, TypeOption> configure
    )
    {
        AddType(configurator, TypeOption.Type.Struct, to => configure(to));
        return configurator.Parent;
    }

    /// <summary>
    /// 添加结构体
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">结构体选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Struct(
        this KeywordOptionConfigurator<NamespaceOption> configurator,
        Action<TypeOption> configure
    )
    {
        AddType(configurator, TypeOption.Type.Struct, configure);
        return configurator.Parent;
    }

    /// <summary>
    /// 添加枚举
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">枚举选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Enum(
        this KeywordOptionConfigurator<NamespaceOption> configurator,
        Func<TypeOption, TypeOption> configure
    )
    {
        AddType(configurator, TypeOption.Type.Enum, to => configure(to));
        return configurator.Parent;
    }

    /// <summary>
    /// 添加枚举
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">枚举选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Enum(
        this KeywordOptionConfigurator<NamespaceOption> configurator,
        Action<TypeOption> configure
    )
    {
        AddType(configurator, TypeOption.Type.Enum, configure);
        return configurator.Parent;
    }

    #endregion

    #region Add member to type

    /// <summary>
    /// 添加方法
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">方法选项配置委托</param>
    /// <returns>类型选项</returns>
    public static TypeOption Method(
        this KeywordOptionConfigurator<TypeOption> configurator,
        Func<MethodOption, MethodOption> configure
    )
    {
        AddMethod(configurator, mo => configure(mo));
        return configurator.Parent;
    }

    /// <summary>
    /// 添加方法
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">方法选项配置委托</param>
    /// <returns>类型选项</returns>
    public static TypeOption Method(
        this KeywordOptionConfigurator<TypeOption> configurator,
        Action<MethodOption> configure
    )
    {
        AddMethod(configurator, configure);
        return configurator.Parent;
    }

    /// <summary>
    /// 添加自动属性
    /// </summary>
    /// <param name="configurator"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption AutoProperty(
        this KeywordOptionConfigurator<TypeOption> configurator,
        Func<AutoPropertyOption, AutoPropertyOption> configure
    )
    {
        configurator.Parent.AddChild<TypeOption, AutoPropertyOption>(po =>
        {
            // ApplyKeywords(configurator, keyword => po.WithKeyword(keyword));
            configurator.Configure(keyword => po.WithKeyword(keyword));
            configure(po);
        });
        return configurator.Parent;
    }

    /// <summary>
    /// 添加自动属性
    /// </summary>
    /// <param name="configurator"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static TypeOption AutoProperty(
        this KeywordOptionConfigurator<TypeOption> configurator,
        Action<AutoPropertyOption> configure
    )
    {
        configurator.Parent.AddChild<TypeOption, AutoPropertyOption>(po =>
        {
            // ApplyKeywords(configurator, keyword => po.WithKeyword(keyword));
            configurator.Configure(keyword => po.WithKeyword(keyword));
            configure(po);
        });
        return configurator.Parent;
    }

    public static TypeOption Field(
        this KeywordOptionConfigurator<TypeOption> configurator,
        Func<FieldOption, FieldOption> configure
    )
    {
        configurator.Parent.AddChild<TypeOption, FieldOption>(fo =>
        {
            // ApplyKeywords(configurator, keyword => fo.WithKeyword(keyword));
            configurator.Configure(keyword => fo.WithKeyword(keyword));
            configure(fo);
        });
        return configurator.Parent;
    }

    public static TypeOption Field(
        this KeywordOptionConfigurator<TypeOption> configurator,
        Action<FieldOption> configure
    )
    {
        configurator.Parent.AddChild<TypeOption, FieldOption>(fo =>
        {
            // ApplyKeywords(configurator, keyword => fo.WithKeyword(keyword));
            configurator.Configure(keyword => fo.WithKeyword(keyword));
            configure(fo);
        });
        return configurator.Parent;
    }

    #endregion
}
