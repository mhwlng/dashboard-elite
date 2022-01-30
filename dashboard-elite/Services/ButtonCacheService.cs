using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace dashboard_elite.Services
{
    public enum ButtonType
    {
        ToggleButton,
        AlarmButton,
        FSSButton,
        HyperspaceButton,
        LimpetButton,
        FireGroupButton,
        PowerButton,
        StaticButton,
        RepeatingStaticButton
    }

    public class ButtonData
    {
        public string Function { get; set; }
        public ButtonType ButtonType { get; set; } // ToggleButton    // AlarmButton  // FSSButton // Hyperspace   // Limpet     // FireGroup
        public string PrimaryIcon { get; set; }   // off             // normal        // off       // off         // normal      // off
        public string SecondaryIcon { get; set; } // on              // alarm         // engaged   // engaged     // disabled   // on
        public string TertiaryIcon { get; set; }  //                 //               // disabled  // disabled    //            // disabled
        public string ClickSound { get; set; }
        public string ErrorSound { get; set; }
        public bool DontSwitchToCombatMode { get; set; } // hyperspace
        public string Firegroup { get; set; } // limpet  firegroup
        public string Fire { get; set; } // limpet

    }

    public class ButtonPageData
    {
        public List<ButtonData> Buttons { get; set; }
    }


    public class ButtonCacheService
    {
        private readonly IWebHostEnvironment _env;

        private Dictionary<string, List<ButtonData>> ButtonCache { get; set; } = new Dictionary<string, List<ButtonData>>();

        public ButtonCacheService(IWebHostEnvironment env)
        {
            _env = env;

            var buttonPath = Path.Combine(dashboard_elite.Program.ExePath, "Data\\ButtonBlocks\\");

            var fileEntries = Directory.GetFiles(buttonPath);
            foreach (var fileName in fileEntries)
            {
                var key = Path.GetFileNameWithoutExtension(fileName).ToLower();

                ButtonCache[key] = JsonConvert
                    .DeserializeObject<ButtonPageData>(File.ReadAllText(fileName))
                    ?.Buttons;
            }

        }

        public bool ButtonPageExists(string key)
        {
            return ButtonCache.ContainsKey(key);
        }


        public List<ButtonData> ButtonPage(string key)
        {
            return ButtonCache.ContainsKey(key) ? ButtonCache[key] : null;
        }

    }
}
