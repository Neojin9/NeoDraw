using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.MicroBiomes {

    public class MineHouse : MicroBiome {

        public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			int i = origin.X;
			int j = origin.Y;

			UnifiedRandom genRand = WorldGen.genRand;

			if (!WorldGen.InWorld(i, j, 50)) {
				DrawInterface.SetStatusBarTempMessage("Too close to edge of world.");
				return false;
			}

			int num  = genRand.Next(6, 12);
			int num2 = genRand.Next(3, 6);
			int num3 = genRand.Next(15, 30);
			int num4 = genRand.Next(15, 30);

			if (WorldGen.SolidTile(i, j)) {
				DrawInterface.SetStatusBarTempMessage("Start tile blocked.");
				return false;
			}

			if (Main.tile[i, j].wall > 0) {
				DrawInterface.SetStatusBarTempMessage("Cannot place where wall exists.");
				return false;
			}

			int num5 = j - num;
			int num6 = j + num2;

			for (int k = 0; k < 2; k++) {

				bool flag = true;

				int num7 = i;
				int num8 = j;
				int num9 = -1;
				int num10 = num3;

				if (k == 1) {
					num9 = 1;
					num10 = num4;
					num7++;
				}

				while (flag) {

					if (num8 - num < num5)
						num5 = num8 - num;
					
					if (num8 + num2 > num6)
						num6 = num8 + num2;
					
					for (int l = 0; l < 2; l++) {

						int num11 = num8;
						
						bool flag2 = true;
						
						int num12 = num;
						int num13 = -1;
						
						if (l == 1) {
							num11++;
							num12 = num2;
							num13 = 1;
						}

						while (flag2) {

							if (num7 != i && Main.tile[num7 - num9, num11].wall != WallID.Planked && (WorldGen.SolidTile(num7 - num9, num11) || !Main.tile[num7 - num9, num11].active() || Main.tile[num7 - num9, num11].halfBrick() || Main.tile[num7 - num9, num11].slope() != 0)) {
								
								undo.Add(new ChangedTile(num7 - num9, num11));
								
								Main.tile[num7 - num9, num11].active(active: true);
								Main.tile[num7 - num9, num11].type = TileID.WoodBlock;

							}

							if (WorldGen.SolidTile(num7 - 1, num11) || Main.tile[num7 - 1, num11].halfBrick() || Main.tile[num7 - 1, num11].slope() != 0) {

								undo.Add(new ChangedTile(num7 - 1, num11));

								Main.tile[num7 - 1, num11].type = TileID.WoodBlock;

							}

							if (WorldGen.SolidTile(num7 + 1, num11) || Main.tile[num7 + 1, num11].halfBrick() || Main.tile[num7 + 1, num11].slope() != 0) {

								undo.Add(new ChangedTile(num7 + 1, num11));

								Main.tile[num7 + 1, num11].type = TileID.WoodBlock;

							}

							if (WorldGen.SolidTile(num7, num11) || Main.tile[num7, num11].halfBrick() || Main.tile[num7, num11].slope() != 0) {

								int num14 = 0;
								
								if (WorldGen.SolidTile(num7 - 1, num11))
									num14++;
								
								if (WorldGen.SolidTile(num7 + 1, num11))
									num14++;
								
								if (WorldGen.SolidTile(num7, num11 - 1))
									num14++;
								
								if (WorldGen.SolidTile(num7, num11 + 1))
									num14++;
								
								if (num14 < 2) {

									undo.Add(new ChangedTile(num7, num11));

									Main.tile[num7, num11].active(active: false);

								}
								else {

									flag2 = false;

									undo.Add(new ChangedTile(num7, num11));

									Main.tile[num7, num11].type = TileID.WoodBlock;

								}

							}
							else {

								undo.Add(new ChangedTile(num7, num11));

								Main.tile[num7, num11].wall = WallID.Planked;
								Main.tile[num7, num11].liquid = 0;
								Main.tile[num7, num11].lava(lava: false);

							}

							num11 += num13;
							num12--;
							
							if (num12 <= 0) {

								if (!Main.tile[num7, num11].active()) {

									undo.Add(new ChangedTile(num7, num11));

									Main.tile[num7, num11].active(active: true);
									Main.tile[num7, num11].type = TileID.WoodBlock;

								}

								flag2 = false;

							}

						}

					}

					num10--;
					num7 += num9;

					if (WorldGen.SolidTile(num7, num8)) {

						int num15 = 0;
						int num16 = 0;
						int num17 = num8;
						
						bool flag3 = true;
						
						while (flag3) {

							num17--;
							num15++;
							
							if (WorldGen.SolidTile(num7 - num9, num17)) {
								num15 = 999;
								flag3 = false;
							}
							else if (!WorldGen.SolidTile(num7, num17)) {
								flag3 = false;
							}

						}

						num17 = num8;
						flag3 = true;
						
						while (flag3) {

							num17++;
							num16++;
							
							if (WorldGen.SolidTile(num7 - num9, num17)) {
								num16 = 999;
								flag3 = false;
							}
							else if (!WorldGen.SolidTile(num7, num17)) {
								flag3 = false;
							}

						}

						if (num16 <= num15) {

							if (num16 > num2) {
								num10 = 0;
							}
							else {
								num8 += num16 + 1;
							}

						}
						else if (num15 > num) {

							num10 = 0;

						}
						else {

							num8 -= num15 + 1;

						}

					}

					if (num10 <= 0)
						flag = false;
					
				}

			}

			int num18 = i - num3 - 1;
			int num19 = i + num4 + 2;
			int num20 = num5 - 1;
			int num21 = num6 + 2;

			for (int m = num18; m < num19; m++) {

				for (int n = num20; n < num21; n++) {

					if (Main.tile[m, n].wall == WallID.Planked && !Main.tile[m, n].active()) {

						if (Main.tile[m - 1, n].wall != WallID.Planked && m < i && !WorldGen.SolidTile(m - 1, n)) {

							PlaceTile(m, n, TileID.WoodBlock, ref undo, mute: true);

							undo.Add(new ChangedTile(m, n));

							Main.tile[m, n].wall = WallID.None;

						}

						if (Main.tile[m + 1, n].wall != WallID.Planked && m > i && !WorldGen.SolidTile(m + 1, n)) {

							PlaceTile(m, n, TileID.WoodBlock, ref undo, mute: true);

							undo.Add(new ChangedTile(m, n));

							Main.tile[m, n].wall = WallID.None;

						}

						for (int num22 = m - 1; num22 <= m + 1; num22++) {

							for (int num23 = n - 1; num23 <= n + 1; num23++) {

								if (WorldGen.SolidTile(num22, num23)) {

									undo.Add(new ChangedTile(num22, num23));

									Main.tile[num22, num23].type = TileID.WoodBlock;

								}

							}

						}

					}

					if (Main.tile[m, n].type == TileID.WoodBlock && Main.tile[m - 1, n].wall == WallID.Planked && Main.tile[m + 1, n].wall == WallID.Planked && (Main.tile[m, n - 1].wall == WallID.Planked || Main.tile[m, n - 1].active()) && (Main.tile[m, n + 1].wall == WallID.Planked || Main.tile[m, n + 1].active())) {

						undo.Add(new ChangedTile(m, n));

						Main.tile[m, n].active(active: false);
						Main.tile[m, n].wall = WallID.Planked;

					}

				}

			}

			for (int num24 = num18; num24 < num19; num24++) {

				for (int num25 = num20; num25 < num21; num25++) {

					if (Main.tile[num24, num25].type == TileID.WoodBlock) {

						if (Main.tile[num24 - 1, num25].wall == WallID.Planked && Main.tile[num24 + 1, num25].wall == WallID.Planked && !Main.tile[num24 - 1, num25].active() && !Main.tile[num24 + 1, num25].active()) {

							undo.Add(new ChangedTile(num24, num25));

							Main.tile[num24, num25].active(active: false);
							Main.tile[num24, num25].wall = WallID.Planked;

						}

						if (!TileID.Sets.BasicChest[Main.tile[num24, num25 - 1].type] && Main.tile[num24 - 1, num25].wall == WallID.Planked && Main.tile[num24 + 1, num25].type == TileID.WoodBlock && Main.tile[num24 + 2, num25].wall == WallID.Planked && !Main.tile[num24 - 1, num25].active() && !Main.tile[num24 + 2, num25].active()) {

							undo.Add(new ChangedTile(num24, num25));
							undo.Add(new ChangedTile(num24 + 1, num25));

							Main.tile[num24, num25].active(active: false);
							Main.tile[num24, num25].wall = WallID.Planked;
							Main.tile[num24 + 1, num25].active(active: false);
							Main.tile[num24 + 1, num25].wall = WallID.Planked;

						}

						if (Main.tile[num24, num25 - 1].wall == WallID.Planked && Main.tile[num24, num25 + 1].wall == WallID.Planked && !Main.tile[num24, num25 - 1].active() && !Main.tile[num24, num25 + 1].active()) {

							undo.Add(new ChangedTile(num24, num25));

							Main.tile[num24, num25].active(active: false);
							Main.tile[num24, num25].wall = WallID.Planked;

						}

					}

				}

			}

			for (int num26 = num18; num26 < num19; num26++) {

				for (int num27 = num21; num27 > num20; num27--) {

					bool flag4 = false;
					
					if (Main.tile[num26, num27].active() && Main.tile[num26, num27].type == TileID.WoodBlock) {

						int num28 = -1;
						
						for (int num29 = 0; num29 < 2; num29++) {

							if (!WorldGen.SolidTile(num26 + num28, num27) && Main.tile[num26 + num28, num27].wall == WallID.None) {

								int num30 = 0;
								int num31 = num27;
								int num32 = num27;
								
								while (Main.tile[num26, num31].active() && Main.tile[num26, num31].type == TileID.WoodBlock && !WorldGen.SolidTile(num26 + num28, num31) && Main.tile[num26 + num28, num31].wall == WallID.None) {
									num31--;
									num30++;
								}

								num31++;
								
								int num33 = num31 + 1;
								
								if (num30 > 4) {

									if (genRand.Next(2) == 0) {

										num31 = num32 - 1;
										
										bool flag5 = true;
										
										for (int num34 = num26 - 2; num34 <= num26 + 2; num34++)
											for (int num35 = num31 - 2; num35 <= num31; num35++)
												if (num34 != num26 && Main.tile[num34, num35].active())
													flag5 = false;
												
										if (flag5) {

											undo.Add(new ChangedTile(num26, num31));
											undo.Add(new ChangedTile(num26, num31 - 1));
											undo.Add(new ChangedTile(num26, num31 - 2));

											Main.tile[num26, num31].active(active: false);
											Main.tile[num26, num31 - 1].active(active: false);
											Main.tile[num26, num31 - 2].active(active: false);

											PlaceTile(num26, num31, TileID.ClosedDoor, ref undo, mute: true);
											
											flag4 = true;

										}

									}

									if (!flag4) {

										for (int num36 = num33; num36 < num32; num36++) {

											undo.Add(new ChangedTile(num26, num31));

											Main.tile[num26, num36].type = TileID.WoodenBeam;

										}

									}

								}

							}

							num28 = 1;

						}

					}

					if (flag4)
						break;
					
				}

			}

			int num37 = genRand.Next(1, 2);
			
			if (genRand.Next(4) == 0)
				num37 = 0;
			
			if (genRand.Next(6) == 0)
				num37++;
			
			if (genRand.Next(10) == 0)
				num37++;
			
			for (int num38 = 0; num38 < num37; num38++) {

				int num39 = 0;
				int num40 = genRand.Next(num18, num19);
				int num41 = genRand.Next(num20, num21);
				
				while (!Main.wallHouse[Main.tile[num40, num41].wall] || Main.tile[num40, num41].active()) {

					num39++;
					
					if (num39 > 1000)
						break;
					
					num40 = genRand.Next(num18, num19);
					num41 = genRand.Next(num20, num21);

				}

				if (num39 > 1000)
					break;

                int num42;
				int num43;
				int num44;
				int num45;

				for (int num47 = 0; num47 < 2; num47++) {

                    num42 = num40;
                    num43 = num40;

                    while (!Main.tile[num42, num41].active() && Main.wallHouse[Main.tile[num42, num41].wall])
                        num42--;

                    num42++;

                    for (; !Main.tile[num43, num41].active() && Main.wallHouse[Main.tile[num43, num41].wall]; num43++) { }

                    num43--;

                    i = (num42 + num43) / 2;

                    num44 = num41;
                    num45 = num41;

                    while (!Main.tile[num40, num44].active() && Main.wallHouse[Main.tile[num40, num44].wall])
                        num44--;

                    num44++;

                    for (; !Main.tile[num40, num45].active() && Main.wallHouse[Main.tile[num40, num45].wall]; num45++) { }

                    num45--;
                    num41 = (num44 + num45) / 2;

                }

                num42 = num40;
				num43 = num40;

				while (!Main.tile[num42, num41].active() && !Main.tile[num42, num41 - 1].active() && !Main.tile[num42, num41 + 1].active())
					num42--;
				
				num42++;

				for (; !Main.tile[num43, num41].active() && !Main.tile[num43, num41 - 1].active() && !Main.tile[num43, num41 + 1].active(); num43++) { }

				num43--;
				num44 = num41;
				num45 = num41;
				
				while (!Main.tile[num40, num44].active() && !Main.tile[num40 - 1, num44].active() && !Main.tile[num40 + 1, num44].active())
					num44--;
				
				num44++;

				for (; !Main.tile[num40, num45].active() && !Main.tile[num40 - 1, num45].active() && !Main.tile[num40 + 1, num45].active(); num45++) { }

				num45--;
				num40 = (num42 + num43) / 2;
				num41 = (num44 + num45) / 2;
				
				int num48 = num43 - num42;

                int num46 = num45 - num44;

                if (num48 <= 7 || num46 <= 5)
					continue;
				
				int num49 = 0;

				if (WorldGen.nearPicture2(i, num41))
					num49 = -1;
				
				if (num49 == 0) {

					Vector2 vector = WorldGen.randHousePicture();
					int pictureType  = (int)vector.X;
					int pictureStyle = (int)vector.Y;
					
					if (!WorldGen.nearPicture(num40, num41))
						PlaceTile(num40, num41, pictureType, ref undo, mute: true, forced: false, -1, pictureStyle);
					
				}

			}

			int num50;
			
			for (num50 = num18; num50 < num19; num50++) {

				bool flag6 = true;
				
				for (int num51 = num20; num51 < num21; num51++)
					for (int num52 = num50 - 3; num52 <= num50 + 3; num52++)
						if (Main.tile[num52, num51].active() && (!WorldGen.SolidTile(num52, num51) || Main.tile[num52, num51].type == TileID.ClosedDoor))
							flag6 = false;
						
				if (flag6)
					for (int num53 = num20; num53 < num21; num53++)
						if (Main.tile[num50, num53].wall == WallID.Planked && !Main.tile[num50, num53].active())
							PlaceTile(num50, num53, TileID.WoodenBeam, ref undo, mute: true);
						
				num50 += genRand.Next(4);

			}

			for (int num54 = 0; num54 < 4; num54++) {

				int num55 = genRand.Next(num18 + 2, num19 - 1);
				int num56 = genRand.Next(num20 + 2, num21 - 1);
				
				while (Main.tile[num55, num56].wall != WallID.Planked) {

					num55 = genRand.Next(num18 + 2, num19 - 1);
					num56 = genRand.Next(num20 + 2, num21 - 1);

				}

				while (Main.tile[num55, num56].active())
					num56--;
				
				for (; !Main.tile[num55, num56].active(); num56++) { }

				num56--;
				
				if (Main.tile[num55, num56].wall != WallID.Planked)
					continue;
				
				if (genRand.Next(3) == 0) {

					int furnitureItem = genRand.Next(9);

					if (furnitureItem == 0)
						furnitureItem = TileID.Tables;
					
					if (furnitureItem == 1)
						furnitureItem = TileID.Anvils;
					
					if (furnitureItem == 2)
						furnitureItem = TileID.WorkBenches;
					
					if (furnitureItem == 3)
						furnitureItem = TileID.Loom;
					
					if (furnitureItem == 4)
						furnitureItem = TileID.Pianos;
					
					if (furnitureItem == 5)
						furnitureItem = TileID.Kegs;
					
					if (furnitureItem == 6)
						furnitureItem = TileID.Bookcases;
					
					if (furnitureItem == 7)
						furnitureItem = TileID.GrandfatherClocks;
					
					if (furnitureItem == 8)
						furnitureItem = TileID.Sawmill;
					
					PlaceTile(num55, num56, furnitureItem, ref undo, mute: true);

				}
				else if (WorldGen.statueList != null) {

					int num58 = genRand.Next(2, WorldGen.statueList.Length);
					PlaceTile(num55, num56, WorldGen.statueList[num58].X, ref undo, mute: true, forced: true, -1, WorldGen.statueList[num58].Y);

				}

			}

			for (int num59 = 0; num59 < 40; num59++) {

				int num60 = genRand.Next(num18 + 2, num19 - 1);
				int num61 = genRand.Next(num20 + 2, num21 - 1);
				
				while (Main.tile[num60, num61].wall != 27) {

					num60 = genRand.Next(num18 + 2, num19 - 1);
					num61 = genRand.Next(num20 + 2, num21 - 1);

				}

				while (Main.tile[num60, num61].active())
					num61--;
				
				for (; !Main.tile[num60, num61].active(); num61++) { }

				num61--;
				
				if (Main.tile[num60, num61].wall == 27 && genRand.Next(2) == 0) {

					int style2 = genRand.Next(22, 26);
					PlaceTile(num60, num61, TileID.LargePiles, ref undo, mute: true, forced: false, -1, style2);

				}

			}

			for (int num62 = 0; num62 < 20; num62++) {

				int num63 = genRand.Next(num18 + 2, num19 - 1);
				int num64 = genRand.Next(num20 + 2, num21 - 1);

				while (Main.tile[num63, num64].wall != WallID.Planked) {

					num63 = genRand.Next(num18 + 2, num19 - 1);
					num64 = genRand.Next(num20 + 2, num21 - 1);

				}

				while (Main.tile[num63, num64].active())
					num64--;
				
				for (; !Main.tile[num63, num64].active(); num64++) { }

				num64--;
				
				if (Main.tile[num63, num64].wall == WallID.Planked && genRand.Next(2) == 0) {

					int x = genRand.Next(31, 34);
					PlaceSmallPile(num63, num64, x, 1, ref undo, TileID.SmallPiles);

				}

			}

			for (int num65 = 0; num65 < 15; num65++) {

				int num66 = genRand.Next(num18 + 2, num19 - 1);
				int num67 = genRand.Next(num20 + 2, num21 - 1);

				while (Main.tile[num66, num67].wall != WallID.Planked) {

					num66 = genRand.Next(num18 + 2, num19 - 1);
					num67 = genRand.Next(num20 + 2, num21 - 1);

				}

				while (Main.tile[num66, num67].active())
					num67--;
				
				while (num67 > 0 && !Main.tile[num66, num67 - 1].active())
					num67--;
				
				if (Main.tile[num66, num67].wall != WallID.Planked)
					continue;
                
				int style3 = 0;
                int num68;

                if (genRand.Next(10) < 9) {
                    num68 = -1;
                }
                else {
                    num68 = TileID.Chandeliers;
                    style3 = genRand.Next(6);
                }

                if (num68 <= 0)
					continue;
				
				PlaceTile(num66, num67, num68, ref undo, mute: true, forced: false, -1, style3);

				if (Main.tile[num66, num67].type != num68)
					continue;
				
				if (num68 == 4) {

					Main.tile[num66, num67].frameX += 54;
					continue;

				}

				int num69 = num66;
				int num70 = num67;

				num67 = num70 - Main.tile[num69, num70].frameY % 54 / 18;
				num66 = Main.tile[num69, num70].frameX / 18;
				
				if (num66 > 2)
					num66 -= 3;
				
				num66 = num69 - num66;
				short num71 = 54;

				if (Main.tile[num66, num67].frameX > 0)
					num71 = -54;
				
				for (int num72 = num66; num72 < num66 + 3; num72++) {

					for (int num73 = num67; num73 < num67 + 3; num73++) {

						undo.Add(new ChangedTile(num72, num73));

						Main.tile[num72, num73].frameX += num71;

					}

				}

			}

			return true;

		}

    }

}
