using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/1/2020 Copy/Paste

		public static void DungeonStairs(int i, int j, ushort tileType, ushort wallType, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			Vector2 zero = Vector2.Zero;

			double num = genRand.Next(5, 9);

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			int num3 = genRand.Next(10, 30);
			int num2 = i <= dEnteranceX ? 1 : -1;

			if (i > Main.maxTilesX - 400) {
				num2 = -1;
			} else if (i < 400) {
				num2 = 1;
			}

			zero.Y = -1f;
			zero.X = num2;

			if (genRand.Next(3) != 0) {
				zero.X *= 1f + genRand.Next(0, 200) * 0.01f;
			} else if (genRand.Next(3) == 0) {
				zero.X *= genRand.Next(50, 76) * 0.01f;
			} else if (genRand.Next(6) == 0) {
				zero.Y *= 2f;
			}

			if (dungeonX < Main.maxTilesX / 2 && zero.X < 0f && zero.X < 0.5)
				zero.X = -0.5f;

			if (dungeonX > Main.maxTilesX / 2 && zero.X > 0f && zero.X > 0.5)
				zero.X = -0.5f;

			if (DrunkWorldGen) {
				num2 *= -1;
				zero.X *= -1f;
            }

			while (num3 > 0) {

				num3--;

				int minX = (int)(vector.X - num - 4.0 - genRand.Next(6));
				int maxX = (int)(vector.X + num + 4.0 + genRand.Next(6));
				int minY = (int)(vector.Y - num - 4.0);
				int maxY = (int)(vector.Y + num + 4.0 + genRand.Next(6));

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				int num8 = 1;

				if (vector.X > Main.maxTilesX / 2)
					num8 = -1;

				int num9  = (int)(vector.X + (float)dxStrength1 * 0.6f * num8 + (float)dxStrength2 * num8);
				int num10 = (int)(dyStrength2 * 0.5);

				if (vector.Y < Main.worldSurface - 5.0 && Main.tile[num9, (int)(vector.Y - num - 6.0 + num10)].wall == 0 && Main.tile[num9, (int)(vector.Y - num - 7.0 + num10)].wall == 0 && Main.tile[num9, (int)(vector.Y - num - 8.0 + num10)].wall == 0) {
					dSurface = true;
					TileRunner(num9, (int)(vector.Y - num - 6.0 + num10), genRand.Next(25, 35), genRand.Next(10, 20), -1, ref undo, false, addTile: false, 0f, -1f);
				}

				for (int k = minX; k < maxX; k++) {

					for (int l = minY; l < maxY; l++) {

						Neo.SetLiquid(k, l, 0, ref undo);

						if (!Main.wallDungeon[Main.tile[k, l].wall])
							Neo.SetTileWall(k, l, tileType, 0);

					}

				}

				for (int m = minX + 1; m < maxX - 1; m++)
					for (int n = minY + 1; n < maxY - 1; n++)
						Neo.SetWall(m, n, wallType, ref undo);

				int num11 = 0;

				if (genRand.Next((int)num) == 0)
					num11 = genRand.Next(1, 3);

				minX = (int)(vector.X - num * 0.5 - num11);
				maxX = (int)(vector.X + num * 0.5 + num11);
				minY = (int)(vector.Y - num * 0.5 - num11);
				maxY = (int)(vector.Y + num * 0.5 + num11);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				for (int num12 = minX; num12 < maxX; num12++) {

					for (int num13 = minY; num13 < maxY; num13++) {

						Neo.SetActive(num12, num13, false, ref undo);
						PlaceWall(num12, num13, wallType, ref undo, mute: true);

					}

				}

				if (dSurface)
					num3 = 0;

				vector += zero;

				if (vector.Y < Main.worldSurface)
					zero.Y *= 0.98f;

			}

			dungeonX = (int)vector.X;
			dungeonY = (int)vector.Y;

		}

	}

}
