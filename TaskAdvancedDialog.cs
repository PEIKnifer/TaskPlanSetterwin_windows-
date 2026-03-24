using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using TaskPlanSetter.Services;

namespace TaskPlanSetter
{
    internal sealed class TaskAdvancedDialog : Form
    {
        private readonly Panel _titleBar;
        private readonly Label _titleLabel;
        private readonly Button _btnClose;
        private readonly CheckBox _chkEnabled;
        private readonly TextBox _txtUserId;
        private readonly ComboBox _cmbLogonType;
        private readonly ComboBox _cmbRunLevel;
        private readonly CheckBox _chkKeepTriggers;
        private readonly ComboBox _cmbTriggerPreset;
        private readonly DateTimePicker _dtTrigger;
        private readonly NumericUpDown _numDailyInterval;
        private readonly CheckBox _chkAllowDemandStart;
        private readonly CheckBox _chkStartWhenAvailable;
        private readonly CheckBox _chkRunOnlyIfIdle;
        private readonly CheckBox _chkWakeToRun;
        private readonly CheckBox _chkNoBatteryLimit;
        private readonly CheckBox _chkStopOnBattery;
        private readonly CheckBox _chkHidden;
        private readonly ComboBox _cmbInstances;
        private readonly FlowLayoutPanel _footerButtons;

        public TaskAdvancedOptions Result { get; private set; }

        public TaskAdvancedDialog(TaskAdvancedOptions initial, bool isExistingTask)
        {
            if (initial == null)
                throw new ArgumentNullException(nameof(initial));

            Text = "任务高级设置";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(640, 508);
            BackColor = TechUi.PanelBg;
            ForeColor = TechUi.TextMuted;
            Font = new Font("Segoe UI", 9f);
            Padding = new Padding(1);

            _titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 34,
                BackColor = Color.FromArgb(11, 20, 31)
            };
            Controls.Add(_titleBar);

            _titleLabel = new Label
            {
                Text = "任务高级设置",
                ForeColor = TechUi.Accent,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Location = new Point(12, 8),
                Size = new Size(260, 20)
            };
            _titleBar.Controls.Add(_titleLabel);

            _btnClose = new Button
            {
                Text = "✕",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(34, 28),
                Location = new Point(ClientSize.Width - 38, 3),
                Cursor = Cursors.Hand
            };
            TechUi.StyleTitleBarButton(_btnClose, Color.FromArgb(180, 40, 40));
            _btnClose.Click += (_, __) => Close();
            _titleBar.Controls.Add(_btnClose);

            _titleBar.MouseDown += Title_MouseDown;
            _titleLabel.MouseDown += Title_MouseDown;
            _titleBar.Paint += (s, e) =>
            {
                using (var p = new Pen(Color.FromArgb(58, 92, 122)))
                    e.Graphics.DrawLine(p, 0, _titleBar.Height - 1, _titleBar.Width, _titleBar.Height - 1);
            };

            Controls.Add(new Label
            {
                Text = "高级设置（标注“推荐”的是最常用默认）",
                ForeColor = TechUi.Accent,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                Location = new Point(16, 46),
                Size = new Size(500, 22)
            });

            var body = new Panel
            {
                Location = new Point(16, 76),
                Size = new Size(608, 390),
                BackColor = Color.FromArgb(14, 20, 28),
                BorderStyle = BorderStyle.FixedSingle
            };
            Controls.Add(body);

            int y = 12;
            body.Controls.Add(MkLabel("启用", 12, y + 2));
            _chkEnabled = MkCheckBox("", 110, y);
            body.Controls.Add(_chkEnabled);
            y += 30;

            body.Controls.Add(MkLabel("启动用户", 12, y + 4));
            _txtUserId = MkText(110, y, 482);
            body.Controls.Add(_txtUserId);
            y += 32;

