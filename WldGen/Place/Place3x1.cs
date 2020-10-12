using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place3x1(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			for (int i = x - 1; i < x + 2; i++) {

				if (Main.tile[i, y] == null)
					Main.tile[i, y] = new Tile();
				
				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!WorldGen.SolidTile2(i, y + 1))
					return false;
				
			}

			if (!Neo.TileCut(new Point[] { new Point(x - 1, y), new Point(x, y), new Point(x + 1, y) }))
				return false;

			short num = (short)(54 * style);

			undo.Add(new ChangedTile(x - 1, y));
			undo.Add(new ChangedTile(x,     y));
			undo.Add(new ChangedTile(x + 1, y));

			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].frameY = 0;
			Main.tile[x - 1, y].frameX = num;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = 0;
			Main.tile[x, y].frameX = (short)(num + 18);
			Main.tile[x, y].type = type;

			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = 0;
			Main.tile[x + 1, y].frameX = (short)(num + 36);
			Main.tile[x + 1, y].type = type;

			return true;

		}

	}

}
