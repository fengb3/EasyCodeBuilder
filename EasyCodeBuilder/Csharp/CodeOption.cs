using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 
/// </summary>
public class CodeOption
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cb"></param>
    /// <returns></returns>
    public virtual CodeBuilder Build(CodeBuilder cb)
    {
        OnChildren?.Invoke(cb);
        return cb;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public CodeRenderFragment? OnChildren = null;
}


public static class Extensions
{
    public static CodeOption AddChild(this CodeOption parent, CodeOption child)
    {
        parent.OnChildren += child.Build;
        // parent.OnChildren += child.OnChildren;
        // parent.OnChildren += child.OnEnd;
        return parent;
    }
    
    public static CodeOption AppendLine(this CodeOption option, params string[] lines)
    {
        option.OnChildren += cb => cb.AppendLines(lines);
        return option;
    }
}