using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using Fengb3.EasyCodeBuilder.Csharp.Abstraction;

namespace Fengb3.EasyCodeBuilder.Csharp;

/// <summary>
/// 关键字选项配置器
/// </summary>
/// <param name="parent">父选项</param>
/// <typeparam name="TParent">父选项类型</typeparam>
public class KeywordOptionConfigurator<TParent>(TParent parent)
    where TParent : CodeOption

{
    /// <summary>
    /// 父选项
    /// </summary>
    public TParent Parent { get; } = parent;

    private readonly ICollection<string> Keywords = new HashSet<string>();

    /// <summary>
    /// 添加 public 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Public
    {
        get
        {
            Keywords.Add("public");
            return this;
        }
    }

    /// <summary>
    /// 添加 private 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Private
    {
        get
        {
            Keywords.Add("private");
            return this;
        }
    }

    /// <summary>
    /// 添加 internal 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Internal
    {
        get
        {
            Keywords.Add("internal");
            return this;
        }
    }

    /// <summary>
    /// 添加 abstract 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Abstract
    {
        get
        {
            Keywords.Add("abstract");
            return this;
        }
    }

    /// <summary>
    /// 添加 static 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Static
    {
        get
        {
            Keywords.Add("static");
            return this;
        }
    }

    /// <summary>
    /// 添加 partial 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Partial
    {
        get
        {
            Keywords.Add("partial");
            return this;
        }
    }

    /// <summary>
    /// 添加 sealed 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Sealed
    {
        get
        {
            Keywords.Add("sealed");
            return this;
        }
    }
    
    /// <summary>
    /// 添加 readonly 关键字
    /// </summary>
    public KeywordOptionConfigurator<TParent> Readonly
    {
        get
        {
            Keywords.Add("readonly");
            return this;
        }
    }

    /// <summary>
    /// configure the keywords into the parent option
    /// </summary>
    /// <param name="keywordSetter"></param>
    public void Configure(Action<string> keywordSetter)
    {
        foreach (var keyword in Keywords)
        {
            keywordSetter(keyword);
        }
    }

    /// <summary>
    /// configure the keywords into the parent option
    /// </summary>
    /// <param name="keywordSetter"></param>
    public void Configure(Action<string[]> keywordSetter)
    {
        keywordSetter(Keywords.ToArray());
    }
}