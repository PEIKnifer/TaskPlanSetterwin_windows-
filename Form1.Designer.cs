namespace TaskPlanSetter
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.treeFolders = new System.Windows.Forms.TreeView();
            this.splitBody = new System.Windows.Forms.SplitContainer();
            this.panelListToolbar = new System.Windows.Forms.Panel();
            this.btnNewTask = new System.Windows.Forms.Button();
            this.btnDeleteTask = new System.Windows.Forms.Button();
            this.listTasks = new System.Windows.Forms.ListView();
            this.colTaskName = new System.Windows.Forms.ColumnHeader();
            this.colState = new System.Windows.Forms.ColumnHeader();
            this.colNext = new System.Windows.Forms.ColumnHeader();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.panelEditFields = new System.Windows.Forms.Panel();
            this.panelEditFooter = new System.Windows.Forms.Panel();
            this.flowFooterButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNoExec = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.btnTestRun = new System.Windows.Forms.Button();
            this.txtStartIn = new System.Windows.Forms.TextBox();
            this.lblStartIn = new System.Windows.Forms.Label();
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.lblArgs = new System.Windows.Forms.Label();
            this.txtProgram = new System.Windows.Forms.TextBox();
            this.btnBrowseProgram = new System.Windows.Forms.Button();
            this.lblProgram = new System.Windows.Forms.Label();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.panelTitleBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).BeginInit();
            this.splitBody.Panel1.SuspendLayout();
            this.splitBody.Panel2.SuspendLayout();
            this.splitBody.SuspendLayout();
            this.panelListToolbar.SuspendLayout();
            this.panelEditor.SuspendLayout();
            this.panelEditFields.SuspendLayout();
            this.panelEditFooter.SuspendLayout();
            this.flowFooterButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // panelTitleBar
            //
            this.panelTitleBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(22)))), ((int)(((byte)(30)))));
            this.panelTitleBar.Controls.Add(this.lblTitle);
            this.panelTitleBar.Controls.Add(this.btnMinimize);
            this.panelTitleBar.Controls.Add(this.btnClose);
            this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitleBar.Location = new System.Drawing.Point(0, 0);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(984, 40);
            this.panelTitleBar.TabIndex = 0;
            this.panelTitleBar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTitleBar_Paint);
            //
            // lblTitle
            //
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(14, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(198, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TaskPlanSetter · 任务计划";
            //
            // btnMinimize
            //
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(56)))), ((int)(((byte)(72)))));
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnMinimize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnMinimize.Location = new System.Drawing.Point(896, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(36, 32);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.Text = "—";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            //
            // btnClose
            //
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnClose.Location = new System.Drawing.Point(938, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(36, 32);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // splitMain
            //
            this.splitMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 40);
            this.splitMain.Name = "splitMain";
            //
            // splitMain.Panel1
            //
            this.splitMain.Panel1.Controls.Add(this.treeFolders);
            //
            // splitMain.Panel2
            //
            this.splitMain.Panel2.Controls.Add(this.splitBody);
            this.splitMain.Size = new System.Drawing.Size(984, 521);
            this.splitMain.SplitterDistance = 260;
            this.splitMain.SplitterWidth = 6;
            this.splitMain.TabIndex = 1;
            //
            // treeFolders
            //
            this.treeFolders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(20)))), ((int)(((byte)(28)))));
            this.treeFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFolders.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.treeFolders.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(245)))));
            this.treeFolders.HideSelection = false;
            this.treeFolders.Location = new System.Drawing.Point(0, 0);
            this.treeFolders.Name = "treeFolders";
            this.treeFolders.ShowLines = true;
            this.treeFolders.ShowPlusMinus = true;
            this.treeFolders.ShowRootLines = true;
            this.treeFolders.Size = new System.Drawing.Size(260, 521);
            this.treeFolders.TabIndex = 0;
            this.treeFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeFolders_AfterSelect);
            //
            // splitBody
            //
            this.splitBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.splitBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBody.Location = new System.Drawing.Point(0, 0);
            this.splitBody.Name = "splitBody";
            this.splitBody.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitBody.Panel1
            //
            this.splitBody.Panel1.Controls.Add(this.listTasks);
            this.splitBody.Panel1.Controls.Add(this.panelListToolbar);
            //
            // splitBody.Panel2
            //
            this.splitBody.Panel2.Controls.Add(this.panelEditor);
            this.splitBody.Size = new System.Drawing.Size(718, 521);
            this.splitBody.SplitterDistance = 200;
            this.splitBody.SplitterWidth = 6;
            this.splitBody.TabIndex = 0;
            this.splitBody.Panel1MinSize = 100;
            this.splitBody.Panel2MinSize = 260;
            //
            // panelListToolbar
            //
            this.panelListToolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(20)))), ((int)(((byte)(28)))));
            this.panelListToolbar.Controls.Add(this.btnDeleteTask);
            this.panelListToolbar.Controls.Add(this.btnNewTask);
            this.panelListToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelListToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelListToolbar.Name = "panelListToolbar";
            this.panelListToolbar.Size = new System.Drawing.Size(718, 40);
            this.panelListToolbar.TabIndex = 1;
            //
            // btnNewTask
            //
            this.btnNewTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewTask.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNewTask.Location = new System.Drawing.Point(10, 6);
            this.btnNewTask.Name = "btnNewTask";
            this.btnNewTask.Size = new System.Drawing.Size(96, 28);
            this.btnNewTask.TabIndex = 0;
            this.btnNewTask.Text = "新建任务";
            this.btnNewTask.UseVisualStyleBackColor = false;
            this.btnNewTask.Click += new System.EventHandler(this.btnNewTask_Click);
            //
            // btnDeleteTask
            //
            this.btnDeleteTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteTask.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDeleteTask.Location = new System.Drawing.Point(116, 6);
            this.btnDeleteTask.Name = "btnDeleteTask";
            this.btnDeleteTask.Size = new System.Drawing.Size(96, 28);
            this.btnDeleteTask.TabIndex = 1;
            this.btnDeleteTask.Text = "删除任务";
            this.btnDeleteTask.UseVisualStyleBackColor = false;
            this.btnDeleteTask.Click += new System.EventHandler(this.btnDeleteTask_Click);
            //
            // listTasks
            //
            this.listTasks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(20)))), ((int)(((byte)(28)))));
            this.listTasks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTaskName,
            this.colState,
            this.colNext});
            this.listTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listTasks.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.listTasks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(245)))));
            this.listTasks.FullRowSelect = true;
            this.listTasks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listTasks.HideSelection = false;
            this.listTasks.Location = new System.Drawing.Point(0, 40);
            this.listTasks.MultiSelect = false;
            this.listTasks.Name = "listTasks";
            this.listTasks.OwnerDraw = false;
            this.listTasks.Size = new System.Drawing.Size(718, 180);
            this.listTasks.TabIndex = 0;
            this.listTasks.UseCompatibleStateImageBehavior = false;
            this.listTasks.View = System.Windows.Forms.View.Details;
            this.listTasks.SelectedIndexChanged += new System.EventHandler(this.listTasks_SelectedIndexChanged);
            //
            // colTaskName
            //
            this.colTaskName.Text = "任务名";
            this.colTaskName.Width = 220;
            //
            // colState
            //
            this.colState.Text = "状态";
            this.colState.Width = 100;
            //
            // colNext
            //
            this.colNext.Text = "下次运行";
            this.colNext.Width = 200;
            //
            // panelEditor
            //
            this.panelEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.panelEditor.Controls.Add(this.panelEditFooter);
            this.panelEditor.Controls.Add(this.panelEditFields);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(0);
            this.panelEditor.Size = new System.Drawing.Size(718, 315);
            this.panelEditor.TabIndex = 0;
            this.panelEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.panelEditor_Paint);
            //
            // panelEditFields
            //
            this.panelEditFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.panelEditFields.Controls.Add(this.lblNoExec);
            this.panelEditFields.Controls.Add(this.txtStartIn);
            this.panelEditFields.Controls.Add(this.lblStartIn);
            this.panelEditFields.Controls.Add(this.txtArgs);
            this.panelEditFields.Controls.Add(this.lblArgs);
            this.panelEditFields.Controls.Add(this.txtProgram);
            this.panelEditFields.Controls.Add(this.btnBrowseProgram);
            this.panelEditFields.Controls.Add(this.lblProgram);
            this.panelEditFields.Controls.Add(this.chkEnabled);
            this.panelEditFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditFields.Location = new System.Drawing.Point(0, 0);
            this.panelEditFields.Name = "panelEditFields";
            this.panelEditFields.Padding = new System.Windows.Forms.Padding(12, 10, 12, 8);
            this.panelEditFields.Size = new System.Drawing.Size(718, 253);
            this.panelEditFields.TabIndex = 1;
            //
            // panelEditFooter
            //
            this.panelEditFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.panelEditFooter.Controls.Add(this.lblHint);
            this.panelEditFooter.Controls.Add(this.flowFooterButtons);
            this.panelEditFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEditFooter.Location = new System.Drawing.Point(0, 251);
            this.panelEditFooter.Name = "panelEditFooter";
            this.panelEditFooter.Size = new System.Drawing.Size(718, 64);
            this.panelEditFooter.TabIndex = 0;
            //
            // flowFooterButtons
            //
            this.flowFooterButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.flowFooterButtons.Controls.Add(this.btnSave);
            this.flowFooterButtons.Controls.Add(this.btnRefresh);
            this.flowFooterButtons.Controls.Add(this.btnAdvanced);
            this.flowFooterButtons.Controls.Add(this.btnTestRun);
            this.flowFooterButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowFooterButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowFooterButtons.Location = new System.Drawing.Point(0, 32);
            this.flowFooterButtons.Name = "flowFooterButtons";
            this.flowFooterButtons.Padding = new System.Windows.Forms.Padding(8, 2, 12, 4);
            this.flowFooterButtons.Size = new System.Drawing.Size(718, 32);
            this.flowFooterButtons.TabIndex = 1;
            this.flowFooterButtons.WrapContents = false;
            //
            // lblNoExec
            //
            this.lblNoExec.AutoSize = true;
            this.lblNoExec.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblNoExec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(180)))), ((int)(((byte)(80)))));
            this.lblNoExec.Location = new System.Drawing.Point(16, 38);
            this.lblNoExec.Name = "lblNoExec";
            this.lblNoExec.Size = new System.Drawing.Size(347, 17);
            this.lblNoExec.TabIndex = 9;
            this.lblNoExec.Text = "当前任务没有「启动程序」操作，本版本无法编辑此项。";
            this.lblNoExec.Visible = false;
            //
            // lblHint
            //
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(150)))), ((int)(((byte)(170)))));
            this.lblHint.Location = new System.Drawing.Point(0, 0);
            this.lblHint.Name = "lblHint";
            this.lblHint.Padding = new System.Windows.Forms.Padding(14, 6, 14, 0);
            this.lblHint.Size = new System.Drawing.Size(718, 32);
            this.lblHint.TabIndex = 0;
            this.lblHint.Text = "提示：程序路径支持“浏览...”选择；高级页可配置启动用户/方式/时机（已存在任务默认读取现有设置）。";
            //
            // btnRefresh
            //
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnRefresh.Location = new System.Drawing.Point(518, 2);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 30);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // btnSave
            //
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnSave.Location = new System.Drawing.Point(622, 2);
            this.btnSave.Margin = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            //
            // btnAdvanced
            //
            this.btnAdvanced.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvanced.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAdvanced.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnAdvanced.Location = new System.Drawing.Point(414, 2);
            this.btnAdvanced.Margin = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(96, 30);
            this.btnAdvanced.TabIndex = 8;
            this.btnAdvanced.Text = "高级...";
            this.btnAdvanced.UseVisualStyleBackColor = false;
            this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            //
            // btnTestRun
            //
            this.btnTestRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestRun.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTestRun.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnTestRun.Location = new System.Drawing.Point(310, 2);
            this.btnTestRun.Margin = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.btnTestRun.Name = "btnTestRun";
            this.btnTestRun.Size = new System.Drawing.Size(96, 30);
            this.btnTestRun.TabIndex = 11;
            this.btnTestRun.Text = "测试启动";
            this.btnTestRun.UseVisualStyleBackColor = false;
            this.btnTestRun.Click += new System.EventHandler(this.btnTestRun_Click);
            //
            // txtStartIn
            //
            this.txtStartIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(26)))), ((int)(((byte)(36)))));
            this.txtStartIn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartIn.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.txtStartIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
            this.txtStartIn.Location = new System.Drawing.Point(120, 104);
            this.txtStartIn.Name = "txtStartIn";
            this.txtStartIn.Size = new System.Drawing.Size(574, 23);
            this.txtStartIn.TabIndex = 5;
            //
            // lblStartIn
            //
            this.lblStartIn.AutoSize = true;
            this.lblStartIn.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblStartIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(235)))));
            this.lblStartIn.Location = new System.Drawing.Point(16, 106);
            this.lblStartIn.Name = "lblStartIn";
            this.lblStartIn.Size = new System.Drawing.Size(48, 17);
            this.lblStartIn.TabIndex = 4;
            this.lblStartIn.Text = "起始于";
            //
            // txtArgs
            //
            this.txtArgs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArgs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(26)))), ((int)(((byte)(36)))));
            this.txtArgs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtArgs.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.txtArgs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
            this.txtArgs.Location = new System.Drawing.Point(120, 72);
            this.txtArgs.Name = "txtArgs";
            this.txtArgs.Size = new System.Drawing.Size(574, 23);
            this.txtArgs.TabIndex = 3;
            //
            // lblArgs
            //
            this.lblArgs.AutoSize = true;
            this.lblArgs.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblArgs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(235)))));
            this.lblArgs.Location = new System.Drawing.Point(16, 74);
            this.lblArgs.Name = "lblArgs";
            this.lblArgs.Size = new System.Drawing.Size(35, 17);
            this.lblArgs.TabIndex = 2;
            this.lblArgs.Text = "参数";
            //
            // txtProgram
            //
            this.txtProgram.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(26)))), ((int)(((byte)(36)))));
            this.txtProgram.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProgram.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.txtProgram.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
            this.txtProgram.Location = new System.Drawing.Point(120, 40);
            this.txtProgram.Name = "txtProgram";
            this.txtProgram.Size = new System.Drawing.Size(482, 23);
            this.txtProgram.TabIndex = 1;
            //
            // btnBrowseProgram
            //
            this.btnBrowseProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseProgram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseProgram.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowseProgram.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.btnBrowseProgram.Location = new System.Drawing.Point(608, 38);
            this.btnBrowseProgram.Name = "btnBrowseProgram";
            this.btnBrowseProgram.Size = new System.Drawing.Size(86, 26);
            this.btnBrowseProgram.TabIndex = 10;
            this.btnBrowseProgram.Text = "浏览...";
            this.btnBrowseProgram.UseVisualStyleBackColor = false;
            this.btnBrowseProgram.Click += new System.EventHandler(this.btnBrowseProgram_Click);
            //
            // lblProgram
            //
            this.lblProgram.AutoSize = true;
            this.lblProgram.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblProgram.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(220)))), ((int)(((byte)(235)))));
            this.lblProgram.Location = new System.Drawing.Point(16, 42);
            this.lblProgram.Name = "lblProgram";
            this.lblProgram.Size = new System.Drawing.Size(61, 17);
            this.lblProgram.TabIndex = 0;
            this.lblProgram.Text = "程序路径";
            //
            // chkEnabled
            //
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.chkEnabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.chkEnabled.Location = new System.Drawing.Point(16, 12);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(54, 21);
            this.chkEnabled.TabIndex = 0;
            this.chkEnabled.Text = "启用";
            this.chkEnabled.UseVisualStyleBackColor = false;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(15)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.panelTitleBar);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(250)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(760, 540);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TaskPlanSetter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelTitleBar.ResumeLayout(false);
            this.panelTitleBar.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.panelListToolbar.ResumeLayout(false);
            this.splitBody.Panel1.ResumeLayout(false);
            this.splitBody.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).EndInit();
            this.splitBody.ResumeLayout(false);
            this.flowFooterButtons.ResumeLayout(false);
            this.flowFooterButtons.PerformLayout();
            this.panelEditFooter.ResumeLayout(false);
            this.panelEditFields.ResumeLayout(false);
            this.panelEditor.ResumeLayout(false);
            this.panelEditor.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTitleBar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView treeFolders;
        private System.Windows.Forms.SplitContainer splitBody;
        private System.Windows.Forms.Panel panelListToolbar;
        private System.Windows.Forms.Button btnNewTask;
        private System.Windows.Forms.Button btnDeleteTask;
        private System.Windows.Forms.ListView listTasks;
        private System.Windows.Forms.ColumnHeader colTaskName;
        private System.Windows.Forms.ColumnHeader colState;
        private System.Windows.Forms.ColumnHeader colNext;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.Panel panelEditFields;
        private System.Windows.Forms.Panel panelEditFooter;
        private System.Windows.Forms.FlowLayoutPanel flowFooterButtons;
        private System.Windows.Forms.CheckBox chkEnabled;
        private System.Windows.Forms.Label lblProgram;
        private System.Windows.Forms.TextBox txtProgram;
        private System.Windows.Forms.Label lblArgs;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.Label lblStartIn;
        private System.Windows.Forms.TextBox txtStartIn;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Button btnTestRun;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.Label lblNoExec;
        private System.Windows.Forms.Button btnBrowseProgram;
    }
}
