using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/27/2020 Copy/Paste

		public static bool PlaceMB(int X, int y, ushort type, int style, ref UndoStep undo) {

			int num = X + 1;

			if (!WorldGen.InWorld(X + 1, y, 5))
				return false;

			Point[] points = new Point[4];
			int k = 0;

			for (int i = num - 1; i < num + 1; i++) {

				for (int j = y - 1; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k++] = new Point(i, j);

				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!Main.tile[i, y + 1].active() || Main.tile[i, y + 1].halfBrick() || (!Main.tileSolid[Main.tile[i, y + 1].type] && !Main.tileTable[Main.tile[i, y + 1].type]))
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			undo.Add(new ChangedTile(num - 1, y - 1));
			undo.Add(new ChangedTile(num,     y - 1));
			undo.Add(new ChangedTile(num - 1, y    ));
			undo.Add(new ChangedTile(num,     y    ));

			int frameX = 0;
			int frameY = style * 36;

			if (type == TileID.Jackolanterns && Main.keyState.PressingAlt())
				frameX += 36;

			Main.tile[num - 1, y - 1].active(true);
			Main.tile[num - 1, y - 1].frameX = (short)(frameX);
			Main.tile[num - 1, y - 1].frameY = (short)(frameY);
			Main.tile[num - 1, y - 1].type = type;

			Main.tile[num, y - 1].active(true);
			Main.tile[num, y - 1].frameX = (short)(frameX + 18);
			Main.tile[num, y - 1].frameY = (short)(frameY);
			Main.tile[num, y - 1].type = type;

			Main.tile[num - 1, y].active(true);
			Main.tile[num - 1, y].frameX = (short)(frameX);
			Main.tile[num - 1, y].frameY = (short)(frameY + 18);
			Main.tile[num - 1, y].type = type;

			Main.tile[num, y].active(true);
			Main.tile[num, y].frameX = (short)(frameX + 18);
			Main.tile[num, y].frameY = (short)(frameY + 18);
			Main.tile[num, y].type = type;

			return true;

		}

	}

}
