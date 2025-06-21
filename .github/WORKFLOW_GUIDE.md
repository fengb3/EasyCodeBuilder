# 🚀 GitHub Actions CI/CD 工作流指南

本项目配置了完整的 CI/CD 工作流，用于自动化构建、测试和发布 NuGet 包。

## 📁 工作流文件

### 1. `ci.yml` - 持续集成
**触发条件**：
- 推送到 `main`, `master`, `develop` 分支
- 向这些分支提交 Pull Request

**功能**：
- ✅ 多平台构建测试 (Ubuntu, Windows, macOS)
- ✅ 代码质量检查
- ✅ XML 文档检查
- ✅ 测试包生成验证

### 2. `nuget-publish.yml` - NuGet 包发布
**触发条件**：
- 推送版本标签 (如 `v1.0.0`, `v2.1.3`)
- 手动触发

**功能**：
- 🔐 **权限验证**：只允许仓库所有者发布
- 🔨 自动构建项目
- 📦 生成 NuGet 包
- 🚀 发布到 NuGet.org
- 🎉 创建 GitHub Release
- 📸 上传包文件作为 Artifacts

## 🔒 权限控制机制

### 多层权限保护

1. **用户身份验证**：
   - ✅ 自动检查触发用户是否为仓库所有者
   - ❌ 非所有者用户将被拒绝执行

2. **Environment保护**：
   - 🛡️ 使用 `production` environment
   - 👥 可配置审批者列表
   - ⏰ 可设置等待时间

3. **条件执行**：
   - 🔐 只有通过权限检查的用户才能执行后续步骤
   - 🚫 未授权用户的工作流会提前终止

### 权限检查流程

```mermaid
graph TD
    A[触发工作流] --> B[权限检查]
    B --> C{是仓库所有者?}
    C -->|是| D[✅ 授权通过]
    C -->|否| E[❌ 拒绝访问]
    D --> F[构建测试]
    F --> G[Environment审批]
    G --> H[发布到NuGet]
    E --> I[工作流终止]
```

## ⚙️ 配置要求

### 1. GitHub Secrets 设置

在 GitHub 仓库设置中添加以下 Secrets：

1. **`NUGET_API_KEY`** (必需)
   - 前往 [nuget.org/account/apikeys](https://www.nuget.org/account/apikeys)
   - 创建新的 API Key
   - 将 API Key 添加到 GitHub Secrets

### 2. Environment配置（可选但推荐）

设置 `production` Environment：

1. **前往仓库设置**：
   - Settings → Environments → New environment
   - 名称：`production`

2. **配置保护规则**：
   - ✅ **Required reviewers**: 添加可以审批的用户
   - ✅ **Wait timer**: 设置等待时间（如5分钟）
   - ✅ **Restrict to protected branches**: 限制到保护分支

3. **添加Secrets**：
   - 在Environment中添加 `NUGET_API_KEY`

### 3. 项目配置

确保项目文件包含必要的 NuGet 包信息：
```xml
<PropertyGroup>
    <PackageId>您的包名</PackageId>
    <Version>0.0.1</Version>
    <Authors>您的姓名</Authors>
    <Description>包描述</Description>
    <!-- 其他包信息 -->
</PropertyGroup>
```

## 🚀 使用方法

### 自动发布新版本（仅限仓库所有者）

1. **更新版本号**（可选，工作流会自动更新）
2. **创建并推送标签**：
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```

3. **等待自动发布**：
   - 权限检查通过后，GitHub Actions 会自动触发
   - 如果配置了Environment审批，需要手动审批
   - 构建、测试、打包、发布
   - 创建 GitHub Release

### 手动发布（仅限仓库所有者）

1. 前往 GitHub Actions 页面
2. 选择 "🚀 NuGet Package CI/CD" 工作流
3. 点击 "Run workflow"
4. 输入版本号
5. 点击 "Run workflow" 按钮
6. 等待权限验证和可能的审批流程

## 🔐 权限控制选项

### 当前配置（仅限所有者）
```yaml
# 检查是否是仓库所有者
if [ "${{ github.actor }}" = "${{ github.repository_owner }}" ]; then
  echo "✅ User is repository owner - authorized"
else
  echo "❌ User is not repository owner - unauthorized"
  exit 1
fi
```

### 扩展权限配置

如需允许其他用户，可修改权限检查逻辑：

```yaml
# 允许特定用户列表
AUTHORIZED_USERS="user1 user2 user3"
if [[ " $AUTHORIZED_USERS " =~ " ${{ github.actor }} " ]]; then
  echo "✅ User authorized"
else
  echo "❌ User not in authorized list"
  exit 1
fi
```

### 基于团队的权限
```yaml
# 需要额外的API调用检查团队成员身份
# 或使用GitHub的内置权限检查
```

## 📊 工作流状态

### CI 状态徽章
添加到 README.md：
```markdown
![CI](https://github.com/您的用户名/您的仓库名/workflows/🔄%20Continuous%20Integration/badge.svg)
![NuGet](https://github.com/您的用户名/您的仓库名/workflows/🚀%20NuGet%20Package%20CI/CD/badge.svg)
```

## 🔧 自定义配置

### 修改权限控制
编辑 `permission-check` job 中的验证逻辑。

### 修改目标框架
编辑工作流文件中的 `DOTNET_VERSION` 环境变量。

### 更改项目路径
修改 `PROJECT_PATH` 环境变量。

### 添加测试项目
工作流会自动检测并运行测试项目。

### 自定义发布条件
修改 `on` 部分的触发条件。

## 🎯 最佳实践

1. **版本管理**：使用语义化版本 (SemVer)
2. **标签格式**：使用 `v` 前缀 (如 `v1.0.0`)
3. **安全性**：
   - 定期轮换 NuGet API Key
   - 启用Environment保护
   - 限制发布权限
4. **测试**：确保有充分的测试覆盖
5. **文档**：保持 README 和文档更新
6. **审计**：定期检查工作流执行日志

## 🛡️ 安全特性

- ✅ **用户身份验证**：自动验证触发用户身份
- ✅ **Environment保护**：可配置审批流程
- ✅ **权限日志**：详细记录权限检查过程
- ✅ **Secrets管理**：安全存储API密钥
- ✅ **条件执行**：未授权用户无法执行关键步骤

## 🐛 故障排除

### 常见问题

**Q: 权限被拒绝**
A: 确保您是仓库所有者，或联系所有者添加您到授权列表

**Q: Environment审批卡住**
A: 检查Environment设置，确保有审批者且已收到通知

**Q: 发布失败，提示 API Key 无效**
A: 检查 GitHub Secrets 中的 `NUGET_API_KEY` 是否正确设置

**Q: 包版本冲突**
A: NuGet.org 不允许覆盖已存在的版本，需要使用新版本号

**Q: 构建失败**
A: 检查项目是否能在本地正常构建

**Q: 权限错误**
A: 确保 GitHub Actions 有足够的权限访问仓库

## 📞 支持

如有问题，请在 GitHub Issues 中提出。

---

*本工作流配置遵循 CI/CD 和安全最佳实践，确保只有授权用户才能发布包* 🔒🚀 