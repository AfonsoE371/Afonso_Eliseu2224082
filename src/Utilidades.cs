using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YGOShop_AfonsoEliseu_2224082
{
    internal class Utilidades
    {

        public void arredondar(PictureBox pb)
        {

            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pb.Width, pb.Height);
            pb.Region = new Region(path);
        }
    }
}
