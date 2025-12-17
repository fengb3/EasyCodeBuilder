using System.Collections.Generic;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 
/// </summary>
public class CodeOption
{
    /// <summary>
    /// called to build code
    /// </summary>
    /// <param name="cb"></param>
    /// <returns></returns>
    public virtual CodeBuilder Build(CodeBuilder cb)
    {
        OnChildren?.Invoke(cb);
        return cb;
    }
    
    /// <summary>
    /// called to build children code
    /// </summary>
    public CodeRenderFragment? OnChildren = null;
}