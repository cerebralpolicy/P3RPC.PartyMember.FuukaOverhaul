
using P3R.CostumeFramework.Interfaces;
using static P3RPC.PartyMember.FuukaOverhaul.Core;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using P3RPC.PartyMember.FuukaOverhaul.Utils.Types;
using Unreal.AtlusScript.Interfaces;
using UnrealEssentials.Interfaces;
using Unreal.ObjectsEmitter.Interfaces;
using AssetType = P3RPC.PartyMember.FuukaOverhaul.Utils.Types.AssetType;
namespace P3RPC.PartyMember.FuukaOverhaul.Modules;

public class Costumes
{
    public const string Halo_Name = "Prodigal Scientist Parka";
    public const string Halo_Desc = "[uf 0 5 65278][uf 2 1]A snug outfit for Fuuka that resembles the attire her favorite video game character wore as a young woman.[n][e]";

    public ICostumeApi CostumeApi { get; set; }

    public Costumes(ICostumeApi costumeApi)
    {
        CostumeApi = costumeApi;
    }

    public static void LoadOverride(ICostumeApi costumeApi, string moduleDir, string overrideFile = "CostumeOverride.yaml")
    {

        var _override = Path.Join(moduleDir,overrideFile);

        costumeApi.AddOverridesFile(_override);
    }
}