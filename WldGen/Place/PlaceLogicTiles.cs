using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/27/2020 Copy/Paste TileCut

		public static bool PlaceLogicTiles(int x, int y, int type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Tile tile = Main.tile[x, y];

			if (Main.tile[x, y] == null) {
				tile = new Tile();
				Main.tile[x, y] = tile;
			}

			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();

			if (!Neo.TileCut(x, y))
				return false;

			if (type == TileID.LogicGateLamp) {

				if (Main.tile[x, y + 1].active() && (Main.tile[x, y + 1].type == TileID.LogicGateLamp || Main.tile[x, y + 1].type == TileID.LogicGate)) {

					undo.Add(new ChangedTile(x, y));

					tile.active(true);
					tile.type = (ushort)type;
					tile.frameX = (short)(style * 18);
					tile.frameY = 0;

					return true;

				}

			} else if (!tile.active()) {

				undo.Add(new ChangedTile(x, y));

				tile.active(true);
				tile.type = (ushort)type;

				if (type == TileID.PixelBox && style == 1) {
					tile.frameX = 18;
					tile.frameY = 0;
				}
				else if (type == TileID.WirePipe) {
					tile.frameX = (short)(18 * style);
					tile.frameY = 0;
                }
				else {
					tile.frameX = 0;
					tile.frameY = (short)(18 * style);
				}

				return true;

			}

			return false;

		}

	}

}
