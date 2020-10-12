using System;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {
    
    public partial class WldGen { // Updated 7/11/2020

		public static bool GrowLivingTree(int i, int j, ref UndoStep undo, ushort bark, ushort leaf, byte wall, int size, bool forced = false) {

			int counter = 0;

			int[]  side   = new int[1000];
			int[]  array2 = new int[1000];
			int[]  edge   = new int[1000];
			int[]  widths = new int[1000];

			int index = 0;

			int[]  xPositions = new int[2000];
			int[]  yPositions = new int[2000];
			bool[] array7 = new bool[2000];

			while (!WorldGen.SolidTile(i, j + 1) && j + 1 < Main.maxTilesY)
				j++;

			if (!WorldGen.SolidTile(i, j + 1)) {
				DrawInterface.SetStatusBarTempMessage("No suitable ground below.");
				DrawInterface.SetStatusBarTempMessage("Hold ALT key to force placement.");
				return false;
			}

			if ((Main.tile[i, j].active() && !Main.tileCut[Main.tile[i, j].type]) && !forced) {
				DrawInterface.SetStatusBarTempMessage("Starting ground tile blocked.");
				DrawInterface.SetStatusBarTempMessage("Hold ALT key to force placement.");
				return false;
			}

			int leftX;
			int rightX;

			bool undergroundRoom = false;

			if (size > 7) { // With Room

				size -= 4;

				undergroundRoom = true;

			}
			else if (size == 0) { // Default size, so it's random whether or not there will be a room.
				undergroundRoom = WorldGen.genRand.Next(3) != 0;
            }

			if (size < 1 || size > 7) {

				leftX = i - WorldGen.genRand.Next(1, 4);
				rightX = i + WorldGen.genRand.Next(1, 4);

			}
			else {

				leftX = i - (int)Math.Ceiling(size / 2f);
				rightX = i + (int)Math.Floor(size / 2f);

			}

			if (j < 150) {
				DrawInterface.SetStatusBarTempMessage("Too close to the top of the world.");
				DrawInterface.SetStatusBarTempMessage("Hold ALT key to force placement.");
				return false;
			}
			
			int farLeft = i - 50;
			int farRight = i + 50;

			for (int curX = farLeft; curX <= farRight; curX++) {
				for (int curY = 5; curY < j - 5; curY++) {
					if ((Main.tile[curX, curY].active() && !Main.tileCut[Main.tile[curX, curY].type]) && !forced) {
						DrawInterface.SetStatusBarTempMessage("Not enough side clearance.");
						DrawInterface.SetStatusBarTempMessage("Hold ALT key to force placement.");
						return false;
					}
				}
			}

			int num7 = leftX;
			int num8 = rightX;
			int num9 = leftX;
			int num10 = rightX;
			int width = rightX - leftX;
			bool flag = true;
			int num12 = WorldGen.genRand.Next(-10, -5);
			int num13 = WorldGen.genRand.Next(2);
			int currentY = j;

			while (flag) {

				num12++;

				if (num12 > WorldGen.genRand.Next(5, 30)) {

					num12 = 0;
					array2[counter] = currentY + WorldGen.genRand.Next(5);

					if (WorldGen.genRand.Next(5) == 0) {
						num13 = ((num13 == 0) ? 1 : 0);
					}

					if (num13 == 0) {

						edge[counter] = -1;
						side[counter] = leftX;
						widths[counter] = rightX - leftX;

						if (WorldGen.genRand.Next(2) == 0)
							leftX++;

						num7++;
						num13 = 1;

					}
					else {

						edge[counter] = 1;
						side[counter] = rightX;
						widths[counter] = rightX - leftX;

						if (WorldGen.genRand.Next(2) == 0)
							rightX--;

						num8--;
						num13 = 0;

					}

					if (num7 == num8)
						flag = false;

					counter++;

				}

				for (int xPos = leftX; xPos <= rightX; xPos++) {

					undo.Add(new ChangedTile(xPos, currentY));

					Main.tile[xPos, currentY].type = bark;
					Main.tile[xPos, currentY].active(active: true);
					Main.tile[xPos, currentY].halfBrick(halfBrick: false);
					Main.tile[xPos, currentY].slope(0);

				}

				currentY--;

			}

			for (int n = 0; n < counter; n++) {

				int currentX = side[n] + edge[n];
				int yPos = array2[n];
				int num17 = (int)(widths[n] * (1f + WorldGen.genRand.Next(20, 30) * 0.1f));

				undo.Add(new ChangedTile(currentX, yPos + 1));

				Main.tile[currentX, yPos + 1].type = bark;
				Main.tile[currentX, yPos + 1].active(active: true);
				Main.tile[currentX, yPos + 1].halfBrick(halfBrick: false);
				Main.tile[currentX, yPos + 1].slope(0);

				int num18 = WorldGen.genRand.Next(3, 5);
				
				while (num17 > 0) {

					num17--;

					undo.Add(new ChangedTile(currentX, yPos));

					Main.tile[currentX, yPos].type = bark;
					Main.tile[currentX, yPos].active(active: true);
					Main.tile[currentX, yPos].halfBrick(halfBrick: false);
					Main.tile[currentX, yPos].slope(0);

					if (WorldGen.genRand.Next(10) == 0) {
						yPos = (WorldGen.genRand.Next(2) == 0 ? yPos-- : yPos++);
					}
					else {
						currentX += edge[n];
					}

					if (num18 > 0) {
						num18--;
					}
					else if (WorldGen.genRand.Next(2) == 0) {

						num18 = WorldGen.genRand.Next(2, 5);

						if (WorldGen.genRand.Next(2) == 0) {

							undo.Add(new ChangedTile(currentX, yPos));
							undo.Add(new ChangedTile(currentX, yPos - 1));

							Main.tile[currentX, yPos].type = bark;
							Main.tile[currentX, yPos].active(active: true);
							Main.tile[currentX, yPos].halfBrick(halfBrick: false);
							Main.tile[currentX, yPos].slope(0);

							Main.tile[currentX, yPos - 1].type = bark;
							Main.tile[currentX, yPos - 1].active(active: true);
							Main.tile[currentX, yPos - 1].halfBrick(halfBrick: false);
							Main.tile[currentX, yPos - 1].slope(0);

							xPositions[index] = currentX;
							yPositions[index] = yPos;
							index++;

						}
						else {

							undo.Add(new ChangedTile(currentX, yPos));
							undo.Add(new ChangedTile(currentX, yPos + 1));

							Main.tile[currentX, yPos].type = bark;
							Main.tile[currentX, yPos].active(active: true);
							Main.tile[currentX, yPos].halfBrick(halfBrick: false);
							Main.tile[currentX, yPos].slope(0);

							Main.tile[currentX, yPos + 1].type = bark;
							Main.tile[currentX, yPos + 1].active(active: true);
							Main.tile[currentX, yPos + 1].halfBrick(halfBrick: false);
							Main.tile[currentX, yPos + 1].slope(0);

							xPositions[index] = currentX;
							yPositions[index] = yPos;
							index++;

						}

					}

					if (num17 == 0) {

						xPositions[index] = currentX;
						yPositions[index] = yPos;
						index++;

					}

				}

			}

			int num19 = (leftX + rightX) / 2;
			int num20 = currentY;
			int num21 = WorldGen.genRand.Next(width * 3, width * 5);
			int num22 = 0;
			int num23 = 0;

			while (num21 > 0) {

				undo.Add(new ChangedTile(num19, num20));

				Main.tile[num19, num20].type = bark;
				Main.tile[num19, num20].active(active: true);
				Main.tile[num19, num20].halfBrick(halfBrick: false);
				Main.tile[num19, num20].slope(0);

				if (num22 > 0)
					num22--;

				if (num23 > 0)
					num23--;

				for (int num24 = -1; num24 < 2; num24++) {

					if (num24 == 0 || ((num24 >= 0 || num22 != 0) && (num24 <= 0 || num23 != 0)) || WorldGen.genRand.Next(2) != 0)
						continue;

					int num25 = num19;
					int num26 = num20;
					int num27 = WorldGen.genRand.Next(width, width * 3);

					if (num24 < 0)
						num22 = WorldGen.genRand.Next(3, 5);

					if (num24 > 0)
						num23 = WorldGen.genRand.Next(3, 5);

					int num28 = 0;

					while (num27 > 0) {

						num27--;
						num25 += num24;

						undo.Add(new ChangedTile(num25, num26));

						Main.tile[num25, num26].type = bark;
						Main.tile[num25, num26].active(active: true);
						Main.tile[num25, num26].halfBrick(halfBrick: false);
						Main.tile[num25, num26].slope(0);

						if (num27 == 0) {

							xPositions[index] = num25;
							yPositions[index] = num26;
							array7[index] = true;
							index++;

						}

						if (WorldGen.genRand.Next(5) == 0) {

							num26 = WorldGen.genRand.Next(2) == 0 ? num26-- : num26++;

							undo.Add(new ChangedTile(num25, num26));

							Main.tile[num25, num26].type = bark;
							Main.tile[num25, num26].active(active: true);
							Main.tile[num25, num26].halfBrick(halfBrick: false);
							Main.tile[num25, num26].slope(0);

						}

						if (num28 > 0) {
							num28--;
						}
						else if (WorldGen.genRand.Next(3) == 0) {

							num28 = WorldGen.genRand.Next(2, 4);
							int num29 = num25;
							int num30 = num26;
							num30 = WorldGen.genRand.Next(2) == 0 ? num30-- : num30++;

							undo.Add(new ChangedTile(num29, num30));

							Main.tile[num29, num30].type = bark;
							Main.tile[num29, num30].active(active: true);
							Main.tile[num29, num30].halfBrick(halfBrick: false);
							Main.tile[num29, num30].slope(0);

							xPositions[index] = num29;
							yPositions[index] = num30;
							array7[index] = true;
							index++;

						}

					}

				}

				xPositions[index] = num19;
				yPositions[index] = num20;
				index++;

				if (WorldGen.genRand.Next(4) == 0) {

					num19 = WorldGen.genRand.Next(2) == 0 ? num19-- : num19++;

					undo.Add(new ChangedTile(num19, num20));

					Main.tile[num19, num20].type = bark;
					Main.tile[num19, num20].active(active: true);
					Main.tile[num19, num20].halfBrick(halfBrick: false);
					Main.tile[num19, num20].slope(0);

				}

				num20--;
				num21--;

			}

			for (int num31 = num9; num31 <= num10; num31++) {

				int num32 = WorldGen.genRand.Next(1, 6);
				int num33 = j + 1;

				while (num32 > 0) {

					if (WorldGen.SolidTile(num31, num33))
						num32--;

					undo.Add(new ChangedTile(num31, num33));

					Main.tile[num31, num33].type = bark;
					Main.tile[num31, num33].active(active: true);
					Main.tile[num31, num33].halfBrick(halfBrick: false);
					Main.tile[num31, num33].slope(0);

					num33++;

				}

				int num34 = num33;
				
				for (int num35 = 0; num35 < 2; num35++) {

					num33 = num34;
					int num36 = (num9 + num10) / 2;
					int num37 = 0;
					int num38 = 1;
					num37 = num31 < num36 ? -1 : 1;
					
					if (num31 == num36 || (width > 6 && (num31 == num36 - 1 || num31 == num36 + 1)))
						num37 = 0;
					
					int num39 = num37;
					int num40 = num31;
					num32 = WorldGen.genRand.Next((int)(width * 2.5), width * 4);
					
					while (num32 > 0) {

						num32--;
						num40 += num37;

						undo.Add(new ChangedTile(num40, num33));

						Main.tile[num40, num33].type = bark;
						Main.tile[num40, num33].active(active: true);
						Main.tile[num40, num33].halfBrick(halfBrick: false);
						Main.tile[num40, num33].slope(0);

						num33 += num38;

						undo.Add(new ChangedTile(num40, num33));

						Main.tile[num40, num33].type = bark;
						Main.tile[num40, num33].active(active: true);
						Main.tile[num40, num33].halfBrick(halfBrick: false);
						Main.tile[num40, num33].slope(0);

						if (!Main.tile[num40, num33 + 1].active()) {
							num37 = 0;
							num38 = 1;
						}

						if (WorldGen.genRand.Next(3) == 0) {
							num37 = ((num39 >= 0) ? ((num39 <= 0) ? WorldGen.genRand.Next(-1, 2) : ((num37 == 0) ? 1 : 0)) : ((num37 == 0) ? (-1) : 0));
						}

						if (WorldGen.genRand.Next(3) == 0)
							num38 = (num38 == 0) ? 1 : 0;

					}

				}

			}

			for (int num41 = 0; num41 < index; num41++) {

				int num42 = WorldGen.genRand.Next(5, 8);
				num42 = (int)(num42 * (1f + width * 0.05f));
				
				if (array7[num41])
					num42 = WorldGen.genRand.Next(7, 13);
				
				int num43 = xPositions[num41] - num42;
				int num44 = xPositions[num41] + num42;
				int num45 = yPositions[num41] - num42;
				int num46 = yPositions[num41] + num42;
				float num47 = 2f - WorldGen.genRand.Next(5) * 0.1f;
				
				for (int num48 = num43; num48 <= num44; num48++) {

					for (int num49 = num45; num49 <= num46; num49++) {

						if (Main.tile[num48, num49].type != bark && Math.Abs(xPositions[num41] - num48) + Math.Abs(yPositions[num41] - num49) * num47 < num42) {

							undo.Add(new ChangedTile(num48, num49));

							Main.tile[num48, num49].type = leaf;
							Main.tile[num48, num49].active(active: true);
							Main.tile[num48, num49].halfBrick(halfBrick: false);
							Main.tile[num48, num49].slope(0);

						}

					}

				}

			}

			if (width >= 4 && undergroundRoom) {

				bool flag2 = false;
				int num50 = num9;
				int num51 = num10;
				int num52 = j - 5;
				int num53 = 50;
				int num54 = WorldGen.genRand.Next(400, 700);
				int num55 = 1;
				bool flag3 = true;

				while (num54 > 0) {

					num52++;
					num54--;
					num53--;
					int num56 = (num9 + num10) / 2;
					int num57 = 0;
					
					if (num52 > j && width == 4)
						num57 = 1;
					
					for (int num58 = num9 - num57; num58 <= num10 + num57; num58++) {

						if (num58 > num56 - 2 && num58 <= num56 + 1) {

							if (Main.tile[num58, num52].type != TileID.Platforms) {

								undo.Add(new ChangedTile(num58, num52));

								Main.tile[num58, num52].active(active: false);

							}

							undo.Add(new ChangedTile(num58, num52));

							Main.tile[num58, num52].wall = wall;

							if (Main.tile[num58 - 1, num52].wall > 0 || num52 >= Main.worldSurface) {

								undo.Add(new ChangedTile(num58 - 1, num52));

								Main.tile[num58 - 1, num52].wall = wall;

							}
							
							if (Main.tile[num58 + 1, num52].wall > 0 || num52 >= Main.worldSurface) {

								undo.Add(new ChangedTile(num58 + 1, num52));

								Main.tile[num58 + 1, num52].wall = wall;

							}

						}
						else {

							undo.Add(new ChangedTile(num58, num52));

							Main.tile[num58, num52].type = bark;
							Main.tile[num58, num52].active(active: true);
							Main.tile[num58, num52].halfBrick(halfBrick: false);
							Main.tile[num58, num52].slope(0);

						}

					}

					num55++;

					if (num55 >= 6) {

						num55 = 0;
						int num59 = WorldGen.genRand.Next(3);
						
						if (num59 == 0)
							num59 = -1;
						
						if (flag3)
							num59 = 2;
						
						if (num59 == 2) {

							flag3 = false;

							for (int num60 = num9; num60 <= num10; num60++) {

								if (num60 > num56 - 2 && num60 <= num56 + 1) {

									undo.Add(new ChangedTile(num60, num52 + 1));

									Main.tile[num60, num52 + 1].active(active: false);
									WorldGen.PlaceTile(num60, num52 + 1, TileID.Platforms, mute: true, forced: false, -1, 23); // Is this caught by GlobalTile's UNDO

								}

							}

						}
						else {

							num9 += num59;
							num10 += num59;

						}

						if (num53 <= 0 && !flag2) {

							flag2 = true;
							int num61 = WorldGen.genRand.Next(2);
							
							if (num61 == 0)
								num61 = -1;
							
							int num62 = num52 - 2;
							int num63 = num52;
							int num64 = (num9 + num10) / 2;
							
							if (num61 < 0)
								num64--;
							
							if (num61 > 0)
								num64++;
							
							int num65 = WorldGen.genRand.Next(15, 30);
							int num66 = num64 + num65;
							
							if (num61 < 0) {
								num66 = num64;
								num64 -= num65;
							}
							
							bool flag4 = false;
							
							for (int num67 = num64; num67 < num66; num67++)
								for (int num68 = num52 - 20; num68 < num52 + 10; num68++)
									if (Main.tile[num67, num68].wall == 0 && !Main.tile[num67, num68].active() && num68 < Main.worldSurface)
										flag4 = true;

							if (!flag4) {

								for (int num69 = num64; num69 <= num66; num69++) {

									for (int num70 = num62 - 2; num70 <= num63 + 2; num70++) {

										if (Main.tile[num69, num70].wall != wall && Main.tile[num69, num70].type != TileID.Platforms) {

											undo.Add(new ChangedTile(num69, num70));

											Main.tile[num69, num70].active(active: true);
											Main.tile[num69, num70].type = bark;
											Main.tile[num69, num70].halfBrick(halfBrick: false);
											Main.tile[num69, num70].slope(0);

										}

										if (num70 >= num62 && num70 <= num63) {

											undo.Add(new ChangedTile(num69, num70));

											Main.tile[num69, num70].liquid = 0;
											Main.tile[num69, num70].wall = wall;
											Main.tile[num69, num70].active(active: false);

										}

									}

								}

								int i2 = (num9 + num10) / 2 + 3 * num61;
								int j2 = num52;

								PlaceDoor(i2, j2, TileID.ClosedDoor, ref undo, 7);

								int num71 = WorldGen.genRand.Next(5, 9);
								int num72 = WorldGen.genRand.Next(4, 6);

								if (num61 < 0) {
									num66 = num64 + num71;
									num64 -= num71;
								}
								else {
									num64 = num66 - num71;
									num66 += num71;
								}
								
								num62 = num63 - num72;
								
								for (int num73 = num64 - 2; num73 <= num66 + 2; num73++) {

									for (int num74 = num62 - 2; num74 <= num63 + 2; num74++) {

										if (Main.tile[num73, num74].wall != wall && Main.tile[num73, num74].type != TileID.Platforms) {

											undo.Add(new ChangedTile(num73, num74));

											Main.tile[num73, num74].active(active: true);
											Main.tile[num73, num74].type = bark;
											Main.tile[num73, num74].halfBrick(halfBrick: false);
											Main.tile[num73, num74].slope(0);

										}

										if (num74 >= num62 && num74 <= num63 && num73 >= num64 && num73 <= num66) {

											undo.Add(new ChangedTile(num73, num74));

											Main.tile[num73, num74].liquid = 0;
											Main.tile[num73, num74].wall = wall;
											Main.tile[num73, num74].active(active: false);

										}

									}

								}

								i2 = num64 - 2;

								if (num61 < 0)
									i2 = num66 + 2;
								
								PlaceDoor(i2, j2, TileID.ClosedDoor, ref undo, 7);
								
								int num75 = num66;
								
								if (num61 < 0)
									num75 = num64;
								
								Place1x2(num75, num52, TileID.Chairs, 5, ref undo);
								
								if (num61 < 0) {

									undo.Add(new ChangedTile(num75, num52 - 1));
									undo.Add(new ChangedTile(num75, num52));

									Main.tile[num75, num52 - 1].frameX += 18;
									Main.tile[num75, num52].frameX += 18;

								}

								num75 = num66 - 2;
								
								if (num61 < 0) {
									num75 = num64 + 2;
								}
								
								Place3x2(num75, num52, TileID.Tables, ref undo, 6);
								
								num75 = num66 - 4;
								
								if (num61 < 0)
									num75 = num64 + 4;

								Place1x2(num75, num52, TileID.Chairs, 5, ref undo);
								
								if (num61 > 0) {

									undo.Add(new ChangedTile(num75, num52 - 1));
									undo.Add(new ChangedTile(num75, num52));

									Main.tile[num75, num52 - 1].frameX += 18;
									Main.tile[num75, num52].frameX += 18;

								}
								
								num75 = num66 - 7;
								
								if (num61 < 0)
									num75 = num64 + 8;

								int contain = WorldGen.genRand.Next(2) == 0 ? ItemID.LivingWoodWand : ItemID.LeafWand;

								AddBuriedChest(num75, num52, ref undo, contain, notNearOtherChests: false, 12, false, 0);

							}

						}

					}

					if (num53 > 0) {
						continue;
					}

					bool flag5 = true;

					for (int num77 = num9; num77 <= num10; num77++)
						for (int num78 = num52 + 1; num78 <= num52 + 4; num78++)
							if (WorldGen.SolidTile(num77, num78))
								flag5 = false;

					if (flag5)
						num54 = 0;

				}

				num9 = num50;
				num10 = num51;
				int num79 = (num9 + num10) / 2;
				
				if (WorldGen.genRand.Next(2) == 0) {
					num10 = num79;
				}
				else {
					num9 = num79;
				}
				
				for (int num80 = num9; num80 <= num10; num80++) {

					for (int num81 = j - 3; num81 <= j; num81++) {

						undo.Add(new ChangedTile(num80, num81));

						Main.tile[num80, num81].active(active: false);
						bool flag6 = true;
						
						for (int num82 = num80 - 1; num82 <= num80 + 1; num82++)
							for (int num83 = num81 - 1; num83 <= num81 + 1; num83++)
								if (!Main.tile[num82, num83].active() && Main.tile[num82, num83].wall == 0)
									flag6 = false;

						if (flag6) {

							undo.Add(new ChangedTile(num80, num81));

							Main.tile[num80, num81].wall = wall;

						}

					}

				}

			}

			undo.ResetFrames();

			return true;

		}

	}

}
