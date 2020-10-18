using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/9/2020 Copy/Paste TileCut

		public static bool PlaceXmasTree(int x, int y, ref UndoStep undo, ushort type = TileID.ChristmasTree) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int leftSide  = x - 1;
			int top       = y - 7;

			Point[] points = new Point[32];
			int m = 0;

			for (int i = leftSide; i < leftSide + 4; i++) {

				for (int j = top; j < top + 8; j++)
					points[m++] = new Point(i, j);

				if (i > leftSide && i < leftSide + 3 && !WorldGen.SolidTile(i, top + 8))
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			int num3 = 0;

			for (int k = leftSide; k < leftSide + 4; k++) {

				int num4 = 0;

				for (int l = top; l < top + 8; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(active: true);

					if (num3 == 0 && num4 == 0) {
						Main.tile[k, l].frameX = 10;
						Main.tile[k, l].frameY = 0;
					} else {
						Main.tile[k, l].frameX = (short)num3;
						Main.tile[k, l].frameY = (short)num4;
					}

					Main.tile[k, l].type = type;
					Main.tile[k, l].active(true);

					num4++;

				}

				num3++;

			}

			return true;

		}

	}

}
