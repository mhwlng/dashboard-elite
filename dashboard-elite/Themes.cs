using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;

namespace dashboard_elite
{
    public static class Themes
    {
        public static readonly MudTheme darkTheme = new MudTheme
        {
            Palette = new Palette()
            {
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "#FFFFFF", //"rgba(255,255,255, 0.50)",
                DrawerIcon = "#FFFFFF", //"rgba(255,255,255, 0.50)",
                AppbarBackground = "#27272f",
                AppbarText = "#FFFFFF", //"rgba(255,255,255, 0.70)",
                TextPrimary = "#FFFFFF", //"rgba(255,255,255, 0.70)",
                TextSecondary = "#FFB000", //"rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Divider = "rgba(255,255,255, 0.12)",
                DividerLight = "rgba(255,255,255, 0.06)",
                TableLines = "rgba(255,255,255, 0.12)",
                LinesDefault = "rgba(255,255,255, 0.12)",
                LinesInputs = "rgba(255,255,255, 0.3)",
                TextDisabled = "rgba(255,255,255, 0.2)",

                Primary = "#2196f3",           // link

                Secondary = "#767676",         // on
                SecondaryDarken = "#767676",

                Tertiary = "#282828",         // disabled
                TertiaryDarken = "#282828",
                TertiaryContrastText = "#a0a0a0",

                Dark = "#424242",            // normal button background
                DarkDarken = "#424242",

                Error = "#f44336",          // alarm
                ErrorDarken = "#f44336"

                //Success = "#00c853",
                //SuccessDarken = "#00c853",


            },
            /*
            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontFamily = new[] {  "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = ".875rem",
                    FontWeight = 400,
                    LineHeight = 1.43,
                    LetterSpacing = ".01071em"
                },
            }*/

            

        };

    }
}
