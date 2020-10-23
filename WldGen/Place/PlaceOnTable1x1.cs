using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/27/2020 Copy/Paste Modified TileCut

		public static bool PlaceOnTable1x1(int x, int y, int type, ref UndoStep undo, int style = 0, int bookSubStyle = -1) {

			if (!WorldGen.InWorld(x, y))
				return false;

			bool flag = false;

			if (Main.tile[x, y] == null)
				Main.tile[x, y] = new Tile();
			
			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();
			
			if (Main.tile[x, y + 1].nactive() && Main.tileTable[Main.tile[x, y + 1].type])
				flag = true;

			if (type == TileID.ClayPot && Main.tile[x, y + 1].nactive() && Main.tileSolid[Main.tile[x, y + 1].type] && !Main.tile[x, y + 1].halfBrick() && Main.tile[x, y + 1].slope() == 0)
				flag = true;

			if (flag) {

				if (!Neo.TileCut(x, y))
					return false;

				undo.Add(new ChangedTile(x, y));

				Main.tile[x, y].active(true);
				Main.tile[x, y].type = (ushort)type;

				if (type == TileID.Candles /*|| type == TileID.WaterCandle || type == TileID.PeaceCandle*/ || type == TileID.PlatinumCandle) { // TODO: Uncomment for v1.4
					Main.tile[x, y].frameX = (short)(Main.keyState.PressingAlt() ? 18 : 0);
					Main.tile[x, y].frameY = (short)(style * 22);
				} else {
					Main.tile[x, y].frameX = (short)(style * 18);
					Main.tile[x, y].frameY = 0;
				}

			}

			return flag;

		}

	}

}
