using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(PhoenixMod.BuildInfo.Description)]
[assembly: AssemblyDescription(PhoenixMod.BuildInfo.Description)]
[assembly: AssemblyCompany(PhoenixMod.BuildInfo.Company)]
[assembly: AssemblyProduct(PhoenixMod.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + PhoenixMod.BuildInfo.Author)]
[assembly: AssemblyTrademark(PhoenixMod.BuildInfo.Company)]
[assembly: AssemblyVersion(PhoenixMod.BuildInfo.Version)]
[assembly: AssemblyFileVersion(PhoenixMod.BuildInfo.Version)]
[assembly: MelonInfo(typeof(PhoenixMod.PhoenixMod), PhoenixMod.BuildInfo.Name, PhoenixMod.BuildInfo.Version, PhoenixMod.BuildInfo.Author, PhoenixMod.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame(null, null)]