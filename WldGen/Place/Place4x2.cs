using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place4x2(int x, int y, ushort type, ref UndoStep undo, int direction = -1, int style = 0) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			Point[] points = new Point[8];
			int k = 0;

			for (int i = x - 1; i < x + 3; i++) {

				for (int j = y - 1; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k++] = new Point(i, j);

				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!WorldGen.SolidTile2(i, y + 1))
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			short frameX = 0;
			
			if (direction == 1)
				frameX = 72;
			
			int frameY = 36 * style;

			undo.Add(new ChangedTile(x - 1, y - 1));
			undo.Add(new ChangedTile(x, y - 1));
			undo.Add(new ChangedTile(x + 1, y - 1));
			undo.Add(new ChangedTile(x + 2, y - 1));
			undo.Add(new ChangedTile(x - 1, y));
			undo.Add(new ChangedTile(x, y));
			undo.Add(new ChangedTile(x + 1, y));
			undo.Add(new ChangedTile(x + 2, y));

			Main.tile[x - 1, y - 1].active(true);
			Main.tile[x - 1, y - 1].frameY = (short)frameY;
			Main.tile[x - 1, y - 1].frameX = frameX;
			Main.tile[x - 1, y - 1].type = type;

			Main.tile[x, y - 1].active(true);
			Main.tile[x, y - 1].frameY = (short)frameY;
			Main.tile[x, y - 1].frameX = (short)(18 + frameX);
			Main.tile[x, y - 1].type = type;

			Main.tile[x + 1, y - 1].active(true);
			Main.tile[x + 1, y - 1].frameY = (short)frameY;
			Main.tile[x + 1, y - 1].frameX = (short)(36 + frameX);
			Main.tile[x + 1, y - 1].type = type;

			Main.tile[x + 2, y - 1].active(true);
			Main.tile[x + 2, y - 1].frameY = (short)frameY;
			Main.tile[x + 2, y - 1].frameX = (short)(54 + frameX);
			Main.tile[x + 2, y - 1].type = type;

			Main.tile[x - 1, y].active(true);
			Main.tile[x - 1, y].frameY = (short)(frameY + 18);
			Main.tile[x - 1, y].frameX = frameX;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(true);
			Main.tile[x, y].frameY = (short)(frameY + 18);
			Main.tile[x, y].frameX = (short)(18 + frameX);
			Main.tile[x, y].type = type;

			Main.tile[x + 1, y].active(true);
			Main.tile[x + 1, y].frameY = (short)(frameY + 18);
			Main.tile[x + 1, y].frameX = (short)(36 + frameX);
			Main.tile[x + 1, y].type = type;

			Main.tile[x + 2, y].active(true);
			Main.tile[x + 2, y].frameY = (short)(frameY + 18);
			Main.tile[x + 2, y].frameX = (short)(54 + frameX);
			Main.tile[x + 2, y].type = type;

			return true;

		}

	}

}
