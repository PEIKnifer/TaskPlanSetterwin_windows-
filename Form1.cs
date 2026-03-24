using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TaskPlanSetter.Services;

namespace TaskPlanSetter
{
    public partial class Form1 : Form
    {
        private readonly TaskPlanService _service = new TaskPlanService();
        private string _folderPath = "\\";
        private string _taskName;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ApplyTechStyles();
            WireTitleBarDrag();
            ApplyThemeHooks();
            try
            {
                BuildFolderTree();
                SelectFolderNode(_folderPath);
                LoadTaskList(_folderPath);
            }
            catch (Exception ex)
            {
                ShowError("初始化任务计划树失败。", ex);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _service.Dispose();
            base.OnFormClosed(e);
        }

        private void ApplyThemeHooks()
        {
            void hook(Control c)
            {
                if (c == null || !c.IsHandleCreated)
                    return;
                NativeMethods.SetWindowTheme(c.Handle, "DarkMode_Explorer", null);
            }

            treeFolders.HandleCreated += (_, __) => hook(treeFolders);
            listTasks.HandleCreated += (_, __) => hook(listTasks);
            if (treeFolders.IsHandleCreated)
                hook(treeFolders);
            if (listTasks.IsHandleCreated)
                hook(listTasks);

            treeFolders.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeFolders.DrawNode -= TreeFolders_DrawNode;
            treeFolders.DrawNode += TreeFolders_DrawNode;
            listTasks.Resize -= ListTasks_Resize;
            listTasks.Resize += ListTasks_Resize;
        }

        private void ApplyTechStyles()
        {
            TechUi.StylePrimaryButton(btnSave);
            TechUi.StyleOutlinedButton(btnRefresh);
            TechUi.StylePrimaryButton(btnNewTask);
            TechUi.StyleOutlinedButton(btnDeleteTask);
            TechUi.StyleOutlinedButton(btnAdvanced);
            TechUi.StyleOutlinedButton(btnTestRun);
            TechUi.StyleOutlinedButton(btnBrowseProgram);
            TechUi.StyleTitleBarButton(btnMinimize);
            TechUi.StyleTitleBarButton(btnClose, Color.FromArgb(180, 40, 40));
            TechUi.StyleCheckBox(chkEnabled, panelEditFields.BackColor);
            TechUi.StyleTextBox(txtProgram);
            TechUi.StyleTextBox(txtArgs);
            TechUi.StyleTextBox(txtStartIn);
            ConfigureListViewVisuals();
        }

        private void ConfigureListViewVisuals()
        {
            listTasks.OwnerDraw = true;
            listTasks.DrawColumnHeader -= ListTasks_DrawColumnHeader;
            listTasks.DrawSubItem -= ListTasks_DrawSubItem;
            listTasks.DrawItem -= ListTasks_DrawItem;
            listTasks.DrawColumnHeader += ListTasks_DrawColumnHeader;
            listTasks.DrawSubItem += ListTasks_DrawSubItem;
            listTasks.DrawItem += ListTasks_DrawItem;
            UpdateTaskColumnWidths();
        }

        private void ListTasks_Resize(object sender, EventArgs e)
        {
            UpdateTaskColumnWidths();
        }

        private void UpdateTaskColumnWidths()
        {
            if (listTasks.Columns.Count < 3)
                return;

            int total = listTasks.ClientSize.Width - 2;
            if (total < 320)
                return;

            int c1 = 250;
            int c2 = 100;
            int c3 = total - c1 - c2;
            if (c3 < 140)
            {
                c3 = 140;
                c1 = Math.Max(140, total - c2 - c3);
            }

            listTasks.Columns[0].Width = c1;
            listTasks.Columns[1].Width = c2;
            listTasks.Columns[2].Width = c3;
        }

        private void ListTasks_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // 子项绘制阶段处理背景和文字，这里保持空实现避免默认白色行。
        }

