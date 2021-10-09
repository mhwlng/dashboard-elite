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
    public partial class LimpetButton
    {
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string AdjustSVG(string svg)
        {
            if (Data.LimpetCount > 0)
            {
                svg = svg.Replace("{0}", Data.LimpetCount.ToString());
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

                return AdjustSVG(svg); // normal
            }
        }

        private string SecondaryIcon
        {
            get
            {
                var svg = SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon);

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

        private MudTripleIconButtonState LimpetState => Data.LimpetCount > 0 && !IsDisabled
            ? MudTripleIconButtonState.Primary
            : MudTripleIconButtonState.Secondary;

        private void ButtonClick()
        {

            InteropMouse.JsMouseUp();

            Thread.Sleep(100);

            if (!IsDisabled && Data.LimpetCount > 0)
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
                }

                var cycle = Convert.ToInt32(fireGroup) - Data.StatusData.Firegroup;

                if (cycle < 0)
                {
                    for (var f = 0; f < -cycle; f++)
                    {
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship].CycleFireGroupPrevious);
                        Thread.Sleep(30);
                    }
                }
                else if (cycle > 0)
                {
                    for (var f = 0; f < cycle; f++)
                    {
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship].CycleFireGroupNext);
                        Thread.Sleep(30);
                    }
                }

                Thread.Sleep(100);

                CommandTools.SendKeypress(ButtonData.Fire.ToLower() == "primary"
                    ? Program.Binding[BindingType.Ship].PrimaryFire
                    : Program.Binding[BindingType.Ship].SecondaryFire);

                Thread.Sleep(100);

                if (cycle < 0)
                {
                    for (var f = 0; f < -cycle; f++)
                    {
                        Thread.Sleep(30);
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship].CycleFireGroupNext);
                    }

                }
                else if (cycle > 0)
                {
                    for (var f = 0; f < cycle; f++)
                    {
                        Thread.Sleep(30);
                        CommandTools.SendKeypress(Program.Binding[BindingType.Ship].CycleFireGroupPrevious);
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