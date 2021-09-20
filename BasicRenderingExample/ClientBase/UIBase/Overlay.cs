using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicRenderingExample.ClientBase.UIBase
{
    public partial class Overlay : Form
    {
        public static Overlay handle;
        public static bool quit = false;

        public Overlay()
        {
            InitializeComponent();
            handle = this;

            new Thread(() =>
            {
                Thread.Sleep(100);
                Invoke((MethodInvoker)delegate { Focus(); });
                while (!quit)
                {
                    Thread.Sleep(5);
                    // Thread.Sleep(1);
                    try
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            var rect = MCM.getMinecraftRect();

                            var e = new Placement();
                            GetWindowPlacement(MCM.mcWinHandle,
                                ref e); // Change window size if fullscreen to match extra offsets
                            var vE = 0;
                            var vA = 0;
                            var vB = 0;
                            var vC = 0;
                            if (e.showCmd == 3) // Perfect window offsets
                            {
                                vE = 8;
                                vA = 2;

                                vB = 9; // these have extra because of the windows shadow effect (Not exactly required but oh well)
                                vC = 3;
                            }

                            Location = new Point(rect.Left + 9 + vA, rect.Top + 35 + vE); // Title bar is 32 pixels high
                            Size = new Size(rect.Right - rect.Left - 18 - vC, rect.Bottom - rect.Top - 44 - vB);
                        });
                    }
                    catch
                    {
                    }
                }

                Application.Exit();
            }).Start();
            TopMost = true;
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref Placement lpwndpl);

        private void TopmostHandler_Tick(object sender, EventArgs e)
        {
            try // fixed
            {
                if (MCM.isMinecraftFocused() && TopMost == false)
                    TopMost = true;
                if (MCM.isMinecraftFocused() || !TopMost) return;
                if (ActiveForm == this) return;
                Opacity = 1;
                TopMost = false;
                SetWindowPos(Handle, new IntPtr(1), 0, 0, 0, 0, 2 | 1 | 10);
            }
            catch
            {
                // ignored
            }
        }

        private struct Placement
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        private void Overlay_FormClosing(object sender, FormClosingEventArgs e) => quit = true;

        private void Overlay_Load(object sender, EventArgs e)
        {

        }
    }
}
