using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/2/2020 Copy/Paste

		public static void DungeonEnt(int i, int j, ushort tileType, ushort wallType, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num = 60;

			for (int k = i - num; k < i + num; k++) {

				for (int l = j - num; l < j + num; l++) {

					Neo.SetLiquid(k, l, 0, ref undo, false);
					Main.tile[k, l].Clear(TileDataType.Slope);

				}

			}

			double num2 = dxStrength1;
			double num3 = dyStrength1;

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j - (float)num3 / 2f;

			dMinY = (int)vector.Y;

			int num4 = 1;

			if (i > Main.maxTilesX / 2)
				num4 = -1;

			if (DrunkWorldGen || GetGoodWorldGen)
				num4 *= -1;
			
			int minX = (int)(vector.X - num2 * 0.6f - genRand.Next(2, 5));
			int maxX = (int)(vector.X + num2 * 0.6f + genRand.Next(2, 5));
			int minY = (int)(vector.Y - num3 * 0.6f - genRand.Next(2, 5));
			int maxY = (int)(vector.Y + num3 * 0.6f + genRand.Next(8, 16));

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			for (int m = minX; m < maxX; m++) {

				for (int n = minY; n < maxY; n++) {

					Neo.SetLiquid(m, n, 0, ref undo);

					if (Main.tile[m, n].wall != wallType) {

						Neo.SetWall(m, n, 0);

						if (m > minX + 1 && m < maxX - 2 && n > minY + 1 && n < maxY - 2)
							Neo.SetWall(m, n, wallType);
						
						Neo.SetTile(m, n, tileType);

					}

				}

			}

			int num9 = minX;
			int num10 = minX + 5 + genRand.Next(4);
			int num11 = minY - 3 - genRand.Next(3);
			int num12 = minY;

			for (int num13 = num9; num13 < num10; num13++) {

				for (int num14 = num11; num14 < num12; num14++) {

					Neo.SetLiquid(num13, num14, 0, ref undo);

					if (Main.tile[num13, num14].wall != wallType)
						Neo.SetTile(num13, num14, tileType);

				}

			}

			num9  = maxX - 5 - genRand.Next(4);
			num10 = maxX;
			num11 = minY - 3 - genRand.Next(3);
			num12 = minY;

			for (int num15 = num9; num15 < num10; num15++) {

				for (int num16 = num11; num16 < num12; num16++) {

					Neo.SetLiquid(num15, num16, 0, ref undo);

					if (Main.tile[num15, num16].wall != wallType)
						Neo.SetTile(num15, num16, tileType);

				}

			}

			int num17 = 1 + genRand.Next(2);
			int num18 = 2 + genRand.Next(4);
			int num19 = 0;

			for (int num20 = minX; num20 < maxX; num20++) {

				for (int num21 = minY - num17; num21 < minY; num21++) {

					Neo.SetLiquid(num20, num21, 0, ref undo);

					if (Main.tile[num20, num21].wall != wallType)
						Neo.SetTile(num20, num21, tileType);

				}

				num19++;

				if (num19 >= num18) {
					num20 += num18;
					num19 = 0;
				}

			}

			for (int num22 = minX; num22 < maxX; num22++) {

				for (int num23 = maxY; num23 < Main.worldSurface; num23++) {

					Neo.SetLiquid(num22, num23, 0, ref undo);

					if (!Main.wallDungeon[Main.tile[num22, num23].wall])
						Neo.SetTile(num22, num23, tileType);

					if (num22 > minX && num22 < maxX - 1)
						Neo.SetWall(num22, num23, wallType);

					Main.tile[num22, num23].Clear(TileDataType.Slope);

				}

			}

			minX = (int)(vector.X - num2 * 0.6f);
			maxX = (int)(vector.X + num2 * 0.6f);
			minY = (int)(vector.Y - num3 * 0.6f);
			maxY = (int)(vector.Y + num3 * 0.6f);

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			for (int num24 = minX; num24 < maxX; num24++) {

				for (int num25 = minY; num25 < maxY; num25++) {

					Neo.SetLiquid(num24, num25, 0, ref undo);
					Neo.SetWall(num24, num25, wallType);
					Main.tile[num24, num25].Clear(TileDataType.Slope);

				}

			}

			minX = (int)(vector.X - num2 * 0.6f - 1.0);
			maxX = (int)(vector.X + num2 * 0.6f + 1.0);
			minY = (int)(vector.Y - num3 * 0.6f - 1.0);
			maxY = (int)(vector.Y + num3 * 0.6f + 1.0);

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			if (DrunkWorldGen)
				minX -= 4;

			for (int num26 = minX; num26 < maxX; num26++) {

				for (int num27 = minY; num27 < maxY; num27++) {

					Neo.SetLiquid(num26, num27, 0, ref undo);
					Neo.SetWall(num26, num27, wallType);
					Main.tile[num26, num27].Clear(TileDataType.Slope);

				}

			}

			minX = (int)(vector.X - num2 * 0.5f);
			maxX = (int)(vector.X + num2 * 0.5f);
			minY = (int)(vector.Y - num3 * 0.5f);
			maxY = (int)(vector.Y + num3 * 0.5f);

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			for (int num28 = minX; num28 < maxX; num28++) {

				for (int num29 = minY; num29 < maxY; num29++) {

					Neo.SetLiquid(num28, num29, 0, ref undo);
					Neo.SetActive(num28, num29, false);
					Neo.SetWall(num28, num29, wallType);

				}

			}

            int num31 = maxY;

			for (int num32 = 0; num32 < 20; num32++) {

                int num30 = (int)vector.X - num32;

                if (!Main.tile[num30, num31].active() && Main.wallDungeon[Main.tile[num30, num31].wall]) {

					DPlatX[numDPlats] = num30;
					DPlatY[numDPlats] = num31;
					numDPlats++;
					break;

				}

				num30 = (int)vector.X + num32;

				if (!Main.tile[num30, num31].active() && Main.wallDungeon[Main.tile[num30, num31].wall]) {

					DPlatX[numDPlats] = num30;
					DPlatY[numDPlats] = num31;
					numDPlats++;
					break;

				}

			}

			vector.X += (float)num2 * 0.6f * num4;
			vector.Y += (float)num3 * 0.5f;

			num2 = dxStrength2;
			num3 = dyStrength2;

			vector.X += (float)num2 * 0.55f * num4;
			vector.Y -= (float)num3 * 0.5f;

			minX = (int)(vector.X - num2 * 0.6f - WorldGen.genRand.Next(1, 3));
			maxX = (int)(vector.X + num2 * 0.6f + WorldGen.genRand.Next(1, 3));
			minY = (int)(vector.Y - num3 * 0.6f - WorldGen.genRand.Next(1, 3));
			maxY = (int)(vector.Y + num3 * 0.6f + WorldGen.genRand.Next(6, 16));

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			for (int num33 = minX; num33 < maxX; num33++) {

				for (int num34 = minY; num34 < maxY; num34++) {

					Neo.SetLiquid(num33, num34, 0, ref undo);

					if (Main.tile[num33, num34].wall == wallType)
						continue;

					bool flag = true;

					if (num4 < 0) {

						if (num33 < vector.X - num2 * 0.5)
							flag = false;

					} else if (num33 > vector.X + num2 * 0.5 - 1.0) {
						flag = false;
					}

					if (flag)
						Neo.SetTileWall(num33, num34, tileType, 0);

				}

			}

			for (int num35 = minX; num35 < maxX; num35++) {

				for (int num36 = maxY; num36 < Main.worldSurface; num36++) {

					Neo.SetLiquid(num35, num36, 0, ref undo);

					if (!Main.wallDungeon[Main.tile[num35, num36].wall])
						Neo.SetTile(num35, num36, tileType);

					Neo.SetWall(num35, num36, wallType);

					Main.tile[num35, num36].Clear(TileDataType.Slope);

				}

			}

			minX = (int)(vector.X - num2 * 0.5);
			maxX = (int)(vector.X + num2 * 0.5);
			num9 = minX;

			if (num4 < 0)
				num9++;

			num10 = num9 + 5 + genRand.Next(4);
			num11 = minY - 3 - genRand.Next(3);
			num12 = minY;

			for (int num37 = num9; num37 < num10; num37++) {

				for (int num38 = num11; num38 < num12; num38++) {

					Neo.SetLiquid(num37, num38, 0, ref undo);

					if (Main.tile[num37, num38].wall != wallType)
						Neo.SetTile(num37, num38, tileType);

				}

			}

			num9 = maxX - 5 - genRand.Next(4);
			num10 = maxX;
			num11 = minY - 3 - genRand.Next(3);
			num12 = minY;

			for (int num39 = num9; num39 < num10; num39++) {

				for (int num40 = num11; num40 < num12; num40++) {

					Neo.SetLiquid(num39, num40, 0, ref undo);

					if (Main.tile[num39, num40].wall != wallType)
						Neo.SetTile(num39, num40, tileType);

				}

			}

			num17 = 1 + genRand.Next(2);
			num18 = 2 + genRand.Next(4);
			num19 = 0;

			if (num4 < 0)
				maxX++;

			for (int num41 = minX + 1; num41 < maxX - 1; num41++) {

				for (int num42 = minY - num17; num42 < minY; num42++) {

					Neo.SetLiquid(num41, num42, 0, ref undo);

					if (Main.tile[num41, num42].wall != wallType)
						Neo.SetTile(num41, num42, tileType);

				}

				num19++;

				if (num19 >= num18) {

					num41 += num18;
					num19 = 0;

				}

			}

			if (!DrunkWorldGen) {

				minX = (int)(vector.X - num2 * 0.6);
				maxX = (int)(vector.X + num2 * 0.6);
				minY = (int)(vector.Y - num3 * 0.6);
				maxY = (int)(vector.Y + num3 * 0.6);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				for (int num43 = minX; num43 < maxX; num43++) {

					for (int num44 = minY; num44 < maxY; num44++) {

						Neo.SetLiquid(num43, num44, 0, ref undo);
						Neo.SetWall(num43, num44, 0);

					}

				}

			}

			minX = (int)(vector.X - num2 * 0.6f);
			maxX = (int)(vector.X + num2 * 0.6f);
			minY = (int)(vector.Y - num3 * 0.6f);
			maxY = (int)(vector.Y + num3 * 0.6f);

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			for (int num45 = minX; num45 < maxX; num45++) {

				for (int num46 = minY; num46 < maxY; num46++) {

					Neo.SetLiquid(num45, num46, 0, ref undo);
					Neo.SetActive(num45, num46, false);
					Neo.SetWall(num45, num46, 0);

				}

			}

			if (DrunkWorldGen) {

				int num48 = (int)Main.worldSurface;

				while (Main.tile[dungeonX, num48].active() || Main.tile[dungeonX, num48].wall > 0 || Main.tile[dungeonX, num48 - 1].active() || Main.tile[dungeonX, num48 - 1].wall > 0 || Main.tile[dungeonX, num48 - 2].active() || Main.tile[dungeonX, num48 - 2].wall > 0 || Main.tile[dungeonX, num48 - 3].active() || Main.tile[dungeonX, num48 - 3].wall > 0 || Main.tile[dungeonX, num48 - 4].active() || Main.tile[dungeonX, num48 - 4].wall > 0) {

					num48--;

					if (num48 < 50)
						break;
					
				}

				if (num48 > 50)
					GrowDungeonTree(dungeonX, num48, ref undo);
				
			}

			if (!DrunkWorldGen) {

				int num49 = 100;

				if (num4 == 1) {

					int num50 = 0;

					for (int num51 = maxX; num51 < maxX + num49; num51++) {

						num50++;

						for (int num52 = maxY + num50; num52 < maxY + num49; num52++) {

							Neo.SetLiquid(num51, num52, 0, ref undo);
							Neo.SetLiquid(num51, num52 - 1, 0, ref undo);
							Neo.SetLiquid(num51, num52 - 2, 0, ref undo);
							Neo.SetLiquid(num51, num52 - 3, 0, ref undo);

							if (!Main.wallDungeon[Main.tile[num51, num52].wall] && Main.tile[num51, num52].wall != 3 && Main.tile[num51, num52].wall != 83)
								Neo.SetTile(num51, num52, tileType);

						}

					}

				}
				else {

					int num53 = 0;

					for (int num54 = minX; num54 > minX - num49; num54--) {

						num53++;

						for (int num55 = maxY + num53; num55 < maxY + num49; num55++) {

							Neo.SetLiquid(num54, num55, 0, ref undo);
							Neo.SetLiquid(num54, num55 - 1, 0, ref undo);
							Neo.SetLiquid(num54, num55 - 2, 0, ref undo);
							Neo.SetLiquid(num54, num55 - 3, 0, ref undo);

							if (!Main.wallDungeon[Main.tile[num54, num55].wall] && Main.tile[num54, num55].wall != 3 && Main.tile[num54, num55].wall != 83)
								Neo.SetTile(num54, num55, tileType);

						}

					}

				}

			}

			num18 = 2 + genRand.Next(4);
			num19 = 0;

			minX = (int)(vector.X - num2 * 0.5);
			maxX = (int)(vector.X + num2 * 0.5);

			if (DrunkWorldGen) {

				if (num4 == 1) {

					maxX--;
					minX--;

				}
				else {

					minX++;
					maxX++;

				}

			}
			else {

				minX += 2;
				maxX -= 2;

			}

			for (int num56 = minX; num56 < maxX; num56++) {

				for (int num57 = minY; num57 < maxY + 1; num57++)
					PlaceWall(num56, num57, wallType, ref undo, mute: true);
				
				if (!DrunkWorldGen) {

					num19++;

					if (num19 >= num18) {

						num56 += num18 * 2;
						num19 = 0;

					}

				}

			}

			if (DrunkWorldGen) {

				minX = (int)(vector.X - num2 * 0.5);
				maxX = (int)(vector.X + num2 * 0.5);

				if (num4 == 1) {
					minX = maxX - 3;
				}
				else {
					maxX = minX + 3;
				}

				for (int num58 = minX; num58 < maxX; num58++)
					for (int num59 = minY; num59 < maxY + 1; num59++)
						Neo.SetTile(num58, num59, tileType, ref undo);
					
			}

			vector.X -= (float)num2 * 0.6f * num4;
			vector.Y += (float)num3 * 0.5f;

			num2 = 15.0;
			num3 = 3.0;

			vector.Y -= (float)num3 * 0.5f;

			minX = (int)(vector.X - num2 * 0.5);
			maxX = (int)(vector.X + num2 * 0.5);
			minY = (int)(vector.Y - num3 * 0.5);
			maxY = (int)(vector.Y + num3 * 0.5);

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			for (int num60 = minX; num60 < maxX; num60++)
				for (int num61 = minY; num61 < maxY; num61++)
					Neo.SetActive(num60, num61, false, ref undo);

			if (num4 < 0)
				vector.X -= 1f;
			
			PlaceTile((int)vector.X, (int)vector.Y + 1, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, 13);

		}
		
	}

}
