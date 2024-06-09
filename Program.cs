﻿using System;
using System.Windows.Forms;

namespace LivelyWall
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new Controller.Controller(); 
            Application.Run();
            Application.Exit();
        }
    }
}
