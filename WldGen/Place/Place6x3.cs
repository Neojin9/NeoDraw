using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place6x3(int x, int y, ushort type, ref UndoStep undo, int direction = -1, int style = 0) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			Point[] points = new Point[18];
			int m = 0;

			for (int i = x - 3; i < x + 3; i++) {

				for (int j = y - 2; j <= y; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[m++] = new Point(i, j);

				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!WorldGen.SolidTile2(i, y + 1) && (!Main.tile[i, y + 1].nactive() || !Main.tileSolidTop[Main.tile[i, y + 1].type] || Main.tile[i, y + 1].frameY != 0))
					return false;

			}

			if (!Neo.TileCut(points))
				return false;

			int frameX = 0;

			for (int k = x - 3; k < x + 3; k++) {

				int frameY = 0;
				
				for (int l = y - 2; l <= y; l++) {

					undo.Add(new ChangedTile(k, l));

					Main.tile[k, l].active(true);

					Main.tile[k, l].type   = type;
					Main.tile[k, l].frameX = (short)frameX;
					Main.tile[k, l].frameY = (short)frameY;

					frameY += 18;

				}

				frameX += 18;

			}

			return true;

		}

	}

}
