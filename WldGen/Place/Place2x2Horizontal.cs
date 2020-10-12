using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place2x2Horizontal(int x, int y, ushort type, ref UndoStep undo, int Style = 0) {

			int left   = x - 2;
			int right  = x + 3;
			int top    = y - 2;
			int bottom = y + 3;

			if (left < 0 || top < 0)
				return false;
			
			if (right > Main.maxTilesX || bottom > Main.maxTilesY)
				return false;
			
			for (int i = left; i < right; i++)
				for (int j = top; j < bottom; j++)
					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

            if (WorldGen.SolidTile2(x, y + 1) && WorldGen.SolidTile2(x + 1, y + 1)) {

				y--;

				Point[] tiles = new Point[4];
				int o = 0;

				for (int m = 0; m < 2; m++)
					for (int n = 0; n < 2; n++)
						tiles[o++] = new Point(x + m, y + n);

				if (!Neo.TileCut(tiles))
					return false;

				int frameX = 36 * Style;

				for (int k = 0; k < 2; k++) {

					for (int l = 0; l < 2; l++) {

						undo.Add(new ChangedTile(x + k, y + l));

                        Main.tile[x + k, y + l].active(active: true);
                        Main.tile[x + k, y + l].type = type;
                        Main.tile[x + k, y + l].frameX = (short)(frameX + 18 * k);
                        Main.tile[x + k, y + l].frameY = (short)(18 * l);

					}

				}

				return true;

			}

			return false;

		}

	}

}
