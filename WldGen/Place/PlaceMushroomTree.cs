using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer {

		public static bool PlaceMushroomTree(int x, int y, ref UndoStep undo) { // GrowShroom

			UnifiedRandom genRand = WorldGen.genRand;

			if (!WorldGen.InWorld(x, y))
				return false;

			while (
				y < Main.maxTilesY &&
				(
					!Main.tile[x, y].active() ||
					(
						Main.tile[x, y].active() &&
						Main.tileCut[Main.tile[x, y].type]
					)
				)
			)
				y++;

			if (Main.tile[x - 1, y - 1].lava() || Main.tile[x - 1, y - 1].lava() || Main.tile[x + 1, y - 1].lava())
				return false;
			
			if (
				Main.tile[x, y].nactive()      &&
				!Main.tile[x, y].halfBrick()   &&
				Main.tile[x, y].slope() == 0   &&
				Main.tile[x, y].type == 70     &&
				Main.tile[x - 1, y].active()   &&
				Main.tile[x - 1, y].type == 70 &&
				Main.tile[x + 1, y].active()   &&
				Main.tile[x + 1, y].type == 70 &&
				EmptyTileCheck(x - 2, x + 2, y - 13, y - 1, 71)) {

				int height = genRand.Next(4, 11);

				for (int j = y - height; j < y; j++) {

					undo.Add(new ChangedTile(x, j));

					Main.tile[x, j].frameNumber((byte)WorldGen.genRand.Next(3));
					Main.tile[x, j].active(active: true);
					Main.tile[x, j].type = 72;

					int styleY = genRand.Next(3);

					if (styleY == 0) {
						Main.tile[x, j].frameX = 0;
						Main.tile[x, j].frameY = 0;
					}

					if (styleY == 1) {
						Main.tile[x, j].frameX = 0;
						Main.tile[x, j].frameY = 18;
					}

					if (styleY == 2) {
						Main.tile[x, j].frameX = 0;
						Main.tile[x, j].frameY = 36;
					}

				}

				int styleX = genRand.Next(3);

				undo.Add(new ChangedTile(x, y - height));

				if (styleX == 0) {
					Main.tile[x, y - height].frameX = 36;
					Main.tile[x, y - height].frameY = 0;
				}

				if (styleX == 1) {
					Main.tile[x, y - height].frameX = 36;
					Main.tile[x, y - height].frameY = 18;
				}

				if (styleX == 2) {
					Main.tile[x, y - height].frameX = 36;
					Main.tile[x, y - height].frameY = 36;
				}

				WorldGen.RangeFrame(x - 2, y - height - 1, x + 2, y + 1);

				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendTileSquare(-1, x, (int)(y - height * 0.5), height + 1);
				
				return true;

			}

			return false;

		}

	}

}
