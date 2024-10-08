﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LivelyWall
{
    public class ScreenDetails
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        [DllImport("user32.dll")]
        private static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
        public Screen[] AllScreens()
        {
            return Screen.AllScreens;
        }
        public Size Dimensions()
        {
            Screen PrimaryScreen = this.PrimaryScreen();
            int widthWithoutTaskbar = PrimaryScreen.Bounds.Width;
            int heightWithoutTaskbar = PrimaryScreen.Bounds.Height;

            Screen[] screenList = AllScreens();

            Dictionary<Screen, decimal> ScalingFactor = new Dictionary<Screen, decimal>();

            foreach (Screen screen1 in screenList)
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(screen1.DeviceName, -1, ref dm);

                var scalingFactor = Math.Round(Decimal.Divide(dm.dmPelsWidth, screen1.Bounds.Width), 2);
                ScalingFactor.Add(screen1, scalingFactor);
            }
            decimal scalingFactorForPrimary;
            if (ScalingFactor.TryGetValue(PrimaryScreen, out scalingFactorForPrimary))
            {
                decimal width = (widthWithoutTaskbar * scalingFactorForPrimary);
                decimal height = (heightWithoutTaskbar * scalingFactorForPrimary);
                return new Size((int)width, (int)height);
            }
            return new Size(widthWithoutTaskbar, heightWithoutTaskbar);
        }
        public Screen PrimaryScreen()
        {
            return Screen.PrimaryScreen;
        }

    }
}
