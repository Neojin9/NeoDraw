using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020 TileCut

		public static bool PlaceChand(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Point[] points = (type == TileID.Pigronata ? new Point[12] : new Point[9]);
			int k = 0;

            int xOffset = (type == TileID.Pigronata ? 2 : 1);

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

			int frameX = style / 36 * 108;
			int frameY = style * 18 * 3;

            for (int j = 0; j < 3; j++) {

                if (type == TileID.Pigronata)
                    undo.Add(new ChangedTile(x - 2, y + j));

                undo.Add(new ChangedTile(x - 1, y + j));
                undo.Add(new ChangedTile(x,     y + j));
                undo.Add(new ChangedTile(x + 1, y + j));

			}

            if (type == TileID.Pigronata) {
                frameX += 18;
            }
			else if (type == TileID.Chandeliers && Main.keyState.PressingAlt()) {
				frameX += 54;
            }

            for (int l = 0; l < 3; l++) {

                if (type == TileID.Pigronata) {

                    Main.tile[x - 2, y + l].active(true);
                    Main.tile[x - 2, y + l].frameY = (short)(frameY + 18 * l);
                    Main.tile[x - 2, y + l].frameX = (short)(frameX - 18);
                    Main.tile[x - 2, y + l].type   = type;

                }

				Main.tile[x - 1, y + l].active(true);
                Main.tile[x - 1, y + l].frameY = (short)(frameY + 18 * l);
                Main.tile[x - 1, y + l].frameX = (short)frameX;
                Main.tile[x - 1, y + l].type   = type;

                Main.tile[x, y + l].active(true);
                Main.tile[x, y + l].frameY = (short)(frameY + 18 * l);
				Main.tile[x, y + l].frameX = (short)(frameX + 18);
                Main.tile[x, y + l].type   = type;

                Main.tile[x + 1, y + l].active(true);
                Main.tile[x + 1, y + l].frameY = (short)(frameY + 18 * l);
				Main.tile[x + 1, y + l].frameX = (short)(frameX + 36);
                Main.tile[x + 1, y + l].type   = type;

			}

			return true;

		}

	}

}
