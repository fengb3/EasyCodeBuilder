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
    /// 生成C#代码 - 改进版本
    /// </summary>
    /// <param name="columns">列信息</param>
    /// <param name="dataRows">数据行</param>
    /// <returns>生成的C#代码</returns>
    private static string GenerateCSharpCode(List<ExcelColumnInfo> columns, List<List<object?>> dataRows)
    {
        if (columns == null || columns.Count == 0)
            throw new ArgumentException("列信息不能为空", nameof(columns));

        if (dataRows == null)
            throw new ArgumentNullException(nameof(dataRows));

        var codeGenerator = new CSharpDataClassGenerator(columns, dataRows);
        return codeGenerator.Generate();
    }

    /// <summary>
    /// C# 数据类代码生成器
    /// </summary>
    private class CSharpDataClassGenerator
    {
        private readonly List<ExcelColumnInfo> _columns;
        private readonly List<List<object?>> _dataRows;
        private readonly IValueFormatter _formatter;

        public CSharpDataClassGenerator(List<ExcelColumnInfo> columns, List<List<object?>> dataRows)
        {
            _columns = columns ?? throw new ArgumentNullException(nameof(columns));
            _dataRows = dataRows ?? throw new ArgumentNullException(nameof(dataRows));
            _formatter = new CSharpValueFormatter();
        }

        public string Generate()
        {
            var builder = new CSharpCodeBuilder();

            builder.Using("System", "System.Collections.Generic", "System.Linq");

            builder.Namespace("GeneratedData", ns =>
            {
                GenerateDataModel(ns);
                ns.AppendLine();
                GenerateDataProvider(ns);
            });

            return builder.ToString();
        }

        private void GenerateDataModel(CSharpCodeBuilder builder)
        {
            builder.Class("DataModel", cls =>
            {
                cls.AppendBatch(_columns, (b, column) =>
                    b.AppendLine($"/// <summary>")
                     .AppendLine($"/// {column.Comment}")
                     .AppendLine($"/// </summary>")
                     .Property(column.Type, column.Name, "public", null, "{ get; set; }")
                );
                return cls;
            });
        }

        private void GenerateDataProvider(CSharpCodeBuilder builder)
        {
            builder.Class("DataProvider", cls =>
            {
                // 生成私有静态字段
                cls.Field("List<DataModel>", "_datas", "private static");
                cls.AppendLine();

                // 生成静态构造函数
                GenerateStaticConstructor(cls);
                cls.AppendLine();

                // 生成工厂方法
                GenerateCreateMethod(cls);
                cls.AppendLine();

                // 生成获取数据方法
                GenerateGetDataMethod(cls);

                return cls;
            }, "public static");
        }

        private void GenerateStaticConstructor(CSharpCodeBuilder builder)
        {
            builder.Constructor("DataProvider", "", "static")(ctor =>
                ctor
                .AppendLine($"_datas = new List<DataModel>({_dataRows.Count});")
                .AppendBatch(_dataRows, (b, row, idx) => b
                    .AppendLine($"_datas[{idx}] = Create({string.Join(", ", FormatRowValues(row))});")
                )
            
            );
        }

        private void GenerateCreateMethod(CSharpCodeBuilder builder)
        {
            var parameters = string.Join(", ", _columns.Select(c => $"{c.Type} {c.Name}"));

            builder.AppendLine("/// <summary>")
                   .AppendLine("/// 创建数据模型实例")
                   .AppendLine("/// </summary>")
                   .Method("Create", "DataModel", "public static", parameters)(method =>
                   {
                       method.AppendLine("return new DataModel")
                             .AppendLine("{");

                       method.AppendBatch(_columns, (b, column) =>
                           b.AppendLine($"    {column.Name} = {column.Name},")
                       );

                       method.AppendLine("};");
                       return method;
                   });
        }

        private void GenerateGetDataMethod(CSharpCodeBuilder builder)
        {
            builder.AppendLine("/// <summary>")
                   .AppendLine("/// 获取硬编码的数据列表")
                   .AppendLine("/// </summary>")
                   .AppendLine("/// <returns>数据列表</returns>")
                   .Method("GetData", "List<DataModel>", "public static")(method =>
                       method.AppendLine("return _datas;")
                   );
        }

        private string[] FormatRowValues(List<object?> row)
        {
            if (row.Count != _columns.Count)
                throw new InvalidOperationException($"数据行列数({row.Count})与表头列数({_columns.Count})不匹配");

            return row.Select((value, index) => _formatter.Format(value, _columns[index].Type))
                      .ToArray();
        }
    }

    /// <summary>
    /// 值格式化器接口
    /// </summary>
    private interface IValueFormatter
    {
        string Format(object? value, string type);
    }

    /// <summary>
    /// C# 值格式化器 - 改进版本
    /// </summary>
    private class CSharpValueFormatter : IValueFormatter
    {
        private static readonly Dictionary<string, Func<object?, string>> _formatters = new()
        {
            ["string"] = value => value == null ? "null" : $"\"{EscapeString(value.ToString())}\"",
            ["int"] = value => value?.ToString() ?? "0",
            ["long"] = value => value == null ? "0L" : $"{value}L",
            ["double"] = value => value == null ? "0.0" : $"{value}",
            ["float"] = value => value == null ? "0.0f" : $"{value}f",
            ["decimal"] = value => value == null ? "0m" : $"{value}m",
            ["bool"] = value => value?.ToString()?.ToLower() ?? "false",
            ["datetime"] = value => value == null ? "DateTime.MinValue" : $"DateTime.Parse(\"{value}\")",
            ["guid"] = value => value == null ? "Guid.Empty" : $"Guid.Parse(\"{value}\")"
        };

        public string Format(object? value, string type)
        {
            if (value == null) return GetDefaultValue(type);

            var lowerType = type.ToLower();
            if (_formatters.TryGetValue(lowerType, out var formatter))
            {
                return formatter(value);
            }

            // 默认按字符串处理
            return $"\"{EscapeString(value.ToString())}\"";
        }

        private static string GetDefaultValue(string type)
        {
            return type.ToLower() switch
            {
                "string" => "null",
                "int" => "0",
                "long" => "0L",
                "double" => "0.0",
                "float" => "0.0f",
                "decimal" => "0m",
                "bool" => "false",
                "datetime" => "DateTime.MinValue",
                "guid" => "Guid.Empty",
                _ => "null"
            };
        }

        private static string EscapeString(string? input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            return input.Replace("\\", "\\\\")
                       .Replace("\"", "\\\"")
                       .Replace("\r", "\\r")
                       .Replace("\n", "\\n")
                       .Replace("\t", "\\t");
        }
    }


}