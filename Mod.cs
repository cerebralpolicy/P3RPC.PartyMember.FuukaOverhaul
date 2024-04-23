using P3RPC.PartyMember.FuukaOverhaul.Configuration;
using P3RPC.PartyMember.FuukaOverhaul.Modules;
using P3RPC.PartyMember.FuukaOverhaul.Template;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using P3RPC.PartyMember.FuukaOverhaul.Utils.Types;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using P3R.CostumeFramework.Interfaces;

namespace P3RPC.PartyMember.FuukaOverhaul;

/// <summary>
/// Your mod logic goes here.
/// </summary>
public class Mod : ModBase // <= Do not Remove.
{

    public const string modName = "P3RPC.PartyMember.FuukaOverhaul";

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

    /// <summary>
    /// Optional Dependencies
    /// </summary>

    private readonly ICostumeApi? costumeApi;

    public string? modDirectory;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;

        var haveUnrealEssentials = false;
        var haveUnrealEmitter = false;
        var haveCostumeApi = false;
        // SET MOD DIRECTORY
        var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);  

        // For more information about this template, please see
        // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

        // If you want to implement e.g. unload support in your mod,
        // and some other neat features, override the methods in ModBase.

        // TODO: Implement some mod logic

        var unrealEssentialsController = _modLoader.GetController<IUnrealEssentials>();
        if (unrealEssentialsController == null || !unrealEssentialsController.TryGetTarget(out var unrealEssentials))
        {
            _logger.WriteLine($"[{modName}] | [My Mod] unable to get controller for Unreal Essentials.", System.Drawing.Color.Red);
            return;
        }
        else
        {
            haveUnrealEssentials = true;
        }
        this.unrealEssentials = unrealEssentials;

        var unrealEmitterController = _modLoader.GetController<IUnreal>();
        if (unrealEmitterController == null || !unrealEmitterController.TryGetTarget(out var unrealEmitter))
        {
            _logger.WriteLine($"[{modName}] | [My Mod] unable to get controller for Unreal Object Emitters.", System.Drawing.Color.Red);
            return;
        }
        else
        {
            haveUnrealEmitter = true;
            _logger.WriteLine($"[{modName}] | Unreal Emitter loaded: {haveUnrealEmitter.ToString()}");
        }
        this.unrealEmitter = unrealEmitter;

        var costumeController = _modLoader.GetController<ICostumeApi>();
        if (costumeController == null || !costumeController.TryGetTarget(out var costumeApi))
        {
            return;
        }
        else
        {
            haveCostumeApi = true;
            _logger.WriteLine($"[{modName}] | Costume API loaded: {haveCostumeApi.ToString()}");
        }
        this.costumeApi = costumeApi;

        // For more information about this template, please see
        // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

        // If you want to implement e.g. unload support in your mod,
        // and some other neat features, override the methods in ModBase.

        // TODO: Implement some mod logic



        // LOAD TEXTURES

        LoadModule(unrealEssentials, modDir, Module.Core);

        var hairStyle = Enum.GetName(_configuration.hairstyleSetting);
        var hairOffset = ((int)_configuration.hairstyleSetting);
        var glassesOffset = ((int)_configuration.glassesSetting);

        var normalHair = (int)Hair.Standard;
        var seesHair = (int)Hair.SEES_Uniform;

        var newNormalHair = normalHair + hairOffset + glassesOffset;
        var newSEESHair = seesHair + hairOffset + glassesOffset;

        var H000 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, normalHair);
        var H052 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, seesHair);
        var Title = Assets.GetAssetPath(Character.Fuuka, AssetType.TitleMesh, normalHair);
        // LOAD HAIRSTYLE

        if (_configuration.DEBUG_MODE)
        {
            // MANUAL OVERRIDES
            LoadModule(unrealEssentials, modDir, Module.Debug);
        }

        if (hairStyle != null)
        {
            LoadModule(unrealEssentials, modDir, Module.Hair);
            if (hairOffset != 0)
            {
                // PONYTAILS HATE BONNETS, REDIRECT TO ALT MAID OUTFIT
                var C106 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 106);
                var newC106 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 906);
                Redirect(C106, newC106);
            }
            if (hairOffset != 0 || glassesOffset != 0)
            {
                var newH000 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, newNormalHair);
                Redirect(H000, newH000);
                var newH052 = Assets.GetAssetPath(Character.Fuuka, AssetType.HairMesh, newSEESHair);
                Redirect(H052, newH052);
                var newTitle = Assets.GetAssetPath(Character.Fuuka, AssetType.TitleMesh, newNormalHair);
                Redirect(Title, newTitle);
            }
        }

        if (haveCostumeApi)
        {

            var C104 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 104);
            var newC104 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 904);
            Redirect(C104, newC104);
            var thisModule = getModule(modDir,Module.Costumes);
            CostumeModule.LoadCostumes(costumeApi, unrealEssentials, thisModule);
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
        CharCreator = 3,
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