            body.Controls.Add(MkLabel("启动方式", 12, y + 4));
            _cmbLogonType = MkCombo(110, y, 230);
            _cmbLogonType.Items.Add(new OptionItem("仅用户登录时运行（推荐）", TaskLogonType.InteractiveToken));
            _cmbLogonType.Items.Add(new OptionItem("不存储密码（S4U）", TaskLogonType.S4U));
            _cmbLogonType.Items.Add(new OptionItem("服务账户（SYSTEM 等）", TaskLogonType.ServiceAccount));
            TechUi.StyleComboBox(_cmbLogonType);
            body.Controls.Add(_cmbLogonType);

            body.Controls.Add(MkLabel("权限级别", 356, y + 4));
            _cmbRunLevel = MkCombo(430, y, 162);
            _cmbRunLevel.Items.Add(new OptionItem("普通权限（推荐）", TaskRunLevel.LUA));
            _cmbRunLevel.Items.Add(new OptionItem("最高权限", TaskRunLevel.Highest));
            TechUi.StyleComboBox(_cmbRunLevel);
            body.Controls.Add(_cmbRunLevel);
            y += 32;

            body.Controls.Add(MkLabel("启动时机", 12, y + 4));
            _cmbTriggerPreset = MkCombo(110, y, 230);
            _cmbTriggerPreset.Items.Add(new OptionItem("每天（推荐）", TriggerPreset.Daily));
            _cmbTriggerPreset.Items.Add(new OptionItem("系统启动时", TriggerPreset.AtStartup));
            _cmbTriggerPreset.Items.Add(new OptionItem("用户登录时", TriggerPreset.AtLogon));
            _cmbTriggerPreset.Items.Add(new OptionItem("仅一次", TriggerPreset.OneTime));
            TechUi.StyleComboBox(_cmbTriggerPreset);
            body.Controls.Add(_cmbTriggerPreset);

            _dtTrigger = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                Location = new Point(356, y),
                Size = new Size(156, 23),
                ShowUpDown = true,
                CalendarForeColor = TechUi.TextMuted,
                CalendarMonthBackground = TechUi.ControlBg
            };
            _dtTrigger.CalendarTitleBackColor = Color.FromArgb(22, 36, 50);
            _dtTrigger.CalendarTitleForeColor = TechUi.TextMuted;
            _dtTrigger.CalendarTrailingForeColor = TechUi.SoftText;
            body.Controls.Add(_dtTrigger);

