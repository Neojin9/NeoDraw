using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020

		public static bool PlaceChestDirect(int x, int y, ushort type, int style, ref UndoStep undo, int id = -1) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Chest.CreateChest(x, y - 1, id);

			for (int i = 0; i <= 1; i++)
				for (int j = -1; j <= 0; j++)
					if (Main.tile[x + i, y + j] == null)
						Main.tile[x + i, y + j] = new Tile();

			undo.Add(new ChangedTile(x,     y - 1));
			undo.Add(new ChangedTile(x + 1, y - 1));
			undo.Add(new ChangedTile(x,     y    ));
			undo.Add(new ChangedTile(x + 1, y    ));

			Main.tile[x, y - 1].active(true);
			Main.tile[x, y - 1].frameY = 0;
			Main.tile[x, y - 1].frameX = (short)(36 * style);
			Main.tile[x, y - 1].type = type;
			Main.tile[x, y - 1].halfBrick(false);

			Main.tile[x + 1, y - 1].active(true);
			Main.tile[x + 1, y - 1].frameY = 0;
			Main.tile[x + 1, y - 1].frameX = (short)(18 + 36 * style);
			Main.tile[x + 1, y - 1].type = type;
			Main.tile[x + 1, y - 1].halfBrick(false);

			Main.tile[x, y].active(true);
			Main.tile[x, y].frameY = 18;
			Main.tile[x, y].frameX = (short)(36 * style);
			Main.tile[x, y].type = type;
			Main.tile[x, y].halfBrick(false);

			Main.tile[x + 1, y].active(true);
			Main.tile[x + 1, y].frameY = 18;
			Main.tile[x + 1, y].frameX = (short)(18 + 36 * style);
			Main.tile[x + 1, y].type = type;
			Main.tile[x + 1, y].halfBrick(false);

			return true;

		}

		public static bool PlaceDresserDirect(int x, int y, ushort type, int style, ref UndoStep undo, int id = -1) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Chest.CreateChest(x - 1, y - 1, id);
			
			for (int i = -1; i <= 1; i++)
				for (int j = -1; j <= 0; j++)
					if (Main.tile[x + i, y + j] == null)
						Main.tile[x + i, y + j] = new Tile();

			short num = (short)(style * 54);

			undo.Add(new ChangedTile(x - 1, y - 1));
			undo.Add(new ChangedTile(x,     y - 1));
			undo.Add(new ChangedTile(x + 1, y - 1));
			undo.Add(new ChangedTile(x - 1, y    ));
			undo.Add(new ChangedTile(x,     y    ));
			undo.Add(new ChangedTile(x + 1, y    ));

			Main.tile[x - 1, y - 1].active(true);
			Main.tile[x - 1, y - 1].frameY = 0;
			Main.tile[x - 1, y - 1].frameX = num;
			Main.tile[x - 1, y - 1].type = type;

			Main.tile[x, y - 1].active(true);
			Main.tile[x, y - 1].frameY = 0;
			Main.tile[x, y - 1].frameX = (short)(num + 18);
			Main.tile[x, y - 1].type = type;

			Main.tile[x + 1, y - 1].active(true);
			Main.tile[x + 1, y - 1].frameY = 0;
			Main.tile[x + 1, y - 1].frameX = (short)(num + 36);
			Main.tile[x + 1, y - 1].type = type;

			Main.tile[x - 1, y].active(true);
			Main.tile[x - 1, y].frameY = 18;
			Main.tile[x - 1, y].frameX = num;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(true);
			Main.tile[x, y].frameY = 18;
			Main.tile[x, y].frameX = (short)(num + 18);
			Main.tile[x, y].type = type;

			Main.tile[x + 1, y].active(true);
			Main.tile[x + 1, y].frameY = 18;
			Main.tile[x + 1, y].frameX = (short)(num + 36);
			Main.tile[x + 1, y].type = type;

			return true;

		}

	}

}
