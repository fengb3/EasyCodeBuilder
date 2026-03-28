using System;
using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 属性访问器类型
/// </summary>
public enum PropertyAccessorType
{
    /// <summary>
    /// Getter
    /// </summary>
    Get,
    /// <summary>
    /// Setter
    /// </summary>
    Set,
    /// <summary>
    /// Init
    /// </summary>
    Init
}

/// <summary>
/// 属性访问器选项
/// </summary>
public class PropertyAccessorOption : CodeOption
{
    /// <summary>
    /// 访问器类型
    /// </summary>
    public PropertyAccessorType AccessorType { get; set; } = PropertyAccessorType.Get;

    /// <summary>
    /// 访问器修饰符（如 private, protected 等）
    /// </summary>
    public string? Modifier { get; set; }

    /// <summary>
    /// 是否为表达式体（如 get => _field;）
    /// </summary>
    public bool IsExpressionBody { get; set; } = false;

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var accessorKeyword = AccessorType switch
        {
            PropertyAccessorType.Get => "get",
            PropertyAccessorType.Set => "set",
            PropertyAccessorType.Init => "init",
            _ => "get"
        };

        var modifierPrefix = string.IsNullOrEmpty(Modifier) ? "" : $"{Modifier} ";

        BeforeChildren?.Invoke(cb);

        if (OnChildren == null)
        {
            // 自动访问器：get;
            cb.AppendLine($"{modifierPrefix}{accessorKeyword};");
        }
        else if (IsExpressionBody)
        {
            // 表达式体：get => ...;
            // 使用 StringBuilder 来捕获表达式内容
            var innerBuilder = new System.Text.StringBuilder();
            var tempCb = new CodeBuilder(' ', 2, "{", "}", 256);
            OnChildren(tempCb);
            var expression = tempCb.ToString().Trim();

            if (string.IsNullOrEmpty(expression))
            {
                cb.AppendLine($"{modifierPrefix}{accessorKeyword};");
            }
            else
            {
                // 表达式体，分号在同一行
                var lines = expression.Split('\n');
                if (lines.Length == 1)
                {
                    // 单行表达式
                    cb.AppendLine($"{modifierPrefix}{accessorKeyword} => {lines[0].Trim()};");
                }
                else
                {
                    // 多行表达式
                    cb.AppendLine($"{modifierPrefix}{accessorKeyword} =>");
                    using (cb.Indent)
                    {
                        foreach (var line in lines)
                        {
                            var trimmed = line.Trim();
                            if (!string.IsNullOrEmpty(trimmed))
                            {
                                cb.AppendLine(trimmed);
                            }
                        }
                    }
                    cb.AppendLine(";");
                }
            }
        }
        else
        {
            // 常规访问器：get { ... }
            cb.CodeBlock(OnChildren, $"{modifierPrefix}{accessorKeyword}");
        }

        return cb;
    }
}

/// <summary>
/// 属性访问器选项扩展方法
/// </summary>
public static class PropertyAccessorOptionExtensions
{
    /// <summary>
    /// 设置访问器修饰符
    /// </summary>
    /// <param name="accessor">访问器选项</param>
    /// <param name="modifier">修饰符</param>
    /// <returns>访问器选项</returns>
    public static PropertyAccessorOption WithModifier(this PropertyAccessorOption accessor, string modifier)
    {
        accessor.Modifier = modifier;
        return accessor;
    }

    /// <summary>
    /// 设置为表达式体
    /// </summary>
    /// <param name="accessor">访问器选项</param>
    /// <returns>访问器选项</returns>
    public static PropertyAccessorOption AsExpressionBody(this PropertyAccessorOption accessor)
    {
        accessor.IsExpressionBody = true;
        return accessor;
    }

    /// <summary>
    /// 添加代码行
    /// </summary>
    /// <param name="accessor">访问器选项</param>
    /// <param name="lines">代码行</param>
    /// <returns>访问器选项</returns>
    public static PropertyAccessorOption AppendLines(this PropertyAccessorOption accessor, params string[] lines)
    {
        accessor.OnChildren += cb => cb.AppendLines(lines);
        return accessor;
    }
}

