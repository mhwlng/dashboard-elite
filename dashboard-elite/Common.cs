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
    public class Common
    {
        private static HttpClientHandler httpClientHandler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };
        public static readonly HttpClient WebClient = new HttpClient(httpClientHandler);

        public static readonly object RefreshJsonLock = new object();
        public static readonly object RefreshSystemLock = new object();

        public static string ExePath;
        public static string WebRootPath;

        public static bool Minimized;
        public static int FullScreenBorder;

        public static IConfigurationRoot ConfigurationRoot;

        public static string Address;

        public static CachedSound _clickSound = null;

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
                var path = Path.Combine(ExePath, "Sounds", fileName);

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

        public static string GetExePath()
        {
            var strExeFilePath = Assembly.GetEntryAssembly().Location;
            return Path.GetDirectoryName(strExeFilePath);
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfigurationRoot configurationRoot) =>

            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Loopback, 0);

                        var externalPort = configurationRoot.GetValue<int>("ExternalPort");

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

        public static void Startup(string[] args)
        {
            WebClient.Timeout = new TimeSpan(0, 0, 5, 0, 0);

            ExePath = GetExePath();

            Directory.SetCurrentDirectory(ExePath);

            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(ConfigurationRoot)
                .CreateLogger();
            
            Log.Information("Starting");

            _clickSound = null;

            var soundpath = ConfigurationRoot.GetValue<string>("ClickSound");

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

            var host = Common.CreateHostBuilder(args, Common.ConfigurationRoot).Build();

            var applicationLifetime =
                host.Services.GetService(typeof(IHostApplicationLifetime)) as IHostApplicationLifetime;

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

#pragma warning disable CS4014
            host.RunAsync();
#pragma warning restore CS4014

            Address = futureAddr.Task.GetAwaiter().GetResult();


        }
    }
}
