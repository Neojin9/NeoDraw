using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;
using NeoDraw.WldGen.WldUtils;
using Microsoft.Xna.Framework;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated 8/11/2020 v1.4 Copy/Paste

		public static bool GrowEpicTree(int i, int y, ref UndoStep undo) {

            if (!Find(new Point(i, y), Searches.Chain(new Searches.Down(100), new Conditions.IsSolid()), out Point ground))
                return false;

            y = ground.Y;

            UnifiedRandom genRand = WorldGen.genRand;

            int j;

            for (j = y; Main.tile[i, j].type == 20; j++) { }

            if (!Main.tile[i, j].active() ||
                Main.tile[i, j].halfBrick() ||
                Main.tile[i, j].slope() != 0 ||
                Main.tile[i, j].type != 2 ||
                Main.tile[i, j - 1].wall != 0 ||
                Main.tile[i, j - 1].liquid != 0 ||
                (   !Main.tile[i - 1, j].active() ||
                    Main.tile[i - 1, j].type != 2 &&
                    Main.tile[i - 1, j].type != 23 &&
                    Main.tile[i - 1, j].type != 60 &&
                    Main.tile[i - 1, j].type != 109
                ) &&
                (   !Main.tile[i + 1, j].active() ||
                    Main.tile[i + 1, j].type != 2 &&
                    Main.tile[i + 1, j].type != 23 &&
                    Main.tile[i + 1, j].type != 60 &&
                    Main.tile[i + 1, j].type != 109)
                ) {

                return false;

            }

            int num = 2;

            if (!EmptyTileCheck(i - num, i + num, j - 55, j - 1, 20)) {
                return false;
            }

            bool flag  = false;
            bool flag2 = false;

            int num2 = genRand.Next(20, 30);

            if (DrunkWorldGen)
                num2 = genRand.Next(3, 7);
            
            int num3;

            for (int k = j - num2; k < j; k++) {

                Neo.SetTile(i, k, 5, ref undo);
                Main.tile[i, k].frameNumber((byte)genRand.Next(3));

                num3 = genRand.Next(3);

                int num4 = genRand.Next(10);

                if (k == j - 1 || k == j - num2)
                    num4 = 0;
                
                while (((num4 == 5 || num4 == 7) && flag) || ((num4 == 6 || num4 == 7) && flag2))
                    num4 = genRand.Next(10);
                
                flag  = false;
                flag2 = false;

                if (num4 == 5 || num4 == 7)
                    flag = true;
                
                if (num4 == 6 || num4 == 7)
                    flag2 = true;
                
                switch (num4) {

                    case 1: {

                            if (num3 == 0) {
                                Main.tile[i, k].frameX = 0;
                                Main.tile[i, k].frameY = 66;
                            }

                            if (num3 == 1) {
                                Main.tile[i, k].frameX = 0;
                                Main.tile[i, k].frameY = 88;
                            }

                            if (num3 == 2) {
                                Main.tile[i, k].frameX = 0;
                                Main.tile[i, k].frameY = 110;
                            }

                            break;

                        }
                    case 2: {

                            if (num3 == 0) {
                                Main.tile[i, k].frameX = 22;
                                Main.tile[i, k].frameY = 0;
                            }

                            if (num3 == 1) {
                                Main.tile[i, k].frameX = 22;
                                Main.tile[i, k].frameY = 22;
                            }

                            if (num3 == 2) {
                                Main.tile[i, k].frameX = 22;
                                Main.tile[i, k].frameY = 44;
                            }

                            break;

                        }
                    case 3:
                        if (num3 == 0) {
                            Main.tile[i, k].frameX = 44;
                            Main.tile[i, k].frameY = 66;
                        }
                        if (num3 == 1) {
                            Main.tile[i, k].frameX = 44;
                            Main.tile[i, k].frameY = 88;
                        }
                        if (num3 == 2) {
                            Main.tile[i, k].frameX = 44;
                            Main.tile[i, k].frameY = 110;
                        }
                        break;
                    case 4:
                        if (num3 == 0) {
                            Main.tile[i, k].frameX = 22;
                            Main.tile[i, k].frameY = 66;
                        }
                        if (num3 == 1) {
                            Main.tile[i, k].frameX = 22;
                            Main.tile[i, k].frameY = 88;
                        }
                        if (num3 == 2) {
                            Main.tile[i, k].frameX = 22;
                            Main.tile[i, k].frameY = 110;
                        }
                        break;
                    case 5:
                        if (num3 == 0) {
                            Main.tile[i, k].frameX = 88;
                            Main.tile[i, k].frameY = 0;
                        }
                        if (num3 == 1) {
                            Main.tile[i, k].frameX = 88;
                            Main.tile[i, k].frameY = 22;
                        }
                        if (num3 == 2) {
                            Main.tile[i, k].frameX = 88;
                            Main.tile[i, k].frameY = 44;
                        }
                        break;
                    case 6:
                        if (num3 == 0) {
                            Main.tile[i, k].frameX = 66;
                            Main.tile[i, k].frameY = 66;
                        }
                        if (num3 == 1) {
                            Main.tile[i, k].frameX = 66;
                            Main.tile[i, k].frameY = 88;
                        }
                        if (num3 == 2) {
                            Main.tile[i, k].frameX = 66;
                            Main.tile[i, k].frameY = 110;
                        }
                        break;
                    case 7:
                        if (num3 == 0) {
                            Main.tile[i, k].frameX = 110;
                            Main.tile[i, k].frameY = 66;
                        }
                        if (num3 == 1) {
                            Main.tile[i, k].frameX = 110;
                            Main.tile[i, k].frameY = 88;
                        }
                        if (num3 == 2) {
                            Main.tile[i, k].frameX = 110;
                            Main.tile[i, k].frameY = 110;
                        }
                        break;
                    default:
                        if (num3 == 0) {
                            Main.tile[i, k].frameX = 0;
                            Main.tile[i, k].frameY = 0;
                        }
                        if (num3 == 1) {
                            Main.tile[i, k].frameX = 0;
                            Main.tile[i, k].frameY = 22;
                        }
                        if (num3 == 2) {
                            Main.tile[i, k].frameX = 0;
                            Main.tile[i, k].frameY = 44;
                        }
                        break;

                }

                if (num4 == 5 || num4 == 7) {

                    Neo.SetTile(i - 1, k, 5, ref undo);

                    num3 = genRand.Next(3);

                    if (genRand.Next(3) < 2) {

                        if (num3 == 0) {
                            Main.tile[i - 1, k].frameX = 44;
                            Main.tile[i - 1, k].frameY = 198;
                        }

                        if (num3 == 1) {
                            Main.tile[i - 1, k].frameX = 44;
                            Main.tile[i - 1, k].frameY = 220;
                        }

                        if (num3 == 2) {
                            Main.tile[i - 1, k].frameX = 44;
                            Main.tile[i - 1, k].frameY = 242;
                        }

                    }
                    else {

                        if (num3 == 0) {
                            Main.tile[i - 1, k].frameX = 66;
                            Main.tile[i - 1, k].frameY = 0;
                        }

                        if (num3 == 1) {
                            Main.tile[i - 1, k].frameX = 66;
                            Main.tile[i - 1, k].frameY = 22;
                        }

                        if (num3 == 2) {
                            Main.tile[i - 1, k].frameX = 66;
                            Main.tile[i - 1, k].frameY = 44;
                        }

                    }

                }

                if (num4 != 6 && num4 != 7)
                    continue;

                Neo.SetTile(i + 1, k, 5, ref undo);

                num3 = genRand.Next(3);

                if (genRand.Next(3) < 2) {

                    if (num3 == 0) {
                        Main.tile[i + 1, k].frameX = 66;
                        Main.tile[i + 1, k].frameY = 198;
                    }

                    if (num3 == 1) {
                        Main.tile[i + 1, k].frameX = 66;
                        Main.tile[i + 1, k].frameY = 220;
                    }

                    if (num3 == 2) {
                        Main.tile[i + 1, k].frameX = 66;
                        Main.tile[i + 1, k].frameY = 242;
                    }

                }
                else {

                    if (num3 == 0) {
                        Main.tile[i + 1, k].frameX = 88;
                        Main.tile[i + 1, k].frameY = 66;
                    }

                    if (num3 == 1) {
                        Main.tile[i + 1, k].frameX = 88;
                        Main.tile[i + 1, k].frameY = 88;
                    }

                    if (num3 == 2) {
                        Main.tile[i + 1, k].frameX = 88;
                        Main.tile[i + 1, k].frameY = 110;
                    }

                }

            }

            int num5 = genRand.Next(3);
            
            bool flag3 = false;
            bool flag4 = false;
            
            if (Main.tile[i - 1, j].active() && !Main.tile[i - 1, j].halfBrick() && Main.tile[i - 1, j].slope() == 0 && (Main.tile[i - 1, j].type == 2 || Main.tile[i - 1, j].type == 23 || Main.tile[i - 1, j].type == 60 || Main.tile[i - 1, j].type == 109)) {
                flag3 = true;
            }

            if (Main.tile[i + 1, j].active() && !Main.tile[i + 1, j].halfBrick() && Main.tile[i + 1, j].slope() == 0 && (Main.tile[i + 1, j].type == 2 || Main.tile[i + 1, j].type == 23 || Main.tile[i + 1, j].type == 60 || Main.tile[i + 1, j].type == 109)) {
                flag4 = true;
            }

            if (!flag3) {

                if (num5 == 0)
                    num5 = 2;
                
                if (num5 == 1)
                    num5 = 3;
                
            }

            if (!flag4) {

                if (num5 == 0)
                    num5 = 1;
                
                if (num5 == 2)
                    num5 = 3;
                
            }

            if (flag3 && !flag4)
                num5 = 2;
            
            if (flag4 && !flag3)
                num5 = 1;
            
            if (num5 == 0 || num5 == 1) {

                Neo.SetTile(i + 1, j - 1, 5, ref undo);

                num3 = genRand.Next(3);

                if (num3 == 0) {
                    Main.tile[i + 1, j - 1].frameX = 22;
                    Main.tile[i + 1, j - 1].frameY = 132;
                }

                if (num3 == 1) {
                    Main.tile[i + 1, j - 1].frameX = 22;
                    Main.tile[i + 1, j - 1].frameY = 154;
                }

                if (num3 == 2) {
                    Main.tile[i + 1, j - 1].frameX = 22;
                    Main.tile[i + 1, j - 1].frameY = 176;
                }

            }

            if (num5 == 0 || num5 == 2) {

                Neo.SetTile(i - 1, j - 1, 5, ref undo);

                num3 = genRand.Next(3);

                if (num3 == 0) {
                    Main.tile[i - 1, j - 1].frameX = 44;
                    Main.tile[i - 1, j - 1].frameY = 132;
                }

                if (num3 == 1) {
                    Main.tile[i - 1, j - 1].frameX = 44;
                    Main.tile[i - 1, j - 1].frameY = 154;
                }

                if (num3 == 2) {
                    Main.tile[i - 1, j - 1].frameX = 44;
                    Main.tile[i - 1, j - 1].frameY = 176;
                }

            }

            num3 = genRand.Next(3);

            switch (num5) {
                case 0:
                    if (num3 == 0) {
                        Main.tile[i, j - 1].frameX = 88;
                        Main.tile[i, j - 1].frameY = 132;
                    }
                    if (num3 == 1) {
                        Main.tile[i, j - 1].frameX = 88;
                        Main.tile[i, j - 1].frameY = 154;
                    }
                    if (num3 == 2) {
                        Main.tile[i, j - 1].frameX = 88;
                        Main.tile[i, j - 1].frameY = 176;
                    }
                    break;
                case 1:
                    if (num3 == 0) {
                        Main.tile[i, j - 1].frameX = 0;
                        Main.tile[i, j - 1].frameY = 132;
                    }
                    if (num3 == 1) {
                        Main.tile[i, j - 1].frameX = 0;
                        Main.tile[i, j - 1].frameY = 154;
                    }
                    if (num3 == 2) {
                        Main.tile[i, j - 1].frameX = 0;
                        Main.tile[i, j - 1].frameY = 176;
                    }
                    break;
                case 2:
                    if (num3 == 0) {
                        Main.tile[i, j - 1].frameX = 66;
                        Main.tile[i, j - 1].frameY = 132;
                    }
                    if (num3 == 1) {
                        Main.tile[i, j - 1].frameX = 66;
                        Main.tile[i, j - 1].frameY = 154;
                    }
                    if (num3 == 2) {
                        Main.tile[i, j - 1].frameX = 66;
                        Main.tile[i, j - 1].frameY = 176;
                    }
                    break;
            }
            if (genRand.Next(13) != 0) {
                num3 = genRand.Next(3);
                if (num3 == 0) {
                    Main.tile[i, j - num2].frameX = 22;
                    Main.tile[i, j - num2].frameY = 198;
                }
                if (num3 == 1) {
                    Main.tile[i, j - num2].frameX = 22;
                    Main.tile[i, j - num2].frameY = 220;
                }
                if (num3 == 2) {
                    Main.tile[i, j - num2].frameX = 22;
                    Main.tile[i, j - num2].frameY = 242;
                }
            }
            else {
                num3 = genRand.Next(3);
                if (num3 == 0) {
                    Main.tile[i, j - num2].frameX = 0;
                    Main.tile[i, j - num2].frameY = 198;
                }
                if (num3 == 1) {
                    Main.tile[i, j - num2].frameX = 0;
                    Main.tile[i, j - num2].frameY = 220;
                }
                if (num3 == 2) {
                    Main.tile[i, j - num2].frameX = 0;
                    Main.tile[i, j - num2].frameY = 242;
                }
            }

            WorldGen.RangeFrame(i - 2, j - num2 - 1, i + 2, j + 1);

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, i, (int)(j - num2 * 0.5), num2 + 1);
            
            return true;

        }

    }

}
