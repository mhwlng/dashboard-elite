# dashboard-elite
Elite Dangerous dashboard for bar style ultra-wide touch screens

Use a wide 'bar style' touch screen as a game dashboard (like [fip-panels](https://github.com/mhwlng/fip-elite)) and button box (like [streamdeck](https://github.com/mhwlng/streamdeck-elite)) for Elite Dangerous :


VERY UNFINISHED PRE-ALPHA RELEASE

![touch screen](https://i.imgur.com/RkHJESd.jpg)

![touch screen](https://i.imgur.com/QLA3Fgm.png)

![touch screen](https://i.imgur.com/dK75qt0.png)


The touch screen is connected via hdmi as a secondary monitor.
So, this will not work on e.g. a tablet or a separate device.

The game has to be set to borderless mode. Fullscreen mode won't work.

The actual on/off state of a button comes from the game.
When pressing a button on the touch screen, it will send the e.g. 'toggle light' keypress (from the game keyboard binding file) to the game.

Big display 1920x515 (12.6 inch, IPS panel):

https://www.aliexpress.com/item/1005001966967133.html

Small display 1920x480 (8.8 inch, IPS panel):

https://www.aliexpress.com/item/1005003014364673.html

(note that these kinds of displays come with or without a touch controller and with or without a case)

The big display looks like the one that is used in the asus duo laptop (google NV126B5M):

https://www.asus.com/Laptops/For-Home/ZenBook/ZenBook-Duo-14-UX482/

When connecting these screens, the touch screen needs to be configured via : Control Panel \ Tablet PC settings \ Setup

![touch screen](https://i.imgur.com/S6Xy3NO.png)

To get similar button sizes on both displays, the buttons are arranged in a 8x4 grid on the big screen and a 10x3 grid on the small screen.
The button arrangement is customizable..

The button images are SVG vector images from [Keath Milligan](https://keathmilligan.net/themeable-icon-pack-for-streamdeck-elite).

button possibilities:
Separate SVG button images for each state
Animated SVG button images
Multiple button profiles
Automatic profile switching
Button press sounds

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
