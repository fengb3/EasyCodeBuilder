using System;
using Fengb3.EasyCodeBuilder.Csharp;
using static Fengb3.EasyCodeBuilder.Csharp.CsharpCode;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test;

public class TestRecord(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void TestNamespace()
    {
        var op = Create()
            .Using("System")
            .Using("System.Linq", "System.Collections.Generic")
            .Namespace(@namespace => {
                @namespace.Name = "MyNamespace";
            })
            .Build();
        
        testOutputHelper.WriteLine(op);
    }
    
    [Fact]
    public void TestType()
    {
        var op = Create()
            .Namespace(@namespace => {
                @namespace.Name = "MyNamespace";
                @namespace.Type(type => {
                    type.TypeKind = TypeOption.Type.Record;
                    type.Keywords.Add("public");
                    type.OnBegin += cb => cb.AppendLine("MyRecord(int Id, string Name);");
                });
                @namespace.Class(cls => {
                    cls.Name = "MyClass";
                    cls.Keywords.Add("public");
                    cls.OnBegin += cb => cb.AppendLine("// Class body");
                });
            })
            .Build();
        
        testOutputHelper.WriteLine(op);
    }
}