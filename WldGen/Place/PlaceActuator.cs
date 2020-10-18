using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020

		public static bool PlaceActuator(int i, int j, ref UndoStep undo) {

			if (!WorldGen.InWorld(i, j))
				return false;

			if (!Main.tile[i, j].actuator()) {

				undo.Add(new ChangedTile(i, j));
				Main.PlaySound(SoundID.Dig, i * 16, j * 16);
				Main.tile[i, j].actuator(true);
				return true;

			}

			return false;

		}

	}

}
