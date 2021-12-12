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
        private static HttpClientHandler httpClientHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
        public static readonly HttpClient WebClient = new HttpClient(httpClientHandler);

        public static readonly object RefreshJsonLock = new object();
        public static readonly object RefreshSystemLock = new object();

        public static string ExePath;
        public static string WebRootPath;

        public static bool Minimized;
        public static int FullScreenBorder;

        public static IConfigurationRoot Configuration;

        private static CachedSound _clickSound = null;

        public static Dictionary<BindingType, UserBindings> Binding = new Dictionary<BindingType, UserBindings>();

        public static void PlayClickSound()
        {
            if (_clickSound != null)
            {
                try
                {
                    AudioPlaybackEngine.Instance.PlaySound(_clickSound);
                }
                catch (Exception ex)
                {
                    Log.Error($"PlaySound: {ex}");
                }
            }
        }

        public static void PlaySound(ref CachedSound clickSound, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && clickSound == null)
            {
                var path = Path.Combine(dashboard_elite.Program.ExePath, "Sounds", fileName);

                if (File.Exists(path))
                {
                    try
                    {
                        clickSound = new CachedSound(path);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex, $"CachedSound: {fileName}");
                        clickSound = null;
                    }
                }
            }

            if (clickSound != null)
            {
                try
                {
                    AudioPlaybackEngine.Instance.PlaySound(clickSound);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, $"PlaySound: {fileName}");
                }
            }
        }

        public static PhotinoWindow mainWindow;

        [STAThread]
        public static int Main(string[] args)
        {
            return MainImpl(args).Result;
        }

        private static string GetExePath()
        {
            var strExeFilePath = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName(strExeFilePath);
        }


        public static async Task<int> MainImpl(string[] args)
        {
            ExePath = GetExePath();

            Directory.SetCurrentDirectory(ExePath);

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();


            Log.Information("Starting");

            _clickSound = null;

            var soundpath = Configuration.GetValue<string>("ClickSound");

            var fileName = Path.Combine(ExePath, "Sounds", soundpath);

            if (File.Exists(fileName))
            {
                try
                {
                    _clickSound = new CachedSound(fileName);
                }
                catch (Exception ex)
                {
                    _clickSound = null;

                    Log.Error($"CachedSound: {ex}");
                }

            }

            var host = CreateHostBuilder(args, Configuration).Build();

            var applicationLifetime =
                host.Services.GetService(typeof(IHostApplicationLifetime)) as IHostApplicationLifetime;

            //[Special for DesktopLoveBlazorWeb]
            TaskCompletionSource<string> futureAddr = new TaskCompletionSource<string>();
            applicationLifetime?.ApplicationStarted.Register((futureAddrObj) =>
            {
                var server = host.Services.GetService(typeof(IServer)) as IServer;
                var logger = host.Services.GetService(typeof(ILogger<Program>)) as ILogger<Program>;

                var addressFeature = server.Features.Get<IServerAddressesFeature>();
                foreach (var addresses in addressFeature.Addresses)
                {
                    logger.LogInformation("Listening on address: " + addresses);
                }

                var addr = addressFeature.Addresses.First();
                (futureAddrObj as TaskCompletionSource<string>).SetResult(addr);
            }, futureAddr);

            //[Special for DesktopLoveBlazorWeb]
#pragma warning disable CS4014
            host.RunAsync();
#pragma warning restore CS4014

            //[Special for DesktopLoveBlazorWeb]
            OpenInLine(await futureAddr.Task, Configuration);

            //TODO
            return 0;
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configuration) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Loopback, 0);

                        var externalPort = Configuration.GetValue<int>("ExternalPort");

                        if (externalPort > 0)
                        {
                            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                            {
                                if (!item.Description.Contains("virtual", StringComparison.CurrentCultureIgnoreCase) &&
                                    item.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                    item.OperationalStatus == OperationalStatus.Up)
                                {
                                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                                    {
                                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                        {
                                            serverOptions.Listen(ip.Address, externalPort);
                                        }
                                    }
                                }
                            }
                        }

                    });
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                });

        public static void OpenInLine(string address, IConfigurationRoot configuration)
        {
            string windowTitle = "Elite Dangerous Dashboard";
            var iconFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "wwwroot/img/elite.ico"
                : "wwwroot/img/elite.png";


            var top = configuration.GetValue<int>("Dimensions:Top");
            var left = configuration.GetValue<int>("Dimensions:left");
            var height = configuration.GetValue<int>("Dimensions:Height");
            var width = configuration.GetValue<int>("Dimensions:Width");
            var fullScreen = configuration.GetValue<bool>("Dimensions:FullScreen");
            var zoom = configuration.GetValue<int>("Dimensions:Zoom");

            FullScreenBorder = configuration.GetValue<int>("Dimensions:FullScreenBorder");


            Minimized = configuration.GetValue<bool>("Dimensions:Minimized");

            mainWindow = new PhotinoWindow()

                .SetIconFile(iconFile)

                .SetTitle(windowTitle)
                .SetUseOsDefaultSize(false)
                .SetUseOsDefaultLocation(false)
                .SetSize(new Size(width, height))
                .SetLeft(left)
                .SetTop(top)
                .SetResizable(!fullScreen)
                //.SetMaximized(fullScreen)
                //.SetFullScreen(fullScreen)
                //.SetTopMost(fullScreen)
                .SetChromeless(fullScreen)
                .SetContextMenuEnabled(!fullScreen)
                .SetDevToolsEnabled(!fullScreen)
                .SetZoom(zoom)
                //.RegisterCustomSchemeHandler("appscript", AppCustomSchemeUsed)
                /*
                .RegisterCustomSchemeHandler("app",
                    (object sender, string scheme, string url, out string contentType) =>
                    {
                        contentType = "text/javascript";
                        return new MemoryStream(Encoding.UTF8.GetBytes(@"
                        (() =>{
                            window.setTimeout(() => {
                                alert(`YYY Dynamically inserted JavaScript.`);
                            }, 5000);
                        })();
                    "));
                    })*/
                .RegisterWindowCreatingHandler(WindowCreating)
                .RegisterWindowCreatedHandler(WindowCreated)
                .RegisterLocationChangedHandler(WindowLocationChanged)
                .RegisterSizeChangedHandler(WindowSizeChanged)
                .RegisterWebMessageReceivedHandler(MessageReceivedFromWindow)
                .RegisterWindowClosingHandler(WindowIsClosing)

                //.SetTemporaryFilesPath(@"C:\Temp")

                .SetLogVerbosity(2)

                .Load(address);

            mainWindow.WaitForClose(); // Starts the application event loop

        }

        //These are the event handlers I'm hooking up
        /*
        private static Stream AppCustomSchemeUsed(object sender, string scheme, string url, out string contentType)
        {
            Log.Information($"Custom scheme '{scheme}' was used.");
            var currentWindow = sender as PhotinoWindow;

            contentType = "text/javascript";

            var js =
                @"
(() =>{
    window.setTimeout(() => {
        const title = document.getElementById('Title');
        const lineage = document.getElementById('Lineage');
        title.innerHTML = "

                + $"'{currentWindow.Title}';" + "\n"

                + $"        lineage.innerHTML = `PhotinoWindow Id: {currentWindow.Id} <br>`;" + "\n";

            //show lineage of this window
            var p = currentWindow.Parent;
            while (p != null)
            {
                js += $"        lineage.innerHTML += `Parent Id: {p.Id} <br>`;" + "\n";
                p = p.Parent;
            }

            js +=
                @"        alert(`XXX Dynamically inserted JavaScript.`);
    }, 1000);
})();
";
            return new MemoryStream(Encoding.UTF8.GetBytes(js));
        }*/

        private static void MessageReceivedFromWindow(object sender, string message)
        {
            //Log.Information($"MessageRecievedFromWindow Callback Fired.");
        }

        private static void WindowCreating(object sender, EventArgs e)
        {
            //Log.Information("WindowCreating Callback Fired.");
        }

        private static void WindowCreated(object sender, EventArgs e)
        {
            //Log.Information("WindowCreated Callback Fired.");

            var currentState = Program.mainWindow.Chromeless;

            if (currentState)
            {

                var monitor = CommandTools.MonitorFromWindow(mainWindow.WindowHandle, CommandTools.MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    var monitorInfo = new CommandTools.NativeMonitorInfo();
                    CommandTools.GetMonitorInfo(monitor, monitorInfo);

                    mainWindow.SetLeft(monitorInfo.Monitor.Left - FullScreenBorder);
                    mainWindow.SetTop(monitorInfo.Monitor.Top - FullScreenBorder);
                    mainWindow.SetWidth(monitorInfo.Monitor.Right - monitorInfo.Monitor.Left + (FullScreenBorder*2));
                    mainWindow.SetHeight(monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top + (FullScreenBorder*2));
                }
            }

        }

        private static void WindowLocationChanged(object sender, Point location)
        {
            var currentstate = Program.mainWindow.Chromeless;

            if ((currentstate || !Minimized) && location.Y > 0 && location.X > 0)
            {
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Top", location.Y);
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Left", location.X);
            }

            //Log.Information($"WindowLocationChanged Callback Fired.  Left: {location.X}  Top: {location.Y}");
        }

        private static void WindowSizeChanged(object sender, Size size)
        {
            var currentstate = Program.mainWindow.Chromeless;

            if (currentstate || !Minimized && size.Height > 300 && size.Width > 600)
            {
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Height", size.Height);
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Width", size.Width);
            }

            //Log.Information( $"WindowSizeChanged Callback Fired.  Height: {size.Height}  Width: {size.Width}");
        }

        private static bool WindowIsClosing(object sender, EventArgs e)
        {
            //Log.Information("WindowIsClosing Callback Fired.");
            return false;   //return true to block closing of the window
        }
    }

}
