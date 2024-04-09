using P3RPC.PartyMember.FuukaOverhaul.Configuration;
using P3RPC.PartyMember.FuukaOverhaul.Template;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using P3RPC.PartyMember.FuukaOverhaul.Utils.Types;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using UnrealEssentials.Interfaces;
using System.Reflection;
using System.Data;
using Reloaded.Mod.Interfaces.Internal;
using System.Collections.Immutable;
using Reloaded.Mod.Interfaces.Structs.Enums;
using System.Collections;

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

    private readonly IUnrealEssentials? unrealEssentials;
    private bool haveUnrealEssentials;
    private readonly IUnreal? unrealEmitter;
    private bool haveUnrealEmitter;

    public string? modDirectory;

    private IEnumerable<string>? activeMods;

    public Mod(ModContext context)
    {
        _modLoader = context.ModLoader;
        _hooks = context.Hooks;
        _logger = context.Logger;
        _owner = context.Owner;
        _configuration = context.Configuration;
        _modConfig = context.ModConfig;

        haveUnrealEssentials = false;
        haveUnrealEmitter = false;
        // SET MOD DIRECTORY
        var modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
        if (modDir != null)
        {
            modDirectory = modDir;
        }
        else
        {
            modDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }



        Dictionary<string,Patch> PatchLibrary = new Dictionary<string, Patch> { };

        ModGenericTuple<IModConfigV1>[] mods = _modLoader.GetActiveMods();

        if (mods.Length > 0)
        {
            foreach (IModConfigV1 mod in mods) {
                var CheckMod = new Patch(mod);
                    CheckMod.ThisPatchExists.Equals(PatchExists(mod.ModId));
                    CheckMod.ThisPatchLoads.Equals(PatchLoads(mod.ModId));
                if (CheckMod.ThisPatchExists)
                {
                    PatchLibrary.Add(mod.ModId, CheckMod);
                    var name = CheckMod.ModName;
                    var author = CheckMod.ModAuthor;
                    if (CheckMod.ThisPatchLoads)
                    {
                        _logger.WriteLine($"[{modName}] | Compatibility patch found for \"{name}\" by {author}. This patch will be automatically applied.",System.Drawing.Color.PowderBlue);
                    }
                    else
                    {
                        _logger.WriteLine($"[{modName}] | Compatibility patch directory found for \"{name}\" by {author}, however it has no content and will not be applied.", System.Drawing.Color.Yellow);
                    }
                }
            }
        }

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
            haveUnrealEssentials.Equals(true);
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
            haveUnrealEmitter.Equals(true);
            _logger.WriteLine($"[{modName}] | Unreal Emitter loaded: {haveUnrealEmitter.ToString()}");
        }
        this.unrealEmitter = unrealEmitter;

        // For more information about this template, please see
        // https://reloaded-project.github.io/Reloaded-II/ModTemplate/

        // If you want to implement e.g. unload support in your mod,
        // and some other neat features, override the methods in ModBase.

        // TODO: Implement some mod logic



        // LOAD TEXTURES

        LoadModule(unrealEssentials, getModule(Module.Core));

        var hairStyle = Enum.GetName(_configuration.hairstyleSetting);
        var glassesStyle = Enum.GetName(_configuration.glassesSetting);
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
            LoadModule(unrealEssentials, getModule(Module.Debug));
        }

        if (hairStyle != null)
        {
            LoadModule(unrealEssentials, getModule(Module.Hair));
            if (hairStyle != "Vanilla")
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
        var patchesApplied = 0;
        foreach (KeyValuePair<string, Patch> patch in PatchLibrary)
        {
            var thisPatch = patch.Value;
            if (thisPatch.ThisPatchLoads)
            {
                var thisPatchID = thisPatch.ModID;
                var name = thisPatch.ModName;
                var author = thisPatch.ModAuthor;
                var thisPatchModule = Path.Combine("02_Patches", thisPatchID);
                LoadModule(unrealEssentials, thisPatchModule);
                patchesApplied++;
                _logger.WriteLine($"[{modName}] | Compatibility patch for \"{name}\" by {author} applied.", System.Drawing.Color.PowderBlue);
            }
        }
        if (patchesApplied > 0)
        {
            _logger.WriteLine($"[{modName}] | All compatibility patches applied.", System.Drawing.Color.PowderBlue);
        }
    }
    private void LoadModule(IUnrealEssentials unreal, string module)
    {
        try
        {
            var modulePath = Path.Combine(modDirectory, "Modules", module);
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
            if (module == null)
            {
                throw new ArgumentNullException($"No {module} module found", ex);
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
    private string PatchPath (string ReloadedModID)
    {
        return Path.Combine(modDirectory, "Modules", "02_Patches", ReloadedModID);
    }
    private bool PatchExists (string ReloadedModID)
    {
        var patchPath = PatchPath(ReloadedModID);
        return Directory.Exists(patchPath);
    }
    private bool PatchLoads(string ReloadedModID)
    {
        var ioEmulator = Path.Combine(PatchPath(ReloadedModID), "P3R");
        return Directory.Exists(ioEmulator);
    }

    private record AssetFNames(string assetFile)
    {
        public string AssetName { get; } = Path.GetFileNameWithoutExtension(assetFile);

        public string AssetPath { get; } = Assets.GetAssetPath(assetFile);
    }

    private record Patch (IModConfigV1 Mod)
    {
        public string ModID { get; } = Mod.ModId;

        public string ModName { get; } = Mod.ModName;

        public string ModAuthor { get; } = Mod.ModAuthor;

        public bool ThisPatchExists;
        public bool ThisPatchLoads;
    }
    private enum Module
    {
        Core = 0,
        Hair = 1,
        Patches = 2,
        CharCreator = 3,
        Debug = 4,
    }

    private string getModule (Module module)
    {
        var moduleID = ((int)module);
        var moduleName = Enum.GetName(typeof(Module), moduleID);
        var moduleFolder = String.Join("_", moduleID.ToString("00"), moduleName);
        
        try
        {
            var modulePath = Path.Combine(modDirectory, "Modules", moduleFolder);
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