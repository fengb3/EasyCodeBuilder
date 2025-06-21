using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneratedData
{
  public class DataModel
  {
    /// <summary>
    /// 用户ID
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// 用户姓名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 年龄
    /// </summary>
    public int Age { get; set; }
    /// <summary>
    /// 电子邮箱
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// 评分
    /// </summary>
    public double Score { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
  }

  public static class DataProvider
  {
    public static DataModel Create(int ID, string Name, int Age, string Email, bool IsActive, double Score, DateTime CreateTime)
    {
      var model = new DataModel();
      model.ID = ID;
      model.Name = Name;
      model.Age = Age;
      model.Email = Email;
      model.IsActive = IsActive;
      model.Score = Score;
      model.CreateTime = CreateTime;
      return model;
    }
    private static List<DataModel> _datas { get; set; }
    static  DataProvider()
    {
      _datas = new List<DataModel>();
      _datas.Add(Create(1, "张三", 25, "zhangsan@example.com", true, 95.5, DateTime.Parse("2024-01-15")));
      _datas.Add(Create(2, "李四", 30, "lisi@example.com", true, 88.2, DateTime.Parse("2024-02-20")));
      _datas.Add(Create(3, "王五", 28, "wangwu@example.com", false, 92.1, DateTime.Parse("2024-03-10")));
      _datas.Add(Create(4, "赵六", 35, "zhaoliu@example.com", true, 87.8, DateTime.Parse("2024-04-05")));
      _datas.Add(Create(5, "孙七", 22, "sunqi@example.com", true, 94.3, DateTime.Parse("2024-05-12")));
    }

    /// <summary>
    /// 获取硬编码的数据列表
    /// </summary>
    /// <returns>数据列表</returns>
    public static List<DataModel> GetData()
    {
      return _datas;
    }

  }
}
