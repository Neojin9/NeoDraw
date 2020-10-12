using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place3x2Wall(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int leftSide = x - 1;

			Point[] points = new Point[6];
			int m = 0;

			for (int i = leftSide; i < leftSide + 3; i++) {
				for (int j = y; j < y + 2; j++) {

					points[m++] = new Point(i, j);

					if (Main.tile[i, j].wall == 0)
						return false;

				}
			}

			if (!Neo.TileCut(points))
				return false;

			int num2 = 0;
			int num3 = style * 36;

			for (int k = leftSide; k < leftSide + 3; k++) {

				for (int l = y; l < y + 2; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(active: true);
					Main.tile[k, l].type = type;
					Main.tile[k, l].frameX = (short)(num2 + 18 * (k - leftSide));
					Main.tile[k, l].frameY = (short)(num3 + 18 * (l - y));

				}

			}

			return true;

		}

	}

}
