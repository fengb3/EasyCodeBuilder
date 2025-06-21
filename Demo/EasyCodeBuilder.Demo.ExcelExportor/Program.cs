using OfficeOpenXml;
using Fengb3.EasyCodeBuilder;

// using GeneratedData;

namespace EasyCodeBuilder.Demo.ExcelExportor;

/// <summary>
/// Excel数据列信息
/// </summary>
public record ExcelColumnInfo(string Name, string Type, string Comment);

/// <summary>
/// Excel数据硬编码生成器演示
/// 支持从Excel文件读取数据并生成硬编码的C#类
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Excel数据硬编码生成器演示 ===");

        // 设置EPPlus许可证上下文
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        try
        {
            // 从Excel生成C#代码
            string generatedCode = GenerateCodeFromExcel("TestExcel.xlsx");

            // 输出生成的代码
            // Console.WriteLine("生成的C#代码：");
            // Console.WriteLine(new string('=', 80));
            // Console.WriteLine(generatedCode);
            // Console.WriteLine(new string('=', 80));

            // 保存到文件
            string outputFile = "GeneratedData.cs";
            File.WriteAllText(outputFile, generatedCode);
            Console.WriteLine($"代码已保存到：{outputFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"发生错误：{ex.Message}");
        }

        // Console.WriteLine("按任意键退出...");
        // Console.ReadKey();
    }

    /// <summary>
    /// 从Excel文件生成C#代码
    /// </summary>
    /// <param name="excelFilePath">Excel文件路径</param>
    /// <returns>生成的C#代码</returns>
    private static string GenerateCodeFromExcel(string excelFilePath)
    {
        using var package = new ExcelPackage(new FileInfo(excelFilePath));
        var worksheet = package.Workbook.Worksheets.First();

        // 解析表头信息
        var columns = ParseHeader(worksheet);
        Console.WriteLine($"解析到 {columns.Count} 个列：");
        foreach (var col in columns)
        {
            Console.WriteLine($"  - {col.Name}({col.Type}): {col.Comment}");
        }

        // 读取数据行
        var dataRows = ReadDataRows(worksheet, columns.Count);
        Console.WriteLine($"读取到 {dataRows.Count} 行数据");

        // 生成C#代码
        return GenerateCSharpCode(columns, dataRows);
    }

    /// <summary>
    /// 解析Excel表头信息
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <returns>列信息列表</returns>
    private static List<ExcelColumnInfo> ParseHeader(ExcelWorksheet worksheet)
    {
        var columns = new List<ExcelColumnInfo>();
        int col = 1;

        while (worksheet.Cells[1, col].Value != null)
        {
            string headerValue = worksheet.Cells[1, col].Value.ToString()!;

            // 解析格式：字段名:数据类型 #注释
            var parts = headerValue.Split('#', 2);
            string fieldPart = parts[0].Trim();
            string comment = parts.Length > 1 ? parts[1].Trim() : "";

            // 分离字段名和数据类型
            var fieldTypeParts = fieldPart.Split(':', 2);
            if (fieldTypeParts.Length != 2)
                throw new InvalidOperationException($"表头格式错误：{headerValue}，应该是 '字段名:数据类型 #注释'");

            string fieldName = fieldTypeParts[0].Trim();
            string dataType = fieldTypeParts[1].Trim();

            columns.Add(new ExcelColumnInfo(fieldName, dataType, comment));
            col++;
        }

        return columns;
    }

    /// <summary>
    /// 读取数据行
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="columnCount">列数</param>
    /// <returns>数据行列表</returns>
    private static List<List<object?>> ReadDataRows(ExcelWorksheet worksheet, int columnCount)
    {
        var dataRows = new List<List<object?>>();
        int row = 2; // 从第2行开始读取数据

        while (worksheet.Cells[row, 1].Value != null)
        {
            var rowData = new List<object?>();
            for (int col = 1; col <= columnCount; col++)
            {
                rowData.Add(worksheet.Cells[row, col].Value);
            }

            dataRows.Add(rowData);
            row++;
        }

        return dataRows;
    }

    /// <summary>
    /// 生成C#代码
    /// </summary>
    /// <param name="columns">列信息</param>
    /// <param name="dataRows">数据行</param>
    /// <returns>生成的C#代码</returns>
    private static string GenerateCSharpCode(List<ExcelColumnInfo> columns, List<List<object?>> dataRows)
    {
        var builder = new CSharpCodeBuilder();

        // 添加using语句
        builder.Using("System", "System.Collections.Generic", "System.Linq");

        // 添加命名空间
        builder.Namespace("GeneratedData", ns =>
        {
            // 生成数据模型类
            ns.Class("DataModel")(MakeDataModel(ns, columns))
                .AppendLine()
                .Class("DataProvider", "public static")(cls =>
                {
                    MakeFactoryMethod(cls, "DataModel", columns)(cls);

                    cls.Property("List<DataModel>", "_datas", "private static");


                    cls.Method("DataProvider", "", "static")(method => method
                        .AppendLine($"_datas = new List<DataModel>();")
                        .AppendBatch(dataRows, (b, row) =>
                        {
                            // 使用索引同时获取值和对应的列类型
                            var formattedValues = row.Select((value, index) =>
                                FormatValue(value, columns[index].Type)).ToArray();
                            return b << $"_datas.Add(Create({string.Join(", ", formattedValues)}));";
                        })
                    );

                    _ = cls
                        << ""
                        << ("/// <summary>")
                        << ("/// 获取硬编码的数据列表")
                        << ("/// </summary>")
                        << ("/// <returns>数据列表</returns>");

                    cls.Method("GetData", "List<DataModel>", "public static")(method =>
                        method << "return _datas;"
                    );

                    cls.AppendLine();

                    return cls;
                });
        });

        return builder.ToString();
    }


    private static Func<CSharpCodeBuilder, CSharpCodeBuilder> MakeDataModel(CSharpCodeBuilder builder,
        List<ExcelColumnInfo> columns)
    {
        return builder => builder.AppendBatch(columns, (builder, column) =>
            (builder
             << $"/// <summary>"
             << $"/// {column.Comment}"
             << $"/// </summary>")
            .Property(column.Type, column.Name)
        );
    }

    public static Func<CSharpCodeBuilder, CSharpCodeBuilder> MakeFactoryMethod(CSharpCodeBuilder builder,
        string className, List<ExcelColumnInfo> columns)
    {
        return builder =>
            builder.Method("Create", className, "public static",
                string.Join(", ", columns.Select(c => $"{c.Type} {c.Name}")))(method =>
                (method << "var model = new DataModel();")
                .AppendBatch(columns, (builder, column) =>
                    (builder << $"model.{column.Name} = {column.Name};")
                )
                .AppendLine("return model;")
            );
    }

    /// <summary>
    /// 格式化值为C#代码
    /// </summary>
    /// <param name="value">原始值</param>
    /// <param name="type">数据类型</param>
    /// <returns>格式化后的值</returns>
    private static string FormatValue(object? value, string type)
    {
        if (value == null) return "null";

        return type.ToLower() switch
        {
            "string" => $"\"{value}\"",
            "int" => value.ToString()!,
            "double" or "float" => $"{value}",
            "bool" => value.ToString()!.ToLower(),
            "datetime" => $"DateTime.Parse(\"{value}\")",
            _ => $"\"{value}\""
        };
    }
}