using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/9/2020 CopyPaste TileCut

		public static bool PlaceSign(int x, int y, ushort type, ref UndoStep undo, int Style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int num  = x - 2;
			int num2 = x + 3;
			int num3 = y - 2;
			int num4 = y + 3;

			if (num < 0)
				return false;
			
			if (num2 > Main.maxTilesX)
				return false;
			
			if (num3 < 0)
				return false;
			
			if (num4 > Main.maxTilesY)
				return false;
			
			for (int i = num; i < num2; i++)
				for (int j = num3; j < num4; j++)
					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

			int leftSide = x;
			int top = y;
			int placeStyle = 0;

			if (type == TileID.Signs || type == TileID.AnnouncementBox || type == 510 || type == 511) {

				if (WorldGen.SolidTile2(x, y + 1) && WorldGen.SolidTile2(x + 1, y + 1)) {
					top--;
					placeStyle = 0;
				} else if (Main.tile[x, y - 1].nactive() && Main.tileSolid[Main.tile[x, y - 1].type] && !Main.tileSolidTop[Main.tile[x, y - 1].type] && Main.tile[x + 1, y - 1].nactive() && Main.tileSolid[Main.tile[x + 1, y - 1].type] && !Main.tileSolidTop[Main.tile[x + 1, y - 1].type]) {
					placeStyle = 1;
				} else if (Main.tile[x - 1, y].nactive() && Main.tileSolid[Main.tile[x - 1, y].type] && !Main.tileSolidTop[Main.tile[x - 1, y].type] && !Main.tileNoAttach[Main.tile[x - 1, y].type] && Main.tile[x - 1, y + 1].nactive() && Main.tileSolid[Main.tile[x - 1, y + 1].type] && !Main.tileSolidTop[Main.tile[x - 1, y + 1].type] && !Main.tileNoAttach[Main.tile[x - 1, y + 1].type]) {
					placeStyle = 2;
				} else if (Main.tile[x + 1, y].nactive() && Main.tileSolid[Main.tile[x + 1, y].type] && !Main.tileSolidTop[Main.tile[x + 1, y].type] && !Main.tileNoAttach[Main.tile[x + 1, y].type] && Main.tile[x + 1, y + 1].nactive() && Main.tileSolid[Main.tile[x + 1, y + 1].type] && !Main.tileSolidTop[Main.tile[x + 1, y + 1].type] && !Main.tileNoAttach[Main.tile[x + 1, y + 1].type]) {
					leftSide--;
					placeStyle = 3;
				} else {

					if (Main.tile[leftSide, top].wall <= 0 || Main.tile[leftSide + 1, top].wall <= 0 || Main.tile[leftSide, top + 1].wall <= 0 || Main.tile[leftSide + 1, top + 1].wall <= 0)
						return false;
					
					placeStyle = 4;

				}

			}

			if (!Neo.TileCut(new[] { new Point(leftSide, top), new Point(leftSide + 1, top), new Point(leftSide, top + 1), new Point(leftSide + 1, top + 1) }))
				return false;

			int frameX = 36 * placeStyle;

			for (int k = 0; k < 2; k++) {

				for (int l = 0; l < 2; l++) {

					undo.Add(new ChangedTile(leftSide + k, top + l));

					Main.tile[leftSide + k, top + l].active(true);
					Main.tile[leftSide + k, top + l].type = type;
					Main.tile[leftSide + k, top + l].frameX = (short)(frameX + 18 * k);
					Main.tile[leftSide + k, top + l].frameY = (short)(18 * l);

				}

			}

			return true;

		}

	}

}
