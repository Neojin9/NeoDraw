using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place2x3Wall(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int top = y - 1;

			Point[] points = new Point[6];
			int m = 0;

			for (int i = x; i < x + 2; i++) {

				for (int j = top; j < top + 3; j++) {

					points[m] = new Point(i, j);

					if (Main.tile[i, j].wall == 0)
						return false;

				}

			}

			if (!Neo.TileCut(points))
				return false;

			int frameX = style * 36;
			int frameY = 0;

			for (int k = x; k < x + 2; k++) {

				for (int l = top; l < top + 3; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(true);
					Main.tile[k, l].type   = type;
					Main.tile[k, l].frameX = (short)(frameX + 18 * (k - x));
					Main.tile[k, l].frameY = (short)(frameY + 18 * (l - top));

				}

			}

			return true;

		}

	}

}
