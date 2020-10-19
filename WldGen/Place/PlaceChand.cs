using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020 TileCut

		public static bool PlaceChand(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Point[] points = (type == 454 ? new Point[12] : new Point[9]);
			int k = 0;

            int xOffset = (type == 454 ? 2 : 1);

			for (int i = x - xOffset; i < x + 2; i++) {
				for (int j = y; j < y + 3; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k++] = new Point(i, j);

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

            for (int j = 0; j < 3; j++) {

                if (type == 454)
                    undo.Add(new ChangedTile(x - 2, y + j));

                undo.Add(new ChangedTile(x - 1, y + j));
                undo.Add(new ChangedTile(x,     y + j));
                undo.Add(new ChangedTile(x + 1, y + j));

			}

            if (type == 454)
                num2 += 18;

            for (int l = 0; l < 3; l++) {

                if (type == 454) {
                    Main.tile[x - 2, y + l].active(true);
                    Main.tile[x - 2, y + l].frameY = (short)(num3 + 18 * l);
                    Main.tile[x - 2, y + l].frameX = (short)(num2 - 18);
                    Main.tile[x - 2, y + l].type   = type;
                }

				Main.tile[x - 1, y + l].active(true);
                Main.tile[x - 1, y + l].frameY = (short)(num3 + 18 * l);
                Main.tile[x - 1, y + l].frameX = (short)num2;
                Main.tile[x - 1, y + l].type   = type;

                Main.tile[x, y + l].active(true);
                Main.tile[x, y + l].frameY = (short)(num3 + 18 * l);
				Main.tile[x, y + l].frameX = (short)(num2 + 18);
                Main.tile[x, y + l].type   = type;

                Main.tile[x + 1, y + l].active(true);
                Main.tile[x + 1, y + l].frameY = (short)(num3 + 18 * l);
				Main.tile[x + 1, y + l].frameX = (short)(num2 + 36);
                Main.tile[x + 1, y + l].type   = type;

			}

			return true;

		}

	}

}
