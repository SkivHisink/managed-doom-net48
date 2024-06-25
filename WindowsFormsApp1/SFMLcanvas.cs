using ManagedDoom.SFML;
using ManagedDoom;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using static ManagedDoom.CommandLineArgs;

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
            RenderLoopWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
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
            RendWind.DispatchEvents();
            RendWind.Clear(new SFML.Graphics.Color((byte)51, (byte)77, (byte)102));
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(ApplicationInfo.Title);
            Console.ResetColor();

            try
            {
                string quitMessage = null;

                using (var app = new SfmlDoom(new CommandLineArgs(new string[] { }), RendWind))
                {
                    app.Run();
                    quitMessage = app.QuitMessage;
                }

                if (quitMessage != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(quitMessage);
                    Console.ResetColor();
                    Console.Write("Press any key to exit.");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
                Console.Write("Press any key to exit.");
                Console.ReadKey();
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.ParentForm.Close();
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
