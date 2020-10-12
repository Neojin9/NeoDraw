using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/26/2020 Copy/Paste

		public static bool PlaceWire(int i, int j, ref UndoStep undo) {

			if (!Main.tile[i, j].wire()) {
				
				undo.Add(new ChangedTile(i, j));
				Main.tile[i, j].wire(wire: true);

				return true;

			}

			return false;

		}

		public static bool PlaceWire2(int i, int j, ref UndoStep undo) {

			if (!Main.tile[i, j].wire2()) {

				undo.Add(new ChangedTile(i, j));
				Main.tile[i, j].wire2(wire2: true);

				return true;

			}

			return false;

		}

		public static bool PlaceWire3(int i, int j, ref UndoStep undo) {

			if (!Main.tile[i, j].wire3()) {

				undo.Add(new ChangedTile(i, j));
				Main.tile[i, j].wire3(wire3: true);

				return true;

			}

			return false;

		}

		public static bool PlaceWire4(int i, int j, ref UndoStep undo) {

			if (!Main.tile[i, j].wire4()) {

				undo.Add(new ChangedTile(i, j));
				Main.tile[i, j].wire4(wire4: true);

				return true;

			}

			return false;

		}

	}

}
