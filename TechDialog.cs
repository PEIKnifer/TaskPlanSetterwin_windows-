using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TaskPlanSetter
{
    /// <summary>与主界面一致的深色对话框，避免系统 MessageBox 上「确定」等按钮文字为黑色。</summary>
    internal static class TechDialog
    {
        public static void ShowInfo(IWin32Window owner, string title, string message)
        {
            using (var f = CreateShell(owner, title, 420, 180))
            {
                var lbl = CreateMessageLabel(message, 16, 16, f.ClientSize.Width - 32, 80);
                f.Controls.Add(lbl);

                var btnOk = CreateOkButton(f, "确定", DialogResult.OK);
                f.Controls.Add(btnOk);
                f.AcceptButton = btnOk;
                f.ShowDialog(owner);
            }
        }

        public static bool ShowConfirm(IWin32Window owner, string title, string message)
        {
            using (var f = CreateShell(owner, title, 440, 200))
            {
                var lbl = CreateMessageLabel(message, 16, 16, f.ClientSize.Width - 32, 90);
                f.Controls.Add(lbl);

                var btnOk = new Button
                {
                    Text = "确定",
                    DialogResult = DialogResult.OK,
                    Size = new Size(96, 30),
                    Location = new Point(f.ClientSize.Width - 216, f.ClientSize.Height - 44),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Cursor = Cursors.Hand
                };
                TechUi.StyleOutlinedButton(btnOk);

                var btnCancel = new Button
                {
                    Text = "取消",
                    DialogResult = DialogResult.Cancel,
                    Size = new Size(96, 30),
                    Location = new Point(f.ClientSize.Width - 108, f.ClientSize.Height - 44),
                    Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                    Cursor = Cursors.Hand
                };
                TechUi.StyleOutlinedButton(btnCancel);

                f.Controls.Add(btnOk);
                f.Controls.Add(btnCancel);
                f.CancelButton = btnCancel;
                f.AcceptButton = btnOk;
                return f.ShowDialog(owner) == DialogResult.OK;
            }
        }

        public static void ShowError(IWin32Window owner, string title, string message, Exception ex)
        {
            var sb = new StringBuilder(message ?? string.Empty);
            if (ex != null)
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.Append(ex);
            }

            using (var f = CreateShell(owner, title, 520, 360))
            {
                var tb = new TextBox
                {
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = TechUi.ControlBg,
                    ForeColor = Color.FromArgb(230, 240, 250),
                    Font = new Font("Consolas", 9f),
                    Location = new Point(16, 48),
                    Size = new Size(f.ClientSize.Width - 32, f.ClientSize.Height - 100),
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    TabStop = false,
                    Text = sb.ToString()
                };
                f.Controls.Add(tb);

                var btnOk = CreateOkButton(f, "确定", DialogResult.OK);
                f.Controls.Add(btnOk);
                f.AcceptButton = btnOk;
                f.ShowDialog(owner);
            }
        }

        private static Form CreateShell(IWin32Window owner, string title, int w, int h)
        {
            var f = new Form
            {
                Text = title,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = false,
                ClientSize = new Size(w, h),
                BackColor = TechUi.PanelBg,
                ForeColor = TechUi.TextMuted,
                Font = new Font("Segoe UI", 9f),
                Padding = new Padding(0)
            };

            var caption = new Label
            {
                Text = title,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = TechUi.Accent,
                AutoSize = false,
                Location = new Point(16, 14),
                Size = new Size(w - 32, 24)
            };
            f.Controls.Add(caption);
            return f;
        }

        private static Label CreateMessageLabel(string text, int x, int y, int width, int height)
        {
            return new Label
            {
                Text = text,
                ForeColor = TechUi.TextMuted,
                Location = new Point(x, y),
                Size = new Size(width, height),
                AutoSize = false
            };
        }

        private static Button CreateOkButton(Form parent, string text, DialogResult dr)
        {
            var btn = new Button
            {
                Text = text,
                DialogResult = dr,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Size = new Size(96, 30),
                Location = new Point(parent.ClientSize.Width - 108, parent.ClientSize.Height - 44),
                Cursor = Cursors.Hand
            };
            TechUi.StyleOutlinedButton(btn);
            return btn;
        }
    }
}
