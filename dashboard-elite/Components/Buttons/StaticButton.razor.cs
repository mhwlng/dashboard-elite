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
                    return Common.Binding[BindingType.Ship].OrderFocusTarget;
                case "orderaggressivebehaviour":
                    return Common.Binding[BindingType.Ship].OrderAggressiveBehaviour;
                case "orderdefensivebehaviour":
                    return Common.Binding[BindingType.Ship].OrderDefensiveBehaviour;
                case "openorders":
                    return Common.Binding[BindingType.Ship].OpenOrders;
                case "orderrequestdock":
                    return Common.Binding[BindingType.Ship].OrderRequestDock;
                case "orderfollow":
                    return Common.Binding[BindingType.Ship].OrderFollow;
                case "orderholdfire":
                    return Common.Binding[BindingType.Ship].OrderHoldFire;
                case "orderholdposition":
                    return Common.Binding[BindingType.Ship].OrderHoldPosition;

                case "headlookpitchdown":
                    return Common.Binding[BindingType.Ship].HeadLookPitchDown;
                case "headlookyawleft":
                    return Common.Binding[BindingType.Ship].HeadLookYawLeft;
                case "headlookyawright":
                    return Common.Binding[BindingType.Ship].HeadLookYawRight;
                case "headlookpitchup":
                    return Common.Binding[BindingType.Ship].HeadLookPitchUp;
                case "headlookreset":
                    return Common.Binding[BindingType.Ship].HeadLookReset;
                case "opencodexgotodiscovery":
                    return Common.Binding[BindingType.Ship].OpenCodexGoToDiscovery;
                case "friendsmenu":
                    return Common.Binding[BindingType.Ship].FriendsMenu;
                case "pause":
                    return Common.Binding[BindingType.Ship].Pause;
                case "microphonemute":
                    return Common.Binding[BindingType.Ship].MicrophoneMute;

                case "hmdreset":
                    return Common.Binding[BindingType.Ship].HMDReset;
                case "oculusreset":
                    return Common.Binding[BindingType.Ship].OculusReset;
                case "radardecreaserange":
                    return Common.Binding[BindingType.Ship].RadarDecreaseRange;
                case "radarincreaserange":
                    return Common.Binding[BindingType.Ship].RadarIncreaseRange;
                case "multicrewthirdpersonfovinbutton":
                    return Common.Binding[BindingType.Ship].MultiCrewThirdPersonFovInButton;
                case "multicrewthirdpersonfovoutbutton":
                    return Common.Binding[BindingType.Ship].MultiCrewThirdPersonFovOutButton;
                case "multicrewprimaryfire":
                    return Common.Binding[BindingType.Ship].MultiCrewPrimaryFire;
                case "multicrewsecondaryfire":
                    return Common.Binding[BindingType.Ship].MultiCrewSecondaryFire;
                case "multicrewtogglemode":
                    return Common.Binding[BindingType.Ship].MultiCrewToggleMode;
                case "multicrewthirdpersonpitchdownbutton":
                    return Common.Binding[BindingType.Ship].MultiCrewThirdPersonPitchDownButton;
                case "multicrewthirdpersonpitchupbutton":
                    return Common.Binding[BindingType.Ship].MultiCrewThirdPersonPitchUpButton;
                case "multicrewprimaryutilityfire":
                    return Common.Binding[BindingType.Ship].MultiCrewPrimaryUtilityFire;
                case "multicrewsecondaryutilityfire":
                    return Common.Binding[BindingType.Ship].MultiCrewSecondaryUtilityFire;
                case "multicrewcockpituicyclebackward":
                    return Common.Binding[BindingType.Ship].MultiCrewCockpitUICycleBackward;
                case "multicrewcockpituicycleforward":
                    return Common.Binding[BindingType.Ship].MultiCrewCockpitUICycleForward;
                case "multicrewthirdpersonyawleftbutton":
                    return Common.Binding[BindingType.Ship].MultiCrewThirdPersonYawLeftButton;
                case "multicrewthirdpersonyawrightbutton":
                    return Common.Binding[BindingType.Ship].MultiCrewThirdPersonYawRightButton;
                case "saathirdpersonfovinbutton":
                    return Common.Binding[BindingType.Ship].SAAThirdPersonFovInButton;
                case "saathirdpersonfovoutbutton":
                    return Common.Binding[BindingType.Ship].SAAThirdPersonFovOutButton;
                case "explorationfssenter":
                    return Common.Binding[BindingType.Ship].ExplorationFSSEnter;
                case "explorationsaaexitthirdperson":
                    return Common.Binding[BindingType.Ship].ExplorationSAAExitThirdPerson;
                case "explorationfssquit":
                    return Common.Binding[BindingType.Ship].ExplorationFSSQuit;
                case "explorationfssshowhelp":
                    return Common.Binding[BindingType.Ship].ExplorationFSSShowHelp;
                case "explorationsaanextgenus":
                    return Common.Binding[BindingType.Ship].ExplorationSAANextGenus;
                case "explorationsaapreviousgenus":
                    return Common.Binding[BindingType.Ship].ExplorationSAAPreviousGenus;
                case "explorationfssdiscoveryscan":
                    return Common.Binding[BindingType.Ship].ExplorationFSSDiscoveryScan;
                case "explorationfsscamerapitchdecreasebutton":
                    return Common.Binding[BindingType.Ship].ExplorationFSSCameraPitchDecreaseButton;
                case "explorationfsscamerapitchincreasebutton":
                    return Common.Binding[BindingType.Ship].ExplorationFSSCameraPitchIncreaseButton;
                case "explorationfssradiotuningx_decrease":
                    return Common.Binding[BindingType.Ship].ExplorationFSSRadioTuningX_Decrease;
                case "explorationfssradiotuningx_increase":
                    return Common.Binding[BindingType.Ship].ExplorationFSSRadioTuningX_Increase;
                case "explorationfsscamerayawdecreasebutton":
                    return Common.Binding[BindingType.Ship].ExplorationFSSCameraYawDecreaseButton;
                case "explorationfsscamerayawincreasebutton":
                    return Common.Binding[BindingType.Ship].ExplorationFSSCameraYawIncreaseButton;
                case "saathirdpersonpitchdownbutton":
                    return Common.Binding[BindingType.Ship].SAAThirdPersonPitchDownButton;
                case "saathirdpersonpitchupbutton":
                    return Common.Binding[BindingType.Ship].SAAThirdPersonPitchUpButton;
                case "explorationfssminizoomin":
                    return Common.Binding[BindingType.Ship].ExplorationFSSMiniZoomIn;
                case "explorationfssminizoomout":
                    return Common.Binding[BindingType.Ship].ExplorationFSSMiniZoomOut;
                case "explorationfsstarget":
                    return Common.Binding[BindingType.Ship].ExplorationFSSTarget;
                case "explorationsaachangescannedareaviewtoggle":
                    return Common.Binding[BindingType.Ship].ExplorationSAAChangeScannedAreaViewToggle;
                case "saathirdpersonyawleftbutton":
                    return Common.Binding[BindingType.Ship].SAAThirdPersonYawLeftButton;
                case "saathirdpersonyawrightbutton":
                    return Common.Binding[BindingType.Ship].SAAThirdPersonYawRightButton;
                case "explorationfsszoomin":
                    return Common.Binding[BindingType.Ship].ExplorationFSSZoomIn;
                case "explorationfsszoomout":
                    return Common.Binding[BindingType.Ship].ExplorationFSSZoomOut;
                case "quickcommspanel":
                    return Common.Binding[BindingType.Ship].QuickCommsPanel;
                case "uifocus":
                    return Common.Binding[BindingType.Ship].UIFocus;
                case "targetwingman0":
                    return Common.Binding[BindingType.Ship].TargetWingman0;
                case "targetwingman1":
                    return Common.Binding[BindingType.Ship].TargetWingman1;
                case "targetwingman2":
                    return Common.Binding[BindingType.Ship].TargetWingman2;
                case "wingnavlock":
                    return Common.Binding[BindingType.Ship].WingNavLock;
                case "selecttargetstarget":
                    return Common.Binding[BindingType.Ship].SelectTargetsTarget;
                case "firechafflauncher":
                    return Common.Binding[BindingType.Ship].FireChaffLauncher;
                case "chargeecm":
                    return Common.Binding[BindingType.Ship].ChargeECM;
                case "primaryfire":
                    return Common.Binding[BindingType.Ship].PrimaryFire;
                case "secondaryfire":
                    return Common.Binding[BindingType.Ship].SecondaryFire;
                case "cyclenexttarget":
                    return Common.Binding[BindingType.Ship].CycleNextTarget;
                case "cyclefiregroupnext":
                    return Common.Binding[BindingType.Ship].CycleFireGroupNext;
                case "cyclenexthostiletarget":
                    return Common.Binding[BindingType.Ship].CycleNextHostileTarget;
                case "cyclenextsubsystem":
                    return Common.Binding[BindingType.Ship].CycleNextSubsystem;
                case "cycleprevioustarget":
                    return Common.Binding[BindingType.Ship].CyclePreviousTarget;
                case "cyclefiregroupprevious":
                    return Common.Binding[BindingType.Ship].CycleFireGroupPrevious;
                case "cycleprevioushostiletarget":
                    return Common.Binding[BindingType.Ship].CyclePreviousHostileTarget;
                case "cycleprevioussubsystem":
                    return Common.Binding[BindingType.Ship].CyclePreviousSubsystem;
                case "useshieldcell":
                    return Common.Binding[BindingType.Ship].UseShieldCell;
                case "selecttarget":
                    return Common.Binding[BindingType.Ship].SelectTarget;
                case "showpgscoresummaryinput":
                    return Common.Binding[BindingType.Ship].ShowPGScoreSummaryInput;
                case "ejectallcargo":
                    return Common.Binding[BindingType.Ship].EjectAllCargo;
                case "enginecolourtoggle":
                    return Common.Binding[BindingType.Ship].EngineColourToggle;
                case "orbitlinestoggle":
                    return Common.Binding[BindingType.Ship].OrbitLinesToggle;
                case "mousereset":
                    return Common.Binding[BindingType.Ship].MouseReset;
                case "headlooktoggle":
                    return Common.Binding[BindingType.Ship].HeadLookToggle;
                case "weaponcolourtoggle":
                    return Common.Binding[BindingType.Ship].WeaponColourToggle;
                case "setspeedminus100":
                    return Common.Binding[BindingType.Ship].SetSpeedMinus100;
                case "setspeed100":
                    return Common.Binding[BindingType.Ship].SetSpeed100;
                case "setspeedminus25":
                    return Common.Binding[BindingType.Ship].SetSpeedMinus25;
                case "setspeed25":
                    return Common.Binding[BindingType.Ship].SetSpeed25;
                case "setspeedminus50":
                    return Common.Binding[BindingType.Ship].SetSpeedMinus50;
                case "setspeed50":
                    return Common.Binding[BindingType.Ship].SetSpeed50;
                case "setspeedminus75":
                    return Common.Binding[BindingType.Ship].SetSpeedMinus75;
                case "setspeed75":
                    return Common.Binding[BindingType.Ship].SetSpeed75;
                case "setspeedzero":
                    return Common.Binding[BindingType.Ship].SetSpeedZero;
                case "usealternateflightvaluestoggle":
                    return Common.Binding[BindingType.Ship].UseAlternateFlightValuesToggle;
                case "useboostjuice":
                    return Common.Binding[BindingType.Ship].UseBoostJuice;
                case "forwardkey":
                    return Common.Binding[BindingType.Ship].ForwardKey;
                case "forwardthrustbutton":
                    return Common.Binding[BindingType.Ship].ForwardThrustButton;
                case "forwardthrustbutton_landing":
                    return Common.Binding[BindingType.Ship].ForwardThrustButton_Landing;
                case "targetnextroutesystem":
                    return Common.Binding[BindingType.Ship].TargetNextRouteSystem;
                case "pitchdownbutton":
                    return Common.Binding[BindingType.Ship].PitchDownButton;
                case "pitchdownbutton_landing":
                    return Common.Binding[BindingType.Ship].PitchDownButton_Landing;
                case "pitchupbutton":
                    return Common.Binding[BindingType.Ship].PitchUpButton;
                case "pitchupbutton_landing":
                    return Common.Binding[BindingType.Ship].PitchUpButton_Landing;
                case "togglereversethrottleinput":
                    return Common.Binding[BindingType.Ship].ToggleReverseThrottleInput;
                case "backwardkey":
                    return Common.Binding[BindingType.Ship].BackwardKey;
                case "backwardthrustbutton":
                    return Common.Binding[BindingType.Ship].BackwardThrustButton;
                case "backwardthrustbutton_landing":
                    return Common.Binding[BindingType.Ship].BackwardThrustButton_Landing;
                case "rollleftbutton":
                    return Common.Binding[BindingType.Ship].RollLeftButton;
                case "rollleftbutton_landing":
                    return Common.Binding[BindingType.Ship].RollLeftButton_Landing;
                case "rollrightbutton":
                    return Common.Binding[BindingType.Ship].RollRightButton;
                case "rollrightbutton_landing":
                    return Common.Binding[BindingType.Ship].RollRightButton_Landing;
                case "disablerotationcorrecttoggle":
                    return Common.Binding[BindingType.Ship].DisableRotationCorrectToggle;
                case "downthrustbutton":
                    return Common.Binding[BindingType.Ship].DownThrustButton;
                case "downthrustbutton_landing":
                    return Common.Binding[BindingType.Ship].DownThrustButton_Landing;
                case "leftthrustbutton":
                    return Common.Binding[BindingType.Ship].LeftThrustButton;
                case "leftthrustbutton_landing":
                    return Common.Binding[BindingType.Ship].LeftThrustButton_Landing;
                case "rightthrustbutton":
                    return Common.Binding[BindingType.Ship].RightThrustButton;
                case "rightthrustbutton_landing":
                    return Common.Binding[BindingType.Ship].RightThrustButton_Landing;
                case "upthrustbutton":
                    return Common.Binding[BindingType.Ship].UpThrustButton;
                case "upthrustbutton_landing":
                    return Common.Binding[BindingType.Ship].UpThrustButton_Landing;
                case "yawleftbutton":
                    return Common.Binding[BindingType.Ship].YawLeftButton;
                case "yawleftbutton_landing":
                    return Common.Binding[BindingType.Ship].YawLeftButton_Landing;
                case "yawrightbutton":
                    return Common.Binding[BindingType.Ship].YawRightButton;
                case "yawrightbutton_landing":
                    return Common.Binding[BindingType.Ship].YawRightButton_Landing;
                case "yawtorollbutton":
                    return Common.Binding[BindingType.Ship].YawToRollButton;


                // general

                case "cyclenextpage":
                    return Common.Binding[BindingType.General].CycleNextPage;
                case "cyclenextpanel":
                    return Common.Binding[BindingType.General].CycleNextPanel;
                case "cyclepreviouspage":
                    return Common.Binding[BindingType.General].CyclePreviousPage;
                case "cyclepreviouspanel":
                    return Common.Binding[BindingType.General].CyclePreviousPanel;
                case "ui_back":
                    return Common.Binding[BindingType.General].UI_Back;
                case "ui_down":
                    return Common.Binding[BindingType.General].UI_Down;
                case "ui_left":
                    return Common.Binding[BindingType.General].UI_Left;
                case "ui_right":
                    return Common.Binding[BindingType.General].UI_Right;
                case "ui_select":
                    return Common.Binding[BindingType.General].UI_Select;
                case "ui_toggle":
                    return Common.Binding[BindingType.General].UI_Toggle;
                case "ui_up":
                    return Common.Binding[BindingType.General].UI_Up;

                case "camtranslatebackward":
                    return Common.Binding[BindingType.General].CamTranslateBackward;
                case "camtranslatedown":
                    return Common.Binding[BindingType.General].CamTranslateDown;
                case "camtranslateforward":
                    return Common.Binding[BindingType.General].CamTranslateForward;
                case "camtranslateleft":
                    return Common.Binding[BindingType.General].CamTranslateLeft;
                case "campitchdown":
                    return Common.Binding[BindingType.General].CamPitchDown;
                case "campitchup":
                    return Common.Binding[BindingType.General].CamPitchUp;
                case "camtranslateright":
                    return Common.Binding[BindingType.General].CamTranslateRight;
                case "camtranslateup":
                    return Common.Binding[BindingType.General].CamTranslateUp;
                case "camyawleft":
                    return Common.Binding[BindingType.General].CamYawLeft;
                case "camyawright":
                    return Common.Binding[BindingType.General].CamYawRight;
                case "camtranslatezhold":
                    return Common.Binding[BindingType.General].CamTranslateZHold;
                case "camzoomin":
                    return Common.Binding[BindingType.General].CamZoomIn;
                case "camzoomout":
                    return Common.Binding[BindingType.General].CamZoomOut;

                case "movefreecambackwards":
                    return Common.Binding[BindingType.General].MoveFreeCamBackwards;
                case "movefreecamdown":
                    return Common.Binding[BindingType.General].MoveFreeCamDown;
                case "movefreecamforward":
                    return Common.Binding[BindingType.General].MoveFreeCamForward;
                case "movefreecamleft":
                    return Common.Binding[BindingType.General].MoveFreeCamLeft;
                case "togglereversethrottleinputfreecam":
                    return Common.Binding[BindingType.General].ToggleReverseThrottleInputFreeCam;
                case "movefreecamright":
                    return Common.Binding[BindingType.General].MoveFreeCamRight;
                case "movefreecamup":
                    return Common.Binding[BindingType.General].MoveFreeCamUp;
                case "freecamspeeddec":
                    return Common.Binding[BindingType.General].FreeCamSpeedDec;
                case "togglefreecam":
                    return Common.Binding[BindingType.General].ToggleFreeCam;
                case "freecamspeedinc":
                    return Common.Binding[BindingType.General].FreeCamSpeedInc;
                case "freecamtogglehud":
                    return Common.Binding[BindingType.General].FreeCamToggleHUD;
                case "freecamzoomin":
                    return Common.Binding[BindingType.General].FreeCamZoomIn;
                case "freecamzoomout":
                    return Common.Binding[BindingType.General].FreeCamZoomOut;

                case "photocameratoggle":
                    return Common.Binding[BindingType.General].PhotoCameraToggle;
                case "storepitchcameradown":
                    return Common.Binding[BindingType.General].StorePitchCameraDown;
                case "storepitchcameraup":
                    return Common.Binding[BindingType.General].StorePitchCameraUp;
                case "storeenablerotation":
                    return Common.Binding[BindingType.General].StoreEnableRotation;
                case "storeyawcameraleft":
                    return Common.Binding[BindingType.General].StoreYawCameraLeft;
                case "storeyawcameraright":
                    return Common.Binding[BindingType.General].StoreYawCameraRight;
                case "storecamzoomin":
                    return Common.Binding[BindingType.General].StoreCamZoomIn;
                case "storecamzoomout":
                    return Common.Binding[BindingType.General].StoreCamZoomOut;
                case "storetoggle":
                    return Common.Binding[BindingType.General].StoreToggle;
                case "toggleadvancemode":
                    return Common.Binding[BindingType.General].ToggleAdvanceMode;
                case "vanitycameraeight":
                    return Common.Binding[BindingType.General].VanityCameraEight;
                case "vanitycameratwo":
                    return Common.Binding[BindingType.General].VanityCameraTwo;
                case "vanitycameraone":
                    return Common.Binding[BindingType.General].VanityCameraOne;
                case "vanitycamerathree":
                    return Common.Binding[BindingType.General].VanityCameraThree;
                case "vanitycamerafour":
                    return Common.Binding[BindingType.General].VanityCameraFour;
                case "vanitycamerafive":
                    return Common.Binding[BindingType.General].VanityCameraFive;
                case "vanitycamerasix":
                    return Common.Binding[BindingType.General].VanityCameraSix;
                case "vanitycameraseven":
                    return Common.Binding[BindingType.General].VanityCameraSeven;
                case "vanitycameranine":
                    return Common.Binding[BindingType.General].VanityCameraNine;
                case "pitchcameradown":
                    return Common.Binding[BindingType.General].PitchCameraDown;
                case "pitchcameraup":
                    return Common.Binding[BindingType.General].PitchCameraUp;
                case "rollcameraleft":
                    return Common.Binding[BindingType.General].RollCameraLeft;
                case "rollcameraright":
                    return Common.Binding[BindingType.General].RollCameraRight;
                case "togglerotationlock":
                    return Common.Binding[BindingType.General].ToggleRotationLock;
                case "yawcameraleft":
                    return Common.Binding[BindingType.General].YawCameraLeft;
                case "yawcameraright":
                    return Common.Binding[BindingType.General].YawCameraRight;
                case "fstopdec":
                    return Common.Binding[BindingType.General].FStopDec;
                case "quitcamera":
                    return Common.Binding[BindingType.General].QuitCamera;
                case "focusdistanceinc":
                    return Common.Binding[BindingType.General].FocusDistanceInc;
                case "focusdistancedec":
                    return Common.Binding[BindingType.General].FocusDistanceDec;
                case "fstopinc":
                    return Common.Binding[BindingType.General].FStopInc;
                case "fixcamerarelativetoggle":
                    return Common.Binding[BindingType.General].FixCameraRelativeToggle;
                case "fixcameraworldtoggle":
                    return Common.Binding[BindingType.General].FixCameraWorldToggle;
                case "vanitycamerascrollright":
                    return Common.Binding[BindingType.General].VanityCameraScrollRight;
                case "vanitycamerascrollleft":
                    return Common.Binding[BindingType.General].VanityCameraScrollLeft;

                case "commandercreator_redo":
                    return Common.Binding[BindingType.General].CommanderCreator_Redo;
                case "commandercreator_rotation":
                    return Common.Binding[BindingType.General].CommanderCreator_Rotation;
                case "commandercreator_rotation_mousetoggle":
                    return Common.Binding[BindingType.General].CommanderCreator_Rotation_MouseToggle;
                case "commandercreator_undo":
                    return Common.Binding[BindingType.General].CommanderCreator_Undo;

                case "galnetaudio_clearqueue":
                    return Common.Binding[BindingType.General].GalnetAudio_ClearQueue;
                case "galnetaudio_skipforward":
                    return Common.Binding[BindingType.General].GalnetAudio_SkipForward;
                case "galnetaudio_play_pause":
                    return Common.Binding[BindingType.General].GalnetAudio_Play_Pause;
                case "galnetaudio_skipbackward":
                    return Common.Binding[BindingType.General].GalnetAudio_SkipBackward;

                // in srv

                case "steerleftbutton":
                    return Common.Binding[BindingType.Srv].SteerLeftButton;
                case "steerrightbutton":
                    return Common.Binding[BindingType.Srv].SteerRightButton;
                case "increasespeedbuttonmax":
                    return Common.Binding[BindingType.Srv].IncreaseSpeedButtonMax;
                case "decreasespeedbuttonmax":
                    return Common.Binding[BindingType.Srv].DecreaseSpeedButtonMax;
                case "increasespeedbuttonpartial":
                    return Common.Binding[BindingType.Srv].IncreaseSpeedButtonPartial;
                case "decreasespeedbuttonpartial":
                    return Common.Binding[BindingType.Srv].DecreaseSpeedButtonPartial;
                case "recalldismissship":
                    return Common.Binding[BindingType.Srv].RecallDismissShip;
                case "verticalthrustersbutton":
                    return Common.Binding[BindingType.Srv].VerticalThrustersButton;

                case "photocameratoggle_buggy":
                    return Common.Binding[BindingType.Srv].PhotoCameraToggle_Buggy;
                case "ejectallcargo_buggy":
                    return Common.Binding[BindingType.Srv].EjectAllCargo_Buggy;
                case "quickcommspanel_buggy":
                    return Common.Binding[BindingType.Srv].QuickCommsPanel_Buggy;
                case "headlooktoggle_buggy":
                    return Common.Binding[BindingType.Srv].HeadLookToggle_Buggy;
                case "uifocus_buggy":
                    return Common.Binding[BindingType.Srv].UIFocus_Buggy;
                case "buggyprimaryfirebutton":
                    return Common.Binding[BindingType.Srv].BuggyPrimaryFireButton;
                case "buggysecondaryfirebutton":
                    return Common.Binding[BindingType.Srv].BuggySecondaryFireButton;
                case "selecttarget_buggy":
                    return Common.Binding[BindingType.Srv].SelectTarget_Buggy;
                case "buggyturretpitchdownbutton":
                    return Common.Binding[BindingType.Srv].BuggyTurretPitchDownButton;
                case "buggyturretyawleftbutton":
                    return Common.Binding[BindingType.Srv].BuggyTurretYawLeftButton;
                case "buggyturretyawrightbutton":
                    return Common.Binding[BindingType.Srv].BuggyTurretYawRightButton;
                case "buggyturretpitchupbutton":
                    return Common.Binding[BindingType.Srv].BuggyTurretPitchUpButton;
                case "headlightsbuggybutton":
                    return Common.Binding[BindingType.Srv].HeadlightsBuggyButton;
                case "buggypitchdownbutton":
                    return Common.Binding[BindingType.Srv].BuggyPitchDownButton;
                case "buggypitchupbutton":
                    return Common.Binding[BindingType.Srv].BuggyPitchUpButton;
                case "buggytogglereversethrottleinput":
                    return Common.Binding[BindingType.Srv].BuggyToggleReverseThrottleInput;
                case "buggyrollleft":
                    return Common.Binding[BindingType.Srv].BuggyRollLeft;
                case "buggyrollleftbutton":
                    return Common.Binding[BindingType.Srv].BuggyRollLeftButton;
                case "buggyrollright":
                    return Common.Binding[BindingType.Srv].BuggyRollRight;
                case "buggyrollrightbutton":
                    return Common.Binding[BindingType.Srv].BuggyRollRightButton;

                // on foot

                case "humanoidclearauthoritylevel":
                    return Common.Binding[BindingType.OnFoot].HumanoidClearAuthorityLevel;
                case "humanoidhealthpack":
                    return Common.Binding[BindingType.OnFoot].HumanoidHealthPack;
                case "humanoidbattery":
                    return Common.Binding[BindingType.OnFoot].HumanoidBattery;
                case "humanoidselectfraggrenade":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectFragGrenade;
                case "humanoidselectempgrenade":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectEMPGrenade;
                case "humanoidselectshieldgrenade":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectShieldGrenade;

                case "photocameratoggle_humanoid":
                    return Common.Binding[BindingType.OnFoot].PhotoCameraToggle_Humanoid;
                case "humanoidforwardbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidForwardButton;
                case "humanoidbackwardbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidBackwardButton;
                case "humanoidstrafeleftbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidStrafeLeftButton;
                case "humanoidstraferightbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidStrafeRightButton;
                case "humanoidsprintbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSprintButton;
                case "humanoidcrouchbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidCrouchButton;
                case "humanoidjumpbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidJumpButton;
                case "humanoidprimaryinteractbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidPrimaryInteractButton;
                case "humanoiditemwheelbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidItemWheelButton;
                case "humanoidprimaryfirebutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidPrimaryFireButton;
                case "humanoidselectprimaryweaponbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectPrimaryWeaponButton;
                case "humanoidselectsecondaryweaponbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectSecondaryWeaponButton;
                case "humanoidhideweaponbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidHideWeaponButton;
                case "humanoidzoombutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidZoomButton;
                case "humanoidreloadbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidReloadButton;
                case "humanoidthrowgrenadebutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidThrowGrenadeButton;
                case "humanoidmeleebutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidMeleeButton;
                case "humanoidopenaccesspanelbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidOpenAccessPanelButton;
                case "humanoidsecondaryinteractbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSecondaryInteractButton;
                case "humanoidswitchtorechargetool":
                    return Common.Binding[BindingType.OnFoot].HumanoidSwitchToRechargeTool;
                case "humanoidswitchtocompanalyser":
                    return Common.Binding[BindingType.OnFoot].HumanoidSwitchToCompAnalyser;
                case "humanoidtoggletoolmodebutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidToggleToolModeButton;
                case "humanoidswitchtosuittool":
                    return Common.Binding[BindingType.OnFoot].HumanoidSwitchToSuitTool;
                case "quickcommspanel_humanoid":
                    return Common.Binding[BindingType.OnFoot].QuickCommsPanel_Humanoid;
                case "humanoidconflictcontextualuibutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidConflictContextualUIButton;
                case "humanoidtoggleshieldsbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidToggleShieldsButton;

                case "humanoidrotateleftbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidRotateLeftButton;
                case "humanoidrotaterightbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidRotateRightButton;
                case "humanoidpitchupbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidPitchUpButton;
                case "humanoidpitchdownbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidPitchDownButton;
                case "humanoidswitchweapon":
                    return Common.Binding[BindingType.OnFoot].HumanoidSwitchWeapon;
                case "humanoidselectutilityweaponbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectUtilityWeaponButton;
                case "humanoidselectnextweaponbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectNextWeaponButton;
                case "humanoidselectpreviousweaponbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectPreviousWeaponButton;
                case "humanoidselectnextgrenadetypebutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectNextGrenadeTypeButton;
                case "humanoidselectpreviousgrenadetypebutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidSelectPreviousGrenadeTypeButton;
                case "humanoidtogglemissionhelppanelbutton":
                    return Common.Binding[BindingType.OnFoot].HumanoidToggleMissionHelpPanelButton;

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

            Common.PlaySound(ref _clickSound, ButtonData.ClickSound);

            var binding = GetStaticButtonBinding(ButtonData.Function.ToLower());

            if (binding?.Primary.Device == "Keyboard" || binding?.Secondary.Device == "Keyboard")
            {
                CommandTools.SendKeypressQueue(binding,focusChange);
            }
        }
    }

}
