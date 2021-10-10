using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using dashboard_elite;
using dashboard_elite.EliteData;
using dashboard_elite.JsInterop;
using Serilog;
using WindowsInput;
using WindowsInput.Native;

namespace Elite
{
    internal static class CommandTools
    {
        public static FifoExecution KeyboardJob = new FifoExecution();



        internal const char MACRO_START_CHAR = '{';
        internal const string MACRO_END = "}}";
        internal const string REGEX_MACRO = @"^\{(\{[^\{\}]+\})+\}$";
        internal const string REGEX_SUB_COMMAND = @"(\{[^\{\}]+\})";


        public const Int32 MONITOR_DEFAULTTOPRIMERTY = 0x00000001;
        public const Int32 MONITOR_DEFAULTTONEAREST = 0x00000002;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, Int32 flags);


        [DllImport("user32.dll")]
        public static extern Boolean GetMonitorInfo(IntPtr hMonitor, NativeMonitorInfo lpmi);


        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct NativeRectangle
        {
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;


            public NativeRectangle(Int32 left, Int32 top, Int32 right, Int32 bottom)
            {
                this.Left = left;
                this.Top = top;
                this.Right = right;
                this.Bottom = bottom;
            }
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class NativeMonitorInfo
        {
            public Int32 Size = Marshal.SizeOf(typeof(NativeMonitorInfo));
            public NativeRectangle Monitor;
            public NativeRectangle Work;
            public Int32 Flags;
        }

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetKeyboardLayout(int WindowsThreadProcessID);

        private static Dictionary<string, string> _lastStatus = new Dictionary<string, string>();

        internal static bool InputRunning;
        internal static bool ForceStop = false;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam,
            int fuFlags,
            int uTimeout,
            IntPtr lpdwResult
        );


        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // When you don't want the ProcessId, use this overload and pass 
        // IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd,
            IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach,
            uint idAttachTo, bool fAttach);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(HandleRef hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        /*
        public static string test()
        {
            int lpdwProcessId;
            IntPtr hWnd = GetForegroundWindow();
            int WinThreadProcId = GetWindowThreadProcessId(hWnd, out lpdwProcessId);

        }*/

        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private enum ShowWindowEnum
        {
            Hide = 0,
            ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
            Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
            Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
            Restore = 9, ShowDefault = 10, ForceMinimized = 11
        };

        public static void BringMainWindowToFront(string processName)
        {
            try
            {

                // get the process
                Process bProcess = Process.GetProcessesByName(processName).FirstOrDefault();

                // check if the process is running
                if (bProcess != null)
                {
                    // check if the window is hidden / minimized
                    //if (bProcess.MainWindowHandle == IntPtr.Zero)
                    //{
                    // the window is hidden so try to restore it before setting focus.
                    //ShowWindow(bProcess.Handle, ShowWindowEnum.Restore);
                    //}

                    /*
                    uint foreThread = GetWindowThreadProcessId(GetForegroundWindow(),
                        IntPtr.Zero);
                    uint appThread = GetCurrentThreadId();
                    const uint SW_SHOW = 5;

                    if (foreThread != appThread)
                    {
                        AttachThreadInput(foreThread, appThread, true);
                        BringWindowToTop(bProcess.MainWindowHandle);
                        ShowWindow(bProcess.MainWindowHandle, SW_SHOW);
                        AttachThreadInput(foreThread, appThread, false);
                    }
                    else
                    {
                        BringWindowToTop(bProcess.MainWindowHandle);
                        ShowWindow(bProcess.MainWindowHandle, SW_SHOW);
                    }*/


                    //ShowWindow(bProcess.MainWindowHandle, ShowWindowEnum.Restore);

                    //var activeWindowHandle = GetForegroundWindow();
                    //if (activeWindowHandle != bProcess.MainWindowHandle)
                    //{
                        // set user the focus to the window
                        SetForegroundWindow(bProcess.MainWindowHandle);
                    //}

                    /*
                    SendMessageTimeout(bProcess.MainWindowHandle, 0, IntPtr.Zero, IntPtr.Zero, 0, 5000, IntPtr.Zero);

                    var activeWindowHandle = GetForegroundWindow();
                    while ((!activeWindowHandle.Equals(bProcess.MainWindowHandle)))
                    {
                        if (!activeWindowHandle.Equals(bProcess.MainWindowHandle))
                        {
                            activeWindowHandle = GetForegroundWindow();
                        }
                    }*/

                }
                //else
                //{
                    // the process is not running, so start it
                    //Process.Start(processName);
                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex,$"BringMainWindowToFront");
            }

        }

        internal static string ExtractMacro(string text, int position)
        {
            try
            {
                var endPosition = text.IndexOf(MACRO_END, position);

                // Found an end, let's verify it's actually a macro
                if (endPosition > position)
                {
                    // Use Regex to verify it's really a macro
                    var match = Regex.Match(text.Substring(position, endPosition - position + MACRO_END.Length), REGEX_MACRO);
                    if (match.Length > 0)
                    {
                        return match.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error( $"ExtractMacro Exception: {ex}");
            }

            return null;
        }

        internal static List<DirectInputKeyCode> ExtractKeyStrokes(string macroText)
        {
            var keyStrokes = new List<DirectInputKeyCode>();


            try
            {
                var matches = Regex.Matches(macroText, REGEX_SUB_COMMAND);
                foreach (var match in matches)
                {
                    var matchText = match.ToString().ToUpperInvariant().Replace("{", "").Replace("}", "");

                    if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "en-US")
                    {
                        // http://kbdlayout.info/kbdusx/shiftstates+scancodes/base

                        // FIRST ROW  DIKGRAVE          DIKMINUS        DIKEQUALS
                        // SECOND ROW DIKLEFTBRACKET    DIKRIGHTBRACKET DIKBACKSLASH
                        // THIRD ROW  DIKSEMICOLON      DIKAPOSTROPHE
                        // FOURTH ROW DIKCOMMA          DIKPERIOD       DIKSLASH
                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "es-ES")
                    {
                        // http://kbdlayout.info/kbdsp/shiftstates+scancodes/base

                        // FIRST ROW
                        // SECOND ROW 
                        // THIRD ROW
                        // FOURTH ROW

                        // all the keys are the same as en-US in binding file , for some reason ????
                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "en-GB")
                    {
                        // http://kbdlayout.info/kbduk/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // second row
                            case "DIKHASH":
                                matchText = "DIKBACKSLASH";
                                break;
                        }
                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "fr-FR")
                    {
                        // http://kbdlayout.info/kbdfr/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // FIRST ROW
                            case "DIKSUPERSCRIPTTWO":
                                matchText = "DIKGRAVE";
                                break;
                            case "DIKAMPERSAND":
                                matchText = "DIK1";
                                break;
                            case "DIKÉ":
                                matchText = "DIK2";
                                break;
                            case "DIKDOUBLEQUOTE":
                                matchText = "DIK3";
                                break;
                            case "DIKAPOSTROPHE":
                                matchText = "DIK4";
                                break;
                            case "DIKLEFTPARENTHESIS":
                                matchText = "DIK5";
                                break;
                            case "DIKMINUS":
                                matchText = "DIK6";
                                break;
                            case "DIKÈ":
                                matchText = "DIK7";
                                break;
                            case "DIKUNDERLINE":
                                matchText = "DIK8";
                                break;
                            case "DIKÇ":
                                matchText = "DIK9";
                                break;
                            case "DIKÀ":
                                matchText = "DIK0";
                                break;
                            case "DIKRIGHTPARENTHESIS":
                                matchText = "DIKMINUS";
                                break;

                            // SECOND ROW
                            case "DIKA":
                                matchText = "DIKQ";
                                break;
                            case "DIKZ":
                                matchText = "DIKW";
                                break;
                            case "DIKCIRCUMFLEX":
                                matchText = "DIKLEFTBRACKET";
                                break;
                            case "DIKDOLLAR":
                                matchText = "DIKRIGHTBRACKET";
                                break;
                            case "DIKASTERISK":
                                matchText = "DIKBACKSLASH";
                                break;

                            // THIRD ROW

                            case "DIKQ":
                                matchText = "DIKA";
                                break;

                            case "DIKM":
                                matchText = "DIKSEMICOLON";
                                break;
                            case "DIKÙ":
                                matchText = "DIKAPOSTROPHE";
                                break;

                            // FOURTH ROW
                            case "DIKW":
                                matchText = "DIKZ";
                                break;
                            case "DIKCOMMA":
                                matchText = "DIKM";
                                break;
                            case "DIKSEMICOLON":
                                matchText = "DIKCOMMA";
                                break;
                            case "DIKCOLON":
                                matchText = "DIKPERIOD";
                                break;
                            case "DIKEXCLAMATIONPOINT":
                                matchText = "DIKSLASH";
                                break;
                        }

                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "de-DE")
                    {
                        // http://kbdlayout.info/kbdgr/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // FIRST ROW
                            case "DIKCIRCUMFLEX":
                                matchText = "DIKGRAVE";
                                break;
                            case "DIKß":
                                matchText = "DIKMINUS";
                                break;
                            case "DIKACUTE":
                                matchText = "DIKEQUALS";
                                break;

                            // SECOND ROW 
                            case "DIKZ":
                                matchText = "DIKY";
                                break;
                            case "DIKÜ":
                                matchText = "DIKLEFTBRACKET";
                                break;
                            case "DIKPLUS":
                                matchText = "DIKRIGHTBRACKET";
                                break;
                            case "DIKHASH":
                                matchText = "DIKBACKSLASH";
                                break;

                            // THIRD ROW
                            case "DIKÖ":
                                matchText = "DIKSEMICOLON";
                                break;
                            case "DIKÄ":
                                matchText = "DIKAPOSTROPHE";
                                break;

                            // FOURTH ROW
                            case "DIKY":
                                matchText = "DIKZ";
                                break;
                            case "DIKMINUS":
                                matchText = "DIKSLASH";
                                break;
                        }

                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "de-CH")
                    {
                        // http://kbdlayout.info/kbdsg/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // FIRST ROW
                            case "DIK§":
                                matchText = "DIKGRAVE";
                                break;
                            case "DIKAPOSTROPHE":
                                matchText = "DIKMINUS";
                                break;
                            case "DIKCIRCUMFLEX":
                                matchText = "DIKEQUALS";
                                break;

                            // SECOND ROW 
                            case "DIKZ":
                                matchText = "DIKY";
                                break;
                            case "DIKÜ":
                                matchText = "DIKLEFTBRACKET";
                                break;
                            case "DIKUMLAUT":
                                matchText = "DIKRIGHTBRACKET";
                                break;
                            case "DIKDOLLAR":
                                matchText = "DIKBACKSLASH";
                                break;

                            // THIRD ROW
                            case "DIKÖ":
                                matchText = "DIKSEMICOLON";
                                break;
                            case "DIKÄ":
                                matchText = "DIKAPOSTROPHE";
                                break;

                            // FOURTH ROW
                            case "DIKY":
                                matchText = "DIKZ";
                                break;
                            case "DIKMINUS":
                                matchText = "DIKSLASH";
                                break;

                        }

                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "da-DK")
                    {
                        // http://kbdlayout.info/kbdda/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // FIRST ROW
                            case "DIKHALF":
                                matchText = "DIKGRAVE";
                                break;
                            case "DIKPLUS":
                                matchText = "DIKMINUS";
                                break;
                            case "DIKACUTE":
                                matchText = "DIKEQUALS";
                                break;

                            // SECOND ROW 
                            case "DIKÅ":
                                matchText = "DIKLEFTBRACKET";
                                break;
                            case "DIKUMLAUT":
                                matchText = "DIKRIGHTBRACKET";
                                break;
                            case "DIKAPOSTROPHE":
                                matchText = "DIKBACKSLASH";
                                break;

                            // THIRD ROW
                            case "DIKÆ":
                                matchText = "DIKSEMICOLON";
                                break;
                            case "DIKØ":
                                matchText = "DIKAPOSTROPHE";
                                break;

                            // FOURTH ROW
                            case "DIKMINUS":
                                matchText = "DIKSLASH";
                                break;
                        }

                    }
                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "it-IT")
                    {
                        // http://kbdlayout.info/kbdit/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // FIRST ROW
                            case "DIKBACKSLASH":
                                matchText = "DIKGRAVE";
                                break;
                            case "DIKAPOSTROPHE":
                                matchText = "DIKMINUS";
                                break;
                            case "DIKÌ":
                                matchText = "DIKEQUALS";
                                break;

                            // SECOND ROW 
                            case "DIKÈ":
                                matchText = "DIKLEFTBRACKET";
                                break;
                            case "DIKPLUS":
                                matchText = "DIKRIGHTBRACKET";
                                break;
                            case "DIKÙ":
                                matchText = "DIKBACKSLASH";
                                break;

                            // THIRD ROW
                            case "DIKÒ":
                                matchText = "DIKSEMICOLON";
                                break;
                            case "DIKÀ":
                                matchText = "DIKAPOSTROPHE";
                                break;

                            // FOURTH ROW
                            case "DIKMINUS":
                                matchText = "DIKSLASH";
                                break;
                        }

                    }

                    else if (Program.Binding[BindingType.OnFoot].KeyboardLayout == "pt-PT")
                    {
                        // http://kbdlayout.info/kbdpo/shiftstates+scancodes/base

                        switch (matchText)
                        {
                            // FIRST ROW
                            case "DIKBACKSLASH":
                                matchText = "DIKGRAVE";
                                break;
                            case "DIKAPOSTROPHE":
                                matchText = "DIKMINUS";
                                break;
                            case "DIK«":
                                matchText = "DIKEQUALS";
                                break;

                            // SECOND ROW 
                            case "DIKPLUS":
                                matchText = "DIKLEFTBRACKET";
                                break;
                            case "DIKACUTE":
                                matchText = "DIKRIGHTBRACKET";
                                break;
                            case "DIKTILDE":
                                matchText = "DIKBACKSLASH";
                                break;

                            // THIRD ROW
                            case "DIKÇ":
                                matchText = "DIKSEMICOLON";
                                break;
                            case "DIKº":
                                matchText = "DIKAPOSTROPHE";
                                break;

                            // FOURTH ROW
                            case "DIKMINUS":
                                matchText = "DIKSLASH";
                                break;
                        }

                    }


                    var stroke = (DirectInputKeyCode)Enum.Parse(typeof(DirectInputKeyCode), matchText, true);

                    keyStrokes.Add(stroke);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error($"ExtractKeyStrokes Exception: {ex}");
            }

            return keyStrokes;
        }


        public static DirectInputKeyCode ConvertLocaleScanCode(DirectInputKeyCode scanCode)
        {
            //german

            // http://kbdlayout.info/KBDGR/shiftstates+scancodes/base

            // french
            // http://kbdlayout.info/kbdfr/shiftstates+scancodes/base

            // usa
            // http://kbdlayout.info/kbdusx/shiftstates+scancodes/base

            if (Program.Binding[BindingType.OnFoot].KeyboardLayout != "en-US")
            {
                Log.Logger.Information(scanCode.ToString() + " " + ((ushort)scanCode).ToString("X"));

                int lpdwProcessId;
                IntPtr hWnd = GetForegroundWindow();
                int WinThreadProcId = GetWindowThreadProcessId(hWnd, out lpdwProcessId);
                var hkl = GetKeyboardLayout(WinThreadProcId);

                Log.Logger.Information(((long)hkl).ToString("X"));

                //hkl = (IntPtr)67568647; // de-DE 4070407

                // Maps the virtual scanCode to key code for the current locale
                var virtualKeyCode = MapVirtualKeyEx((ushort)scanCode, 3, hkl);

                if (virtualKeyCode > 0)
                {
                    // map key code back to en-US scan code :

                    hkl = (IntPtr)67699721; // en-US 4090409

                    var virtualScanCode = MapVirtualKeyEx((ushort)virtualKeyCode, 4, hkl);

                    if (virtualScanCode > 0)
                    {
                        Log.Logger.Information(
                            "keycode " + virtualKeyCode.ToString("X") + " scancode " + virtualScanCode.ToString("X") +
                            " keyboard code " + hkl.ToString("X"));

                        return (DirectInputKeyCode)(virtualScanCode & 0xff); // only use low byte
                    }
                }
            }

            return scanCode;
        }

        private static async void SendInput(string inputText)
        {
            InputRunning = true;
            await Task.Run(() =>
            {
                var text = inputText;

                for (var idx = 0; idx < text.Length && !ForceStop; idx++)
                {
                    var macro = CommandTools.ExtractMacro(text, idx);
                    idx += macro.Length - 1;
                    macro = macro.Substring(1, macro.Length - 2);

                    HandleMacro(macro);
                }
            });
            InputRunning = false;
        }


        private static void SendInputDown(string inputText)
        {
            var text = inputText;

            for (var idx = 0; idx < text.Length && !ForceStop; idx++)
            {
                var macro = CommandTools.ExtractMacro(text, idx);
                idx += macro.Length - 1;
                macro = macro.Substring(1, macro.Length - 2);

                HandleMacroDown(macro);
            }
        }

        private static void SendInputUp(string inputText)
        {
            var text = inputText;

            for (var idx = 0; idx < text.Length && !ForceStop; idx++)
            {
                var macro = CommandTools.ExtractMacro(text, idx);
                idx += macro.Length - 1;
                macro = macro.Substring(1, macro.Length - 2);

                HandleMacroUp(macro);
            }
        }

        private static void HandleMacro(string macro)
        {
            var keyStrokes = CommandTools.ExtractKeyStrokes(macro);

            // Actually initiate the keystrokes
            if (keyStrokes.Count > 0)
            {
                var iis = new InputSimulator();
                var keyCode = keyStrokes.Last();
                keyStrokes.Remove(keyCode);

                if (keyStrokes.Count > 0)
                {
                    //iis.Keyboard.ModifiedKeyStroke(keyStrokes.Select(ks => ks).ToArray(), keyCode);

                    iis.Keyboard.DelayedModifiedKeyStroke(keyStrokes.Select(ks => ks), keyCode, 40);

                }
                else // Single Keycode
                {
                    //iis.Keyboard.KeyPress(keyCode);

                    iis.Keyboard.DelayedKeyPress(keyCode, 40);
                }
            }
        }

        private static void HandleMacroDown(string macro)
        {
            var keyStrokes = CommandTools.ExtractKeyStrokes(macro);

            // Actually initiate the keystrokes
            if (keyStrokes.Count > 0)
            {
                var iis = new InputSimulator();
                var keyCode = keyStrokes.Last();
                keyStrokes.Remove(keyCode);

                if (keyStrokes.Count > 0)
                {
                    iis.Keyboard.DelayedModifiedKeyStrokeDown(keyStrokes.Select(ks => ks), keyCode, 40);

                }
                else // Single Keycode
                {
                    iis.Keyboard.DelayedKeyPressDown(keyCode, 40);
                }
            }
        }


        private static void HandleMacroUp(string macro)
        {
            var keyStrokes = CommandTools.ExtractKeyStrokes(macro);

            // Actually initiate the keystrokes
            if (keyStrokes.Count > 0)
            {
                var iis = new InputSimulator();
                var keyCode = keyStrokes.Last();
                keyStrokes.Remove(keyCode);

                if (keyStrokes.Count > 0)
                {
                    iis.Keyboard.DelayedModifiedKeyStrokeUp(keyStrokes.Select(ks => ks), keyCode, 40);

                }
                else // Single Keycode
                {
                    iis.Keyboard.DelayedKeyPressUp(keyCode, 40);
                }
            }
        }

        private static string BuildInputText(StandardBindingInfo keyInfo)
        {
            var inputText = "";

            if (keyInfo.Primary.Device == "Keyboard")
            {
                inputText =
                    "{" + keyInfo.Primary.Key.Replace("Key_", "DIK") + "}";
                foreach (var m in keyInfo.Primary.Modifier)
                {
                    if (m.Device == "Keyboard")
                    {
                        inputText =
                            "{" + m.Key.Replace("Key_", "DIK") +
                            "}" + inputText;
                    }
                }

            }
            else if (keyInfo.Secondary.Device == "Keyboard")
            {
                inputText =
                    "{" + keyInfo.Secondary.Key.Replace("Key_", "DIK") + "}";
                foreach (var m in keyInfo.Secondary.Modifier)
                {
                    if (m.Device == "Keyboard")
                    {
                        inputText =
                            "{" + m.Key.Replace("Key_", "DIK") +
                            "}" + inputText;
                    }
                }
            }

            if (!string.IsNullOrEmpty(inputText))
            {
                inputText = inputText.Replace("_", "")

                    .Replace("Subtract", "MINUS") //DIKNumpadSubtract   -> DikNumpadMinus
                    .Replace("Add", "PLUS") //DIKNumpadAdd        -> DikNumpadPlus
                    .Replace("Divide", "SLASH") //DIKNumpadDivide     -> DikNumpadSlash
                    .Replace("Decimal", "PERIOD") //DIKNumpadDecimal    -> DikNumpadPeriod
                    .Replace("Multiply", "STAR") //DIKNumpadMultiply   -> DikNumpadStar
                    .Replace("DIKEnter", "DIKRETURN")  // don't affect DIKNumpadEnter
                    .Replace("Backspace", "BACK")
                    .Replace("UpArrow", "UP")
                    .Replace("DownArrow", "DOWN")
                    .Replace("LeftArrow", "LEFT")
                    .Replace("RightArrow", "RIGHT")
                    .Replace("LeftAlt", "LMENU")
                    .Replace("RightAlt", "RMENU")
                    .Replace("RightControl", "RCONTROL")
                    .Replace("LeftControl", "LCONTROL")
                    .Replace("RightShift", "RSHIFT")
                    .Replace("LeftShift", "LSHIFT");

                //Logger.Instance.LogMessage(TracingLevel.DEBUG, $"{inputText}");

            }

            return inputText;
        }

        private class KeyboardJobCallbackInfo
        {
            public StandardBindingInfo StandardBindingInfo { get; set; }
            public bool FocusChange { get; set; }
        }

        private static void KeyboardJobCallback(Object threadContext)
        {
            var keyInfo = (KeyboardJobCallbackInfo)threadContext;

            if (keyInfo.FocusChange)
            {
                InteropMouse.JsMouseUp();

                Thread.Sleep(200);

                InteropMouse.JsMouseUp();
            }

            SendKeypress(keyInfo.StandardBindingInfo);
        }

        internal static void SendKeypressQueue(StandardBindingInfo keyInfo, bool focusChange)
        {
            KeyboardJob.QueueUserWorkItem(KeyboardJobCallback, new KeyboardJobCallbackInfo
            {
                StandardBindingInfo = keyInfo,
                FocusChange = focusChange
            });

        }

        internal static void SendKeypress(StandardBindingInfo keyInfo, int repeatCount = 1)
        {
            var inputText = BuildInputText(keyInfo);

            if (!string.IsNullOrEmpty(inputText))
            {

                //Logger.Instance.LogMessage(TracingLevel.DEBUG, $"{inputText}");

                for (var i = 0; i < repeatCount; i++)
                {
                    if (repeatCount > 1 && i > 0)
                    {
                        Thread.Sleep(20);
                    }
                    SendInput("{" + inputText + "}");

                }

                // keyboard test page : https://w3c.github.io/uievents/tools/key-event-viewer.html
            }

        }

        internal static void SendKeypressDown(StandardBindingInfo keyInfo)
        {
            var inputText = BuildInputText(keyInfo);

            if (!string.IsNullOrEmpty(inputText))
            {
                SendInputDown("{" + inputText + "}");
            }
        }


        internal static void SendKeypressUp(StandardBindingInfo keyInfo)
        {
            var inputText = BuildInputText(keyInfo);

            if (!string.IsNullOrEmpty(inputText))
            {
                SendInputUp("{" + inputText + "}");
            }
        }

        internal static bool CheckForGif(string imageFilename)
        {
            return imageFilename?.ToLower().EndsWith(".gif") == true;
        }

        public static void AddOrUpdateAppSetting<T>(string key, T value)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "appSettings.json");
            string json = File.ReadAllText(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            var sectionPath = key.Split(":")[0];

            if (!string.IsNullOrEmpty(sectionPath))
            {
                var keyPath = key.Split(":")[1];
                jsonObj[sectionPath][keyPath] = value;
            }
            else
            {
                jsonObj[sectionPath] = value; // if no sectionpath just set the value
            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);
        }

    }
}
