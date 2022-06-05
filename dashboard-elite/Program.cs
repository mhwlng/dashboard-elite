using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using dashboard_elite.Audio;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using PhotinoNET;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using System.Net.Http;

namespace dashboard_elite
{

    public class Program
    {

        public static PhotinoWindow mainWindow;

        [STAThread]
        public static int Main(string[] args)
        {
            Common.Startup(args);

            OpenInLine(Common.Address, Common.ConfigurationRoot);

            return 0;
        }
        
        public static void OpenInLine(string address, IConfigurationRoot configurationRoot)
        {
            string windowTitle = "Elite Dangerous Dashboard";
            var iconFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "wwwroot/img/elite.ico"
                : "wwwroot/img/elite.png";


            var top = configurationRoot.GetValue<int>("Dimensions:Top");
            var left = configurationRoot.GetValue<int>("Dimensions:left");
            var height = configurationRoot.GetValue<int>("Dimensions:Height");
            var width = configurationRoot.GetValue<int>("Dimensions:Width");
            var fullScreen = configurationRoot.GetValue<bool>("Dimensions:FullScreen");
            var zoom = configurationRoot.GetValue<int>("Dimensions:Zoom");

            Common.FullScreenBorder = configurationRoot.GetValue<int>("Dimensions:FullScreenBorder");


            Common.Minimized = configurationRoot.GetValue<bool>("Dimensions:Minimized");

            bool Debug = false;
#if DEBUG
            Debug = true;
#endif

            mainWindow = new PhotinoWindow()

                .SetIconFile(iconFile)

                .SetTitle(windowTitle)
                .SetUseOsDefaultSize(false)
                .SetUseOsDefaultLocation(false)
                .SetSize(new Size(width, height))
                .SetLeft(left)
                .SetTop(top)
                .SetResizable(!fullScreen)
                .SetChromeless(fullScreen)
                .SetContextMenuEnabled(!fullScreen || Debug)
                .SetDevToolsEnabled(!fullScreen || Debug)
                .SetZoom(zoom)
                .RegisterWindowCreatedHandler(WindowCreated)
                .RegisterLocationChangedHandler(WindowLocationChanged)
                .RegisterSizeChangedHandler(WindowSizeChanged)
                .SetLogVerbosity(2)

                .Load(address);

            mainWindow.WaitForClose(); // Starts the application event loop

        }

        private static void WindowCreated(object sender, EventArgs e)
        {
            var window = sender as PhotinoWindow;


            //Log.Information("WindowCreated Callback Fired.");

            var currentState = window.Chromeless;

            if (currentState)
            {

                var monitor = CommandTools.MonitorFromWindow(window.WindowHandle, CommandTools.MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    var monitorInfo = new CommandTools.NativeMonitorInfo();
                    CommandTools.GetMonitorInfo(monitor, monitorInfo);

                    window.SetLeft(monitorInfo.Monitor.Left - Common.FullScreenBorder);
                    window.SetTop(monitorInfo.Monitor.Top - Common.FullScreenBorder);
                    window.SetWidth(monitorInfo.Monitor.Right - monitorInfo.Monitor.Left + (Common.FullScreenBorder *2));
                    window.SetHeight(monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top + (Common.FullScreenBorder *2));
                }
            }

        }

        private static void WindowLocationChanged(object sender, Point location)
        {
            var window = sender as PhotinoWindow;

            var currentstate = window.Chromeless;

            if ((currentstate || !Common.Minimized) && location.Y > 0 && location.X > 0)
            {
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Top", location.Y);
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Left", location.X);
            }

            //Log.Information($"WindowLocationChanged Callback Fired.  Left: {location.X}  Top: {location.Y}");
        }

        private static void WindowSizeChanged(object sender, Size size)
        {
            var window = sender as PhotinoWindow;

            var currentstate = window.Chromeless;

            if (currentstate || !Common.Minimized && size.Height > 300 && size.Width > 600)
            {
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Height", size.Height);
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Width", size.Width);
            }

            //Log.Information( $"WindowSizeChanged Callback Fired.  Height: {size.Height}  Width: {size.Width}");
        }

    }

}
