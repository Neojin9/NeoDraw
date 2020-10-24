using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated 8/9/2020

		public static bool PlaceTree(int x, int y, ref UndoStep undo, int style = -1) {

            if (!WorldGen.InWorld(x, y))
                return false;

            const int MAX_HEIGHT = 17;
            const int MIN_HEIGHT = 5;

            UnifiedRandom genRand = WorldGen.genRand;

			while (
                y < Main.maxTilesY &&
                (
                    !Main.tile[x, y].active() ||
                    (
                        Main.tile[x, y].active() &&
                        Main.tileCut[Main.tile[x, y].type]
                    )
                )
            )
				y++;

			if (
                Main.tile[x, y].nactive()    && 
				!Main.tile[x, y].halfBrick() &&
				Main.tile[x, y].slope() == 0 && 
				(
					Main.tile[x, y].type == TileID.Grass         || 
					Main.tile[x, y].type == TileID.CorruptGrass  || 
					Main.tile[x, y].type == TileID.JungleGrass   ||
                    Main.tile[x, y].type == TileID.MushroomGrass ||
                    Main.tile[x, y].type == TileID.HallowedGrass || 
					Main.tile[x, y].type == TileID.SnowBlock     || 
					Main.tile[x, y].type == TileID.FleshGrass    || 
					TileLoader.CanGrowModTree(Main.tile[x, y].type)
				) &&
				(
					(
						Main.tile[x - 1, y].active() && 
						(
							Main.tile[x - 1, y].type == TileID.Grass         ||  
							Main.tile[x - 1, y].type == TileID.CorruptGrass  ||  
							Main.tile[x - 1, y].type == TileID.JungleGrass   || 
							Main.tile[x - 1, y].type == TileID.MushroomGrass || 
							Main.tile[x - 1, y].type == TileID.HallowedGrass || 
							Main.tile[x - 1, y].type == TileID.SnowBlock     ||  
							Main.tile[x - 1, y].type == TileID.FleshGrass    ||
                            TileLoader.CanGrowModTree(Main.tile[x - 1, y].type)
						)
					) ||
					(
						Main.tile[x + 1, y].active() && 
						(
							Main.tile[x + 1, y].type == TileID.Grass         ||  
							Main.tile[x + 1, y].type == TileID.CorruptGrass  ||  
							Main.tile[x + 1, y].type == TileID.JungleGrass   || 
							Main.tile[x + 1, y].type == TileID.MushroomGrass || 
							Main.tile[x + 1, y].type == TileID.HallowedGrass || 
							Main.tile[x + 1, y].type == TileID.SnowBlock     ||  
							Main.tile[x + 1, y].type == TileID.FleshGrass    ||
                            TileLoader.CanGrowModTree(Main.tile[x + 1, y].type)
						)
					)
				)
			) {

                byte color = Main.tile[x, y].color();
                int halfWidth = 2;
                int height = genRand.Next(MIN_HEIGHT, MAX_HEIGHT);
                int heightWithTop = height + 4;

                if (Main.tile[x, y].type == TileID.JungleGrass)
                    heightWithTop += 5;

                if (Main.tile[x, y].type == TileID.MushroomGrass && (!EmptyTileCheck(x - halfWidth, x + halfWidth, y - (heightWithTop + 1), y - 3, 20, true) || !EmptyTileCheck(x - 1, x + 1, y - 2, y - 1, 20, true)))
                    return false;

                if (!EmptyTileCheck(x - halfWidth, x + halfWidth, y - (heightWithTop + 1), y - 1, 20, true))
                    return false;

                bool flag  = false;
                bool flag2 = false;
                int num4;

                for (int k = y - height; k < y; k++) {

                    undo.Add(new ChangedTile(x, k));

                    Main.tile[x, k].frameNumber((byte)genRand.Next(3));
                    Main.tile[x, k].active(active: true);
                    Main.tile[x, k].type = TileID.Trees;
                    Main.tile[x, k].color(color);

                    num4 = genRand.Next(3);

                    int num5 = genRand.Next(10);

                    if (k == y - 1 || k == y - height)
                        num5 = 0;

                    while (((num5 == 5 || num5 == 7) && flag) || ((num5 == 6 || num5 == 7) && flag2))
                        num5 = genRand.Next(10);

                    flag  = false;
                    flag2 = false;

                    if (num5 == 5 || num5 == 7)
                        flag = true;

                    if (num5 == 6 || num5 == 7)
                        flag2 = true;

                    switch (num5) {

                        case 1: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 0;
                                            Main.tile[x, k].frameY = 66;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 0;
                                            Main.tile[x, k].frameY = 88;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 0;
                                            Main.tile[x, k].frameY = 110;
                                            break;
                                        }

                                }

                                break;

                            }
                        case 2: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 22;
                                            Main.tile[x, k].frameY = 0;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 22;
                                            Main.tile[x, k].frameY = 22;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 22;
                                            Main.tile[x, k].frameY = 44;
                                            break;
                                        }

                                }

                                break;

                            }
                        case 3: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 44;
                                            Main.tile[x, k].frameY = 66;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 44;
                                            Main.tile[x, k].frameY = 88;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 44;
                                            Main.tile[x, k].frameY = 110;
                                            break;
                                        }

                                }

                                break;

                            }
                        case 4: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 22;
                                            Main.tile[x, k].frameY = 66;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 22;
                                            Main.tile[x, k].frameY = 88;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 22;
                                            Main.tile[x, k].frameY = 110;
                                            break;
                                        }

                                }

                                break;

                            }
                        case 5: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 88;
                                            Main.tile[x, k].frameY = 0;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 88;
                                            Main.tile[x, k].frameY = 22;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 88;
                                            Main.tile[x, k].frameY = 44;
                                            break;
                                        }

                                }

                                break;

                            }
                        case 6: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 66;
                                            Main.tile[x, k].frameY = 66;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 66;
                                            Main.tile[x, k].frameY = 88;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 66;
                                            Main.tile[x, k].frameY = 110;
                                            break;
                                        }

                                }

                                break;

                            }
                        case 7: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 110;
                                            Main.tile[x, k].frameY = 66;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 110;
                                            Main.tile[x, k].frameY = 88;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 110;
                                            Main.tile[x, k].frameY = 110;
                                            break;
                                        }

                                }

                                break;

                            }
                        default: {

                                switch (num4) {

                                    case 0: {
                                            Main.tile[x, k].frameX = 0;
                                            Main.tile[x, k].frameY = 0;
                                            break;
                                        }
                                    case 1: {
                                            Main.tile[x, k].frameX = 0;
                                            Main.tile[x, k].frameY = 22;
                                            break;
                                        }
                                    case 2: {
                                            Main.tile[x, k].frameX = 0;
                                            Main.tile[x, k].frameY = 44;
                                            break;
                                        }

                                }

                                break;

                            }

                    }

                    if (num5 == 5 || num5 == 7) {

                        undo.Add(new ChangedTile(x - 1, k));

                        Main.tile[x - 1, k].active(active: true);
                        Main.tile[x - 1, k].type = TileID.Trees;
                        Main.tile[x - 1, k].color(color);

                        num4 = genRand.Next(3);

                        if (genRand.Next(3) < 2) {

                            switch (num4) {

                                case 0: {
                                        Main.tile[x - 1, k].frameX = 44;
                                        Main.tile[x - 1, k].frameY = 198;
                                        break;
                                    }
                                case 1: {
                                        Main.tile[x - 1, k].frameX = 44;
                                        Main.tile[x - 1, k].frameY = 220;
                                        break;
                                    }
                                case 2: {
                                        Main.tile[x - 1, k].frameX = 44;
                                        Main.tile[x - 1, k].frameY = 242;
                                        break;
                                    }

                            }

                        }
                        else {

                            switch (num4) {

                                case 0: {
                                        Main.tile[x - 1, k].frameX = 66;
                                        Main.tile[x - 1, k].frameY = 0;
                                        break;
                                    }
                                case 1: {
                                        Main.tile[x - 1, k].frameX = 66;
                                        Main.tile[x - 1, k].frameY = 22;
                                        break;
                                    }
                                case 2: {
                                        Main.tile[x - 1, k].frameX = 66;
                                        Main.tile[x - 1, k].frameY = 44;
                                        break;
                                    }

                            }

                        }

                    }

                    if (num5 != 6 && num5 != 7)
                        continue;

                    undo.Add(new ChangedTile(x + 1, k));

                    Main.tile[x + 1, k].active(active: true);
                    Main.tile[x + 1, k].type = TileID.Trees;
                    Main.tile[x + 1, k].color(color);

                    num4 = genRand.Next(3);

                    if (genRand.Next(3) < 2) {

                        switch (num4) {

                            case 0: {
                                    Main.tile[x + 1, k].frameX = 66;
                                    Main.tile[x + 1, k].frameY = 198;
                                    break;
                                }
                            case 1: {
                                    Main.tile[x + 1, k].frameX = 66;
                                    Main.tile[x + 1, k].frameY = 220;
                                    break;
                                }
                            case 2: {
                                    Main.tile[x + 1, k].frameX = 66;
                                    Main.tile[x + 1, k].frameY = 242;
                                    break;
                                }

                        }

                    }
                    else {

                        switch (num4) {

                            case 0: {
                                    Main.tile[x + 1, k].frameX = 88;
                                    Main.tile[x + 1, k].frameY = 66;
                                    break;
                                }
                            case 1: {
                                    Main.tile[x + 1, k].frameX = 88;
                                    Main.tile[x + 1, k].frameY = 88;
                                    break;
                                }
                            case 2: {
                                    Main.tile[x + 1, k].frameX = 88;
                                    Main.tile[x + 1, k].frameY = 110;
                                    break;
                                }

                        }

                    }

                }

                int num6 = genRand.Next(3);
                bool flag3 = false;
                bool flag4 = false;

                if (Main.tile[x - 1, y].nactive() && !Main.tile[x - 1, y].halfBrick() && Main.tile[x - 1, y].slope() == 0 && (Main.tile[x - 1, y].type == 2 || Main.tile[x - 1, y].type == 23 || Main.tile[x - 1, y].type == 60 || Main.tile[x - 1, y].type == 109 || Main.tile[x - 1, y].type == 147 || Main.tile[x - 1, y].type == 199 || TileLoader.CanGrowModTree(Main.tile[x - 1, y].type))) {
                    flag3 = true;
                }

                if (Main.tile[x + 1, y].nactive() && !Main.tile[x + 1, y].halfBrick() && Main.tile[x + 1, y].slope() == 0 && (Main.tile[x + 1, y].type == 2 || Main.tile[x + 1, y].type == 23 || Main.tile[x + 1, y].type == 60 || Main.tile[x + 1, y].type == 109 || Main.tile[x + 1, y].type == 147 || Main.tile[x + 1, y].type == 199 || TileLoader.CanGrowModTree(Main.tile[x + 1, y].type))) {
                    flag4 = true;
                }

                if (!flag3) {

                    if (num6 == 0)
                        num6 = 2;
                    
                    if (num6 == 1)
                        num6 = 3;
                    
                }

                if (!flag4) {

                    if (num6 == 0)
                        num6 = 1;
                    
                    if (num6 == 2)
                        num6 = 3;
                    
                }

                if (flag3 && !flag4)
                    num6 = 2;

                if (flag4 && !flag3)
                    num6 = 1;

                if (num6 == 0 || num6 == 1) {

                    undo.Add(new ChangedTile(x + 1, y - 1));

                    Main.tile[x + 1, y - 1].active(active: true);
                    Main.tile[x + 1, y - 1].type = TileID.Trees;
                    Main.tile[x + 1, y - 1].color(color);

                    num4 = genRand.Next(3);

                    switch (num4) {

                        case 0: {
                                Main.tile[x + 1, y - 1].frameX = 22;
                                Main.tile[x + 1, y - 1].frameY = 132;
                                break;
                            }
                        case 1: {
                                Main.tile[x + 1, y - 1].frameX = 22;
                                Main.tile[x + 1, y - 1].frameY = 154;
                                break;
                            }
                        case 2: {
                                Main.tile[x + 1, y - 1].frameX = 22;
                                Main.tile[x + 1, y - 1].frameY = 176;
                                break;
                            }

                    }

                }

                if (num6 == 0 || num6 == 2) {

                    undo.Add(new ChangedTile(x - 1, y - 1));

                    Main.tile[x - 1, y - 1].active(active: true);
                    Main.tile[x - 1, y - 1].type = TileID.Trees;
                    Main.tile[x - 1, y - 1].color(color);

                    num4 = genRand.Next(3);

                    switch (num4) {

                        case 0: {
                                Main.tile[x - 1, y - 1].frameX = 44;
                                Main.tile[x - 1, y - 1].frameY = 132;
                                break;
                            }
                        case 1: {
                                Main.tile[x - 1, y - 1].frameX = 44;
                                Main.tile[x - 1, y - 1].frameY = 154;
                                break;
                            }
                        case 2: {
                                Main.tile[x - 1, y - 1].frameX = 44;
                                Main.tile[x - 1, y - 1].frameY = 176;
                                break;
                            }

                    }

                }

                num4 = genRand.Next(3);

                undo.Add(new ChangedTile(x, y - 1));

                switch (num6) {

                    case 0: {

                            switch (num4) {

                                case 0: {
                                        Main.tile[x, y - 1].frameX = 88;
                                        Main.tile[x, y - 1].frameY = 132;
                                        break;
                                    }
                                case 1: {
                                        Main.tile[x, y - 1].frameX = 88;
                                        Main.tile[x, y - 1].frameY = 154;
                                        break;
                                    }
                                case 2: {
                                        Main.tile[x, y - 1].frameX = 88;
                                        Main.tile[x, y - 1].frameY = 176;
                                        break;
                                    }

                            }

                            break;

                        }
                    case 1: {

                            switch (num4) {

                                case 0: {
                                        Main.tile[x, y - 1].frameX = 0;
                                        Main.tile[x, y - 1].frameY = 132;
                                        break;
                                    }
                                case 1: {
                                        Main.tile[x, y - 1].frameX = 0;
                                        Main.tile[x, y - 1].frameY = 154;
                                        break;
                                    }
                                case 2: {
                                        Main.tile[x, y - 1].frameX = 0;
                                        Main.tile[x, y - 1].frameY = 176;
                                        break;
                                    }

                            }

                            break;

                        }
                    case 2: {

                            switch (num4) {

                                case 0: {
                                        Main.tile[x, y - 1].frameX = 66;
                                        Main.tile[x, y - 1].frameY = 132;
                                        break;
                                    }
                                case 1: {
                                        Main.tile[x, y - 1].frameX = 66;
                                        Main.tile[x, y - 1].frameY = 154;
                                        break;
                                    }
                                case 2: {
                                        Main.tile[x, y - 1].frameX = 66;
                                        Main.tile[x, y - 1].frameY = 176;
                                        break;
                                    }

                            }

                            break;

                        }

                }

                undo.Add(new ChangedTile(x, y - height));

                if (genRand.Next(8) != 0) {

                    num4 = genRand.Next(3);

                    switch (num4) {

                        case 0: {
                                Main.tile[x, y - height].frameX = 22;
                                Main.tile[x, y - height].frameY = 198;
                                break;
                            }
                        case 1: {
                                Main.tile[x, y - height].frameX = 22;
                                Main.tile[x, y - height].frameY = 220;
                                break;
                            }
                        case 2: {
                                Main.tile[x, y - height].frameX = 22;
                                Main.tile[x, y - height].frameY = 242;
                                break;
                            }

                    }

                }
                else {

                    num4 = genRand.Next(3);

                    switch (num4) {

                        case 0: {
                                Main.tile[x, y - height].frameX = 0;
                                Main.tile[x, y - height].frameY = 198;
                                break;
                            }
                        case 1: {
                                Main.tile[x, y - height].frameX = 0;
                                Main.tile[x, y - height].frameY = 220;
                                break;
                            }
                        case 2: {
                                Main.tile[x, y - height].frameX = 0;
                                Main.tile[x, y - height].frameY = 242;
                                break;
                            }

                    }

                }

                WorldGen.RangeFrame(x - 2, y - (height + 1), x + 2, y + 1);

                if (Main.netMode == NetmodeID.Server)
                    NetMessage.SendTileSquare(-1, x, (int)(y - height * 0.5), height + 1);

                return true;

            }

            return false;

		}

	}

}
