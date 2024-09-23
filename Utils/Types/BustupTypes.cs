using System.Text;

namespace P3RPC.PartyMember.FuukaOverhaul.Utils.Types;

public enum BustupComponent
{
    BaseLayer,
    Emote,
}

[Flags]
public enum HairFlag
{
    Vanilla = 1 << 0,
    Glasses = 1 << 1,
    Ponytail = 1 << 2,
}

public static class HairFlags
{
    public static string ToSuffix(this HairFlag flag)
    {
        List<string> list = [];
        if (flag.HasFlag(HairFlag.Ponytail))
        {
            list.Add("Pony");
        }
        if (flag.HasFlag(HairFlag.Vanilla))
        {
            list.Add("Vanilla");
        }
        if (flag.HasFlag(HairFlag.Glasses))
        {
            list.Add("Glasses");
        }
        var sb = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            if (i > 0)
            {
                sb.Append('_');
            }
            sb.Append(list[i]);
        }
        return sb.ToString();
    }
    public static int ToId(this HairFlag flag)
    {
        int id = 0;
        if (flag.HasFlag(HairFlag.Ponytail))
            id++;
        if (flag.HasFlag(HairFlag.Glasses))
            id += 10;
        return id;
    }
}