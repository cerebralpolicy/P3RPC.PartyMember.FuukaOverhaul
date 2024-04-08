namespace P3RPC.PartyMember.FuukaOverhaul.Utils.Types;

public class CharacterConfig
{
    public CharacterBase Base { get; set; } = new();

    public CharacterAnims Animations { get; set; } = new();

    public Dictionary<Outfit, string?> Outfits { get; set; } = new();
}

public class CharacterBase
{
    public Character Character { get; set; } = Character.Player;

    public string? BaseSkeleton { get; set; }

    public string? Hair { get; set; }

    public string? Face { get; set; }
}

public class CharacterAnims
{
    public string? Common { get; set; }

    public string? Combine { get; set; }

    public string? Event { get; set; }

    public string? Face { get; set; }
}