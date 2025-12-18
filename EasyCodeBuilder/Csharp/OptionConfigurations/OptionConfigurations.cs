using System;

namespace Fengb3.EasyCodeBuilder.Csharp.OptionConfigurations;

/// <summary>
/// 关键字配置器扩展方法
/// </summary>
public static class KeywordConfiguratorExtensions
{
    /// <summary>
    /// 添加类
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">类选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Class(this KeywordOptionConfigurator<NamespaceOption> configurator, Action<TypeOption> configure)
    {
        configurator.Parent.AddChild<NamespaceOption, TypeOption>(to => {
            to.TypeKind = TypeOption.Type.Class;
            configurator.Configure(keyword => { to.WithKeyword(keyword);});
            configure(to);
        });
        return configurator.Parent;
    }
    
    /// <summary>
    /// 添加结构体
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">结构体选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Struct(this KeywordOptionConfigurator<NamespaceOption> configurator, Action<TypeOption> configure)
    {
        configurator.Parent.AddChild<NamespaceOption, TypeOption>(to => {
            to.TypeKind = TypeOption.Type.Struct;
            configurator.Configure(keyword => { to.WithKeyword(keyword);});
            configure(to);
        });
        return configurator.Parent;
    }
    
    /// <summary>
    /// 添加枚举
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">枚举选项配置委托</param>
    /// <returns>命名空间选项</returns>
    public static NamespaceOption Enum(this KeywordOptionConfigurator<NamespaceOption> configurator, Action<TypeOption> configure)
    {
        configurator.Parent.AddChild<NamespaceOption, TypeOption>(to => {
            to.TypeKind = TypeOption.Type.Enum;
            configurator.Configure(keyword => { to.WithKeyword(keyword);});
            configure(to);
        });
        return configurator.Parent;
    }
    
    /// <summary>
    /// 添加方法
    /// </summary>
    /// <param name="configurator">关键字配置器</param>
    /// <param name="configure">方法选项配置委托</param>
    /// <returns>类型选项</returns>
    public static TypeOption Method(this KeywordOptionConfigurator<TypeOption> configurator, Action<MethodOption> configure)
    {
        configurator.Parent.AddChild<TypeOption, MethodOption>(mo => {
            configurator.Configure(keyword => { mo.WithKeyword(keyword);});
            configure(mo);
        });
        return configurator.Parent;
    }
}