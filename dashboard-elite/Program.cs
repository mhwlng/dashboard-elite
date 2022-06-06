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
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using System.Net.Http;

namespace dashboard_elite
{

    public class Program
    {

        [STAThread]
        public static int Main(string[] args)
        {
            Common.Startup(args);

            var app = new App();
            app.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
            app.Run();

            return 0;
        }

    }

}
