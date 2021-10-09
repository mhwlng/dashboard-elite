# dashboard-elite
Elite Dangerous dashboard for bar style ultra-wide touch screens

Use a wide 'bar style' touch screen as a game dashboard (like [fip-panels](https://github.com/mhwlng/fip-elite)) and button box (like [streamdeck](https://github.com/mhwlng/streamdeck-elite)) for Elite Dangerous :


VERY UNFINISHED PRE-ALPHA RELEASE

![touch screen](https://i.imgur.com/RkHJESd.jpg)

![touch screen](https://i.imgur.com/QLA3Fgm.png)

![touch screen](https://i.imgur.com/dK75qt0.png)

Technology used:
- [.net 5.0](https://dotnet.microsoft.com/download/dotnet/5.0/runtime)  choose : download x64
- [WebView2 Runtime](https://go.microsoft.com/fwlink/p/?LinkId=2124703)
- blazor server
- mudblazor

The touch screen is connected via hdmi as a secondary monitor.
So, this will not work on e.g. a tablet or a separate device.

The game has to be set to borderless mode. Fullscreen mode won't work.

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

The button arrangement is customizable. (see the wwwroot\css\site.css file)

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


The buttons only work with keyboard bindings. 
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

All the button svg images can be found in the wwwroot\img\buttons directory. 

The button captions are inside the svg files.

Animated SVG files are possible.

various button type can be configured, each with their own set of parameters :

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

The supported toggle-buttons are:

| Function                | Description     |
| ----------------------- | --------------- |
| PlayerHUDModeToggle     | Analysis Mode   |
| ToggleCargoScoop        | Cargo Scoop     |
| ToggleFlightAssist      | Flight Assist   |
| GalaxyMap               | Galaxy Map      |
| DeployHardpointToggle   | Hardpoints      |
| LandingGearToggle       | Landing Gear    |
| ShipSpotLightToggle     | Lights          |
| NightVisionToggle       | Night Vision    |
| ToggleButtonUpInput     | Silent Running  |
| ToggleDriveAssist       | Srv DriveAssist |
| AutoBreakBuggyButton    | Srv Handbrake   |
| ToggleBuggyTurretButton | Srv Turret      |
| Supercruise             | Supercruise     |
| SystemMap               | System Map      |
| FocusCommsPanel         | Comms Panel     |
| FocusLeftPanel          | Nav Panel       |
| FocusRadarPanel         | Role Panel      |
| FocusRightPanel         | Systems Panel   |

For Odyssey, when On Foot, the Galaxy Map,System Map,Lights & Night Vision buttons will call the on-foot key bindings, 
but there is no state feedback. So the button image won't change.

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

The supported alarm-buttons are:

| Function            | Description      | Note                         |
| ------------------- | ---------------- | ---------------------------- |
| SelectHighestThreat | Highest Threat   | alarm = under attack status  | 
| DeployHeatsink      | Heat sink        | alarm = under attack status  | 
| DeployChaff         | Chaff            | alarm = overheating status   | 
| DeployShieldCell    | Shield Cell Bank | alarm = shields down status. In that case DON'T fire a shield cell bank.   | 

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

| Function              | Description     | Note                                |
| --------------------- | --------------- | ----------------------------------- |
| HyperSuperCombination | Toggle FSD      | also shows remaining jumps in route |
| Supercruise           | Supercruise     |                                     |
| Hyperspace            | Hyperspace Jump | also shows remaining jumps in route |

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

| Function | Description | Note                                               |
| -------- | ----------- | -------------------------------------------------- |
| SYS      | System      | shows alarm state when under attack and not 4 pips |
| ENG      | Engines     |                                                    |
| WEP      | Weapons     |                                                    |
| RST      | Reset       |                                                    |

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

| Function                | Combat     |
| ----------------------- | --------------- |
| ChargeECM | ECM |
| CycleFireGroupNext | Next Fire Group |
| CycleFireGroupPrevious | Prev Fire Group |
| CycleNextHostileTarget | Next Hostile |
| CyclePreviousHostileTarget | Prev Hostile |
| CycleNextSubsystem | Next Subsystem |
| CyclePreviousSubsystem | Prev Subsystem |
| CycleNextTarget | Next Contact |
| CyclePreviousTarget | Prev Contact |
| SelectTarget | Target Ahead |

| Function                | Fighter     |
| ----------------------- | --------------- |
| OpenOrders | Crew Orders |
| OrderAggressiveBehaviour | Be Aggressive |
| OrderDefensiveBehaviour | Be Defensive |
| OrderFocusTarget | Attack My Target |
| OrderFollow | Follow |
| OrderHoldFire | Hold Fire |
| OrderHoldPosition | Hold Position |
| OrderRequestDock | Dock SLF |

| Function                | Misc     |
| ----------------------- | --------------- |
| FriendsMenu | Friends |
| GalnetAudio_ClearQueue | Clear Audio Queue |
| GalnetAudio_Play_Pause | Play/Pause Audio |
| GalnetAudio_SkipBackward | Prev Audio Track |
| GalnetAudio_SkipForward | Next Audio Track |
| MicrophoneMute | Microphone |
| OpenCodexGoToDiscovery | Codex |
| Pause | Main Menu |
| HMDReset | Reset HMD |
| OculusReset | Reset Oculus |
| RadarDecreaseRange | Dec Sensor Range |
| RadarIncreaseRange | Inc Sensor Range |

| Function                | Navigation     |
| ----------------------- | --------------- |
| SetSpeed100 | 100% Throttle |
| SetSpeed75 | 75% Throttle |
| SetSpeed50 | 50% Throttle |
| SetSpeed25 | 25% Throttle |
| SetSpeedMinus100 | 100% Reverse |
| SetSpeedMinus75 | 75% Reverse |
| SetSpeedMinus50 | 50% Reverse |
| SetSpeedMinus25 | 25% Reverse |
| SetSpeedZero | All Stop |
| TargetNextRouteSystem | Next Jump Dest |
| ToggleReverseThrottleInput | Reverse |
| UseAlternateFlightValuesToggle | Alternate Controls |
| UseBoostJuice | Boost |
| DisableRotationCorrectToggle | Rotational Correction |

| Function                | Ship     |
| ----------------------- | --------------- |
| FocusCommsPanel | Comms Panel |
| FocusLeftPanel | Nav Panel |
| FocusRadarPanel | Role Panel |
| FocusRightPanel | Systems Panel |
| QuickCommsPanel | Quick Comms |
| EjectAllCargo | Eject All Cargo |
| HeadLookToggle | Toggle Headlook |
| MouseReset | Reset Mouse |
| OrbitLinesToggle | Orbit Lines |
| SelectTargetsTarget | Wingman's target |
| WingNavLock | Wingman Navlock |
| TargetWingman0 | Wingman 1 |
| TargetWingman1 | Wingman 2 |
| TargetWingman2 | Wingman 3 |
| EngineColourToggle | Engine Colour |
| WeaponColourToggle | Weapon Colour |

| Function                | SRV     |
| ----------------------- | --------------- |
| RecallDismissShip | Recall/Dismiss Ship |
| BuggyToggleReverseThrottleInput | Reverse |
| DecreaseSpeedButtonMax | Zero Speed |
| IncreaseSpeedButtonMax | Maximum Speed |
| SelectTarget_Buggy | Target Ahead |

| Function                | Hyperspace     |
| ----------------------- | --------------- |
| Supercruise | Supercruise |
| Hyperspace | Hyperspace Jump |

| Function                | On Foot     |
| ----------------------- | --------------- |
| HumanoidPrimaryInteractButton | Primary Interact |
| HumanoidSecondaryInteractButton | Secondary Interact |
| HumanoidSelectPrimaryWeaponButton | Select Primary Weapon |
| HumanoidSelectSecondaryWeaponButton | Select Secondary Weapon |
| HumanoidHideWeaponButton | Holster Weapon |
| HumanoidSwitchWeapon | Switch Weapon |
| HumanoidSelectUtilityWeaponButton | Select Tool |
| HumanoidSelectNextWeaponButton | Next Weapon |
| HumanoidSelectPreviousWeaponButton | Previous Weapon |
| HumanoidReloadButton | Reload |
| HumanoidSelectNextGrenadeTypeButton | Next Grenade Type |
| HumanoidSelectPreviousGrenadeTypeButton | Previous Grenade Type |
| HumanoidThrowGrenadeButton | Throw Grenade |
| HumanoidMeleeButton | Melee |
| HumanoidSwitchToRechargeTool | Energy Link |
| HumanoidSwitchToCompAnalyser | Profile Analyser |
| HumanoidToggleToolModeButton | Tool Mode |
| HumanoidToggleNightVisionButton | Night Vision |
| HumanoidSwitchToSuitTool | Suit Specific Tool |
| HumanoidToggleFlashlightButton | Flashlight |
| GalaxyMapOpen_Humanoid | Galaxy Map |
| SystemMapOpen_Humanoid | System Map |
| FocusCommsPanel_Humanoid | Comms Panel |
| QuickCommsPanel_Humanoid | Quick Comms |
| HumanoidConflictContextualUIButton | Conflict Zone Battle Stats |
| HumanoidToggleShieldsButton | Shields |
| HumanoidToggleMissionHelpPanelButton | Help |
| HumanoidClearAuthorityLevel | Clear Stolen Profile |
| HumanoidHealthPack | Use Medkit |
| HumanoidBattery | Use Energy Cells |
| HumanoidSelectFragGrenade | Select Frag Grenade |
| HumanoidSelectEMPGrenade | Select EMP Grenade |
| HumanoidSelectShieldGrenade | Select Shield Grenade |
            





Thanks to :

https://github.com/congzhangzh/desktoploveblazorweb

https://www.tryphotino.io/

https://mudblazor.com/

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
