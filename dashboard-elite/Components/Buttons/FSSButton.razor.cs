using System;
using System.Threading;
using dashboard_elite.EliteData;
using dashboard_elite.JsInterop;
using dashboard_elite.Services;
using Elite;
using EliteJournalReader.Events;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace dashboard_elite.Components.Buttons
{
    public partial class FSSButton
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon); // off

        private string SecondaryIcon => SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon); // engaged

        private string TertiaryIcon => SvgCacheService.ButtonIcon(ButtonData.TertiaryIcon); // disabled

        private CachedSound _clickSound = null;
        private CachedSound _errorSound = null;

        private MudTripleIconButtonState FssState
        {
            get
            {
                var state = MudTripleIconButtonState.Secondary; // engaged

                if (!Data.StatusData.Supercruise)
                {
                    state = MudTripleIconButtonState.Tertiary; // disabled
                }
                else
                {
                    if (Data.StatusData.GuiFocus != StatusGuiFocus.FSSMode)
                    {
                        state = MudTripleIconButtonState.Primary; // off
                    }
                }

                return state;
            }
        }


        private void ButtonClick()
        {
            var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

            if (focusChange)
            {
                InteropMouse.JsMouseUp();

                Thread.Sleep(100);
            }

            if (Data.StatusData.Supercruise)
            {
                if (Data.StatusData.GuiFocus == StatusGuiFocus.FSSMode)
                {
                    CommandTools.SendKeypress(Program.Binding[BindingType.Ship].ExplorationFSSQuit);

                    if (!ButtonData.DontSwitchToCombatMode && Data.StatusData.HudInAnalysisMode)
                    {
                        Thread.Sleep(300);

                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship]
                            .PlayerHUDModeToggle); // back to combat mode
                    }
                }
                else
                {
                    if (!Data.StatusData.HudInAnalysisMode)
                    {
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship]
                            .PlayerHUDModeToggle); // to analysis mode
                        Thread.Sleep(100);
                    }

                    CommandTools.SendKeypress(Program.Binding[BindingType.Ship].ExplorationFSSEnter);
                }

                Program.PlaySound(ref _clickSound, ButtonData.ClickSound);

            }
            else
            {
                Program.PlaySound(ref _errorSound, ButtonData.ErrorSound);
            }

        }

    }
}
