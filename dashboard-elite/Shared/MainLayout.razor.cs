using System.Diagnostics;
using Elite;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace dashboard_elite.Shared
{
    public partial class MainLayout
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        private MudTheme _currentTheme = Themes.darkTheme;

        bool _drawerOpen = false;

        void Close()
        {
            Program.mainWindow.Close();
        }

        void Maximize()
        {
            var currentstate = Program.mainWindow.Chromeless;

            Program.mainWindow.SetMaximized(!currentstate);

            CommandTools.AddOrUpdateAppSetting<bool>("Dimensions:FullScreen", !currentstate);

            Process.Start(Process.GetCurrentProcess().MainModule.FileName);

            Program.mainWindow.Close();
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }

        

    }
}
