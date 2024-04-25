using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine.SceneManagement;
using Il2Cpp;
using UnityEngine.Localization.Settings;
using Discord;
using WebSocketSharp;
using WebSocketSharp.Server;
using MelonLoader.Utils;
using System.Runtime.InteropServices;

namespace PhoenixMod
{
    public static class BuildInfo
    {
        public const string Name = "PhoenixMod";
        public const string Description = "The first (as far as I know) mod for Rhythm Quest. Currently only available for the Demo, as the full game hasn't been released.";
        public const string Author = "gingerphoenix10";
        public const string Company = null;
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://www.github.com/gingerphoenix10/Rhythm-Quest-PhoenixMod/releases/latest";
    }
    public class QuestWebSocket : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            switch (e.Data.ToLower())
            {
                case "connect":
                    Send("{\"status\": \"Connected Successfully!\"}");
                    break;

                default:
                    Send("{\"status\": \"Invalid command\"}");
                    break;
            }
        }
    }

    public class PhoenixMod : MelonMod
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary(string lpFileName);
        Discord.Discord discord;
        int startTime = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        public override void OnInitializeMelon() {
            FileInfo discordSDK = new FileInfo(Path.Combine(MelonEnvironment.GameRootDirectory, "discord_game_sdk.dll"));
            if (!discordSDK.Exists)
            {
                Stream manifestResourceStream = MelonAssembly.Assembly.GetManifestResourceStream("PhoenixMod.discord_game_sdk.dll");
                byte[] array = new byte[manifestResourceStream.Length];
                _ = manifestResourceStream.Read(array, 0, array.Length);
                File.WriteAllBytes(discordSDK.FullName, array);
            }
            LoadLibrary(discordSDK.FullName);
            discord = new Discord.Discord(1232052875602038814, (ulong)Discord.CreateFlags.NoRequireDiscord);
        }
        WebSocketServer wssv;
        public override void OnLateInitializeMelon() {
            wssv = new WebSocketServer("ws://127.0.0.1:4575");

            wssv.AddWebSocketService<QuestWebSocket>("/");
            wssv.Start();
        }

        public override void OnSceneWasLoaded(int buildindex, string sceneName)
        {
        }
        public void SetDiscord(Discord.Activity activity)
        {
            var discordActivityManager = discord.GetActivityManager();
            discordActivityManager.UpdateActivity(activity, (res) => { });
        }
        public override void OnSceneWasInitialized(int buildindex, string sceneName)
        {
            wssv.WebSocketServices["/"].Sessions.BroadcastAsync("{\"type\": \"SceneInit\", \"sceneName\": \""+sceneName+"\"}", () => { });
            switch (sceneName) {
                case "NoticeScene":
                    LocalizationSettings.StringDatabase.GetTable("Main String Table").AddEntry("phoenix_button", "Opens the PhoenixMod panel.");
                    MelonLogger.Msg("[PHOENIX] Hello from PhoenixMod");
                    var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Text");
                    foreach (var gameObj in objects)
                    {
                        if (gameObj.GetComponent<TMP_Text>().text.StartsWith("Welcome to the Rhythm Quest Demo!"))
                        {
                            gameObj.GetComponent<TMP_Text>().text = "Welcome to Rhythm Quest Modded!";
                        }
                        if (gameObj.GetComponent<TMP_Text>().text.StartsWith("This is an early demo build of Rhythm Quest"))
                        {
                            string newMessage = @"This is an early demo build of Rhythm Quest, so only some levels are accessible, and everything in the game is still subject to change!

Thank you for installing PhoenixMod, The (as far as I know) First ever Rhythm Quest mod.

Have fun, and thanks for playing, and modding!
                                                                    --DDRKirby(ISQ), gingerphoenix10";
                            gameObj.GetComponent<TMP_Text>().text = newMessage;
                        }
                    }
                    SetDiscord(new Discord.Activity
                    {
                        Details = "Notice Screen",
                        State = "",
                        Assets =
                        {
                            LargeImage = "sayuri",
                            LargeText = "PhoenixMod",
                            SmallImage = "gingerphoenix10",
                            SmallText = "by gingerphoenix10"
                        },
                        Timestamps =
                        {
                            Start = startTime
                        }
                    });
                    break;
                case "MenuScene":
                    GameObject ogButton = GameObject.Find("UI Canvas/GameModSettings/Background/Layout/Back");
                    GameObject clone = GameObject.Instantiate(ogButton);
                    clone.transform.parent = ogButton.transform.parent;
                    clone.transform.SetSiblingIndex(1);
                    clone.name = "PhoenixMod";
                    SetDiscord(new Discord.Activity
                    {
                        Details = "Main Menu",
                        State = "",
                        Assets =
                        {
                            LargeImage = "sayuri",
                            LargeText = "PhoenixMod",
                            SmallImage = "gingerphoenix10",
                            SmallText = "by gingerphoenix10"
                        },
                        Timestamps =
                        {
                            Start = startTime
                        }
                    });
                    break;
            }
            if (sceneName.StartsWith("level"))
            {
                SetDiscord(new Discord.Activity
                {
                    Details = "Level " + sceneName.Remove(0,5),
                    State = "",
                    Assets = 
                    {
                        LargeImage = "sayuri",
                        LargeText = "PhoenixMod",
                        SmallImage = "gingerphoenix10",
                        SmallText = "by gingerphoenix10"
                    },
                    Timestamps =
                    {
                        Start = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds
                    }
                });
            }
        }
        public override void OnUpdate()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.StartsWith("level"))
            {
                try
                {
                    var audio = GameObject.Find("MusicSource (1)");
                    double ExpectedCompletion = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds + (Math.Floor(audio.GetComponent<AudioSource>().clip.length / audio.GetComponent<AudioSource>().pitch - audio.GetComponent<AudioSource>().time / audio.GetComponent<AudioSource>().pitch));
                    wssv.WebSocketServices["/"].Sessions.BroadcastAsync("{\"type\": \"LevelUpdate\", \"level\": \"" + sceneName.Remove(0, 5) + "\", \"songLength\": " + audio.GetComponent<AudioSource>().clip.length + ", \"levelLength\": " + audio.GetComponent<AudioSource>().clip.length / audio.GetComponent<AudioSource>().pitch + ", \"currentSongTime\": " + audio.GetComponent<AudioSource>().time + ", \"currentLevelTime\": " + (audio.GetComponent<AudioSource>().time / audio.GetComponent<AudioSource>().pitch) + "}", () => { });
                    if (ExpectedCompletion - prevExpectancy >= 2)
                    {
                        prevExpectancy = ExpectedCompletion;
                        SetDiscord(new Discord.Activity
                        {
                            Details = "Level " + sceneName.Remove(0, 5),
                            State = "",
                            Assets =
                        {
                            LargeImage = "sayuri",
                            LargeText = "PhoenixMod",
                            SmallImage = "gingerphoenix10",
                            SmallText = "by gingerphoenix10"
                        },
                            Timestamps =
                        {
                            Start = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds,
                            End = (long)ExpectedCompletion
                        }
                        });
                        wssv.WebSocketServices["/"].Sessions.BroadcastAsync("{\"type\": \"EstimationChanged\", \"epoch\": \"" + ExpectedCompletion + "\"}", () => { });
                    }
                }
                catch (Exception) { }
            }
            if (sceneName == "MenuScene")
            {
                if (GameObject.Find("Credits/Background/1/GameObject (3)/Text") != null)
                {
                    GameObject.Find("Credits/Background/1/GameObject (3)/Text").GetComponent<TMP_Text>().text = "Moral Support: Fuzzybunny2 \nPhoenixMod: gingerphoenix10 (Phoenix Payne-Barnes)";
                }
                if (GameObject.Find("UI Canvas/GameModSettings/Background/Layout/PhoenixMod") != null) {
                    GameObject.Find("UI Canvas/GameModSettings/Background/Layout/PhoenixMod/Offset/Text").GetComponent<TMP_Text>().text = "PhoenixMod";
                }
            }
            discord.RunCallbacks();
        }
        double prevExpectancy = 0;
        public override void OnLateUpdate()
        {
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) {
        }

        public override void OnApplicationQuit()
        {
            discord.Dispose();
        }

        public override void OnPreferencesSaved()
        {
        }

        public override void OnPreferencesLoaded()
        {
        }
    }
}