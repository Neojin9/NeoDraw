using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place1xX(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int frameY = style * 18;
			int height = 3;

			if (type == TileID.Lampposts) {
				height = 6;
			}
			else if (type == TileID.TallGateClosed || type == TileID.TallGateOpen) {
				height = 5;
            }

			if (!WorldGen.SolidTile2(x, y + 1))
				return false;

			Point[] points = new Point[height];
			int k = 0;

			for (int i = y - height + 1; i < y + 1; i++)
				points[k++] = new Point(x, i);

			if (!Neo.TileCut(points))
				return false;

			for (int j = 0; j < height; j++) {

				undo.Add(new ChangedTile(x, y - height + 1 + j));

				Main.tile[x, y - height + 1 + j].active(active: true);
				Main.tile[x, y - height + 1 + j].frameY = (short)(j * 18 + height * frameY);
				Main.tile[x, y - height + 1 + j].frameX = 0;
				Main.tile[x, y - height + 1 + j].type = type;

			}

			return true;

		}

	}

}
