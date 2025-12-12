using Fengb3.EasyCodeBuilder;

namespace EasyCodeBuilder.Csharp;

public record CodeOption
{
    public Action<CodeBuilder>? OnBegin;
    public Action<CodeBuilder>? OnEnd;
    public Action<CodeBuilder>? OnChildren;
}

public record NamespaceOption : CodeOption
{
    public string Name { get; set; }
}

public record TypeOption : CodeOption
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
}

public static class CsharpCode
{
    public static CodeBuilder Create()
    {
        return new CodeBuilder(" ", 2, "{", "}", 1024);
    }
}

public static class Extensions
{
    public static CodeBuilder Using(this CodeBuilder builder, params string[] usings)
    {
        builder.AppendLines(usings.Select(@using => $"using {@using};").ToArray());
        return builder;
    }

    public static CodeBuilder Namespace(this CodeBuilder builder, Action<NamespaceOption> action)
    {
        var option = new NamespaceOption
        {
            
        };
        
        action(option);
        
        option.OnBegin?.Invoke(builder);
        
        builder.AppendLine($"namespace {option.Name}");
        
        using (builder.Indent)
        {
            option.OnChildren += b => { };
            option.OnChildren?.Invoke(builder);
        }
        
        option.OnEnd?.Invoke(builder);
        

        return builder;
    }

    public static void Class(this NamespaceOption @namespace, Action<TypeOption> action)
    {
        var typeOption = new TypeOption
        { 
            TypeKind = TypeOption.Type.Class
        };
        
        action(typeOption);
        
        @namespace.OnBegin += typeOption.OnBegin;
        @namespace.OnChildren += typeOption.OnChildren;
        @namespace.OnEnd += typeOption.OnEnd;
    }


}