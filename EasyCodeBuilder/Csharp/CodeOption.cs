using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 代码选项基类
/// </summary>
public class CodeOption
{
    /// <summary>
    /// 构建代码
    /// </summary>
    /// <param name="cb">代码构建器</param>
    /// <returns>代码构建器</returns>
    public virtual CodeBuilder Build(CodeBuilder cb)
    {
        OnChildren?.Invoke(cb);
        return cb;
    }
    
    /// <summary>
    /// 子节点构建委托
    /// </summary>
    public CodeRenderFragment? OnChildren = null;
}