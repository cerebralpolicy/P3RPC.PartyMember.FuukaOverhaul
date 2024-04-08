using P3RPC.PartyMember.FuukaOverhaul.ModAssets;
using P3RPC.PartyMember.FuukaOverhaul.Redirector.Types;
using P3RPC.PartyMember.FuukaOverhaul.Redirector.TypesReplaced;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3RPC.PartyMember.FuukaOverhaul.Redirector;

public class AssetDictionaries
{
    public class OutfitDEF
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Redirect { get; set; }
    }
    public class HairDEF
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Redirect { get; set; }
    }
    public static IDictionary Outfits(Character chr)
    {
        var OutfitDictionary = new Dictionary<int, OutfitDEF>();
        foreach (var outfit in Enum.GetValues(typeof(Outfit)))
        {
            int outfitID = (int)outfit;
            var outfitName = Enum.GetName(typeof(Outfit), outfitID);
            if (outfitName != null)
            {
                OutfitDictionary.Add(outfitID, new OutfitDEF
                {
                    ID = outfitID,
                    Name = outfitName.ToString(),
                    Path = Assets.GetAssetPath(chr, AssetType.CostumeMesh, outfitID)
                }
                );
            }
        }
        foreach (var outfit in Enum.GetValues(typeof(OutfitReplaced)))
        {
            int outfitID = (int)outfit;
            var existingDEF = OutfitDictionary[outfitID];
            if (existingDEF.Path != null)
            {
                existingDEF.Redirect = Assets.GetNewAssetPath(existingDEF.Path);
            }
        }
        return OutfitDictionary;
    }
    public static IDictionary Hairs(Character chr, Configuration.Config.HairstyleSetting hairstyleSetting)
    {
        var HairDictionary = new Dictionary<int, HairDEF>();
        foreach (var hair in Enum.GetValues(typeof(Hair)))
        {
            int hairID = (int)hair;
            var hairName = Enum.GetName(typeof(Hair), hairID);

            if (hairName != null)
            {
                HairDictionary.Add(hairID, new HairDEF
                {
                    ID = hairID,
                    Name = hairName.ToString(),
                    Path = Assets.GetAssetPath(chr, AssetType.CostumeMesh, hairID)
                }
                );
            }
            else
            {
                HairDictionary.Add(hairID, new HairDEF
                {
                    ID = hairID,
                    Path = Assets.GetAssetPath(chr, AssetType.CostumeMesh, hairID)
                }
                );
            };
        }
        if (hairstyleSetting != Configuration.Config.HairstyleSetting.Vanilla) {
            foreach (var hair in Enum.GetValues(typeof(HairReplaced)))
            {
                int baseHairID = (int)hair;
                int newHairID = 0;
                if (baseHairID == 0)
                {
                    if (hairstyleSetting == Configuration.Config.HairstyleSetting.Ponytail)
                    {
                        newHairID = (int)StandardHairAssetID.Ponytail;
                    }
                    else
                    {
                        newHairID = (int)StandardHairAssetID.Bangs_Ponytail;
                    }
                }
                else if (baseHairID == 52)
                {
                    if (hairstyleSetting == Configuration.Config.HairstyleSetting.Ponytail)
                    {
                        newHairID = (int)BattleHairAssetID.Ponytail;
                    }
                    else
                    {
                        newHairID = (int)BattleHairAssetID.Bangs_Ponytail;
                    }
                }
                var existingDEF = HairDictionary[baseHairID];
                if (existingDEF.Path != null)
                {
                    existingDEF.Redirect = Assets.GetNewAssetPath(existingDEF.Path, baseHairID, newHairID);
                }
            }
        }
        return HairDictionary;
    }
}
