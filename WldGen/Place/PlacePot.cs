using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/26/2020 Copy/Paste Modified TileCut

		public static bool PlacePot(int x, int y, ref UndoStep undo, ushort type = TileID.Pots, int style = 0, int subStyleX = -1, int subStyleY = -1) {

			if (!WorldGen.InWorld(x, y))
				return false;

			UnifiedRandom genRand = WorldGen.genRand;

			Point[] points = new Point[4];
			int m = 0;

			for (int i = x; i < x + 2; i++) {

				for (int j = y - 1; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[m++] = new Point(i, j);
					
				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!Main.tile[i, y + 1].nactive() || Main.tile[i, y + 1].halfBrick() || Main.tile[i, y + 1].slope() != 0 || !Main.tileSolid[Main.tile[i, y + 1].type])
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			int styleX = (subStyleX == -1 ? genRand.Next(3) : subStyleX) * 36;
			int styleY = (subStyleY == -1 ? genRand.Next(3) : subStyleY) * 36;

			for (int k = 0; k < 2; k++) {

				for (int l = -1; l < 1; l++) {

					int frameX = styleX + (k * 18);
					int frameY = styleY + ((l + 1) * 18);

					undo.Add(new ChangedTile(x + k, y + l));

					Main.tile[x + k, y + l].active(true);
					Main.tile[x + k, y + l].frameX = (short)frameX;
					Main.tile[x + k, y + l].frameY = (short)(frameY + style * 36);
					Main.tile[x + k, y + l].type = type;
					Main.tile[x + k, y + l].halfBrick(false);

				}

			}

			return true;

		}

	}

}
