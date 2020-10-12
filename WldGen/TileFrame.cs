using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NeoDraw.WldGen {

    public partial class WldGen {

        public static bool mergeUp;
        public static bool mergeDown;
        public static bool mergeLeft;
        public static bool mergeRight;

		public static void TileFrame(int i, int j, ref UndoStep undo, bool resetFrame = false, bool noBreak = false) {

			bool addToList = false;

			try {

				if (i > 5 && j > 5 && i < Main.maxTilesX - 5 && j < Main.maxTilesY - 5 && Main.tile[i, j] != null) {

					addToList = WorldGen.UpdateMapTile(i, j);
					Tile tile = Main.tile[i, j];
					
					if (!tile.active()) {
						tile.halfBrick(halfBrick: false);
						tile.color(0);
						tile.slope(0);
					}

					if (tile.liquid > 0 && Main.netMode != NetmodeID.MultiplayerClient && !WorldGen.noLiquidCheck)
						Liquid.AddWater(i, j);
					
					if (tile.active()) {

						if (!TileLoader.TileFrame(i, j, tile.type, ref resetFrame, ref noBreak) || (noBreak && Main.tileFrameImportant[tile.type] && !TileLoader.IsTorch(tile.type)))
							return;
						
						int num = tile.type;

						if (Main.tileStone[num])
							num = 1;
						
						int frameX = tile.frameX;
						int frameY = tile.frameY;

						Rectangle rectangle = new Rectangle(-1, -1, 0, 0);
						
						if (Main.tileFrameImportant[tile.type]) {

							if (!TileLoader.IsTorch(num)) {

								switch (num) {
									case 442: {
											Tile tile39 = Main.tile[i, j - 1];
											Tile tile40 = Main.tile[i, j + 1];
											Tile tile41 = Main.tile[i - 1, j];
											Tile tile42 = Main.tile[i + 1, j];
											Tile tile43 = Main.tile[i - 1, j + 1];
											Tile tile44 = Main.tile[i + 1, j + 1];
											Tile tile45 = Main.tile[i - 1, j - 1];
											Tile tile46 = Main.tile[i + 1, j - 1];
											int num34 = -1;
											int num35 = -1;
											int num36 = -1;
											int num37 = -1;
											int num38 = -1;
											int num39 = -1;
											int num40 = -1;
											int num41 = -1;
											if (tile39 != null && tile39.nactive() && !tile39.bottomSlope()) {
												num35 = tile39.type;
											}
											if (tile40 != null && tile40.nactive() && !tile40.halfBrick() && !tile40.topSlope()) {
												num34 = tile40.type;
											}
											if (tile41 != null && tile41.nactive() && (tile41.slope() == 0 || (int)tile41.slope() % 2 != 1)) {
												num36 = tile41.type;
											}
											if (tile42 != null && tile42.nactive() && (tile42.slope() == 0 || (int)tile42.slope() % 2 != 0)) {
												num37 = tile42.type;
											}
											if (tile43 != null && tile43.nactive()) {
												num38 = tile43.type;
											}
											if (tile44 != null && tile44.nactive()) {
												num39 = tile44.type;
											}
											if (tile45 != null && tile45.nactive()) {
												num40 = tile45.type;
											}
											if (tile46 != null && tile46.nactive()) {
												num41 = tile46.type;
											}
											bool flag3 = false;
											bool flag4 = false;
											bool flag5 = false;
											bool flag6 = false;
											if (num34 >= 0 && Main.tileSolid[num34] && (!Main.tileNoAttach[num34] || TileID.Sets.Platforms[num34]) && (tile40.bottomSlope() || tile40.slope() == 0) && !tile40.halfBrick()) {
												flag6 = true;
											}
											if (num35 >= 0 && Main.tileSolid[num35] && (!Main.tileNoAttach[num35] || (TileID.Sets.Platforms[num35] && tile39.halfBrick())) && (tile39.topSlope() || tile39.slope() == 0 || tile39.halfBrick())) {
												flag3 = true;
											}
											if ((num36 >= 0 && Main.tileSolid[num36] && !Main.tileNoAttach[num36] && (tile41.leftSlope() || tile41.slope() == 0) && !tile41.halfBrick()) || num36 == 124 || (num36 == 5 && num40 == 5 && num38 == 5)) {
												flag4 = true;
											}
											if ((num37 >= 0 && Main.tileSolid[num37] && !Main.tileNoAttach[num37] && (tile42.rightSlope() || tile42.slope() == 0) && !tile42.halfBrick()) || num37 == 124 || (num37 == 5 && num41 == 5 && num39 == 5)) {
												flag5 = true;
											}
											int num42 = tile.frameX / 22;
											bool flag7 = false;
											switch (num42) {
												case 0:
													flag7 = !flag6;
													break;
												case 1:
													flag7 = !flag3;
													break;
												case 2:
													flag7 = !flag4;
													break;
												case 3:
													flag7 = !flag5;
													break;
												default:
													flag7 = true;
													break;
											}
											if (flag7) {
												if (flag6) {
													tile.frameX = 0;
												}
												else if (flag3) {
													tile.frameX = 22;
												}
												else if (flag4) {
													tile.frameX = 44;
												}
												else if (flag5) {
													tile.frameX = 66;
												}
												else {
													KillTile(i, j, ref undo);
												}
											}
											return;
										}
									case 136: {
											Tile tile31 = Main.tile[i, j - 1];
											Tile tile32 = Main.tile[i, j + 1];
											Tile tile33 = Main.tile[i - 1, j];
											Tile tile34 = Main.tile[i + 1, j];
											Tile tile35 = Main.tile[i - 1, j + 1];
											Tile tile36 = Main.tile[i + 1, j + 1];
											Tile tile37 = Main.tile[i - 1, j - 1];
											Tile tile38 = Main.tile[i + 1, j - 1];
											int num27 = -1;
											int num28 = -1;
											int num29 = -1;
											int num30 = -1;
											int num31 = -1;
											int num32 = -1;
											int num33 = -1;
											if (tile31 != null && tile31.nactive()) {
												_ = tile31.type;
											}
											if (tile32 != null && tile32.nactive() && !tile32.halfBrick() && !tile32.topSlope()) {
												num27 = tile32.type;
											}
											if (tile33 != null && tile33.nactive()) {
												num28 = tile33.type;
											}
											if (tile34 != null && tile34.nactive()) {
												num29 = tile34.type;
											}
											if (tile35 != null && tile35.nactive()) {
												num30 = tile35.type;
											}
											if (tile36 != null && tile36.nactive()) {
												num31 = tile36.type;
											}
											if (tile37 != null && tile37.nactive()) {
												num32 = tile37.type;
											}
											if (tile38 != null && tile38.nactive()) {
												num33 = tile38.type;
											}
											if (num27 >= 0 && Main.tileSolid[num27] && !Main.tileNoAttach[num27] && !tile32.halfBrick() && (tile32.slope() == 0 || tile32.bottomSlope())) {
												tile.frameX = 0;
											}
											else if ((num28 >= 0 && Main.tileSolid[num28] && !Main.tileNoAttach[num28] && (tile33.leftSlope() || tile33.slope() == 0) && !tile33.halfBrick()) || num28 == 124 || (num28 == 5 && num32 == 5 && num30 == 5)) {
												tile.frameX = 18;
											}
											else if ((num29 >= 0 && Main.tileSolid[num29] && !Main.tileNoAttach[num29] && (tile34.rightSlope() || tile34.slope() == 0) && !tile34.halfBrick()) || num29 == 124 || (num29 == 5 && num33 == 5 && num31 == 5)) {
												tile.frameX = 36;
											}
											else if (tile.wall > 0) {
												tile.frameX = 54;
											}
											else {
												KillTile(i, j, ref undo);
											}
											return;
										}
									case 129:
									case 149: {
											Tile tile47 = Main.tile[i, j - 1];
											Tile tile48 = Main.tile[i, j + 1];
											Tile tile49 = Main.tile[i - 1, j];
											Tile tile50 = Main.tile[i + 1, j];
											int num43 = -1;
											int num44 = -1;
											int num45 = -1;
											int num46 = -1;
											if (tile47 != null && tile47.nactive() && !tile47.bottomSlope()) {
												num44 = tile47.type;
											}
											if (tile48 != null && tile48.nactive() && !tile48.halfBrick() && !tile48.topSlope()) {
												num43 = tile48.type;
											}
											if (tile49 != null && tile49.nactive()) {
												num45 = tile49.type;
											}
											if (tile50 != null && tile50.nactive()) {
												num46 = tile50.type;
											}
											if (num43 >= 0 && Main.tileSolid[num43] && !Main.tileSolidTop[num43]) {
												tile.frameY = 0;
											}
											else if (num45 >= 0 && Main.tileSolid[num45] && !Main.tileSolidTop[num45]) {
												tile.frameY = 54;
											}
											else if (num46 >= 0 && Main.tileSolid[num46] && !Main.tileSolidTop[num46]) {
												tile.frameY = 36;
											}
											else if (num44 >= 0 && Main.tileSolid[num44] && !Main.tileSolidTop[num44]) {
												tile.frameY = 18;
											}
											else {
												KillTile(i, j, ref undo);
											}
											return;
										}
									default:
										if (num != 461) {
											switch (num) {
												case 178: {
														Tile tile27 = Main.tile[i, j - 1];
														Tile tile28 = Main.tile[i, j + 1];
														Tile tile29 = Main.tile[i - 1, j];
														Tile tile30 = Main.tile[i + 1, j];
														int num22 = -1;
														int num23 = -1;
														int num24 = -1;
														int num25 = -1;
														if (tile27 != null && tile27.active() && !tile27.bottomSlope()) {
															num23 = tile27.type;
														}
														if (tile28 != null && tile28.active() && !tile28.halfBrick() && !tile28.topSlope()) {
															num22 = tile28.type;
														}
														if (tile29 != null && tile29.active()) {
															num24 = tile29.type;
														}
														if (tile30 != null && tile30.active()) {
															num25 = tile30.type;
														}
														short num26 = (short)(WorldGen.genRand.Next(3) * 18);
														if (num22 >= 0 && Main.tileSolid[num22] && !Main.tileSolidTop[num22]) {
															if (tile.frameY < 0 || tile.frameY > 36) {
																tile.frameY = num26;
															}
														}
														else if (num24 >= 0 && Main.tileSolid[num24] && !Main.tileSolidTop[num24]) {
															if (tile.frameY < 108 || tile.frameY > 54) {
																tile.frameY = (short)(108 + num26);
															}
														}
														else if (num25 >= 0 && Main.tileSolid[num25] && !Main.tileSolidTop[num25]) {
															if (tile.frameY < 162 || tile.frameY > 198) {
																tile.frameY = (short)(162 + num26);
															}
														}
														else if (num23 >= 0 && Main.tileSolid[num23] && !Main.tileSolidTop[num23]) {
															if (tile.frameY < 54 || tile.frameY > 90) {
																tile.frameY = (short)(54 + num26);
															}
														}
														else {
															KillTile(i, j, ref undo);
														}
														return;
													}
												case 184: {
														Tile tile23 = Main.tile[i, j - 1];
														Tile tile24 = Main.tile[i, j + 1];
														Tile tile25 = Main.tile[i - 1, j];
														Tile tile26 = Main.tile[i + 1, j];
														int num17 = -1;
														int num18 = -1;
														int num19 = -1;
														int num20 = -1;
														if (tile23 != null && tile23.active() && !tile23.bottomSlope()) {
															num18 = tile23.type;
														}
														if (tile24 != null && tile24.active() && !tile24.halfBrick() && !tile24.topSlope()) {
															num17 = tile24.type;
														}
														if (tile25 != null && tile25.active()) {
															num19 = tile25.type;
														}
														if (tile26 != null && tile26.active()) {
															num20 = tile26.type;
														}
														short num21 = (short)(WorldGen.genRand.Next(3) * 18);
														if (num17 >= 0 && Main.tileMoss[num17]) {
															if (num17 == 381) {
																tile.frameX = 110;
															}
															else {
																tile.frameX = (short)(22 * (num17 - 179));
															}
															if (tile.frameY < 0 || tile.frameY > 36) {
																tile.frameY = num21;
															}
														}
														else if (num18 >= 0 && Main.tileMoss[num18]) {
															if (num18 == 381) {
																tile.frameX = 110;
															}
															else {
																tile.frameX = (short)(22 * (num18 - 179));
															}
															if (tile.frameY < 54 || tile.frameY > 90) {
																tile.frameY = (short)(54 + num21);
															}
														}
														else if (num19 >= 0 && Main.tileMoss[num19]) {
															if (num19 == 381) {
																tile.frameX = 110;
															}
															else {
																tile.frameX = (short)(22 * (num19 - 179));
															}
															if (tile.frameY < 108 || tile.frameY > 54) {
																tile.frameY = (short)(108 + num21);
															}
														}
														else if (num20 >= 0 && Main.tileMoss[num20]) {
															if (num20 == 381) {
																tile.frameX = 110;
															}
															else {
																tile.frameX = (short)(22 * (num20 - 179));
															}
															if (tile.frameY < 162 || tile.frameY > 198) {
																tile.frameY = (short)(162 + num21);
															}
														}
														else {
															KillTile(i, j, ref undo);
														}
														return;
													}
												case 3:
												case 24:
												case 61:
												case 71:
												case 73:
												case 74:
												case 110:
												case 113:
												case 201:
													WorldGen.PlantCheck(i, j);
													return;
												case 227:
													WorldGen.CheckDye(i, j);
													return;
												case 12:
												case 31:
													WorldGen.CheckOrb(i, j, num);
													return;
												case 165:
													WorldGen.CheckTight(i, j);
													return;
												case 324: {
														Tile tile22 = Main.tile[i, j + 1];
														if (tile22 == null) {
															tile22 = new Tile();
															Main.tile[i, j + 1] = tile22;
														}
														if (!tile22.nactive() || (!Main.tileSolid[tile22.type] && !Main.tileSolidTop[tile22.type])) {
															KillTile(i, j, ref undo);
														}
														return;
													}
												case 235:
													WorldGen.Check3x1(i, j, num);
													return;
												case 185:
													WorldGen.CheckPile(i, j);
													return;
												default:
													if (num != 296 && num != 297 && num != 309 && num != 358 && num != 359 && num != 413 && num != 414) {
														if (num == 10) {
															if (!WorldGen.destroyObject) {
																int num2 = j;
																bool flag = false;
																int frameY2 = tile.frameY;
																int num3 = frameY2 / 54;
																num3 += tile.frameX / 54 * 36;
																num2 = j - frameY2 % 54 / 18;
																Tile tile2 = Main.tile[i, num2 - 1];
																Tile tile3 = Main.tile[i, num2];
																Tile tile4 = Main.tile[i, num2 + 1];
																Tile tile5 = Main.tile[i, num2 + 2];
																Tile tile6 = Main.tile[i, num2 + 3];
																if (tile2 == null) {
																	tile2 = new Tile();
																	Main.tile[i, num2 - 1] = tile2;
																}
																if (tile3 == null) {
																	tile3 = new Tile();
																	Main.tile[i, num2] = tile3;
																}
																if (tile4 == null) {
																	tile4 = new Tile();
																	Main.tile[i, num2 + 1] = tile4;
																}
																if (tile5 == null) {
																	tile5 = new Tile();
																	Main.tile[i, num2 + 2] = tile5;
																}
																if (tile6 == null) {
																	tile6 = new Tile();
																	Main.tile[i, num2 + 3] = tile6;
																}
																if (!tile2.active() || !Main.tileSolid[tile2.type]) {
																	flag = true;
																}
																if (!WorldGen.SolidTile(tile6)) {
																	flag = true;
																}
																if (!tile3.active() || tile3.type != num) {
																	flag = true;
																}
																if (!tile4.active() || tile4.type != num) {
																	flag = true;
																}
																if (!tile5.active() || tile5.type != num) {
																	flag = true;
																}
																if (flag) {
																	WorldGen.destroyObject = true;
																	KillTile(i, num2, ref undo);
																	KillTile(i, num2 + 1, ref undo);
																	KillTile(i, num2 + 2, ref undo);
																	WorldGen.DropDoorItem(i, j, num3);
																}
																WorldGen.destroyObject = false;
															}
														}
														else if (num == 11) {
															if (!WorldGen.destroyObject) {
																int num4 = 0;
																int num5 = i;
																int num6 = j;
																short frameX2 = tile.frameX;
																int frameY3 = tile.frameY;
																int num7 = frameY3 / 54;
																num7 += tile.frameX / 72 * 36;
																num6 = j - frameY3 % 54 / 18;
																bool flag2 = false;
																switch (frameX2 % 72) {
																	case 0:
																		num5 = i;
																		num4 = 1;
																		break;
																	case 18:
																		num5 = i - 1;
																		num4 = 1;
																		break;
																	case 36:
																		num5 = i + 1;
																		num4 = -1;
																		break;
																	case 54:
																		num5 = i;
																		num4 = -1;
																		break;
																}
																Tile tile7 = Main.tile[num5, num6 - 1];
																Tile tile8 = Main.tile[num5, num6 + 3];
																if (tile7 == null) {
																	tile7 = new Tile();
																	Main.tile[num5, num6 - 1] = tile7;
																}
																if (tile8 == null) {
																	tile8 = new Tile();
																	Main.tile[num5, num6 + 3] = tile8;
																}
																if (!tile7.active() || !Main.tileSolid[tile7.type] || !WorldGen.SolidTile(tile8)) {
																	flag2 = true;
																	WorldGen.destroyObject = true;
																	WorldGen.DropDoorItem(i, j, num7);
																}
																int num8 = num5;
																if (num4 == -1) {
																	num8 = num5 - 1;
																}
																for (int k = num8; k < num8 + 2; k++) {
																	for (int l = num6; l < num6 + 3; l++) {
																		if (!flag2) {
																			Tile tile9 = Main.tile[k, l];
																			if (!tile9.active() || tile9.type != 11) {
																				WorldGen.destroyObject = true;
																				WorldGen.DropDoorItem(i, j, num7);
																				flag2 = true;
																				k = num8;
																				l = num6;
																			}
																		}
																		if (flag2) {
																			KillTile(k, l, ref undo);
																		}
																	}
																}
																WorldGen.destroyObject = false;
															}
														}
														else if (num == 314) {
															Minecart.FrameTrack(i, j, pound: false);
														}
														else if (num == 380) {
															Tile tile10 = Main.tile[i - 1, j];
															if (tile10 != null) {
																Tile tile11 = Main.tile[i + 1, j];
																if (tile11 != null && Main.tile[i - 1, j + 1] != null && Main.tile[i + 1, j + 1] != null && Main.tile[i - 1, j - 1] != null && Main.tile[i + 1, j - 1] != null) {
																	int num9 = -1;
																	int num10 = -1;
																	if (tile10 != null && tile10.active()) {
																		num10 = (Main.tileStone[tile10.type] ? 1 : tile10.type);
																	}
																	if (tile11 != null && tile11.active()) {
																		num9 = (Main.tileStone[tile11.type] ? 1 : tile11.type);
																	}
																	if (num9 >= 0 && !Main.tileSolid[num9]) {
																		num9 = -1;
																	}
																	if (num10 >= 0 && !Main.tileSolid[num10]) {
																		num10 = -1;
																	}
																	if (num10 == num && num9 == num) {
																		rectangle.X = 18;
																	}
																	else if (num10 == num && num9 != num) {
																		rectangle.X = 36;
																	}
																	else if (num10 != num && num9 == num) {
																		rectangle.X = 0;
																	}
																	else {
																		rectangle.X = 54;
																	}
																	tile.frameX = (short)rectangle.X;
																}
															}
														}
														else if (num >= 0 && TileID.Sets.Platforms[num]) {
															Tile tile12 = Main.tile[i - 1, j];
															if (tile12 != null) {
																Tile tile13 = Main.tile[i + 1, j];
																if (tile13 != null) {
																	Tile tile14 = Main.tile[i - 1, j + 1];
																	if (tile14 != null) {
																		Tile tile15 = Main.tile[i + 1, j + 1];
																		if (tile15 != null) {
																			Tile tile16 = Main.tile[i - 1, j - 1];
																			if (tile16 != null) {
																				Tile tile17 = Main.tile[i + 1, j - 1];
																				if (tile17 != null) {
																					int num11 = -1;
																					int num12 = -1;
																					if (tile12 != null && tile12.active()) {
																						num12 = (Main.tileStone[tile12.type] ? 1 : ((!TileID.Sets.Platforms[tile12.type]) ? tile12.type : num));
																					}
																					if (tile13 != null && tile13.active()) {
																						num11 = (Main.tileStone[tile13.type] ? 1 : ((!TileID.Sets.Platforms[tile13.type]) ? tile13.type : num));
																					}
																					if (num11 >= 0 && !Main.tileSolid[num11]) {
																						num11 = -1;
																					}
																					if (num12 >= 0 && !Main.tileSolid[num12]) {
																						num12 = -1;
																					}
																					if (num12 == num && tile12.halfBrick() != tile.halfBrick()) {
																						num12 = -1;
																					}
																					if (num11 == num && tile13.halfBrick() != tile.halfBrick()) {
																						num11 = -1;
																					}
																					if (num12 != -1 && num12 != num && tile.halfBrick()) {
																						num12 = -1;
																					}
																					if (num11 != -1 && num11 != num && tile.halfBrick()) {
																						num11 = -1;
																					}
																					if (num12 == -1 && tile16.active() && tile16.type == num && tile16.slope() == 1) {
																						num12 = num;
																					}
																					if (num11 == -1 && tile17.active() && tile17.type == num && tile17.slope() == 2) {
																						num11 = num;
																					}
																					if (num12 == num && tile12.slope() == 2 && num11 != num) {
																						num11 = -1;
																					}
																					if (num11 == num && tile13.slope() == 1 && num12 != num) {
																						num12 = -1;
																					}
																					if (tile.slope() == 1) {
																						if (TileID.Sets.Platforms[tile13.type] && tile13.slope() == 0) {
																							rectangle.X = 468;
																						}
																						else if (!tile15.active() && (!TileID.Sets.Platforms[tile15.type] || tile15.slope() == 2)) {
																							if (!tile12.active() && (!TileID.Sets.Platforms[tile16.type] || tile16.slope() != 1)) {
																								rectangle.X = 432;
																							}
																							else {
																								rectangle.X = 360;
																							}
																						}
																						else if (!tile12.active() && (!TileID.Sets.Platforms[tile16.type] || tile16.slope() != 1)) {
																							rectangle.X = 396;
																						}
																						else {
																							rectangle.X = 180;
																						}
																					}
																					else if (tile.slope() == 2) {
																						if (TileID.Sets.Platforms[tile12.type] && tile12.slope() == 0) {
																							rectangle.X = 450;
																						}
																						else if (!tile14.active() && (!TileID.Sets.Platforms[tile14.type] || tile14.slope() == 1)) {
																							if (!tile13.active() && (!TileID.Sets.Platforms[tile17.type] || tile17.slope() != 2)) {
																								rectangle.X = 414;
																							}
																							else {
																								rectangle.X = 342;
																							}
																						}
																						else if (!tile13.active() && (!TileID.Sets.Platforms[tile17.type] || tile17.slope() != 2)) {
																							rectangle.X = 378;
																						}
																						else {
																							rectangle.X = 144;
																						}
																					}
																					else if (num12 == num && num11 == num) {
																						if (tile12.slope() == 2 && tile13.slope() == 1) {
																							rectangle.X = 252;
																						}
																						else if (tile12.slope() == 2) {
																							rectangle.X = 216;
																						}
																						else if (tile13.slope() == 1) {
																							rectangle.X = 234;
																						}
																						else {
																							rectangle.X = 0;
																						}
																					}
																					else if (num12 == num && num11 == -1) {
																						if (tile12.slope() == 2) {
																							rectangle.X = 270;
																						}
																						else {
																							rectangle.X = 18;
																						}
																					}
																					else if (num12 == -1 && num11 == num) {
																						if (tile13.slope() == 1) {
																							rectangle.X = 288;
																						}
																						else {
																							rectangle.X = 36;
																						}
																					}
																					else if (num12 != num && num11 == num) {
																						rectangle.X = 54;
																					}
																					else if (num12 == num && num11 != num) {
																						rectangle.X = 72;
																					}
																					else if (num12 != num && num12 != -1 && num11 == -1) {
																						rectangle.X = 108;
																					}
																					else if (num12 == -1 && num11 != num && num11 != -1) {
																						rectangle.X = 126;
																					}
																					else {
																						rectangle.X = 90;
																					}
																					tile.frameX = (short)rectangle.X;
																				}
																			}
																		}
																	}
																}
															}
														}
														else {
															switch (num) {
																case 233:
																case 236:
																case 238:
																	WorldGen.CheckJunglePlant(i, j, num);
																	return;
																case 240:
																case 440:
																	WorldGen.Check3x3Wall(i, j);
																	return;
																case 245:
																	WorldGen.Check2x3Wall(i, j);
																	return;
																case 246:
																	WorldGen.Check3x2Wall(i, j);
																	return;
																case 241:
																	WorldGen.Check4x3Wall(i, j);
																	return;
																case 242:
																	WorldGen.Check6x4Wall(i, j);
																	return;
																case 464:
																case 466:
																	WorldGen.Check5x4(i, j, num);
																	return;
																case 334:
																	WorldGen.CheckWeaponsRack(i, j);
																	return;
																case 34:
																case 454:
																	WorldGen.CheckChand(i, j, num);
																	return;
																default:
																	if (num != 354 && num != 406 && num != 412 && num != 355 && num != 452 && num != 455) {
																		if (num != 15 && num != 20 && !TileLoader.IsSapling(num)) {
																			switch (num) {
																				case 216:
																				case 338:
																				case 390:
																					break;
																				default:
																					if (num < 391 || num > 394) {
																						switch (num) {
																							case 36:
																							case 135:
																							case 141:
																							case 144:
																							case 210:
																							case 239:
																							case 428:
																								WorldGen.Check1x1(i, j, num);
																								return;
																							case 419:
																							case 420:
																							case 423:
																							case 424:
																							case 429:
																							case 445:
																								WorldGen.CheckLogicTiles(i, j, num);
																								return;
																							case 16:
																							case 18:
																							case 29:
																							case 103:
																							case 134:
																							case 462:
																								WorldGen.Check2x1(i, j, (ushort)num);
																								return;
																							case 13:
																							case 33:
																							case 50:
																							case 78:
																							case 174:
																							case 372:
																								WorldGen.CheckOnTable1x1(i, j, (byte)num);
																								return;
																							default:
																								if (num < TileNames.OriginalTileCount && TileID.Sets.BasicChest[num]) {
																									WorldGen.CheckChest(i, j, num);
																								}
																								else {
																									switch (num) {
																										case 128:
																											WorldGen.CheckMan(i, j);
																											return;
																										case 269:
																											WorldGen.CheckWoman(i, j);
																											return;
																										case 27:
																											WorldGen.CheckSunflower(i, j);
																											return;
																										case 28:
																											WorldGen.CheckPot(i, j);
																											return;
																										case 171:
																											WorldGen.CheckXmasTree(i, j);
																											return;
																										default:
																											if (!TileID.Sets.BasicChestFake[num] && num != 457) {
																												switch (num) {
																													case 335:
																													case 411:
																														WorldGen.Check2x2(i, j, num);
																														return;
																													default:
																														if (num < 316 || num > 318) {
																															switch (num) {
																																case 376:
																																case 443:
																																case 444:
																																	WorldGen.CheckSuper(i, j, num);
																																	return;
																																case 91:
																																	WorldGen.CheckBanner(i, j, (byte)num);
																																	return;
																																case 35:
																																case 139:
																																	WorldGen.CheckMB(i, j, (byte)num);
																																	return;
																																case 386:
																																case 387:
																																	WorldGen.CheckTrapDoor(i, j, num);
																																	return;
																																case 388:
																																case 389:
																																	WorldGen.CheckTallGate(i, j, num);
																																	return;
																																case 92:
																																case 93:
																																case 453:
																																	WorldGen.Check1xX(i, j, (short)num);
																																	return;
																																case 104:
																																case 105:
																																case 207:
																																case 320:
																																case 337:
																																case 349:
																																case 356:
																																case 378:
																																case 410:
																																case 456:
																																case 465:
																																	WorldGen.Check2xX(i, j, (ushort)num);
																																	return;
																																case 101:
																																case 102:
																																case 463:
																																	WorldGen.Check3x4(i, j, num);
																																	return;
																																case 42:
																																case 270:
																																case 271:
																																	WorldGen.Check1x2Top(i, j, (ushort)num);
																																	return;
																																case 55:
																																case 85:
																																case 395:
																																case 425:
																																	WorldGen.CheckSign(i, j, (ushort)num);
																																	return;
																																case 209:
																																	WorldGen.CheckCannon(i, j, num);
																																	return;
																																case 79:
																																case 90:
																																	WorldGen.Check4x2(i, j, num);
																																	return;
																																case 94:
																																case 95:
																																case 97:
																																case 98:
																																case 99:
																																case 100:
																																case 125:
																																case 126:
																																case 173:
																																case 282:
																																case 287:
																																case 319:
																																	WorldGen.Check2x2(i, j, num);
																																	return;
																																case 96:
																																	WorldGen.Check2x2Style(i, j, num);
																																	return;
																																case 81: {
																																		Tile tile20 = Main.tile[i, j - 1];
																																		Tile tile21 = Main.tile[i, j + 1];
																																		_ = Main.tile[i - 1, j];
																																		_ = Main.tile[i + 1, j];
																																		int num15 = -1;
																																		int num16 = -1;
																																		if (tile20 != null && tile20.active()) {
																																			num16 = tile20.type;
																																		}
																																		if (tile21 != null && tile21.active()) {
																																			num15 = tile21.type;
																																		}
																																		if (num16 != -1) {
																																			KillTile(i, j, ref undo);
																																		}
																																		else if (num15 < 0 || !Main.tileSolid[num15] || tile21.halfBrick() || tile21.topSlope()) {
																																			KillTile(i, j, ref undo);
																																		}
																																		return;
																																	}
																																default:
																																	if (Main.tileAlch[num]) {
																																		WorldGen.CheckAlch(i, j);
																																	}
																																	else {
																																		switch (num) {
																																			case 72: {
																																					Tile tile18 = Main.tile[i, j - 1];
																																					Tile tile19 = Main.tile[i, j + 1];
																																					int num13 = -1;
																																					int num14 = -1;
																																					if (tile18 != null && tile18.active()) {
																																						num14 = tile18.type;
																																					}
																																					if (tile19 != null && tile19.active()) {
																																						num13 = tile19.type;
																																					}
																																					if (num13 != num && num13 != 70) {
																																						KillTile(i, j, ref undo);
																																					}
																																					else if (num14 != num && tile.frameX == 0) {
																																						tile.frameNumber((byte)WorldGen.genRand.Next(3));
																																						if (tile.frameNumber() == 0) {
																																							tile.frameX = 18;
																																							tile.frameY = 0;
																																						}
																																						if (tile.frameNumber() == 1) {
																																							tile.frameX = 18;
																																							tile.frameY = 18;
																																						}
																																						if (tile.frameNumber() == 2) {
																																							tile.frameX = 18;
																																							tile.frameY = 36;
																																						}
																																					}
																																					break;
																																				}
																																			case 5:
																																				WorldGen.CheckTree(i, j);
																																				break;
																																			case 323:
																																				WorldGen.CheckPalmTree(i, j);
																																				break;
																																		}
																																		TileLoader.CheckModTile(i, j, num);
																																	}
																																	return;
																																case 172:
																																case 360:
																																	break;
																															}
																														}
																														break;
																													case 132:
																													case 138:
																													case 142:
																													case 143:
																													case 288:
																													case 289:
																													case 290:
																													case 291:
																													case 292:
																													case 293:
																													case 294:
																													case 295:
																														break;
																												}
																												WorldGen.Check2x2(i, j, num);
																												return;
																											}
																											break;
																										case 254:
																											break;
																									}
																									WorldGen.Check2x2Style(i, j, num);
																								}
																								return;
																							case 405:
																								break;
																						}
																					}
																					goto case 14;
																				case 14:
																				case 17:
																				case 26:
																				case 77:
																				case 86:
																				case 87:
																				case 88:
																				case 89:
																				case 114:
																				case 133:
																				case 186:
																				case 187:
																				case 215:
																				case 217:
																				case 218:
																				case 237:
																				case 244:
																				case 285:
																				case 286:
																				case 298:
																				case 299:
																				case 310:
																				case 339:
																				case 361:
																				case 362:
																				case 363:
																				case 364:
																				case 377:
																				case 469:
																					WorldGen.Check3x2(i, j, (ushort)num);
																					return;
																			}
																		}
																		WorldGen.Check1x2(i, j, (ushort)num);
																		return;
																	}
																	break;
																case 106:
																case 212:
																case 219:
																case 220:
																case 228:
																case 231:
																case 243:
																case 247:
																case 283:
																case 300:
																case 301:
																case 302:
																case 303:
																case 304:
																case 305:
																case 306:
																case 307:
																case 308:
																	break;
															}
															WorldGen.Check3x3(i, j, (ushort)num);
														}
														return;
													}
													break;
												case 275:
												case 276:
												case 277:
												case 278:
												case 279:
												case 280:
												case 281:
													break;
											}
											WorldGen.Check6x3(i, j, num);
											return;
										}
										break;
									case 373:
									case 374:
									case 375:
										break;
								}

								Tile tile51 = Main.tile[i, j - 1];

								if (tile51 == null || !tile51.active() || tile51.bottomSlope() || !Main.tileSolid[tile51.type] || Main.tileSolidTop[tile51.type])
									KillTile(i, j, ref undo);
								
							}
							else {

								Tile tile52 = Main.tile[i, j - 1];
								Tile tile53 = Main.tile[i, j + 1];
								Tile tile54 = Main.tile[i - 1, j];
								Tile tile55 = Main.tile[i + 1, j];
								Tile tile56 = Main.tile[i - 1, j + 1];
								Tile tile57 = Main.tile[i + 1, j + 1];
								Tile tile58 = Main.tile[i - 1, j - 1];
								Tile tile59 = Main.tile[i + 1, j - 1];
								short num47 = 0;

								if (tile.frameX >= 66)
									num47 = 66;
								
								int num48 = -1;
								int num49 = -1;
								int num50 = -1;
								int num51 = -1;
								int num52 = -1;
								int num53 = -1;
								int num54 = -1;

								if (tile52 != null && tile52.active() && !tile52.bottomSlope()) {
									_ = tile52.type;
								}
								if (tile53 != null && tile53.active() && !tile53.halfBrick() && !tile53.topSlope()) {
									num48 = tile53.type;
								}
								if (tile54 != null && tile54.active() && (tile54.slope() == 0 || (int)tile54.slope() % 2 != 1)) {
									num49 = tile54.type;
								}
								if (tile55 != null && tile55.active() && (tile55.slope() == 0 || (int)tile55.slope() % 2 != 0)) {
									num50 = tile55.type;
								}
								if (tile56 != null && tile56.active()) {
									num51 = tile56.type;
								}
								if (tile57 != null && tile57.active()) {
									num52 = tile57.type;
								}
								if (tile58 != null && tile58.active()) {
									num53 = tile58.type;
								}
								if (tile59 != null && tile59.active()) {
									num54 = tile59.type;
								}
								if (num48 >= 0 && Main.tileSolid[num48] && (!Main.tileNoAttach[num48] || TileID.Sets.Platforms[num48])) {
									tile.frameX = num47;
								}
								else if ((num49 >= 0 && Main.tileSolid[num49] && !Main.tileNoAttach[num49]) || num49 == 124 || (num49 == 5 && num53 == 5 && num51 == 5)) {
									tile.frameX = (short)(22 + num47);
								}
								else if ((num50 >= 0 && Main.tileSolid[num50] && !Main.tileNoAttach[num50]) || num50 == 124 || (num50 == 5 && num54 == 5 && num52 == 5)) {
									tile.frameX = (short)(44 + num47);
								}
								else if (tile.wall > 0) {
									tile.frameX = num47;
								}
								else {
									KillTile(i, j, ref undo);
								}
							}
							return;
						}

						if ((num >= 255 && num <= 268) || num == 385 || (uint)(num - 446) <= 2u) {
							Framing.SelfFrame8Way(i, j, tile, resetFrame);
							return;
						}

						Tile tile60 = Main.tile[i, j - 1];
						Tile tile61 = Main.tile[i, j + 1];
						Tile tile62 = Main.tile[i - 1, j];
						Tile tile63 = Main.tile[i + 1, j];
						Tile tile64 = Main.tile[i - 1, j + 1];
						Tile tile65 = Main.tile[i + 1, j + 1];
						Tile tile66 = Main.tile[i - 1, j - 1];
						Tile tile67 = Main.tile[i + 1, j - 1];

						int upLeft = -1;
						int up = -1;
						int upRight = -1;
						int left = -1;
						int right = -1;
						int downLeft = -1;
						int down = -1;
						int downRight = -1;

						if (tile62 != null && tile62.active()) {
							left = (Main.tileStone[tile62.type] ? 1 : tile62.type);
							if (tile62.slope() == 1 || tile62.slope() == 3) {
								left = -1;
							}
						}
						if (tile63 != null && tile63.active()) {
							right = (Main.tileStone[tile63.type] ? 1 : tile63.type);
							if (tile63.slope() == 2 || tile63.slope() == 4) {
								right = -1;
							}
						}
						if (tile60 != null && tile60.active()) {
							up = (Main.tileStone[tile60.type] ? 1 : tile60.type);
							if (tile60.slope() == 3 || tile60.slope() == 4) {
								up = -1;
							}
						}
						if (tile61 != null && tile61.active()) {
							down = (Main.tileStone[tile61.type] ? 1 : tile61.type);
							if (tile61.slope() == 1 || tile61.slope() == 2) {
								down = -1;
							}
						}
						if (tile66 != null && tile66.active()) {
							upLeft = (Main.tileStone[tile66.type] ? 1 : tile66.type);
						}
						if (tile67 != null && tile67.active()) {
							upRight = (Main.tileStone[tile67.type] ? 1 : tile67.type);
						}
						if (tile64 != null && tile64.active()) {
							downLeft = (Main.tileStone[tile64.type] ? 1 : tile64.type);
						}
						if (tile65 != null && tile65.active()) {
							downRight = (Main.tileStone[tile65.type] ? 1 : tile65.type);
						}
						if (tile.slope() == 2) {
							up = -1;
							left = -1;
						}
						if (tile.slope() == 1) {
							up = -1;
							right = -1;
						}
						if (tile.slope() == 4) {
							down = -1;
							left = -1;
						}
						if (tile.slope() == 3) {
							down = -1;
							right = -1;
						}

						if (TileID.Sets.Snow[num]) {
							WorldGen.TileMergeAttempt(num, Main.tileBrick, TileID.Sets.Ices, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						}
						else if (!TileID.Sets.Ices[num]) {
							if (num == 162) {
								WorldGen.TileMergeAttempt(num, Main.tileBrick, TileID.Sets.IcesSnow, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							}
							else if (Main.tileBrick[num]) {
								if (TileID.Sets.GrassSpecial[num]) {
									WorldGen.TileMergeAttempt(num, Main.tileBrick, TileID.Sets.Mud, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								}
								else {
									WorldGen.TileMergeAttempt(num, Main.tileBrick, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								}
							}
							else if (Main.tilePile[num]) {
								WorldGen.TileMergeAttempt(num, Main.tilePile, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							}
						}
						else {
							WorldGen.TileMergeAttempt(num, Main.tileBrick, TileID.Sets.Snow, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						}

						if ((TileID.Sets.Stone[num] || Main.tileMoss[num]) && down == 165) {
							if (tile61.frameY == 72) {
								down = num;
							}
							else if (tile61.frameY == 0) {
								down = num;
							}
						}

						if ((TileID.Sets.Stone[num] || Main.tileMoss[num]) && up == 165) {
							if (tile60.frameY == 90) {
								up = num;
							}
							else if (tile60.frameY == 54) {
								up = num;
							}
						}

						if (num == 225) {
							if (down == 165) {
								down = num;
							}
							if (up == 165) {
								up = num;
							}
						}

						if (TileID.Sets.Ices[num] && down == 165) {
							down = num;
						}
						if ((tile.slope() == 1 || tile.slope() == 2) && down > -1 && !TileID.Sets.Platforms[down]) {
							down = num;
						}
						if (up > -1 && (tile60.slope() == 1 || tile60.slope() == 2) && !TileID.Sets.Platforms[up]) {
							up = num;
						}
						if ((tile.slope() == 3 || tile.slope() == 4) && up > -1 && !TileID.Sets.Platforms[up]) {
							up = num;
						}
						if (down > -1 && (tile61.slope() == 3 || tile61.slope() == 4) && !TileID.Sets.Platforms[down]) {
							down = num;
						}
						if (num == 124) {
							if (up > -1 && Main.tileSolid[up]) {
								up = num;
							}
							if (down > -1 && Main.tileSolid[down]) {
								down = num;
							}
						}

						if (up > -1 && tile60.halfBrick() && !TileID.Sets.Platforms[up])
							up = num;
						
						if (left > -1 && tile62.halfBrick()) {
							if (tile.halfBrick()) {
								left = num;
							}
							else if (tile62.type != num) {
								left = -1;
							}
						}

						if (right > -1 && tile63.halfBrick()) {
							if (tile.halfBrick()) {
								right = num;
							}
							else if (tile63.type != num) {
								right = -1;
							}
						}

						if (tile.halfBrick()) {
							if (left != num) {
								left = -1;
							}
							if (right != num) {
								right = -1;
							}
							up = -1;
						}

						if (tile61 != null && tile61.halfBrick())
							down = -1;
						
						if (!Main.tileSolid[num]) {
							switch (num) {
								case 49:
									WorldGen.CheckOnTable1x1(i, j, (byte)num);
									return;
								case 80:
									WorldGen.CactusFrame(i, j);
									return;
							}
						}

						mergeUp = false;
						mergeDown = false;
						mergeLeft = false;
						mergeRight = false;

						int num55 = 0;

						if (resetFrame) {
							num55 = WorldGen.genRand.Next(0, 3);
							tile.frameNumber((byte)num55);
						} else {
							num55 = tile.frameNumber();
						}

						if (Main.tileLargeFrames[num] == 1) {
							int num56 = j % 4;
							int num57 = i % 3;
							num55 = (new int[4, 3]
							{
								{
									2,
									4,
									2
								},
								{
									1,
									3,
									1
								},
								{
									2,
									2,
									4
								},
								{
									1,
									1,
									3
								}
							})[num56, num57] - 1;
						}

						WorldGen.TileMergeAttempt(num, Main.tileBlendAll, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						
						if (Main.tileBlendAll[num])
							WorldGen.TileMergeAttempt(num, Main.tileSolid, Main.tileSolidTop, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						
						switch (num) {
							case 0:
								if (up > -1 && Main.tileMergeDirt[up]) {
									TileFrame(i, j - 1, ref undo);
									if (mergeDown) {
										up = num;
									}
								}
								else if (up > -1 && TileID.Sets.Snow[up]) {
									TileFrame(i, j - 1, ref undo);
									if (mergeDown) {
										up = num;
									}
								}
								if (down > -1 && Main.tileMergeDirt[down]) {
									TileFrame(i, j + 1, ref undo);
									if (mergeUp) {
										down = num;
									}
								}
								else if (down > -1 && TileID.Sets.Snow[down]) {
									TileFrame(i, j + 1, ref undo);
									if (mergeUp) {
										down = num;
									}
								}
								if (left > -1 && Main.tileMergeDirt[left]) {
									TileFrame(i - 1, j, ref undo);
									if (mergeRight) {
										left = num;
									}
								}
								else if (left > -1 && TileID.Sets.Snow[left]) {
									TileFrame(i - 1, j, ref undo);
									if (mergeRight) {
										left = num;
									}
								}
								if (right > -1 && Main.tileMergeDirt[right]) {
									TileFrame(i + 1, j, ref undo);
									if (mergeLeft) {
										right = num;
									}
								}
								else if (right > -1 && TileID.Sets.Snow[right]) {
									TileFrame(i + 1, j, ref undo);
									if (mergeLeft) {
										right = num;
									}
								}
								if (up > -1 && TileID.Sets.Grass[up]) {
									up = num;
								}
								if (down > -1 && TileID.Sets.Grass[down]) {
									down = num;
								}
								if (left > -1 && TileID.Sets.Grass[left]) {
									left = num;
								}
								if (right > -1 && TileID.Sets.Grass[right]) {
									right = num;
								}
								if (upLeft > -1 && Main.tileMergeDirt[upLeft]) {
									upLeft = num;
								}
								else if (upLeft > -1 && TileID.Sets.Grass[upLeft]) {
									upLeft = num;
								}
								if (upRight > -1 && Main.tileMergeDirt[upRight]) {
									upRight = num;
								}
								else if (upRight > -1 && TileID.Sets.Grass[upRight]) {
									upRight = num;
								}
								if (downLeft > -1 && Main.tileMergeDirt[downLeft]) {
									downLeft = num;
								}
								else if (downLeft > -1 && TileID.Sets.Grass[downLeft]) {
									downLeft = num;
								}
								if (downRight > -1 && Main.tileMergeDirt[downRight]) {
									downRight = num;
								}
								else if (downRight > -1 && TileID.Sets.Grass[downRight]) {
									downRight = num;
								}
								WorldGen.TileMergeAttempt(-2, 59, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttempt(num, 191, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								break;
							case 213:
								if (up > -1 && Main.tileSolid[up] && !Main.tileSolidTop[up]) {
									up = num;
								}
								if (down > -1 && Main.tileSolid[down]) {
									down = num;
								}
								if (up != num) {
									if (left > -1 && Main.tileSolid[left]) {
										left = num;
									}
									if (right > -1 && Main.tileSolid[right]) {
										right = num;
									}
								}
								break;
							case 53:
								WorldGen.TileMergeAttemptFrametest(i, j, num, 397, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttemptFrametest(i, j, num, 396, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								break;
							case 234:
								WorldGen.TileMergeAttemptFrametest(i, j, num, 399, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttemptFrametest(i, j, num, 401, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								break;
							case 112:
								WorldGen.TileMergeAttemptFrametest(i, j, num, 398, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttemptFrametest(i, j, num, 400, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								break;
							case 116:
								WorldGen.TileMergeAttemptFrametest(i, j, num, 402, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttemptFrametest(i, j, num, 403, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								break;
						}
						if (Main.tileMergeDirt[num]) {
							WorldGen.TileMergeAttempt(-2, 0, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							if (num == 1) {
								if ((double)j > Main.rockLayer) {
									WorldGen.TileMergeAttemptFrametest(i, j, num, 59, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								}
								WorldGen.TileMergeAttemptFrametest(i, j, num, 57, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							}
						}
						else if (!TileID.Sets.HellSpecial[num]) {
							if (num == 57) {
								WorldGen.TileMergeAttempt(-2, 1, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttemptFrametest(i, j, num, TileID.Sets.HellSpecial, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							}
							else if (!TileID.Sets.Mud[num]) {
								switch (num) {
									case 211:
										WorldGen.TileMergeAttempt(59, 60, ref up, ref down, ref left, ref right);
										WorldGen.TileMergeAttempt(-2, 59, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										break;
									case 225:
									case 226:
										WorldGen.TileMergeAttempt(-2, 59, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										break;
									case 60:
										WorldGen.TileMergeAttempt(59, 211, ref up, ref down, ref left, ref right);
										break;
									case 189:
										WorldGen.TileMergeAttemptFrametest(i, j, num, 196, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										break;
									case 196:
										WorldGen.TileMergeAttempt(-2, 189, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										break;
									default:
										if (TileID.Sets.Snow[num]) {
											WorldGen.TileMergeAttemptFrametest(i, j, num, TileID.Sets.IcesSlush, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										}
										else if (!TileID.Sets.IcesSlush[num]) {
											switch (num) {
												case 162:
													WorldGen.TileMergeAttempt(-2, TileID.Sets.IcesSnow, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 32:
													if (down == 23) {
														down = num;
													}
													break;
												case 352:
													if (down == 199) {
														down = num;
													}
													break;
												case 69:
													if (down == 60) {
														down = num;
													}
													break;
												case 51:
													WorldGen.TileMergeAttempt(num, TileID.Sets.AllTiles, Main.tileNoAttach, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 192:
													WorldGen.TileMergeAttemptFrametest(i, j, num, 191, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 191:
													WorldGen.TileMergeAttempt(-2, 192, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttempt(num, 0, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 384:
													WorldGen.TileMergeAttemptFrametest(i, j, num, 383, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 383:
													WorldGen.TileMergeAttempt(-2, 384, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttempt(num, 59, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 407:
													WorldGen.TileMergeAttempt(-2, 404, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 404:
													WorldGen.TileMergeAttempt(-2, 396, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttemptFrametest(i, j, num, 407, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 397:
													WorldGen.TileMergeAttempt(-2, 53, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttemptFrametest(i, j, num, 396, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 396:
													WorldGen.TileMergeAttempt(-2, 397, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttempt(-2, 53, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttemptFrametest(i, j, num, 404, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 398:
													WorldGen.TileMergeAttempt(-2, 112, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttemptFrametest(i, j, num, 400, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 400:
													WorldGen.TileMergeAttempt(-2, 398, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttempt(-2, 112, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 399:
													WorldGen.TileMergeAttempt(-2, 234, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttemptFrametest(i, j, num, 401, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 401:
													WorldGen.TileMergeAttempt(-2, 399, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttempt(-2, 234, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 402:
													WorldGen.TileMergeAttempt(-2, 116, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttemptFrametest(i, j, num, 403, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
												case 403:
													WorldGen.TileMergeAttempt(-2, 402, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													WorldGen.TileMergeAttempt(-2, 116, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
													break;
											}
										}
										else {
											WorldGen.TileMergeAttempt(-2, 147, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										}
										break;
								}
							}
							else {

								if (j > Main.rockLayer)
									WorldGen.TileMergeAttempt(-2, 1, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								
								WorldGen.TileMergeAttempt(num, TileID.Sets.GrassSpecial, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								WorldGen.TileMergeAttemptFrametest(i, j, num, TileID.Sets.JungleSpecial, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								
								if (j < Main.rockLayer) {
									WorldGen.TileMergeAttemptFrametest(i, j, num, 0, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
								} else {
									WorldGen.TileMergeAttempt(num, 0, ref up, ref down, ref left, ref right);
								}

							}

						}
						else {
							WorldGen.TileMergeAttempt(-2, 57, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						}

						if (Main.tileStone[num] || num == 1) {
							WorldGen.TileMergeAttempt(num, Main.tileMoss, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						}

						bool flag8 = false;

						if (up == -2 && tile.color() != tile60.color()) {
							up = num;
							mergeUp = true;
						}

						if (down == -2 && tile.color() != tile61.color()) {
							down = num;
							mergeDown = true;
						}

						if (left == -2 && tile.color() != tile62.color()) {
							left = num;
							mergeLeft = true;
						}

						if (right == -2 && tile.color() != tile63.color()) {
							right = num;
							mergeRight = true;
						}

						if (TileID.Sets.Grass[num] || TileID.Sets.GrassSpecial[num] || Main.tileMoss[num] || TileID.Sets.NeedsGrassFraming[num]) {

							flag8 = true;
							
							WorldGen.TileMergeAttemptWeird(num, -1, Main.tileSolid, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							
							int num58 = TileID.Sets.NeedsGrassFramingDirt[num];
							
							if (TileID.Sets.GrassSpecial[num]) {
								num58 = 59;
							}
							else if (Main.tileMoss[num]) {
								num58 = 1;
							}
							else {
								switch (num) {
									case 2:
										WorldGen.TileMergeAttempt(num58, 23, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										break;
									case 23:
										WorldGen.TileMergeAttempt(num58, 2, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
										break;
								}
							}

							if (up != num && up != num58 && (down == num || down == num58)) {
								if (left == num58 && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 198;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 198;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 198;
											break;
									}
								}
								else if (left == num && right == num58) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 198;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 198;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 198;
											break;
									}
								}
							}
							else if (down != num && down != num58 && (up == num || up == num58)) {
								if (left == num58 && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 216;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 216;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 216;
											break;
									}
								}
								else if (left == num && right == num58) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 216;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 216;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 216;
											break;
									}
								}
							}
							else if (left != num && left != num58 && (right == num || right == num58)) {
								if (up == num58 && down == num) {
									switch (num55) {
										case 0:
											rectangle.X = 72;
											rectangle.Y = 144;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 162;
											break;
										default:
											rectangle.X = 72;
											rectangle.Y = 180;
											break;
									}
								}
								else if (down == num && up == num58) {
									switch (num55) {
										case 0:
											rectangle.X = 72;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 108;
											break;
										default:
											rectangle.X = 72;
											rectangle.Y = 126;
											break;
									}
								}
							}
							else if (right != num && right != num58 && (left == num || left == num58)) {
								if (up == num58 && down == num) {
									switch (num55) {
										case 0:
											rectangle.X = 90;
											rectangle.Y = 144;
											break;
										case 1:
											rectangle.X = 90;
											rectangle.Y = 162;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 180;
											break;
									}
								}
								else if (down == num && right == up) {
									switch (num55) {
										case 0:
											rectangle.X = 90;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 90;
											rectangle.Y = 108;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 126;
											break;
									}
								}
							}
							else if (up == num && down == num && left == num && right == num) {
								if (upLeft != num && upRight != num && downLeft != num && downRight != num) {
									if (downRight == num58) {
										switch (num55) {
											case 0:
												rectangle.X = 108;
												rectangle.Y = 324;
												break;
											case 1:
												rectangle.X = 126;
												rectangle.Y = 324;
												break;
											default:
												rectangle.X = 144;
												rectangle.Y = 324;
												break;
										}
									}
									else if (upRight == num58) {
										switch (num55) {
											case 0:
												rectangle.X = 108;
												rectangle.Y = 342;
												break;
											case 1:
												rectangle.X = 126;
												rectangle.Y = 342;
												break;
											default:
												rectangle.X = 144;
												rectangle.Y = 342;
												break;
										}
									}
									else if (downLeft == num58) {
										switch (num55) {
											case 0:
												rectangle.X = 108;
												rectangle.Y = 360;
												break;
											case 1:
												rectangle.X = 126;
												rectangle.Y = 360;
												break;
											default:
												rectangle.X = 144;
												rectangle.Y = 360;
												break;
										}
									}
									else if (upLeft == num58) {
										switch (num55) {
											case 0:
												rectangle.X = 108;
												rectangle.Y = 378;
												break;
											case 1:
												rectangle.X = 126;
												rectangle.Y = 378;
												break;
											default:
												rectangle.X = 144;
												rectangle.Y = 378;
												break;
										}
									}
									else {
										switch (num55) {
											case 0:
												rectangle.X = 144;
												rectangle.Y = 234;
												break;
											case 1:
												rectangle.X = 198;
												rectangle.Y = 234;
												break;
											default:
												rectangle.X = 252;
												rectangle.Y = 234;
												break;
										}
									}
								}
								else if (upLeft != num && downRight != num) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 306;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 306;
											break;
										default:
											rectangle.X = 72;
											rectangle.Y = 306;
											break;
									}
								}
								else if (upRight != num && downLeft != num) {
									switch (num55) {
										case 0:
											rectangle.X = 90;
											rectangle.Y = 306;
											break;
										case 1:
											rectangle.X = 108;
											rectangle.Y = 306;
											break;
										default:
											rectangle.X = 126;
											rectangle.Y = 306;
											break;
									}
								}
								else if (upLeft != num && upRight == num && downLeft == num && downRight == num) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 180;
											break;
									}
								}
								else if (upLeft == num && upRight != num && downLeft == num && downRight == num) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 180;
											break;
									}
								}
								else if (upLeft == num && upRight == num && downLeft != num && downRight == num) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 162;
											break;
									}
								}
								else if (upLeft == num && upRight == num && downLeft == num && downRight != num) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 162;
											break;
									}
								}
							}
							else if (up == num && down == num58 && left == num && right == num && upLeft == -1 && upRight == -1) {
								switch (num55) {
									case 0:
										rectangle.X = 108;
										rectangle.Y = 18;
										break;
									case 1:
										rectangle.X = 126;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 144;
										rectangle.Y = 18;
										break;
								}
							}
							else if (up == num58 && down == num && left == num && right == num && downLeft == -1 && downRight == -1) {
								switch (num55) {
									case 0:
										rectangle.X = 108;
										rectangle.Y = 36;
										break;
									case 1:
										rectangle.X = 126;
										rectangle.Y = 36;
										break;
									default:
										rectangle.X = 144;
										rectangle.Y = 36;
										break;
								}
							}
							else if (up == num && down == num && left == num58 && right == num && upRight == -1 && downRight == -1) {
								switch (num55) {
									case 0:
										rectangle.X = 198;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 198;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 198;
										rectangle.Y = 36;
										break;
								}
							}
							else if (up == num && down == num && left == num && right == num58 && upLeft == -1 && downLeft == -1) {
								switch (num55) {
									case 0:
										rectangle.X = 180;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 180;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 180;
										rectangle.Y = 36;
										break;
								}
							}
							else if (up == num && down == num58 && left == num && right == num) {
								if (upRight != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 180;
											break;
									}
								}
								else if (upLeft != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 180;
											break;
									}
								}
							}
							else if (up == num58 && down == num && left == num && right == num) {
								if (downRight != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 162;
											break;
									}
								}
								else if (downLeft != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 162;
											break;
									}
								}
							}
							else if (up == num && down == num && left == num && right == num58) {
								if (upLeft != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 162;
											break;
									}
								}
								else if (downLeft != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 180;
											break;
									}
								}
							}
							else if (up == num && down == num && left == num58 && right == num) {
								if (upRight != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 162;
											break;
									}
								}
								else if (downRight != -1) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 180;
											break;
									}
								}
							}
							else if ((up == num58 && down == num && left == num && right == num) || (up == num && down == num58 && left == num && right == num) || (up == num && down == num && left == num58 && right == num) || (up == num && down == num && left == num && right == num58)) {
								switch (num55) {
									case 0:
										rectangle.X = 18;
										rectangle.Y = 18;
										break;
									case 1:
										rectangle.X = 36;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 54;
										rectangle.Y = 18;
										break;
								}
							}
							if ((up == num || up == num58) && (down == num || down == num58) && (left == num || left == num58) && (right == num || right == num58)) {
								if (upLeft != num && upLeft != num58 && (upRight == num || upRight == num58) && (downLeft == num || downLeft == num58) && (downRight == num || downRight == num58)) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 180;
											break;
									}
								}
								else if (upRight != num && upRight != num58 && (upLeft == num || upLeft == num58) && (downLeft == num || downLeft == num58) && (downRight == num || downRight == num58)) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 180;
											break;
									}
								}
								else if (downLeft != num && downLeft != num58 && (upLeft == num || upLeft == num58) && (upRight == num || upRight == num58) && (downRight == num || downRight == num58)) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 162;
											break;
									}
								}
								else if (downRight != num && downRight != num58 && (upLeft == num || upLeft == num58) && (downLeft == num || downLeft == num58) && (upRight == num || upRight == num58)) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 162;
											break;
									}
								}
							}
							if (up != num58 && up != num && down == num && left != num58 && left != num && right == num && downRight != num58 && downRight != num) {
								switch (num55) {
									case 0:
										rectangle.X = 90;
										rectangle.Y = 270;
										break;
									case 1:
										rectangle.X = 108;
										rectangle.Y = 270;
										break;
									default:
										rectangle.X = 126;
										rectangle.Y = 270;
										break;
								}
							}
							else if (up != num58 && up != num && down == num && left == num && right != num58 && right != num && downLeft != num58 && downLeft != num) {
								switch (num55) {
									case 0:
										rectangle.X = 144;
										rectangle.Y = 270;
										break;
									case 1:
										rectangle.X = 162;
										rectangle.Y = 270;
										break;
									default:
										rectangle.X = 180;
										rectangle.Y = 270;
										break;
								}
							}
							else if (down != num58 && down != num && up == num && left != num58 && left != num && right == num && upRight != num58 && upRight != num) {
								switch (num55) {
									case 0:
										rectangle.X = 90;
										rectangle.Y = 288;
										break;
									case 1:
										rectangle.X = 108;
										rectangle.Y = 288;
										break;
									default:
										rectangle.X = 126;
										rectangle.Y = 288;
										break;
								}
							}
							else if (down != num58 && down != num && up == num && left == num && right != num58 && right != num && upLeft != num58 && upLeft != num) {
								switch (num55) {
									case 0:
										rectangle.X = 144;
										rectangle.Y = 288;
										break;
									case 1:
										rectangle.X = 162;
										rectangle.Y = 288;
										break;
									default:
										rectangle.X = 180;
										rectangle.Y = 288;
										break;
								}
							}
							else if (up != num && up != num58 && down == num && left == num && right == num && downLeft != num && downLeft != num58 && downRight != num && downRight != num58) {
								switch (num55) {
									case 0:
										rectangle.X = 144;
										rectangle.Y = 216;
										break;
									case 1:
										rectangle.X = 198;
										rectangle.Y = 216;
										break;
									default:
										rectangle.X = 252;
										rectangle.Y = 216;
										break;
								}
							}
							else if (down != num && down != num58 && up == num && left == num && right == num && upLeft != num && upLeft != num58 && upRight != num && upRight != num58) {
								switch (num55) {
									case 0:
										rectangle.X = 144;
										rectangle.Y = 252;
										break;
									case 1:
										rectangle.X = 198;
										rectangle.Y = 252;
										break;
									default:
										rectangle.X = 252;
										rectangle.Y = 252;
										break;
								}
							}
							else if (left != num && left != num58 && down == num && up == num && right == num && upRight != num && upRight != num58 && downRight != num && downRight != num58) {
								switch (num55) {
									case 0:
										rectangle.X = 126;
										rectangle.Y = 234;
										break;
									case 1:
										rectangle.X = 180;
										rectangle.Y = 234;
										break;
									default:
										rectangle.X = 234;
										rectangle.Y = 234;
										break;
								}
							}
							else if (right != num && right != num58 && down == num && up == num && left == num && upLeft != num && upLeft != num58 && downLeft != num && downLeft != num58) {
								switch (num55) {
									case 0:
										rectangle.X = 162;
										rectangle.Y = 234;
										break;
									case 1:
										rectangle.X = 216;
										rectangle.Y = 234;
										break;
									default:
										rectangle.X = 270;
										rectangle.Y = 234;
										break;
								}
							}
							else if (up != num58 && up != num && (down == num58 || down == num) && left == num58 && right == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 36;
										rectangle.Y = 270;
										break;
									case 1:
										rectangle.X = 54;
										rectangle.Y = 270;
										break;
									default:
										rectangle.X = 72;
										rectangle.Y = 270;
										break;
								}
							}
							else if (down != num58 && down != num && (up == num58 || up == num) && left == num58 && right == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 36;
										rectangle.Y = 288;
										break;
									case 1:
										rectangle.X = 54;
										rectangle.Y = 288;
										break;
									default:
										rectangle.X = 72;
										rectangle.Y = 288;
										break;
								}
							}
							else if (left != num58 && left != num && (right == num58 || right == num) && up == num58 && down == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 0;
										rectangle.Y = 270;
										break;
									case 1:
										rectangle.X = 0;
										rectangle.Y = 288;
										break;
									default:
										rectangle.X = 0;
										rectangle.Y = 306;
										break;
								}
							}
							else if (right != num58 && right != num && (left == num58 || left == num) && up == num58 && down == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 18;
										rectangle.Y = 270;
										break;
									case 1:
										rectangle.X = 18;
										rectangle.Y = 288;
										break;
									default:
										rectangle.X = 18;
										rectangle.Y = 306;
										break;
								}
							}
							else if (up == num && down == num58 && left == num58 && right == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 198;
										rectangle.Y = 288;
										break;
									case 1:
										rectangle.X = 216;
										rectangle.Y = 288;
										break;
									default:
										rectangle.X = 234;
										rectangle.Y = 288;
										break;
								}
							}
							else if (up == num58 && down == num && left == num58 && right == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 198;
										rectangle.Y = 270;
										break;
									case 1:
										rectangle.X = 216;
										rectangle.Y = 270;
										break;
									default:
										rectangle.X = 234;
										rectangle.Y = 270;
										break;
								}
							}
							else if (up == num58 && down == num58 && left == num && right == num58) {
								switch (num55) {
									case 0:
										rectangle.X = 198;
										rectangle.Y = 306;
										break;
									case 1:
										rectangle.X = 216;
										rectangle.Y = 306;
										break;
									default:
										rectangle.X = 234;
										rectangle.Y = 306;
										break;
								}
							}
							else if (up == num58 && down == num58 && left == num58 && right == num) {
								switch (num55) {
									case 0:
										rectangle.X = 144;
										rectangle.Y = 306;
										break;
									case 1:
										rectangle.X = 162;
										rectangle.Y = 306;
										break;
									default:
										rectangle.X = 180;
										rectangle.Y = 306;
										break;
								}
							}
							if (up != num && up != num58 && down == num && left == num && right == num) {
								if ((downLeft == num58 || downLeft == num) && downRight != num58 && downRight != num) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 324;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 324;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 324;
											break;
									}
								}
								else if ((downRight == num58 || downRight == num) && downLeft != num58 && downLeft != num) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 324;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 324;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 324;
											break;
									}
								}
							}
							else if (down != num && down != num58 && up == num && left == num && right == num) {
								if ((upLeft == num58 || upLeft == num) && upRight != num58 && upRight != num) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 342;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 342;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 342;
											break;
									}
								}
								else if ((upRight == num58 || upRight == num) && upLeft != num58 && upLeft != num) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 342;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 342;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 342;
											break;
									}
								}
							}
							else if (left != num && left != num58 && up == num && down == num && right == num) {
								if ((upRight == num58 || upRight == num) && downRight != num58 && downRight != num) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 360;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 360;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 360;
											break;
									}
								}
								else if ((downRight == num58 || downRight == num) && upRight != num58 && upRight != num) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 360;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 360;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 360;
											break;
									}
								}
							}
							else if (right != num && right != num58 && up == num && down == num && left == num) {
								if ((upLeft == num58 || upLeft == num) && downLeft != num58 && downLeft != num) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 378;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 378;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 378;
											break;
									}
								}
								else if ((downLeft == num58 || downLeft == num) && upLeft != num58 && upLeft != num) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 378;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 378;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 378;
											break;
									}
								}
							}
							if ((up == num || up == num58) && (down == num || down == num58) && (left == num || left == num58) && (right == num || right == num58) && upLeft != -1 && upRight != -1 && downLeft != -1 && downRight != -1) {
								if ((i + j) % 2 == 1) {
									switch (num55) {
										case 0:
											rectangle.X = 108;
											rectangle.Y = 198;
											break;
										case 1:
											rectangle.X = 126;
											rectangle.Y = 198;
											break;
										default:
											rectangle.X = 144;
											rectangle.Y = 198;
											break;
									}
								}
								else {
									switch (num55) {
										case 0:
											rectangle.X = 18;
											rectangle.Y = 18;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 18;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 18;
											break;
									}

								}

							}

							WorldGen.TileMergeAttempt(-2, num58, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);

						}

						WorldGen.TileMergeAttempt(num, Main.tileMerge[num], ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
						
						if (rectangle.X == -1 && rectangle.Y == -1 && (Main.tileMergeDirt[num] || (num > -1 && TileID.Sets.ChecksForMerge[num]))) {

							if (!flag8) {
								flag8 = true;
								WorldGen.TileMergeAttemptWeird(num, -1, Main.tileSolid, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							}
							if (up > -1 && up != num) {
								up = -1;
							}
							if (down > -1 && down != num) {
								down = -1;
							}
							if (left > -1 && left != num) {
								left = -1;
							}
							if (right > -1 && right != num) {
								right = -1;
							}
							if (up != -1 && down != -1 && left != -1 && right != -1) {
								if (up == -2 && down == num && left == num && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 144;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 162;
											rectangle.Y = 108;
											break;
										default:
											rectangle.X = 180;
											rectangle.Y = 108;
											break;
									}
									mergeUp = true;
								}
								else if (up == num && down == -2 && left == num && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 144;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 162;
											rectangle.Y = 90;
											break;
										default:
											rectangle.X = 180;
											rectangle.Y = 90;
											break;
									}
									mergeDown = true;
								}
								else if (up == num && down == num && left == -2 && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 162;
											rectangle.Y = 126;
											break;
										case 1:
											rectangle.X = 162;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 162;
											rectangle.Y = 162;
											break;
									}
									mergeLeft = true;
								}
								else if (up == num && down == num && left == num && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 144;
											rectangle.Y = 126;
											break;
										case 1:
											rectangle.X = 144;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 144;
											rectangle.Y = 162;
											break;
									}
									mergeRight = true;
								}
								else if (up == -2 && down == num && left == -2 && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 162;
											break;
									}
									mergeUp = true;
									mergeLeft = true;
								}
								else if (up == -2 && down == num && left == num && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 126;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 162;
											break;
									}
									mergeUp = true;
									mergeRight = true;
								}
								else if (up == num && down == -2 && left == -2 && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 36;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 180;
											break;
									}
									mergeDown = true;
									mergeLeft = true;
								}
								else if (up == num && down == -2 && left == num && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 108;
											break;
										case 1:
											rectangle.X = 54;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 180;
											break;
									}
									mergeDown = true;
									mergeRight = true;
								}
								else if (up == num && down == num && left == -2 && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 180;
											rectangle.Y = 126;
											break;
										case 1:
											rectangle.X = 180;
											rectangle.Y = 144;
											break;
										default:
											rectangle.X = 180;
											rectangle.Y = 162;
											break;
									}
									mergeLeft = true;
									mergeRight = true;
								}
								else if (up == -2 && down == -2 && left == num && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 144;
											rectangle.Y = 180;
											break;
										case 1:
											rectangle.X = 162;
											rectangle.Y = 180;
											break;
										default:
											rectangle.X = 180;
											rectangle.Y = 180;
											break;
									}
									mergeUp = true;
									mergeDown = true;
								}
								else if (up == -2 && down == num && left == -2 && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 198;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 198;
											rectangle.Y = 108;
											break;
										default:
											rectangle.X = 198;
											rectangle.Y = 126;
											break;
									}
									mergeUp = true;
									mergeLeft = true;
									mergeRight = true;
								}
								else if (up == num && down == -2 && left == -2 && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 198;
											rectangle.Y = 144;
											break;
										case 1:
											rectangle.X = 198;
											rectangle.Y = 162;
											break;
										default:
											rectangle.X = 198;
											rectangle.Y = 180;
											break;
									}
									mergeDown = true;
									mergeLeft = true;
									mergeRight = true;
								}
								else if (up == -2 && down == -2 && left == num && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 216;
											rectangle.Y = 144;
											break;
										case 1:
											rectangle.X = 216;
											rectangle.Y = 162;
											break;
										default:
											rectangle.X = 216;
											rectangle.Y = 180;
											break;
									}
									mergeUp = true;
									mergeDown = true;
									mergeRight = true;
								}
								else if (up == -2 && down == -2 && left == -2 && right == num) {
									switch (num55) {
										case 0:
											rectangle.X = 216;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 216;
											rectangle.Y = 108;
											break;
										default:
											rectangle.X = 216;
											rectangle.Y = 126;
											break;
									}
									mergeUp = true;
									mergeDown = true;
									mergeLeft = true;
								}
								else if (up == -2 && down == -2 && left == -2 && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 108;
											rectangle.Y = 198;
											break;
										case 1:
											rectangle.X = 126;
											rectangle.Y = 198;
											break;
										default:
											rectangle.X = 144;
											rectangle.Y = 198;
											break;
									}
									mergeUp = true;
									mergeDown = true;
									mergeLeft = true;
									mergeRight = true;
								}
								else if (up == num && down == num && left == num && right == num) {
									if (upLeft == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 18;
												rectangle.Y = 108;
												break;
											case 1:
												rectangle.X = 18;
												rectangle.Y = 144;
												break;
											default:
												rectangle.X = 18;
												rectangle.Y = 180;
												break;
										}
									}
									if (upRight == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 0;
												rectangle.Y = 108;
												break;
											case 1:
												rectangle.X = 0;
												rectangle.Y = 144;
												break;
											default:
												rectangle.X = 0;
												rectangle.Y = 180;
												break;
										}
									}
									if (downLeft == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 18;
												rectangle.Y = 90;
												break;
											case 1:
												rectangle.X = 18;
												rectangle.Y = 126;
												break;
											default:
												rectangle.X = 18;
												rectangle.Y = 162;
												break;
										}
									}
									if (downRight == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 0;
												rectangle.Y = 90;
												break;
											case 1:
												rectangle.X = 0;
												rectangle.Y = 126;
												break;
											default:
												rectangle.X = 0;
												rectangle.Y = 162;
												break;
										}
									}
								}
							}
							else {
								if (!TileID.Sets.Grass[num] && !TileID.Sets.GrassSpecial[num]) {
									if (up == -1 && down == -2 && left == num && right == num) {
										switch (num55) {
											case 0:
												rectangle.X = 234;
												rectangle.Y = 0;
												break;
											case 1:
												rectangle.X = 252;
												rectangle.Y = 0;
												break;
											default:
												rectangle.X = 270;
												rectangle.Y = 0;
												break;
										}
										mergeDown = true;
									}
									else if (up == -2 && down == -1 && left == num && right == num) {
										switch (num55) {
											case 0:
												rectangle.X = 234;
												rectangle.Y = 18;
												break;
											case 1:
												rectangle.X = 252;
												rectangle.Y = 18;
												break;
											default:
												rectangle.X = 270;
												rectangle.Y = 18;
												break;
										}
										mergeUp = true;
									}
									else if (up == num && down == num && left == -1 && right == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 234;
												rectangle.Y = 36;
												break;
											case 1:
												rectangle.X = 252;
												rectangle.Y = 36;
												break;
											default:
												rectangle.X = 270;
												rectangle.Y = 36;
												break;
										}
										mergeRight = true;
									}
									else if (up == num && down == num && left == -2 && right == -1) {
										switch (num55) {
											case 0:
												rectangle.X = 234;
												rectangle.Y = 54;
												break;
											case 1:
												rectangle.X = 252;
												rectangle.Y = 54;
												break;
											default:
												rectangle.X = 270;
												rectangle.Y = 54;
												break;
										}
										mergeLeft = true;
									}
								}
								if (up != -1 && down != -1 && left == -1 && right == num) {
									if (up == -2 && down == num) {
										switch (num55) {
											case 0:
												rectangle.X = 72;
												rectangle.Y = 144;
												break;
											case 1:
												rectangle.X = 72;
												rectangle.Y = 162;
												break;
											default:
												rectangle.X = 72;
												rectangle.Y = 180;
												break;
										}
										mergeUp = true;
									}
									else if (down == -2 && up == num) {
										switch (num55) {
											case 0:
												rectangle.X = 72;
												rectangle.Y = 90;
												break;
											case 1:
												rectangle.X = 72;
												rectangle.Y = 108;
												break;
											default:
												rectangle.X = 72;
												rectangle.Y = 126;
												break;
										}
										mergeDown = true;
									}
								}
								else if (up != -1 && down != -1 && left == num && right == -1) {
									if (up == -2 && down == num) {
										switch (num55) {
											case 0:
												rectangle.X = 90;
												rectangle.Y = 144;
												break;
											case 1:
												rectangle.X = 90;
												rectangle.Y = 162;
												break;
											default:
												rectangle.X = 90;
												rectangle.Y = 180;
												break;
										}
										mergeUp = true;
									}
									else if (down == -2 && up == num) {
										switch (num55) {
											case 0:
												rectangle.X = 90;
												rectangle.Y = 90;
												break;
											case 1:
												rectangle.X = 90;
												rectangle.Y = 108;
												break;
											default:
												rectangle.X = 90;
												rectangle.Y = 126;
												break;
										}
										mergeDown = true;
									}
								}
								else if (up == -1 && down == num && left != -1 && right != -1) {
									if (left == -2 && right == num) {
										switch (num55) {
											case 0:
												rectangle.X = 0;
												rectangle.Y = 198;
												break;
											case 1:
												rectangle.X = 18;
												rectangle.Y = 198;
												break;
											default:
												rectangle.X = 36;
												rectangle.Y = 198;
												break;
										}
										mergeLeft = true;
									}
									else if (right == -2 && left == num) {
										switch (num55) {
											case 0:
												rectangle.X = 54;
												rectangle.Y = 198;
												break;
											case 1:
												rectangle.X = 72;
												rectangle.Y = 198;
												break;
											default:
												rectangle.X = 90;
												rectangle.Y = 198;
												break;
										}
										mergeRight = true;
									}
								}
								else if (up == num && down == -1 && left != -1 && right != -1) {
									if (left == -2 && right == num) {
										switch (num55) {
											case 0:
												rectangle.X = 0;
												rectangle.Y = 216;
												break;
											case 1:
												rectangle.X = 18;
												rectangle.Y = 216;
												break;
											default:
												rectangle.X = 36;
												rectangle.Y = 216;
												break;
										}
										mergeLeft = true;
									}
									else if (right == -2 && left == num) {
										switch (num55) {
											case 0:
												rectangle.X = 54;
												rectangle.Y = 216;
												break;
											case 1:
												rectangle.X = 72;
												rectangle.Y = 216;
												break;
											default:
												rectangle.X = 90;
												rectangle.Y = 216;
												break;
										}
										mergeRight = true;
									}
								}
								else if (up != -1 && down != -1 && left == -1 && right == -1) {
									if (up == -2 && down == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 108;
												rectangle.Y = 216;
												break;
											case 1:
												rectangle.X = 108;
												rectangle.Y = 234;
												break;
											default:
												rectangle.X = 108;
												rectangle.Y = 252;
												break;
										}
										mergeUp = true;
										mergeDown = true;
									}
									else if (up == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 126;
												rectangle.Y = 144;
												break;
											case 1:
												rectangle.X = 126;
												rectangle.Y = 162;
												break;
											default:
												rectangle.X = 126;
												rectangle.Y = 180;
												break;
										}
										mergeUp = true;
									}
									else if (down == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 126;
												rectangle.Y = 90;
												break;
											case 1:
												rectangle.X = 126;
												rectangle.Y = 108;
												break;
											default:
												rectangle.X = 126;
												rectangle.Y = 126;
												break;
										}
										mergeDown = true;
									}
								}
								else if (up == -1 && down == -1 && left != -1 && right != -1) {
									if (left == -2 && right == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 162;
												rectangle.Y = 198;
												break;
											case 1:
												rectangle.X = 180;
												rectangle.Y = 198;
												break;
											default:
												rectangle.X = 198;
												rectangle.Y = 198;
												break;
										}
										mergeLeft = true;
										mergeRight = true;
									}
									else if (left == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 0;
												rectangle.Y = 252;
												break;
											case 1:
												rectangle.X = 18;
												rectangle.Y = 252;
												break;
											default:
												rectangle.X = 36;
												rectangle.Y = 252;
												break;
										}
										mergeLeft = true;
									}
									else if (right == -2) {
										switch (num55) {
											case 0:
												rectangle.X = 54;
												rectangle.Y = 252;
												break;
											case 1:
												rectangle.X = 72;
												rectangle.Y = 252;
												break;
											default:
												rectangle.X = 90;
												rectangle.Y = 252;
												break;
										}
										mergeRight = true;
									}
								}
								else if (up == -2 && down == -1 && left == -1 && right == -1) {
									switch (num55) {
										case 0:
											rectangle.X = 108;
											rectangle.Y = 144;
											break;
										case 1:
											rectangle.X = 108;
											rectangle.Y = 162;
											break;
										default:
											rectangle.X = 108;
											rectangle.Y = 180;
											break;
									}
									mergeUp = true;
								}
								else if (up == -1 && down == -2 && left == -1 && right == -1) {
									switch (num55) {
										case 0:
											rectangle.X = 108;
											rectangle.Y = 90;
											break;
										case 1:
											rectangle.X = 108;
											rectangle.Y = 108;
											break;
										default:
											rectangle.X = 108;
											rectangle.Y = 126;
											break;
									}
									mergeDown = true;
								}
								else if (up == -1 && down == -1 && left == -2 && right == -1) {
									switch (num55) {
										case 0:
											rectangle.X = 0;
											rectangle.Y = 234;
											break;
										case 1:
											rectangle.X = 18;
											rectangle.Y = 234;
											break;
										default:
											rectangle.X = 36;
											rectangle.Y = 234;
											break;
									}
									mergeLeft = true;
								}
								else if (up == -1 && down == -1 && left == -1 && right == -2) {
									switch (num55) {
										case 0:
											rectangle.X = 54;
											rectangle.Y = 234;
											break;
										case 1:
											rectangle.X = 72;
											rectangle.Y = 234;
											break;
										default:
											rectangle.X = 90;
											rectangle.Y = 234;
											break;
									}
									mergeRight = true;
								}
							}
						}

						int num59 = tile.blockType();

						if (TileID.Sets.HasSlopeFrames[num]) {

							if (num59 == 0) {

								bool flag9 = num == up && tile60.topSlope();
								bool flag10 = num == left && tile62.leftSlope();
								bool flag11 = num == right && tile63.rightSlope();
								bool flag12 = num == down && tile61.bottomSlope();
								int num60 = 0;
								int num61 = 0;
								if (flag9.ToInt() + flag10.ToInt() + flag11.ToInt() + flag12.ToInt() > 2) {
									int num62 = (tile60.slope() == 1).ToInt() + (tile63.slope() == 1).ToInt() + (tile61.slope() == 4).ToInt() + (tile62.slope() == 4).ToInt();
									int num63 = (tile60.slope() == 2).ToInt() + (tile63.slope() == 3).ToInt() + (tile61.slope() == 3).ToInt() + (tile62.slope() == 2).ToInt();
									if (num62 == num63) {
										num60 = 2;
										num61 = 4;
									}
									else if (num62 > num63) {
										bool num64 = num == upLeft && tile66.slope() == 0;
										bool flag13 = num == downRight && tile65.slope() == 0;
										if (num64 && flag13) {
											num61 = 4;
										}
										else if (flag13) {
											num60 = 6;
										}
										else {
											num60 = 7;
											num61 = 1;
										}
									}
									else {
										bool num65 = num == upRight && tile67.slope() == 0;
										bool flag14 = num == downLeft && tile64.slope() == 0;
										if (num65 && flag14) {
											num61 = 4;
											num60 = 1;
										}
										else if (flag14) {
											num60 = 7;
										}
										else {
											num60 = 6;
											num61 = 1;
										}
									}
									rectangle.X = (18 + num60) * 18;
									rectangle.Y = num61 * 18;
								}
								else {
									if (flag9 && flag10 && num == down && num == right) {
										num61 = 2;
									}
									else if (flag9 && flag11 && num == down && num == left) {
										num60 = 1;
										num61 = 2;
									}
									else if (flag11 && flag12 && num == up && num == left) {
										num60 = 1;
										num61 = 3;
									}
									else if (flag12 && flag10 && num == up && num == right) {
										num61 = 3;
									}
									if (num60 != 0 || num61 != 0) {
										rectangle.X = (18 + num60) * 18;
										rectangle.Y = num61 * 18;
									}
								}
							}

							if (num59 >= 2 && (rectangle.X < 0 || rectangle.Y < 0)) {

								int num66 = -1;
								int num67 = -1;
								int num68 = -1;
								int num69 = 0;
								int num70 = 0;

								switch (num59) {
									case 2:
										num66 = left;
										num67 = down;
										num68 = downLeft;
										num69++;
										break;
									case 3:
										num66 = right;
										num67 = down;
										num68 = downRight;
										break;
									case 4:
										num66 = left;
										num67 = up;
										num68 = upLeft;
										num69++;
										num70++;
										break;
									case 5:
										num66 = right;
										num67 = up;
										num68 = upRight;
										num70++;
										break;
								}

								if (num != num66 || num != num67 || num != num68) {

									if (num == num66 && num == num67) {
										num69 += 2;
									}
									else if (num == num66) {
										num69 += 4;
									}
									else if (num == num67) {
										num69 += 4;
										num70 += 2;
									}
									else {
										num69 += 2;
										num70 += 2;
									}

								}

								rectangle.X = (18 + num69) * 18;
								rectangle.Y = num70 * 18;

							}

						}

						if (rectangle.X < 0 || rectangle.Y < 0) {

							if (!flag8) {
								flag8 = true;
								WorldGen.TileMergeAttemptWeird(num, -1, Main.tileSolid, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							}

							if (TileID.Sets.Grass[num] || TileID.Sets.GrassSpecial[num] || Main.tileMoss[num])
								WorldGen.TileMergeAttempt(num, -2, ref up, ref down, ref left, ref right, ref upLeft, ref upRight, ref downLeft, ref downRight);
							
							if (up == num && down == num && left == num && right == num) {

								if (upLeft != num && upRight != num) {

									switch (num55) {
										case 0:
											rectangle.X = 108;
											rectangle.Y = 18;
											break;
										case 1:
											rectangle.X = 126;
											rectangle.Y = 18;
											break;
										default:
											rectangle.X = 144;
											rectangle.Y = 18;
											break;
									}
								}
								else if (downLeft != num && downRight != num) {

									switch (num55) {
										case 0:
											rectangle.X = 108;
											rectangle.Y = 36;
											break;
										case 1:
											rectangle.X = 126;
											rectangle.Y = 36;
											break;
										default:
											rectangle.X = 144;
											rectangle.Y = 36;
											break;
									}

								}
								else if (upLeft != num && downLeft != num) {

									switch (num55) {
										case 0:
											rectangle.X = 180;
											rectangle.Y = 0;
											break;
										case 1:
											rectangle.X = 180;
											rectangle.Y = 18;
											break;
										default:
											rectangle.X = 180;
											rectangle.Y = 36;
											break;
									}

								}
								else if (upRight != num && downRight != num) {

									switch (num55) {
										case 0:
											rectangle.X = 198;
											rectangle.Y = 0;
											break;
										case 1:
											rectangle.X = 198;
											rectangle.Y = 18;
											break;
										default:
											rectangle.X = 198;
											rectangle.Y = 36;
											break;
									}

								}
								else {

									switch (num55) {
										case 0:
											rectangle.X = 18;
											rectangle.Y = 18;
											break;
										case 1:
											rectangle.X = 36;
											rectangle.Y = 18;
											break;
										default:
											rectangle.X = 54;
											rectangle.Y = 18;
											break;
									}

								}

							}
							else if (up != num && down == num && left == num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 18;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 36;
										rectangle.Y = 0;
										break;
									default:
										rectangle.X = 54;
										rectangle.Y = 0;
										break;
								}

							}
							else if (up == num && down != num && left == num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 18;
										rectangle.Y = 36;
										break;
									case 1:
										rectangle.X = 36;
										rectangle.Y = 36;
										break;
									default:
										rectangle.X = 54;
										rectangle.Y = 36;
										break;
								}

							}
							else if (up == num && down == num && left != num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 0;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 0;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 0;
										rectangle.Y = 36;
										break;
								}

							}
							else if (up == num && down == num && left == num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 72;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 72;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 72;
										rectangle.Y = 36;
										break;
								}

							}
							else if (up != num && down == num && left != num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 0;
										rectangle.Y = 54;
										break;
									case 1:
										rectangle.X = 36;
										rectangle.Y = 54;
										break;
									default:
										rectangle.X = 72;
										rectangle.Y = 54;
										break;
								}

							}
							else if (up != num && down == num && left == num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 18;
										rectangle.Y = 54;
										break;
									case 1:
										rectangle.X = 54;
										rectangle.Y = 54;
										break;
									default:
										rectangle.X = 90;
										rectangle.Y = 54;
										break;
								}

							}
							else if (up == num && down != num && left != num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 0;
										rectangle.Y = 72;
										break;
									case 1:
										rectangle.X = 36;
										rectangle.Y = 72;
										break;
									default:
										rectangle.X = 72;
										rectangle.Y = 72;
										break;
								}

							}
							else if (up == num && down != num && left == num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 18;
										rectangle.Y = 72;
										break;
									case 1:
										rectangle.X = 54;
										rectangle.Y = 72;
										break;
									default:
										rectangle.X = 90;
										rectangle.Y = 72;
										break;
								}

							}
							else if (up == num && down == num && left != num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 90;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 90;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 90;
										rectangle.Y = 36;
										break;
								}

							}
							else if (up != num && down != num && left == num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 108;
										rectangle.Y = 72;
										break;
									case 1:
										rectangle.X = 126;
										rectangle.Y = 72;
										break;
									default:
										rectangle.X = 144;
										rectangle.Y = 72;
										break;
								}

							}
							else if (up != num && down == num && left != num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 108;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 126;
										rectangle.Y = 0;
										break;
									default:
										rectangle.X = 144;
										rectangle.Y = 0;
										break;
								}

							}
							else if (up == num && down != num && left != num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 108;
										rectangle.Y = 54;
										break;
									case 1:
										rectangle.X = 126;
										rectangle.Y = 54;
										break;
									default:
										rectangle.X = 144;
										rectangle.Y = 54;
										break;
								}

							}
							else if (up != num && down != num && left != num && right == num) {

								switch (num55) {
									case 0:
										rectangle.X = 162;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 162;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 162;
										rectangle.Y = 36;
										break;
								}

							}
							else if (up != num && down != num && left == num && right != num) {

								switch (num55) {
									case 0:
										rectangle.X = 216;
										rectangle.Y = 0;
										break;
									case 1:
										rectangle.X = 216;
										rectangle.Y = 18;
										break;
									default:
										rectangle.X = 216;
										rectangle.Y = 36;
										break;
								}

							}
							else if (up != num && down != num && left != num && right != num) {
								
								switch (num55) {
									case 0:
										rectangle.X = 162;
										rectangle.Y = 54;
										break;
									case 1:
										rectangle.X = 180;
										rectangle.Y = 54;
										break;
									default:
										rectangle.X = 198;
										rectangle.Y = 54;
										break;
								}

							}

						}

						if (rectangle.X <= -1 || rectangle.Y <= -1) {

							if (num55 <= 0) {
								rectangle.X = 18;
								rectangle.Y = 18;
							}
							else if (num55 == 1) {
								rectangle.X = 36;
								rectangle.Y = 18;
							}

							if (num55 >= 2) {
								rectangle.X = 54;
								rectangle.Y = 18;
							}

						}

						if (Main.tileLargeFrames[num] == 1 && num55 == 3)
							rectangle.Y += 90;
						
						tile.frameX = (short)rectangle.X;
						tile.frameY = (short)rectangle.Y;

						if (num == 52 || num == 62 || num == 115 || num == 205) {

							up = ((tile60 == null) ? num : ((!tile60.active()) ? (-1) : ((!tile60.bottomSlope()) ? tile60.type : (-1))));
							
							if ((num == 52 || num == 205) && (up == 109 || up == 115)) {
								tile.type = 115;
								SquareTileFrame(i, j, ref undo);
								return;
							}

							if ((num == 115 || num == 205) && (up == 2 || up == 52)) {
								tile.type = 52;
								SquareTileFrame(i, j, ref undo);
								return;
							}

							if ((num == 52 || num == 115) && (up == 199 || up == 205)) {
								tile.type = 205;
								SquareTileFrame(i, j, ref undo);
								return;
							}

							if (up != num) {

								bool flag15 = false;

								if (up == -1)
									flag15 = true;
								
								if (num == 52 && up != 2 && up != 192)
									flag15 = true;
								
								if (num == 62 && up != 60)
									flag15 = true;
								
								if (num == 115 && up != 109)
									flag15 = true;
								
								if (num == 205 && up != 199)
									flag15 = true;
								
								if (flag15)
									KillTile(i, j, ref undo);
								

							}

						}

						if (!WorldGen.noTileActions && tile.active() && (num == 53 || num == 112 || num == 116 || num == 123 || num == 234 || num == 224 || num == 330 || num == 331 || num == 332 || num == 333)) {

							if (Main.netMode == NetmodeID.SinglePlayer) {

								if (tile61 != null && !tile61.active()) {

									bool flag16 = true;

									if (tile60.active() && (TileID.Sets.BasicChest[tile60.type] || TileID.Sets.BasicChestFake[tile60.type] || tile60.type == 323 || TileLoader.IsDresser(tile60.type)))
										flag16 = false;
									
									if (flag16) {

										tile.ClearTile();

										SquareTileFrame(i, j, ref undo);

									}

								}

							}
							else if (Main.netMode == NetmodeID.Server && tile61 != null && !tile61.active()) {

								bool flag17 = true;

								if (tile60.active() && (TileID.Sets.BasicChest[tile60.type] || TileID.Sets.BasicChestFake[tile60.type] || tile60.type == 323 || TileLoader.IsDresser(tile60.type)))
									flag17 = false;

								if (flag17) {

									tile.active(active: false);

									NetMessage.SendTileSquare(-1, i, j, 1);
									SquareTileFrame(i, j, ref undo);

								}

							}

						}

						if (rectangle.X != frameX && rectangle.Y != frameY && frameX >= 0 && frameY >= 0) {

							WorldGen.tileReframeCount++;

							if (WorldGen.tileReframeCount < 55) {

								bool num74 = mergeUp;
								bool flag19 = mergeDown;
								bool flag20 = mergeLeft;
								bool flag21 = mergeRight;
								TileFrame(i - 1, j, ref undo);
								TileFrame(i + 1, j, ref undo);
								TileFrame(i, j - 1, ref undo);
								TileFrame(i, j + 1, ref undo);
								mergeUp = num74;
								mergeDown = flag19;
								mergeLeft = flag20;
								mergeRight = flag21;

							}

							WorldGen.tileReframeCount--;

						}

					}

				}

			}
			catch {	}

			if (i > 0 && j > 0)
				WorldGen.UpdateMapTile(i, j, addToList);
			
		}

	}

}
