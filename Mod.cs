using P3RPC.PartyMember.FuukaOverhaul.Configuration;
using P3RPC.PartyMember.FuukaOverhaul.Modules;
using P3RPC.PartyMember.FuukaOverhaul.Template;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using P3RPC.PartyMember.FuukaOverhaul.Utils.Types;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
//using P3R.CostumeFramework.Interfaces;
using System.Drawing;

namespace P3RPC.PartyMember.FuukaOverhaul;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{

    /// <summary>
    /// Provides access to the mod loader API.
    /// </summary>
    private readonly IModLoader _modLoader;

    /// <summary>
    /// Provides access to the Reloaded.Hooks API.
    /// </summary>
    /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
    private readonly IReloadedHooks? _hooks;

    /// <summary>
    /// Provides access to the Reloaded logger.
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Entry point into the mod, instance that created this class.
    /// </summary>
    private readonly IMod _owner;

    /// <summary>
    /// Provides access to this mod's configuration.
    /// </summary>
    private Config _configuration;

    /// <summary>
    /// The configuration of the currently executing mod.
    /// </summary>
    private readonly IModConfig _modConfig;

    /// <summary>
    /// MANDATORY DEPENDENCIES
    /// </summary>
    private readonly IUnrealEssentials? unrealEssentials;
    private readonly IUnreal? unrealEmitter;
    //private readonly ICostumeApi? costumeApi;

    /// <summary>
    /// Optional Dependencies
    /// </summary>

    public const string modName = "P3RPC.PartyMember.FuukaOverhaul";

    public string? modDirectory;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;

        // INIT DEPENDENCIES

        // INIT PROJECT.UTILS LOGGER

        Project.Init(_modConfig, _modLoader, _logger);
        Log.LogLevel = _configuration.LogLevel;

