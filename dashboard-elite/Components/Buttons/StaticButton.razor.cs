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
    public partial class StaticButton
    {
        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon); 

        private CachedSound _clickSound = null;

        private void ButtonClick()
        {

            InteropMouse.JsMouseUp();

            Program.PlaySound(ref _clickSound, ButtonData.ClickSound);

            Thread.Sleep(100);

            switch (ButtonData.Function.ToLower())
            {
                case "orderfocustarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderFocusTarget);
                    break;
                case "orderaggressivebehaviour":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderAggressiveBehaviour);
                    break;
                case "orderdefensivebehaviour":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderDefensiveBehaviour);
                    break;
                case "openorders":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OpenOrders);
                    break;
                case "orderrequestdock":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderRequestDock);
                    break;
                case "orderfollow":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderFollow);
                    break;
                case "orderholdfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderHoldFire);
                    break;
                case "orderholdposition":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrderHoldPosition);
                    break;

                case "headlookpitchdown":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HeadLookPitchDown);
                    break;
                case "headlookyawleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HeadLookYawLeft);
                    break;
                case "headlookyawright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HeadLookYawRight);
                    break;
                case "headlookpitchup":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HeadLookPitchUp);
                    break;
                case "headlookreset":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HeadLookReset);
                    break;
                case "opencodexgotodiscovery":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OpenCodexGoToDiscovery);
                    break;
                case "friendsmenu":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FriendsMenu);
                    break;
                case "pause":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].Pause);
                    break;
                case "microphonemute":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MicrophoneMute);
                    break;

                case "hmdreset":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HMDReset);
                    break;
                case "oculusreset":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OculusReset);
                    break;
                case "radardecreaserange":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RadarDecreaseRange);
                    break;
                case "radarincreaserange":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RadarIncreaseRange);
                    break;
                case "multicrewthirdpersonfovinbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewThirdPersonFovInButton);
                    break;
                case "multicrewthirdpersonfovoutbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewThirdPersonFovOutButton);
                    break;
                case "multicrewprimaryfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewPrimaryFire);
                    break;
                case "multicrewsecondaryfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewSecondaryFire);
                    break;
                case "multicrewtogglemode":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewToggleMode);
                    break;
                case "multicrewthirdpersonpitchdownbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewThirdPersonPitchDownButton);
                    break;
                case "multicrewthirdpersonpitchupbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewThirdPersonPitchUpButton);
                    break;
                case "multicrewprimaryutilityfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewPrimaryUtilityFire);
                    break;
                case "multicrewsecondaryutilityfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewSecondaryUtilityFire);
                    break;
                case "multicrewcockpituicyclebackward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewCockpitUICycleBackward);
                    break;
                case "multicrewcockpituicycleforward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewCockpitUICycleForward);
                    break;
                case "multicrewthirdpersonyawleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewThirdPersonYawLeftButton);
                    break;
                case "multicrewthirdpersonyawrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MultiCrewThirdPersonYawRightButton);
                    break;
                case "saathirdpersonfovinbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SAAThirdPersonFovInButton);
                    break;
                case "saathirdpersonfovoutbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SAAThirdPersonFovOutButton);
                    break;
                case "explorationfssenter":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSEnter);
                    break;
                case "explorationsaaexitthirdperson":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationSAAExitThirdPerson);
                    break;
                case "explorationfssquit":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSQuit);
                    break;
                case "explorationfssshowhelp":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSShowHelp);
                    break;
                case "explorationsaanextgenus":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationSAANextGenus);
                    break;
                case "explorationsaapreviousgenus":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationSAAPreviousGenus);
                    break;
                case "explorationfssdiscoveryscan":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSDiscoveryScan);
                    break;
                case "explorationfsscamerapitchdecreasebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSCameraPitchDecreaseButton);
                    break;
                case "explorationfsscamerapitchincreasebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSCameraPitchIncreaseButton);
                    break;
                case "explorationfssradiotuningx_decrease":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSRadioTuningX_Decrease);
                    break;
                case "explorationfssradiotuningx_increase":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSRadioTuningX_Increase);
                    break;
                case "explorationfsscamerayawdecreasebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSCameraYawDecreaseButton);
                    break;
                case "explorationfsscamerayawincreasebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSCameraYawIncreaseButton);
                    break;
                case "saathirdpersonpitchdownbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SAAThirdPersonPitchDownButton);
                    break;
                case "saathirdpersonpitchupbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SAAThirdPersonPitchUpButton);
                    break;
                case "explorationfssminizoomin":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSMiniZoomIn);
                    break;
                case "explorationfssminizoomout":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSMiniZoomOut);
                    break;
                case "explorationfsstarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSTarget);
                    break;
                case "explorationsaachangescannedareaviewtoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationSAAChangeScannedAreaViewToggle);
                    break;
                case "saathirdpersonyawleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SAAThirdPersonYawLeftButton);
                    break;
                case "saathirdpersonyawrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SAAThirdPersonYawRightButton);
                    break;
                case "explorationfsszoomin":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSZoomIn);
                    break;
                case "explorationfsszoomout":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ExplorationFSSZoomOut);
                    break;
                case "quickcommspanel":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].QuickCommsPanel);
                    break;
                case "uifocus":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UIFocus);
                    break;
                case "targetwingman0":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].TargetWingman0);
                    break;
                case "targetwingman1":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].TargetWingman1);
                    break;
                case "targetwingman2":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].TargetWingman2);
                    break;
                case "wingnavlock":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].WingNavLock);
                    break;
                case "selecttargetstarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SelectTargetsTarget);
                    break;
                case "firechafflauncher":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].FireChaffLauncher);
                    break;
                case "chargeecm":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ChargeECM);
                    break;
                case "increaseenginespower":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].IncreaseEnginesPower);
                    break;
                case "primaryfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].PrimaryFire);
                    break;
                case "secondaryfire":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SecondaryFire);
                    break;
                case "cyclenexttarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CycleNextTarget);
                    break;
                case "cyclefiregroupnext":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CycleFireGroupNext);
                    break;
                case "cyclenexthostiletarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CycleNextHostileTarget);
                    break;
                case "cyclenextsubsystem":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CycleNextSubsystem);
                    break;
                case "cycleprevioustarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CyclePreviousTarget);
                    break;
                case "cyclefiregroupprevious":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CycleFireGroupPrevious);
                    break;
                case "cycleprevioushostiletarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CyclePreviousHostileTarget);
                    break;
                case "cycleprevioussubsystem":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].CyclePreviousSubsystem);
                    break;
                case "resetpowerdistribution":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ResetPowerDistribution);
                    break;
                case "useshieldcell":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UseShieldCell);
                    break;
                case "increasesystemspower":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].IncreaseSystemsPower);
                    break;
                case "selecttarget":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SelectTarget);
                    break;
                case "increaseweaponspower":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].IncreaseWeaponsPower);
                    break;
                case "showpgscoresummaryinput":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ShowPGScoreSummaryInput);
                    break;
                case "ejectallcargo":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].EjectAllCargo);
                    break;
                case "enginecolourtoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].EngineColourToggle);
                    break;
                case "orbitlinestoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].OrbitLinesToggle);
                    break;
                case "mousereset":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].MouseReset);
                    break;
                case "headlooktoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].HeadLookToggle);
                    break;
                case "weaponcolourtoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].WeaponColourToggle);
                    break;
                case "setspeedminus100":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeedMinus100);
                    break;
                case "setspeed100":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeed100);
                    break;
                case "setspeedminus25":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeedMinus25);
                    break;
                case "setspeed25":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeed25);
                    break;
                case "setspeedminus50":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeedMinus50);
                    break;
                case "setspeed50":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeed50);
                    break;
                case "setspeedminus75":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeedMinus75);
                    break;
                case "setspeed75":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeed75);
                    break;
                case "setspeedzero":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].SetSpeedZero);
                    break;
                case "usealternateflightvaluestoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UseAlternateFlightValuesToggle);
                    break;
                case "useboostjuice":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UseBoostJuice);
                    break;
                case "forwardkey":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ForwardKey);
                    break;
                case "forwardthrustbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ForwardThrustButton);
                    break;
                case "forwardthrustbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ForwardThrustButton_Landing);
                    break;
                case "targetnextroutesystem":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].TargetNextRouteSystem);
                    break;
                case "pitchdownbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].PitchDownButton);
                    break;
                case "pitchdownbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].PitchDownButton_Landing);
                    break;
                case "pitchupbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].PitchUpButton);
                    break;
                case "pitchupbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].PitchUpButton_Landing);
                    break;
                case "togglereversethrottleinput":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].ToggleReverseThrottleInput);
                    break;
                case "backwardkey":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].BackwardKey);
                    break;
                case "backwardthrustbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].BackwardThrustButton);
                    break;
                case "backwardthrustbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].BackwardThrustButton_Landing);
                    break;
                case "rollleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RollLeftButton);
                    break;
                case "rollleftbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RollLeftButton_Landing);
                    break;
                case "rollrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RollRightButton);
                    break;
                case "rollrightbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RollRightButton_Landing);
                    break;
                case "disablerotationcorrecttoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].DisableRotationCorrectToggle);
                    break;
                case "downthrustbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].DownThrustButton);
                    break;
                case "downthrustbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].DownThrustButton_Landing);
                    break;
                case "leftthrustbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].LeftThrustButton);
                    break;
                case "leftthrustbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].LeftThrustButton_Landing);
                    break;
                case "rightthrustbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RightThrustButton);
                    break;
                case "rightthrustbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].RightThrustButton_Landing);
                    break;
                case "upthrustbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UpThrustButton);
                    break;
                case "upthrustbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].UpThrustButton_Landing);
                    break;
                case "yawleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].YawLeftButton);
                    break;
                case "yawleftbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].YawLeftButton_Landing);
                    break;
                case "yawrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].YawRightButton);
                    break;
                case "yawrightbutton_landing":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].YawRightButton_Landing);
                    break;
                case "yawtorollbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Ship].YawToRollButton);
                    break;


                // general

                case "cyclenextpage":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CycleNextPage);
                    break;
                case "cyclenextpanel":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CycleNextPanel);
                    break;
                case "cyclepreviouspage":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CyclePreviousPage);
                    break;
                case "cyclepreviouspanel":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CyclePreviousPanel);
                    break;
                case "ui_back":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Back);
                    break;
                case "ui_down":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Down);
                    break;
                case "ui_left":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Left);
                    break;
                case "ui_right":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Right);
                    break;
                case "ui_select":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Select);
                    break;
                case "ui_toggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Toggle);
                    break;
                case "ui_up":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].UI_Up);
                    break;

                case "camtranslatebackward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateBackward);
                    break;
                case "camtranslatedown":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateDown);
                    break;
                case "camtranslateforward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateForward);
                    break;
                case "camtranslateleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateLeft);
                    break;
                case "campitchdown":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamPitchDown);
                    break;
                case "campitchup":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamPitchUp);
                    break;
                case "camtranslateright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateRight);
                    break;
                case "camtranslateup":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateUp);
                    break;
                case "camyawleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamYawLeft);
                    break;
                case "camyawright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamYawRight);
                    break;
                case "camtranslatezhold":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamTranslateZHold);
                    break;
                case "camzoomin":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamZoomIn);
                    break;
                case "camzoomout":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CamZoomOut);
                    break;

                case "movefreecambackwards":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].MoveFreeCamBackwards);
                    break;
                case "movefreecamdown":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].MoveFreeCamDown);
                    break;
                case "movefreecamforward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].MoveFreeCamForward);
                    break;
                case "movefreecamleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].MoveFreeCamLeft);
                    break;
                case "togglereversethrottleinputfreecam":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].ToggleReverseThrottleInputFreeCam);
                    break;
                case "movefreecamright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].MoveFreeCamRight);
                    break;
                case "movefreecamup":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].MoveFreeCamUp);
                    break;
                case "freecamspeeddec":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FreeCamSpeedDec);
                    break;
                case "togglefreecam":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].ToggleFreeCam);
                    break;
                case "freecamspeedinc":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FreeCamSpeedInc);
                    break;
                case "freecamtogglehud":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FreeCamToggleHUD);
                    break;
                case "freecamzoomin":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FreeCamZoomIn);
                    break;
                case "freecamzoomout":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FreeCamZoomOut);
                    break;

                case "photocameratoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].PhotoCameraToggle);
                    break;
                case "storepitchcameradown":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StorePitchCameraDown);
                    break;
                case "storepitchcameraup":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StorePitchCameraUp);
                    break;
                case "storeenablerotation":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StoreEnableRotation);
                    break;
                case "storeyawcameraleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StoreYawCameraLeft);
                    break;
                case "storeyawcameraright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StoreYawCameraRight);
                    break;
                case "storecamzoomin":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StoreCamZoomIn);
                    break;
                case "storecamzoomout":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StoreCamZoomOut);
                    break;
                case "storetoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].StoreToggle);
                    break;
                case "toggleadvancemode":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].ToggleAdvanceMode);
                    break;
                case "vanitycameraeight":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraEight);
                    break;
                case "vanitycameratwo":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraTwo);
                    break;
                case "vanitycameraone":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraOne);
                    break;
                case "vanitycamerathree":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraThree);
                    break;
                case "vanitycamerafour":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraFour);
                    break;
                case "vanitycamerafive":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraFive);
                    break;
                case "vanitycamerasix":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraSix);
                    break;
                case "vanitycameraseven":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraSeven);
                    break;
                case "vanitycameranine":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraNine);
                    break;
                case "pitchcameradown":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].PitchCameraDown);
                    break;
                case "pitchcameraup":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].PitchCameraUp);
                    break;
                case "rollcameraleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].RollCameraLeft);
                    break;
                case "rollcameraright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].RollCameraRight);
                    break;
                case "togglerotationlock":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].ToggleRotationLock);
                    break;
                case "yawcameraleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].YawCameraLeft);
                    break;
                case "yawcameraright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].YawCameraRight);
                    break;
                case "fstopdec":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FStopDec);
                    break;
                case "quitcamera":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].QuitCamera);
                    break;
                case "focusdistanceinc":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FocusDistanceInc);
                    break;
                case "focusdistancedec":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FocusDistanceDec);
                    break;
                case "fstopinc":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FStopInc);
                    break;
                case "fixcamerarelativetoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FixCameraRelativeToggle);
                    break;
                case "fixcameraworldtoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].FixCameraWorldToggle);
                    break;
                case "vanitycamerascrollright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraScrollRight);
                    break;
                case "vanitycamerascrollleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].VanityCameraScrollLeft);
                    break;

                case "commandercreator_redo":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CommanderCreator_Redo);
                    break;
                case "commandercreator_rotation":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CommanderCreator_Rotation);
                    break;
                case "commandercreator_rotation_mousetoggle":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CommanderCreator_Rotation_MouseToggle);
                    break;
                case "commandercreator_undo":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].CommanderCreator_Undo);
                    break;

                case "galnetaudio_clearqueue":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].GalnetAudio_ClearQueue);
                    break;
                case "galnetaudio_skipforward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].GalnetAudio_SkipForward);
                    break;
                case "galnetaudio_play_pause":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].GalnetAudio_Play_Pause);
                    break;
                case "galnetaudio_skipbackward":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.General].GalnetAudio_SkipBackward);
                    break;

                // in srv

                case "steerleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].SteerLeftButton);
                    break;
                case "steerrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].SteerRightButton);
                    break;
                case "increasespeedbuttonmax":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseSpeedButtonMax);
                    break;
                case "decreasespeedbuttonmax":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].DecreaseSpeedButtonMax);
                    break;
                case "increasespeedbuttonpartial":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseSpeedButtonPartial);
                    break;
                case "decreasespeedbuttonpartial":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].DecreaseSpeedButtonPartial);
                    break;
                case "recalldismissship":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].RecallDismissShip);
                    break;
                case "verticalthrustersbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].VerticalThrustersButton);
                    break;

                case "photocameratoggle_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].PhotoCameraToggle_Buggy);
                    break;
                case "ejectallcargo_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].EjectAllCargo_Buggy);
                    break;
                case "quickcommspanel_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].QuickCommsPanel_Buggy);
                    break;
                case "headlooktoggle_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].HeadLookToggle_Buggy);
                    break;
                case "uifocus_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].UIFocus_Buggy);
                    break;
                case "increaseenginespower_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseEnginesPower_Buggy);
                    break;
                case "buggyprimaryfirebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyPrimaryFireButton);
                    break;
                case "resetpowerdistribution_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].ResetPowerDistribution_Buggy);
                    break;
                case "buggysecondaryfirebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggySecondaryFireButton);
                    break;
                case "increasesystemspower_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseSystemsPower_Buggy);
                    break;
                case "selecttarget_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].SelectTarget_Buggy);
                    break;
                case "buggyturretpitchdownbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyTurretPitchDownButton);
                    break;
                case "buggyturretyawleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyTurretYawLeftButton);
                    break;
                case "buggyturretyawrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyTurretYawRightButton);
                    break;
                case "buggyturretpitchupbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyTurretPitchUpButton);
                    break;
                case "increaseweaponspower_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].IncreaseWeaponsPower_Buggy);
                    break;
                case "togglecargoscoop_buggy":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].ToggleCargoScoop_Buggy);
                    break;
                case "headlightsbuggybutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].HeadlightsBuggyButton);
                    break;
                case "buggypitchdownbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyPitchDownButton);
                    break;
                case "buggypitchupbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyPitchUpButton);
                    break;
                case "buggytogglereversethrottleinput":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyToggleReverseThrottleInput);
                    break;
                case "buggyrollleft":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyRollLeft);
                    break;
                case "buggyrollleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyRollLeftButton);
                    break;
                case "buggyrollright":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyRollRight);
                    break;
                case "buggyrollrightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.Srv].BuggyRollRightButton);
                    break;

                // on foot

                case "humanoidclearauthoritylevel":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidClearAuthorityLevel);
                    break;
                case "humanoidhealthpack":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidHealthPack);
                    break;
                case "humanoidbattery":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidBattery);
                    break;
                case "humanoidselectfraggrenade":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectFragGrenade);
                    break;
                case "humanoidselectempgrenade":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectEMPGrenade);
                    break;
                case "humanoidselectshieldgrenade":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectShieldGrenade);
                    break;

                case "photocameratoggle_humanoid":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].PhotoCameraToggle_Humanoid);
                    break;
                case "humanoidforwardbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidForwardButton);
                    break;
                case "humanoidbackwardbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidBackwardButton);
                    break;
                case "humanoidstrafeleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidStrafeLeftButton);
                    break;
                case "humanoidstraferightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidStrafeRightButton);
                    break;
                case "humanoidsprintbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSprintButton);
                    break;
                case "humanoidcrouchbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidCrouchButton);
                    break;
                case "humanoidjumpbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidJumpButton);
                    break;
                case "humanoidprimaryinteractbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidPrimaryInteractButton);
                    break;
                case "humanoiditemwheelbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidItemWheelButton);
                    break;
                case "humanoidprimaryfirebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidPrimaryFireButton);
                    break;
                case "humanoidselectprimaryweaponbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectPrimaryWeaponButton);
                    break;
                case "humanoidselectsecondaryweaponbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectSecondaryWeaponButton);
                    break;
                case "humanoidhideweaponbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidHideWeaponButton);
                    break;
                case "humanoidzoombutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidZoomButton);
                    break;
                case "humanoidreloadbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidReloadButton);
                    break;
                case "humanoidthrowgrenadebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidThrowGrenadeButton);
                    break;
                case "humanoidmeleebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidMeleeButton);
                    break;
                case "humanoidopenaccesspanelbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidOpenAccessPanelButton);
                    break;
                case "humanoidsecondaryinteractbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSecondaryInteractButton);
                    break;
                case "humanoidswitchtorechargetool":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSwitchToRechargeTool);
                    break;
                case "humanoidswitchtocompanalyser":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSwitchToCompAnalyser);
                    break;
                case "humanoidtoggletoolmodebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidToggleToolModeButton);
                    break;
                case "humanoidtogglenightvisionbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidToggleNightVisionButton);
                    break;
                case "humanoidswitchtosuittool":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSwitchToSuitTool);
                    break;
                case "humanoidtoggleflashlightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidToggleFlashlightButton);
                    break;
                case "quickcommspanel_humanoid":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].QuickCommsPanel_Humanoid);
                    break;
                case "humanoidconflictcontextualuibutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidConflictContextualUIButton);
                    break;
                case "humanoidtoggleshieldsbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidToggleShieldsButton);
                    break;

                case "humanoidrotateleftbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidRotateLeftButton);
                    break;
                case "humanoidrotaterightbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidRotateRightButton);
                    break;
                case "humanoidpitchupbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidPitchUpButton);
                    break;
                case "humanoidpitchdownbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidPitchDownButton);
                    break;
                case "humanoidswitchweapon":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSwitchWeapon);
                    break;
                case "humanoidselectutilityweaponbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectUtilityWeaponButton);
                    break;
                case "humanoidselectnextweaponbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectNextWeaponButton);
                    break;
                case "humanoidselectpreviousweaponbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectPreviousWeaponButton);
                    break;
                case "humanoidselectnextgrenadetypebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectNextGrenadeTypeButton);
                    break;
                case "humanoidselectpreviousgrenadetypebutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidSelectPreviousGrenadeTypeButton);
                    break;
                case "humanoidtogglemissionhelppanelbutton":
                    CommandTools.SendKeypressQueue(Program.Binding[BindingType.OnFoot].HumanoidToggleMissionHelpPanelButton);
                    break;

            }


        }
    }

}
