using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen {
    
    public partial class WldGen { // Updated v1.4 7/29/2020 Copy/Paste

		public static void outerTempled(int x, int y, ref UndoStep undo) {

			if ((Main.tile[x, y].active() & (Main.tile[x, y].type == TileID.LihzahrdBrick)) || Main.tile[x, y].wall == WallID.LihzahrdBrickUnsafe)
				return;
			
			int num = 6;

			for (int i = x - num; i <= x + num; i++) {

				for (int j = y - num; j <= y + num; j++) {

					if (!Main.tile[i, j].active() && Main.tile[i, j].wall == WallID.LihzahrdBrickUnsafe) {

						i = x;
						j = y;

						Neo.SetTile(i, j, TileID.LihzahrdBrick, ref undo);
						Neo.SetLiquid(i, j, 0);

						return;

					}

				}

			}

		}

	}

}
