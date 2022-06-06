using dashboard_elite.Helpers;
using Microsoft.JSInterop;
using System.Windows;

namespace dashboard_elite.JsInterop
{
    public static class InteropMouse
    {
        [JSInvokable]
        public static void JsMouseUp()
        {
            try
            {
                Common.MainWindow.Dispatcher.Invoke(() =>
                {

                    if (Common.MainWindow.WindowStyle == WindowStyle.None)
                    {
                        CommandTools.BringMainWindowToFront("EliteDangerous64");
                    }
                });
            }
            catch
            {
                ///
            }
        }

        [JSInvokable]
        public static void JsClick()
        {
            Common.PlayClickSound();

        }
    }
}
