using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 
/// </summary>
public class MethodOption : CodeOption
{
    public ICollection<string> Keywords { get; set; } = [];

    public string ReturnType { get; set; } = "void";

    public string Name { get; set; } = "";
    
    public ICollection<string> Parameters { get; set; } = [];

    // public MethodOption()
    // {
    //     // // var indenterStack = new Stack<IDisposable>();
    //     // OnBuild += cb => {
    //     //     var keywords   = string.Join(" ", Keywords);
    //     //     var parameters = string.Join(", ", Parameters);
    //     //     
    //     //     cb.CodeBlock(OnChildren, $"{keywords} {ReturnType} {Name}({parameters})");
    //     //     
    //     //     return cb;
    //     // };
    // }

    public override CodeBuilder Build(CodeBuilder cb)
    {
        var keywords   = string.Join(" ", Keywords);
        var parameters = string.Join(", ", Parameters);
        
        cb.CodeBlock(OnChildren, $"{keywords} {ReturnType} {Name}({parameters})");
        
        return cb;
    }
}

public static class MethodOptionExtensions
{
    public static MethodOption WithKeyword(this MethodOption method, string keyword)
    {
        method.Keywords.Add(keyword);
        return method;
    }
    
    public static MethodOption WithName(this MethodOption method, string name)
    {
        method.Name = name;
        return method;
    }
    
    public static MethodOption WithReturnType(this MethodOption method, string returnType)
    {
        method.ReturnType = returnType;
        return method;
    }
    
    public static MethodOption WithParameters(this MethodOption method, params string[] parameter)
    {
        method.Parameters = parameter;
        return method;
    }
}