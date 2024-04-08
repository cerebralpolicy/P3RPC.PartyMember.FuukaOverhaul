using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace P3RPC.PartyMember.FuukaOverhaul.Utils
{
    public class PathLogic
    {
        public static string CharModelPath (string modPath, bool partyMember, int characterID)
        {
            var CharFolder = Path.Combine(modPath, "Staged", "P3R", "Content", "Xrd777", "Characters");
            if (partyMember) { 
                string charID = "PC" + int.Parse(characterID.ToString("D4"));
                var CharSubDir = Path.Combine(CharFolder, "Player", charID);
                return CharSubDir;
            }
            else {
                string charID = "SC" + int.Parse(characterID.ToString("D4"));
                var CharSubDir = Path.Combine(CharFolder, "Sub", charID);
                return CharSubDir;
            }
        }
    }
}
