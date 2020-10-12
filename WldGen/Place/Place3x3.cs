using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place3x3(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int num = 0;

			Point[] points = new Point[9];
			int n = 0;

			if (type == 106 || type == 212 || type == 219 || type == 220 || type == 228 || type == 231 || 
				type == 243 || type == 247 || type == 283 || (type >= 300 && type <= 308) || type == 354 || 
				type == 355 || type == 406) {
				
				num = -2;
				
				for (int i = x - 1; i < x + 2; i++) {

					for (int j = y - 2; j < y + 1; j++) {

						if (Main.tile[i, j] == null)
							Main.tile[i, j] = new Tile();

						points[n++] = new Point(i, j);

					}

				}

				for (int k = x - 1; k < x + 2; k++) {

					if (Main.tile[k, y + 1] == null)
						Main.tile[k, y + 1] = new Tile();
					
					if (!WorldGen.SolidTile2(k, y + 1))
						return false;
					
				}

			} else {

				for (int l = x - 1; l < x + 2; l++) {

					for (int m = y; m < y + 3; m++) {

						if (Main.tile[l, m] == null)
							Main.tile[l, m] = new Tile();

						points[n++] = new Point(l, m);

					}

				}

				if (Main.tile[x, y - 1] == null)
					Main.tile[x, y - 1] = new Tile();
				
				if (!Main.tile[x, y - 1].nactive() || !Main.tileSolid[Main.tile[x, y - 1].type] || Main.tileSolidTop[Main.tile[x, y - 1].type])
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			int num2 = style * 18 * 3;

			undo.Add(new ChangedTile(x - 1, y + num    ));
			undo.Add(new ChangedTile(x,     y + num    ));
			undo.Add(new ChangedTile(x + 1, y + num    ));
			undo.Add(new ChangedTile(x - 1, y + 1 + num));
			undo.Add(new ChangedTile(x,     y + 1 + num));
			undo.Add(new ChangedTile(x + 1, y + 1 + num));
			undo.Add(new ChangedTile(x - 1, y + 2 + num));
			undo.Add(new ChangedTile(x,     y + 2 + num));
			undo.Add(new ChangedTile(x + 1, y + 2 + num));

			Main.tile[x - 1, y + num].active(active: true);
			Main.tile[x - 1, y + num].frameY = 0;
			Main.tile[x - 1, y + num].frameX = (short)num2;
			Main.tile[x - 1, y + num].type = type;

			Main.tile[x, y + num].active(active: true);
			Main.tile[x, y + num].frameY = 0;
			Main.tile[x, y + num].frameX = (short)(num2 + 18);
			Main.tile[x, y + num].type = type;

			Main.tile[x + 1, y + num].active(active: true);
			Main.tile[x + 1, y + num].frameY = 0;
			Main.tile[x + 1, y + num].frameX = (short)(num2 + 36);
			Main.tile[x + 1, y + num].type = type;

			Main.tile[x - 1, y + 1 + num].active(active: true);
			Main.tile[x - 1, y + 1 + num].frameY = 18;
			Main.tile[x - 1, y + 1 + num].frameX = (short)num2;
			Main.tile[x - 1, y + 1 + num].type = type;

			Main.tile[x, y + 1 + num].active(active: true);
			Main.tile[x, y + 1 + num].frameY = 18;
			Main.tile[x, y + 1 + num].frameX = (short)(num2 + 18);
			Main.tile[x, y + 1 + num].type = type;

			Main.tile[x + 1, y + 1 + num].active(active: true);
			Main.tile[x + 1, y + 1 + num].frameY = 18;
			Main.tile[x + 1, y + 1 + num].frameX = (short)(num2 + 36);
			Main.tile[x + 1, y + 1 + num].type = type;

			Main.tile[x - 1, y + 2 + num].active(active: true);
			Main.tile[x - 1, y + 2 + num].frameY = 36;
			Main.tile[x - 1, y + 2 + num].frameX = (short)num2;
			Main.tile[x - 1, y + 2 + num].type = type;

			Main.tile[x, y + 2 + num].active(active: true);
			Main.tile[x, y + 2 + num].frameY = 36;
			Main.tile[x, y + 2 + num].frameX = (short)(num2 + 18);
			Main.tile[x, y + 2 + num].type = type;

			Main.tile[x + 1, y + 2 + num].active(active: true);
			Main.tile[x + 1, y + 2 + num].frameY = 36;
			Main.tile[x + 1, y + 2 + num].frameX = (short)(num2 + 36);
			Main.tile[x + 1, y + 2 + num].type = type;

			return true;

		}

	}

}
