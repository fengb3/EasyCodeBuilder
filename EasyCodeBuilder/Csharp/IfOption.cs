using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class IfOption : CodeOption
{
    public string Condition { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"if ({Condition})");   
    }
}

public class ElseIfOption : CodeOption
{
    public string Condition { get; set; } = "";

    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, $"else if ({Condition})");
    }
}

public class ElseOption : CodeOption
{
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, "else");
    }
}