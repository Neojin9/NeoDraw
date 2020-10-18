using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/6/2020 Copy/Paste TileCut

		public static bool PlaceJunglePlant(int x, int y, ushort type, int styleX, int styleY, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			Point[] points;
			int m = 0;

			if (styleY > 0 || type == TileID.LifeFruit || type == TileID.PlanteraBulb) {

				int num = y;

				points = new Point[4];

				if (type == TileID.ChineseLanterns || type == TileID.DiscoBall)
					num++;
				
				for (int i = x - 1; i < x + 1; i++) {

					for (int j = num - 1; j < num + 1; j++) {

						if (Main.tile[i, j] == null)
							Main.tile[i, j] = new Tile();
						
						if (Main.tile[i, j].active() && !Main.tileCut[Main.tile[i, j].type] && Main.tile[i, j].type != 61 && Main.tile[i, j].type != 62 && Main.tile[i, j].type != 69 && Main.tile[i, j].type != 74 && (type != 236 || Main.tile[i, j].type != 233) && (type != 238 || Main.tile[i, j].type != 233) && (Main.tile[i, j].type != 185 || Main.tile[i, j].frameY != 0))
							return false;
						
						if (type == TileID.SkullLanterns && Main.tile[i, j].liquid > 0)
							return false;

						points[m++] = new Point(i, j);

					}

					if (Main.tile[i, num + 1] == null)
						Main.tile[i, num + 1] = new Tile();
					
					if (!WorldGen.SolidTile(i, num + 1) || Main.tile[i, num + 1].type != TileID.JungleGrass)
						return false;
					
				}

				if (!Neo.TileCut(points))
					return false;

				short num2 = 36;

				if (type == TileID.LifeFruit || type == TileID.PlanteraBulb)
					num2 = 0;

				short num3 = (short)(36 * styleX);

				undo.Add(new ChangedTile(x - 1, num - 1));
				undo.Add(new ChangedTile(x,     num - 1));
				undo.Add(new ChangedTile(x - 1, num    ));
				undo.Add(new ChangedTile(x,     num    ));

				Main.tile[x - 1, num - 1].active(true);
				Main.tile[x - 1, num - 1].frameY = num2;
				Main.tile[x - 1, num - 1].frameX = num3;
				Main.tile[x - 1, num - 1].type = type;

				Main.tile[x, num - 1].active(true);
				Main.tile[x, num - 1].frameY = num2;
				Main.tile[x, num - 1].frameX = (short)(18 + num3);
				Main.tile[x, num - 1].type = type;

				Main.tile[x - 1, num].active(true);
				Main.tile[x - 1, num].frameY = (short)(num2 + 18);
				Main.tile[x - 1, num].frameX = num3;
				Main.tile[x - 1, num].type = type;

				Main.tile[x, num].active(true);
				Main.tile[x, num].frameY = (short)(num2 + 18);
				Main.tile[x, num].frameX = (short)(18 + num3);
				Main.tile[x, num].type = type;

			}
			else {

				points = new Point[6];

				for (int k = x - 1; k < x + 2; k++) {

					for (int l = y - 1; l < y + 1; l++) {

						if (Main.tile[k, l] == null)
							Main.tile[k, l] = new Tile();
						
						if (Main.tile[k, l].active() && !Main.tileCut[Main.tile[k, l].type] && Main.tile[k, l].type != 61 && Main.tile[k, l].type != 62 && Main.tile[k, l].type != 69 && Main.tile[k, l].type != 74 && (Main.tile[k, l].type != 185 || Main.tile[k, l].frameY != 0))
							return false;

						points[m++] = new Point(k, l);

					}

					if (Main.tile[k, y + 1] == null)
						Main.tile[k, y + 1] = new Tile();
					
					if (!WorldGen.SolidTile(k, y + 1) || Main.tile[k, y + 1].type != TileID.JungleGrass)
						return false;
					
				}

				if (!Neo.TileCut(points))
					return false;

				short num4 = (short)(54 * styleX);

				undo.Add(new ChangedTile(x - 1, y - 1));
				undo.Add(new ChangedTile(x,     y - 1));
				undo.Add(new ChangedTile(x + 1, y - 1));
				undo.Add(new ChangedTile(x - 1, y    ));
				undo.Add(new ChangedTile(x,     y    ));
				undo.Add(new ChangedTile(x + 1, y    ));

				Main.tile[x - 1, y - 1].active(true);
				Main.tile[x - 1, y - 1].frameY = 0;
				Main.tile[x - 1, y - 1].frameX = num4;
				Main.tile[x - 1, y - 1].type = type;

				Main.tile[x, y - 1].active(true);
				Main.tile[x, y - 1].frameY = 0;
				Main.tile[x, y - 1].frameX = (short)(num4 + 18);
				Main.tile[x, y - 1].type = type;

				Main.tile[x + 1, y - 1].active(true);
				Main.tile[x + 1, y - 1].frameY = 0;
				Main.tile[x + 1, y - 1].frameX = (short)(num4 + 36);
				Main.tile[x + 1, y - 1].type = type;

				Main.tile[x - 1, y].active(true);
				Main.tile[x - 1, y].frameY = 18;
				Main.tile[x - 1, y].frameX = num4;
				Main.tile[x - 1, y].type = type;

				Main.tile[x, y].active(true);
				Main.tile[x, y].frameY = 18;
				Main.tile[x, y].frameX = (short)(num4 + 18);
				Main.tile[x, y].type = type;

				Main.tile[x + 1, y].active(true);
				Main.tile[x + 1, y].frameY = 18;
				Main.tile[x + 1, y].frameX = (short)(num4 + 36);
				Main.tile[x + 1, y].type = type;

			}

			return true;
		}

	}

}
