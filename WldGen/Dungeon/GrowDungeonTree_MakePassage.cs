using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/2/2020 Copy/Paste

		private static void GrowDungeonTree_MakePassage(int j, int W, ref int minl, ref int minr, ref UndoStep undo, bool noSecretRoom = false) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num = minl;
			int num2 = minr;
			_ = (minl + minr) / 2;
			int num3 = 5;
			int num4 = j - 6;
			int num5 = 0;

			bool flag = true;

			genRand.Next(5, 16);

			while (true) {

				num4++;
				
				if (num4 > dungeonY - 5)
					break;
				
				int num6 = (minl + minr) / 2;
				int num7 = 1;

				if (num4 > j && W <= 4)
					num7++;
				
				for (int i = minl - num7; i <= minr + num7; i++) {

					if (i > num6 - 2 && i <= num6 + 1) {

						if (num4 > j - 4) {

							if (Main.tile[i, num4].type != 19 && Main.tile[i, num4].type != 15 && Main.tile[i, num4].type != 304 && Main.tile[i, num4].type != 21 && Main.tile[i, num4].type != 10 && Main.tile[i, num4 - 1].type != 15 && Main.tile[i, num4 - 1].type != 304 && Main.tile[i, num4 - 1].type != 21 && Main.tile[i, num4 - 1].type != 10 && Main.tile[i, num4 + 1].type != 10)
								Neo.SetActive(i, num4, false, ref undo);
							
							if (!Main.wallDungeon[Main.tile[i, num4].wall])
								Neo.SetWall(i, num4, 244, ref undo);

							if (!Main.wallDungeon[Main.tile[i - 1, num4].wall] && (Main.tile[i - 1, num4].wall > 0 || (double)num4 >= Main.worldSurface))
								Neo.SetWall(i - 1, num4, 244, ref undo);

							if (!Main.wallDungeon[Main.tile[i + 1, num4].wall] && (Main.tile[i + 1, num4].wall > 0 || (double)num4 >= Main.worldSurface))
								Neo.SetWall(i + 1, num4, 244, ref undo);

							if (num4 == j && i > num6 - 2 && i <= num6 + 1) {

								Neo.SetActive(i, num4 + 1, false, ref undo);
								PlaceTile(i, num4 + 1, 19, ref undo, mute: true, forced: false, -1, 23);

							}

						}

					}
					else {

						if (Main.tile[i, num4].type != 15 && Main.tile[i, num4].type != 304 && Main.tile[i, num4].type != 21 && Main.tile[i, num4].type != 10 && Main.tile[i - 1, num4].type != 10 && Main.tile[i + 1, num4].type != 10) {

							if (!Main.wallDungeon[Main.tile[i, num4].wall])
								Neo.SetTile(i, num4, 191, ref undo);

							if (Main.tile[i - 1, num4].type == 40)
								Neo.SetTile(i - 1, num4, 0, ref undo);

							if (Main.tile[i + 1, num4].type == 40)
								Neo.SetTile(i + 1, num4, 0, ref undo);

						}

						if (num4 <= j && num4 > j - 4 && i > minl - num7 && i <= minr + num7 - 1)
							Neo.SetWall(i, num4, 244, ref undo);

					}

					WorldGen.SquareTileFrame(i, num4);
					WorldGen.SquareWallFrame(i, num4);

				}

				num5++;

				if (num5 < 6)
					continue;
				
				num5 = 0;

				int num8 = genRand.Next(3);

				if (num8 == 0)
					num8 = -1;
				
				if (flag)
					num8 = 2;
				
				if (num8 == -1 && Main.tile[minl - num3, num4].wall == 244) {
					num8 = 1;
				}
				else if (num8 == 1 && Main.tile[minr + num3, num4].wall == 244) {
					num8 = -1;
				}

				if (num8 == 2) {

					flag = false;

					int num9 = 23;

					if (Main.wallDungeon[Main.tile[minl, num4 + 1].wall] || Main.wallDungeon[Main.tile[minl + 1, num4 + 1].wall] || Main.wallDungeon[Main.tile[minl + 2, num4 + 1].wall])
						num9 = 12;
					
					if (!WorldGen.SolidTile(minl - 1, num4 + 1) && !WorldGen.SolidTile(minr + 1, num4 + 1) && num9 == 12)
						continue;
					
					for (int k = minl; k <= minr; k++) {

						if (k > num6 - 2 && k <= num6 + 1) {

							Neo.SetActive(k, num4 + 1, false, ref undo);
							PlaceTile(k, num4 + 1, 19, ref undo, mute: true, forced: false, -1, num9);

						}

					}

				}
				else {

					minl += num8;
					minr += num8;

				}

			}

			minl = num;
			minr = num2;
			_ = (minl + minr) / 2;

			for (int l = minl; l <= minr; l++) {

				for (int m = j - 3; m <= j; m++) {

					Neo.SetActive(l, m, false, ref undo);

					if (!Main.wallDungeon[Main.tile[l, m].wall])
						Neo.SetWall(l, m, 244);

				}

			}

		}

	}

}
