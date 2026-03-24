---
name: packaging-release-sop
description: NuGet/MSBuild 打包发布 SOP（.NET）：版本、依赖、符号包、可重复构建、签名与验证。
---

# 打包与发布 SOP（.NET）

## 1) 目标与约束
- 明确发布对象：NuGet 包/可执行程序/容器镜像/内部制品库。
- 明确目标框架与最低支持运行时（Framework/.NET 版本、平台）。

## 2) 版本与元数据
- 版本号来源一致且可追踪（建议集中在 `Directory.Build.props` 或 CI 注入）。
- 包元数据完整：`PackageId`、`Authors`、`RepositoryUrl`、`PackageLicenseExpression`（按项目要求）。

## 3) 依赖治理
- 避免 public API 暴露第三方类型导致依赖冲突扩散。
- 使用 `PrivateAssets`/`IncludeAssets` 控制传递依赖（按需要）。
- 多目标时，确保每个 TFM 的依赖与特性一致或解释差异。

## 4) 产物质量与可重复构建
- 打包前至少：`dotnet build` 成功；建议额外 `dotnet test`（若有）。
- 明确符号策略（PDB/符号包）与 SourceLink（按需要）。

## 5) 签名与密钥
- 仅在需求明确时启用强签名/包签名。
- 密钥/证书不得入库；通过环境/安全存储注入。

## 6) 发布验证
- 安装验证：新建最小消费项目引用包/产物，验证关键 API/运行路径。
- 回滚策略：明确如何撤回版本/降级/禁用功能（按项目需要）。
