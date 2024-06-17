using P3RPC.PartyMember.FuukaOverhaul.Utils.Types;
using static P3RPC.PartyMember.FuukaOverhaul.Utils.Lists.BustupsReplaced;

namespace P3RPC.PartyMember.FuukaOverhaul.Utils;

internal static class Assets
{
    // FORMATTING
    public static string FormatCharID(Character chr)
    {
        return ((int)chr).ToString("0000");
    }
    public static string FormatAssetPath(string assetFile, Character chr)
    {
        var formattedPath = assetFile.Replace("\\", "/").Replace(".uasset",string.Empty);
        if (!formattedPath.StartsWith("/Game/"))
        {
            formattedPath = $"/Game/{formattedPath}";
        }
        return formattedPath;
    }
    //PATHING
    public static string GetCharPath(Character chr)
    {
        var path = $"/Cerebral/Characters/Player/PC{FormatCharID(chr)}/Models/";
        return path;
    }
    public static string GetAssetPath(Character chr, AssetType asset, int assetID = 0)
        => asset switch
        {
            AssetType.BaseMesh => FormatAssetPath($"/Game/Xrd777/Characters/Player/PC{FormatCharID(chr)}/Models/SK_PC{FormatCharID(chr)}_BaseSkelton", chr),
            AssetType.CostumeMesh => FormatAssetPath($"/Game/Xrd777/Characters/Player/PC{FormatCharID(chr)}/Models/SK_PC{FormatCharID(chr)}_C{assetID:000}", chr),
            AssetType.HairMesh => FormatAssetPath($"/Game/Xrd777/Characters/Player/PC{FormatCharID(chr)}/Models/SK_PC{FormatCharID(chr)}_H{assetID:000}", chr),
            AssetType.FaceMesh => FormatAssetPath($"/Game/Xrd777/Characters/Player/PC{FormatCharID(chr)}/Models/SK_PC{FormatCharID(chr)}_F{assetID:000}", chr),
            AssetType.CommonAnim => FormatAssetPath($"/Game/Xrd777/Characters/Data/DataAsset/Player/PC{FormatCharID(chr)}/DA_PC{FormatCharID(chr)}_CommonAnim", chr),
            AssetType.CombineAnim => FormatAssetPath($"/Game/Xrd777/Characters/Data/DataAsset/Player/PC{FormatCharID(chr)}/DA_PC{FormatCharID(chr)}_CombineAnim", chr),
            AssetType.DungeonAnim => FormatAssetPath($"/Game/Xrd777/Characters/Data/DataAsset/Player/PC{FormatCharID(chr)}/DA_PC{FormatCharID(chr)}_DungeonAnim", chr),
            AssetType.EventAnim => FormatAssetPath($"/Game/Xrd777/Characters/Data/DataAsset/Player/PC{FormatCharID(chr)}/DA_PC{FormatCharID(chr)}_EventAnim", chr),
            AssetType.FaceAnim => FormatAssetPath($"/Game/Xrd777/Characters/Data/DataAsset/Player/PC{FormatCharID(chr)}/DA_PC{FormatCharID(chr)}_FaceAnim", chr),
            AssetType.TitleMesh => FormatAssetPath($"/Game/Xrd777/Characters/Player/PC{FormatCharID(chr)}/Models/SK_PC{FormatCharID(chr)}_Title_{assetID:00}", chr),
            _ => throw new Exception(),
        };
    public static string GetAssetFile(Character character, AssetType type, Outfit outfit)
    => GetAssetPath(character, type, (int)outfit);
    public static string GetAssetPath(string assetFile)
    {
        var adjustedPath = assetFile.Replace('\\', '/').Replace(".uasset", string.Empty);

        if (adjustedPath.IndexOf("Content") is int contentIndex && contentIndex > -1)
        {
            adjustedPath = adjustedPath.Substring(contentIndex + 8);
        }

        if (!adjustedPath.StartsWith("/Game/"))
        {
            adjustedPath = $"/Game/{adjustedPath}";
        }
        return adjustedPath;
    }
    public static string Bustup(string bustUpName, Character chr)
    {
        var chrIndex = (int)chr;
        string? path;
        if (chrIndex < 100)
        {
            path = FormatAssetPath($"/Game/Content/Xrd777/UI/Bustup/Textures/PC{FormatCharID(chr)}/{bustUpName}", chr);
        }
        else
        {
            path = FormatAssetPath($"/Game/Content/Xrd777/UI/Bustup/Textures/SC{FormatCharID(chr)}/{bustUpName}", chr);
        }
        return path;
    }
    public static string NewBustup(string bustUpName, Character chr, BustupComponent type, int hairAssetID = 0)
    {
        var chrIndex = (int)chr;
        string? path;
        string? subfolder;
        if (type == BustupComponent.Emote)
        {
            subfolder = "Glasses";
        }
        else
        {
            subfolder = $"H{hairAssetID:000}";
        }

        if (chrIndex < 100)
        {
            path = FormatAssetPath($"/Game/Content/Xrd777/UI/Bustup/Textures/PC{FormatCharID(chr)}/{subfolder}/{bustUpName}", chr);
        }
        else
        {
            path = FormatAssetPath($"/Game/Content/Xrd777/UI/Bustup/Textures/SC{FormatCharID(chr)}/{subfolder}/{bustUpName}", chr);
        }
        return path;
    }
    public static string[] GetBustUpPaths(string[] bustupCollection, Character chr)
    {

        List<string> paths = [];
        foreach (var bustup in bustupCollection)
        {
            string pathTo = Bustup(bustup, chr);
            paths.Add(pathTo);
        }
        return [.. paths];
    }
}
