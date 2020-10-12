using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 7/31/2020 Copy/Paste

		private static float MakeDungeon_GroundFurniture(int wallType, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			float num = 2000f * Main.maxTilesX / 4200f;
			int num2 = 1 + Main.maxTilesX / 4200;
			int num3 = 1 + Main.maxTilesX / 4200;

			for (int i = 0; i < num; i++) {

				if (num2 > 0 || num3 > 0)
					i--;

				int num4 = genRand.Next(dMinX, dMaxX);
				int j = genRand.Next((int)Main.worldSurface + 10, dMaxY);

				while (!Main.wallDungeon[Main.tile[num4, j].wall] || Main.tile[num4, j].active()) {

					num4 = genRand.Next(dMinX, dMaxX);
					j = genRand.Next((int)Main.worldSurface + 10, dMaxY);

				}

				if (!Main.wallDungeon[Main.tile[num4, j].wall] || Main.tile[num4, j].active())
					continue;

				for (; !WorldGen.SolidTile(num4, j) && j < Main.maxTilesY - 200; j++) { }

				j--;
				int num5 = num4;
				int k = num4;

				while (!Main.tile[num5, j].active() && WorldGen.SolidTile(num5, j + 1))
					num5--;

				num5++;

				for (; !Main.tile[k, j].active() && WorldGen.SolidTile(k, j + 1); k++) { }

				k--;
				int num6 = k - num5;
				int num7 = (k + num5) / 2;

				if (Main.tile[num7, j].active() || !Main.wallDungeon[Main.tile[num7, j].wall] || !WorldGen.SolidTile(num7, j + 1) || Main.tile[num7, j + 1].type == 48)
					continue;

				int style = 13;
				int style2 = 10;
				int style3 = 11;
				int num8 = 1;
				int num9 = 46;
				int style4 = 1;
				int num10 = 5;
				int num11 = 11;
				int num12 = 5;
				int num13 = 6;
				int num14 = 21;
				int num15 = 22;
				int num16 = 24;
				int num17 = 30;

				switch (wallType) {

					case 8:
						style = 14;
						style2 = 11;
						style3 = 12;
						num8 = 2;
						num9 = 47;
						style4 = 2;
						num10 = 6;
						num11 = 12;
						num12 = 6;
						num13 = 7;
						num14 = 22;
						num15 = 23;
						num16 = 25;
						num17 = 31;
						break;

					case 9:
						style = 15;
						style2 = 12;
						style3 = 13;
						num8 = 3;
						num9 = 48;
						style4 = 3;
						num10 = 7;
						num11 = 13;
						num12 = 7;
						num13 = 8;
						num14 = 23;
						num15 = 24;
						num16 = 26;
						num17 = 32;
						break;

				}

				if (Main.tile[num7, j].wall >= 94 && Main.tile[num7, j].wall <= 105) {

					style = 17;
					style2 = 14;
					style3 = 15;
					num8 = -1;
					num9 = -1;
					style4 = 5;
					num10 = -1;
					num11 = -1;
					num12 = -1;
					num13 = -1;
					num14 = -1;
					num15 = -1;
					num16 = -1;
					num17 = -1;

				}

				int num18 = genRand.Next(13);

				if ((num18 == 10 || num18 == 11 || num18 == 12) && genRand.Next(4) != 0)
					num18 = genRand.Next(13);

				while ((num18 == 2 && num9 == -1) || (num18 == 5 && num10 == -1) || (num18 == 6 && num11 == -1) || (num18 == 7 && num12 == -1) || (num18 == 8 && num13 == -1) || (num18 == 9 && num14 == -1) || (num18 == 10 && num15 == -1) || (num18 == 11 && num16 == -1) || (num18 == 12 && num17 == -1))
					num18 = genRand.Next(13);

				int num19 = 0;
				int num20 = 0;

				if (num18 == 0) {
					num19 = 5;
					num20 = 4;
				}

				if (num18 == 1) {
					num19 = 4;
					num20 = 3;
				}

				if (num18 == 2) {
					num19 = 3;
					num20 = 5;
				}

				if (num18 == 3) {
					num19 = 4;
					num20 = 6;
				}

				if (num18 == 4) {
					num19 = 3;
					num20 = 3;
				}

				if (num18 == 5) {
					num19 = 5;
					num20 = 3;
				}

				if (num18 == 6) {
					num19 = 5;
					num20 = 4;
				}

				if (num18 == 7) {
					num19 = 5;
					num20 = 4;
				}

				if (num18 == 8) {
					num19 = 5;
					num20 = 4;
				}

				if (num18 == 9) {
					num19 = 5;
					num20 = 3;
				}

				if (num18 == 10) {
					num19 = 2;
					num20 = 4;
				}

				if (num18 == 11) {
					num19 = 3;
					num20 = 3;
				}

				if (num18 == 12) {
					num19 = 2;
					num20 = 5;
				}

				for (int l = num7 - num19; l <= num7 + num19; l++) {

					for (int m = j - num20; m <= j; m++) {

						if (Main.tile[l, m].active()) {

							num18 = -1;
							break;

						}

					}

				}

				if (num6 < num19 * 1.75)
					num18 = -1;

				if (num2 > 0 || num3 > 0) {

					if (num2 > 0) {

						PlaceTile(num7, j, 355, ref undo, mute: true);

						if (Main.tile[num7, j].type == 355)
							num2--;

					} else if (num3 > 0) {

						PlaceTile(num7, j, 354, ref undo, mute: true);

						if (Main.tile[num7, j].type == 354)
							num3--;

					}

					continue;

				}

				switch (num18) {

					case 0: {

							PlaceTile(num7, j, 14, ref undo, mute: true, forced: false, -1, style2);

							if (Main.tile[num7, j].active()) {

								if (!Main.tile[num7 - 2, j].active()) {

									PlaceTile(num7 - 2, j, 15, ref undo, mute: true, forced: false, -1, style);

									if (Main.tile[num7 - 2, j].active()) {

										undo.Add(new ChangedTile(num7 - 2, j));
										undo.Add(new ChangedTile(num7 - 2, j - 1));

										Main.tile[num7 - 2, j].frameX += 18;
										Main.tile[num7 - 2, j - 1].frameX += 18;

									}

								}

								if (!Main.tile[num7 + 2, j].active())
									PlaceTile(num7 + 2, j, 15, ref undo, mute: true, forced: false, -1, style);

							}

							for (int num22 = num7 - 1; num22 <= num7 + 1; num22++) {

								if (genRand.Next(2) == 0 && !Main.tile[num22, j - 2].active()) {

									int num23 = genRand.Next(5);

									if (num8 != -1 && num23 <= 1 && !Main.tileLighted[Main.tile[num22 - 1, j - 2].type])
										PlaceTile(num22, j - 2, 33, ref undo, mute: true, forced: false, -1, num8);

									if (num23 == 2 && !Main.tileLighted[Main.tile[num22 - 1, j - 2].type])
										PlaceTile(num22, j - 2, 49, ref undo, mute: true);

									if (num23 == 3)
										PlaceTile(num22, j - 2, 50, ref undo, mute: true);

									if (num23 == 4)
										PlaceTile(num22, j - 2, 103, ref undo, mute: true);

								}

							}

							break;

						}
					case 1: {

							PlaceTile(num7, j, 18, ref undo, mute: true, forced: false, -1, style3);

							if (!Main.tile[num7, j].active())
								break;

							if (genRand.Next(2) == 0) {

								if (!Main.tile[num7 - 1, j].active()) {

									PlaceTile(num7 - 1, j, 15, ref undo, mute: true, forced: false, -1, style);

									if (Main.tile[num7 - 1, j].active()) {

										undo.Add(new ChangedTile(num7 - 1, j));
										undo.Add(new ChangedTile(num7 - 1, j - 1));

										Main.tile[num7 - 1, j].frameX += 18;
										Main.tile[num7 - 1, j - 1].frameX += 18;

									}

								}

							} else if (!Main.tile[num7 + 2, j].active()) {
								PlaceTile(num7 + 2, j, 15, ref undo, mute: true, forced: false, -1, style);
							}

							for (int n = num7; n <= num7 + 1; n++) {

								if (genRand.Next(2) == 0 && !Main.tile[n, j - 1].active()) {

									int num21 = genRand.Next(5);

									if (num8 != -1 && num21 <= 1 && !Main.tileLighted[Main.tile[n - 1, j - 1].type])
										PlaceTile(n, j - 1, 33, ref undo, mute: true, forced: false, -1, num8);

									if (num21 == 2 && !Main.tileLighted[Main.tile[n - 1, j - 1].type])
										PlaceTile(n, j - 1, 49, ref undo, mute: true);

									if (num21 == 3)
										PlaceTile(n, j - 1, 50, ref undo, mute: true);

									if (num21 == 4)
										PlaceTile(n, j - 1, 103, ref undo, mute: true);

								}

							}

							break;

						}
					case 2: {
							PlaceTile(num7, j, 105, ref undo, mute: true, forced: false, -1, num9);
							break;
						}
					case 3: {
							PlaceTile(num7, j, 101, ref undo, mute: true, forced: false, -1, style4);
							break;
						}
					case 4: {

							if (genRand.Next(2) == 0) {

								PlaceTile(num7, j, 15, ref undo, mute: true, forced: false, -1, style);

								undo.Add(new ChangedTile(num7, j));
								undo.Add(new ChangedTile(num7, j - 1));

								Main.tile[num7, j].frameX += 18;
								Main.tile[num7, j - 1].frameX += 18;

							}
							else {
								PlaceTile(num7, j, 15, ref undo, mute: true, forced: false, -1, style);
							}

							break;
						}
					case 5: {
							Place4x2(num7, j, 79, ref undo, WorldGen.genRand.Next(2) == 0 ? 1 : -1, num14);
							break;
						}
					case 6: {
							PlaceTile(num7, j, 87, ref undo, mute: true, forced: false, -1, num11);
							break;
						}
					case 7: {
							PlaceTile(num7, j, 88, ref undo, mute: true, forced: false, -1, num12);
							break;
						}
					case 8: {
							PlaceTile(num7, j, 89, ref undo, mute: true, forced: false, -1, num13);
							break;
						}
					case 9: {
							Place4x2(num7, j, 90, ref undo, WorldGen.genRand.Next(2) == 0 ? 1 : -1, num14);
							break;
						}
					case 10: {
							PlaceTile(num7, j, 93, ref undo, mute: true, forced: false, -1, num16);
							break;
						}
					case 11: {
							PlaceTile(num7, j, 100, ref undo, mute: true, forced: false, -1, num15);
							break;
						}
					case 12: {
							PlaceTile(num7, j, 104, ref undo, mute: true, forced: false, -1, num17);
							break;
						}

				}

			}

			return num;

		}

	}

}
