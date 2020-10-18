using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Not used.

		public static bool PlaceTrack(int i, int j, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(i, j))
				return false;

			Tile tile = Main.tile[i, j];

			tile.active(true);
			tile.type = 314;
			tile.frameY = -1;

			switch (style) {

				case 0:
					tile.frameX = -1;
					break;
				
				case 1:
					//tile.frameX = _firstPressureFrame;
					break;
				
				case 2:
					//tile.frameX = _firstLeftBoostFrame;
					break;
				
				case 3:
					//tile.frameX = _firstRightBoostFrame;
					break;

			}

			return true;

		}

	}

}
