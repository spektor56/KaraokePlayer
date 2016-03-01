using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KaraokePlayer
{
    internal class OverlayForm : Form
    {
        private const int DwmwaTransitionsForcedisabled = 3;

        public OverlayForm(ContainerControl parent)
        {
            BackColor = Color.FromArgb(1, 1, 1);
            TransparencyKey = Color.FromArgb(1, 1, 1);
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            AutoScaleMode = AutoScaleMode.None;
            Location = parent.PointToScreen(Point.Empty);
            ClientSize = parent.ClientSize;
            parent.ParentForm.LocationChanged += Cover_LocationChanged;
            parent.ParentForm.ClientSizeChanged += Cover_ClientSizeChanged;
            Show(parent.ParentForm);
            parent.ParentForm.Focus();
            // Disable Aero transitions, the plexiglass gets too visible
            if (Environment.OSVersion.Version.Major >= 6)
            {
                var value = 1;
                DwmSetWindowAttribute(parent.ParentForm.Handle, DwmwaTransitionsForcedisabled, ref value, 4);
            }
        }

        public sealed override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private void Cover_LocationChanged(object sender, EventArgs e)
        {
           // Location = Owner.PointToScreen(Point.Empty);
        }

        private void Cover_ClientSizeChanged(object sender, EventArgs e)
        {
            //ClientSize = Owner.ClientSize;
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

        protected override void OnActivated(EventArgs e)
        {
            // Always keep the owner activated instead
            BeginInvoke(new Action(() => Owner.Activate()));
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int value, int attrLen);
    }
}