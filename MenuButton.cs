using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace AdamOneilSoftware.Controls
{
    public class MenuButton : Button
    {
        public ContextMenuStrip Menu { get; set; }

        public MenuButton()
        {
            //TextAlign = ContentAlignment.MiddleLeft;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (Menu != null) Menu.Show(this, new Point(0, this.Height));
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            int left = this.Width - 20;
            int top = 10;
            int width = 10;
            int height = 5;

            pevent.Graphics.FillPolygon(new SolidBrush(Color.Black), new Point[] {
                new Point(left, top), new Point(left + width, top), new Point(left + (width / 2), height + top)
            });
        }
    }
}
