using dashboard_elite.Helpers;
using Microsoft.JSInterop;

namespace dashboard_elite.JsInterop
{
    public static class InteropMouse
    {
        [JSInvokable]
        public static void JsMouseUp()
        {
            var currentstate = Program.mainWindow.Chromeless;

            if (currentstate)
            {
                CommandTools.BringMainWindowToFront("EliteDangerous64");
            }
        }

        [JSInvokable]
        public static void JsClick()
        {
            Program.PlayClickSound();

        }
    }
}
