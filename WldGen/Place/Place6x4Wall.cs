using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place6x4Wall(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int leftSide = x - 2;
			int top      = y - 2;

			Point[] points = new Point[24];
			int m = 0;

			for (int i = leftSide; i < leftSide + 6; i++) {

				for (int j = top; j < top + 4; j++) {

					if (Main.tile[i, j].wall == 0)
						return false;

					points[m++] = new Point(i, j);

				}

			}

			if (!Neo.TileCut(points))
				return false;

            int frameX = style / 27 * 108;
			int frameY = style % 27 * 72;

			for (int k = leftSide; k < leftSide + 6; k++) {

				for (int l = top; l < top + 4; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(true);

					Main.tile[k, l].type   = type;
					Main.tile[k, l].frameX = (short)(frameX + 18 * (k - leftSide));
					Main.tile[k, l].frameY = (short)(frameY + 18 * (l - top));

				}

			}

			return true;

		}

	}

}
