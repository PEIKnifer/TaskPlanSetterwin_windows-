using System;
using System.Collections.Generic;
using Microsoft.Win32.TaskScheduler;

namespace TaskPlanSetter.Services
{
    /// <summary>
    /// 封装本机任务计划（Task Scheduler）的只读枚举与 MVP 写回：首个 <see cref="ExecAction"/> + 启用状态。
    /// </summary>
    public sealed class TaskPlanService : IDisposable
    {
        private readonly TaskService _service = new TaskService();
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;
            _service.Dispose();
            _disposed = true;
        }

        public TaskFolder ResolveFolder(string folderPath)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(folderPath) || folderPath == "\\")
                return _service.RootFolder;

            return _service.GetFolder(folderPath);
        }

        public IReadOnlyList<string> EnumerateSubFolderPaths(string folderPath)
        {
            ThrowIfDisposed();
            var folder = ResolveFolder(folderPath);
            var result = new List<string>();
            foreach (TaskFolder sub in folder.SubFolders)
            {
                if (sub != null && !string.IsNullOrWhiteSpace(sub.Path))
                    result.Add(sub.Path);
            }

            result.Sort(StringComparer.OrdinalIgnoreCase);
            return result;
        }

        public IReadOnlyList<TaskPlanListRow> EnumerateTasks(string folderPath)
        {
            ThrowIfDisposed();
            var folder = ResolveFolder(folderPath);
            var result = new List<TaskPlanListRow>();

            foreach (Task task in folder.GetTasks())
            {
                DateTime? next = null;
                try
                {
                    var n = task.NextRunTime;
                    if (n != DateTime.MinValue)
                        next = n;
                }
                catch
                {
                    // 部分任务无法查询下次运行时间，保持为 null。
                }

                result.Add(new TaskPlanListRow(task.Name, task.State, next));
            }

            result.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            return result;
        }

        public TaskPlanEditModel LoadTask(string folderPath, string taskName)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));

            var fullPath = CombineTaskPath(folderPath, taskName);
            using (var task = _service.GetTask(fullPath))
            {
                if (task == null)
                    throw new InvalidOperationException($"未找到任务: {taskName}");

                var def = task.Definition;
                ExecAction exec = FindFirstExecAction(def);

                return new TaskPlanEditModel(
                    task.Enabled,
                    exec != null,
                    exec?.Path ?? string.Empty,
                    exec?.Arguments ?? string.Empty,
                    exec?.WorkingDirectory ?? string.Empty);
            }
        }

        public void CreateTask(
            string folderPath,
            string taskName,
            bool enabled,
            string programPath,
            string arguments,
            string workingDirectory)
        {
            var options = TaskAdvancedOptions.CreateDefault();
            options.Enabled = enabled;
            CreateTask(folderPath, taskName, programPath, arguments, workingDirectory, options);
        }

        public void CreateTask(
            string folderPath,
            string taskName,
            string programPath,
            string arguments,
            string workingDirectory,
            TaskAdvancedOptions options)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));
            if (string.IsNullOrWhiteSpace(programPath))
                throw new ArgumentException("程序路径不能为空。", nameof(programPath));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var fullPath = CombineTaskPath(folderPath, taskName);
            using (var existing = _service.GetTask(fullPath))
            {
                if (existing != null)
                    throw new InvalidOperationException($"已存在同名任务: {taskName}");
            }

            var folder = ResolveFolder(folderPath);
            var def = _service.NewTask();
            def.RegistrationInfo.Description = "由 TaskPlanSetter 创建";
            def.Actions.Add(new ExecAction(programPath, arguments ?? string.Empty, workingDirectory ?? string.Empty));
            ApplyAdvancedOptions(def, options, creatingNewTask: true);

            var principal = def.Principal;
            folder.RegisterTaskDefinition(
                taskName,
                def,
                TaskCreation.Create,
                principal.UserId,
                null,
                principal.LogonType);
        }

        public void DeleteTask(string folderPath, string taskName)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));

            var folder = ResolveFolder(folderPath);
            folder.DeleteTask(taskName, exceptionOnNotExists: true);
        }

        public void SaveTask(
            string folderPath,
            string taskName,
            bool enabled,
            string programPath,
            string arguments,
            string workingDirectory)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));

            var folder = ResolveFolder(folderPath);
            var fullPath = CombineTaskPath(folderPath, taskName);
            using (var task = _service.GetTask(fullPath))
            {
                if (task == null)
                    throw new InvalidOperationException($"未找到任务: {taskName}");

                var def = task.Definition;
                var exec = FindFirstExecAction(def);
                if (exec == null)
                    throw new InvalidOperationException("当前任务没有「启动程序」操作，MVP 无法保存。");

                exec.Path = programPath ?? string.Empty;
                exec.Arguments = arguments ?? string.Empty;
                exec.WorkingDirectory = workingDirectory ?? string.Empty;
                def.Settings.Enabled = enabled;

                var principal = def.Principal;
                folder.RegisterTaskDefinition(
                    taskName,
                    def,
                    TaskCreation.Update,
                    principal.UserId,
                    null,
                    principal.LogonType);
            }
        }

        public void RunTaskNow(string folderPath, string taskName)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));

            var fullPath = CombineTaskPath(folderPath, taskName);
            using (var task = _service.GetTask(fullPath))
            {
                if (task == null)
                    throw new InvalidOperationException($"未找到任务: {taskName}");

                task.Run();
            }
        }

        public TaskAdvancedOptions GetTaskAdvancedOptions(string folderPath, string taskName)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));

            var fullPath = CombineTaskPath(folderPath, taskName);
            using (var task = _service.GetTask(fullPath))
            {
                if (task == null)
                    throw new InvalidOperationException($"未找到任务: {taskName}");

                var def = task.Definition;
                var options = TaskAdvancedOptions.CreateDefault();
                options.Enabled = task.Enabled;
                options.UserId = def.Principal?.UserId ?? string.Empty;
                options.LogonType = def.Principal?.LogonType ?? TaskLogonType.InteractiveToken;
                options.RunLevel = def.Principal?.RunLevel ?? TaskRunLevel.LUA;
                options.AllowDemandStart = def.Settings.AllowDemandStart;
                options.StartWhenAvailable = def.Settings.StartWhenAvailable;
                options.RunOnlyIfIdle = def.Settings.RunOnlyIfIdle;
                options.WakeToRun = def.Settings.WakeToRun;
                options.DisallowStartIfOnBatteries = def.Settings.DisallowStartIfOnBatteries;
                options.StopIfGoingOnBatteries = def.Settings.StopIfGoingOnBatteries;
                options.Hidden = def.Settings.Hidden;
                options.MultipleInstances = def.Settings.MultipleInstances;
                options.KeepExistingTriggers = true;

                if (def.Triggers != null && def.Triggers.Count > 0)
                {
                    var first = def.Triggers[0];
                    options.TriggerStartBoundary = first.StartBoundary == DateTime.MinValue
                        ? NextScheduleMinute(DateTime.Now)
                        : first.StartBoundary;
                    if (first is DailyTrigger daily)
                    {
                        options.TriggerPreset = TriggerPreset.Daily;
                        options.DailyInterval = (short)(daily.DaysInterval <= 0 ? 1 : daily.DaysInterval);
                    }
                    else if (first is LogonTrigger)
                    {
                        options.TriggerPreset = TriggerPreset.AtLogon;
                    }
                    else if (first is BootTrigger)
                    {
                        options.TriggerPreset = TriggerPreset.AtStartup;
                    }
                    else
                    {
                        options.TriggerPreset = TriggerPreset.OneTime;
                    }
                }

                return options;
            }
        }

        public void UpdateTaskAdvancedOptions(string folderPath, string taskName, TaskAdvancedOptions options)
        {
            ThrowIfDisposed();
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var fullPath = CombineTaskPath(folderPath, taskName);
            using (var task = _service.GetTask(fullPath))
            {
                if (task == null)
                    throw new InvalidOperationException($"未找到任务: {taskName}");

                var folder = ResolveFolder(folderPath);
                var def = task.Definition;
                ApplyAdvancedOptions(def, options, creatingNewTask: false);
                var principal = def.Principal;
                folder.RegisterTaskDefinition(
                    taskName,
                    def,
                    TaskCreation.Update,
                    principal.UserId,
                    null,
                    principal.LogonType);
            }
        }

        /// <summary>取「下一整分」作为首次触发时间，避免边界时刻被判定为过去时间。</summary>
        private static DateTime NextScheduleMinute(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0).AddMinutes(1);
        }

        private static void ApplyAdvancedOptions(TaskDefinition def, TaskAdvancedOptions options, bool creatingNewTask)
        {
            def.Settings.Enabled = options.Enabled;
            def.Settings.AllowDemandStart = options.AllowDemandStart;
            def.Settings.StartWhenAvailable = options.StartWhenAvailable;
            def.Settings.RunOnlyIfIdle = options.RunOnlyIfIdle;
            def.Settings.WakeToRun = options.WakeToRun;
            def.Settings.DisallowStartIfOnBatteries = options.DisallowStartIfOnBatteries;
            def.Settings.StopIfGoingOnBatteries = options.StopIfGoingOnBatteries;
            def.Settings.Hidden = options.Hidden;
            def.Settings.MultipleInstances = options.MultipleInstances;

            def.Principal.LogonType = options.LogonType;
            def.Principal.RunLevel = options.RunLevel;
            if (!string.IsNullOrWhiteSpace(options.UserId))
                def.Principal.UserId = options.UserId.Trim();
            else if (creatingNewTask)
                def.Principal.UserId = null;

            if (!options.KeepExistingTriggers || creatingNewTask || def.Triggers.Count == 0)
            {
                def.Triggers.Clear();
                switch (options.TriggerPreset)
                {
                    case TriggerPreset.AtLogon:
                        def.Triggers.Add(new LogonTrigger { StartBoundary = options.TriggerStartBoundary });
                        break;
                    case TriggerPreset.AtStartup:
                        def.Triggers.Add(new BootTrigger { StartBoundary = options.TriggerStartBoundary });
                        break;
                    case TriggerPreset.OneTime:
                        def.Triggers.Add(new TimeTrigger { StartBoundary = options.TriggerStartBoundary });
                        break;
                    default:
                        def.Triggers.Add(new DailyTrigger
                        {
                            StartBoundary = options.TriggerStartBoundary,
                            DaysInterval = (short)(options.DailyInterval <= 0 ? 1 : options.DailyInterval)
                        });
                        break;
                }
            }
        }

        /// <summary>任务计划 API 使用完整路径，例如 <c>\Folder\TaskName</c>。</summary>
        private static string CombineTaskPath(string folderPath, string taskName)
        {
            if (string.IsNullOrWhiteSpace(taskName))
                throw new ArgumentException("任务名称不能为空。", nameof(taskName));

            var fp = string.IsNullOrEmpty(folderPath) || folderPath == "\\"
                ? string.Empty
                : folderPath.TrimEnd('\\');

            return string.IsNullOrEmpty(fp) ? "\\" + taskName : fp + "\\" + taskName;
        }

        private static ExecAction FindFirstExecAction(TaskDefinition def)
        {
            if (def?.Actions == null)
                return null;

            for (var i = 0; i < def.Actions.Count; i++)
            {
                if (def.Actions[i] is ExecAction ea)
                    return ea;
            }

            return null;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TaskPlanService));
        }
    }

    public sealed class TaskPlanListRow
    {
        public TaskPlanListRow(string name, TaskState state, DateTime? nextRunLocalOrUtc)
        {
            Name = name;
            State = state;
            NextRun = nextRunLocalOrUtc;
        }

        public string Name { get; }
        public TaskState State { get; }
        /// <summary>库返回的时间；展示时按本地时间格式化即可。</summary>
        public DateTime? NextRun { get; }
    }

    public sealed class TaskPlanEditModel
    {
        public TaskPlanEditModel(
            bool enabled,
            bool hasExecAction,
            string programPath,
            string arguments,
            string workingDirectory)
        {
            Enabled = enabled;
            HasExecAction = hasExecAction;
            ProgramPath = programPath;
            Arguments = arguments;
            WorkingDirectory = workingDirectory;
        }

        public bool Enabled { get; }
        public bool HasExecAction { get; }
        public string ProgramPath { get; }
        public string Arguments { get; }
        public string WorkingDirectory { get; }
    }

    public enum TriggerPreset
    {
        Daily,
        AtLogon,
        AtStartup,
        OneTime
    }

    public sealed class TaskAdvancedOptions
    {
        public bool Enabled { get; set; }
        public string UserId { get; set; }
        public TaskLogonType LogonType { get; set; }
        public TaskRunLevel RunLevel { get; set; }
        public bool AllowDemandStart { get; set; }
        public bool StartWhenAvailable { get; set; }
        public bool RunOnlyIfIdle { get; set; }
        public bool WakeToRun { get; set; }
        public bool DisallowStartIfOnBatteries { get; set; }
        public bool StopIfGoingOnBatteries { get; set; }
        public bool Hidden { get; set; }
        public TaskInstancesPolicy MultipleInstances { get; set; }

        public bool KeepExistingTriggers { get; set; }
        public TriggerPreset TriggerPreset { get; set; }
        public DateTime TriggerStartBoundary { get; set; }
        public short DailyInterval { get; set; }

        public static TaskAdvancedOptions CreateDefault()
        {
            return new TaskAdvancedOptions
            {
                Enabled = true,
                UserId = string.Empty,
                LogonType = TaskLogonType.InteractiveToken,
                RunLevel = TaskRunLevel.LUA,
                AllowDemandStart = true,
                StartWhenAvailable = true,
                RunOnlyIfIdle = false,
                WakeToRun = false,
                DisallowStartIfOnBatteries = false,
                StopIfGoingOnBatteries = false,
                Hidden = false,
                MultipleInstances = TaskInstancesPolicy.IgnoreNew,
                KeepExistingTriggers = false,
                TriggerPreset = TriggerPreset.Daily,
                TriggerStartBoundary = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0),
                DailyInterval = 1
            };
        }
    }
}
