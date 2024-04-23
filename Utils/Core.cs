using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using P3RPC.PartyMember.FuukaOverhaul.Utils;

namespace P3RPC.PartyMember.FuukaOverhaul;

internal static class Core
{
    public const string modName = "P3RPC.PartyMember.FuukaOverhaul";
    private static void LoadModule(IUnrealEssentials unreal, string modDir, Module module, string patch = "null")
    {
        if (patch == "null")
        {
            try
            {
                var modulePath = getModule(modDir, module);
                if (Directory.Exists(modulePath))
                {
                    if (Directory.Exists(Path.Combine(modulePath, "P3R")))
                    {
                        unreal.AddFromFolder(modulePath);
                    }
                }
            }
            catch (Exception ex)
            {
                if (getModule(modDir, module) == null)
                {
                    throw new ArgumentNullException($"No {module} module found", ex);
                }
            }
        }
    }
    public static void Redirect(string vanillaAssetPath, string modAssetPath, IUnreal unrealEmitter)
    {
        var van = vanillaAssetPath;
        var mod = modAssetPath;
        if (van == mod)
        {
            return;
        }

        var vanFNames = new AssetFNames(van);
        var modFNames = new AssetFNames(mod);
        if (unrealEmitter != null)
        {
            unrealEmitter.AssignFName(modName, vanFNames.AssetName, modFNames.AssetName);
            unrealEmitter.AssignFName(modName, vanFNames.AssetPath, modFNames.AssetPath);
        }
    }
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