using System;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/27/2020 Copy/Paste Modified Heavily

		public static bool PlaceOasis(int x, int y, ref UndoStep undo, int oasisHeight = 20) {

            if (Main.tile[x, y].active())
                return false;

            UnifiedRandom genRand = WorldGen.genRand;

            for (; !Main.tile[x, y].active() && !Main.tileCut[Main.tile[x, y].type] && y <= Main.worldSurface; y++) { }

            ushort tileType = Main.tile[x, y].type;

            if (tileType != TileID.Sand && tileType != TileID.HardenedSand && tileType != TileID.Mud && tileType != TileID.JungleGrass) {
                DrawInterface.SetStatusBarTempMessage("Must be placed in a desert or jungle biome.");
                return false;
            }

            if (tileType == TileID.JungleGrass)
                tileType = TileID.Mud;

            int width  = genRand.Next(45, 61);
            int height = oasisHeight;
            int halfWidth = width / 2;

            int tileLeft  = x - width * 3;
            int tileRight = x + width * 3;
            int tileAbove = y - height * 4;
            int tileBelow = y + height * 3;

            Neo.WorldRestrain(ref tileLeft, ref tileRight, ref tileAbove, ref tileBelow);

            for (int xPos = tileLeft; xPos < tileRight; xPos++) {

                for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

                    float num13 = Math.Abs(xPos - x) * 0.7f;
                    float num14 = Math.Abs(yPos - y) * 1.35f;
                    
                    double num15 = Math.Sqrt(num13 * num13 + num14 * num14);
                    
                    float num16 = halfWidth * (0.53f + genRand.NextFloat() * 0.04f);
                    float num17 = Math.Abs(xPos - x) / (float)(tileRight - x);
                    
                    num17 = 1f - num17;
                    num17 *= 2.3f;
                    num17 *= num17;
                    num17 *= num17;
                    
                    if (num15 < num16) {

                        undo.Add(new ChangedTile(xPos, yPos));

                        if (yPos == y + 1) {
                            Neo.SetLiquid(xPos, yPos, 127, false);
                        }
                        else if (yPos > y + 1) {
                            Neo.SetLiquid(xPos, yPos, byte.MaxValue, false);
                        }

                        Neo.SetActive(xPos, yPos, false);

                    }
                    else if (yPos < y && num13 < num16 + Math.Abs(yPos - y) * 3f * num17) {

                        if (Main.tile[xPos, yPos].type == tileType)
                            Neo.SetActive(xPos, yPos, false, ref undo);

                    }
                    else if (yPos >= y && num13 < num16 + Math.Abs(yPos - y) * num17 && Main.tile[xPos, yPos].wall == 0) {

                        if (Main.tile[xPos, yPos].active() && Main.tileSolid[Main.tile[xPos, yPos].type] && !Main.tileSolidTop[Main.tile[xPos, yPos].type]) {

                            undo.Add(new ChangedTile(xPos, yPos));

                            Main.tile[xPos, yPos].Clear(TileDataType.Slope);

                            continue;

                        }

                        Neo.SetTile(xPos, yPos, tileType, ref undo);

                    }

                }

            }

            int num18 = 50;

            tileLeft  = x - width * 2;
            tileRight = x + width * 2;
            tileBelow = y + height * 2;

            for (int num19 = tileLeft; num19 < tileRight; num19++) {

                for (int num20 = tileBelow; num20 >= y; num20--) {

                    float num21 = Math.Abs(num19 - x) * 0.7f;
                    float num22 = Math.Abs(num20 - y) * 1.35f;
                    double num23 = Math.Sqrt(num21 * num21 + num22 * num22);
                    float num24 = halfWidth * 0.57f;
                    
                    if (num23 > num24) {

                        if (!Main.tile[num19, num20].active() && Main.tile[num19, num20].wall == 0) {

                            int num25 = -1;
                            int num26 = -1;

                            bool flag;

                            for (int num27 = num19; num27 <= num19 + num18 && Main.tile[num27, num20 + 1].active() && Main.tileSolid[Main.tile[num27, num20 + 1].type] && Main.tile[num27, num20].wall <= 0; num27++) {

                                if (Main.tile[num27, num20].active() && Main.tileSolid[Main.tile[num27, num20].type]) {

                                    if (Main.tile[num27, num20].type == tileType)
                                        flag = true;

                                    num26 = num27;

                                    break;

                                }

                                if (Main.tile[num27, num20].active())
                                    break;

                            }

                            int num28 = num19;

                            while (num28 >= num19 - num18 && Main.tile[num28, num20 + 1].active() && Main.tileSolid[Main.tile[num28, num20 + 1].type] && Main.tile[num28, num20].wall <= 0) {

                                if (Main.tile[num28, num20].active() && Main.tileSolid[Main.tile[num28, num20].type]) {

                                    num25 = num28;

                                    break;

                                }

                                if (Main.tile[num28, num20].active())
                                    break;

                                num28--;

                            }

                            flag = true;

                            if (num25 > -1 && num26 > -1 && flag) {

                                int num29 = 0;

                                for (int num30 = num25 + 1; num30 < num26; num30++) {

                                    if (num26 - num25 > 5 && genRand.Next(5) == 0)
                                        num29 = genRand.Next(5, 10);

                                    Neo.SetTile(num30, num20, tileType, ref undo);

                                    if (num29 > 0) {

                                        num29--;

                                        Neo.SetTile(num30, num20 - 1, tileType, ref undo);

                                    }

                                }

                            }

                        }
                    }

                }

            }

            undo.ResetFrames();

            return true;

        }

    }

}
