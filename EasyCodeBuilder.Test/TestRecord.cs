using System;
using Xunit.Abstractions;

namespace EasyCodeBuilder.Test;

public class TestRecord
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TestRecord(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private record Something(string name)
    {
        public string Name { get; set; } = name;
    }
    
    [Fact]
    public void Test()
    {
        void Action(Something something)
        {
            something.Name = "hello";
        }

        var something = new Something("world");
        Action(something);

        _testOutputHelper.WriteLine(something.Name);
    }
}