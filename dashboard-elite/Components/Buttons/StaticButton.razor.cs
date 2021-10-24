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
    public partial class StaticButton
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private SvgCacheService SvgCacheService { get; set; }

        [Parameter] public ButtonData ButtonData { get; set; }

        private string PrimaryIcon => SvgCacheService.ButtonIcon(ButtonData.PrimaryIcon); 

        private CachedSound _clickSound = null;

        private bool StaticButtonBindingExists(string function)
        {
            var binding = GetStaticButtonBinding(ButtonData.Function.ToLower());

            return binding?.Primary.Device == "Keyboard" || binding?.Secondary.Device == "Keyboard";
        }

        private StandardBindingInfo GetStaticButtonBinding(string function)
        {
            switch (function)
            {
                case "orderfocustarget":
                    return Program.Binding[BindingType.Ship].OrderFocusTarget;
                case "orderaggressivebehaviour":
                    return Program.Binding[BindingType.Ship].OrderAggressiveBehaviour;
                case "orderdefensivebehaviour":
                    return Program.Binding[BindingType.Ship].OrderDefensiveBehaviour;
                case "openorders":
                    return Program.Binding[BindingType.Ship].OpenOrders;
                case "orderrequestdock":
                    return Program.Binding[BindingType.Ship].OrderRequestDock;
                case "orderfollow":
                    return Program.Binding[BindingType.Ship].OrderFollow;
                case "orderholdfire":
                    return Program.Binding[BindingType.Ship].OrderHoldFire;
                case "orderholdposition":
                    return Program.Binding[BindingType.Ship].OrderHoldPosition;

                case "headlookpitchdown":
                    return Program.Binding[BindingType.Ship].HeadLookPitchDown;
                case "headlookyawleft":
                    return Program.Binding[BindingType.Ship].HeadLookYawLeft;
                case "headlookyawright":
                    return Program.Binding[BindingType.Ship].HeadLookYawRight;
                case "headlookpitchup":
                    return Program.Binding[BindingType.Ship].HeadLookPitchUp;
                case "headlookreset":
                    return Program.Binding[BindingType.Ship].HeadLookReset;
                case "opencodexgotodiscovery":
                    return Program.Binding[BindingType.Ship].OpenCodexGoToDiscovery;
                case "friendsmenu":
                    return Program.Binding[BindingType.Ship].FriendsMenu;
                case "pause":
                    return Program.Binding[BindingType.Ship].Pause;
                case "microphonemute":
                    return Program.Binding[BindingType.Ship].MicrophoneMute;

                case "hmdreset":
                    return Program.Binding[BindingType.Ship].HMDReset;
                case "oculusreset":
                    return Program.Binding[BindingType.Ship].OculusReset;
                case "radardecreaserange":
                    return Program.Binding[BindingType.Ship].RadarDecreaseRange;
                case "radarincreaserange":
                    return Program.Binding[BindingType.Ship].RadarIncreaseRange;
                case "multicrewthirdpersonfovinbutton":
                    return Program.Binding[BindingType.Ship].MultiCrewThirdPersonFovInButton;
                case "multicrewthirdpersonfovoutbutton":
                    return Program.Binding[BindingType.Ship].MultiCrewThirdPersonFovOutButton;
                case "multicrewprimaryfire":
                    return Program.Binding[BindingType.Ship].MultiCrewPrimaryFire;
                case "multicrewsecondaryfire":
                    return Program.Binding[BindingType.Ship].MultiCrewSecondaryFire;
                case "multicrewtogglemode":
                    return Program.Binding[BindingType.Ship].MultiCrewToggleMode;
                case "multicrewthirdpersonpitchdownbutton":
                    return Program.Binding[BindingType.Ship].MultiCrewThirdPersonPitchDownButton;
                case "multicrewthirdpersonpitchupbutton":
                    return Program.Binding[BindingType.Ship].MultiCrewThirdPersonPitchUpButton;
                case "multicrewprimaryutilityfire":
                    return Program.Binding[BindingType.Ship].MultiCrewPrimaryUtilityFire;
                case "multicrewsecondaryutilityfire":
                    return Program.Binding[BindingType.Ship].MultiCrewSecondaryUtilityFire;
                case "multicrewcockpituicyclebackward":
                    return Program.Binding[BindingType.Ship].MultiCrewCockpitUICycleBackward;
                case "multicrewcockpituicycleforward":
                    return Program.Binding[BindingType.Ship].MultiCrewCockpitUICycleForward;
                case "multicrewthirdpersonyawleftbutton":
                    return Program.Binding[BindingType.Ship].MultiCrewThirdPersonYawLeftButton;
                case "multicrewthirdpersonyawrightbutton":
                    return Program.Binding[BindingType.Ship].MultiCrewThirdPersonYawRightButton;
                case "saathirdpersonfovinbutton":
                    return Program.Binding[BindingType.Ship].SAAThirdPersonFovInButton;
                case "saathirdpersonfovoutbutton":
                    return Program.Binding[BindingType.Ship].SAAThirdPersonFovOutButton;
                case "explorationfssenter":
                    return Program.Binding[BindingType.Ship].ExplorationFSSEnter;
                case "explorationsaaexitthirdperson":
                    return Program.Binding[BindingType.Ship].ExplorationSAAExitThirdPerson;
                case "explorationfssquit":
                    return Program.Binding[BindingType.Ship].ExplorationFSSQuit;
                case "explorationfssshowhelp":
                    return Program.Binding[BindingType.Ship].ExplorationFSSShowHelp;
                case "explorationsaanextgenus":
                    return Program.Binding[BindingType.Ship].ExplorationSAANextGenus;
                case "explorationsaapreviousgenus":
                    return Program.Binding[BindingType.Ship].ExplorationSAAPreviousGenus;
                case "explorationfssdiscoveryscan":
                    return Program.Binding[BindingType.Ship].ExplorationFSSDiscoveryScan;
                case "explorationfsscamerapitchdecreasebutton":
                    return Program.Binding[BindingType.Ship].ExplorationFSSCameraPitchDecreaseButton;
                case "explorationfsscamerapitchincreasebutton":
                    return Program.Binding[BindingType.Ship].ExplorationFSSCameraPitchIncreaseButton;
                case "explorationfssradiotuningx_decrease":
                    return Program.Binding[BindingType.Ship].ExplorationFSSRadioTuningX_Decrease;
                case "explorationfssradiotuningx_increase":
                    return Program.Binding[BindingType.Ship].ExplorationFSSRadioTuningX_Increase;
                case "explorationfsscamerayawdecreasebutton":
                    return Program.Binding[BindingType.Ship].ExplorationFSSCameraYawDecreaseButton;
                case "explorationfsscamerayawincreasebutton":
                    return Program.Binding[BindingType.Ship].ExplorationFSSCameraYawIncreaseButton;
                case "saathirdpersonpitchdownbutton":
                    return Program.Binding[BindingType.Ship].SAAThirdPersonPitchDownButton;
                case "saathirdpersonpitchupbutton":
                    return Program.Binding[BindingType.Ship].SAAThirdPersonPitchUpButton;
                case "explorationfssminizoomin":
                    return Program.Binding[BindingType.Ship].ExplorationFSSMiniZoomIn;
                case "explorationfssminizoomout":
                    return Program.Binding[BindingType.Ship].ExplorationFSSMiniZoomOut;
                case "explorationfsstarget":
                    return Program.Binding[BindingType.Ship].ExplorationFSSTarget;
                case "explorationsaachangescannedareaviewtoggle":
                    return Program.Binding[BindingType.Ship].ExplorationSAAChangeScannedAreaViewToggle;
                case "saathirdpersonyawleftbutton":
                    return Program.Binding[BindingType.Ship].SAAThirdPersonYawLeftButton;
                case "saathirdpersonyawrightbutton":
                    return Program.Binding[BindingType.Ship].SAAThirdPersonYawRightButton;
                case "explorationfsszoomin":
                    return Program.Binding[BindingType.Ship].ExplorationFSSZoomIn;
                case "explorationfsszoomout":
                    return Program.Binding[BindingType.Ship].ExplorationFSSZoomOut;
                case "quickcommspanel":
                    return Program.Binding[BindingType.Ship].QuickCommsPanel;
                case "uifocus":
                    return Program.Binding[BindingType.Ship].UIFocus;
                case "targetwingman0":
                    return Program.Binding[BindingType.Ship].TargetWingman0;
                case "targetwingman1":
                    return Program.Binding[BindingType.Ship].TargetWingman1;
                case "targetwingman2":
                    return Program.Binding[BindingType.Ship].TargetWingman2;
                case "wingnavlock":
                    return Program.Binding[BindingType.Ship].WingNavLock;
                case "selecttargetstarget":
                    return Program.Binding[BindingType.Ship].SelectTargetsTarget;
                case "firechafflauncher":
                    return Program.Binding[BindingType.Ship].FireChaffLauncher;
                case "chargeecm":
                    return Program.Binding[BindingType.Ship].ChargeECM;
                case "primaryfire":
                    return Program.Binding[BindingType.Ship].PrimaryFire;
                case "secondaryfire":
                    return Program.Binding[BindingType.Ship].SecondaryFire;
                case "cyclenexttarget":
                    return Program.Binding[BindingType.Ship].CycleNextTarget;
                case "cyclefiregroupnext":
                    return Program.Binding[BindingType.Ship].CycleFireGroupNext;
                case "cyclenexthostiletarget":
                    return Program.Binding[BindingType.Ship].CycleNextHostileTarget;
                case "cyclenextsubsystem":
                    return Program.Binding[BindingType.Ship].CycleNextSubsystem;
                case "cycleprevioustarget":
                    return Program.Binding[BindingType.Ship].CyclePreviousTarget;
                case "cyclefiregroupprevious":
                    return Program.Binding[BindingType.Ship].CycleFireGroupPrevious;
                case "cycleprevioushostiletarget":
                    return Program.Binding[BindingType.Ship].CyclePreviousHostileTarget;
                case "cycleprevioussubsystem":
                    return Program.Binding[BindingType.Ship].CyclePreviousSubsystem;
                case "useshieldcell":
                    return Program.Binding[BindingType.Ship].UseShieldCell;
                case "selecttarget":
                    return Program.Binding[BindingType.Ship].SelectTarget;
                case "showpgscoresummaryinput":
                    return Program.Binding[BindingType.Ship].ShowPGScoreSummaryInput;
                case "ejectallcargo":
                    return Program.Binding[BindingType.Ship].EjectAllCargo;
                case "enginecolourtoggle":
                    return Program.Binding[BindingType.Ship].EngineColourToggle;
                case "orbitlinestoggle":
                    return Program.Binding[BindingType.Ship].OrbitLinesToggle;
                case "mousereset":
                    return Program.Binding[BindingType.Ship].MouseReset;
                case "headlooktoggle":
                    return Program.Binding[BindingType.Ship].HeadLookToggle;
                case "weaponcolourtoggle":
                    return Program.Binding[BindingType.Ship].WeaponColourToggle;
                case "setspeedminus100":
                    return Program.Binding[BindingType.Ship].SetSpeedMinus100;
                case "setspeed100":
                    return Program.Binding[BindingType.Ship].SetSpeed100;
                case "setspeedminus25":
                    return Program.Binding[BindingType.Ship].SetSpeedMinus25;
                case "setspeed25":
                    return Program.Binding[BindingType.Ship].SetSpeed25;
                case "setspeedminus50":
                    return Program.Binding[BindingType.Ship].SetSpeedMinus50;
                case "setspeed50":
                    return Program.Binding[BindingType.Ship].SetSpeed50;
                case "setspeedminus75":
                    return Program.Binding[BindingType.Ship].SetSpeedMinus75;
                case "setspeed75":
                    return Program.Binding[BindingType.Ship].SetSpeed75;
                case "setspeedzero":
                    return Program.Binding[BindingType.Ship].SetSpeedZero;
                case "usealternateflightvaluestoggle":
                    return Program.Binding[BindingType.Ship].UseAlternateFlightValuesToggle;
                case "useboostjuice":
                    return Program.Binding[BindingType.Ship].UseBoostJuice;
                case "forwardkey":
                    return Program.Binding[BindingType.Ship].ForwardKey;
                case "forwardthrustbutton":
                    return Program.Binding[BindingType.Ship].ForwardThrustButton;
                case "forwardthrustbutton_landing":
                    return Program.Binding[BindingType.Ship].ForwardThrustButton_Landing;
                case "targetnextroutesystem":
                    return Program.Binding[BindingType.Ship].TargetNextRouteSystem;
                case "pitchdownbutton":
                    return Program.Binding[BindingType.Ship].PitchDownButton;
                case "pitchdownbutton_landing":
                    return Program.Binding[BindingType.Ship].PitchDownButton_Landing;
                case "pitchupbutton":
                    return Program.Binding[BindingType.Ship].PitchUpButton;
                case "pitchupbutton_landing":
                    return Program.Binding[BindingType.Ship].PitchUpButton_Landing;
                case "togglereversethrottleinput":
                    return Program.Binding[BindingType.Ship].ToggleReverseThrottleInput;
                case "backwardkey":
                    return Program.Binding[BindingType.Ship].BackwardKey;
                case "backwardthrustbutton":
                    return Program.Binding[BindingType.Ship].BackwardThrustButton;
                case "backwardthrustbutton_landing":
                    return Program.Binding[BindingType.Ship].BackwardThrustButton_Landing;
                case "rollleftbutton":
                    return Program.Binding[BindingType.Ship].RollLeftButton;
                case "rollleftbutton_landing":
                    return Program.Binding[BindingType.Ship].RollLeftButton_Landing;
                case "rollrightbutton":
                    return Program.Binding[BindingType.Ship].RollRightButton;
                case "rollrightbutton_landing":
                    return Program.Binding[BindingType.Ship].RollRightButton_Landing;
                case "disablerotationcorrecttoggle":
                    return Program.Binding[BindingType.Ship].DisableRotationCorrectToggle;
                case "downthrustbutton":
                    return Program.Binding[BindingType.Ship].DownThrustButton;
                case "downthrustbutton_landing":
                    return Program.Binding[BindingType.Ship].DownThrustButton_Landing;
                case "leftthrustbutton":
                    return Program.Binding[BindingType.Ship].LeftThrustButton;
                case "leftthrustbutton_landing":
                    return Program.Binding[BindingType.Ship].LeftThrustButton_Landing;
                case "rightthrustbutton":
                    return Program.Binding[BindingType.Ship].RightThrustButton;
                case "rightthrustbutton_landing":
                    return Program.Binding[BindingType.Ship].RightThrustButton_Landing;
                case "upthrustbutton":
                    return Program.Binding[BindingType.Ship].UpThrustButton;
                case "upthrustbutton_landing":
                    return Program.Binding[BindingType.Ship].UpThrustButton_Landing;
                case "yawleftbutton":
                    return Program.Binding[BindingType.Ship].YawLeftButton;
                case "yawleftbutton_landing":
                    return Program.Binding[BindingType.Ship].YawLeftButton_Landing;
                case "yawrightbutton":
                    return Program.Binding[BindingType.Ship].YawRightButton;
                case "yawrightbutton_landing":
                    return Program.Binding[BindingType.Ship].YawRightButton_Landing;
                case "yawtorollbutton":
                    return Program.Binding[BindingType.Ship].YawToRollButton;


                // general

                case "cyclenextpage":
                    return Program.Binding[BindingType.General].CycleNextPage;
                case "cyclenextpanel":
                    return Program.Binding[BindingType.General].CycleNextPanel;
                case "cyclepreviouspage":
                    return Program.Binding[BindingType.General].CyclePreviousPage;
                case "cyclepreviouspanel":
                    return Program.Binding[BindingType.General].CyclePreviousPanel;
                case "ui_back":
                    return Program.Binding[BindingType.General].UI_Back;
                case "ui_down":
                    return Program.Binding[BindingType.General].UI_Down;
                case "ui_left":
                    return Program.Binding[BindingType.General].UI_Left;
                case "ui_right":
                    return Program.Binding[BindingType.General].UI_Right;
                case "ui_select":
                    return Program.Binding[BindingType.General].UI_Select;
                case "ui_toggle":
                    return Program.Binding[BindingType.General].UI_Toggle;
                case "ui_up":
                    return Program.Binding[BindingType.General].UI_Up;

                case "camtranslatebackward":
                    return Program.Binding[BindingType.General].CamTranslateBackward;
                case "camtranslatedown":
                    return Program.Binding[BindingType.General].CamTranslateDown;
                case "camtranslateforward":
                    return Program.Binding[BindingType.General].CamTranslateForward;
                case "camtranslateleft":
                    return Program.Binding[BindingType.General].CamTranslateLeft;
                case "campitchdown":
                    return Program.Binding[BindingType.General].CamPitchDown;
                case "campitchup":
                    return Program.Binding[BindingType.General].CamPitchUp;
                case "camtranslateright":
                    return Program.Binding[BindingType.General].CamTranslateRight;
                case "camtranslateup":
                    return Program.Binding[BindingType.General].CamTranslateUp;
                case "camyawleft":
                    return Program.Binding[BindingType.General].CamYawLeft;
                case "camyawright":
                    return Program.Binding[BindingType.General].CamYawRight;
                case "camtranslatezhold":
                    return Program.Binding[BindingType.General].CamTranslateZHold;
                case "camzoomin":
                    return Program.Binding[BindingType.General].CamZoomIn;
                case "camzoomout":
                    return Program.Binding[BindingType.General].CamZoomOut;

                case "movefreecambackwards":
                    return Program.Binding[BindingType.General].MoveFreeCamBackwards;
                case "movefreecamdown":
                    return Program.Binding[BindingType.General].MoveFreeCamDown;
                case "movefreecamforward":
                    return Program.Binding[BindingType.General].MoveFreeCamForward;
                case "movefreecamleft":
                    return Program.Binding[BindingType.General].MoveFreeCamLeft;
                case "togglereversethrottleinputfreecam":
                    return Program.Binding[BindingType.General].ToggleReverseThrottleInputFreeCam;
                case "movefreecamright":
                    return Program.Binding[BindingType.General].MoveFreeCamRight;
                case "movefreecamup":
                    return Program.Binding[BindingType.General].MoveFreeCamUp;
                case "freecamspeeddec":
                    return Program.Binding[BindingType.General].FreeCamSpeedDec;
                case "togglefreecam":
                    return Program.Binding[BindingType.General].ToggleFreeCam;
                case "freecamspeedinc":
                    return Program.Binding[BindingType.General].FreeCamSpeedInc;
                case "freecamtogglehud":
                    return Program.Binding[BindingType.General].FreeCamToggleHUD;
                case "freecamzoomin":
                    return Program.Binding[BindingType.General].FreeCamZoomIn;
                case "freecamzoomout":
                    return Program.Binding[BindingType.General].FreeCamZoomOut;

                case "photocameratoggle":
                    return Program.Binding[BindingType.General].PhotoCameraToggle;
                case "storepitchcameradown":
                    return Program.Binding[BindingType.General].StorePitchCameraDown;
                case "storepitchcameraup":
                    return Program.Binding[BindingType.General].StorePitchCameraUp;
                case "storeenablerotation":
                    return Program.Binding[BindingType.General].StoreEnableRotation;
                case "storeyawcameraleft":
                    return Program.Binding[BindingType.General].StoreYawCameraLeft;
                case "storeyawcameraright":
                    return Program.Binding[BindingType.General].StoreYawCameraRight;
                case "storecamzoomin":
                    return Program.Binding[BindingType.General].StoreCamZoomIn;
                case "storecamzoomout":
                    return Program.Binding[BindingType.General].StoreCamZoomOut;
                case "storetoggle":
                    return Program.Binding[BindingType.General].StoreToggle;
                case "toggleadvancemode":
                    return Program.Binding[BindingType.General].ToggleAdvanceMode;
                case "vanitycameraeight":
                    return Program.Binding[BindingType.General].VanityCameraEight;
                case "vanitycameratwo":
                    return Program.Binding[BindingType.General].VanityCameraTwo;
                case "vanitycameraone":
                    return Program.Binding[BindingType.General].VanityCameraOne;
                case "vanitycamerathree":
                    return Program.Binding[BindingType.General].VanityCameraThree;
                case "vanitycamerafour":
                    return Program.Binding[BindingType.General].VanityCameraFour;
                case "vanitycamerafive":
                    return Program.Binding[BindingType.General].VanityCameraFive;
                case "vanitycamerasix":
                    return Program.Binding[BindingType.General].VanityCameraSix;
                case "vanitycameraseven":
                    return Program.Binding[BindingType.General].VanityCameraSeven;
                case "vanitycameranine":
                    return Program.Binding[BindingType.General].VanityCameraNine;
                case "pitchcameradown":
                    return Program.Binding[BindingType.General].PitchCameraDown;
                case "pitchcameraup":
                    return Program.Binding[BindingType.General].PitchCameraUp;
                case "rollcameraleft":
                    return Program.Binding[BindingType.General].RollCameraLeft;
                case "rollcameraright":
                    return Program.Binding[BindingType.General].RollCameraRight;
                case "togglerotationlock":
                    return Program.Binding[BindingType.General].ToggleRotationLock;
                case "yawcameraleft":
                    return Program.Binding[BindingType.General].YawCameraLeft;
                case "yawcameraright":
                    return Program.Binding[BindingType.General].YawCameraRight;
                case "fstopdec":
                    return Program.Binding[BindingType.General].FStopDec;
                case "quitcamera":
                    return Program.Binding[BindingType.General].QuitCamera;
                case "focusdistanceinc":
                    return Program.Binding[BindingType.General].FocusDistanceInc;
                case "focusdistancedec":
                    return Program.Binding[BindingType.General].FocusDistanceDec;
                case "fstopinc":
                    return Program.Binding[BindingType.General].FStopInc;
                case "fixcamerarelativetoggle":
                    return Program.Binding[BindingType.General].FixCameraRelativeToggle;
                case "fixcameraworldtoggle":
                    return Program.Binding[BindingType.General].FixCameraWorldToggle;
                case "vanitycamerascrollright":
                    return Program.Binding[BindingType.General].VanityCameraScrollRight;
                case "vanitycamerascrollleft":
                    return Program.Binding[BindingType.General].VanityCameraScrollLeft;

                case "commandercreator_redo":
                    return Program.Binding[BindingType.General].CommanderCreator_Redo;
                case "commandercreator_rotation":
                    return Program.Binding[BindingType.General].CommanderCreator_Rotation;
                case "commandercreator_rotation_mousetoggle":
                    return Program.Binding[BindingType.General].CommanderCreator_Rotation_MouseToggle;
                case "commandercreator_undo":
                    return Program.Binding[BindingType.General].CommanderCreator_Undo;

                case "galnetaudio_clearqueue":
                    return Program.Binding[BindingType.General].GalnetAudio_ClearQueue;
                case "galnetaudio_skipforward":
                    return Program.Binding[BindingType.General].GalnetAudio_SkipForward;
                case "galnetaudio_play_pause":
                    return Program.Binding[BindingType.General].GalnetAudio_Play_Pause;
                case "galnetaudio_skipbackward":
                    return Program.Binding[BindingType.General].GalnetAudio_SkipBackward;

                // in srv

                case "steerleftbutton":
                    return Program.Binding[BindingType.Srv].SteerLeftButton;
                case "steerrightbutton":
                    return Program.Binding[BindingType.Srv].SteerRightButton;
                case "increasespeedbuttonmax":
                    return Program.Binding[BindingType.Srv].IncreaseSpeedButtonMax;
                case "decreasespeedbuttonmax":
                    return Program.Binding[BindingType.Srv].DecreaseSpeedButtonMax;
                case "increasespeedbuttonpartial":
                    return Program.Binding[BindingType.Srv].IncreaseSpeedButtonPartial;
                case "decreasespeedbuttonpartial":
                    return Program.Binding[BindingType.Srv].DecreaseSpeedButtonPartial;
                case "recalldismissship":
                    return Program.Binding[BindingType.Srv].RecallDismissShip;
                case "verticalthrustersbutton":
                    return Program.Binding[BindingType.Srv].VerticalThrustersButton;

                case "photocameratoggle_buggy":
                    return Program.Binding[BindingType.Srv].PhotoCameraToggle_Buggy;
                case "ejectallcargo_buggy":
                    return Program.Binding[BindingType.Srv].EjectAllCargo_Buggy;
                case "quickcommspanel_buggy":
                    return Program.Binding[BindingType.Srv].QuickCommsPanel_Buggy;
                case "headlooktoggle_buggy":
                    return Program.Binding[BindingType.Srv].HeadLookToggle_Buggy;
                case "uifocus_buggy":
                    return Program.Binding[BindingType.Srv].UIFocus_Buggy;
                case "buggyprimaryfirebutton":
                    return Program.Binding[BindingType.Srv].BuggyPrimaryFireButton;
                case "buggysecondaryfirebutton":
                    return Program.Binding[BindingType.Srv].BuggySecondaryFireButton;
                case "selecttarget_buggy":
                    return Program.Binding[BindingType.Srv].SelectTarget_Buggy;
                case "buggyturretpitchdownbutton":
                    return Program.Binding[BindingType.Srv].BuggyTurretPitchDownButton;
                case "buggyturretyawleftbutton":
                    return Program.Binding[BindingType.Srv].BuggyTurretYawLeftButton;
                case "buggyturretyawrightbutton":
                    return Program.Binding[BindingType.Srv].BuggyTurretYawRightButton;
                case "buggyturretpitchupbutton":
                    return Program.Binding[BindingType.Srv].BuggyTurretPitchUpButton;
                case "headlightsbuggybutton":
                    return Program.Binding[BindingType.Srv].HeadlightsBuggyButton;
                case "buggypitchdownbutton":
                    return Program.Binding[BindingType.Srv].BuggyPitchDownButton;
                case "buggypitchupbutton":
                    return Program.Binding[BindingType.Srv].BuggyPitchUpButton;
                case "buggytogglereversethrottleinput":
                    return Program.Binding[BindingType.Srv].BuggyToggleReverseThrottleInput;
                case "buggyrollleft":
                    return Program.Binding[BindingType.Srv].BuggyRollLeft;
                case "buggyrollleftbutton":
                    return Program.Binding[BindingType.Srv].BuggyRollLeftButton;
                case "buggyrollright":
                    return Program.Binding[BindingType.Srv].BuggyRollRight;
                case "buggyrollrightbutton":
                    return Program.Binding[BindingType.Srv].BuggyRollRightButton;

                // on foot

                case "humanoidclearauthoritylevel":
                    return Program.Binding[BindingType.OnFoot].HumanoidClearAuthorityLevel;
                case "humanoidhealthpack":
                    return Program.Binding[BindingType.OnFoot].HumanoidHealthPack;
                case "humanoidbattery":
                    return Program.Binding[BindingType.OnFoot].HumanoidBattery;
                case "humanoidselectfraggrenade":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectFragGrenade;
                case "humanoidselectempgrenade":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectEMPGrenade;
                case "humanoidselectshieldgrenade":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectShieldGrenade;

                case "photocameratoggle_humanoid":
                    return Program.Binding[BindingType.OnFoot].PhotoCameraToggle_Humanoid;
                case "humanoidforwardbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidForwardButton;
                case "humanoidbackwardbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidBackwardButton;
                case "humanoidstrafeleftbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidStrafeLeftButton;
                case "humanoidstraferightbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidStrafeRightButton;
                case "humanoidsprintbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSprintButton;
                case "humanoidcrouchbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidCrouchButton;
                case "humanoidjumpbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidJumpButton;
                case "humanoidprimaryinteractbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidPrimaryInteractButton;
                case "humanoiditemwheelbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidItemWheelButton;
                case "humanoidprimaryfirebutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidPrimaryFireButton;
                case "humanoidselectprimaryweaponbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectPrimaryWeaponButton;
                case "humanoidselectsecondaryweaponbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectSecondaryWeaponButton;
                case "humanoidhideweaponbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidHideWeaponButton;
                case "humanoidzoombutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidZoomButton;
                case "humanoidreloadbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidReloadButton;
                case "humanoidthrowgrenadebutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidThrowGrenadeButton;
                case "humanoidmeleebutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidMeleeButton;
                case "humanoidopenaccesspanelbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidOpenAccessPanelButton;
                case "humanoidsecondaryinteractbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSecondaryInteractButton;
                case "humanoidswitchtorechargetool":
                    return Program.Binding[BindingType.OnFoot].HumanoidSwitchToRechargeTool;
                case "humanoidswitchtocompanalyser":
                    return Program.Binding[BindingType.OnFoot].HumanoidSwitchToCompAnalyser;
                case "humanoidtoggletoolmodebutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidToggleToolModeButton;
                case "humanoidswitchtosuittool":
                    return Program.Binding[BindingType.OnFoot].HumanoidSwitchToSuitTool;
                case "quickcommspanel_humanoid":
                    return Program.Binding[BindingType.OnFoot].QuickCommsPanel_Humanoid;
                case "humanoidconflictcontextualuibutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidConflictContextualUIButton;
                case "humanoidtoggleshieldsbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidToggleShieldsButton;

                case "humanoidrotateleftbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidRotateLeftButton;
                case "humanoidrotaterightbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidRotateRightButton;
                case "humanoidpitchupbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidPitchUpButton;
                case "humanoidpitchdownbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidPitchDownButton;
                case "humanoidswitchweapon":
                    return Program.Binding[BindingType.OnFoot].HumanoidSwitchWeapon;
                case "humanoidselectutilityweaponbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectUtilityWeaponButton;
                case "humanoidselectnextweaponbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectNextWeaponButton;
                case "humanoidselectpreviousweaponbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectPreviousWeaponButton;
                case "humanoidselectnextgrenadetypebutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectNextGrenadeTypeButton;
                case "humanoidselectpreviousgrenadetypebutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidSelectPreviousGrenadeTypeButton;
                case "humanoidtogglemissionhelppanelbutton":
                    return Program.Binding[BindingType.OnFoot].HumanoidToggleMissionHelpPanelButton;

            }

            return null;
        }

        private void ButtonClick()
        {
            var focusChange = NavigationManager.Uri.Contains("127.0.0.1");

            if (focusChange)
            {
                InteropMouse.JsMouseUp();

                Thread.Sleep(100);
            }

            Program.PlaySound(ref _clickSound, ButtonData.ClickSound);

            var binding = GetStaticButtonBinding(ButtonData.Function.ToLower());

            if (binding?.Primary.Device == "Keyboard" || binding?.Secondary.Device == "Keyboard")
            {
                CommandTools.SendKeypressQueue(binding,focusChange);
            }
        }
    }

}
