# TaskPlanSetter

一个基于 WinForms（.NET Framework 4.8）的 Windows 任务计划可视化编辑工具，聚焦“快速查看、快速改、快速验证”。

## 功能特性

- 任务树与任务列表浏览（按任务计划程序库文件夹分层）
- 任务基础编辑（启用、程序路径、参数、起始于）
- 新建任务 / 删除任务
- 程序路径文件浏览器选择
- 高级设置面板（启动用户、启动方式、触发时机、并发策略等）
- 测试启动（模拟任务计划“运行”动作，免重启验证）
- 深色科技风 UI、无系统标题栏、可拖拽窗口、边缘缩放

## 技术栈

- .NET Framework `net48`
- WinForms
- NuGet: `TaskScheduler`（`Microsoft.Win32.TaskScheduler`）

## 运行环境

- Windows 10/11
- .NET Framework 4.8 Runtime
- Visual Studio 2022（建议）或 `dotnet SDK`

## 快速开始

```bash
dotnet restore
dotnet build TaskPlanSetter.sln -c Debug
```

> 如果程序正在运行，构建时可能出现输出文件被锁定。请先关闭运行中的 `TaskPlanSetter.exe`。

## 使用说明（简版）

1. 左侧选择任务文件夹，右侧选中任务。
2. 在详细区修改程序路径/参数/起始于，点击“保存”。
3. 点击“测试启动”立即触发任务，验证配置是否生效。
4. 需要更细配置时点击“高级...”。

## 权限说明

- 项目默认为 `asInvoker`（非强制管理员）。
- 对系统任务或受保护任务执行编辑/删除/运行时，可能需要“以管理员身份运行”。

## 文档

- [架构与模块说明](docs/ARCHITECTURE.md)
- [用户操作手册](docs/USER_GUIDE.md)

## 目录结构

```text
TaskPlanSetter/
  Form1.cs
  Form1.Designer.cs
  Services/
    TaskPlanService.cs
  TaskAdvancedDialog.cs
  TaskCreateDialog.cs
  TechUi.cs
  TechDialog.cs
  docs/
    ARCHITECTURE.md
    USER_GUIDE.md
```

## 许可证

当前仓库未声明开源许可证。如需开源，请补充 `LICENSE` 文件。
