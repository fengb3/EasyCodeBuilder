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
    public static TParent AddChildByConfiguration<TParent, TChild>(this TParent parent, Action<TChild> configureChild) where TParent : CodeOption where TChild : CodeOption, new()
    {
        var child = new TChild();
        configureChild(child);
        parent.OnChildren += child.Build;
        return parent;
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
        parent.OnChildren += child.Build;
        return parent;
    }
    
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
        => option.AddChildByConfiguration(configure);

    public static string Build(this CodeOption root)
    {
        var cb = new CodeBuilder(' ', 2, "\n{", "}", 1024);

        root.Build(cb);

        return cb.ToString();
    }
}