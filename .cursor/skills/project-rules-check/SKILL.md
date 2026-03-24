---
name: project-rules-check
description: 任务开始前/大改前的规则核对清单（.NET），用于发现冲突与风险并正确路由子代理。
---

# 项目规则核对清单（.NET）

## 必读规则（快速复诵）
- 先复述目标与约束；澄清问题 ≤2（仅在关键前提缺失时）。
- 最小影响面：禁止无关重构/改名/顺便优化。
- 禁止吞异常/静默失败/无限重试式兜底；优先根因与可观测诊断。
- 每次改动必须可验证：build/test/手工步骤至少其一。

## 任务分类与路由建议
- 报错/bug → `bugfix`（`bugfix-sop`）
- 新增功能 → `feature`（`feature-add-sop`）
- 需求不清晰 → `requirement-parser`（`requirement-parse-flow`）
- public API/兼容性 → `api-compat-guardian`（`public-api-compat-check`）
- 打包发布 → `packaging-release`（`packaging-release-sop`）
- 提交前守门 → `commit-guard`（`coding-commit-rules`）

## 风险点提醒（按需）
- 运行时差异：.NET Framework vs .NET，平台差异（Windows/Linux/macOS）。
- 依赖冲突：公共 API 不暴露第三方类型；必要时隔离。
- 安全：敏感信息、注入、鉴权、证书/密钥管理。
