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

                DisplayNames[55] = "Cave 2 Unsafe";
                DisplayNames[56] = "Cave 3 Unsafe";
                DisplayNames[57] = "Cave 4 Unsafe";
                DisplayNames[58] = "Cave 5 Unsafe";
                DisplayNames[59] = "Cave 6 Unsafe";
                DisplayNames[61] = "Cave 7 Unsafe";

            }

            return;

            DisplayNames[0]   = "Sky";
            DisplayNames[1]   = "Stone Wall";
            DisplayNames[2]   = "Dirt Wall - Natural";
            DisplayNames[3]   = "Ebonstone Wall - Natural";
            DisplayNames[4]   = "Wood Wall";
            DisplayNames[5]   = "Gray Brick Wall";
            DisplayNames[6]   = "Red Brick Wall";
            DisplayNames[7]   = "Blue Brick Wall - Natural";
            DisplayNames[8]   = "Green Brick Wall - Natural";
            DisplayNames[9]   = "Pink Brick Wall - Natural";
            DisplayNames[10]  = "Gold Brick Wall";
            DisplayNames[11]  = "Silver Brick Wall";
            DisplayNames[12]  = "Copper Brick Wall";
            DisplayNames[13]  = "Hellstone Brick Wall - Natural";
            DisplayNames[14]  = "Obsidian Brick Wall - Natural";
            DisplayNames[15]  = "Mud Wall - Natural";
            DisplayNames[16]  = "Dirt Wall";
            DisplayNames[17]  = "Blue Brick Wall";
            DisplayNames[18]  = "Green Brick Wall";
            DisplayNames[19]  = "Pink Brick Wall";
            DisplayNames[20]  = "Obsidian Brick Wall";
            DisplayNames[21]  = "Glass Wall";
            DisplayNames[22]  = "Pearlstone Brick Wall";
            DisplayNames[23]  = "Iridescent Brick Wall";
            DisplayNames[24]  = "Mudstone Brick Wall";
            DisplayNames[25]  = "Cobalt Brick Wall";
            DisplayNames[26]  = "Mythril Brick Wall";
            DisplayNames[27]  = "Planked Wall";
            DisplayNames[28]  = "Pearlstone Wall - Natural";
            DisplayNames[29]  = "Candy Cane Wall";
            DisplayNames[30]  = "Green Candy Cane Wall";
            DisplayNames[31]  = "Snow Brick Wall";
            DisplayNames[32]  = "Adamantite Beam Wall";
            DisplayNames[33]  = "Demonite Brick Wall";
            DisplayNames[34]  = "Sandstone Brick Wall";
            DisplayNames[35]  = "Ebonstone Brick Wall";
            DisplayNames[36]  = "Red Stucco Wall";
            DisplayNames[37]  = "Yellow Stucco Wall";
            DisplayNames[38]  = "Green Stucco Wall";
            DisplayNames[39]  = "Gray Stucco Wall";
            DisplayNames[40]  = "Snow Wall - Natural";
            DisplayNames[41]  = "Ebonwood Wall";
            DisplayNames[42]  = "Rich Mahogany Wall";
            DisplayNames[43]  = "Pearlwood Wall";
            DisplayNames[44]  = "Rainbow Brick Wall";
            DisplayNames[45]  = "Tin Brick Wall";
            DisplayNames[46]  = "Tungsten Brick Wall";
            DisplayNames[47]  = "Platinum Brick Wall";
            DisplayNames[48]  = "Amethyst Wall - Natural";
            DisplayNames[49]  = "Topaz Wall - Natural";
            DisplayNames[50]  = "Sapphire Wall - Natural";
            DisplayNames[51]  = "Emerald Wall - Natural";
            DisplayNames[52]  = "Ruby Wall - Natural";
            DisplayNames[53]  = "Diamond Wall - Natural";
            DisplayNames[54]  = "Unique Cave Wall 1 - Natural";
            DisplayNames[55]  = "Unique Cave Wall 2 - Natural";
            DisplayNames[56]  = "Unique Cave Wall 3 - Natural";
            DisplayNames[57]  = "Unique Cave Wall 4 - Natural";
            DisplayNames[58]  = "Unique Cave Wall 5 - Natural";
            DisplayNames[59]  = "Cave Wall - Natural";
            DisplayNames[60]  = "Leaves Wall - Natural";
            DisplayNames[61]  = "Unique Cave Wall 6 - Natural";
            DisplayNames[62]  = "Spider Wall - Natural";
            DisplayNames[63]  = "Grass Wall - Natural";
            DisplayNames[64]  = "Jungle Wall - Natural";
            DisplayNames[65]  = "Flower Wall - Natural";
            DisplayNames[66]  = "Grass Wall";
            DisplayNames[67]  = "Jungle Wall";
            DisplayNames[68]  = "Flower Wall";
            DisplayNames[69]  = "Corrupt Grass Wall - Natural";
            DisplayNames[70]  = "Hallowed Grass Wall - Natural";
            DisplayNames[71]  = "Ice Wall - Natural";
            DisplayNames[72]  = "Cactus Wall";
            DisplayNames[73]  = "Cloud Wall";
            DisplayNames[74]  = "Mushroom Wall";
            DisplayNames[75]  = "Bone Block Wall";
            DisplayNames[76]  = "Slime Block Wall";
            DisplayNames[77]  = "Flesh Block Wall";
            DisplayNames[78]  = "Living Wood Wall";
            DisplayNames[79]  = "Obsidian Back Wall - Natural";
            DisplayNames[80]  = "Mushroom Wall - Natural";
            DisplayNames[81]  = "Crimson Grass Wall - Natural";
            DisplayNames[82]  = "Disc Wall";
            DisplayNames[83]  = "Crimstone Wall - Natural";
            DisplayNames[84]  = "Ice Brick Wall";
            DisplayNames[85]  = "Shadewood Wall";
            DisplayNames[86]  = "Hive Wall - Natural";
            DisplayNames[87]  = "Lihzahrd Brick Wall - Natural";
            DisplayNames[88]  = "Purple Stained Glass";
            DisplayNames[89]  = "Yellow Stained Glass";
            DisplayNames[90]  = "Blue Stained Glass";
            DisplayNames[91]  = "Green Stained Glass";
            DisplayNames[92]  = "Red Stained Glass";
            DisplayNames[93]  = "Multicolored Stained Glass";
            DisplayNames[94]  = "Blue Slab Wall - Natural";
            DisplayNames[95]  = "Blue Tiled Wall - Natural";
            DisplayNames[96]  = "Pink Slab Wall - Natural";
            DisplayNames[97]  = "Pink Tiled Wall - Natural";
            DisplayNames[98]  = "Green Slab Wall - Natural";
            DisplayNames[99]  = "Green Tiled Wall - Natural";
            DisplayNames[100] = "Blue Slab Wall";
            DisplayNames[101] = "Blue Tiled Wall";
            DisplayNames[102] = "Pink Slab Wall";
            DisplayNames[103] = "Pink Tiled Wall";
            DisplayNames[104] = "Green Slab Wall";
            DisplayNames[105] = "Green Tiled Wall";
            DisplayNames[106] = "Wooden Fence";
            DisplayNames[107] = "Lead Fence";
            DisplayNames[108] = "Hive Wall";
            DisplayNames[109] = "Palladium Column Wall";
            DisplayNames[110] = "Bubblegum Block Wall";
            DisplayNames[111] = "Titanstone Block Wall";
            DisplayNames[112] = "Lihzahrd Brick Wall";
            DisplayNames[113] = "Pumpkin Wall";
            DisplayNames[114] = "Hay Wall";
            DisplayNames[115] = "Spooky Wood Wall";
            DisplayNames[116] = "Christmas Tree Wallpaper";
            DisplayNames[117] = "Ornament Wallpaper";
            DisplayNames[118] = "Candy Cane Wallpaper";
            DisplayNames[119] = "Festive Wallpaper";
            DisplayNames[120] = "Stars Wallpaper";
            DisplayNames[121] = "Squiggles Wallpaper";
            DisplayNames[122] = "Snowflake Wallpaper";
            DisplayNames[123] = "Krampus Horn Wallpaper";
            DisplayNames[124] = "Bluegreen Wallpaper";
            DisplayNames[125] = "Grinch Finger Wallpaper";
            DisplayNames[126] = "Fancy Grey Wallpaper";
            DisplayNames[127] = "Ice Floe Wallpaper";
            DisplayNames[128] = "Music Wallpaper";
            DisplayNames[129] = "Purple Rain Wallpaper";
            DisplayNames[130] = "Rainbow Wallpaper";
            DisplayNames[131] = "Sparkle Stone Wallpaper";
            DisplayNames[132] = "Starlit Heaven Wallpaper";
            DisplayNames[133] = "Bubble Wallpaper";
            DisplayNames[134] = "Copper Pipe Wallpaper";
            DisplayNames[135] = "Ducky Wallpaper";
            DisplayNames[136] = "Waterfall Wall";
            DisplayNames[137] = "Lavafall Wall";
            DisplayNames[138] = "Ebonwood Fence";
            DisplayNames[139] = "Rich Mahogany Fence";
            DisplayNames[140] = "Pearlwood Fence";
            DisplayNames[141] = "Shadewood Fence";
            DisplayNames[142] = "White Dynasty Wall";
            DisplayNames[143] = "Blue Dynasty Wall";
            DisplayNames[144] = "Arcane Rune Wall";
            DisplayNames[145] = "Iron Fence";
            DisplayNames[146] = "Copper Plating";
            DisplayNames[147] = "Stone Slab Wall";
            DisplayNames[148] = "Sail";
            DisplayNames[149] = "Boreal Wood Wall";
            DisplayNames[150] = "Boreal Wood Fence";
            DisplayNames[151] = "Palm Wood Wall";
            DisplayNames[152] = "Palm Wood Fence";
            DisplayNames[153] = "Amber Gemspark Wall";
            DisplayNames[154] = "Amethyst Gemspark Wall";
            DisplayNames[155] = "Diamond Gemspark Wall";
            DisplayNames[156] = "Emerald Gemspark Wall";
            DisplayNames[157] = "Offline Amber Gemspark Wall";
            DisplayNames[158] = "Offline Amethyst Gemspark Wall";
            DisplayNames[159] = "Offline Diamond Gemspark Wall";
            DisplayNames[160] = "Offline Emerald Gemspark Wall";
            DisplayNames[161] = "Offline Ruby Gemspark Wall";
            DisplayNames[162] = "Offline Sapphire Gemspark Wall";
            DisplayNames[163] = "Offline Topaz Gemspark Wall";
            DisplayNames[164] = "Ruby Gemspark Wall";
            DisplayNames[165] = "Sapphire Gemspark Wall";
            DisplayNames[166] = "Topaz Gemspark Wall";
            DisplayNames[167] = "Tin Plating Wall";
            DisplayNames[168] = "Confetti Wall";
            DisplayNames[169] = "Midnight Confetti Wall";
            DisplayNames[170] = "Unique Cave Wall 7 - Natural";
            DisplayNames[171] = "Unique Cave Wall 8 - Natural";
            DisplayNames[172] = "Honeyfall Wall";
            DisplayNames[173] = "Chlorophyte Brick Wall";
            DisplayNames[174] = "Crimtane Brick Wall";
            DisplayNames[175] = "Shroomite Plating Wall";
            DisplayNames[176] = "Martian Conduit Wall";
            DisplayNames[177] = "Hellstone Brick Wall";
            DisplayNames[178] = "Marble Wall - Natural";
            DisplayNames[179] = "Marble Wall";
            DisplayNames[180] = "Granite Wall - Natural";
            DisplayNames[181] = "Granite Wall";
            DisplayNames[182] = "Meteorite Brick Wall";
            DisplayNames[183] = "Marble Wall";
            DisplayNames[184] = "Granite Wall";
            DisplayNames[185] = "Cave Wall - Natural";
            DisplayNames[186] = "Crystal Block Wall";
            DisplayNames[187] = "Sandstone Wall";
            DisplayNames[188] = "Corruption Wall - Natural";
            DisplayNames[189] = "Corruption Wall 2 - Natural";
            DisplayNames[190] = "Corruption Wall 3 - Natural";
            DisplayNames[191] = "Corruption Wall 4 - Natural";
            DisplayNames[192] = "Crimson Wall - Natural";
            DisplayNames[193] = "Crimson Wall 2 - Natural";
            DisplayNames[194] = "Crimson Wall 3 - Natural";
            DisplayNames[195] = "Crimson Wall 4 - Natural";
            DisplayNames[196] = "Dirt Wall - Natural";
            DisplayNames[197] = "Dirt Wall 2 - Natural";
            DisplayNames[198] = "Dirt Wall 3 - Natural";
            DisplayNames[199] = "Dirt Wall 4 - Natural";
            DisplayNames[200] = "Hallow Wall - Natural";

        }
        
    }

}
