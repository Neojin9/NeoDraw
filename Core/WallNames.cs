using System.Text.RegularExpressions;
using Terraria.ID;

namespace NeoDraw.Core {

    public class WallNames {

        public static string[] DisplayNames = new string[WallID.Count];

        public static void Setup() {

            DisplayNames = new string[WallID.Count];

            for (int i = 0; i < WallID.Count; i++) {

                string displayName = WallID.Search.GetName(i) ?? "";

                displayName = Regex.Replace(displayName, "([A-Z])", " $1").Trim();

                DisplayNames[i] = displayName;

            }

            DisplayNames[55] = "Cave 2 Unsafe";
            DisplayNames[56] = "Cave 3 Unsafe";
            DisplayNames[57] = "Cave 4 Unsafe";
            DisplayNames[58] = "Cave 5 Unsafe";
            DisplayNames[59] = "Cave 6 Unsafe";
            DisplayNames[61] = "Cave 7 Unsafe";

        }

    }

}
