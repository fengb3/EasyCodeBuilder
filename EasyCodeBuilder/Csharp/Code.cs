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

    public static CodeOption Namespace(this CodeOption root, Action<NamespaceOption> configure)
    {
        var @namespace = new NamespaceOption();
        configure(@namespace);
        root.AddChild(@namespace);
        return root;
    }

    public static CodeOption Type(this CodeOption @namespace, Action<TypeOption> configure)
    {
        var type = new TypeOption();
        configure(type);

        @namespace.AddChild(type);

        return @namespace;
    }
    
    public static CodeOption Class(this CodeOption @namespace, Action<TypeOption> configure)
    {
        return @namespace.Type(option => {
            option.TypeKind = TypeOption.Type.Class;
            configure(option);
        });
    }
    
    public static CodeOption Struct(this CodeOption @namespace, Action<TypeOption> configure)
    {
        return @namespace.Type(option => {
            option.TypeKind = TypeOption.Type.Struct;
            configure(option);
        });
    }
    
    public static CodeOption Interface(this CodeOption @namespace, Action<TypeOption> configure)
    {
        return @namespace.Type(option => {
            option.TypeKind = TypeOption.Type.Interface;
            configure(option);
        });
    }
    
    public static CodeOption Method(this CodeOption type, Action<MethodOption> configure)
    {
        var method = new MethodOption();
        configure(method);
        type.AddChild(method);
        return type;
    }
    
    public static CodeOption Property(this CodeOption type, Action<AutoPropertyOption> configure)
    {
        var prop = new AutoPropertyOption();
        configure(prop);
        type.AddChild(prop);

        return type;
    }
    
    public static CodeOption If(this CodeOption parent, Action<IfOption> configure)
    {
        var @if = new IfOption();
        configure(@if);
        parent.AddChild(@if);
        return parent;
    }
    
    public static CodeOption ElseIf(this IfOption parent, Action<ElseIfOption> configure)
    {
        var elseif = new ElseIfOption();
        configure(elseif);
        parent.AddChild(elseif);
        return parent;
    }
    
    public static CodeOption Else(this CodeOption parent, Action<ElseOption> configure)
    {
        var @else = new ElseOption();
        configure(@else);
        parent.AddChild(@else);
        return parent;
    }

    public static string Build(this CodeOption root)
    {
        var cb = new CodeBuilder(' ', 2, "\n{", "}", 1024);

        root.Build(cb);
        // root.OnChildren?.Invoke(cb);
        // root.OnEnd?.Invoke(cb);

        return cb.ToString();
    }
}