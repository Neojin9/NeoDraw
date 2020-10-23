using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020

		public static bool PlaceDoor(int i, int j, int type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(i, j))
				return false;

			UnifiedRandom genRand = WorldGen.genRand;

			int column = style / 36; // Style Wrap Limit
			int row    = style % 36;
			int frameX = 54 * column;
			int frameY = 54 * row;

			try {

				if (Main.tile[i, j - 2].nactive() && Main.tileSolid[Main.tile[i, j - 2].type] && WorldGen.SolidTile(i, j + 2)) {

					undo.Add(new ChangedTile(i, j - 1));
					undo.Add(new ChangedTile(i, j    ));
					undo.Add(new ChangedTile(i, j + 1));

					Main.tile[i, j - 1].active(true);
					Main.tile[i, j - 1].type = 10;
					Main.tile[i, j - 1].frameY = (short)frameY;
					Main.tile[i, j - 1].frameX = (short)(frameX + genRand.Next(3) * 18);

					Main.tile[i, j].active(true);
					Main.tile[i, j].type = 10;
					Main.tile[i, j].frameY = (short)(frameY + 18);
					Main.tile[i, j].frameX = (short)(frameX + genRand.Next(3) * 18);

					Main.tile[i, j + 1].active(true);
					Main.tile[i, j + 1].type = 10;
					Main.tile[i, j + 1].frameY = (short)(frameY + 36);
					Main.tile[i, j + 1].frameX = (short)(frameX + genRand.Next(3) * 18);

					return true;

				}

				return false;

			} catch {
				return false;
			}

		}

	}

}
