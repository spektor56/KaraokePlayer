using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KaraokePlayer
{
    public partial class KaraokeVideoOverlay : Form
    {
        private const int DwmwaTransitionsForcedisabled = 3;
        ContainerControl _parent;

        public KaraokeVideoOverlay(ContainerControl parent)
        {
            var parentForm = parent.FindForm();
            InitializeComponent();
            Graphic.BackColor = Color.Transparent;
            _parent = parent;
            BackColor = Color.FromArgb(1, 1, 1);
            TransparencyKey = Color.FromArgb(1, 1, 1);
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            AutoScaleMode = AutoScaleMode.None;
            Show(parent);
            parentForm.LocationChanged += Cover_LocationChanged;
            parent.LocationChanged += Cover_LocationChanged;
            parent.VisibleChanged += Cover_LocationChanged;
            parent.ClientSizeChanged += Cover_ClientSizeChanged;            
            // Disable Aero transitions, the plexiglass gets too visible
            if (Environment.OSVersion.Version.Major >= 6)
            {
                var value = 1;
                DwmSetWindowAttribute(parentForm.Handle, DwmwaTransitionsForcedisabled, ref value, 4);
            }
            Location = parent.PointToScreen(Point.Empty);
            ClientSize = parent.ClientSize;
        }

        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private void Cover_LocationChanged(object sender, EventArgs e)
        {
            ClientSize = _parent.ClientSize;
            Location = _parent.PointToScreen(Point.Empty);
        }

        private void Cover_ClientSizeChanged(object sender, EventArgs e)
        {
            ClientSize = _parent.ClientSize;
            Location = _parent.PointToScreen(Point.Empty);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Restore owner
            Owner.LocationChanged -= Cover_LocationChanged;
            Owner.ClientSizeChanged -= Cover_ClientSizeChanged;
            if (!Owner.IsDisposed && Environment.OSVersion.Version.Major >= 6)
            {
                var value = 1;
                DwmSetWindowAttribute(Owner.Handle, DwmwaTransitionsForcedisabled, ref value, 4);
            }
            base.OnFormClosing(e);
        }
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);

        private void Graphic_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void OverlayForm_DoubleClick(object sender, EventArgs e)
        {

        }


    }
}
