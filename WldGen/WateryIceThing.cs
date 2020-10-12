using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020

		public static void MakeWateryIceThing(int x, int y, ref UndoStep undo) {

			while (!Main.tile[x, y].active() && Main.tile[x, y].liquid > 0) {

				y++;

				if (y > Main.maxTilesY - 50)
					return;
				
			}

			y--;

			while (Main.tile[x, y].liquid > 0) {

				y--;

				if (y < 10)
					return;
				
			}

			if (Main.tile[x, y].active())
				return;
			
			y++;

			if (!Main.tile[x, y].active()) {

				int num2 = x;

				while (!Main.tile[num2, y].active() && Main.tile[num2, y].liquid > 0 && Main.tile[num2, y - 1].liquid == 0 && !Main.tile[num2, y - 1].active() && !Main.tile[num2 - 1, y].halfBrick()) {

					PlaceTile(num2, y, TileID.BreakableIce, ref undo, mute: true);
					num2--;

				}

				for (num2 = x + 1; !Main.tile[num2, y].active() && Main.tile[num2, y].liquid > 0 && Main.tile[num2, y - 1].liquid == 0 && !Main.tile[num2, y - 1].active() && !Main.tile[num2 + 1, y].halfBrick(); num2++)
					PlaceTile(num2, y, TileID.BreakableIce, ref undo, mute: true);

			}

		}

	}

}
