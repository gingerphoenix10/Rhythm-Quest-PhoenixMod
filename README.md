# Rhythm Quest PhoenixMod
The (as far as I can tell) first mod for Rhythm Quest. <br> 
Currently only available for the Demo, as the full game hasn't been released. <br>
<br>
## Current features:

- [X] Discord RPC
- [X] Websocket
- [ ] Settings Menu (Button exists, but is not selectable or clickable.)
- [ ] Custom Characters (Not sure if i'll ever get this one done, but i'll try)
- [ ] (Not part of the mod) Rhythm Quest Mod Manager

## Installation
(I've thought of making a Rhythm Quest mod manager, but I haven't made it yet, so this is currently the only way)<br><br>
First install [MelonLoader](https://melonwiki.xyz/#/) onto the Rhythm Quest Demo. Open the installer application, then click the `Select` next to "Unity Game:", then locate and choose your Rhythm Quest Demo executable.<br>
(Usually installed in `C:\Program Files (x86)\Steam\steamapps\common\Rhythm Quest Demo\Rhythm Quest Demo.exe` on Windows.)<br><br>
Now click `Install`, and navigate to the directory of `Rhythm Quest Demo.exe` (The one used earlier), where there should now be a `Mods` folder.<br><br>
Install the latest DLL of PhoenixMod from the [Releases](https://www.github.com/gingerphoenix10/Rhythm-Quest-PhoenixMod/releases/latest) tab, and place it into the `Mods` Folder. (The download may be flagged as a virus, as DLL files aren't usually downloaded, but it is completely safe.)<br><br>
Once installed, Starting Rhythm Quest should open a second Console window. That means MelonLoader is installed correctly.<br><br>
If PhoenixMod is installed correctly, you should see a new message on the notice screen starting with `"Thank you for installing PhoenixMod"`.<br><br>
If you accidentally skipped past this screen, There will also be an (unclickable) PhoenixMod button in the `Game Mods` menu in extras. (If the mod glitched, there will be 2 back buttons. Mod should still work)<br><br>
The mod should now be installed! You should now be able to see your Discord presence updating to show your current menu, Or the level you are currently in.<br><br>
If you know how, You can also test the Websocket by connecting to `ws://127.0.0.1:4575/` or `ws://localhost:4574/`. The events that are given by the socket are below:

## Websocket Events
### `SceneInit`
Fired every time the user loads into a new scene.<br>
```
sceneName: string - Name of the loaded scene
```
Example:
```JSON
{
    "type": "SceneInit",
    "sceneName": "MenuScene"
}
```
### `LevelUpdate`
Fired every frame while the user is in a level.
```
level: string - The level the user is currently playing

songLength: float - Length of the song file in seconds.milliseconds

levelLength: float - Length of the level (song with playback speed multiplier) in seconds.milliseconds

currentSongTime: float - Current position in the song file in seconds.milliseconds

currentLevelTime: float - Current position in the level (song position with speed multiplier) in seconds.milliseconds
```
Example:
```JSON
{
    "type": "LevelUpdate",
    "level": "3-4",
    "songLength": 114.46154,
    "levelLength": 57.23077,
    "currentSongTime": 76.307693,
    "currentLevelTime": 38.153846
}
```
### `EstimationChanged`
Fires whenever the expected Epoch time of completion changes (death and menu usually)
```
epoch: int - Expected time of completion in Epoch time (seconds since January 1st 1970, 00:00:00 UTC)
```
Example:
```JSON
{
    "type": "EstimationChanged",
    "epoch": 1726759815
}
```
## Contributing
Create a GitHub Issues ticket if you would like to give suggestions, like feature requests or optimisations, or of course... send issues with the mod.<br><br>
If you would like to build this mod from the source code, Clone this github repository, and open the solution in Visual Studio. Requires DotNet 6.0. For more help, Follow any instructions for building a MelonLoader mod.<br><br>
I will warn you, The code isn't great. This is the first mod i've made, and it uses a lot of things that probably aren't recommended at all. If I have done something like this, again, feel free to leave an issue about it.