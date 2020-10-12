using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 7/31/2020 Copy/Paste

		private static void MakeDungeon_Traps(ref int failCount, int failMax, ref int numAdd, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			while (numAdd < Main.maxTilesX / 500) {

				failCount++;

				int curX = genRand.Next(dMinX, dMaxX);
				int curY = genRand.Next(dMinY, dMaxY);

				while (curY < Main.worldSurface)
					curY = genRand.Next(dMinY, dMaxY);

				if (Main.wallDungeon[Main.tile[curX, curY].wall] && placeTrap(curX, curY, ref undo, 0))
					failCount = failMax;

				if (failCount > failMax) {

					numAdd++;
					failCount = 0;

				}

			}

		}

	}

}
