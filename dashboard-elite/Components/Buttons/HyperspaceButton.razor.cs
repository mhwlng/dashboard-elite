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
    public partial class HyperspaceButton
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string AdjustSVG(string svg)
        {
            if (ButtonData.Function.ToLower() != "supercruise" &&
                Data.LocationData.StarSystem != Data.LocationData.FsdTargetName &&
                Data.LocationData.RemainingJumpsInRoute > 0)
            {
                svg = svg.Replace("{0}", Data.LocationData.RemainingJumpsInRoute.ToString());
            }
            else
            {
                svg = svg.Replace("{0}", "");
            }

            return svg;
        }

        private string PrimaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon);

                return AdjustSVG(svg); // off
            }
        }

        private string SecondaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon);

                return AdjustSVG(svg); // engaged
            }
        }

        private string TertiaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.TertiaryIcon);

                return AdjustSVG(svg); // disabled
            }
        }

        private CachedSound _clickSound = null;
        private CachedSound _errorSound = null;

        private bool IsDisabled =>
            (Data.StatusData.OnFoot ||
             Data.StatusData.InSRV ||
             Data.StatusData.Docked ||
             Data.StatusData.Landed ||
             Data.StatusData.LandingGearDown ||
             Data.StatusData.CargoScoopDeployed ||

             //Data.StatusData.SilentRunning ||
             //Data.StatusData.ScoopingFuel ||
             //Data.StatusData.IsInDanger ||
             //Data.StatusData.BeingInterdicted ||
             //Data.StatusData.HudInAnalysisMode ||

             Data.StatusData.FsdMassLocked ||
             //Data.StatusData.FsdCharging ||
             Data.StatusData.FsdCooldown ||

             //Data.StatusData.Supercruise ||
             //Data.StatusData.FsdJump ||
             Data.StatusData.HardpointsDeployed);

        private MudTripleIconButtonState HyperspaceState
        {
            get
            {
                var state = MudTripleIconButtonState.Secondary; // engaged

                if (IsDisabled)
                {
                    state = MudTripleIconButtonState.Tertiary; // Disabled Image
                }
                else
                {
                    switch (ButtonData.Function.ToLower())
                    {
                        case "hypersupercombination":
                            if (!Data.StatusData.FsdJump)
                            {
                                state = MudTripleIconButtonState.Primary; // off
                            }

                            break;
                        case "hyperspace":
                            if (!Data.StatusData.FsdJump)
                            {
                                state = MudTripleIconButtonState.Primary; // off
                            }

                            break;
                        case "supercruise":
                            if (!Data.StatusData.Supercruise)
                            {
                                state = MudTripleIconButtonState.Primary; // off
                            }

                            break;
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

            if (!IsDisabled)
            {
                switch (ButtonData.Function.ToLower())
                {
                    case "hypersupercombination"
                        : // context dependent, i.e. jump if another system is targeted, supercruise if not.
                        CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].HyperSuperCombination, focusChange);
                        break;
                    case "supercruise": // supercruise even if another system targeted
                        CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].Supercruise, focusChange);
                        break;
                    case "hyperspace": // jump
                        CommandTools.SendKeypressQueue(Common.Binding[BindingType.Ship].Hyperspace, focusChange);
                        break;
                }

                Common.PlaySound(ref _clickSound, ButtonData.ClickSound);

            }
            else
            {
                Common.PlaySound(ref _errorSound, ButtonData.ErrorSound);
            }


        }
    }
}
