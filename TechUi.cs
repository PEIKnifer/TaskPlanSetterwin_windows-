using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TaskPlanSetter
{
    /// <summary>科技风主题色与控件样式（解决 Flat 按钮在 EnableVisualStyles 下文字仍为黑色等问题）。</summary>
    internal static class TechUi
    {
        public static readonly Color Accent = Color.FromArgb(56, 215, 255);
        public static readonly Color AccentStrong = Color.FromArgb(26, 175, 255);
        public static readonly Color PanelBg = Color.FromArgb(9, 14, 23);
        public static readonly Color ControlBg = Color.FromArgb(17, 28, 42);
        public static readonly Color TextMuted = Color.FromArgb(208, 224, 238);
        public static readonly Color SoftText = Color.FromArgb(176, 192, 206);
        public static readonly Color BorderSoft = Color.FromArgb(62, 94, 122);

        public static void StyleOutlinedButton(Button b)
        {
            b.UseVisualStyleBackColor = false;
            b.UseCompatibleTextRendering = true;
            b.FlatStyle = FlatStyle.Flat;
            b.ForeColor = Accent;
            b.BackColor = ControlBg;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = BorderSoft;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 45, 64);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(24, 36, 54);
        }

        public static void StylePrimaryButton(Button b)
        {
            b.UseVisualStyleBackColor = false;
            b.UseCompatibleTextRendering = true;
            b.FlatStyle = FlatStyle.Flat;
            b.ForeColor = Color.FromArgb(8, 20, 33);
            b.BackColor = Accent;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(92, 216, 244);
            b.FlatAppearance.MouseDownBackColor = AccentStrong;
        }

        public static void StyleTitleBarButton(Button b, Color? hoverBack = null)
        {
            b.UseVisualStyleBackColor = false;
            b.UseCompatibleTextRendering = true;
            b.FlatStyle = FlatStyle.Flat;
            b.ForeColor = Accent;
            b.BackColor = Color.FromArgb(16, 22, 30);
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = hoverBack ?? Color.FromArgb(40, 56, 72);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(28, 40, 52);
        }

        public static void StyleCheckBox(CheckBox c, Color back)
        {
            c.UseVisualStyleBackColor = false;
            c.UseCompatibleTextRendering = true;
            c.FlatStyle = FlatStyle.Flat;
            c.ForeColor = Accent;
            c.BackColor = back;
        }

        public static void StyleComboBox(ComboBox combo)
        {
            combo.FlatStyle = FlatStyle.Flat;
            combo.BackColor = ControlBg;
            combo.ForeColor = TextMuted;
            combo.DrawMode = DrawMode.OwnerDrawFixed;
            combo.DrawItem -= Combo_DrawItem;
            combo.DrawItem += Combo_DrawItem;
        }

        public static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = ControlBg;
            textBox.ForeColor = TextMuted;
        }

        public static void StyleNumeric(NumericUpDown numeric)
        {
            numeric.BorderStyle = BorderStyle.FixedSingle;
            numeric.BackColor = ControlBg;
            numeric.ForeColor = TextMuted;
        }

        public static void ApplyRounded(Control c, int radius)
        {
            if (radius <= 0)
                return;

            void update()
            {
                if (c.Width <= 1 || c.Height <= 1)
                    return;
                var path = CreateRoundedPath(new Rectangle(0, 0, c.Width, c.Height), radius);
                c.Region?.Dispose();
                c.Region = new Region(path);
                path.Dispose();
            }

            c.SizeChanged -= Rounded_SizeChanged;
            c.SizeChanged += Rounded_SizeChanged;
            update();

            void Rounded_SizeChanged(object sender, System.EventArgs e) => update();
        }

        private static GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static void Combo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (!(sender is ComboBox combo))
                return;
            if (e.Index < 0)
            {
                e.DrawBackground();
                return;
            }

            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var back = selected ? Color.FromArgb(34, 56, 78) : ControlBg;
            using (var b = new SolidBrush(back))
            {
                e.Graphics.FillRectangle(b, e.Bounds);
            }

            var text = combo.Items[e.Index]?.ToString() ?? string.Empty;
            TextRenderer.DrawText(
                e.Graphics,
                text,
                combo.Font,
                e.Bounds,
                TextMuted,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.LeftAndRightPadding);
        }
    }
}
