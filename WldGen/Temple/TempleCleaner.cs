using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/25/2020 Copy/Paste

		public static void templeCleaner(int x, int y, ref UndoStep undo) {

			int num = 0;

			if (Main.tile[x + 1, y].active() && Main.tile[x + 1, y].type == TileID.LihzahrdBrick)
				num++;
			
			if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y].type == TileID.LihzahrdBrick)
				num++;
			
			if (Main.tile[x, y + 1].active() && Main.tile[x, y + 1].type == TileID.LihzahrdBrick)
				num++;
			
			if (Main.tile[x, y - 1].active() && Main.tile[x, y - 1].type == TileID.LihzahrdBrick)
				num++;
			
			if (Main.tile[x, y].active() && Main.tile[x, y].type == TileID.LihzahrdBrick) {

				if (num <= 1)
					Neo.SetWall(x, y, WallID.LihzahrdBrickUnsafe, ref undo, false);

			}
			else if (!Main.tile[x, y].active() && num == 3) {

				Neo.SetTile(x, y, TileID.LihzahrdBrick, ref undo);
				Neo.SetLiquid(x, y, 0);

			}

		}

	}

}
