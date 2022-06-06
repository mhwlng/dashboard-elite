using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dashboard_elite.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using System.Windows.Interop;
using Microsoft.Web.WebView2.Wpf;

namespace dashboard_elite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Common.MainWindow = this;

            var top = Common.ConfigurationRoot.GetValue<int>("Dimensions:Top");
            var left = Common.ConfigurationRoot.GetValue<int>("Dimensions:left");
            var height = Common.ConfigurationRoot.GetValue<int>("Dimensions:Height");
            var width = Common.ConfigurationRoot.GetValue<int>("Dimensions:Width");

            var zoom = Common.ConfigurationRoot.GetValue<int>("Dimensions:Zoom");

            var windows = Common.ConfigurationRoot.GetValue<int>("Windows");

            var fullScreen = Common.ConfigurationRoot.GetValue<bool>("Dimensions:FullScreen");

            Common.FullScreenBorder = Common.ConfigurationRoot.GetValue<int>("Dimensions:FullScreenBorder");

            Common.Minimized = Common.ConfigurationRoot.GetValue<bool>("Dimensions:Minimized");

            InitializeComponent();

            grd.RowDefinitions.Add(new RowDefinition());

            for (var i = 0; i < windows; i++)
            {
                grd.ColumnDefinitions.Add(new ColumnDefinition());
                var wv1 = new WebView2();
                wv1.Name = $"webView{i+1}";
                wv1.ZoomFactor = zoom / 100.0;
                wv1.Source = new Uri($"{Common.Address}#{i+1}-{windows}");
                RegisterName(wv1.Name, wv1);
                Grid.SetRow(wv1, i);
                Grid.SetColumn(wv1, i);
                grd.Children.Add(wv1);
            }

            InitializeAsync();

            this.Top = top;
            this.Left = left;
            this.Height = height;
            this.Width = width;

            if (fullScreen)
            {
                this.WindowStyle = WindowStyle.None; 
                this.ResizeMode = ResizeMode.NoResize;
            }
        }

        async void InitializeAsync()
        {
            var wv = this.grd.Children.OfType<WebView2>()?.ToList();
            if (wv?.Any() == true)
            {
                await wv[0].EnsureCoreWebView2Async(null);
            }
        }

        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var currentstate = (this.WindowStyle == WindowStyle.None);

            if (currentstate || !Common.Minimized && e.NewSize.Height > 300 && e.NewSize.Width > 600)
            {
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Height", (int)e.NewSize.Height);
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Width", (int)e.NewSize.Width);
            }

        }

        private void MainWindow_OnLocationChanged(object sender, EventArgs e)
        {
            var currentstate = (this.WindowStyle == WindowStyle.None);

            if ((currentstate || !Common.Minimized) &&  this.Top > 0 && this.Left > 0)
            {
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Top", (int)this.Top);
                CommandTools.AddOrUpdateAppSetting<int>("Dimensions:Left", (int)this.Left);
            }

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var currentState = (Common.MainWindow.WindowStyle == WindowStyle.None);

            if (currentState)
            {
                var monitor = CommandTools.MonitorFromWindow(new WindowInteropHelper(this).Handle, CommandTools.MONITOR_DEFAULTTONEAREST);

                if (monitor != IntPtr.Zero)
                {
                    var monitorInfo = new CommandTools.NativeMonitorInfo();
                    CommandTools.GetMonitorInfo(monitor, monitorInfo);

                    this.Left = monitorInfo.Monitor.Left - Common.FullScreenBorder;
                    this.Top = monitorInfo.Monitor.Top - Common.FullScreenBorder;
                    this.Width = monitorInfo.Monitor.Right - monitorInfo.Monitor.Left + (Common.FullScreenBorder * 2);
                    this.Height = monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top + (Common.FullScreenBorder * 2);
                }
            }
        }
    }
}