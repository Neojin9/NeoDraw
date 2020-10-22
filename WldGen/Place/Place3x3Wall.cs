using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place3x3Wall(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int leftSide = x - 1;
			int top = y - 1;

			Point[] points = new Point[9];
			int m = 0;

			for (int i = leftSide; i < leftSide + 3; i++) {

				for (int j = top; j < top + 3; j++) {

					points[m++] = new Point(i, j);

					if (Main.tile[i, j].wall == 0)
						return false;

				}

			}

			if (!Neo.TileCut(points))
				return false;

			int num3 = 0;

			while (style > 35) {
				num3++;
				style -= 36;
			}

			if (type == 440) {

				if (style > 6) {

					style -= 7;
					num3 = 1;

				}

			}

			int frameX = style * 54;
			int num5 = num3 * 54;

			for (int k = leftSide; k < leftSide + 3; k++) {

				for (int l = top; l < top + 3; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(true);
					Main.tile[k, l].type = type;
					Main.tile[k, l].frameX = (short)(frameX + 18 * (k - leftSide));
					Main.tile[k, l].frameY = (short)(num5 + 18 * (l - top));

				}

			}

			return true;

		}

	}

}
