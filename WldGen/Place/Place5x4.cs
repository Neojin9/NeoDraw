using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place5x4(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			Point[] points = new Point[20];
			int l = 0;

			for (int i = x - 2; i < x + 3; i++) {

				for (int j = y - 3; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[l++] = new Point(i, j);

				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!WorldGen.SolidTile2(i, y + 1))
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			for (int k = -3; k <= 0; k++) {

				short frameY = (short)((3 + k) * 18);

				undo.Add(new ChangedTile(x - 2, y + k));
				undo.Add(new ChangedTile(x - 1, y + k));
				undo.Add(new ChangedTile(x, y + k));
				undo.Add(new ChangedTile(x + 1, y + k));
				undo.Add(new ChangedTile(x + 2, y + k));

				Main.tile[x - 2, y + k].active(true);
				Main.tile[x - 2, y + k].frameY = frameY;
				Main.tile[x - 2, y + k].frameX = 0;
				Main.tile[x - 2, y + k].type = type;

				Main.tile[x - 1, y + k].active(true);
				Main.tile[x - 1, y + k].frameY = frameY;
				Main.tile[x - 1, y + k].frameX = 18;
				Main.tile[x - 1, y + k].type = type;

				Main.tile[x, y + k].active(true);
				Main.tile[x, y + k].frameY = frameY;
				Main.tile[x, y + k].frameX = 36;
				Main.tile[x, y + k].type = type;

				Main.tile[x + 1, y + k].active(true);
				Main.tile[x + 1, y + k].frameY = frameY;
				Main.tile[x + 1, y + k].frameX = 54;
				Main.tile[x + 1, y + k].type = type;

				Main.tile[x + 2, y + k].active(true);
				Main.tile[x + 2, y + k].frameY = frameY;
				Main.tile[x + 2, y + k].frameX = 72;
				Main.tile[x + 2, y + k].type = type;

			}

			return true;

		}

	}

}
