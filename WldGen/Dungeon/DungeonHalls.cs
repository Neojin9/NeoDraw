using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/1/2020 Copy/Paste

		public static void DungeonHalls(int i, int j, ushort tileType, ushort wallType, ref UndoStep undo, bool forceX = false) {

			UnifiedRandom genRand = WorldGen.genRand;

			Vector2 zero = Vector2.Zero;

			double num  = genRand.Next(4, 6);
			double num2 = num;

			Vector2 zero2 = Vector2.Zero;
			Vector2 zero3 = Vector2.Zero;

			int num3;

			Vector2 vector = default;
            vector.X = i;
			vector.Y = j;

			int num4 = genRand.Next(35, 80);

			bool crackedHall = false;

			if (genRand.Next(5) == 0)
				crackedHall = true;

			crackedHall = false; // TODO: Remove for v1.4

			if (forceX) {
				num4 += 20;
				lastDungeonHall = Vector2.Zero;
			}
			else if (genRand.Next(5) == 0) {
				num *= 2.0;
				num4 /= 2;
			}

			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = true;

            while (!flag2) {

                bool flag5 = false;

                if (flag4 && !forceX) {

                    bool flag6 = true;
                    bool flag7 = true;
                    bool flag8 = true;
                    bool flag9 = true;

                    int num5 = num4;
                    
					bool flag10 = false;
                    
					for (int num6 = j; num6 > j - num5; num6--) {

                        if (Main.tile[i, num6].wall == wallType) {

                            if (flag10) {

                                flag6 = false;

                                break;

                            }

                        }
                        else {

                            flag10 = true;

                        }

                    }

                    flag10 = false;

                    for (int k = j; k < j + num5; k++) {

                        if (Main.tile[i, k].wall == wallType) {

                            if (flag10) {

                                flag7 = false;

                                break;

                            }

                        }
                        else {

                            flag10 = true;

                        }

                    }

                    flag10 = false;

                    for (int num7 = i; num7 > i - num5; num7--) {

                        if (Main.tile[num7, j].wall == wallType) {

                            if (flag10) {

                                flag8 = false;

                                break;

                            }

                        }
                        else {

                            flag10 = true;

                        }

                    }

                    flag10 = false;

                    for (int l = i; l < i + num5; l++) {

                        if (Main.tile[l, j].wall == wallType) {

                            if (flag10) {

                                flag9 = false;

                                break;

                            }

                        }
                        else {

                            flag10 = true;

                        }

                    }

                    if (!flag8 && !flag9 && !flag6 && !flag7) {

                        num3 = ((genRand.Next(2) != 0) ? 1 : (-1));

                        if (genRand.Next(2) == 0)
                            flag5 = true;
                        
                    }
                    else {

						int num8;

                        do { num8 = genRand.Next(4); }
                        while (!(num8 == 0 && flag6) && !(num8 == 1 && flag7) && !(num8 == 2 && flag8) && !(num8 == 3 && flag9));

                        switch (num8) {

                            case 0:

                                num3 = -1;

                                break;

                            case 1:

                                num3 = 1;

                                break;

                            default:

                                flag5 = true;
                                num3 = ((num8 != 2) ? 1 : (-1));

                                break;

                        }

                    }

                }
                else {

                    num3 = ((genRand.Next(2) != 0) ? 1 : (-1));
                    
					if (genRand.Next(2) == 0)
                        flag5 = true;
                    
                }

                flag4 = false;
                
				if (forceX)
                    flag5 = true;
                
                if (flag5) {

                    zero2.Y = 0f;
                    zero2.X = num3;
                    zero3.Y = 0f;
                    zero3.X = -num3;
                    zero.Y = 0f;
                    zero.X = num3;
                    
					if (genRand.Next(3) == 0) {

                        if (genRand.Next(2) == 0) {
                            zero.Y = -0.2f;
                        }
                        else {
                            zero.Y = 0.2f;
                        }

                    }

                }
                else {

                    num += 1.0;
                    zero.Y = num3;
                    zero.X = 0f;
                    zero2.X = 0f;
                    zero2.Y = num3;
                    zero3.X = 0f;
                    zero3.Y = -num3;

                    if (genRand.Next(3) != 0) {

                        flag3 = true;
                        
						if (genRand.Next(2) == 0) {
                            zero.X = genRand.Next(10, 20) * 0.1f;
                        }
                        else {
                            zero.X = -genRand.Next(10, 20) * 0.1f;
                        }

                    }
                    else if (genRand.Next(2) == 0) {

                        if (genRand.Next(2) == 0) {
                            zero.X = genRand.Next(20, 40) * 0.01f;
                        }
                        else {
                            zero.X = -genRand.Next(20, 40) * 0.01f;
                        }

                    }
                    else {

                        num4 /= 2;

                    }

                }

                if (lastDungeonHall != zero3)
                    flag2 = true;
                
            }

            int num9 = 0;

			if (!forceX) {

				if (vector.X > Main.maxTilesX - 200) {

					num3 = -1;
					zero2.Y = 0f;
					zero2.X = num3;
					zero.Y = 0f;
					zero.X = num3;

					if (genRand.Next(3) == 0) {

						if (genRand.Next(2) == 0) {
							zero.Y = -0.2f;
						}
						else {
							zero.Y = 0.2f;
						}

					}

				}
				else if (vector.X < 200f) {

					num3 = 1;
					zero2.Y = 0f;
					zero2.X = num3;
					zero.Y = 0f;
					zero.X = num3;

					if (genRand.Next(3) == 0) {

						if (genRand.Next(2) == 0) {
							zero.Y = -0.2f;
						}
						else {
							zero.Y = 0.2f;
						}

					}

				}
				else if (vector.Y > Main.maxTilesY - 300) {

					num3 = -1;
					num += 1.0;
					zero.Y = num3;
					zero.X = 0f;
					zero2.X = 0f;
					zero2.Y = num3;

					if (genRand.Next(2) == 0) {

						if (genRand.Next(2) == 0) {
							zero.X = genRand.Next(20, 50) * 0.01f;
						}
						else {
							zero.X = -genRand.Next(20, 50) * 0.01f;
						}

					}

				}
				else if (vector.Y < Main.rockLayer + 100.0) {

					num3 = 1;
					num += 1.0;
					zero.Y = num3;
					zero.X = 0f;
					zero2.X = 0f;
					zero2.Y = num3;
					
					if (genRand.Next(3) != 0) {

						flag3 = true;
						
						if (genRand.Next(2) == 0) {
							zero.X = genRand.Next(10, 20) * 0.1f;
						}
						else {
							zero.X = -genRand.Next(10, 20) * 0.1f;
						}

					}
					else if (genRand.Next(2) == 0) {
						
						if (genRand.Next(2) == 0) {
							zero.X = genRand.Next(20, 50) * 0.01f;
						}
						else {
							zero.X = genRand.Next(20, 50) * 0.01f;
						}

					}

				}
				else if (vector.X < Main.maxTilesX / 2 && vector.X > Main.maxTilesX * 0.25) {

					num3 = -1;
					zero2.Y = 0f;
					zero2.X = num3;
					zero.Y = 0f;
					zero.X = num3;
					
					if (genRand.Next(3) == 0) {

						if (genRand.Next(2) == 0) {
							zero.Y = -0.2f;
						}
						else {
							zero.Y = 0.2f;
						}

					}

				}
				else if (vector.X > Main.maxTilesX / 2 && vector.X < Main.maxTilesX * 0.75) {

					num3 = 1;
					zero2.Y = 0f;
					zero2.X = num3;
					zero.Y = 0f;
					zero.X = num3;

					if (genRand.Next(3) == 0) {

						if (genRand.Next(2) == 0) {
							zero.Y = -0.2f;
						}
						else {
							zero.Y = 0.2f;
						}

					}

				}

			}

			if (zero2.Y == 0f) {

				DDoorX[numDDoors] = (int)vector.X;
				DDoorY[numDDoors] = (int)vector.Y;
				DDoorPos[numDDoors] = 0;
				numDDoors++;

			}
			else {

				DPlatX[numDPlats] = (int)vector.X;
				DPlatY[numDPlats] = (int)vector.Y;
				numDPlats++;

			}

			lastDungeonHall = zero2;

			if (Math.Abs(zero.X) > Math.Abs(zero.Y) && genRand.Next(3) != 0)
				num = (int)((float)num2 * (genRand.Next(110, 150) * 0.01));
			
			while (num4 > 0) {

				num9++;
				
				if (zero2.X > 0f && vector.X > Main.maxTilesX - 100) {
					num4 = 0;
				}
				else if (zero2.X < 0f && vector.X < 100f) {
					num4 = 0;
				}
				else if (zero2.Y > 0f && vector.Y > Main.maxTilesY - 100) {
					num4 = 0;
				}
				else if (zero2.Y < 0f && vector.Y < Main.rockLayer + 50.0) {
					num4 = 0;
				}
				
				num4--;
				
				int minX = (int)(vector.X - num - 4.0 - genRand.Next(6));
				int maxX = (int)(vector.X + num + 4.0 + genRand.Next(6));
				int minY = (int)(vector.Y - num - 4.0 - genRand.Next(6));
				int maxY = (int)(vector.Y + num + 4.0 + genRand.Next(6));

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				for (int m = minX; m < maxX; m++) {

					for (int n = minY; n < maxY; n++) {

						if (m < dMinX)
							dMinX = m;
						
						if (m > dMaxX)
							dMaxX = m;
						
						if (n > dMaxY)
							dMaxY = n;

						Neo.SetLiquid(m, n, 0, ref undo);

						if (!Main.wallDungeon[Main.tile[m, n].wall])
							Neo.SetTile(m, n, tileType);

					}

				}

				for (int num14 = minX + 1; num14 < maxX - 1; num14++)
					for (int num15 = minY + 1; num15 < maxY - 1; num15++)
						Neo.SetWall(num14, num15, wallType, ref undo);

				int num16 = 0;

				if (zero.Y == 0f && genRand.Next((int)num + 1) == 0) {
					num16 = genRand.Next(1, 3);
				}
				else if (zero.X == 0f && genRand.Next((int)num - 1) == 0) {
					num16 = genRand.Next(1, 3);
				}
				else if (genRand.Next((int)num * 3) == 0) {
					num16 = genRand.Next(1, 3);
				}

				minX = (int)(vector.X - num * 0.5 - num16);
				maxX = (int)(vector.X + num * 0.5 + num16);
				minY = (int)(vector.Y - num * 0.5 - num16);
				maxY = (int)(vector.Y + num * 0.5 + num16);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				for (int num17 = minX; num17 < maxX; num17++) {

					for (int num18 = minY; num18 < maxY; num18++) {

						undo.Add(new ChangedTile(num17, num18));

						Main.tile[num17, num18].Clear(TileDataType.Slope);

						if (crackedHall) {

							if (Main.tile[num17, num18].active() || Main.tile[num17, num18].wall != wallType)
								Neo.SetTile(num17, num18, CrackedType);

						}
						else {

							Neo.SetActive(num17, num18, false);

						}

						Main.tile[num17, num18].Clear(TileDataType.Slope);
						Neo.SetWall(num17, num18, wallType);

					}

				}

				vector += zero;

				if (flag3 && num9 > genRand.Next(10, 20)) {

					num9 = 0;
					zero.X *= -1f;

				}

			}

			dungeonX = (int)vector.X;
			dungeonY = (int)vector.Y;
			
			if (zero2.Y == 0f) {

				DDoorX[numDDoors] = (int)vector.X;
				DDoorY[numDDoors] = (int)vector.Y;
				DDoorPos[numDDoors] = 0;
				numDDoors++;

			}
			else {

				DPlatX[numDPlats] = (int)vector.X;
				DPlatY[numDPlats] = (int)vector.Y;
				numDPlats++;

			}

		}

	}

}
