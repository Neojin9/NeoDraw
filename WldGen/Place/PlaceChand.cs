using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020 TileCut

		public static bool PlaceChand(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Point[] points = new Point[12];
			int k = 0;

			for (int i = x - 1; i < x + 2; i++) {
				for (int j = y; j < y + 3; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k] = new Point(i, j);

				}
			}

			if (Main.tile[x, y - 1] == null)
				Main.tile[x, y - 1] = new Tile();
			
			if (!Main.tile[x, y - 1].nactive() || !Main.tileSolid[Main.tile[x, y - 1].type] || Main.tileSolidTop[Main.tile[x, y - 1].type])
				return false;

			if (!Neo.TileCut(points))
				return false;

			int num2 = style / 36 * 108;
			int num3 = style * 18 * 3;

			undo.Add(new ChangedTile(x - 1, y    ));
			undo.Add(new ChangedTile(x,     y    ));
			undo.Add(new ChangedTile(x + 1, y    ));
			undo.Add(new ChangedTile(x - 1, y + 1));
			undo.Add(new ChangedTile(x,     y + 1));
			undo.Add(new ChangedTile(x + 1, y + 1));
			undo.Add(new ChangedTile(x - 1, y + 2));
			undo.Add(new ChangedTile(x,     y + 2));
			undo.Add(new ChangedTile(x + 1, y + 2));

			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].frameY = (short)num3;
			Main.tile[x - 1, y].frameX = (short)num2;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = (short)num3;
			Main.tile[x, y].frameX = (short)(num2 + 18);
			Main.tile[x, y].type = type;

			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = (short)num3;
			Main.tile[x + 1, y].frameX = (short)(num2 + 36);
			Main.tile[x + 1, y].type = type;

			Main.tile[x - 1, y + 1].active(active: true);
			Main.tile[x - 1, y + 1].frameY = (short)(num3 + 18);
			Main.tile[x - 1, y + 1].frameX = (short)num2;
			Main.tile[x - 1, y + 1].type = type;

			Main.tile[x, y + 1].active(active: true);
			Main.tile[x, y + 1].frameY = (short)(num3 + 18);
			Main.tile[x, y + 1].frameX = (short)(num2 + 18);
			Main.tile[x, y + 1].type = type;

			Main.tile[x + 1, y + 1].active(active: true);
			Main.tile[x + 1, y + 1].frameY = (short)(num3 + 18);
			Main.tile[x + 1, y + 1].frameX = (short)(num2 + 36);
			Main.tile[x + 1, y + 1].type = type;

			Main.tile[x - 1, y + 2].active(active: true);
			Main.tile[x - 1, y + 2].frameY = (short)(num3 + 36);
			Main.tile[x - 1, y + 2].frameX = (short)num2;
			Main.tile[x - 1, y + 2].type = type;

			Main.tile[x, y + 2].active(active: true);
			Main.tile[x, y + 2].frameY = (short)(num3 + 36);
			Main.tile[x, y + 2].frameX = (short)(num2 + 18);
			Main.tile[x, y + 2].type = type;

			Main.tile[x + 1, y + 2].active(active: true);
			Main.tile[x + 1, y + 2].frameY = (short)(num3 + 36);
			Main.tile[x + 1, y + 2].frameX = (short)(num2 + 36);
			Main.tile[x + 1, y + 2].type = type;

			return true;

		}

	}

}
