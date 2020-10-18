using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place2xX(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int frameX = style * 36;
			int frameY = 0;
			int height = 3;

			if (type == TileID.WaterFountain) {
				height = 4;
			}
			else if (type == TileID.GrandfatherClocks) {
				height = 5;
			}

			if (type == TileID.Statues && style == 34) { // Mushroom Statue

				type = TileID.MushroomStatue;
				style = 0;
				frameX   = 0;

			}

			if (type == TileID.Statues) {

				int num4 = style / 55;
				frameX -= 1980 * num4;
				frameY += 54 * num4;

			}

			Point[] points = new Point[2 * height];
			int k = 0;

			if (type == TileID.WarTableBanner) {

				if (Main.tile[x, y - 1] == null)
					Main.tile[x, y - 1] = new Tile();

				if (Main.tile[x + 1, y - 1] == null)
					Main.tile[x + 1, y - 1] = new Tile();

				if (!Main.tile[x, y - 1].active() || !Main.tile[x + 1, y - 1].active() || !WorldGen.SolidTile(x, y - 1) || !WorldGen.SolidTile(x + 1, y - 1))
					return false;

				for (int i = y; i < y + height; i++) {

					if (Main.tile[x, i] == null)
						Main.tile[x, i] = new Tile();

					if (Main.tile[x + 1, i] == null)
						Main.tile[x + 1, i] = new Tile();

					points[k++] = new Point(x, i);
					points[k++] = new Point(x + 1, i);

                }

				if (!Neo.TileCut(points))
					return false;

				for (int j = 0; j < height; j++) {

					undo.Add(new ChangedTile(x, y + j));
					undo.Add(new ChangedTile(x + 1, y + j));

					Main.tile[x, y + j].active(true);
					Main.tile[x, y + j].frameY = (short)(frameY + j * 18);
					Main.tile[x, y + j].frameX = (short)frameX;
					Main.tile[x, y + j].type = type;

					Main.tile[x + 1, y + j].active(true);
					Main.tile[x + 1, y + j].frameY = (short)(frameY + j * 18);
					Main.tile[x + 1, y + j].frameX = (short)(frameX + 18);
					Main.tile[x + 1, y + j].type = type;

				}

				return true;

			}

			for (int i = y - height + 1; i < y + 1; i++) {

				if (Main.tile[x, i] == null)
					Main.tile[x, i] = new Tile();
				
				if (Main.tile[x + 1, i] == null)
					Main.tile[x + 1, i] = new Tile();

				points[k++] = new Point(x, i);
				points[k++] = new Point(x + 1, i);

			}

			if (!Neo.TileCut(points))
				return false;

			if (!WorldGen.SolidTile2(x, y + 1) || !WorldGen.SolidTile2(x + 1, y + 1))
				return false;

			for (int j = 0; j < height; j++) {

				undo.Add(new ChangedTile(x,     y - height + 1 + j));
				undo.Add(new ChangedTile(x + 1, y - height + 1 + j));

				Main.tile[x,     y - height + 1 + j].active(true);
				Main.tile[x,     y - height + 1 + j].frameY = (short)(frameY + j * 18);
				Main.tile[x,     y - height + 1 + j].frameX = (short)frameX;
				Main.tile[x,     y - height + 1 + j].type = type;

				Main.tile[x + 1, y - height + 1 + j].active(true);
				Main.tile[x + 1, y - height + 1 + j].frameY = (short)(frameY + j * 18);
				Main.tile[x + 1, y - height + 1 + j].frameX = (short)(frameX + 18);
				Main.tile[x + 1, y - height + 1 + j].type = type;

			}

			return true;

		}

	}

}
