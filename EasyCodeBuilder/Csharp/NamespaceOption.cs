using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class NamespaceOption : CodeOption
{
    public string Name { get; set; } = "";
    
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"namespace {Name}");
    }
}