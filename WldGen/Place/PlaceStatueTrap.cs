using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/9/2020 Copy/Paste TileCut

		public static void PlaceStatueTrap(int x, int y, ref UndoStep undo) {

			for (int i = -10; i <= 10; i++) {

				for (int j = -10; j <= 10; j++) {

					Tile tile     = Main.tile[x + i, y + j];
					Tile testTile = Main.tile[x + i, y + j + 1];

					if (WorldGen.SolidTile2(testTile) && Neo.TileCut(x + i, y + j)) {

						PlaceTile(x + i, y + j, TileID.PressurePlates, ref undo, true);
						
						if (tile.active() && tile.type == TileID.PressurePlates) {
							WldUtils.WldUtils.WireLine(new Point(x, y), new Point(x + i, y + j), ref undo);
							return;
						}

					}

				}

			}

		}

	}

}
