using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/1/2020 Copy/Paste

		private static float MakeDungeon_Banners(int[] roomWall, float count, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			count = 840000f / Main.maxTilesX;

			for (int i = 0; i < count; i++) {

				int num  = genRand.Next(dMinX, dMaxX);
				int num2 = genRand.Next(dMinY, dMaxY);

				while (!Main.wallDungeon[Main.tile[num, num2].wall] || Main.tile[num, num2].active()) {

					num  = genRand.Next(dMinX, dMaxX);
					num2 = genRand.Next(dMinY, dMaxY);

				}

				while (!WorldGen.SolidTile(num, num2) && num2 > 10)
					num2--;

				num2++;

				if (!Main.wallDungeon[Main.tile[num, num2].wall] || Main.tile[num, num2 - 1].type == TileID.Spikes || Main.tile[num, num2].active() || Main.tile[num, num2 + 1].active() || Main.tile[num, num2 + 2].active() || Main.tile[num, num2 + 3].active())
					continue;

				bool flag = true;

				for (int j = num - 1; j <= num + 1; j++)
					for (int k = num2; k <= num2 + 3; k++)
						if (Main.tile[j, k].active() && (Main.tile[j, k].type == TileID.ClosedDoor || Main.tile[j, k].type == TileID.OpenDoor || Main.tile[j, k].type == TileID.Banners))
							flag = false;

				if (flag) {

					int num3 = 10;

					if (Main.tile[num, num2].wall == roomWall[1])
						num3 = 12;

					if (Main.tile[num, num2].wall == roomWall[2])
						num3 = 14;

					num3 += genRand.Next(2);

					PlaceTile(num, num2, TileID.Banners, ref undo, mute: true, forced: false, -1, num3);

				}

			}

			return count;

		}

	}

}
