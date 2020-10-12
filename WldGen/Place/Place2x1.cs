using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 - TileCut

		public static bool Place2x1(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			if (Main.tile[x, y] == null)
				Main.tile[x, y] = new Tile();
			
			if (Main.tile[x + 1, y] == null)
				Main.tile[x + 1, y] = new Tile();
			
			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();
			
			if (Main.tile[x + 1, y + 1] == null)
				Main.tile[x + 1, y + 1] = new Tile();
			
			bool flag = false;

			if (type != TileID.DjinnLamp && type != TileID.PiggyBank && type != TileID.Bowls && WorldGen.SolidTile2(x, y + 1) && WorldGen.SolidTile2(x + 1, y + 1)) {
				flag = true;
			} else if ((type == TileID.DjinnLamp || type == TileID.PiggyBank || type == TileID.Bowls) && Main.tile[x, y + 1].active() && Main.tile[x + 1, y + 1].active() && Main.tileTable[Main.tile[x, y + 1].type] && Main.tileTable[Main.tile[x + 1, y + 1].type]) {
				flag = true;
			}

			if (flag) {

				if (!Neo.TileCut(new Point[] { new Point(x, y), new Point(x + 1, y) }))
					return false;

				undo.Add(new ChangedTile(x, y));
				undo.Add(new ChangedTile(x + 1, y));

				Main.tile[x, y].active(active: true);
				Main.tile[x, y].frameY = 0;
				Main.tile[x, y].frameX = (short)(36 * style);
				Main.tile[x, y].type = type;

				Main.tile[x + 1, y].active(active: true);
				Main.tile[x + 1, y].frameY = 0;
				Main.tile[x + 1, y].frameX = (short)(36 * style + 18);
				Main.tile[x + 1, y].type = type;

			}

			return flag;

		}

	}

}
