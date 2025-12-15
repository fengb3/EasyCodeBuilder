using System;
using System.Collections.Generic;
using System.Linq;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 
/// </summary>
public delegate CodeBuilder CodeRenderFragment(CodeBuilder builder);

public class CodeOption
{
    public CodeRenderFragment? OnBegin;
    public CodeRenderFragment? OnEnd;
    public CodeRenderFragment? OnChildren;
}

public class NamespaceOption : CodeOption
{
    public string Name { get; set; } = "";

    public NamespaceOption()
    {
        IDisposable? indenter = null;
        OnBegin += cb => {
            cb.AppendLines($"namespace {Name}", "{");
            indenter = cb.Indent;
            return cb;
        };

        OnEnd += cb => {
            indenter?.Dispose();
            cb.AppendLine("}");
            return cb;
        };
    }
}

public class TypeOption : CodeOption
{
    public enum Type
    {
        Class,
        Struct,
        Interface,
        Enum,
        Record
    }

    public Type TypeKind { get; set; } = Type.Class;

    public ICollection<string> Keywords { get; set; } = [];

    public string Name { get; set; }

    public TypeOption()
    {
        var indenterStack = new Stack<IDisposable>();
        OnBegin += cb => {
            var keywords    = string.Join(" ", Keywords);
            var typeKeyword = TypeKind.ToString().ToLower();
            cb.AppendLines($"{keywords} {typeKeyword} {Name}", "{");
            indenterStack.Push(cb.Indent);
            return cb;
        };

        OnEnd += cb => {
            if (indenterStack.Count > 0)
            {
                indenterStack.Pop().Dispose();
            }
            cb.AppendLine("}");
            return cb;
        };
    }
}

/// <summary>
/// 
/// </summary>
public static class CsharpCode
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static CodeOption Create()
    {
        return new();
    }

    public static CodeOption Using(this CodeOption option, params string[] usings)
    {
        option.OnBegin += cb => {
            foreach (var u in usings)
            {
                cb.AppendLine($"using {u};");
            }
            cb.AppendLine();
            return cb;
        };
        return option;
    }

    public static CodeOption Namespace(this CodeOption root, Action<NamespaceOption> configure)
    {
        var option = new NamespaceOption();
        configure(option);
        root.OnBegin    += option.OnBegin;
        root.OnEnd      += option.OnEnd;
        root.OnChildren += option.OnChildren;
        return root;
    }

    public static CodeOption Type(this CodeOption @namespace, Action<TypeOption> configure)
    {
        var option = new TypeOption();
        configure(option);

        @namespace.OnChildren += option.OnBegin;
        @namespace.OnChildren += option.OnChildren;
        @namespace.OnChildren += option.OnEnd;

        return @namespace;
    }
    
    public static CodeOption Class(this CodeOption @namespace, Action<TypeOption> configure)
    {
        return @namespace.Type(option => {
            option.TypeKind = TypeOption.Type.Class;
            configure(option);
        });
    }

    public static string Build(this CodeOption root)
    {
        var cb = new CodeBuilder(" ", 2, "{", "}", 1024);

        root.OnBegin?.Invoke(cb);
        root.OnChildren?.Invoke(cb);
        root.OnEnd?.Invoke(cb);

        return cb.ToString();
    }
}