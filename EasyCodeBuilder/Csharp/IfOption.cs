using System;

namespace Fengb3.EasyCodeBuilder.Csharp;

public class IfOption : CodeOption
{
    public string Condition { get; set; } = "";

    // public IfOption()
    // {
    //     OnBuild += cb => {
    //         var line = $"if ({Condition})";
    //         cb.CodeBlock(OnChildren, line);
    //         return cb;
    //     };
    // }

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

    // public ElseIfOption()
    // {
    //     IDisposable? indenter = null;
    //     OnBuild += cb => {
    //         cb.AppendLines($"else if ({Condition})", "{");
    //         indenter = cb.Indent;
    //         return cb;
    //     };
    //
    //     OnEnd += cb => {
    //         indenter?.Dispose();
    //         cb.AppendLines("}");
    //         return cb;
    //     };
    // }
}

public class ElseOption : CodeOption
{
    // public ElseOption()
    // {
    //     IDisposable? indenter = null;
    //     OnBuild += cb => {
    //         cb.AppendLines("else", "{");
    //         indenter = cb.Indent;
    //         return cb;
    //     };
    //
    //     OnEnd += cb => {
    //         indenter?.Dispose();
    //         cb.AppendLines("}");
    //         return cb;
    //     };
    // }
    
    public override CodeBuilder Build(CodeBuilder cb)
    {
        return cb.CodeBlock(OnChildren, "else");
    }
}