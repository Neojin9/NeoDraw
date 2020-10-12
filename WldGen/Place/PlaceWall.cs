using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/26/2020 Copy/Paste

		public static bool PlaceWall(int i, int j, int type, ref UndoStep undo, bool mute = true) {

			if (!WorldGen.InWorld(i, j, 2))
				return false;
			
			if (Main.tile[i, j] == null)
				Main.tile[i, j] = new Tile();

			undo.Add(new ChangedTile(i, j));
			Main.tile[i, j].wall = (ushort)type;
			WorldGen.SquareWallFrame(i, j);

			return true;

		}

	}

}