        bool haveUnrealEssentials;
        bool haveUnrealEmitter;
        // SET MOD DIRECTORY
        var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);  

        var unrealEssentialsController = _modLoader.GetController<IUnrealEssentials>();
        if (unrealEssentialsController == null || !unrealEssentialsController.TryGetTarget(out var unrealEssentials))
        {
            Log.Error("Unable to get controller for Unreal Essentials.");
//            _logger.WriteLine($"[{modName}] | [My Mod] Unable to get controller for Unreal Essentials.", System.Drawing.Color.Red);
            return;
        }
        else
        {
            var check = true;
            haveUnrealEssentials = check;
        }
        this.unrealEssentials = unrealEssentials;

        var unrealEmitterController = _modLoader.GetController<IUnreal>();
        if (unrealEmitterController == null || !unrealEmitterController.TryGetTarget(out var unrealEmitter))
        {
            Log.Error("Unable to get controller for Unreal Object Emitters.");
            //            _logger.WriteLine($"[{modName}] | [My Mod] unable to get controller for Unreal Object Emitters.", System.Drawing.Color.Red);
            return;
        }
        else
        {
            haveUnrealEmitter = true;
            _logger.WriteLine($"[{modName}] | Unreal Emitter loaded: {haveUnrealEmitter.ToString()}");
        }
        this.unrealEmitter = unrealEmitter;

        // END INIT DEPENDENCIES

        // LOAD TEXTURES

        LoadModule(this.unrealEssentials, modDir, Module.Core);

        var hairStyle = Enum.GetName(_configuration.hairstyleSetting);
        var hairOffset = (int)_configuration.hairstyleSetting;
        var glassesOffset = (int)_configuration.glassesSetting;

        Log.Debug($"Current hair offset: {hairOffset}");
        Log.Debug($"Current glasses offset: {glassesOffset}");

        var normalHair = 0;
        var seesHair = 52;

        var newNormalHair = hairOffset + glassesOffset;
        Log.Debug($"Current hair index: {newNormalHair:000}");
        var newSEESHair = 52 + newNormalHair;

        var H000 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, normalHair);
        var H052 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, seesHair);
        var Title = Assets.GetAssetPath(Character.Fuuka, AssetType.TitleMesh, normalHair);

        /// Possible hair redirects
        /// H001 - Ponytail no glasses  [Headset: H053]
        /// H010 - Glasses no ponytail  [Headset: H062]
        /// H011 - Glasses & ponytail   [Headset: H063]


        // LOAD HAIRSTYLE

        if (hairStyle != null)
        {
            LoadModule(this.unrealEssentials, modDir, Module.Hair);
            if (hairOffset != 0)
            {
                // PONYTAILS HATE BONNETS, REDIRECT TO ALT MAID OUTFIT
                var C106 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 106);
                var newC106 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 906);
                Redirect(C106, newC106);
            }
            if (newNormalHair > 0)
            {
                var newH000 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, newNormalHair);
                var newH052 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, newSEESHair);
                var newTitle = Assets.GetAssetPath(Character.Fuuka, AssetType.TitleMesh, newNormalHair);
                Log.Debug($"Current hair path: {newH000}");
                Log.Debug($"Current combat hair path: {newH052}");
                Redirect(H000, newH000);
                Redirect(H052, newH052);
                Redirect(Title, newTitle);
                // BUSTUPS
                LoadModule(unrealEssentials, modDir, Module.Bustups);
                BustupRedirects(newNormalHair);
            }
        }
        Project.Start();
    }
    private void BustupRedirects(int hairIndex)
    {
        var glassesEnabled = hairIndex > 9;
        var hairChanged = hairIndex > 0;

        if (glassesEnabled)
        {
            foreach (var texture in EyeTextures)
            {
                var oldAsset = Assets.GetBustupPath(texture, Character.Fuuka);
                var newAsset = Assets.GetAltBustupPath(texture, Character.Fuuka, BustupComponent.Emote);
                Redirect(oldAsset, newAsset);
            }
        }
        if (hairChanged)
        {
            foreach (var texture in BaseTextures)
            {
                var oldAsset = Assets.GetBustupPath(texture, Character.Fuuka);
                var newAsset = Assets.GetAltBustupPath(texture, Character.Fuuka, BustupComponent.BaseLayer, hairIndex);
                Redirect(oldAsset, newAsset);
            }
        }
    }
    private void LoadModule(IUnrealEssentials unreal, string modDir, Module module, string patch = "null")
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
    public void Redirect(string vanillaAssetPath, string modAssetPath)
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
        Bustups = 3,
        Debug = 4,
    }

    private string getModule (string modDir, Module module)
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
            throw new ArgumentNullException("Mod directory not found",e);
        }
    }
    public static string[] BaseTextures = [
        "T_BU_PC0006_PoseA_C001",
        "T_BU_PC0006_PoseA_C002",
        "T_BU_PC0006_PoseA_C005",
        "T_BU_PC0006_PoseA_C006",
        "T_BU_PC0006_PoseA_C052",
        "T_BU_PC0006_PoseA_C102",
        "T_BU_PC0006_PoseA_C154",
        "T_BU_PC0006_PoseA_C155",
        "T_BU_PC0006_PoseA_C156",
        "T_BU_PC0006_PoseB_C001",
        "T_BU_PC0006_PoseB_C002",
        "T_BU_PC0006_PoseB_C005",
        "T_BU_PC0006_PoseB_C006",
        "T_BU_PC0006_PoseB_C052",
        "T_BU_PC0006_PoseB_C102",
        "T_BU_PC0006_PoseB_C154",
        "T_BU_PC0006_PoseB_C155",
        "T_BU_PC0006_PoseB_C156",
    ];
    public static string[] EyeTextures = [
        "T_BU_PC0006_F00_C900_E1",
        "T_BU_PC0006_F00_C900_E2",
        "T_BU_PC0006_F00_C900_E3",
        "T_BU_PC0006_F00_C901_E1",
        "T_BU_PC0006_F00_C901_E2",
        "T_BU_PC0006_F00_C901_E3",
        "T_BU_PC0006_F01_C900_E1",
        "T_BU_PC0006_F01_C900_E2",
        "T_BU_PC0006_F01_C900_E3",
        "T_BU_PC0006_F02_C900_E1",
        "T_BU_PC0006_F02_C900_E2",
        "T_BU_PC0006_F02_C900_E3",
        "T_BU_PC0006_F03_C900_E1",
        "T_BU_PC0006_F03_C900_E2",
        "T_BU_PC0006_F03_C900_E3",
        "T_BU_PC0006_F04_C052_M1",
        "T_BU_PC0006_F04_C052_M2",
        "T_BU_PC0006_F04_C052_M3",
        "T_BU_PC0006_F04_C900_E1",
        "T_BU_PC0006_F04_C900_E2",
        "T_BU_PC0006_F04_C900_E3",
        "T_BU_PC0006_F04_C900_M1",
        "T_BU_PC0006_F04_C900_M2",
        "T_BU_PC0006_F04_C900_M3",
        "T_BU_PC0006_F05_C900_E1",
        "T_BU_PC0006_F06_C900_E1",
        "T_BU_PC0006_F06_C900_E2",
        "T_BU_PC0006_F06_C900_E3",
        "T_BU_PC0006_F08_C052_M1",
        "T_BU_PC0006_F08_C052_M2",
        "T_BU_PC0006_F08_C052_M3",
        "T_BU_PC0006_F08_C900_E1",
        "T_BU_PC0006_F08_C900_E2",
        "T_BU_PC0006_F08_C900_E3",
        "T_BU_PC0006_F08_C900_M1",
        "T_BU_PC0006_F08_C900_M2",
        "T_BU_PC0006_F08_C900_M3",
        "T_BU_PC0006_F10_C900_E1",
        "T_BU_PC0006_F10_C900_E2",
        "T_BU_PC0006_F10_C900_E3",
        "T_BU_PC0006_F11_C052_M1",
        "T_BU_PC0006_F11_C052_M2",
        "T_BU_PC0006_F11_C052_M3",
        "T_BU_PC0006_F11_C900_E1",
        "T_BU_PC0006_F11_C900_M1",
        "T_BU_PC0006_F11_C900_M2",
        "T_BU_PC0006_F11_C900_M3",
        "T_BU_PC0006_F30_C052_M1",
        "T_BU_PC0006_F30_C052_M2",
        "T_BU_PC0006_F30_C052_M3",
        "T_BU_PC0006_F30_C900_E1",
        "T_BU_PC0006_F30_C900_M1",
        "T_BU_PC0006_F30_C900_M2",
        "T_BU_PC0006_F30_C900_M3",
        "T_BU_PC0006_F31_C900_E1",
    ];


    /*    public static void LoadOverride(ICostumeApi costumeApi, string moduleDir, string overrideFile = "CostumeOverride.yaml")
        {

            var _override = Path.Join(moduleDir, overrideFile);

            costumeApi.AddOverridesFile(_override);
        }

        public void InitOverrides(string folder, ICostumeApi costumeApi, Config config)
        {
            var thisFolder = folder;

            var overrideFolder = Path.Join(thisFolder, "Overrides");

            var overrideFiles = Directory.GetFiles(overrideFolder, "*.yaml");

            foreach (var overrideFile in overrideFiles)
            {
                var file = Path.GetFileName(overrideFile);
                var option = Path.GetFileNameWithoutExtension(overrideFile);

                if (Enum.IsDefined(typeof(NewOutfits), option))
                {
                    var enumOption = (NewOutfits)Enum.Parse(typeof(NewOutfits), option);
                    if (optionBool(enumOption, config))
                    {
                        LoadOverride(costumeApi, overrideFolder, file);
                    }
                }
            }
        }


        public bool optionBool(NewOutfits outfit, Config config)
        => outfit switch
        {
            NewOutfits.UNSC_Parka => config.UNSC_Parka,
            _ => throw new NotImplementedException(),
        };
    */
    private record class AssetIDFuuka (AssetType Type, int AssetID)
    {

    }
    #region Standard Overrides
    public override void ConfigurationUpdated(Config configuration)
    {
        // Apply settings from configuration.
        // ... your code here.
        _configuration = configuration;
        _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
    }
    #endregion

    #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Mod() { }
#pragma warning restore CS8618
    #endregion

}