using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020

		public static bool PlaceDoor(int i, int j, int type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(i, j))
				return false;

			UnifiedRandom genRand = WorldGen.genRand;

			int num  = style / 36;
			int num2 = style % 36;
			int num3 = 54 * num;
			int num4 = 54 * num2;

			try {

				if (Main.tile[i, j - 2].nactive() && Main.tileSolid[Main.tile[i, j - 2].type] && WorldGen.SolidTile(i, j + 2)) {

					undo.Add(new ChangedTile(i, j - 1));
					undo.Add(new ChangedTile(i, j    ));
					undo.Add(new ChangedTile(i, j + 1));

					Main.tile[i, j - 1].active(active: true);
					Main.tile[i, j - 1].type = 10;
					Main.tile[i, j - 1].frameY = (short)num4;
					Main.tile[i, j - 1].frameX = (short)(num3 + genRand.Next(3) * 18);

					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = 10;
					Main.tile[i, j].frameY = (short)(num4 + 18);
					Main.tile[i, j].frameX = (short)(num3 + genRand.Next(3) * 18);

					Main.tile[i, j + 1].active(active: true);
					Main.tile[i, j + 1].type = 10;
					Main.tile[i, j + 1].frameY = (short)(num4 + 36);
					Main.tile[i, j + 1].frameX = (short)(num3 + genRand.Next(3) * 18);

					return true;

				}

				return false;

			} catch {
				return false;
			}

		}

	}

}
