using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		private static int hellChest = 0;

		private static int[] hellChestItem = new int[5];

		public static bool AddBuriedChest(int i, int j, ref UndoStep undo, int contain = 0, bool notNearOtherChests = false, int Style = -1, bool trySlope = false, ushort chestTileType = 0) {
			
			if (chestTileType == 0)
				chestTileType = 21;
			
			bool flag  = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;

			int maxValue = 15;

			for (int k = j; k < Main.maxTilesY - 10; k++) {

				int num = -1;
				int num2 = -1;

				if (trySlope && Main.tile[i, k].active() && Main.tileSolid[Main.tile[i, k].type] && !Main.tileSolidTop[Main.tile[i, k].type]) {
					
					if (Style == 17) {

						int num3 = 30;

						for (int l = i - num3; l <= i + num3; l++) {

							for (int m = k - num3; m <= k + num3; m++) {

								if (!WorldGen.InWorld(l, m, 5))
									return false;
								
								if (Main.tile[l, m].active() && (Main.tile[l, m].type == 21 || Main.tile[l, m].type == 467))
									return false;
								
							}

						}

					}

					if (Main.tile[i - 1, k].topSlope()) {
						num = Main.tile[i - 1, k].slope();
						Main.tile[i - 1, k].slope(0);
					}

					if (Main.tile[i, k].topSlope()) {
						num2 = Main.tile[i, k].slope();
						Main.tile[i, k].slope(0);
					}

				}

				int num4 = 2;
				
				for (int n = i - num4; n <= i + num4; n++)
					for (int num5 = k - num4; num5 <= k + num4; num5++)
						if (Main.tile[n, num5].active() && (Main.tile[n, num5].type == TileID.Boulder || Main.tile[n, num5].type == 484 || Main.tile[n, num5].type == TileID.DemonAltar || Main.tile[n, num5].type == TileID.LihzahrdAltar))
							return false;

				if (!WorldGen.SolidTile(i, k))
					continue;
				
				bool flag9 = false;
				int num6 = k;
				int num7 = -1;
				int chestStyle = 0;

				if (num6 >= Main.worldSurface + 25.0 || contain > 0)
					chestStyle = 1;
				
				if (Style >= 0)
					chestStyle = Style;
				
				if (contain == 0 && num6 >= Main.worldSurface + 25.0 && num6 <= Main.maxTilesY - 205 && WldUtils.WldUtils.IsUndergroundDesert(i, k)) {
					
					flag2 = true;
					chestStyle = 10;
					chestTileType = 467;
					
					int num9 = -1;
					int num10 = -1;
					
					for (int num11 = (int)Main.worldSurface - 100; num11 < Main.maxTilesY - 200; num11++) {

						for (int num12 = 100; num12 < Main.maxTilesX - 100; num12++) {

							if (Main.tile[num12, num11].wall == 216 || Main.tile[num12, num11].wall == 187) {

								if (num9 == -1)
									num9 = num11;
								
								num10 = num11;

								break;

							}

						}

					}

					/*contain = ((num6 <= (num9 * 3 + num10 * 4) / 7) ? Utils.SelectRandom(WorldGen.genRand, new short[4] {
						4056,
						4055,
						4262,
						4263
					}) : Utils.SelectRandom(WorldGen.genRand, new short[3] {
						4061,
						4062,
						4276
					}));*/

					/*if (WorldGen.getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && (chestStyle == 11 || (contain == 0 && num6 >= Main.worldSurface + 25.0 && num6 <= Main.maxTilesY - 205 && (Main.tile[i, k].type == 147 || Main.tile[i, k].type == 161 || Main.tile[i, k].type == 162)))) {

					flag = true;
					chestStyle = 11;

					switch (WorldGen.genRand.Next(6)) {

						case 0:
							contain = 670;
							break;
						case 1:
							contain = 724;
							break;
						case 2:
							contain = 950;
							break;
						case 3:
							contain = 1319;
							break;
						case 4:
							contain = 987;
							break;
						default:
							contain = 1579;
							break;

					}

					if (WorldGen.genRand.Next(20) == 0)
						contain = 997;
					
					if (WorldGen.genRand.Next(50) == 0)
						contain = 669;
					
					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && (Style == 10 || contain == 211 || contain == 212 || contain == 213 || contain == 753)) {
					
					flag3 = true;
					chestStyle = 10;
					
					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && num6 > Main.maxTilesY - 205 && contain == 0) {

					flag7 = true;

					if (hellChest == hellChestItem[1]) {
						contain = 220;
						chestStyle = 4;
						flag9 = true;
					}
					else if (hellChest == hellChestItem[2]) {
						contain = 112;
						chestStyle = 4;
						flag9 = true;
					}
					else if (hellChest == hellChestItem[3]) {
						contain = 218;
						chestStyle = 4;
						flag9 = true;
					}
					else if (hellChest == hellChestItem[4]) {
						contain = 274;
						chestStyle = 4;
						flag9 = true;
					}
					else/* if (hellChest == hellChestItem[5])*/ {
						contain = 3019;
						chestStyle = 4;
						flag9 = true;
					}
					/*else {
						contain = 5010;
						num8 = 4;
						flag9 = true;
					}*/

					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && chestStyle == 17) {

					flag4 = true;

					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && chestStyle == 12) {

					flag5 = true;

					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && chestStyle == 32) {
					
					flag6 = true;

					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				if (chestTileType == 21 && chestStyle != 0 && WldUtils.WldUtils.IsDungeon(i, k)) {

					flag8 = true;

					/*if (getGoodWorldGen && WorldGen.genRand.Next(maxValue) == 0)
						contain = 52;
					*/
				}

				num7 = ((chestTileType != 467) ? PlaceChest(i - 1, num6 - 1, ref undo, chestTileType, notNearOtherChests, chestStyle) : PlaceChest(i - 1, num6 - 1, ref undo, chestTileType, notNearOtherChests, chestStyle));
				
				if (num7 >= 0) {

					if (flag9) {

						hellChest++;
						
						if (hellChest > 4)
							hellChest = 0;
						
					}

					Chest chest = Main.chest[num7];

					int num13 = 0;
					
					while (num13 == 0) {

						if ((chestStyle == 0 && num6 < Main.worldSurface + 25.0) || contain == 848) {

							if (contain > 0) {

								chest.item[num13].SetDefaults(contain);
								chest.item[num13].Prefix(-1);
								num13++;
								
								switch (contain) {

									case 848:

										chest.item[num13].SetDefaults(866);
										num13++;

										break;

									case 832:

										chest.item[num13].SetDefaults(933);
										num13++;

										/*if (WorldGen.genRand.Next(10) == 0) {

											int num14 = WorldGen.genRand.Next(2);

											switch (num14) {

												case 0:

													num14 = 4429;

													break;

												case 1:

													num14 = 4427;

													break;
											}

											chest.item[num13].SetDefaults(num14);
											num13++;

										}*/

										break;

								}

							}
							else {

								int num15 = WorldGen.genRand.Next(11);//12 Changed for Pre 1.4
								
								if (num15 == 0) {
									chest.item[num13].SetDefaults(280);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 1) {
									chest.item[num13].SetDefaults(281);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 2) {
									chest.item[num13].SetDefaults(284);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 3) {
									chest.item[num13].SetDefaults(282);
									chest.item[num13].stack = WorldGen.genRand.Next(40, 75);
								}
								else if (num15 == 4) {
									chest.item[num13].SetDefaults(279);
									chest.item[num13].stack = WorldGen.genRand.Next(150, 300);
								}
								else if (num15 == 5) {
									chest.item[num13].SetDefaults(285);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 6) {
									chest.item[num13].SetDefaults(953);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 7) {
									chest.item[num13].SetDefaults(946);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 8) {
									chest.item[num13].SetDefaults(3068);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 9) {
									chest.item[num13].SetDefaults(3069);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 10) {
									chest.item[num13].SetDefaults(3084);
									chest.item[num13].Prefix(-1);
								}
								else if (num15 == 11) {
									chest.item[num13].SetDefaults(4341);
									chest.item[num13].Prefix(-1);
								}

								num13++;

							}

							if (WorldGen.genRand.Next(6) == 0) {

								chest.item[num13].SetDefaults(3093);
								chest.item[num13].stack = 1;

								if (WorldGen.genRand.Next(5) == 0)
									chest.item[num13].stack += WorldGen.genRand.Next(2);
								
								if (WorldGen.genRand.Next(10) == 0)
									chest.item[num13].stack += WorldGen.genRand.Next(3);
								
								num13++;

							}

							/*if (WorldGen.genRand.Next(6) == 0) {

								chest.item[num13].SetDefaults(4345);
								chest.item[num13].stack = 1;
								
								if (WorldGen.genRand.Next(5) == 0)
									chest.item[num13].stack += WorldGen.genRand.Next(2);
								
								if (WorldGen.genRand.Next(10) == 0)
									chest.item[num13].stack += WorldGen.genRand.Next(3);
								
								num13++;

							}*/

							if (WorldGen.genRand.Next(3) == 0) {

								chest.item[num13].SetDefaults(168);
								chest.item[num13].stack = WorldGen.genRand.Next(3, 6);
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num16 = WorldGen.genRand.Next(2);
								int stack = WorldGen.genRand.Next(8) + 3;
								
								if (num16 == 0)
									chest.item[num13].SetDefaults(WorldGen.copperBar);
								
								if (num16 == 1)
									chest.item[num13].SetDefaults(WorldGen.ironBar);
								
								chest.item[num13].stack = stack;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack2 = WorldGen.genRand.Next(50, 101);

								chest.item[num13].SetDefaults(965);
								chest.item[num13].stack = stack2;
								num13++;

							}

							if (WorldGen.genRand.Next(3) != 0) {

								int num17 = WorldGen.genRand.Next(2);
								int stack3 = WorldGen.genRand.Next(26) + 25;
								
								if (num17 == 0)
									chest.item[num13].SetDefaults(40);
								
								if (num17 == 1)
									chest.item[num13].SetDefaults(42);
								
								chest.item[num13].stack = stack3;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack4 = WorldGen.genRand.Next(3) + 3;

								chest.item[num13].SetDefaults(28);
								chest.item[num13].stack = stack4;
								num13++;

							}

							if (WorldGen.genRand.Next(3) != 0) {

								chest.item[num13].SetDefaults(2350);
								chest.item[num13].stack = WorldGen.genRand.Next(3, 6);
								num13++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num18  = WorldGen.genRand.Next(6);
								int stack5 = WorldGen.genRand.Next(1, 3);
								
								if (num18 == 0) {
									chest.item[num13].SetDefaults(292);
								}
								else if (num18 == 1) {
									chest.item[num13].SetDefaults(298);
								}
								else if (num18 == 2) {
									chest.item[num13].SetDefaults(299);
								}
								else if (num18 == 3) {
									chest.item[num13].SetDefaults(290);
								}
								else if (num18 == 4) {
									chest.item[num13].SetDefaults(2322);
								}
								else if (num18 == 5) {
									chest.item[num13].SetDefaults(2325);
								}

								chest.item[num13].stack = stack5;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num19  = WorldGen.genRand.Next(2);
								int stack6 = WorldGen.genRand.Next(11) + 10;
								
								if (num19 == 0) {
									chest.item[num13].SetDefaults(8);
								}
								else if (num19 == 1) {
									chest.item[num13].SetDefaults(31);
								}

								chest.item[num13].stack = stack6;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								chest.item[num13].SetDefaults(72);
								chest.item[num13].stack = WorldGen.genRand.Next(10, 30);
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								chest.item[num13].SetDefaults(9);
								chest.item[num13].stack = WorldGen.genRand.Next(50, 100);
								num13++;

							}

						}
						else if (num6 < Main.rockLayer) {

							if (contain > 0) {

								if (contain == 832) {
									chest.item[num13].SetDefaults(933);
									num13++;
								}

								chest.item[num13].SetDefaults(contain);
								chest.item[num13].Prefix(-1);
								num13++;
								
								/*if (flag4 && WorldGen.genRand.Next(2) == 0) {
									chest.item[num13].SetDefaults(4460);
									num13++;
								}*/

								/*if (flag5 && WorldGen.genRand.Next(10) == 0) {

									int num20 = WorldGen.genRand.Next(2);
									
									switch (num20) {

										case 0:

											num20 = 4429;

											break;

										case 1:

											num20 = 4427;

											break;

									}

									chest.item[num13].SetDefaults(num20);
									num13++;

								}*/

								/*if (flag8 && (!WorldGen.generatedShadowKey || WorldGen.genRand.Next(3) == 0)) {

									WorldGen.generatedShadowKey = true;
									chest.item[num13].SetDefaults(329);
									num13++;

								}*/

							}
							else {

								switch (WorldGen.genRand.Next(5)) { //6 Changed for 1.4

									case 0:
										chest.item[num13].SetDefaults(49);
										chest.item[num13].Prefix(-1);
										break;

									case 1:
										chest.item[num13].SetDefaults(50);
										chest.item[num13].Prefix(-1);
										break;

									case 2:
										chest.item[num13].SetDefaults(53);
										chest.item[num13].Prefix(-1);
										break;

									case 3:
										chest.item[num13].SetDefaults(54);
										chest.item[num13].Prefix(-1);
										break;

									case 4:
										chest.item[num13].SetDefaults(975);
										chest.item[num13].Prefix(-1);
										break;

									default:
										chest.item[num13].SetDefaults(5011);
										chest.item[num13].Prefix(-1);
										break;

								}

								num13++;
								
								if (WorldGen.genRand.Next(20) == 0) {

									chest.item[num13].SetDefaults(997);
									chest.item[num13].Prefix(-1);
									num13++;

								}
								else if (WorldGen.genRand.Next(20) == 0) {

									chest.item[num13].SetDefaults(930);
									chest.item[num13].Prefix(-1);
									num13++;

									chest.item[num13].SetDefaults(931);
									chest.item[num13].stack = WorldGen.genRand.Next(26) + 25;
									num13++;

								}

								/*if (flag6 && WorldGen.genRand.Next(2) == 0) {

									chest.item[num13].SetDefaults(4450);
									num13++;

								}*/

								/*if (flag6 && WorldGen.genRand.Next(3) == 0) {

									chest.item[num13].SetDefaults(4779);
									num13++;
									chest.item[num13].SetDefaults(4780);
									num13++;
									chest.item[num13].SetDefaults(4781);
									num13++;

								}*/

							}

							if (flag2) {

								/*if (WorldGen.genRand.Next(3) == 0) {

									chest.item[num13].SetDefaults(4423);
									chest.item[num13].stack = WorldGen.genRand.Next(10, 20);
									num13++;

								}*/

							}
							else if (WorldGen.genRand.Next(3) == 0) {

								chest.item[num13].SetDefaults(166);
								chest.item[num13].stack = WorldGen.genRand.Next(10, 20);
								num13++;

							}

							if (WorldGen.genRand.Next(5) == 0) {

								chest.item[num13].SetDefaults(52);
								num13++;

							}

							if (WorldGen.genRand.Next(3) == 0) {

								int stack7 = WorldGen.genRand.Next(50, 101);
								
								chest.item[num13].SetDefaults(965);
								chest.item[num13].stack = stack7;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {
								
								int num21  = WorldGen.genRand.Next(2);
								int stack8 = WorldGen.genRand.Next(10) + 5;
								
								if (num21 == 0) {
									chest.item[num13].SetDefaults(WorldGen.ironBar);
								}
								else if (num21 == 1) {
									chest.item[num13].SetDefaults(WorldGen.silverBar);
								}

								chest.item[num13].stack = stack8;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num22  = WorldGen.genRand.Next(2);
								int stack9 = WorldGen.genRand.Next(25) + 25;
								
								if (num22 == 0) {
									chest.item[num13].SetDefaults(40);
								}
								else if (num22 == 1) {
									chest.item[num13].SetDefaults(42);
								}

								chest.item[num13].stack = stack9;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack10 = WorldGen.genRand.Next(3) + 3;
								
								chest.item[num13].SetDefaults(28);
								chest.item[num13].stack = stack10;
								num13++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num23 = WorldGen.genRand.Next(9);
								int stack11 = WorldGen.genRand.Next(1, 3);
								
								if (num23 == 0) {
									chest.item[num13].SetDefaults(289);
								}
								else if (num23 == 1) {
									chest.item[num13].SetDefaults(298);
								}
								else if (num23 == 2) {
									chest.item[num13].SetDefaults(299);
								}
								else if (num23 == 3) {
									chest.item[num13].SetDefaults(290);
								}
								else if (num23 == 4) {
									chest.item[num13].SetDefaults(303);
								}
								else if (num23 == 5) {
									chest.item[num13].SetDefaults(291);
								}
								else if (num23 == 6) {
									chest.item[num13].SetDefaults(304);
								}
								else if (num23 == 7) {
									chest.item[num13].SetDefaults(2322);
								}
								else if (num23 == 8) {
									chest.item[num13].SetDefaults(2329);
								}

								chest.item[num13].stack = stack11;
								num13++;

							}

							if (WorldGen.genRand.Next(3) != 0) {

								int stack12 = WorldGen.genRand.Next(2, 5);
								
								chest.item[num13].SetDefaults(2350);
								chest.item[num13].stack = stack12;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack13 = WorldGen.genRand.Next(11) + 10;
								
								if (chestStyle == 11) {
									chest.item[num13].SetDefaults(974);
								}
								else {
									chest.item[num13].SetDefaults(8);
								}

								chest.item[num13].stack = stack13;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								chest.item[num13].SetDefaults(72);
								chest.item[num13].stack = WorldGen.genRand.Next(50, 90);
								num13++;

							}

						}
						else if (num6 < Main.maxTilesY - 250) {

							if (contain > 0) {

								chest.item[num13].SetDefaults(contain);
								chest.item[num13].Prefix(-1);
								num13++;
								
								if (flag && WorldGen.genRand.Next(5) == 0) {

									chest.item[num13].SetDefaults(3199);
									num13++;

								}

								/*if (flag2) {

									if (WorldGen.genRand.Next(7) == 0) {

										chest.item[num13].SetDefaults(4346);
										num13++;

									}

									if (WorldGen.genRand.Next(15) == 0) {

										chest.item[num13].SetDefaults(4066);
										num13++;

									}

								}*/

								if (flag3 && WorldGen.genRand.Next(6) == 0) {

									chest.item[num13++].SetDefaults(3360);
									chest.item[num13++].SetDefaults(3361);

								}

								/*if (flag3 && WorldGen.genRand.Next(10) == 0)
									chest.item[num13++].SetDefaults(4426);
								
								if (flag4 && WorldGen.genRand.Next(2) == 0) {

									chest.item[num13].SetDefaults(4460);
									num13++;

								}*/

								/*if (flag8 && (!WorldGen.generatedShadowKey || WorldGen.genRand.Next(3) == 0)) {
									WorldGen.generatedShadowKey = true;
									chest.item[num13].SetDefaults(329);
									num13++;
								}*/

							}
							else {

								int num24 = WorldGen.genRand.Next(6);//7 Changed for 1.4
								
								if (WorldGen.genRand.Next(40) == 0 && num6 > WorldGen.lavaLine) {

									chest.item[num13].SetDefaults(906);
									chest.item[num13].Prefix(-1);

								}
								else if (WorldGen.genRand.Next(15) == 0) {

									chest.item[num13].SetDefaults(997);
									chest.item[num13].Prefix(-1);

								}
								else {

									if (num24 == 0) {

										chest.item[num13].SetDefaults(49);
										chest.item[num13].Prefix(-1);

									}
									else if (num24 == 1) {

										chest.item[num13].SetDefaults(50);
										chest.item[num13].Prefix(-1);

									}
									else if (num24 == 2) {

										chest.item[num13].SetDefaults(53);
										chest.item[num13].Prefix(-1);

									}
									else if (num24 == 3) {

										chest.item[num13].SetDefaults(54);
										chest.item[num13].Prefix(-1);

									}
									else if (num24 == 4) {

										chest.item[num13].SetDefaults(930);
										chest.item[num13].Prefix(-1);
										num13++;

										chest.item[num13].SetDefaults(931);
										chest.item[num13].stack = WorldGen.genRand.Next(26) + 25;

									}
									else if (num24 == 5) {

										chest.item[num13].SetDefaults(975);
										chest.item[num13].Prefix(-1);

									}
									else if (num24 == 6) {

										chest.item[num13].SetDefaults(5011);
										chest.item[num13].Prefix(-1);

									}

								}

								num13++;
								
								/*if (flag6 && WorldGen.genRand.Next(2) == 0) {

									chest.item[num13].SetDefaults(4450);
									num13++;

								}*/

								/*if (flag6 && WorldGen.genRand.Next(3) == 0) {

									chest.item[num13].SetDefaults(4779);
									num13++;
									chest.item[num13].SetDefaults(4780);
									num13++;
									chest.item[num13].SetDefaults(4781);
									num13++;

								}*/

							}

							if (WorldGen.genRand.Next(5) == 0) {

								chest.item[num13].SetDefaults(43);
								num13++;

							}

							if (WorldGen.genRand.Next(3) == 0) {

								chest.item[num13].SetDefaults(167);
								num13++;

							}

							if (WorldGen.genRand.Next(4) == 0) {

								chest.item[num13].SetDefaults(51);
								chest.item[num13].stack = WorldGen.genRand.Next(26) + 25;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num25   = WorldGen.genRand.Next(2);
								int stack14 = WorldGen.genRand.Next(8) + 3;

								if (num25 == 0) {
									chest.item[num13].SetDefaults(WorldGen.goldBar);
								}
								else if (num25 == 1) {
									chest.item[num13].SetDefaults(WorldGen.silverBar);
								}

								chest.item[num13].stack = stack14;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num26   = WorldGen.genRand.Next(2);
								int stack15 = WorldGen.genRand.Next(26) + 25;
								
								if (num26 == 0) {
									chest.item[num13].SetDefaults(41);
								}
								else if (num26 == 1) {
									chest.item[num13].SetDefaults(279);
								}

								chest.item[num13].stack = stack15;
								num13++;

							}
							if (WorldGen.genRand.Next(2) == 0) {

								int stack16 = WorldGen.genRand.Next(3) + 3;
								
								chest.item[num13].SetDefaults(188);
								chest.item[num13].stack = stack16;
								num13++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num27   = WorldGen.genRand.Next(6);
								int stack17 = WorldGen.genRand.Next(1, 3);
								
								if (num27 == 0) {
									chest.item[num13].SetDefaults(296);
								}
								else if (num27 == 1) {
									chest.item[num13].SetDefaults(295);
								}
								else if (num27 == 2) {
									chest.item[num13].SetDefaults(299);
								}
								else if (num27 == 3) {
									chest.item[num13].SetDefaults(302);
								}
								else if (num27 == 4) {
									chest.item[num13].SetDefaults(303);
								}
								else if (num27 == 5) {
									chest.item[num13].SetDefaults(305);
								}

								chest.item[num13].stack = stack17;
								num13++;

							}

							if (WorldGen.genRand.Next(3) > 1) {

								int num28 = WorldGen.genRand.Next(6);
								int stack18 = WorldGen.genRand.Next(1, 3);
								
								if (num28 == 0) {
									chest.item[num13].SetDefaults(301);
								}
								else if (num28 == 1) {
									chest.item[num13].SetDefaults(297);
								}
								else if (num28 == 2) {
									chest.item[num13].SetDefaults(304);
								}
								else if (num28 == 3) {
									chest.item[num13].SetDefaults(2329);
								}
								else if (num28 == 4) {
									chest.item[num13].SetDefaults(2351);
								}
								else if (num28 == 5) {
									chest.item[num13].SetDefaults(2326);
								}

								chest.item[num13].stack = stack18;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack19 = WorldGen.genRand.Next(2, 5);
								
								chest.item[num13].SetDefaults(2350);
								chest.item[num13].stack = stack19;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num29 = WorldGen.genRand.Next(2);
								int stack20 = WorldGen.genRand.Next(15) + 15;
								
								if (num29 == 0) {

									if (chestStyle == 11) {
										chest.item[num13].SetDefaults(974);
									}
									else {
										chest.item[num13].SetDefaults(8);
									}

								}

								if (num29 == 1)
									chest.item[num13].SetDefaults(282);
								
								chest.item[num13].stack = stack20;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								chest.item[num13].SetDefaults(73);
								chest.item[num13].stack = WorldGen.genRand.Next(1, 3);
								num13++;

							}

						}
						else {

							if (contain > 0) {

								chest.item[num13].SetDefaults(contain);
								chest.item[num13].Prefix(-1);
								num13++;

								/*if (flag7 && WorldGen.genRand.Next(10) == 0) {

									chest.item[num13].SetDefaults(4443);
									num13++;

								}

								if (flag7 && WorldGen.genRand.Next(10) == 0) {

									chest.item[num13].SetDefaults(4737);
									num13++;

								}
								else if (flag7 && WorldGen.genRand.Next(10) == 0) {

									chest.item[num13].SetDefaults(4551);
									num13++;

								}*/

							}
							else {

								int num30 = WorldGen.genRand.Next(4);
								
								if (num30 == 0) {

									chest.item[num13].SetDefaults(49);
									chest.item[num13].Prefix(-1);

								}
								else if (num30 == 1) {

									chest.item[num13].SetDefaults(50);
									chest.item[num13].Prefix(-1);

								}
								else if (num30 == 2) {

									chest.item[num13].SetDefaults(53);
									chest.item[num13].Prefix(-1);

								}
								else if (num30 == 3) {

									chest.item[num13].SetDefaults(54);
									chest.item[num13].Prefix(-1);

								}

								num13++;

							}

							if (WorldGen.genRand.Next(3) == 0) {

								chest.item[num13].SetDefaults(167);
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num31   = WorldGen.genRand.Next(2);
								int stack21 = WorldGen.genRand.Next(15) + 15;
								
								if (num31 == 0) {
									chest.item[num13].SetDefaults(117);
								}
								else if (num31 == 1) {
									chest.item[num13].SetDefaults(WorldGen.goldBar);
								}

								chest.item[num13].stack = stack21;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num32   = WorldGen.genRand.Next(2);
								int stack22 = WorldGen.genRand.Next(25) + 50;
								
								if (num32 == 0) {
									chest.item[num13].SetDefaults(265);
								}
								else if (num32 == 1) {

									/*if (WorldGen.SavedOreTiers.Silver == 168) {
										chest.item[num13].SetDefaults(4915);
									}
									else {*/
										chest.item[num13].SetDefaults(278);
									//}

								}

								chest.item[num13].stack = stack22;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack23 = WorldGen.genRand.Next(6) + 15;
								
								chest.item[num13].SetDefaults(227);
								chest.item[num13].stack = stack23;
								num13++;

							}

							if (WorldGen.genRand.Next(4) > 0) {

								int num33   = WorldGen.genRand.Next(8);
								int stack24 = WorldGen.genRand.Next(1, 3);

								if (num33 == 0) {
									chest.item[num13].SetDefaults(296);
								}
								else if (num33 == 1) {
									chest.item[num13].SetDefaults(295);
								}
								else if (num33 == 2) {
									chest.item[num13].SetDefaults(293);
								}
								else if (num33 == 3) {
									chest.item[num13].SetDefaults(288);
								}
								else if (num33 == 4) {
									chest.item[num13].SetDefaults(294);
								}
								else if (num33 == 5) {
									chest.item[num13].SetDefaults(297);
								}
								else if (num33 == 6) {
									chest.item[num13].SetDefaults(304);
								}
								else if (num33 == 7) {
									chest.item[num13].SetDefaults(2323);
								}

								chest.item[num13].stack = stack24;
								num13++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num34   = WorldGen.genRand.Next(8);
								int stack25 = WorldGen.genRand.Next(1, 3);
								
								if (num34 == 0) {
									chest.item[num13].SetDefaults(305);
								}
								else if (num34 == 1) {
									chest.item[num13].SetDefaults(301);
								}
								else if (num34 == 2) {
									chest.item[num13].SetDefaults(302);
								}
								else if (num34 == 3) {
									chest.item[num13].SetDefaults(288);
								}
								else if (num34 == 4) {
									chest.item[num13].SetDefaults(300);
								}
								else if (num34 == 5) {
									chest.item[num13].SetDefaults(2351);
								}
								else if (num34 == 6) {
									chest.item[num13].SetDefaults(2348);
								}
								else if (num34 == 7) {
									chest.item[num13].SetDefaults(2345);
								}

								chest.item[num13].stack = stack25;
								num13++;

							}

							if (WorldGen.genRand.Next(3) == 0) {

								int stack26 = WorldGen.genRand.Next(1, 3);
								
								/*if (WorldGen.genRand.Next(2) == 0) {*/
									chest.item[num13].SetDefaults(2350);
								/*}
								else {
									chest.item[num13].SetDefaults(4870);
								}*/

								chest.item[num13].stack = stack26;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num35   = WorldGen.genRand.Next(2);
								int stack27 = WorldGen.genRand.Next(15) + 15;
								
								if (num35 == 0) {
									chest.item[num13].SetDefaults(8);
								}
								else if (num35 == 1) {
									chest.item[num13].SetDefaults(282);
								}

								chest.item[num13].stack = stack27;
								num13++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								chest.item[num13].SetDefaults(73);
								chest.item[num13].stack = WorldGen.genRand.Next(2, 5);
								num13++;

							}

						}

						if (num13 <= 0 || chestTileType != 21)
							continue;
						
						if (chestStyle == 10 && WorldGen.genRand.Next(4) == 0) {

							chest.item[num13].SetDefaults(2204);
							num13++;

						}

						if (chestStyle == 11 && WorldGen.genRand.Next(7) == 0) {

							chest.item[num13].SetDefaults(2198);
							num13++;

						}

						if (chestStyle == 13 && WorldGen.genRand.Next(3) == 0) {

							chest.item[num13].SetDefaults(2197);
							num13++;

						}

						if (chestStyle == 16) {

							chest.item[num13].SetDefaults(2195);
							num13++;

						}

						if (Main.wallDungeon[Main.tile[i, k].wall] && WorldGen.genRand.Next(8) == 0) {

							chest.item[num13].SetDefaults(2192);
							num13++;

						}

						if (chestStyle == 16) {

							if (WorldGen.genRand.Next(5) == 0) {

								chest.item[num13].SetDefaults(2767);
								num13++;

							}
							else {

								chest.item[num13].SetDefaults(2766);
								chest.item[num13].stack = WorldGen.genRand.Next(3, 8);
								num13++;

							}

						}

					}

					return true;

				}

				if (trySlope) {

					if (num > -1)
						Main.tile[i - 1, k].slope((byte)num);
					
					if (num2 > -1)
						Main.tile[i, k].slope((byte)num2);
					
				}

				return false;

			}

			return false;

		}

		public static bool AddBuriedChest(int x, int y, ref UndoStep undo, int contain = 0, bool notNearOtherChests = false, int Style = -1) {

			SetupHellChest();

			bool isIceChest = false;
			bool isIvyChest = false;

			for (int yPos = y; yPos < Main.maxTilesY; yPos++) {

				if (!WorldGen.SolidTile(x, yPos))
					continue;

				bool isHellChest = false;
                int chestStyle = 0;

				if (yPos >= Main.worldSurface + 25.0 || contain > 0) {

					chestStyle = 1;

					if (Style == 10 || contain == 211 || contain == 212 || contain == 213 || contain == 753) {

						chestStyle = 10;
						isIvyChest = true;

					}

				}

				if (Style >= 0)
					chestStyle = Style;

				if (chestStyle == 11 || (contain == 0 && yPos >= Main.worldSurface + 25.0 && yPos <= Main.maxTilesY - 205 && (Main.tile[x, yPos].type == 147 || Main.tile[x, yPos].type == 161 || Main.tile[x, yPos].type == 162))) {

					isIceChest = true;
					chestStyle = 11;

					switch (WorldGen.genRand.Next(6)) {
						case 0:
							contain = 670;
							break;
						case 1:
							contain = 724;
							break;
						case 2:
							contain = 950;
							break;
						case 3:
							contain = 1319;
							break;
						case 4:
							contain = 987;
							break;
						default:
							contain = 1579;
							break;
					}

					if (WorldGen.genRand.Next(20) == 0)
						contain = 997;

					if (WorldGen.genRand.Next(50) == 0)
						contain = 669;

				}

				if (yPos > Main.maxTilesY - 205 && contain == 0) {

					if (hellChest == hellChestItem[0]) {
						contain = 274;
						chestStyle = 4;
						isHellChest = true;
					}
					else if (hellChest == hellChestItem[1]) {
						contain = 220;
						chestStyle = 4;
						isHellChest = true;
					}
					else if (hellChest == hellChestItem[2]) {
						contain = 112;
						chestStyle = 4;
						isHellChest = true;
					}
					else if (hellChest == hellChestItem[3]) {
						contain = 218;
						chestStyle = 4;
						isHellChest = true;
					}
					else {
						contain = 3019;
						chestStyle = 4;
						isHellChest = true;
					}

				}

                int chestIndex = PlaceChest(x - 1, yPos - 1, ref undo, 21, notNearOtherChests, chestStyle);

                if (chestIndex >= 0) {

					if (isHellChest) {

						hellChest++;

						if (hellChest > 4)
							hellChest = 0;

					}

					int slotIndex = 0;

					while (slotIndex == 0) {

						if ((chestStyle == 0 && yPos < Main.worldSurface + 25.0) || contain == 848) {

							if (contain > 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(contain);
								Main.chest[chestIndex].item[slotIndex].Prefix(-1);

								switch (contain) {

									case 848:
										slotIndex++;
										Main.chest[chestIndex].item[slotIndex].SetDefaults(866);
										break;

									case 832:
										slotIndex++;
										Main.chest[chestIndex].item[slotIndex].SetDefaults(933);
										break;

								}

								slotIndex++;

							}
							else {

								int num5 = WorldGen.genRand.Next(11);

								if (num5 == 0) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(280);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 1) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(281);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 2) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(284);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 3) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(282);
									Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(40, 75);
								}

								if (num5 == 4) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(279);
									Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(70, 150);
								}

								if (num5 == 5) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(285);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 6) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(953);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 7) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(946);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 8) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(3068);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 9) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(3069);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num5 == 10) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(3084);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								slotIndex++;

							}

							if (WorldGen.genRand.Next(6) == 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(3093);
								Main.chest[chestIndex].item[slotIndex].stack = 1;

								if (WorldGen.genRand.Next(5) == 0)
									Main.chest[chestIndex].item[slotIndex].stack += WorldGen.genRand.Next(2);

								if (WorldGen.genRand.Next(10) == 0)
									Main.chest[chestIndex].item[slotIndex].stack += WorldGen.genRand.Next(3);

								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) == 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(168);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(3, 6);
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num6 = WorldGen.genRand.Next(2);
								int stack = WorldGen.genRand.Next(8) + 3;

								if (num6 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.copperBar);

								if (num6 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.ironBar);

								Main.chest[chestIndex].item[slotIndex].stack = stack;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack2 = WorldGen.genRand.Next(50, 101);
								Main.chest[chestIndex].item[slotIndex].SetDefaults(965);
								Main.chest[chestIndex].item[slotIndex].stack = stack2;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) != 0) {

								int num7 = WorldGen.genRand.Next(2);
								int stack3 = WorldGen.genRand.Next(26) + 25;

								if (num7 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(40);

								if (num7 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(42);

								Main.chest[chestIndex].item[slotIndex].stack = stack3;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num8 = WorldGen.genRand.Next(1);
								int stack4 = WorldGen.genRand.Next(3) + 3;

								if (num8 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(28);

								Main.chest[chestIndex].item[slotIndex].stack = stack4;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) != 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(2350);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(2, 5);
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num9 = WorldGen.genRand.Next(6);
								int stack5 = WorldGen.genRand.Next(1, 3);

								if (num9 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(292);

								if (num9 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(298);

								if (num9 == 2)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(299);

								if (num9 == 3)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(290);

								if (num9 == 4)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2322);

								if (num9 == 5)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2325);

								Main.chest[chestIndex].item[slotIndex].stack = stack5;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num10 = WorldGen.genRand.Next(2);
								int stack6 = WorldGen.genRand.Next(11) + 10;

								if (num10 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(8);

								if (num10 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(31);

								Main.chest[chestIndex].item[slotIndex].stack = stack6;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(72);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(10, 30);
								slotIndex++;

							}

						}
						else if (yPos < Main.rockLayer) {

							if (contain > 0) {

								if (contain == 832) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(933);
									slotIndex++;
								}

								Main.chest[chestIndex].item[slotIndex].SetDefaults(contain);
								Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								slotIndex++;

							}
							else {

								int num11 = WorldGen.genRand.Next(7);

								if (WorldGen.genRand.Next(20) == 0) {

									Main.chest[chestIndex].item[slotIndex].SetDefaults(997);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);

								}
								else {

									if (num11 == 0) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(49);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}

									if (num11 == 1) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(50);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}

									if (num11 == 2) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(53);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}

									if (num11 == 3) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(54);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}

									if (num11 == 4) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(55);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}

									if (num11 == 5) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(975);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}

									if (num11 == 6) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(930);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
										slotIndex++;
										Main.chest[chestIndex].item[slotIndex].SetDefaults(931);
										Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(26) + 25;
									}

								}

								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(166);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(10, 20);
								slotIndex++;
							}

							if (WorldGen.genRand.Next(5) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(52);
								slotIndex++;
							}

							if (WorldGen.genRand.Next(3) == 0) {
								int stack7 = WorldGen.genRand.Next(50, 101);
								Main.chest[chestIndex].item[slotIndex].SetDefaults(965);
								Main.chest[chestIndex].item[slotIndex].stack = stack7;
								slotIndex++;
							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num12 = WorldGen.genRand.Next(2);
								int stack8 = WorldGen.genRand.Next(10) + 5;

								if (num12 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.ironBar);

								if (num12 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.silverBar);

								Main.chest[chestIndex].item[slotIndex].stack = stack8;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num13 = WorldGen.genRand.Next(2);
								int stack9 = WorldGen.genRand.Next(25) + 25;

								if (num13 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(40);

								if (num13 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(42);

								Main.chest[chestIndex].item[slotIndex].stack = stack9;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num14 = WorldGen.genRand.Next(1);
								int stack10 = WorldGen.genRand.Next(3) + 3;

								if (num14 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(28);

								Main.chest[chestIndex].item[slotIndex].stack = stack10;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num15 = WorldGen.genRand.Next(9);
								int stack11 = WorldGen.genRand.Next(1, 3);

								if (num15 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(289);

								if (num15 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(298);

								if (num15 == 2)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(299);

								if (num15 == 3)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(290);

								if (num15 == 4)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(303);

								if (num15 == 5)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(291);

								if (num15 == 6)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(304);

								if (num15 == 7)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2322);

								if (num15 == 8)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2329);

								Main.chest[chestIndex].item[slotIndex].stack = stack11;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) != 0) {
								int stack12 = WorldGen.genRand.Next(1, 3);
								Main.chest[chestIndex].item[slotIndex].SetDefaults(2350);
								Main.chest[chestIndex].item[slotIndex].stack = stack12;
								slotIndex++;
							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack13 = WorldGen.genRand.Next(11) + 10;

								if (chestStyle == 11) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(974);
								} else {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(8);
								}

								Main.chest[chestIndex].item[slotIndex].stack = stack13;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(72);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(50, 90);
								slotIndex++;
							}

						}
						else if (yPos < Main.maxTilesY - 250) {

							if (contain > 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(contain);
								Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								slotIndex++;

								if (isIceChest && WorldGen.genRand.Next(5) == 0) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(3199);
									slotIndex++;
								}

								if (isIvyChest && WorldGen.genRand.Next(6) == 0) {
									Main.chest[chestIndex].item[slotIndex++].SetDefaults(3360);
									Main.chest[chestIndex].item[slotIndex++].SetDefaults(3361);
								}

							}
							else {

								int num16 = WorldGen.genRand.Next(7);

								if (WorldGen.genRand.Next(40) == 0) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(906);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}
								else if (WorldGen.genRand.Next(15) == 0) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(997);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}
								else {

									if (num16 == 0) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(49);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}
									if (num16 == 1) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(50);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}
									if (num16 == 2) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(53);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}
									if (num16 == 3) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(54);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}
									if (num16 == 4) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(55);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}
									if (num16 == 5) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(975);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
									}
									if (num16 == 6) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(930);
										Main.chest[chestIndex].item[slotIndex].Prefix(-1);
										slotIndex++;
										Main.chest[chestIndex].item[slotIndex].SetDefaults(931);
										Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(26) + 25;
									}

								}

								slotIndex++;

							}

							if (WorldGen.genRand.Next(5) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(43);
								slotIndex++;
							}

							if (WorldGen.genRand.Next(3) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(167);
								slotIndex++;
							}

							if (WorldGen.genRand.Next(4) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(51);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(26) + 25;
								slotIndex++;
							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num17 = WorldGen.genRand.Next(2);
								int stack14 = WorldGen.genRand.Next(8) + 3;

								if (num17 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.goldBar);

								if (num17 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.silverBar);

								Main.chest[chestIndex].item[slotIndex].stack = stack14;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num18 = WorldGen.genRand.Next(2);
								int stack15 = WorldGen.genRand.Next(26) + 25;

								if (num18 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(41);

								if (num18 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(279);

								Main.chest[chestIndex].item[slotIndex].stack = stack15;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num19 = WorldGen.genRand.Next(1);
								int stack16 = WorldGen.genRand.Next(3) + 3;

								if (num19 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(188);

								Main.chest[chestIndex].item[slotIndex].stack = stack16;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num20 = WorldGen.genRand.Next(6);
								int stack17 = WorldGen.genRand.Next(1, 3);

								if (num20 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(296);

								if (num20 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(295);

								if (num20 == 2)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(299);

								if (num20 == 3)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(302);

								if (num20 == 4)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(303);

								if (num20 == 5)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(305);

								Main.chest[chestIndex].item[slotIndex].stack = stack17;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) > 1) {

								int num21 = WorldGen.genRand.Next(7);
								int stack18 = WorldGen.genRand.Next(1, 3);

								if (num21 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(301);

								if (num21 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(302);

								if (num21 == 2)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(297);

								if (num21 == 3)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(304);

								if (num21 == 4)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2329);

								if (num21 == 5)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2351);

								if (num21 == 6)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2329);

								Main.chest[chestIndex].item[slotIndex].stack = stack18;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int stack19 = WorldGen.genRand.Next(1, 3);
								Main.chest[chestIndex].item[slotIndex].SetDefaults(2350);
								Main.chest[chestIndex].item[slotIndex].stack = stack19;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num22 = WorldGen.genRand.Next(2);
								int stack20 = WorldGen.genRand.Next(15) + 15;

								if (num22 == 0) {

									if (chestStyle == 11) {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(974);
									} else {
										Main.chest[chestIndex].item[slotIndex].SetDefaults(8);
									}

								}

								if (num22 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(282);

								Main.chest[chestIndex].item[slotIndex].stack = stack20;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(73);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(1, 3);
								slotIndex++;

							}

						}
						else {

							if (contain > 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(contain);
								Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								slotIndex++;

							}
							else {

								int num23 = WorldGen.genRand.Next(4);

								if (num23 == 0) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(49);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num23 == 1) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(50);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num23 == 2) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(53);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								if (num23 == 3) {
									Main.chest[chestIndex].item[slotIndex].SetDefaults(54);
									Main.chest[chestIndex].item[slotIndex].Prefix(-1);
								}

								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(167);
								slotIndex++;
							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num24 = WorldGen.genRand.Next(2);
								int stack21 = WorldGen.genRand.Next(15) + 15;

								if (num24 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(117);

								if (num24 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(WorldGen.goldBar);

								Main.chest[chestIndex].item[slotIndex].stack = stack21;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num25 = WorldGen.genRand.Next(2);
								int stack22 = WorldGen.genRand.Next(25) + 50;

								if (num25 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(265);

								if (num25 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(278);

								Main.chest[chestIndex].item[slotIndex].stack = stack22;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num26 = WorldGen.genRand.Next(2);
								int stack23 = WorldGen.genRand.Next(6) + 15;

								if (num26 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(226);

								if (num26 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(227);

								Main.chest[chestIndex].item[slotIndex].stack = stack23;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(4) > 0) {

								int num27 = WorldGen.genRand.Next(8);
								int stack24 = WorldGen.genRand.Next(1, 3);

								if (num27 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(296);

								if (num27 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(295);

								if (num27 == 2)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(293);

								if (num27 == 3)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(288);

								if (num27 == 4)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(294);

								if (num27 == 5)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(297);

								if (num27 == 6)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(304);

								if (num27 == 7)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2323);

								Main.chest[chestIndex].item[slotIndex].stack = stack24;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) > 0) {

								int num28 = WorldGen.genRand.Next(8);
								int stack25 = WorldGen.genRand.Next(1, 3);

								if (num28 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(305);

								if (num28 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(301);

								if (num28 == 2)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(302);

								if (num28 == 3)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(288);

								if (num28 == 4)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(300);

								if (num28 == 5)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2351);

								if (num28 == 6)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2348);

								if (num28 == 7)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(2345);

								Main.chest[chestIndex].item[slotIndex].stack = stack25;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(3) == 0) {

								int stack26 = WorldGen.genRand.Next(1, 3);
								Main.chest[chestIndex].item[slotIndex].SetDefaults(2350);
								Main.chest[chestIndex].item[slotIndex].stack = stack26;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								int num29 = WorldGen.genRand.Next(2);
								int stack27 = WorldGen.genRand.Next(15) + 15;

								if (num29 == 0)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(8);

								if (num29 == 1)
									Main.chest[chestIndex].item[slotIndex].SetDefaults(282);

								Main.chest[chestIndex].item[slotIndex].stack = stack27;
								slotIndex++;

							}

							if (WorldGen.genRand.Next(2) == 0) {

								Main.chest[chestIndex].item[slotIndex].SetDefaults(73);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(2, 5);
								slotIndex++;

							}

						}

						if (slotIndex <= 0)
							continue;

						if (chestStyle == 10 && WorldGen.genRand.Next(4) == 0) {
							Main.chest[chestIndex].item[slotIndex].SetDefaults(2204);
							slotIndex++;
						}

						if (chestStyle == 11 && WorldGen.genRand.Next(7) == 0) {
							Main.chest[chestIndex].item[slotIndex].SetDefaults(2198);
							slotIndex++;
						}

						if (chestStyle == 12 && WorldGen.genRand.Next(2) == 0) {
							Main.chest[chestIndex].item[slotIndex].SetDefaults(2196);
							slotIndex++;
						}

						if (chestStyle == 13 && WorldGen.genRand.Next(3) == 0) {
							Main.chest[chestIndex].item[slotIndex].SetDefaults(2197);
							slotIndex++;
						}

						if (chestStyle == 16) {
							Main.chest[chestIndex].item[slotIndex].SetDefaults(2195);
							slotIndex++;
						}

						if (Main.wallDungeon[Main.tile[x, yPos].wall] && WorldGen.genRand.Next(8) == 0) {
							Main.chest[chestIndex].item[slotIndex].SetDefaults(2192);
							slotIndex++;
						}

						if (chestStyle == 16) {

							if (WorldGen.genRand.Next(5) == 0) {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(2767);
								slotIndex++;
							} else {
								Main.chest[chestIndex].item[slotIndex].SetDefaults(2766);
								Main.chest[chestIndex].item[slotIndex].stack = WorldGen.genRand.Next(3, 8);
								slotIndex++;
							}

						}

					}

					return true;

				}

				return false;

			}

			return false;

		}

		private static void SetupHellChest() {

			for (int j = 0; j < hellChestItem.Length; j++) {

				bool flag = true;

				while (flag) {

					flag = false;

					hellChestItem[j] = WorldGen.genRand.Next(hellChestItem.Length);

					for (int k = 0; k < j; k++)
						if (hellChestItem[k] == hellChestItem[j])
							flag = true;

				}

			}

		}

	}

}
