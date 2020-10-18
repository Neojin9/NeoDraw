using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/27/2020 Copy/Paste

		public static bool PlacePumpkin(int x, int y, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			ushort type  = TileID.Pumpkins;
			short frameY = (short)(genRand.Next(6) * 36);

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			Point[] points = new Point[4];
			int k = 0;

			for (int i = x - 1; i < x + 1; i++) {

				for (int j = y - 1; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k++] = new Point(i, j);

				}

				if (!WorldGen.SolidTile(i, y + 1) || (Main.tile[i, y + 1].type != TileID.Grass && Main.tile[i, y + 1].type != TileID.HallowedGrass))
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			undo.Add(new ChangedTile(x - 1, y - 1));
			undo.Add(new ChangedTile(x,     y - 1));
			undo.Add(new ChangedTile(x - 1, y    ));
			undo.Add(new ChangedTile(x,     y    ));

			Main.tile[x - 1, y - 1].active(true);
			Main.tile[x - 1, y - 1].frameY = frameY;
			Main.tile[x - 1, y - 1].frameX = 0;
			Main.tile[x - 1, y - 1].type = type;

			Main.tile[x, y - 1].active(true);
			Main.tile[x, y - 1].frameY = frameY;
			Main.tile[x, y - 1].frameX = 18;
			Main.tile[x, y - 1].type = type;

			Main.tile[x - 1, y].active(true);
			Main.tile[x - 1, y].frameY = (short)(frameY + 18);
			Main.tile[x - 1, y].frameX = 0;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(true);
			Main.tile[x, y].frameY = (short)(frameY + 18);
			Main.tile[x, y].frameX = 18;
			Main.tile[x, y].type = type;

			return true;

		}

	}

}
