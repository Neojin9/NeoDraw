using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static bool DestroyChest(int X, int Y) {

			for (int i = 0; i < 1000; i++) {

				Chest chest = Main.chest[i];

				if (chest == null || chest.x != X || chest.y != Y)
					continue;
				
				Main.chest[i] = null;

				if (Main.player[Main.myPlayer].chest == i)
					Main.player[Main.myPlayer].chest = -1;
				
				Recipe.FindRecipes();

				return true;

			}

			return true;

		}

		public static void DestroyChestDirect(int X, int Y, int id) {

			if (id >= 0 && id < Main.chest.Length) {

				try {

					Chest chest = Main.chest[id];

					if (chest != null && chest.x == X && chest.y == Y) {

						Main.chest[id] = null;

						if (Main.player[Main.myPlayer].chest == id)
							Main.player[Main.myPlayer].chest = -1;
						
						Recipe.FindRecipes();

					}

				}
				catch { }

			}

		}

	}

}
