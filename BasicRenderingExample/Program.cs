using BasicRenderingExample.ClientBase.UIBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicRenderingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            MCM.openGame();
            MCM.openWindowHost();

            new Thread(() => Application.Run(new Overlay())).Start();

            Thread.Sleep(5);

            while (!Overlay.quit) { }
        }
    }
}
