using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 7/31/2020 Copy/Paste

		private static void MakeDungeon_Lights(ushort tileType, ref int failCount, int failMax, ref int numAdd, int[] roomWall, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			int[] array = new int[3] {
				genRand.Next(7),
				genRand.Next(7),
				0
			};

			while (array[1] == array[0])
				array[1] = genRand.Next(7);

			array[2] = genRand.Next(7);

			while (array[2] == array[0] || array[2] == array[1])
				array[2] = genRand.Next(7);

			while (numAdd < Main.maxTilesX / 150) {

				failCount++;
				int num  = genRand.Next(dMinX, dMaxX);
				int num2 = genRand.Next(dMinY, dMaxY);

				if (Main.wallDungeon[Main.tile[num, num2].wall]) {

					for (int num3 = num2; num3 > dMinY; num3--) {

						if (Main.tile[num, num3 - 1].active() && Main.tile[num, num3 - 1].type == tileType) {

							bool flag = false;

							for (int i = num - 15; i < num + 15; i++) {

								for (int j = num3 - 15; j < num3 + 15; j++) {

									if (i > 0 && i < Main.maxTilesX && j > 0 && j < Main.maxTilesY && (Main.tile[i, j].type == 42 || Main.tile[i, j].type == 34)) {

										flag = true;
										break;

									}

								}

							}

							if (Main.tile[num - 1, num3].active() || Main.tile[num + 1, num3].active() || Main.tile[num - 1, num3 + 1].active() || Main.tile[num + 1, num3 + 1].active() || Main.tile[num, num3 + 2].active())
								flag = true;

							if (flag)
								break;

							bool flag2 = false;

							if (!flag2 && genRand.Next(7) == 0) {

								int style = 27;

								switch (roomWall[0]) {

									case 7:
										style = 27;
										break;

									case 8:
										style = 28;
										break;

									case 9:
										style = 29;
										break;

								}

								bool flag3 = false;

								for (int k = 0; k < 15; k++) {

									if (WorldGen.SolidTile(num, num3 + k)) {
										flag3 = true;
										break;
									}

								}

								if (!flag3)
									PlaceChand(num, num3, 34, ref undo, style);

								if (Main.tile[num, num3].type == 34) {

									flag2 = true;
									failCount = 0;
									numAdd++;

									for (int l = 0; l < 1000; l++) {

										int num4 = num + genRand.Next(-12, 13);
										int num5 = num3 + genRand.Next(3, 21);

										if (Main.tile[num4, num5].active() || Main.tile[num4, num5 + 1].active() || Main.tile[num4 - 1, num5].type == 48 || Main.tile[num4 + 1, num5].type == 48 || !Collision.CanHit(new Vector2(num4 * 16, num5 * 16), 16, 16, new Vector2(num * 16, num3 * 16 + 1), 16, 16))
											continue;

										if ((WorldGen.SolidTile(num4 - 1, num5) && Main.tile[num4 - 1, num5].type != 10) || (WorldGen.SolidTile(num4 + 1, num5) && Main.tile[num4 + 1, num5].type != 10) || WorldGen.SolidTile(num4, num5 + 1))
											PlaceTile(num4, num5, 136, ref undo, mute: true);

										if (!Main.tile[num4, num5].active())
											continue;

										while (num4 != num || num5 != num3) {

											undo.Add(new ChangedTile(num4, num5));
											Main.tile[num4, num5].wire(wire: true);

											if (num4 > num)
												num4--;

											if (num4 < num)
												num4++;

											undo.Add(new ChangedTile(num4, num5));
											Main.tile[num4, num5].wire(wire: true);

											if (num5 > num3)
												num5--;

											if (num5 < num3)
												num5++;

											undo.Add(new ChangedTile(num4, num5));
											Main.tile[num4, num5].wire(wire: true);

										}

										if (WorldGen.genRand.Next(3) > 0) {

											undo.Add(new ChangedTile(num, num3));
											undo.Add(new ChangedTile(num, num3 + 1));
											Main.tile[num, num3].frameX = 18;
											Main.tile[num, num3 + 1].frameX = 18;

										}

										break;

									}

								}

							}

							if (flag2)
								break;

							int style2 = array[0];

							if (Main.tile[num, num3].wall == roomWall[1])
								style2 = array[1];

							if (Main.tile[num, num3].wall == roomWall[2])
								style2 = array[2];

							Place1x2Top(num, num3, 42, style2, ref undo);

							if (Main.tile[num, num3].type != 42)
								break;
                            
							failCount = 0;
							numAdd++;

							for (int m = 0; m < 1000; m++) {

								int num6 = num + genRand.Next(-12, 13);
								int num7 = num3 + genRand.Next(3, 21);

								if (Main.tile[num6, num7].active() || Main.tile[num6, num7 + 1].active() || Main.tile[num6 - 1, num7].type == 48 || Main.tile[num6 + 1, num7].type == 48 || !Collision.CanHit(new Vector2(num6 * 16, num7 * 16), 16, 16, new Vector2(num * 16, num3 * 16 + 1), 16, 16))
									continue;

								if ((WorldGen.SolidTile(num6 - 1, num7) && Main.tile[num6 - 1, num7].type != 10) || (WorldGen.SolidTile(num6 + 1, num7) && Main.tile[num6 + 1, num7].type != 10) || WorldGen.SolidTile(num6, num7 + 1))
									PlaceTile(num6, num7, 136, ref undo, mute: true);

								if (!Main.tile[num6, num7].active())
									continue;

								while (num6 != num || num7 != num3) {

									undo.Add(new ChangedTile(num6, num7));
									Main.tile[num6, num7].wire(wire: true);

									if (num6 > num)
										num6--;

									if (num6 < num)
										num6++;

									undo.Add(new ChangedTile(num6, num7));
									Main.tile[num6, num7].wire(wire: true);

									if (num7 > num3)
										num7--;

									if (num7 < num3)
										num7++;

									undo.Add(new ChangedTile(num6, num7));
									Main.tile[num6, num7].wire(wire: true);

								}

								if (genRand.Next(3) > 0) {

									undo.Add(new ChangedTile(num, num3));
									undo.Add(new ChangedTile(num, num3 + 1));
									Main.tile[num, num3].frameX = 18;
									Main.tile[num, num3 + 1].frameX = 18;

								}

								break;

							}

							break;

						}

					}

				}

				if (failCount > failMax) {

					numAdd++;
					failCount = 0;

				}

			}

		}

	}

}
