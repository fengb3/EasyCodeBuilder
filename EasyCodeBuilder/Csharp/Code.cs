using System;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 
/// </summary>
public static partial class Code
{

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static CodeOption Create()
    {
        return new CodeOption();
    }

    /// <summary>
    /// 添加 using 语句
    /// </summary>
    /// <param name="option">代码选项</param>
    /// <param name="usings">using 命名空间列表</param>
    /// <returns>代码选项</returns>
    public static CodeOption Using(this CodeOption option, params string[] usings)
    {
        option.OnChildren += cb => {
            foreach (var u in usings)
            {
                cb.AppendLine($"using {u};");
            }
            cb.AppendLine();
            return cb;
        };
        return option;
    }

    /// <summary>
    /// add and configure a child parent
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="configureChild"></param>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <returns></returns>
    public static TParent AddChild<TParent, TChild>(this TParent parent, Action<TChild> configureChild) where TParent : CodeOption where TChild : CodeOption, new()
    {
        var child = new TChild();
        configureChild?.Invoke(child);
        return parent.AddChild(child);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child"></param>
    /// <typeparam name="TParent"></typeparam>
    /// <typeparam name="TChild"></typeparam>
    /// <returns></returns>
    public static TParent AddChild<TParent, TChild>(this TParent parent, TChild child) where TParent : CodeOption where TChild : CodeOption
    {
        // configureChild?.Invoke(child);
        parent.OnChildren += child.Build;
        return parent;
    }

    /// <summary>
    /// 添加一行或多行代码
    /// </summary>
    /// <param name="option">代码选项</param>
    /// <param name="lines">代码行</param>
    /// <returns>代码选项</returns>
    public static CodeOption AppendLine(this CodeOption option, params string[] lines)
    {
        option.OnChildren += cb => cb.AppendLines(lines);
        return option;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="option"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static CodeOption Namespace(this CodeOption option, Action<NamespaceOption> configure)
        => option.AddChild(configure);

    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="root">根选项</param>
    /// <param name="cb">代码构建器</param>
    /// <returns>生成的代码字符串</returns>
    public static string Build(this CodeOption root, CodeBuilder? cb = null)
    {
        cb ??= new CodeBuilder(' ', 2, "\n{", "}", 1024);

        root.Build(cb);

        return cb.ToString();
    }
}