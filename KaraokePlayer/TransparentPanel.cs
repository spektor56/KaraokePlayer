using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraokePlayer
{
    class TransparentPanel : PictureBox
    {
        public TransparentPanel()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
                        ControlStyles.AllPaintingInWmPaint |
                        ControlStyles.UserPaint |
                        ControlStyles.Opaque, true);

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            e.Graphics.FillRectangle(myBrush, new Rectangle(0, 0, 200, 300));
            //base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            e.Graphics.FillRectangle(myBrush, new Rectangle(0, 0, 200, 300));
            //base.OnPaintBackground(e);
        }


    }
}