        private void ListTasks_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var b = new SolidBrush(Color.FromArgb(24, 34, 46)))
            using (var p = new Pen(Color.FromArgb(48, 70, 92)))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
                e.Graphics.DrawRectangle(p, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            }

            TextRenderer.DrawText(
                e.Graphics,
                e.Header.Text,
                e.Font,
                e.Bounds,
                TechUi.SoftText,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.LeftAndRightPadding);
        }

        private void ListTasks_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            bool selected = e.Item.Selected;
            bool odd = (e.ItemIndex % 2) == 1;
            var back = selected
                ? Color.FromArgb(40, 62, 84)
                : (odd ? Color.FromArgb(15, 24, 35) : Color.FromArgb(13, 20, 30));
            using (var b = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            TextRenderer.DrawText(
                e.Graphics,
                e.SubItem.Text,
                listTasks.Font,
                e.Bounds,
                selected ? Color.FromArgb(235, 242, 248) : TechUi.TextMuted,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.LeftAndRightPadding);
        }

        private void TreeFolders_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null)
                return;

            bool selected = (e.State & TreeNodeStates.Selected) != 0;
            bool focused = treeFolders.Focused;
            var back = selected
                ? (focused ? Color.FromArgb(38, 62, 84) : Color.FromArgb(30, 48, 66))
                : treeFolders.BackColor;
            var fore = selected ? Color.FromArgb(232, 242, 250) : TechUi.TextMuted;
            // 仅绘制文本区域，保留左侧系统绘制的展开/折叠按钮与引导线。
            int textLeft = Math.Max(0, e.Bounds.Left - 2);
            var bounds = new Rectangle(textLeft, e.Bounds.Top, treeFolders.ClientSize.Width - textLeft, e.Bounds.Height);

            using (var b = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(b, bounds);
            }

            TextRenderer.DrawText(
                e.Graphics,
                e.Node.Text,
                treeFolders.Font,
                bounds,
                fore,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        /// <summary>
        /// 无边框窗体上子控件会拦截命中测试，仅靠 WM_NCHITTEST 往往拖不动；用 WM_NCLBUTTONDOWN 模拟标题栏拖动。
        /// </summary>
        private void WireTitleBarDrag()
        {
            panelTitleBar.Cursor = Cursors.SizeAll;
            lblTitle.Cursor = Cursors.SizeAll;
            btnClose.Cursor = Cursors.Default;
            btnMinimize.Cursor = Cursors.Default;
            panelTitleBar.MouseDown += TitleBar_MouseDown;
            lblTitle.MouseDown += TitleBar_MouseDown;
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var src = (Control)sender;
            var screenPt = src.PointToScreen(e.Location);
            if (btnClose.RectangleToScreen(btnClose.ClientRectangle).Contains(screenPt))
                return;
            if (btnMinimize.RectangleToScreen(btnMinimize.ClientRectangle).Contains(screenPt))
                return;

            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, (IntPtr)NativeMethods.HTCAPTION, IntPtr.Zero);
        }

        private void panelTitleBar_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(
                panelTitleBar.ClientRectangle,
                Color.FromArgb(10, 22, 36),
                Color.FromArgb(8, 16, 28),
                LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, panelTitleBar.ClientRectangle);
            }
            using (var pen = new Pen(Color.FromArgb(90, TechUi.Accent), 1f))
            {
                var y = panelTitleBar.Height - 1;
                e.Graphics.DrawLine(pen, 0, y, panelTitleBar.Width, y);
            }
        }

        private void panelEditor_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(60, TechUi.Accent), 1f))
            {
                e.Graphics.DrawLine(pen, 12, 0, panelEditor.Width - 12, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void treeFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string path)
            {
                _folderPath = path;
                _taskName = null;
                ClearEditor();
                try
                {
                    LoadTaskList(path);
                }
                catch (Exception ex)
                {
                    ShowError("加载任务列表失败。", ex);
                }
            }
        }

        private void listTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listTasks.SelectedItems.Count == 0)
            {
                _taskName = null;
                ClearEditor();
                return;
            }

            var name = listTasks.SelectedItems[0].Tag as string;
            if (string.IsNullOrEmpty(name))
                return;

            _taskName = name;
            try
            {
                var model = _service.LoadTask(_folderPath, name);
                chkEnabled.Checked = model.Enabled;
                txtProgram.Text = model.ProgramPath;
                txtArgs.Text = model.Arguments;
                txtStartIn.Text = model.WorkingDirectory;
                lblNoExec.Visible = !model.HasExecAction;
                SetEditorEnabled(model.HasExecAction);
            }
            catch (Exception ex)
            {
                ShowError($"读取任务「{name}」失败。", ex);
                ClearEditor();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_taskName))
            {
                TechDialog.ShowInfo(this, "保存", "请先在列表中选择一个任务。");
                return;
            }

            if (lblNoExec.Visible)
            {
                TechDialog.ShowInfo(this, "保存", "当前任务没有可编辑的「启动程序」操作。");
                return;
            }

            try
            {
                _service.SaveTask(
                    _folderPath,
                    _taskName,
                    chkEnabled.Checked,
                    txtProgram.Text,
                    txtArgs.Text,
                    txtStartIn.Text);
                LoadTaskList(_folderPath);
                ReselectTask(_taskName);
                TechDialog.ShowInfo(this, "保存", "已保存。");
            }
            catch (Exception ex)
            {
                ShowError("保存失败。若目标为系统任务，请尝试以管理员身份运行本程序。", ex);
            }
        }

        private void btnNewTask_Click(object sender, EventArgs e)
        {
            using (var dlg = new TaskCreateDialog())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                var folder = _folderPath;
                var newName = dlg.TaskNameInput;
                try
                {
                    _service.CreateTask(
                        folder,
                        newName,
                        dlg.ProgramPath,
                        dlg.Arguments,
                        dlg.WorkingDirectory,
                        dlg.AdvancedOptions);
                    BuildFolderTree();
                    SelectFolderNode(folder);
                    _folderPath = (treeFolders.SelectedNode?.Tag as string) ?? "\\";
                    LoadTaskList(_folderPath);
                    ReselectTask(newName);
                    TechDialog.ShowInfo(this, "新建任务", "已创建。默认触发为「每日」一次，可在系统任务计划程序中修改触发器。");
                }
                catch (Exception ex)
                {
                    ShowError("创建任务失败。若当前文件夹为系统目录，请尝试以管理员身份运行。", ex);
                }
            }
        }

        private void btnBrowseProgram_Click(object sender, EventArgs e)
        {
            if (lblNoExec.Visible)
            {
                TechDialog.ShowInfo(this, "程序路径", "当前任务没有可编辑的“启动程序”操作。");
                return;
            }

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "选择要运行的程序";
                ofd.Filter = "可执行文件|*.exe;*.bat;*.cmd;*.ps1|所有文件|*.*";
                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                txtProgram.Text = ofd.FileName;
                if (string.IsNullOrWhiteSpace(txtStartIn.Text))
                    txtStartIn.Text = Path.GetDirectoryName(ofd.FileName) ?? string.Empty;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_taskName))
            {
                TechDialog.ShowInfo(this, "高级设置", "请先在列表中选择一个任务。");
                return;
            }

            try
            {
                var options = _service.GetTaskAdvancedOptions(_folderPath, _taskName);
                using (var dlg = new TaskAdvancedDialog(options, isExistingTask: true))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                        return;

                    _service.UpdateTaskAdvancedOptions(_folderPath, _taskName, dlg.Result);
                    LoadTaskList(_folderPath);
                    ReselectTask(_taskName);
                    TechDialog.ShowInfo(this, "高级设置", "高级配置已应用。");
                }
            }
            catch (Exception ex)
            {
                ShowError("应用高级配置失败。", ex);
            }
        }

        private void btnTestRun_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_taskName))
            {
                TechDialog.ShowInfo(this, "测试启动", "请先在列表中选择一个任务。");
                return;
            }

            try
            {
                _service.RunTaskNow(_folderPath, _taskName);
                TechDialog.ShowInfo(this, "测试启动", "已触发任务运行请求。可在“状态/任务计划程序”中查看是否启动成功。");
                LoadTaskList(_folderPath);
                ReselectTask(_taskName);
            }
            catch (Exception ex)
            {
                ShowError("测试启动失败。若为系统任务，请尝试以管理员身份运行。", ex);
            }
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_taskName))
            {
                TechDialog.ShowInfo(this, "删除任务", "请先在列表中选择要删除的任务。");
                return;
            }

            if (!TechDialog.ShowConfirm(this, "删除任务", $"确定删除任务「{_taskName}」？此操作不可撤销。"))
                return;

            var name = _taskName;
            try
            {
                _service.DeleteTask(_folderPath, name);
                _taskName = null;
                ClearEditor();
                LoadTaskList(_folderPath);
                TechDialog.ShowInfo(this, "删除任务", "已删除。");
            }
            catch (Exception ex)
            {
                ShowError("删除失败。若为系统任务或受保护任务，请尝试以管理员身份运行。", ex);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var rememberFolder = _folderPath;
            var rememberTask = _taskName;
            try
            {
                BuildFolderTree();
                SelectFolderNode(rememberFolder);
                _folderPath = (treeFolders.SelectedNode?.Tag as string) ?? "\\";
                LoadTaskList(_folderPath);
                if (!string.IsNullOrEmpty(rememberTask))
                    ReselectTask(rememberTask);
            }
            catch (Exception ex)
            {
                ShowError("刷新失败。", ex);
            }
        }

        private void BuildFolderTree()
        {
            treeFolders.BeginUpdate();
            try
            {
                treeFolders.Nodes.Clear();
                var root = new TreeNode("任务计划程序库") { Tag = "\\" };
                treeFolders.Nodes.Add(root);
                AddChildFolders(root, "\\");
                root.Expand();
            }
            finally
            {
                treeFolders.EndUpdate();
            }
        }

        private void AddChildFolders(TreeNode parent, string path)
        {
            foreach (var sub in _service.EnumerateSubFolderPaths(path))
            {
                var display = GetFolderDisplayName(sub);
                var node = new TreeNode(display) { Tag = sub };
                parent.Nodes.Add(node);
                AddChildFolders(node, sub);
            }
        }

        private static string GetFolderDisplayName(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath) || fullPath == "\\")
                return "任务计划程序库";

            var trimmed = fullPath.TrimEnd('\\');
            var idx = trimmed.LastIndexOf('\\');
            return idx >= 0 ? trimmed.Substring(idx + 1) : trimmed;
        }

        private void SelectFolderNode(string folderPath)
        {
            var found = FindNodeByPath(treeFolders.Nodes, folderPath);
            if (found != null)
            {
                treeFolders.SelectedNode = found;
                found.EnsureVisible();
            }
            else if (treeFolders.Nodes.Count > 0)
            {
                treeFolders.SelectedNode = treeFolders.Nodes[0];
            }
        }

        private static TreeNode FindNodeByPath(TreeNodeCollection nodes, string path)
        {
            foreach (TreeNode n in nodes)
            {
                if (n.Tag is string p && string.Equals(p, path, StringComparison.OrdinalIgnoreCase))
                    return n;
                var child = FindNodeByPath(n.Nodes, path);
                if (child != null)
                    return child;
            }

            return null;
        }

        private void LoadTaskList(string folderPath)
        {
            listTasks.BeginUpdate();
            try
            {
                listTasks.Items.Clear();
                foreach (var row in _service.EnumerateTasks(folderPath))
                {
                    var item = new ListViewItem(row.Name);
                    item.SubItems.Add(row.State.ToString());
                    item.SubItems.Add(row.NextRun.HasValue ? row.NextRun.Value.ToString("g") : "—");
                    item.Tag = row.Name;
                    listTasks.Items.Add(item);
                }
                UpdateTaskColumnWidths();
            }
            finally
            {
                listTasks.EndUpdate();
            }
        }

        private void ReselectTask(string taskName)
        {
            foreach (ListViewItem it in listTasks.Items)
            {
                if (string.Equals(it.Tag as string, taskName, StringComparison.OrdinalIgnoreCase))
                {
                    it.Selected = true;
                    it.EnsureVisible();
                    break;
                }
            }
        }

        private void ClearEditor()
        {
            chkEnabled.Checked = false;
            txtProgram.Clear();
            txtArgs.Clear();
            txtStartIn.Clear();
            lblNoExec.Visible = false;
            SetEditorEnabled(false);
        }

        private void SetEditorEnabled(bool enabled)
        {
            chkEnabled.Enabled = enabled && !string.IsNullOrEmpty(_taskName);
            txtProgram.Enabled = enabled && !string.IsNullOrEmpty(_taskName);
            txtArgs.Enabled = enabled && !string.IsNullOrEmpty(_taskName);
            txtStartIn.Enabled = enabled && !string.IsNullOrEmpty(_taskName);
            btnBrowseProgram.Enabled = enabled && !string.IsNullOrEmpty(_taskName);
            btnSave.Enabled = enabled && !string.IsNullOrEmpty(_taskName);
            btnAdvanced.Enabled = !string.IsNullOrEmpty(_taskName);
            btnTestRun.Enabled = !string.IsNullOrEmpty(_taskName);
        }

        private void ShowError(string message, Exception ex)
        {
            TechDialog.ShowError(this, "TaskPlanSetter", message, ex);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;

            if (m.Msg == WM_NCHITTEST)
            {
                var screen = ScreenPointFromLParam(m.LParam);
                var client = PointToClient(screen);
                const int edge = 8;
                var w = ClientSize.Width;
                var h = ClientSize.Height;

                var left = client.X <= edge;
                var right = client.X >= w - edge;
                var top = client.Y <= edge;
                var bottom = client.Y >= h - edge;

                if (top && left)
                {
                    m.Result = (IntPtr)NativeMethods.HTTOPLEFT;
                    return;
                }

                if (top && right)
                {
                    m.Result = (IntPtr)NativeMethods.HTTOPRIGHT;
                    return;
                }

                if (bottom && left)
                {
                    m.Result = (IntPtr)NativeMethods.HTBOTTOMLEFT;
                    return;
                }

                if (bottom && right)
                {
                    m.Result = (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                    return;
                }

                if (left)
                {
                    m.Result = (IntPtr)NativeMethods.HTLEFT;
                    return;
                }

                if (right)
                {
                    m.Result = (IntPtr)NativeMethods.HTRIGHT;
                    return;
                }

                if (top)
                {
                    m.Result = (IntPtr)NativeMethods.HTTOP;
                    return;
                }

                if (bottom)
                {
                    m.Result = (IntPtr)NativeMethods.HTBOTTOM;
                    return;
                }

                if (panelTitleBar != null)
                {
                    var titleRect = panelTitleBar.RectangleToScreen(panelTitleBar.ClientRectangle);
                    if (titleRect.Contains(screen))
                    {
                        if (btnClose != null && btnClose.RectangleToScreen(btnClose.ClientRectangle).Contains(screen))
                        {
                            m.Result = (IntPtr)NativeMethods.HTCLIENT;
                            return;
                        }

                        if (btnMinimize != null && btnMinimize.RectangleToScreen(btnMinimize.ClientRectangle).Contains(screen))
                        {
                            m.Result = (IntPtr)NativeMethods.HTCLIENT;
                            return;
                        }

                        m.Result = (IntPtr)NativeMethods.HTCAPTION;
                        return;
                    }
                }
            }

            base.WndProc(ref m);
        }

        private static Point ScreenPointFromLParam(IntPtr lParam)
        {
            var lp = lParam.ToInt32();
            return new Point((short)(lp & 0xFFFF), (short)((lp >> 16) & 0xFFFF));
        }

        private static class NativeMethods
        {
            internal const int HTCLIENT = 1;
            internal const int HTCAPTION = 2;
            internal const int HTLEFT = 10;
            internal const int HTRIGHT = 11;
            internal const int HTTOP = 12;
            internal const int HTTOPLEFT = 13;
            internal const int HTTOPRIGHT = 14;
            internal const int HTBOTTOM = 15;
            internal const int HTBOTTOMLEFT = 16;
            internal const int HTBOTTOMRIGHT = 17;

            [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
            internal static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

            internal const int WM_NCLBUTTONDOWN = 0xA1;

            [DllImport("user32.dll")]
            internal static extern bool ReleaseCapture();

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        }
    }
}
