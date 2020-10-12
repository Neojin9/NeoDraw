using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer {

		public static bool PlaceObject(int x, int y, int type, ref UndoStep undo, bool mute = false, int style = 0, int alternate = 0, int random = -1, int direction = -1) {

			if (!TileObject.CanPlace(x, y, type, style, direction, out TileObject objectData))
				return false;
			
			objectData.random = random;

			if (Place(objectData, ref undo))
				WorldGen.SquareTileFrame(x, y);
			
			return false;

		}

	}

}
