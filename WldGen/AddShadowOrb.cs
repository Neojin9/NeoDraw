using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated 8/10/2020 v1.4 Copy/Paste

		public static bool AddShadowOrb(int x, int y, ref UndoStep undo, short style = OrbType.Default) {

			if (!WorldGen.InWorld(x, y, 10))
				return false;

			for (int i = x - 1; i < x + 1; i++)
				for (int j = y - 1; j < y + 1; j++)
					if (Main.tile[i, j].active() && Main.tile[i, j].type == 31)
						return false;

			short num = 0;

			if (crimson)
				num = (short)(num + 36);

			if (style != OrbType.Default)
				num = style;

			undo.Add(new ChangedTile(x - 1, y - 1));
			undo.Add(new ChangedTile(x, y - 1));
			undo.Add(new ChangedTile(x - 1, y));
			undo.Add(new ChangedTile(x, y));

			Main.tile[x - 1, y - 1].active(active: true);
			Main.tile[x - 1, y - 1].type = 31;
			Main.tile[x - 1, y - 1].frameX = num;
			Main.tile[x - 1, y - 1].frameY = 0;

			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].type = 31;
			Main.tile[x, y - 1].frameX = (short)(18 + num);
			Main.tile[x, y - 1].frameY = 0;

			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].type = 31;
			Main.tile[x - 1, y].frameX = num;
			Main.tile[x - 1, y].frameY = 18;

			Main.tile[x, y].active(active: true);
			Main.tile[x, y].type = 31;
			Main.tile[x, y].frameX = (short)(18 + num);
			Main.tile[x, y].frameY = 18;

			return true;

		}

	}

	public struct OrbType {
		public const short Default = -1;
		public const short ShadowOrb = 0;
		public const short CrimsonHeart = 36;
    }

}