            _numDailyInterval = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 365,
                Value = 1,
                Location = new Point(520, y),
                Size = new Size(72, 23),
                BackColor = TechUi.ControlBg,
                ForeColor = TechUi.TextMuted
            };
            TechUi.StyleNumeric(_numDailyInterval);
            body.Controls.Add(_numDailyInterval);
            y += 32;

            _chkKeepTriggers = MkCheckBox("保持现有触发器（已有任务推荐）", 110, y);
            body.Controls.Add(_chkKeepTriggers);
            y += 34;

            _chkAllowDemandStart = MkCheckBox("允许按需运行（推荐）", 110, y);
            body.Controls.Add(_chkAllowDemandStart);
            _chkStartWhenAvailable = MkCheckBox("错过后尽快启动（推荐）", 320, y);
            body.Controls.Add(_chkStartWhenAvailable);
            y += 28;

            _chkRunOnlyIfIdle = MkCheckBox("仅空闲时运行", 110, y);
            body.Controls.Add(_chkRunOnlyIfIdle);
            _chkWakeToRun = MkCheckBox("允许唤醒运行", 320, y);
            body.Controls.Add(_chkWakeToRun);
            y += 28;

            _chkNoBatteryLimit = MkCheckBox("电池供电也允许运行（推荐）", 110, y);
            body.Controls.Add(_chkNoBatteryLimit);
            _chkStopOnBattery = MkCheckBox("转电池时停止", 360, y);
            body.Controls.Add(_chkStopOnBattery);
            y += 28;

            _chkHidden = MkCheckBox("隐藏任务", 110, y);
            body.Controls.Add(_chkHidden);
            body.Controls.Add(MkLabel("并发策略", 320, y + 4));
            _cmbInstances = MkCombo(390, y, 202);
            _cmbInstances.Items.Add(new OptionItem("忽略新实例（推荐）", TaskInstancesPolicy.IgnoreNew));
            _cmbInstances.Items.Add(new OptionItem("并行运行", TaskInstancesPolicy.Parallel));
            _cmbInstances.Items.Add(new OptionItem("排队运行", TaskInstancesPolicy.Queue));
            _cmbInstances.Items.Add(new OptionItem("停止旧实例", TaskInstancesPolicy.StopExisting));
            TechUi.StyleComboBox(_cmbInstances);
            body.Controls.Add(_cmbInstances);

            _footerButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 42,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(8, 6, 12, 6),
                BackColor = TechUi.PanelBg,
                WrapContents = false
            };
            Controls.Add(_footerButtons);

            var btnOk = new Button
            {
                Text = "应用",
                Size = new Size(96, 30),
                Margin = new Padding(8, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            TechUi.StylePrimaryButton(btnOk);
            btnOk.Click += (_, __) =>
            {
                Result = Collect();
                DialogResult = DialogResult.OK;
                Close();
            };
            _footerButtons.Controls.Add(btnOk);

            var btnCancel = new Button
            {
                Text = "取消",
                Size = new Size(96, 30),
                Margin = new Padding(8, 0, 0, 0),
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            TechUi.StyleOutlinedButton(btnCancel);
            _footerButtons.Controls.Add(btnCancel);
            CancelButton = btnCancel;
            AcceptButton = btnOk;

            _cmbTriggerPreset.SelectedIndexChanged += (_, __) => UpdateTriggerInputs();
            _chkKeepTriggers.CheckedChanged += (_, __) => UpdateTriggerInputs();

            ApplyNativeDarkTheme(this);
            Shown += (_, __) => ApplyPopupDarkTheme();
            SetFrom(initial, isExistingTask);
            UpdateTriggerInputs();
        }

        private void Title_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, (IntPtr)NativeMethods.HTCAPTION, IntPtr.Zero);
        }

        private static void ApplyNativeDarkTheme(Control root)
        {
            if (root == null)
                return;
            root.HandleCreated += (_, __) => NativeMethods.SetWindowTheme(root.Handle, "DarkMode_Explorer", null);
            if (root.IsHandleCreated)
                NativeMethods.SetWindowTheme(root.Handle, "DarkMode_Explorer", null);
            foreach (Control child in root.Controls)
                ApplyNativeDarkTheme(child);
        }

        private void ApplyPopupDarkTheme()
        {
            NativeMethods.SetWindowTheme(_cmbLogonType.Handle, "DarkMode_CFD", null);
            NativeMethods.SetWindowTheme(_cmbRunLevel.Handle, "DarkMode_CFD", null);
            NativeMethods.SetWindowTheme(_cmbTriggerPreset.Handle, "DarkMode_CFD", null);
            NativeMethods.SetWindowTheme(_cmbInstances.Handle, "DarkMode_CFD", null);
            NativeMethods.SetWindowTheme(_dtTrigger.Handle, "DarkMode_CFD", null);
        }

        private void SetFrom(TaskAdvancedOptions options, bool isExistingTask)
        {
            _chkEnabled.Checked = options.Enabled;
            _txtUserId.Text = options.UserId ?? string.Empty;
            SelectByValue(_cmbLogonType, options.LogonType);
            SelectByValue(_cmbRunLevel, options.RunLevel);
            _chkKeepTriggers.Checked = isExistingTask && options.KeepExistingTriggers;
            SelectByValue(_cmbTriggerPreset, options.TriggerPreset);
            _dtTrigger.Value = options.TriggerStartBoundary <= DateTime.MinValue ? DateTime.Now : options.TriggerStartBoundary;
            _numDailyInterval.Value = options.DailyInterval <= 0 ? 1 : options.DailyInterval;
            _chkAllowDemandStart.Checked = options.AllowDemandStart;
            _chkStartWhenAvailable.Checked = options.StartWhenAvailable;
            _chkRunOnlyIfIdle.Checked = options.RunOnlyIfIdle;
            _chkWakeToRun.Checked = options.WakeToRun;
            _chkNoBatteryLimit.Checked = !options.DisallowStartIfOnBatteries;
            _chkStopOnBattery.Checked = options.StopIfGoingOnBatteries;
            _chkHidden.Checked = options.Hidden;
            SelectByValue(_cmbInstances, options.MultipleInstances);
        }

        private TaskAdvancedOptions Collect()
        {
            return new TaskAdvancedOptions
            {
                Enabled = _chkEnabled.Checked,
                UserId = _txtUserId.Text.Trim(),
                LogonType = GetSelected<TaskLogonType>(_cmbLogonType, TaskLogonType.InteractiveToken),
                RunLevel = GetSelected<TaskRunLevel>(_cmbRunLevel, TaskRunLevel.LUA),
                KeepExistingTriggers = _chkKeepTriggers.Checked,
                TriggerPreset = GetSelected<TriggerPreset>(_cmbTriggerPreset, TriggerPreset.Daily),
                TriggerStartBoundary = _dtTrigger.Value,
                DailyInterval = (short)_numDailyInterval.Value,
                AllowDemandStart = _chkAllowDemandStart.Checked,
                StartWhenAvailable = _chkStartWhenAvailable.Checked,
                RunOnlyIfIdle = _chkRunOnlyIfIdle.Checked,
                WakeToRun = _chkWakeToRun.Checked,
                DisallowStartIfOnBatteries = !_chkNoBatteryLimit.Checked,
                StopIfGoingOnBatteries = _chkStopOnBattery.Checked,
                Hidden = _chkHidden.Checked,
                MultipleInstances = GetSelected<TaskInstancesPolicy>(_cmbInstances, TaskInstancesPolicy.IgnoreNew)
            };
        }

        private void UpdateTriggerInputs()
        {
            var keep = _chkKeepTriggers.Checked;
            _cmbTriggerPreset.Enabled = !keep;
            _dtTrigger.Enabled = !keep;
            var preset = GetSelected<TriggerPreset>(_cmbTriggerPreset, TriggerPreset.Daily);
            _numDailyInterval.Enabled = !keep && preset == TriggerPreset.Daily;
        }

        private static Label MkLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                ForeColor = TechUi.TextMuted,
                Location = new Point(x, y),
                AutoSize = true
            };
        }

        private static TextBox MkText(int x, int y, int w)
        {
            var t = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(w, 23),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = TechUi.ControlBg,
                ForeColor = TechUi.TextMuted,
                Font = new Font("Consolas", 9.75f)
            };
            TechUi.StyleTextBox(t);
            return t;
        }

        private static ComboBox MkCombo(int x, int y, int w)
        {
            return new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(w, 23),
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Popup,
                BackColor = TechUi.ControlBg,
                ForeColor = TechUi.TextMuted
            };
        }

        private static CheckBox MkCheckBox(string text, int x, int y)
        {
            var c = new CheckBox
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true
            };
            TechUi.StyleCheckBox(c, Color.FromArgb(14, 20, 28));
            return c;
        }

        private static T GetSelected<T>(ComboBox combo, T fallback)
        {
            if (combo.SelectedItem is OptionItem item && item.Value is T t)
                return t;
            return fallback;
        }

        private static void SelectByValue(ComboBox combo, object value)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i] is OptionItem item && Equals(item.Value, value))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
            if (combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        private sealed class OptionItem
        {
            public OptionItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; }
            public object Value { get; }
            public override string ToString() => Text;
        }

        private static class NativeMethods
        {
            internal const int WM_NCLBUTTONDOWN = 0xA1;
            internal const int HTCAPTION = 2;

            [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
            internal static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

            [DllImport("user32.dll")]
            internal static extern bool ReleaseCapture();

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        }
    }
}

