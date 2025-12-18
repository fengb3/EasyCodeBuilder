namespace Fengb3.EasyCodeBuilder;

/// <summary>
/// 代码渲染片段委托
/// </summary>
/// <param name="builder">代码构建器</param>
/// <returns>代码构建器</returns>
public delegate CodeBuilder CodeRenderFragment(CodeBuilder builder);