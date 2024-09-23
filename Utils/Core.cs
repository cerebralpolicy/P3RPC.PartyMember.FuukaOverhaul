using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using P3RPC.PartyMember.FuukaOverhaul.Utils;

namespace P3RPC.PartyMember.FuukaOverhaul;

internal static class Core
{
    public const string modName = "P3RPC.PartyMember.FuukaOverhaul";
    private record AssetFNames(string assetFile)
    {
        public string AssetName { get; } = Path.GetFileNameWithoutExtension(assetFile);

        public string AssetPath { get; } = Assets.GetAssetPath(assetFile);
    }

    private enum Module
    {
        Core = 0,
        Hair = 1,
        Costumes = 2,
        CharCreator = 3,
        Debug = 4,
    }

    private static string getModule(string modDir, Module module)
    {
        var moduleID = ((int)module);
        var moduleName = Enum.GetName(typeof(Module), moduleID);
        var moduleFolder = String.Join("_", moduleID.ToString("00"), moduleName);

        try
        {
            var modulePath = Path.Combine(modDir, "Modules", moduleFolder);
            return modulePath;
        }
        catch (Exception e)
        {
            throw new ArgumentNullException("Mod directory not found", e);
        }
    }
}