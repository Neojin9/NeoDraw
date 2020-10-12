using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static void OceanCave(int i, int j, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			List<Point> chestLocations = new List<Point>();

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			Vector2 vector2 = default;

			if (i < Main.maxTilesX / 2) {
				vector2.X = 0.25f + genRand.NextFloat() * 0.25f;
			}
			else {
				vector2.X = -0.35f - genRand.NextFloat() * 0.5f;
			}

			vector2.Y = 0.4f + genRand.NextFloat() * 0.25f;
			
			ushort sapphireTile  = TileID.SapphireGemspark;
			ushort sand = TileID.Sand;
			ushort hardenedSand = TileID.HardenedSand;
			double num4 = genRand.Next(17, 25);
			double iterations = genRand.Next(600, 800);
			double stoppingPoint = 4.0;

			bool flag = true;

			while (num4 > stoppingPoint && iterations > 0.0) {

				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = true;

				if (vector.X > BeachDistance - 50 && vector.X < Main.maxTilesX - BeachDistance + 50) {
					num4 *= 0.96;
					iterations *= 0.96;
				}

				if (num4 < stoppingPoint + 2.0 || iterations < 20.0)
					flag4 = false;
				
				if (flag) {
					num4 -= 0.01 + genRand.NextFloat() * 0.01;
					iterations -= 0.5;
				}
				else {
					num4 -= 0.02 + genRand.NextFloat() * 0.02;
					iterations -= 1.0;
				}

				if (flag4) {
					chestLocations.Add(new Point((int)vector.X, (int)vector.Y));
				}

				int num7  = (int)(vector.X - num4 * 3.0);
				int num8  = (int)(vector.X + num4 * 3.0);
				int num9  = (int)(vector.Y - num4 * 3.0);
				int num10 = (int)(vector.Y + num4 * 3.0);

				if (num7 < 1)
					num7 = 1;
				
				if (num8 > Main.maxTilesX - 1)
					num8 = Main.maxTilesX - 1;
				
				if (num9 < 1)
					num9 = 1;
				
				if (num10 > Main.maxTilesY - 1)
					num10 = Main.maxTilesY - 1;
				
				for (int x1 = num7; x1 < num8; x1++) {

					for (int y1 = num9; y1 < num10; y1++) {

						//if (badOceanCaveTiles(k, l))
						//	continue;
						
						float num11 = new Vector2(Math.Abs(x1 - vector.X), Math.Abs(y1 - vector.Y)).Length();

						if (flag4 && num11 < num4 * 0.5 + 1.0) {

							undo.Add(new ChangedTile(x1, y1));

							Main.tile[x1, y1].type = sapphireTile;
							Main.tile[x1, y1].active(active: false);

						}
						else if (num11 < num4 * 1.5 + 1.0 && Main.tile[x1, y1].type != sapphireTile) {

							if (y1 < vector.Y) {

								if ((vector2.X < 0f && x1 < vector.X) || (vector2.X > 0f && x1 > vector.X)) {

									if (num11 < num4 * 1.1 + 1.0) {

										undo.Add(new ChangedTile(x1, y1));

										Main.tile[x1, y1].slope(0);
										Main.tile[x1, y1].halfBrick(halfBrick: false);
										Main.tile[x1, y1].type = hardenedSand;
										
										if (Main.tile[x1, y1].liquid == byte.MaxValue)
											Main.tile[x1, y1].wall = 0;
										
									}
									else if (Main.tile[x1, y1].type != hardenedSand) {

										undo.Add(new ChangedTile(x1, y1));

										Main.tile[x1, y1].slope(0);
										Main.tile[x1, y1].halfBrick(halfBrick: false);
										Main.tile[x1, y1].type = sand;

									}

								}

							}
							else if ((vector2.X < 0f && x1 < i) || (vector2.X > 0f && x1 > i)) {

								undo.Add(new ChangedTile(x1, y1));

								if (Main.tile[x1, y1].liquid == byte.MaxValue)
									Main.tile[x1, y1].wall = 0;

								Main.tile[x1, y1].slope(0);
								Main.tile[x1, y1].halfBrick(halfBrick: false);
								Main.tile[x1, y1].type = sand;
								Main.tile[x1, y1].active(active: true);

								if (x1 == (int)vector.X && flag2) {

									flag2 = false;
									
									int num12 = 50 + genRand.Next(3);
									int num13 = 43 + genRand.Next(3);
									int num14 = 20 + genRand.Next(3);
									int num15 = x1;
									int num16 = x1 + num14;
									
									if (vector2.X < 0f) {

										num15 = x1 - num14;
										num16 = x1;

									}

									if (iterations < 100.0) {

										num12 = (int)((float)num12 * (iterations / 100.0));
										num13 = (int)((float)num13 * (iterations / 100.0));
										num14 = (int)((float)num14 * (iterations / 100.0));

									}

									if (num4 < stoppingPoint + 5.0) {

										double num17 = (num4 - stoppingPoint) / 5.0;
										
										num12 = (int)((float)num12 * num17);
										num13 = (int)((float)num13 * num17);
										num14 = (int)((float)num14 * num17);

									}

									for (int x2 = num15; x2 <= num16; x2++) {

										for (int y2 = y1; y2 < y1 + num12 /*&& !badOceanCaveTiles(m, n)*/; y2++) {

											undo.Add(new ChangedTile(x2, y2));

											Main.tile[x2, y2].slope(0);
											Main.tile[x2, y2].halfBrick(halfBrick: false);

											if (y2 > y1 + num13) {

												if (WorldGen.SolidTile(x2, y2) && Main.tile[x2, y2].type != sand)
													break;

												Main.tile[x2, y2].type = hardenedSand;

											}
											else {

												Main.tile[x2, y2].type = sand;

											}

											Main.tile[x2, y2].active(active: true);
											
											if (genRand.Next(3) == 0) {

												undo.Add(new ChangedTile(x2 - 1, y2));

												Main.tile[x2 - 1, y2].type = sand;
												Main.tile[x2 - 1, y2].active(active: true);

											}
											
											if (genRand.Next(3) == 0) {

												undo.Add(new ChangedTile(x2 + 1, y2));

												Main.tile[x2 + 1, y2].type = sand;
												Main.tile[x2 + 1, y2].active(active: true);

											}

										}

									}

								}

							}

						}

						if (num11 < num4 * 1.3 + 1.0 && y1 > j - 10) {

							undo.Add(new ChangedTile(x1, y1));

							Main.tile[x1, y1].liquid = byte.MaxValue;

						}

						if (!flag3 || x1 != (int)vector.X || !(y1 > vector.Y))
							continue;
						
						flag3 = false;

						int num18 = 100;
						int num19 = 2;
						
						for (int x3 = x1 - num19; x3 <= x1 + num19; x3++) {

							for (int y3 = y1; y3 < y1 + num18; y3++) {

								if (Main.tile[x3, y3] == null)
									Main.tile[x3, y3] = new Tile();

								//if (!badOceanCaveTiles(x3, y3)) {
									Main.tile[x3, y3].liquid = byte.MaxValue;
								//}

							}

						}

					}

				}

				vector += vector2;
				vector2.X += genRand.NextFloat() * 0.1f - 0.05f;
				vector2.Y += genRand.NextFloat() * 0.1f - 0.05f;
				
				if (flag) {

					if (vector.Y > (Main.worldSurface * 2.0 + Main.rockLayer) / 3.0 && vector.Y > j + 30)
						flag = false;
					
					vector2.Y = MathHelper.Clamp(vector2.Y, 0.35f, 1f);

				}
				else {

					if (vector.X < Main.maxTilesX / 2) {

						if (vector2.X < 0.5f)
							vector2.X += 0.02f;
						
					}
					else if (vector2.X > -0.5f) {

						vector2.X -= 0.02f;

					}

					if (!flag4) {

						if (vector2.Y < 0f)
							vector2.Y *= 0.95f;
						
						vector2.Y += 0.04f;

					}
					else if (vector.Y < (Main.worldSurface * 4.0 + Main.rockLayer) / 5.0) {

						if (vector2.Y < 0f)
							vector2.Y *= 0.97f;
						
						vector2.Y += 0.02f;

					}
					else if (vector2.Y > -0.1f) {

						vector2.Y *= 0.99f;
						vector2.Y -= 0.01f;

					}

					vector2.Y = MathHelper.Clamp(vector2.Y, -1f, 1f);

				}

				if (vector.X < Main.maxTilesX / 2) {

					vector2.X = MathHelper.Clamp(vector2.X, 0.1f, 1f);

				}
				else {

					vector2.X = MathHelper.Clamp(vector2.X, -1f, -0.1f);

				}

			}

			int chestLocationsCount = chestLocations.Count;

			if (chestLocationsCount > 0) {

				int contain = genRand.NextFromList(new short[4] {
				863,
				186,
				277,
				187/*,
				4404,
				4425*/
			});

				bool chestPlaced = false;

				float distance = 2f;

				while (!chestPlaced && distance < 50f) {

					distance += 0.1f;

					int xPos = genRand.Next(chestLocations[chestLocationsCount - 1].X - (int)distance, chestLocations[chestLocationsCount - 1].X + (int)distance + 1);
					int yPos = genRand.Next(chestLocations[chestLocationsCount - 1].Y - (int)distance / 2, chestLocations[chestLocationsCount - 1].Y + (int)distance / 2 + 1);
					
					xPos = ((xPos >= Main.maxTilesX) ? ((int)((float)xPos + distance / 2f)) : ((int)((float)xPos - distance / 2f)));
					
					if (Main.tile[xPos, yPos].liquid > 250 && Main.tile[xPos, yPos].liquidType() == 0)
						chestPlaced = AddBuriedChest(xPos, yPos, ref undo, contain, notNearOtherChests: false, 17, trySlope: true, 0);

				}

			}

			undo.ResetFrames();

		}

	}

}
