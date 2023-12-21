using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class SFMLcanvas : UserControl
    {
        private RenderWindow RendWind;

        public SFMLcanvas()
        {
            InitializeComponent();
        }

        public void StartSLMF()
        {
            RenderLoopWorker.RunWorkerAsync(this.Handle); //RenderLoopWorker is a BackgroundWorker
        }

        private void DrawStuff()
        {
            CircleShape CS = new CircleShape(10);
            CS.FillColor = SFML.Graphics.Color.Magenta;
            RendWind.Draw(CS);
        }

        private void RenderLoopWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            RendWind = new RenderWindow((IntPtr)e.Argument);
            while (RendWind.IsOpen)
            {
                RendWind.DispatchEvents();
                RendWind.Clear(new SFML.Graphics.Color((byte)51, (byte)77, (byte)102));
                DrawStuff();
                RendWind.Display();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (RendWind == null)
                base.OnPaint(e);
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
        {
            if (RendWind == null)
                base.OnPaintBackground(pevent);
        }
    }
}
