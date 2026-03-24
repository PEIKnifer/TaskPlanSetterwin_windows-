using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TaskPlanSetter.Services;

namespace TaskPlanSetter
{
    /// <summary>新建任务：名称 + 启动程序信息；触发器由服务层写入默认策略。</summary>
    internal sealed class TaskCreateDialog : Form
    {
        private readonly TextBox _txtName;
        private readonly TextBox _txtProgram;
        private readonly TextBox _txtArgs;
        private readonly TextBox _txtStartIn;
        private readonly CheckBox _chkEnabled;
        private TaskAdvancedOptions _advanced;

        public string TaskNameInput => _txtName.Text.Trim();
        public string ProgramPath => _txtProgram.Text.Trim();
        public string Arguments => _txtArgs.Text;
        public string WorkingDirectory => _txtStartIn.Text.Trim();
        public bool TaskEnabled => _chkEnabled.Checked;
        public TaskAdvancedOptions AdvancedOptions => _advanced;

        public TaskCreateDialog()
        {
            Text = "新建任务计划";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            ClientSize = new Size(640, 360);
            BackColor = TechUi.PanelBg;
            Font = new Font("Segoe UI", 9f);
            _advanced = TaskAdvancedOptions.CreateDefault();

            int y = 16;
            const int labelW = 88;
            const int left = 16;
            const int gap = 34;

            Controls.Add(MkCaption("新建任务（默认高级选项可通过“高级...”调整）", left, y, ClientSize.Width - 32));
            y += 36;

            Controls.Add(MkLabel("任务名称", left, y));
            _txtName = MkText(left + labelW, y, ClientSize.Width - left - labelW - 16);
            Controls.Add(_txtName);
            y += gap;

            Controls.Add(MkLabel("程序路径", left, y));
            _txtProgram = MkText(left + labelW, y, ClientSize.Width - left - labelW - 124);
            Controls.Add(_txtProgram);
            var btnBrowseProgram = new Button
            {
                Text = "浏览...",
                Location = new Point(ClientSize.Width - 108, y - 1),
                Size = new Size(92, 26),
                Cursor = Cursors.Hand
            };
            TechUi.StyleOutlinedButton(btnBrowseProgram);
            btnBrowseProgram.Click += (_, __) => BrowseProgram(_txtProgram, _txtStartIn);
            Controls.Add(btnBrowseProgram);
            y += gap;

            Controls.Add(MkLabel("参数", left, y));
            _txtArgs = MkText(left + labelW, y, ClientSize.Width - left - labelW - 16);
            Controls.Add(_txtArgs);
            y += gap;

            Controls.Add(MkLabel("起始于", left, y));
            _txtStartIn = MkText(left + labelW, y, ClientSize.Width - left - labelW - 16);
            Controls.Add(_txtStartIn);
            y += gap + 2;

            _chkEnabled = new CheckBox
            {
                Text = "创建后启用",
                Location = new Point(left + labelW, y),
                AutoSize = true,
                Checked = true
            };
            TechUi.StyleCheckBox(_chkEnabled, TechUi.PanelBg);
            Controls.Add(_chkEnabled);

            var btnAdvanced = new Button
            {
                Text = "高级...",
                Size = new Size(96, 30),
                Location = new Point(16, ClientSize.Height - 44),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Cursor = Cursors.Hand
            };
            TechUi.StyleOutlinedButton(btnAdvanced);
            btnAdvanced.Click += (_, __) =>
            {
                using (var dlg = new TaskAdvancedDialog(_advanced, isExistingTask: false))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        _advanced = dlg.Result;
                        _chkEnabled.Checked = _advanced.Enabled;
                    }
                }
            };

            var btnOk = new Button
            {
                Text = "创建",
                DialogResult = DialogResult.None,
                Size = new Size(96, 30),
                Location = new Point(ClientSize.Width - 220, ClientSize.Height - 44),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            TechUi.StylePrimaryButton(btnOk);
            btnOk.Click += (_, __) =>
            {
                if (!TryValidate())
                    return;
                _advanced.Enabled = _chkEnabled.Checked;
                DialogResult = DialogResult.OK;
                Close();
            };

            var btnCancel = new Button
            {
                Text = "取消",
                DialogResult = DialogResult.Cancel,
                Size = new Size(96, 30),
                Location = new Point(ClientSize.Width - 108, ClientSize.Height - 44),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            TechUi.StyleOutlinedButton(btnCancel);

            Controls.Add(btnAdvanced);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }

        private bool TryValidate()
        {
            if (string.IsNullOrWhiteSpace(TaskNameInput))
            {
                TechDialog.ShowInfo(this, "校验", "请输入任务名称。");
                _txtName.Focus();
                return false;
            }

            if (TaskNameInput.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                TaskNameInput.Contains("\\") || TaskNameInput.Contains("/"))
            {
                TechDialog.ShowInfo(this, "校验", "任务名称不能包含路径分隔符或非法文件名字符。");
                _txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(ProgramPath))
            {
                TechDialog.ShowInfo(this, "校验", "请输入要启动的程序路径。");
                _txtProgram.Focus();
                return false;
            }

            return true;
        }

        private static void BrowseProgram(TextBox programBox, TextBox startInBox)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "选择要运行的程序";
                ofd.Filter = "可执行文件|*.exe;*.bat;*.cmd;*.ps1|所有文件|*.*";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                programBox.Text = ofd.FileName;
                if (string.IsNullOrWhiteSpace(startInBox.Text))
                    startInBox.Text = Path.GetDirectoryName(ofd.FileName) ?? string.Empty;
            }
        }

        private static Label MkCaption(string text, int x, int y, int w)
        {
            return new Label
            {
                Text = text,
                ForeColor = TechUi.Accent,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(x, y),
                Size = new Size(w, 36),
                AutoSize = false
            };
        }

        private static Label MkLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                ForeColor = TechUi.TextMuted,
                Location = new Point(x, y + 3),
                AutoSize = true
            };
        }

        private static TextBox MkText(int x, int y, int width)
        {
            var t = new TextBox
            {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = TechUi.ControlBg,
                ForeColor = TechUi.TextMuted,
                Font = new Font("Consolas", 9.75f),
                Location = new Point(x, y),
                Size = new Size(width, 23)
            };
            TechUi.StyleTextBox(t);
            return t;
        }
    }
}