/// <summary>
/// 属性选项（支持自定义 getter/setter）
/// </summary>
public class PropertyOption : CodeOption
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
    /// 是否为表达式体属性（如 => _field;）
    /// </summary>
    public bool IsExpressionBody { get; set; } = false;

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords = string.Join(" ", CsharpKeywordOrdering.OrderForMember(Keywords));

        BeforeChildren?.Invoke(cb);

        if (IsExpressionBody)
        {
            // 表达式体属性：public string Name => _name;
            if (OnChildren != null)
            {
                var tempCb = new CodeBuilder(' ', 2, "{", "}", 256);
                OnChildren(tempCb);
                var expression = tempCb.ToString().Trim();

                if (string.IsNullOrEmpty(expression))
                {
                    cb.AppendLine($"{keywords} {Type} {Name};".Trim());
                }
                else
                {
                    var lines = expression.Split('\n');
                    if (lines.Length == 1)
                    {
                        cb.AppendLine($"{keywords} {Type} {Name} => {lines[0].Trim()};".Trim());
                    }
                    else
                    {
                        cb.AppendLine($"{keywords} {Type} {Name} =>".Trim());
                        using (cb.Indent)
                        {
                            foreach (var line in lines)
                            {
                                var trimmed = line.Trim();
                                if (!string.IsNullOrEmpty(trimmed))
                                {
                                    cb.AppendLine(trimmed);
                                }
                            }
                        }
                        cb.AppendLine(";");
                    }
                }
            }
            else
            {
                cb.AppendLine($"{keywords} {Type} {Name};".Trim());
            }
        }
        else
        {
            // 常规属性：public string Name { get; set; }
            cb.CodeBlock(OnChildren, $"{keywords} {Type} {Name}".Trim());
        }

        return cb;
    }

    /// <summary>
    /// 关键字配置器
    /// </summary>
    private KeywordOptionConfigurator<PropertyOption> KeywordConfigurator => new(this);

    /// <summary>
    /// 公共访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<PropertyOption> Public => KeywordConfigurator.Public;

    /// <summary>
    /// 内部访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<PropertyOption> Internal => KeywordConfigurator.Internal;

    /// <summary>
    /// 私有访问修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<PropertyOption> Private => KeywordConfigurator.Private;

    /// <summary>
    /// 静态修饰符配置器
    /// </summary>
    public KeywordOptionConfigurator<PropertyOption> Static => KeywordConfigurator.Static;
}

/// <summary>
/// 属性选项扩展方法
/// </summary>
public static class PropertyOptionExtensions
{
    /// <summary>
    /// 添加关键字
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="keyword">关键字</param>
    /// <returns>属性选项</returns>
    public static PropertyOption WithKeyword(this PropertyOption prop, string keyword)
    {
        prop.Keywords.Add(keyword);
        return prop;
    }

    /// <summary>
    /// 设置类型
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="type">类型</param>
    /// <returns>属性选项</returns>
    public static PropertyOption WithType(this PropertyOption prop, string type)
    {
        prop.Type = type;
        return prop;
    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="name">名称</param>
    /// <returns>属性选项</returns>
    public static PropertyOption WithName(this PropertyOption prop, string name)
    {
        prop.Name = name;
        return prop;
    }

    /// <summary>
    /// 设置为表达式体属性
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <returns>属性选项</returns>
    public static PropertyOption AsExpressionBody(this PropertyOption prop)
    {
        prop.IsExpressionBody = true;
        return prop;
    }

    /// <summary>
    /// 添加 getter
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="configure">getter 配置委托</param>
    /// <returns>属性选项</returns>
    public static PropertyOption Getter(this PropertyOption prop, Action<PropertyAccessorOption> configure)
        => prop.AddChild(configure);

    /// <summary>
    /// 添加 setter
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="configure">setter 配置委托</param>
    /// <returns>属性选项</returns>
    public static PropertyOption Setter(this PropertyOption prop, Action<PropertyAccessorOption> configure)
        => prop.AddChild<PropertyOption, PropertyAccessorOption>(a =>
        {
            a.AccessorType = PropertyAccessorType.Set;
            configure(a);
        });

    /// <summary>
    /// 添加 init
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="configure">init 配置委托</param>
    /// <returns>属性选项</returns>
    public static PropertyOption Init(this PropertyOption prop, Action<PropertyAccessorOption> configure)
        => prop.AddChild<PropertyOption, PropertyAccessorOption>(a =>
        {
            a.AccessorType = PropertyAccessorType.Init;
            configure(a);
        });

    /// <summary>
    /// 添加自动 getter（get;）
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <returns>属性选项</returns>
    public static PropertyOption AutoGetter(this PropertyOption prop)
        => prop.Getter(g => { });

    /// <summary>
    /// 添加自动 setter（set;）
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <returns>属性选项</returns>
    public static PropertyOption AutoSetter(this PropertyOption prop)
        => prop.Setter(s => { });

    /// <summary>
    /// 添加自动 init（init;）
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <returns>属性选项</returns>
    public static PropertyOption AutoInit(this PropertyOption prop)
        => prop.Init(i => { });

    /// <summary>
    /// 添加代码行（用于表达式体属性）
    /// </summary>
    /// <param name="prop">属性选项</param>
    /// <param name="lines">代码行</param>
    /// <returns>属性选项</returns>
    public static PropertyOption AppendLines(this PropertyOption prop, params string[] lines)
    {
        prop.OnChildren += cb => cb.AppendLines(lines);
        return prop;
    }
}
