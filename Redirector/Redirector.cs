using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P3RPC.PartyMember.FuukaOverhaul.Utils;
using Unreal.ObjectsEmitter;
using Unreal.ObjectsEmitter.Interfaces;

namespace P3RPC.PartyMember.FuukaOverhaul.Redirector;

public class RedirectorService
{
    private readonly IUnreal unreal;
    public RedirectorService(IUnreal unreal)
    {
        this.unreal = unreal;
    }

    public void AssetRedirector(string vanillaAssetPath, string modAssetPath)
    {
        var van = vanillaAssetPath;
        var mod = modAssetPath;
        if (van == mod)
        {
            return;
        }

        var vanFNames = new AssetFNames(van);
        var modFNames = new AssetFNames(mod);

    }

    private record AssetFNames(string assetFile)
    {
        public string AssetName { get; } = Path.GetFileNameWithoutExtension(assetFile);

        public string AssetPath { get; } = Assets.GetAssetPath(assetFile);
    }
}
