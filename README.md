# dashboard-elite
Use a wide 'bar style' touch screen as a game dashboard (like [fip-panels](https://github.com/mhwlng/fip-elite)) and button box (like [streamdeck](https://github.com/mhwlng/streamdeck-elite)) for Elite Dangerous :


VERY UNFINISHED PRE-ALPHA RELEASE

![touch screen](https://i.imgur.com/RkHJESd.jpg)

![touch screen](https://i.imgur.com/QLA3Fgm.png)

![touch screen](https://i.imgur.com/dK75qt0.png)

Technology used:
- [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0/runtime)  choose : download x64
- [WebView2 Runtime](https://go.microsoft.com/fwlink/p/?LinkId=2124703)
- Blazor server

The touch screen is connected via hdmi as a secondary monitor.

So, this will NOT work on e.g. a tablet or a separate device.

The game has to be set to borderless mode. Fullscreen mode WON'T work.

The actual on/off state of a button comes from the game.

When pressing a button on the touch screen, it will send the e.g. 'toggle light' keypress (from the game keyboard binding file) to the game.

Big display 1920x515 (12.6 inch, IPS panel):

https://www.aliexpress.com/item/1005001966967133.html

Small display 1920x480 (8.8 inch, IPS panel):

https://www.aliexpress.com/item/1005003014364673.html

(Note, that these kinds of displays come with or without a touch controller and with or without a case)

When connecting these screens, the touch screen needs to be configured via : Control Panel \ Tablet PC settings \ Setup

![touch screen](https://i.imgur.com/S6Xy3NO.png)

To get similar button sizes on both displays, the buttons are arranged in a 8x4 grid on the big screen and a 10x3 grid on the small screen.

The button arrangement is customizable. (See the wwwroot\css\site.css file.)

The button images are SVG vector images from [Keath Milligan](https://keathmilligan.net/themeable-icon-pack-for-streamdeck-elite).

Button possibilities:
- Separate SVG button images for each state
- Animated SVG button images
- Multiple button profiles
- Automatic profile switching
- Button press sounds

The button functionality works, by changing the focus back to elite dangerous, when pressing a button on the touch screen.

This focus change mechanism is ONLY enabled, if the application runs full screen on the touch screen.

**Otherwise, touching a button on the touch screen will have NO EFFECT in the game.**

The easiest way to set this up, is to first drag the application to the touch screen area, then press the maximize button:

![touch screen](https://i.imgur.com/14qcSb1.png)

This will causes the application to restart and then the application should be full screen and the window title bar should no longer be visible.

To make the window title bar appear again, press the maximize button again.

The application window settings can be found in appsettings.json


The buttons will only work with keyboard bindings. 
So, when there is only a binding to a joystick / controller / mouse for a function, then you need to add a keyboard binding first.

The button profiles (i.e. the top row buttons) can be configured in the Data\profiles.json file :

```
"onfoot": {
"primaryIcon": "profile-onfoot.svg",
"secondaryIcon": "profile-onfoot.svg",
"clickSound": "beep-3.wav"
}
```

The name of each section refers to the name of the related button set .json file in the Data\ButtonBlocks directory.

An automatic profile switching mechanism will be used, in case button sets exist with these file names:

- galaxymap.json
- systemmap.json
- orrery.json
- fssmode.json
- saamode.json
- infighter.json
- srvturret.json
- insrv.json
- onfoot.json
- analysismode.json
- cargoscoop.json
- hardpoints.json

If none of the above profiles are activated, then a profile with the file name default.json will be activated.

Each button set is configured via a .json file in the Data\ButtonBlocks directory.

Button sounds can be found in the Sounds directory.

All the button SVG images can be found in the wwwroot\img\buttons directory. 

The button captions are inside the .svg files.

Animated SVG files are possible.

Various button type can be configured, each with their own set of parameters :

**ToggleButton**

```
{
"buttonType": "ToggleButton",
"function": "FocusLeftPanel",
"primaryIcon": "nav-panel.svg",
"secondaryIcon": "nav-panel.svg",
"clickSound": "beep-3.wav"
}
```

| Function               Description    
| --------------------------------------
| PlayerHUDModeToggle    Analysis Mode  
| ToggleCargoScoop       Cargo Scoop    
| ToggleFlightAssist     Flight Assist  
| GalaxyMap              Galaxy Map     
| DeployHardpointToggle  Hardpoints     
| LandingGearToggle      Landing Gear   
| ShipSpotLightToggle    Lights         
| NightVisionToggle      Night Vision   
| ToggleButtonUpInput    Silent Running 
| ToggleDriveAssist      Srv DriveAssist
| AutoBreakBuggyButton   Srv Handbrake  
| ToggleBuggyTurretButtonSrv Turret     
| Supercruise            Supercruise    
| SystemMap              System Map     
| FocusCommsPanel        Comms Panel    
| FocusLeftPanel         Nav Panel      
| FocusRadarPanel        Role Panel     
| FocusRightPanel        Systems Panel  

For Odyssey, when On Foot, the Galaxy Map,System Map,Lights & Night Vision buttons will call the on-foot key bindings, 
but there is no state feedback. So the button state won't change.

**AlarmButton**

```
{
"buttonType": "AlarmButton",
"function": "SelectHighestThreat",
"primaryIcon": "target-highest.svg",
"secondaryIcon": "target-highest.svg",
"clickSound": "beep-3.wav"
}
```

| Function           Description     Note                        
| ---------------------------------------------------------------
| SelectHighestThreatHighest Threat  alarm = under attack status 
| DeployHeatsink     Heat sink       alarm = under attack status 
| DeployChaff        Chaff           alarm = overheating status  
| DeployShieldCell   Shield Cell Bankalarm = shields down status. In that case DON'T fire a shield cell bank.  

**HyperspaceButton**

```
{
"buttonType": "HyperspaceButton",
"function": "HyperSuperCombination",
"primaryIcon": "hyperspace.svg",
"secondaryIcon": "hyperspace.svg",
"tertiaryIcon": "hyperspace.svg",
"clickSound": "",
"errorSound": ""
}
```

| Function             Description    Note                               
| -----------------------------------------------------------------------
| HyperSuperCombinationToggle FSD     also shows remaining jumps in route
| Supercruise          Supercruise                                       
| Hyperspace           Hyperspace Jumpalso shows remaining jumps in route

**PowerButton**

```
{
  "buttonType": "PowerButton",
  "function": "SYS",
  "primaryIcon": "shield-power.svg",
  "secondaryIcon": "shield-power.svg",
  "tertiaryIcon": "shield-power.svg",
  "clickSound": "beep-3.wav"
}
```

| FunctionDescriptionNote                                              
| ---------------------------------------------------------------------
| SYS     System     shows alarm state when under attack and not 4 pips
| ENG     Engines                                                      
| WEP     Weapons                                                      
| RST     Reset                                                        

**LimpetButton**

```
{
"buttonType": "LimpetButton",
"firegroup": "E",
"fire": "Secondary",
"primaryIcon": "limpet-collector.svg",
"secondaryIcon": "limpet-collector.svg",
"clickSound": "",
"errorSound": ""
}
```

The limpet controller button works with any type of limpet controller.

The button shows the current number of limpets in the cargo hold. (The same value is shown on all buttons).

There is no specific keybind for any type of limpet controller.
Instead, you need to set up a fire group letter and primary or secondary fire button.

**FSSButton**

```
{
"buttonType": "FSSButton",
"primaryIcon": "fss.svg",
"secondaryIcon": "fss.svg",
"tertiaryIcon": "fss.svg",
"clickSound": "",
"errorSound": "",
"dontSwitchToCombatMode": false
}
```

**StaticButton**

```
{
  "buttonType": "StaticButton",
  "function": "SelectTarget",
  "primaryIcon": "xxx.svg",
  "clickSound": ""
}
```

| Function               Combat    
| --------------------------------------
| ChargeECMECM
| CycleFireGroupNextNext Fire Group
| CycleFireGroupPreviousPrev Fire Group
| CycleNextHostileTargetNext Hostile
| CyclePreviousHostileTargetPrev Hostile
| CycleNextSubsystemNext Subsystem
| CyclePreviousSubsystemPrev Subsystem
| CycleNextTargetNext Contact
| CyclePreviousTargetPrev Contact
| SelectTargetTarget Ahead

| Function               Fighter    
| --------------------------------------
| OpenOrdersCrew Orders
| OrderAggressiveBehaviourBe Aggressive
| OrderDefensiveBehaviourBe Defensive
| OrderFocusTargetAttack My Target
| OrderFollowFollow
| OrderHoldFireHold Fire
| OrderHoldPositionHold Position
| OrderRequestDockDock SLF

| Function               Misc    
| --------------------------------------
| FriendsMenuFriends
| GalnetAudio_ClearQueueClear Audio Queue
| GalnetAudio\_Play\_PausePlay/Pause Audio
| GalnetAudio_SkipBackwardPrev Audio Track
| GalnetAudio_SkipForwardNext Audio Track
| MicrophoneMuteMicrophone
| OpenCodexGoToDiscoveryCodex
| PauseMain Menu
| HMDResetReset HMD
| OculusResetReset Oculus
| RadarDecreaseRangeDec Sensor Range
| RadarIncreaseRangeInc Sensor Range

| Function               Navigation    
| --------------------------------------
| SetSpeed100100% Throttle
| SetSpeed7575% Throttle
| SetSpeed5050% Throttle
| SetSpeed2525% Throttle
| SetSpeedMinus100100% Reverse
| SetSpeedMinus7575% Reverse
| SetSpeedMinus5050% Reverse
| SetSpeedMinus2525% Reverse
| SetSpeedZeroAll Stop
| TargetNextRouteSystemNext Jump Dest
| ToggleReverseThrottleInputReverse
| UseAlternateFlightValuesToggleAlternate Controls
| UseBoostJuiceBoost
| DisableRotationCorrectToggleRotational Correction

| Function               Ship    
| --------------------------------------
| FocusCommsPanelComms Panel
| FocusLeftPanelNav Panel
| FocusRadarPanelRole Panel
| FocusRightPanelSystems Panel
| QuickCommsPanelQuick Comms
| EjectAllCargoEject All Cargo
| HeadLookToggleToggle Headlook
| MouseResetReset Mouse
| OrbitLinesToggleOrbit Lines
| SelectTargetsTargetWingman's target
| WingNavLockWingman Navlock
| TargetWingman0Wingman 1
| TargetWingman1Wingman 2
| TargetWingman2Wingman 3
| EngineColourToggleEngine Colour
| WeaponColourToggleWeapon Colour

| Function               SRV    
| --------------------------------------
| RecallDismissShipRecall/Dismiss Ship
| BuggyToggleReverseThrottleInputReverse
| DecreaseSpeedButtonMaxZero Speed
| IncreaseSpeedButtonMaxMaximum Speed
| SelectTarget_BuggyTarget Ahead

| Function               Hyperspace    
| --------------------------------------
| SupercruiseSupercruise
| HyperspaceHyperspace Jump

| Function               On Foot    
| --------------------------------------
| HumanoidPrimaryInteractButtonPrimary Interact
| HumanoidSecondaryInteractButtonSecondary Interact
| HumanoidSelectPrimaryWeaponButtonSelect Primary Weapon
| HumanoidSelectSecondaryWeaponButtonSelect Secondary Weapon
| HumanoidHideWeaponButtonHolster Weapon
| HumanoidSwitchWeaponSwitch Weapon
| HumanoidSelectUtilityWeaponButtonSelect Tool
| HumanoidSelectNextWeaponButtonNext Weapon
| HumanoidSelectPreviousWeaponButtonPrevious Weapon
| HumanoidReloadButtonReload
| HumanoidSelectNextGrenadeTypeButtonNext Grenade Type
| HumanoidSelectPreviousGrenadeTypeButtonPrevious Grenade Type
| HumanoidThrowGrenadeButtonThrow Grenade
| HumanoidMeleeButtonMelee
| HumanoidSwitchToRechargeToolEnergy Link
| HumanoidSwitchToCompAnalyserProfile Analyser
| HumanoidToggleToolModeButtonTool Mode
| HumanoidToggleNightVisionButtonNight Vision
| HumanoidSwitchToSuitToolSuit Specific Tool
| HumanoidToggleFlashlightButtonFlashlight
| GalaxyMapOpen_HumanoidGalaxy Map
| SystemMapOpen_HumanoidSystem Map
| FocusCommsPanel_HumanoidComms Panel
| QuickCommsPanel_HumanoidQuick Comms
| HumanoidConflictContextualUIButtonConflict Zone Battle Stats
| HumanoidToggleShieldsButtonShields
| HumanoidToggleMissionHelpPanelButtonHelp
| HumanoidClearAuthorityLevelClear Stolen Profile
| HumanoidHealthPackUse Medkit
| HumanoidBatteryUse Energy Cells
| HumanoidSelectFragGrenadeSelect Frag Grenade
| HumanoidSelectEMPGrenadeSelect EMP Grenade
| HumanoidSelectShieldGrenadeSelect Shield Grenade



Thanks to :

https://github.com/congzhangzh/desktoploveblazorweb

https://www.tryphotino.io/

https://mudblazor.com/

https://keathmilligan.net/themeable-icon-pack-for-streamdeck-elite for the button icons

https://github.com/EDCD/EDDI

https://github.com/MagicMau/EliteJournalReader

https://github.com/msarilar/EDEngineer

https://www.hwinfo.com/

DaftMav for POI list [see here](https://www.reddit.com/r/EliteDangerous/comments/9mfiug/edison_a_tool_which_helps_getting_to_planet/)

https://eddb.io/ and https://www.edsm.net/ for station, system and body data

https://inara.cz/ for pricing data

https://www.edsm.net/ for the galaxy image

https://edassets.org/ CMDR Qohen Leth and CMDR Nuse for the ship images

http://edtools.ddns.net/
