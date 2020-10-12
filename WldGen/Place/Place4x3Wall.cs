using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place4x3Wall(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int leftSide = x - 1;
			int top      = y - 1;

			Point[] points = new Point[12];
			int m = 0;

			for (int i = leftSide; i < leftSide + 4; i++) {

				for (int j = top; j < top + 3; j++) {

					if (Main.tile[i, j].wall == 0)
						return false;

					points[m++] = new Point(i, j);

				}

			}

			if (!Neo.TileCut(points))
				return false;

			int num3 = 0;
			int num4 = style * 54;

			for (int k = leftSide; k < leftSide + 4; k++) {

				for (int l = top; l < top + 3; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(active: true);
					Main.tile[k, l].type = type;
					Main.tile[k, l].frameX = (short)(num3 + 18 * (k - leftSide));
					Main.tile[k, l].frameY = (short)(num4 + 18 * (l - top));

				}

			}

			return true;

		}

	}

}
