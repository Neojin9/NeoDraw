using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NeoDraw.Core {

    public class TileNames {

        public static int TileCount;
        public static int OriginalTileCount;
        public static int ModTileCount;

        public static string[] DisplayNames = new string[TileID.Count];
        public static Dictionary<int, List<string>> SubTileNames = new Dictionary<int, List<string>>();  

        public static void Setup() {

            OriginalTileCount = TileID.Count;
            TileCount = GetInstance<NeoConfigServer>().ModTilesAllowed ? TileLoader.TileCount : TileID.Count;
            ModTileCount = TileCount - OriginalTileCount;
            
            DisplayNames = new string[TileCount];

            for (int i = 0; i < TileCount; i++) {

                string displayName;

                if (i < OriginalTileCount) {
                    displayName = TileID.Search.GetName(i);
                }
                else {

                    ModTile modTile = TileLoader.GetTile(i);

                    if (modTile != null) {
                        displayName = modTile.Name + " - " + modTile.mod.Name;
                    }
                    else {
                        displayName = i.ToString();
                    }

                }

                displayName = Regex.Replace(displayName, "([A-Z])", " $1").Trim();

                DisplayNames[i] = displayName;

            }

            DisplayNames[3]   = "Short Grass Plants";
            DisplayNames[21]  = "Chests";
            DisplayNames[31]  = "Orb / Heart";
            DisplayNames[61]  = "Short Jungle Plants";
            DisplayNames[73]  = "Tall Grass Plants";
            DisplayNames[74]  = "Tall Jungle Plants";
            DisplayNames[110] = "Short Hallowed Plants";
            DisplayNames[113] = "Tall Hallowed Plants";
            DisplayNames[201] = "Short Flesh Weeds";
            DisplayNames[217] = "Blend-O-Matic";
            DisplayNames[231] = "Queen Bee Larva";
            DisplayNames[240] = "Trophy / Painting 3x3";
            DisplayNames[241] = "Catacomb / Painting 4x3";
            DisplayNames[242] = "Painting 6x4";
            DisplayNames[245] = "Painting 2x3";
            DisplayNames[246] = "Painting 3x2";
            DisplayNames[270] = "Firefly in a Bottle";
            DisplayNames[271] = "Lightning Bug in a Bottle";
            DisplayNames[274] = "Sandstone Slab";
            DisplayNames[424] = "Junction Box";
            DisplayNames[441] = "Fake Chests";
            DisplayNames[467] = "Chests 2";
            DisplayNames[468] = "Trapped Chests";
            DisplayNames[469] = "Tables 2";

        }
        
        public static void SetupSubTiles() {

            SubTileNames = new Dictionary<int, List<string>>();

            SubTileNames.Add(TileID.Plants,                new List<string> {
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Blade 5", "Blade 6", "Flower 1", "Flower 2", "Mushroom", "Flower 3", "Flower 4", "Red Flower 1", "Red Flower 2", "Red Flower 3",
                "Yellow Flower 1", "Yellow Flower 2", "Yellow Flower 3", "White Flower 1", "White Flower 2", "White Flower 3", "Magenta Flower", "Purple Flower", "Violet Flower" });            
            SubTileNames.Add(TileID.Torches,               new List<string> {
                "Normal", "Blue", "Red", "Green", "Purple", "White", "Yellow", "Obsidian", "Cursed", "Ice", "Orange", "Ichor", "Ultrabright", "Bone", "Rainbow", "Pink" });
            SubTileNames.Add(TileID.ClosedDoor,            new List<string> {
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Cactus", "Flesh", "Mushroom", "Living Wood", "Bone", "Skyware", "Shadewood", "Lihzahrd01", "Lihzahrd02", "Dungeon", "Lead", "Iron",
                "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian", "Glass", "Golden", "Honey", "Steampunk", "Pumpkin", "Spooky", "Pine", "Frozen", "Dynasty", "Palm", "Boreal", "Slime",
                "Martian Door", "Meteorite Door", "Granite Door", "Marble Door", "Crystal Door" });
            SubTileNames.Add(TileID.OpenDoor,              new List<string> { 
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Cactus", "Flesh", "Mushroom", "Living Wood", "Bone", "Skyware", "Shadewood", "Lihzahrd01", "Lihzahrd02", "Dungeon", "Lead", "Iron",
                "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian", "Glass", "Golden", "Honey", "Steampunk", "Pumpkin", "Spooky", "Pine", "Frozen", "Dynasty", "Palm", "Boreal", "Slime",
                "Martian Door", "Meteorite Door", "Granite Door", "Marble Door", "Crystal Door" });
            SubTileNames.Add(TileID.Bottles,               new List<string> {
                "Bottle", "Health Potion", "Mana Potion", "Pink Vase", "Mug", "Dynasty Cup", "Wine Glass", "Honey Cup", "Chalice" });
            SubTileNames.Add(TileID.Tables,                new List<string> { 
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Bone", "Flesh", "Living Wood", "Skyware", "Shadewood", "Lihzahrd", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian",
                "Gothic", "Glass", "Banquet", "Bar", "Golden", "Honey", "Steampunk", "Pumpkin", "Spooky", "Pine", "Frozen", "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Cactus", "Martian",
                "Meteorite", "Granite", "Marble"/*, "Crystal"*/ });
            SubTileNames.Add(TileID.Chairs,                new List<string> {
                "Wooden", "Toilet", "Ebonwood", "Rich Mahogany", "Pearlwood", "Living Wood", "Cactus", "Bone", "Flesh", "Mushroom", "Sky", "Shadewood", "Lihzahrd", "Blue Dungeon", "Green Dungeon",
                "Pink Dungeon", "Obsidian", "Gothic", "Glass", "Golden", "Golden Toilet", "Bar Stool", "Honey", "Steampunk", "Pumpkin", "Spooky", "Pine", "Dynasty", "Frozen", "Palm", "Boreal", "Slime",
                "Martian Hover", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Anvils,                new List<string> {
                "Iron", "Lead" });
            SubTileNames.Add(TileID.WorkBenches,           new List<string> {
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Bone", "Cactus", "Flesh", "Mushroom", "Slime", "Shadewood", "Lihzahrd", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian",
                "Gothic", "Pumpkin", "Spooky", "Dynasty", "Honey", "Frozen", "Steampunk", "Palm", "Boreal", "Sky", "Glass", "Living Wood", "Martian", "Meteorite", "Granite", "Marble", "Crystal", "Golden" });
            SubTileNames.Add(TileID.Platforms,             new List<string> {
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Bone", "Shadewood", "Blue Dungeon", "Pink Dungeon", "Green Dungeon", "Metal Shelf", "Brass Shelf", "Wood Shelf", "Dungeon Shelf",
                "Obsidian", "Glass", "Pumpkin", "Spooky", "Palm", "Mushroom", "Boreal", "Slime", "Steampunk", "Sky", "Living Wood", "Honey", "Cactus", "Martian", "Meteorite", "Granite", "Marble",
                "Crystal", "Golden", "Dynasty", "Lihzahrd", "Flesh", "Frozen" });
            SubTileNames.Add(TileID.Containers,            new List<string> {
                "Wooden", "Gold", "Gold - Locked", "Shadow", "Shadow - Locked", "Barrel", "Trash Can", "Ebonwood", "Rich Mahogany", "Pearlwood", "Ivy", "Ice", "Living Wood", "Skyware", "Obsidian",
                "Web Covered", "Lihzahrd", "Water", "Biome - Jungle", "Biome - Corruption", "Biome - Crimson", "Biome - Hallowed", "Biome - Frozen", "Biome - Jungle - Locked", "Biome - Corruption - Locked",
                "Biome - Crimson - Locked", "Biome - Hallowed - Locked", "Biome - Frozen - Locked", "Dynasty", "Honey", "Steampunk", "Palm", "Mushroom", "Boreal", "Slime", "Green Dungeon",
                "Green Dungeon - Locked", "Pink Dungeon", "Pink Dungeon - Locked", "Blue Dungeon", "Blue Dungeon - Locked", "Bone", "Cactus", "Flesh", "Obsidian", "Pumpkin", "Spooky", "Glass", "Martian",
                "Meteorite", "Granite", "Marble", "Crystal", "Golden" });
            SubTileNames.Add(TileID.CorruptPlants,         new List<string> {
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Blade 5", "Blade 6", "Flower 1", "Flower 2", "Mushroom", "Plant 1", "Plant 2", "Plant 3", "Plant 4", "Plant 5", "Plant 6", "Plant 7", "Plant 8",
                "Plant 9", "Plant 10", "Plant 11", "Plant 12", "Plant 13", "Plant 14" });
            SubTileNames.Add(TileID.DemonAltar,            new List<string> {
                "Demon Altar", "Crimson Altar" });
            SubTileNames.Add(TileID.Pots,                  new List<string> {
                "Forest", "Tundra", "Jungle", "Dungeon", "Underworld", "Corrupt", "Spider", "Flesh", "Egyptian", "Lihzahrd", "Marble" });
            SubTileNames.Add(TileID.ShadowOrbs,            new List<string> {
                "Shadow Orb", "Crimson Heart" });            
            SubTileNames.Add(TileID.Candles,               new List<string> {
                "Candle", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Cactus", "Ebonwood", "Flesh", "Glass", "Frozen", "Rich Mahogany", "Pearlwood", "Lihzahrd", "Skyware", "Pumpkin", "Living Wood",
                "Shadewood", "Golden", "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Honey", "Steampunk", "Spooky", "Obsidian" });
            SubTileNames.Add(TileID.Chandeliers,           new List<string> {
                "Copper", "Silver", "Gold", "Tin", "Tungsten", "Platinum", "Jackelier", "Cactus", "Ebonwood", "Flesh", "Honey", "Frozen", "Rich Mahogany", "Pearlwood", "Lihzahrd", "Skyware", "Spooky",
                "Glass", "Living Wood", "Shadewood", "Golden", "Bone", "Dynasty", "Palm", "Mushroom",  "Boreal", "Slime", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Steampunk", "Pumpkin", "Obsidian" });
            SubTileNames.Add(TileID.Jackolanterns,         new List<string> {
                "Glare", "Happy", "Frown", "Smile", "Sad", "Cry", "Laugh", "Stitched", "Jack S." });
            SubTileNames.Add(TileID.Presents,              new List<string> {
                "Red & White", "Red & Blue", "Green & White", "Green & Red", "Yellow & White", "Yellow & Green", "Blue & White", "Blue & Yellow" });
            SubTileNames.Add(TileID.HangingLanterns,       new List<string> {
                "Chain", "Brass", "Caged", "Carriage", "Alchemy", "Diablost", "Oil Rag", "Bottle", "Jack 'O Lantern", "Heart", "Cactus", "Ebonwood", "Flesh", "Honey", "Steampunk", "Glass",
                "Rich Mahogany", "Pearlwood", "Frozen", "Lihzahrd", "Skyware", "Spooky", "Living Wood", "Shadewood", "Golden", "Bone", "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Pumpkin",
                "Obsidian", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Books,                 new List<string> {
                "Set 1", "Set 2", "Set 3", "Set 4", "Set 5", "Set 6" });
            SubTileNames.Add(TileID.JunglePlants,          new List<string> {
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Blade 5", "Blade 6", "Flower 1", "Flower 2", "Spore", "Flower 3" }); // Add the missing 13 other
            SubTileNames.Add(TileID.MushroomPlants,        new List<string> {
                "Small Mushroom 1", "Small Mushroom 2", "Mushroom Group", "Mushroom 1", "Mushroom 2" });            
            SubTileNames.Add(TileID.Plants2,               new List<string> {
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Blade 5", "Blade 6", "Flower 1", "Flower 2", "Blank", "Flower 3", "Flower 4", "Flower 5", "Flower 6", "Flower 7", "Flower 8", "Flower 9",
                "Flower 10", "Flower 11", "Flower 12", "Flower 13", "Flower 14" });
            SubTileNames.Add(TileID.JunglePlants2,         new List<string> {
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Blade 5", "Blade 6", "Flower 1", "Flower 2", "Blank", "Flower 3", "Flower 4", "Flower 5", "Flower 6", "Flower 7", "Flower 8", "Flower 9",
                "Flower 10" });
            SubTileNames.Add(TileID.Beds,                  new List<string> {
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Shadewood", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian", "Glass", "Golden", "Honey", "Steampunk", "Cactus", "Flesh",
                "Frozen", "Lihzahrd", "Skyware", "Spooky", "Living Wood", "Bone", "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Pumpkin", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Coral,                 new List<string> {
                "Coral", "Pink", "Yellow", "Green", "Blue - Small", "Yellow - Small" });
            SubTileNames.Add(TileID.ImmatureHerbs,         new List<string> {
                "Daybloom", "Moonglow", "Blinkroot", "Deathweed", "Waterleaf", "Fireblossom", "Shiverthorn" });
            SubTileNames.Add(TileID.MatureHerbs,           new List<string> {
                "Daybloom", "Moonglow", "Blinkroot", "Deathweed", "Waterleaf", "Fireblossom", "Shiverthorn" });
            SubTileNames.Add(TileID.BloomingHerbs,         new List<string> {
                "Daybloom", "Moonglow", "Blinkroot", "Deathweed", "Waterleaf", "Fireblossom", "Shiverthorn" });
            SubTileNames.Add(TileID.Tombstones,            new List<string> {
            "Tombstone", "Grave Marker", "Cross Grave Marker", "Headstone", "Gravestone", "Obelisk" });
            SubTileNames.Add(TileID.Pianos,                new List<string> {
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Shadewood", "Living Wood", "Flesh", "Frozen", "Glass", "Honey", "Steampunk", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Golden", "Obsidian",
                "Bone", "Cactus", "Spooky", "Skyware", "Lihzahrd", "Palm", "Mushroom", "Boreal", "Slime", "Pumpkin", "Martian", "Meteorite", "Granite", "Marble", "Crystal", "Dynasty" });
            SubTileNames.Add(TileID.Dressers,              new List<string> {
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Shadewood", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Golden", "Obsidian", "Bone", "Cactus", "Spooky", "Skyware", "Honey",
                "Lihzahrd", "Palm", "Mushroom", "Boreal", "Slime", "Pumpkin", "Steampunk", "Glass", "Flesh", "Frozen", "Meteorite", "Granite", "Marble", "Crystal", "Dynasty", "Frozen", "Living Wood" });
            SubTileNames.Add(TileID.Benches,               new List<string> {
                "Wooden Bench", "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Shadewood", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Golden", "Obsidian", "Bone", "Cactus", "Spooky", "Skyware",
                "Honey", "Steampunk", "Mushroom", "Glass", "Pumpkin", "Lihzahrd", "Palm Bench", "Palm", "Mushroom Bench", "Boreal", "Slime", "Flesh", "Frozen", "Living Wood", "Martian", "Meteorite", "Granite",
                "Marble", "Crystal", "Dynasty" });
            SubTileNames.Add(TileID.Bathtubs,              new List<string> {
                "Metal", "Cactus", "Ebonwood", "Flesh", "Glass", "Frozen", "Rich Mahogany", "Pearlwood", "Lihzahrd", "Skyware", "Spooky", "Honey", "Steampunk", "Living Wood", "Shadewood", "Bone", "Dynasty",
                "Palm", "Mushroom", "Boreal", "Slime", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Pumpkin", "Obsidian", "Golden", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Banners,               new List<string> { 
                "Red",              "Green",        "Blue",           "Yellow",          "Ankh",           "Snake",           "Omega",           "World",            "Sun",            "Gravity",
                "Marching Bones",   "Necromantic",  "Rusted Company", "Ragged",          "Molten",         "Diabolic",        "Hellbound",       "Hell Hammer",      "Helltower",      "Lost Hopes of Man",
                "Obsidian",         "Lava Erupts",  "Angler Fish",    "Angry Nimbus",    "Anomura Fungus", "Antlion",         "Arapaima",        "Armored Skeleton", "Cave Bat",       "Bird",
                "Black Recluse",    "Blood Feeder", "Blood Jelly",    "Blood Crawler",   "Bone Serpent",   "Bunny",           "Chaos Elemental", "Mimic",            "Clown",          "Corrupt Bunny",
                "Corrupt Goldfish", "Crab",         "Crimera",        "Crimson Axe",     "Cursed Hammer",  "Demon",           "Demon Eye",       "Derpling",         "Eater of Souls", "Enchanted Sword",
                "Frozen Zombie",    "Face Monster", "Floaty Gross",   "Flying Fish",     "Flying Snake",   "Frankenstein",    "Fungi Bulb",      "Fungo Fish",       "Gastropod",      "Goblin Thief",
                "Goblin Sorcerer",  "Goblin Peon",  "Goblin Archer",  "Goblin Warrior",  "Goldfish",       "Harpy",           "Hellbat",         "Herpling",         "Hornet",         "Ice Elemental",
                "Icy Merman",       "Fire Imp",     "Blue Jellyfish", "Jungle Creeper",  "Lihzahrd",       "Man Eater",       "Meteor Head",     "Moth",             "Mummy",          "Mushi Ladybug",
                "Parrot Banner",    "Pigron",       "Piranha",        "Pirate Deckhand", "Pixie",          "Raincoat Zombie", "Reaper",          "Shark",            "Skeleton",       "Skeleton Mage",
                "Slime",            "Snow Flinx",   "Spider",         "Spore Zombie",    "Swamp Thing",    "Tortoise",        "Toxic Sludge",    "Umbrella Slime",   "Unicorn",        "Vampire",
                "Vulture",          "Nymph",        "Werewolf",       "Wolf",            "World Feeder",   "Worm",            "Wraith",          "Wyvern",           "Zombie",         "Angry Trapper",
                "Armored Viking", /* End of top row */

                "Black Slime",          "Blue Armored Bones",     "Blue Cultist Archer",   "Lunatic Devotee",        "Blue Cultist Fighter", "Bone Lee",           "Clinger",                "Cochineal Beetle", "Corrupt Penguin",  "Corrupt Slime",
                "Corruptor",            "Crimslime",              "Cursed Skull",          "Cyan Beetle",            "Devourer",             "Diabolist",          "Doctor Bones",           "Dungeon Slime",    "Dungeon Spirit",   "Elf Archer",
                "Elf Copter",           "Eyezor",                 "Flocko",                "Ghost",                  "Giant Bat",            "Giant Cursed Skull", "Giant Flying Fox",       "Gingerbread Man",  "Goblin Archer",    "Green Slime",
                "Headless Horseman",    "Hell Armored Bones",     "Hellhound",             "Hoppin' Jack",           "Ice Bat",              "Ice Golem",          "Ice Slime",              "Ichor",            "Illuminant Bat",   "Illuminant Slime",
                "Jungle Bat",           "Jungle Slime",           "Krampus",               "Lac Beetle",             "Lava Bat",             "Lava Slime",         "Martian Brainscrambler", "Martian Drone",    "Martian Engineer", "Martian Gigazapper",
                "Martian Gray Grunt",   "Martian Officer",        "Martian Ray Gunner",    "Martian Scutlix Gunner", "Martian Tesla Turret", "Mister Stabby",      "Mother Slime",           "Necromancer",      "Nutcracker",       "Paladin",
                "Penguin",              "Pinky",                  "Poltergeist",           "Possessed Armor",        "Present Mimic",        "Purple Slime",       "Ragged Caster",          "Rainbow",          "Raven",            "Red Slime",
                "Rune Wizard",          "Rusty Armored Bones",    "Scarecrow",             "Scutlix",                "Skeleton Archer",      "Skeleton Commando",  "Skeleton Sniper",        "Slimer",           "Snatcher",         "Snow Balla",
                "Snowman Gangsta",      "Spiked Ice Slime",       "Spiked Jungle Slime",   "Splinterling",           "Squid",                "Tactical Skeleton",  "The Groom",              "Tim",              "Undead Miner",     "Undead Viking",
                "White Cultist Archer", "White Cultist Caster",   "White Cultist Fighter", "Yellow Slime",           "Yeti",                 "Zombie Elf",         "",                       "Salamander",       "Giant Shelly",     "Crawdad",
                "Fritz",                "Creature from the Deep", "Dr. Man Fly",           "Mothron",                "Severed Hand",         "The Possessed",      "Butcher",                "Psycho",           "Deadly Sphere",    "Nailhead",
                "Poisonous Spore"

            });
            SubTileNames.Add(TileID.Lamps,                 new List<string> {
                "Tiki Torch", "Cactus", "Ebonwood", "Flesh", "Glass", "Frozen", "Rich Mahogany", "Pearlwood", "Lihzahrd", "Skyware", "Spooky", "Honey", "Steampunk", "Living Wood", "Shadewood", "Golden",
                "Bone", "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Pumpkin", "Obsidian", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.CookingPots,           new List<string> {
                "Cooking Pot", "Cauldron" });
            SubTileNames.Add(TileID.Candelabras,           new List<string> {
                "Gold", "Cactus", "Ebonwood", "Flesh", "Honey", "Steampunk", "Glass", "Rich Mahogany", "Pearlwood", "Frozen", "Lihzahrd", "Skyware", "Spooky", "Living Wood", "Shadewood", "Golden", "Bone",
                "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian", "Pumpkin", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Bookcases,             new List<string> { 
                "Wooden", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian", "Gothic", "Cactus", "Ebonwood", "Flesh", "Honey", "Steampunk", "Glass", "Rich Mahogany", "Pearlwood", "Spooky", "Skyware",
                "Lihzahrd", "Frozen", "Living Wood", "Shadewood", "Golden", "Bone", "Dynasty", "Palm", "Mushroom", "Boreal", "Slime", "Pumpkin", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Bowls,                 new List<string> { 
                "Bowl", "Dynasty Bowl", "Fancy Dishes", "Glass Bowl" });
            SubTileNames.Add(TileID.GrandfatherClocks,     new List<string> {
                "Wooden", "Dynasty", "Golden", "Glass", "Honey", "Steampunk", "Boreal", "Slime", "Bone", "Cactus", "Ebonwood", "Frozen", "Lihzahrd", "Living Wood", "Rich Mahogany", "Flesh", "Mushroom",
                "Obsidian", "Palm", "Bone", "Pumpkin", "Shadewood", "Spooky", "Skyware", "Martian", "Meteorite", "Granite", "Marble", "Crystal" });
            SubTileNames.Add(TileID.Statues,               new List<string> { 
                "Armor", "Angel", "Star", "Sword", "Slime", "Goblin", "Shield", "Bat", "Fish", "Bunny", "Skeleton", "Reaper", "Woman", "Imp", "Gargoyle", "Gloom", "Hornet", "Bomb", "Crab", "Hammer", "Potion",
                "Spear", "Cross", "Jellyfish", "Bow", "Boomerang", "Boot", "Chest", "Bird", "Axe", "Corrupt", "Tree", "Anvil", "Pickaxe", "Mushroom", "Eyeball", "Pillar", "Heart", "Pot", "Sunflower", "King", 
                "Queen", "Piranha", "Lihzahrd", "Lihzahrd Watcher", "Lihzahrd Guardian", "Blue Dungeon Pot", "Green Dungeon Pot", "Pink Dungeon Pot", "Obsidian Pot", "Shark", "Squirrel", "Butterfly", "Worm",
                "Firefly", "Scorpion", "Snail", "Grasshopper", "Mouse", "Duck", "Penguin", "Frog", "Buggy", "Wall Creeper", "Unicorn", "Drippler", "Wraith", "Bone Skeleton", "Undead Viking", "Medusa", "Harpy",
                "Pigron", "Hoplite", "Granite Golem", "Armed Zombie", "Blood Zombie", "Owl", "Seagull"/*, "Dragonfly", "Turtle"*/ });
            SubTileNames.Add(TileID.HallowedPlants,        new List<string> { 
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Flower 1", "Blade 5", "Flower 2", "Flower 3", "Mushroom", "Blade 6", "Plant 1", "Plant 2", "Plant 3", "Plant 4", "Plant 5", "Plant 6", "Plant 7",
                "Plant 8", "Plant 9", "Plant 10", "Plant 10", "Plant 12", "Plant 13" });
            SubTileNames.Add(TileID.HallowedPlants2,       new List<string> { 
                "Blade 1", "Blade 2", "Flower 1", "Flower 2", "Flower 3", "Blade 3", "Flower 4", "Flower 5" });
            SubTileNames.Add(TileID.AdamantiteForge,       new List<string> {
                "Adamantite", "Titanium" });
            SubTileNames.Add(TileID.MythrilAnvil,          new List<string> { 
                "Mythril", "Orichalcum" });
            SubTileNames.Add(TileID.PressurePlates,        new List<string> {
                "Red - All", "Green - All", "Grey - Player", "Brown - Player", "Blue - Player", "Yellow - NPC/Enemies", "Lihzahrd - Player", "Orange - Player, Breaks" });
            SubTileNames.Add(TileID.Traps,                 new List<string> {
                "Dart", "Super Dart", "Flame", "Spiky Ball", "Spear" });
            SubTileNames.Add(TileID.MusicBoxes,            new List<string> {
                "Overworld Day", "Eerie", "Night", "Title", "Underground", "Boss 1", "Jungle", "Corruption", "Underground Corruption", "The Hallow", "Boss 2", "Underground Hallow", "Boss 3", "Snow",
                "Space", "Crimson", "Boss 4", "Alt Overworld Day", "Rain", "Ice", "Desert", "Ocean", "Dungeon", "Plantera", "Boss 5", "Temple", "Eclipse", "Mushrooms", "Pumpkin Moon", "Alt Underground", 
                "Frost Moon", "Underground Crimson", "Lunar Boss", "Martian Madness", "Pirate Invasion", "Hell", "The Towers", "Goblin Invasion", "Sandstorm", "Old One's Army" });
            SubTileNames.Add(TileID.Timers,                new List<string> { 
                "1 Sec", "3 Sec", "5 Sec"/*, "1/2 Second", "1/4 Second"*/ });
            SubTileNames.Add(TileID.HolidayLights,         new List<string> { 
                "Blue", "Red", "Green" });
            SubTileNames.Add(TileID.Stalactite,            new List<string> {
                "Tall", "Short"/*"Frozen", "Rocky", "Spider", "(Blank)", "Hallow", "Corruption", "Flesh", "Hardened Sand", "Granite", "Marble", "Frozen - Short", "Rocky - Short", "(Blank)", "Honey - Short", "Hallow - Short",
                "Corruption - Short", "Flesh - Short", "Hardened Sand - Short", "Granite - Short", "Marble - Short"*/ });
            SubTileNames.Add(TileID.Sinks,                 new List<string> { 
                "Wooden", "Ebonwood", "Rich Mahogany", "Pearlwood", "Bone", "Flesh", "Living Wood", "Skyware", "Shadewood", "Lihzahrd", "Blue Dungeon", "Green Dungeon", "Pink Dungeon", "Obsidian", "Metal",
                "Glass", "Golden", "Honey", "Steampunk", "Pumpkin", "Spooky", "Frozen", "Dynasty", "Palm Wood", "Mushroom", "Boreal Wood", "Slime", "Cactus", "Martian", "Meteorite", "Granite", "Marble", 
                "Crystal"/*, "Spider", "Lesion", "Solar", "Vortex", "Nebula", "Stardust", "Sandstone", "Bamboo"*/ });
            SubTileNames.Add(TileID.ExposedGems,           new List<string> { 
                "Amethyst", "Topaz", "Sapphire", "Emerald", "Ruby", "Diamond", "Amber" });
            SubTileNames.Add(TileID.SmallPiles,            new List<string> {
                // ====================== Small Piles
                "Small Stone 1", "Small Stone 2", "Small Stone 3", "Small Stone 4", "Small Stone 5", "Small Stone 6", // 0 - 5
                "Small Dirt 1", "Small Dirt 2", "Small Dirt 3", "Small Dirt 4", "Small Dirt 5", "Small Dirt 6", // 6 - 11
                "Small Bone 1", "Small Bone 2", "Small Bone 3", "Small Bone 4", "Small Bone 5", "Small Bone 6", "Small Bone 7", "Small Bone 8", // 12 - 19
                "Small Bloody Bone 1", "Small Bloody Bone 2", "Small Bloody Bone 3", "Small Bloody Bone 4", "Small Bloody Bone 5", "Small Bloody Bone 6", "Small Bloody Bone 7", "Small Bloddy Bone 8", // 20 - 27
                "Pickaxe Head", "Axe Head", "Sword Hilt", "Hammer Head", "Mining Helmet", "Arrow 1", "Arrow 2", "Bow", // 28 - 35
                "Small Snow 1", "Small Snow 2", "Small Snow 3", "Small Snow 4", "Small Snow 5", "Small Snow 6", // 36 - 41
                "Small Ice 1", "Small Ice 2", "Small Ice 3", "Small Ice 4", "Small Ice 5", "Small Ice 6", // 42 - 47
                "Small Spider 1", "Small Spider 2", "Small Spider 3", "Small Spider 4", "Small Spider 5", "Small Spider 6", // 48 - 53
                "Sandstone 1", "Sandstone 2", "Sandstone 3", "Sandstone 4", "Sandstone 5", "Sandstone 6", // 54 - 59
                "Granite 1", "Granite 2", "Granite 3", "Granite 4", "Granite 5", "Granite 6", // 60 - 65
                "Marble 1", "Marble 2", "Marble 3", "Marble 4", "Marble 5", "Marble 6", // 66 - 71
                // ====================== Big Piles
                "Rock 1", "Rock 2", "Rock 3", "Rock 4", "Rock 5", "Rock 6", // 72 - 77        // 0 - 5
                "Bone 1", "Bone 2", "Bone 3", "Bone 4", "Bone 5", // 78 - 82           // 6 - 10
                "Bloody Bone 1", "Bloody Bone 2", "Bloody Bone 3", "Bloody Bone 4", "Bloody Bone 5", // 83 - 87      // 11 - 15
                "Bag of Copper", "Bog of Silver", "Bag of Gold", // 88 - 90       // 16 - 18
                "Rock w/ Amethyst", "Rock w/ Topaz", "Rock w/ Sapphire", "Rock w/ Emerald", "Rock w/ Ruby", "Rock w/ Diamond", // 91 - 96     // 19 - 24
                "Snow 1", "Snow 2", "Snow 3", "Snow 4", "Snow 5", "Snow 6", // 97 - 102            // 25 - 30
                "Webbed Chair", "Webbed Workbench", "Webbed Toilet", "Cobweb 1", "Cobweb 2", "Cobweb 3", "Cobweb 4", // 103 - 109     // 31 - 37
                "Rock w/ Moss 1", "Rock w/ Moss 2", "Rock w/ Moss 3", // 110 - 112        // 38 - 40
                "Hardened Sand 1", "Hardened Sand 2", "Hardened Sand 3", "Hardened Sand 4", "Hardened Sand 5", "Hardened Sand 6", // 113 - 118    // 41 - 46
                "Granite 1", "Granite 2", "Granite 3", "Granite 4", "Granite 5", "Granite 6", // 119 - 124       // 47 - 52
                // ===================== Big Piles Row 2
                "Marble 1", "Marble 2", "Marble 3", "Marble 4", "Marble 5", "Marble 6" }); // 125 - 130     // 0 - 5
            SubTileNames.Add(TileID.LargePiles,            new List<string> { 
                "Bone Pile 1", "Bone Pile 2", "Bone Pile 3", "Bone Pile 4", "Bone Pile 5", "Bone Pile 6", "Skeleton Impaled by Sword", "Rock Pile 1", "Rock Pile 2", "Rock Pile 3", "Rock Pile 4", "Rock Pile 5",
                "Rock Pile 6", "Rock Pile w/ Mining Helmet", "Rock Pile w/ Pickaxe", "Rock Pile w/ Sword", "Rock w/ Copper Coin 1", "Rock w/ Copper Coin 2", "Rock w/ Silver Coin 1", "Rock w/ Silver Coin 2",
                "Rock w/ Gold Coin 1", "Rock w/ Gold Coin 2", "Webbed Bed", "Webbed Grandfather Clock", "Webbed Chest", "Webbed Chandelier", "Ice Chunk 1", "Ice Chunk 2", "Ice Chunk 3", "Ice Chunk 4", 
                "Ice Chunk 5", "Ice Chunk 6", "Stone w/ Mushroom Grass 1", "Stone w/ Mushroom Grass 2", "Stone w/ Mushroom Grass 3" });
            SubTileNames.Add(TileID.LargePiles2,           new List<string> { 
                "Stone w/ Jungle Grass 1", "Stone w/ Jungle Grass 2", "Stone w/ Jungle Grass 3", "Mud w/ Jungle Grass 1", "Mud w/ Jungle Grass 2", "Mud w/ Jungle Grass 3", "Ash w/ Hellstone 1",
                "Ash w/ Hellstone 2", "Ash w/ Hellstone 3", "Cobweb Glob 1", "Cobweb Glob 2", "Cobweb Glob 3", "Cobweb Glob 4", "Webbed Body", "Stone w/ Grass 1", "Stone w/ Grass 2", "Stone w/ Grass 3",
                "Stone w/ Enchanted Sword", "Broken Lihzahrd Brick", "Broken Head of a Golem", "Skull on Lihzahrd Brick", "Empty Mossy Cage", "Mossy Cage w/ Chicken", "Rusty Mine Cart", "Old Well",
                "Dirt Pile w/ Shovel", "Tent", "Wheelbarrow w/ Dirt", "Post w/ Rope" });
            SubTileNames.Add(TileID.FleshWeeds,            new List<string> {
                "Blade 1", "Blade 2", "Blade 3", "Blade 4", "Blade 5", "Blade 6", "Blade 7", "Blade 8", "Blade 9", "Blade 10", "Blade 11", "Blade 12", "Blade 13", "Blade 14", "Blade 15", "Vicious Mushroom", 
                "Plant 1", "Plant 2", "Plant 3", "Plant 4", "Plant 5", "Plant 6", "Plant 7" });
            SubTileNames.Add(TileID.WaterFountain,         new List<string> { 
                "Pure", "Desert", "Jungle", "Icy", "Corrupt", "Crimson", "Hallowed", "Blood", "Cavern", "Oasis" });
            SubTileNames.Add(TileID.Cannon,                new List<string> {
                "Cannon", "Bunny", "Confetti", "Portal Gun Station - Blue", "Portal Gun Station - Orange" });
            SubTileNames.Add(TileID.Campfire,              new List<string> { 
                "Campfire", "Cursed", "Demon", "Frozen", "Ichor", "Rainbow", "Ultra Bright", "Bone"/*, "Desert", "Coral", "Corrupt", "Crimson", "Hallowed", "Jungle"*/ });
            SubTileNames.Add(TileID.Firework,              new List<string> { 
                "Red", "Green", "Blue", "Yellow" });
            SubTileNames.Add(TileID.DyePlants,             new List<string> { 
                "Teal Mushroom", "Green Mushroom", "Sky Blue Flower", "Yellow Marigold", "Blue Berries", "Lime Kelp", "Pink Prickly Pear", "Orange Bloodroot", "Strange Plant - Purple", "Strange Plant - Orange",
                "Strange Plant - Green", "Strange Plant - Red" });
            SubTileNames.Add(TileID.PlantDetritus,         new List<string> {
                "Large Lime 1", "Large Lime 2", "Large Lime 3", "Large Green 1", "Large Green 2", "Large Green 3", "Large Blue", "Large Yellow", "Large Red", "Small Lime 1", "Small Lime 2", "Small Lime 3", 
                "Small Green 1", "Small Green 2", "Small Green 3", "Small Yellow 1", "Small Yellow 2", "Small Yellow 3", "Small Blue", "Small Yellow 4", "Small Red" });
            SubTileNames.Add(TileID.LifeFruit,             new List<string> { 
                "Style 1", "Style 2", "Style 3" });
            SubTileNames.Add(TileID.MetalBars,             new List<string> { 
                "Copper", "Tin", "Iron", "Lead", "Silver", "Tungsten", "Gold", "Platinum", "Demonite", "Meteorite", "Hellstone", "Cobalt", "Palladium", "Mythril", "Orichalcum", "Adamantite", "Titanium",
                "Chlorophyte", "Hallowed", "Crimtane", "Shroomite", "Spectre", "Luminite" });
            SubTileNames.Add(TileID.Painting3X3,           new List<string> { 
                "Eye of Cthulhu Trophy", "Eater of Worlds Trophy", "Brain of Cthulhu Trophy", "Skeletron Trophy", "Queen Bee Trophy", "Wall of Flesh Trophy", "Destroyer Trophy", "Skeletron Prime Trophy", 
                "Retinazer Trophy", "Spazmatism Trophy", "Plantera Trophy", "Golem Trophy", "Blood Moon Rising", "The Hanged Man", "Glory of the Fire", "Bone Warp", "Hanging Skeleton", "Hanging Skeleton Inverted",
                "Skellington J Skellingsworth", "The Cursed Man", "Sunflowers", "Terrarian Gothic", "Guide Picasso", "The Guardian's Gaze", "Father of Someone", "Nurse Lisa", "Discover", "Hand Earth", "Old Miner",
                "Skelehead", "Imp Face", "Ominous Presence", "Shining Moon", "The Merchant", "Crowno Devours His Lunch", "Rare Enchantment", "Mourning Wood Trophy", "Pumpking Trophy", "Ice Queen Trophy",
                "Santa-NK1 Trophy", "Everscream Trophy", "Blacksmith Rack", "Carpentry Rack", "Helmet Rack", "Spear Rack", "Sword Rack", "Life Preserver", "Ship's Wheel", "Compass Rose", "Wall Anchor", 
                "Goldfish Trophy", "Bunnyfish Trophy", "Swordfish Trophy", "Sharkteeth Trophy", "King Slime Trophy", "Duke Fishron Trophy", "Ancient Cultist", "Martian Saucer", "Flying Dutchman", "Moon Lord",
                "Dark Mage", "Betsy", "Ogre"/*, "Andrew Sphinx", "Watchful Antlion", "Burning Spirit", "Jaws of Death", "The Sands of Slime", "Snakes, I Hate Shankes", "Fore!", "Nevermore", "Reborn",
                "Empress of Light Trophy", "Queen Slime Trophy" */});
            SubTileNames.Add(TileID.Painting4X3,           new List<string> {
                "Style 1", "Style 2", "Style 3", "Style 4", "Style 5", "Style 6", "Style 7", "Style 8", "Style 9" });
            SubTileNames.Add(TileID.Painting6X4,           new List<string> { 
                "The Eye Sees the End", "Something Evil is Watching You", "The Twins Have Awoken", "The Screamer", "Goblins Playing Poker", "Dryadisque", "Impact", "Powered by Birds", "The Destroyer (item)",
                "The Persistency of Eyes", "Unicorn Crossing the Hallows", "Great Wave", "Starry Night", "Facing the Cerebral Mastermind", "Lake of Fire", "Trio Super Heroes", "The Creation of the Guide", 
                "Jacking Skeletron", "Bitter Harvest", "Blood Moon Countess", "Hallow's Eve", "Morbid Curiosity", "Tiger Skin", "Leopard Skin", "Zebra Skin", "Treasure Map", "Pillagin Me Pixels" }); // TODO: Add 1.4 Paintings
            SubTileNames.Add(TileID.Painting2X3,           new List<string> { 
                "Waldo", "Darkness", "Dark Soul Reaper", "Land", "Trapped Ghost", "American Explosive", "Glorious Night"/*, "Bandage Boy", "Divine Eye", "Study of a Ball at Rest", "Ghost Manifestation", 
                "Wicked Undead", "Bloody Goblet"*/ });
            SubTileNames.Add(TileID.Painting3X2,           new List<string> { 
                "Demon's Eye", "Finding Gold", "First Encounter", "Good Morning", "Underground Reward", "Through the Window", "Place Above the Clouds", "Do Not Step on the Grass", "Cold Waters in the White Land",
                "Lightless Chasms", "The Land of Deceiving Looks", "Daylight", "Secret of the Sands", "Deadland Comes Alive", "Evil Presence", "Sky Guardian", "Living Gore", "Flowing Magma", "Wreath"/*, 
                "The Duplicity of Reflections", "Still Life"*/ });
            SubTileNames.Add(TileID.Pumpkins,              new List<string> {
                "Stage 1", "Stage 2", "Stage 3", "Stage 4", "Stage 5" });
            SubTileNames.Add(TileID.MinecartTrack,         new List<string> {
                "Minecart Track", "Pressure Plate Track", "Booster Track" });
            SubTileNames.Add(TileID.BeachPiles,            new List<string> {
                "Orange Seashell", "Tan Seashell", "Brown Pile", "Red Starfish", "Pink Starfish", "Orange Starfish"/*, "Conch", "Orange/Red Pile", "Grey/Brown Pile"*/ });
            SubTileNames.Add(TileID.AlphabetStatues,       new List<string> { 
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" });
            SubTileNames.Add(TileID.FishingCrate,          new List<string> { 
                "Wooden", "Iron", "Golden", "Corrupt", "Crimson", "Dungeon", "Sky", "Hallowed", "Jungle"/*, "Pearlwood", "Mythril", "Titanium", "Defiled", "Hematic", "Stockade", "Azure", "Divine", "Bramble",
                "Frozen", "Boreal", "Oasis", "Mirage", "Obsidian", "Hellstone", "Ocean", "Seaside"*/ });
            SubTileNames.Add(TileID.PlanterBox,            new List<string> { 
                "Daybloom", "Moonglow", "Deathweed", "Fleshweed", "Blinkroot", "Waterleaf", "Shiverthorn", "Fireblossom" });
            SubTileNames.Add(TileID.LunarMonolith,         new List<string> { 
                "Vortex", "Nebula", "Stardust", "Solar" });
            SubTileNames.Add(TileID.LogicGateLamp,         new List<string> { 
                "Off", "On", "Faulty" });
            SubTileNames.Add(TileID.LogicGate,             new List<string> { 
                "AND", "OR", "NAND", "NOR", "XOR", "XNOR" });
            SubTileNames.Add(TileID.LogicSensor,           new List<string> {
                "Day", "Night", "Player Above", "Water", "Lava", "Honey", "Any" });
            SubTileNames.Add(TileID.WirePipe,              new List<string> {
                "Left->Right", "Left->Up", "Left->Down" });
            SubTileNames.Add(TileID.WeightedPressurePlate, new List<string> { 
                "Orange", "Cyan", "Purple", "Pink" });
            SubTileNames.Add(TileID.GemLocks,              new List<string> {
                "Ruby", "Sapphire", "Emerald", "Topaz", "Amethyst", "Diamond", "Amber", "Ruby w/ Gem", "Sapphire", "Emerald w/ Gem", "Topaz w/ Gem", "Amethyst w/ Gem", "Diamond w/ Gem", "Amber w/ Gem" });
            SubTileNames.Add(TileID.FakeContainers,        new List<string> {
                "Wooden", "Gold", "Gold - Locked", "Shadow", "Shadow - Locked", "Barrel", "Trash Can", "Ebonwood", "Rich Mahogany", "Pearlwood", "Ivy", "Ice", "Living Wood", "Skyware", "Shadewood", 
                "Web Covered", "Lihzahrd", "Water", "Biome - Jungle", "Biome - Corruption", "Biome - Crimson", "Biome - Hallowed", "Biome - Frozen", "Biome - Jungle - Locked", "Biome - Corruption - Locked",
                "Biome - Crimson - Locked", "Biome - Hallowed - Locked", "Biome - Frozen - Locked", "Dynasty", "Honey", "Steampunk", "Palm Wood", "Mushroom", "Boreal", "Slime", "Green Dungeon",
                "Green Dungeon - Locked", "Pink Dungeon", "Pink Dungeon - Locked", "Blue Dungeon", "Blue Dungeon - Locked", "Bone", "Cactus", "Flesh", "Obsidian", "Pumpkin", "Spooky", "Glass", "Martian",
                "Meteorite", "Granite", "Marble", "Crystal", "Golden" });
            SubTileNames.Add(TileID.PixelBox,              new List<string> {
                "Off", "On" });
            SubTileNames.Add(TileID.SillyBalloonTile,      new List<string> {
                "Purple", "Green", "Pink" });
            SubTileNames.Add(TileID.PartyPresent,          new List<string> {
                "Dark Purple", "Pink", "Teal", "Purple", "Light Purple" });
            SubTileNames.Add(TileID.Containers2,           new List<string> {
                "Crystal", "Golden"/*, "Spider", "Lesion", "Dead Man's", "Solar", "Vortex", "Nebula", "Stardust", "Golf", "Sandstone", "Bamboo", "Desert", "Desert - Locked"*/ });
            SubTileNames.Add(TileID.FakeContainers2,       new List<string> { 
                "Crystal", "Golden"/*, "Spider", "Lesion", "Dead Man's", "Solar", "Vortex", "Nebula", "Stardust", "Golf", "Sandstone", "Bamboo", "Desert", "Desert - Locked"*/ });
            SubTileNames.Add(TileID.Tables2,               new List<string> {
                "Crystal", /*"Spider", "Lesion", "Solar", "Vortex", "Nebula", "Stardust", "Sandstone", "Bamboo"*/ });

        }

    }

}
