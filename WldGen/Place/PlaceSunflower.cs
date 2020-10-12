using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated 8/9/2020 TileCut

		public static bool PlaceSunflower(int x, int y, ref UndoStep undo, ushort type = TileID.Sunflower) {

			UnifiedRandom genRand = WorldGen.genRand;

			if (!WorldGen.InWorld(x, y))
				return false;

			Point[] points = new Point[8];
			int m = 0;

			for (int i = x; i < x + 2; i++) {

				for (int j = y - 3; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[m++] = new Point(i, j);
					
				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				if (!Main.tile[i, y + 1].nactive() || Main.tile[i, y + 1].halfBrick() || Main.tile[i, y + 1].slope() != 0 || (Main.tile[i, y + 1].type != 2 && Main.tile[i, y + 1].type != 109))
					return false;
				
			}

			if (!Neo.TileCut(points))
				return false;

			int num = genRand.Next(3);

			for (int k = 0; k < 2; k++) {

				for (int l = -3; l < 1; l++) {

					int num2 = k * 18 + genRand.Next(3) * 36;

					if (l <= -2)
						num2 = k * 18 + num * 36;
					
					int num3 = (l + 3) * 18;

					undo.Add(new ChangedTile(x + k, y + l));

					Main.tile[x + k, y + l].active(active: true);
					Main.tile[x + k, y + l].frameX = (short)num2;
					Main.tile[x + k, y + l].frameY = (short)num3;
					Main.tile[x + k, y + l].type = type;

				}

			}

			return true;

		}

	}

}
