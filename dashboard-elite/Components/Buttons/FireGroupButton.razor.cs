using System;
using System.Threading;
using dashboard_elite.Audio;
using dashboard_elite.EliteData;
using dashboard_elite.Helpers;
using dashboard_elite.JsInterop;
using dashboard_elite.Services;
using EliteJournalReader.Events;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace dashboard_elite.Components.Buttons
{
    public partial class FireGroupButton
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon); // off

        private string SecondaryIcon => SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon); // on

        private string TertiaryIcon => SvgCacheService.ButtonIcon(ButtonData.TertiaryIcon); // disabled

        private CachedSound _clickSound = null;
        private CachedSound _errorSound = null;

        private int FireGroup
        {
            get
            {
                var fireGroup = 0;
                switch (ButtonData.Firegroup.ToUpper())
                {
                    case "A":
                        fireGroup = 0;
                        break;
                    case "B":
                        fireGroup = 1;
                        break;
                    case "C":
                        fireGroup = 2;
                        break;
                    case "D":
                        fireGroup = 3;
                        break;
                    case "E":
                        fireGroup = 4;
                        break;
                    case "F":
                        fireGroup = 5;
                        break;
                    case "G":
                        fireGroup = 6;
                        break;
                    case "H":
                        fireGroup = 7;
                        break;

                }

                return fireGroup;
            }
        }

        private MudTripleIconButtonState FireGroupState
        {
            get
            {
                var state = MudTripleIconButtonState.Tertiary;

                if (!IsDisabled)
                {
                    if (FireGroup == Data.StatusData.Firegroup)
                    {
                        state = MudTripleIconButtonState.Secondary; // on
                        
                    }
                    else
                    {
                        state = MudTripleIconButtonState.Primary; // off
                    }
                }

                return state;
            }
        }

        private bool IsDisabled =>
            (Data.StatusData.OnFoot ||
             Data.StatusData.InSRV ||
             Data.StatusData.Docked ||
             Data.StatusData.Landed ||
             Data.StatusData.LandingGearDown ||
             Data.StatusData.Supercruise ||
             Data.StatusData.FsdJump //||

            //Data.StatusData.CargoScoopDeployed ||
            //Data.StatusData.SilentRunning ||
            //Data.StatusData.ScoopingFuel ||
            //Data.StatusData.IsInDanger ||
            //Data.StatusData.BeingInterdicted ||
            //Data.StatusData.HudInAnalysisMode ||
            //Data.StatusData.FsdMassLocked ||
            //Data.StatusData.FsdCharging ||
            //Data.StatusData.FsdCooldown ||

            //Data.StatusData.HardpointsDeployed
            );

        private void ButtonClick()
        {
            var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

            if (focusChange)
            {
                InteropMouse.JsMouseUp();

                Thread.Sleep(100);
            }

            if (!IsDisabled && Data.LimpetCount > 0)
            {

                var cycle = FireGroup - Data.StatusData.Firegroup;

                if (cycle < 0)
                {
                    for (var f = 0; f < -cycle; f++)
                    {
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship].CycleFireGroupPrevious);
                        Thread.Sleep(70);
                    }
                }
                else if (cycle > 0)
                {
                    for (var f = 0; f < cycle; f++)
                    {
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship].CycleFireGroupNext);
                        Thread.Sleep(70);
                    }
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