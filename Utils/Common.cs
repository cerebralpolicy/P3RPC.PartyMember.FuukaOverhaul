namespace P3RPC.PartyMember.FuukaOverhaul.Utils;
internal static class Common
{
    public static void DoIfString(string? str, Action<string> action)
    {
        if (string.IsNullOrEmpty(str) == false)
        {
            action(str);
        }
    }
}