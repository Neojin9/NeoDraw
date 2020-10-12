using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Update v1.4 7/26/2020 Copy/Paste Modified

		public static bool PlaceSmallPile(int i, int j, int X, int Y, ref UndoStep undo, ushort type = TileID.SmallPiles) {

			if (!WorldGen.InWorld(i, j))
				return false;

			short frameY = (short)(Y * 18);
			short frameX = (short)(X * 18);

			if (Main.tile[i, j] == null)
				Main.tile[i, j] = new Tile();
			
			if (Main.tile[i + 1, j] == null)
				Main.tile[i + 1, j] = new Tile();
			
			if (Main.tile[i, j + 1] == null)
				Main.tile[i, j + 1] = new Tile();
			
			if (Main.tile[i + 1, j + 1] == null)
				Main.tile[i + 1, j + 1] = new Tile();
			
			if (Main.tile[i, j].lava())
				return false;
			
			if (Y == 1 || Y == 3) {

				frameX = (short)(X * 36);

				if (!WorldGen.SolidTile2(i, j + 1) || !WorldGen.SolidTile2(i + 1, j + 1))
					return false;

				if (!Neo.TileCut(new Point[] { new Point(i, j), new Point(i + 1, j) }))
					return false;

				undo.Add(new ChangedTile(i, j));
				undo.Add(new ChangedTile(i + 1, j));

				Main.tile[i, j].active(active: true);
				Main.tile[i, j].frameY = frameY;
				Main.tile[i, j].frameX = frameX;
				Main.tile[i, j].type = type;

				Main.tile[i + 1, j].active(active: true);
				Main.tile[i + 1, j].frameY = frameY;
				Main.tile[i + 1, j].frameX = (short)(frameX + 18);
				Main.tile[i + 1, j].type = type;

				return true;

			}
			else if (WorldGen.SolidTile2(i, j + 1)) {

				if (!Neo.TileCut(i, j))
					return false;

				undo.Add(new ChangedTile(i, j));

				Main.tile[i, j].active(active: true);
				Main.tile[i, j].frameY = frameY;
				Main.tile[i, j].frameX = frameX;
				Main.tile[i, j].type = type;

				return true;

			}

			return false;

		}

	}

}
