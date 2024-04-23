
using P3R.CostumeFramework.Interfaces;
using static P3RPC.PartyMember.FuukaOverhaul.Core;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using P3RPC.PartyMember.FuukaOverhaul.Utils.Types;
using Unreal.AtlusScript.Interfaces;
using UnrealEssentials.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using AssetType = P3RPC.PartyMember.FuukaOverhaul.Utils.Types.AssetType;
using P3RPC.PartyMember.FuukaOverhaul.Modules.Costumes;
using P3RPC.PartyMember.FuukaOverhaul.Modules.Costumes.Types;

namespace P3RPC.PartyMember.FuukaOverhaul.Modules;

public class CostumeModule
{
    public const string Halo_Name = "Prodigal Scientist Parka";
    public const string Halo_Desc = "[uf 0 5 65278][uf 2 1]A snug outfit for Fuuka that resembles the attire her favorite video game character wore as a young woman.[n][e]";
    public static void LoadCostumes(ICostumeApi costumeApi, IUnrealEssentials unrealEssentials, string moduleDir)
    {
        var thisModule = moduleDir;
        unrealEssentials.AddFromFolder(thisModule);

        var overridesFile = Path.Combine(thisModule, "CostumeOverride.yaml");
        costumeApi.AddOverridesFile(overridesFile);
    }

    public static void OverrideCostumes(IAtlusAssets atlusAssets, IUnreal unrealEmitter, IUObjects uObjects, IUnrealEssentials unrealEssentials, string moduleDir)
    {
        var thisModule = moduleDir;
        unrealEssentials.AddFromFolder(thisModule);

        var C104 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 104);
        var newC104 = Assets.GetAssetPath(Character.Fuuka, AssetType.CostumeMesh, 904);
        Redirect(C104, newC104, unrealEmitter);
    }
}