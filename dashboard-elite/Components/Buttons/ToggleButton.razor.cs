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
    public partial class ToggleButton
    {
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public Data Data { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon); // off

        private string SecondaryIcon => SvgCacheService.ButtonIcon(ButtonData.SecondaryIcon); // on

        private CachedSound _clickSound = null;

        private MudTripleIconButtonState ToggleState
        {
            get
            {
                var isOn = false;

                switch (ButtonData.Function.ToLower())
                {
                    case "focuscommspanel":
                        isOn = Data.StatusData.GuiFocus == StatusGuiFocus.CommsPanel;
                        break;
                    case "focusradarpanel":
                        isOn = Data.StatusData.GuiFocus == StatusGuiFocus.RolePanel;
                        break;
                    case "focusrightpanel":
                        isOn = Data.StatusData.GuiFocus == StatusGuiFocus.InternalPanel;
                        break;
                    case "focusleftpanel":
                        isOn = Data.StatusData.GuiFocus == StatusGuiFocus.ExternalPanel;
                        break;

                    case "galaxymap":
                        isOn = Data.StatusData.GuiFocus == StatusGuiFocus.GalaxyMap;
                        break;
                    case "systemmap":
                        isOn = Data.StatusData.GuiFocus == StatusGuiFocus.SystemMap ||
                               Data.StatusData.GuiFocus == StatusGuiFocus.Orrery;
                        break;

                    case "togglecargoscoop":
                        isOn = Data.StatusData.CargoScoopDeployed;
                        break;
                    case "landinggeartoggle":
                        isOn = Data.StatusData.LandingGearDown;
                        break;

                    case "toggleflightassist":
                        isOn = !Data.StatusData.FlightAssistOff;
                        break;

                    case "shipspotlighttoggle":
                        isOn = Data.StatusData.LightsOn || Data.StatusData.SrvHighBeam;
                        break;

                    case "nightvisiontoggle":
                        isOn = Data.StatusData.NightVision;
                        break;

                    case "playerhudmodetoggle":
                        isOn = Data.StatusData.HudInAnalysisMode;
                        break;

                    case "deployhardpointtoggle":
                        isOn = Data.StatusData.HardpointsDeployed;
                        break;

                    case "supercruise":
                        isOn = Data.StatusData.Supercruise;
                        break;

                    case "togglebuttonupinput":
                        isOn = Data.StatusData.SilentRunning;
                        break;

                    case "togglebuggyturretbutton":
                        isOn = Data.StatusData.SrvTurret;
                        break;

                    case "toggledriveassist":
                        isOn = Data.StatusData.SrvDriveAssist;
                        break;

                    case "autobreakbuggybutton":
                        isOn = Data.StatusData.SrvHandbrake;
                        break;
                }

                return isOn ? MudTripleIconButtonState.Secondary : MudTripleIconButtonState.Primary;
            }
        }

        private void ButtonClick()
        {

            InteropMouse.JsMouseUp();

            Program.PlaySound(ref _clickSound, ButtonData.ClickSound);

            Thread.Sleep(100);

            switch (ButtonData.Function.ToLower())
            {
                case "focuscommspanel":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].FocusCommsPanel_Buggy);
                    else if (Data.StatusData.OnFoot)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].FocusCommsPanel_Humanoid);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FocusCommsPanel);
                    break;
                case "focusleftpanel":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].FocusLeftPanel_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FocusLeftPanel);
                    break;
                case "focusradarpanel":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].FocusRadarPanel_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FocusRadarPanel);
                    break;
                case "focusrightpanel":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].FocusRightPanel_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FocusRightPanel);
                    break;

                case "galaxymap":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].GalaxyMapOpen_Buggy);
                    else if (Data.StatusData.OnFoot)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].GalaxyMapOpen_Humanoid);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].GalaxyMapOpen);
                    break;
                case "systemmap":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].SystemMapOpen_Buggy);
                    else if (Data.StatusData.OnFoot)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].SystemMapOpen_Humanoid);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SystemMapOpen);
                    break;

                case "togglecargoscoop":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].ToggleCargoScoop_Buggy);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ToggleCargoScoop);
                    break;
                case "landinggeartoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].LandingGearToggle);
                    break;

                case "toggleflightassist":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ToggleFlightAssist);
                    break;

                case "shipspotlighttoggle":
                    if (Data.StatusData.InSRV)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].HeadlightsBuggyButton);
                    else if (Data.StatusData.OnFoot)
                        CommandTools.SendKeypressQueue(
                            Program.Binding[BindingType.OnFoot].HumanoidToggleFlashlightButton);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ShipSpotLightToggle);
                    break;

                case "nightvisiontoggle":
                    if (Data.StatusData.OnFoot)
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot]
                            .HumanoidToggleNightVisionButton);
                    else
                        CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].NightVisionToggle);
                    break;

                case "playerhudmodetoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].PlayerHUDModeToggle);
                    break;

                case "deployhardpointtoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].DeployHardpointToggle);
                    break;

                case "supercruise":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].Supercruise);
                    break;

                case "togglebuttonupinput":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ToggleButtonUpInput);
                    break;

                case "togglebuggyturretbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].ToggleBuggyTurretButton);
                    break;

                case "toggledriveassist":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].ToggleDriveAssist);
                    break;

                case "autobreakbuggybutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].AutoBreakBuggyButton);
                    break;
            }

        }
    }

}
