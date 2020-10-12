using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place2x2Style(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			short frameY = 0;

			if (type == TileID.Pumpkins) {
				frameY = (short)(x % 12 / 2);
				frameY = (short)(frameY * 36);
			}

			Point[] points = new Point[4];
			int k = 0;

			for (int i = x - 1; i < x + 1; i++) {

				for (int j = y - 1; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k++] = new Point(i, j);

				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!WorldGen.SolidTile(i, y + 1))
					return false;
				
				if (type == TileID.Pumpkins && Main.tile[i, y + 1].type != TileID.Grass && Main.tile[i, y + 1].type != TileID.HallowedGrass)
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			undo.Add(new ChangedTile(x - 1, y - 1));
			undo.Add(new ChangedTile(x,     y - 1));
			undo.Add(new ChangedTile(x - 1, y    ));
			undo.Add(new ChangedTile(x,     y    ));

			short num2 = (short)(36 * style);

			Main.tile[x - 1, y - 1].active(active: true);
			Main.tile[x - 1, y - 1].frameY = frameY;
			Main.tile[x - 1, y - 1].frameX = num2;
			Main.tile[x - 1, y - 1].type = type;

			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].frameY = frameY;
			Main.tile[x, y - 1].frameX = (short)(num2 + 18);
			Main.tile[x, y - 1].type = type;

			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].frameY = (short)(frameY + 18);
			Main.tile[x - 1, y].frameX = num2;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = (short)(frameY + 18);
			Main.tile[x, y].frameX = (short)(num2 + 18);
			Main.tile[x, y].type = type;

			return true;

		}

	}

}
