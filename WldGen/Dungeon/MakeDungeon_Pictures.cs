using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 7/31/2020 Copy/Paste

		private static float MakeDungeon_Pictures(int[] roomWall, float count, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			count = 420000f / Main.maxTilesX;

			for (int i = 0; i < count; i++) {

				int curX = genRand.Next(dMinX, dMaxX);
				int curY = genRand.Next((int)Main.worldSurface, dMaxY);

				while (!Main.wallDungeon[Main.tile[curX, curY].wall] || Main.tile[curX, curY].active()) {

					curX = genRand.Next(dMinX, dMaxX);
					curY = genRand.Next((int)Main.worldSurface, dMaxY);

				}

				int num3;
				int num4;
				int num5;
				int num6;

                for (int j = 0; j < 2; j++) {

					num3 = curX;
					num4 = curX;

					while (!Main.tile[num3, curY].active() && Main.wallDungeon[Main.tile[num3, curY].wall])
						num3--;

					num3++;

					for (; !Main.tile[num4, curY].active() && Main.wallDungeon[Main.tile[num4, curY].wall]; num4++) { }

					num4--;
					curX = (num3 + num4) / 2;
					num5 = curY;
					num6 = curY;

					while (!Main.tile[curX, num5].active() && Main.wallDungeon[Main.tile[curX, num5].wall])
						num5--;

					num5++;

					for (; !Main.tile[curX, num6].active() && Main.wallDungeon[Main.tile[curX, num6].wall]; num6++) { }

					num6--;
					curY = (num5 + num6) / 2;

				}

				num3 = curX;
				num4 = curX;

				while (!Main.tile[num3, curY].active() && !Main.tile[num3, curY - 1].active() && !Main.tile[num3, curY + 1].active())
					num3--;

				num3++;

				for (; !Main.tile[num4, curY].active() && !Main.tile[num4, curY - 1].active() && !Main.tile[num4, curY + 1].active(); num4++) { }

				num4--;
				num5 = curY;
				num6 = curY;

				while (!Main.tile[curX, num5].active() && !Main.tile[curX - 1, num5].active() && !Main.tile[curX + 1, num5].active())
					num5--;

				num5++;

				for (; !Main.tile[curX, num6].active() && !Main.tile[curX - 1, num6].active() && !Main.tile[curX + 1, num6].active(); num6++) { }

				num6--;
				curX = (num3 + num4) / 2;
				curY = (num5 + num6) / 2;

                int num7 = num4 - num3;
                int num8 = num6 - num5;

                if (num7 <= 7 || num8 <= 5)
					continue;

				bool[] array = new bool[] {
					true,
					false,
					false
				};

				if (num7 > num8 * 3 && num7 > 21)
					array[1] = true;

				if (num8 > num7 * 3 && num8 > 21)
					array[2] = true;

				int num9 = genRand.Next(3);

				if (Main.tile[curX, curY].wall == roomWall[0])
					num9 = 0;

				while (!array[num9])
					num9 = genRand.Next(3);

				if (WorldGen.nearPicture2(curX, curY))
					num9 = -1;

				switch (num9) {

					case 0: {

							Vector2 vector2 = WorldGen.randPictureTile();

							if (Main.tile[curX, curY].wall != roomWall[0])
								vector2 = WorldGen.randBoneTile();

							int type2  = (int)vector2.X;
							int style2 = (int)vector2.Y;

							if (!WorldGen.nearPicture(curX, curY))
								PlaceTile(curX, curY, type2, ref undo, mute: true, forced: false, -1, style2);

							break;

						}
					case 1: {

							Vector2 vector3 = WorldGen.randPictureTile();

							if (Main.tile[curX, curY].wall != roomWall[0])
								vector3 = WorldGen.randBoneTile();

							int type3  = (int)vector3.X;
							int style3 = (int)vector3.Y;

							if (!Main.tile[curX, curY].active())
								PlaceTile(curX, curY, type3, ref undo, mute: true, forced: false, -1, style3);

							int num13 = curX;
							int num14 = curY;
							int num15 = curY;

							for (int m = 0; m < 2; m++) {

								curX += 7;
								num5 = num15;
								num6 = num15;

								while (!Main.tile[curX, num5].active() && !Main.tile[curX - 1, num5].active() && !Main.tile[curX + 1, num5].active())
									num5--;

								num5++;

								for (; !Main.tile[curX, num6].active() && !Main.tile[curX - 1, num6].active() && !Main.tile[curX + 1, num6].active(); num6++) { }

								num6--;
								num15 = (num5 + num6) / 2;
								vector3 = WorldGen.randPictureTile();

								if (Main.tile[curX, num15].wall != roomWall[0])
									vector3 = WorldGen.randBoneTile();

								type3  = (int)vector3.X;
								style3 = (int)vector3.Y;

								if (Math.Abs(num14 - num15) >= 4 || WorldGen.nearPicture(curX, num15))
									break;

								PlaceTile(curX, num15, type3, ref undo, mute: true, forced: false, -1, style3);

							}

							num15 = curY;
							curX = num13;

							for (int n = 0; n < 2; n++) {

								curX -= 7;
								num5 = num15;
								num6 = num15;

								while (!Main.tile[curX, num5].active() && !Main.tile[curX - 1, num5].active() && !Main.tile[curX + 1, num5].active())
									num5--;

								num5++;

								for (; !Main.tile[curX, num6].active() && !Main.tile[curX - 1, num6].active() && !Main.tile[curX + 1, num6].active(); num6++) { }

								num6--;
								num15 = (num5 + num6) / 2;
								vector3 = WorldGen.randPictureTile();

								if (Main.tile[curX, num15].wall != roomWall[0])
									vector3 = WorldGen.randBoneTile();

								type3 = (int)vector3.X;
								style3 = (int)vector3.Y;

								if (Math.Abs(num14 - num15) >= 4 || WorldGen.nearPicture(curX, num15))
									break;

								PlaceTile(curX, num15, type3, ref undo, mute: true, forced: false, -1, style3);

							}

							break;

						}
					case 2: {

							Vector2 vector = WorldGen.randPictureTile();

							if (Main.tile[curX, curY].wall != roomWall[0])
								vector = WorldGen.randBoneTile();

							int type  = (int)vector.X;
							int style = (int)vector.Y;

							if (!Main.tile[curX, curY].active())
								PlaceTile(curX, curY, type, ref undo, mute: true, forced: false, -1, style);

							int num10 = curY;
							int num11 = curX;
							int num12 = curX;

							for (int k = 0; k < 3; k++) {

								curY += 7;
								num3 = num12;
								num4 = num12;

								while (!Main.tile[num3, curY].active() && !Main.tile[num3, curY - 1].active() && !Main.tile[num3, curY + 1].active())
									num3--;

								num3++;

								for (; !Main.tile[num4, curY].active() && !Main.tile[num4, curY - 1].active() && !Main.tile[num4, curY + 1].active(); num4++) { }

								num4--;
								num12 = (num3 + num4) / 2;
								vector = WorldGen.randPictureTile();

								if (Main.tile[num12, curY].wall != roomWall[0])
									vector = WorldGen.randBoneTile();

								type  = (int)vector.X;
								style = (int)vector.Y;

								if (Math.Abs(num11 - num12) >= 4 || WorldGen.nearPicture(num12, curY))
									break;

								PlaceTile(num12, curY, type, ref undo, mute: true, forced: false, -1, style);

							}

							num12 = curX;
							curY = num10;

							for (int l = 0; l < 3; l++) {

								curY -= 7;
								num3 = num12;
								num4 = num12;

								while (!Main.tile[num3, curY].active() && !Main.tile[num3, curY - 1].active() && !Main.tile[num3, curY + 1].active())
									num3--;

								num3++;

								for (; !Main.tile[num4, curY].active() && !Main.tile[num4, curY - 1].active() && !Main.tile[num4, curY + 1].active(); num4++) { }

								num4--;
								num12 = (num3 + num4) / 2;
								vector = WorldGen.randPictureTile();

								if (Main.tile[num12, curY].wall != roomWall[0])
									vector = WorldGen.randBoneTile();

								type  = (int)vector.X;
								style = (int)vector.Y;

								if (Math.Abs(num11 - num12) >= 4 || WorldGen.nearPicture(num12, curY))
									break;

								PlaceTile(num12, curY, type, ref undo, mute: true, forced: false, -1, style);

							}

							break;

						}

				}

			}

			return count;

		}

	}

}
