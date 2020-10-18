using Microsoft.Xna.Framework;
using NeoDraw.Core;
using Terraria;
using Terraria.ID;

namespace NeoDraw.Undo {

    public class ChangedTile {

        #region Variables

        #region Public Variables

        public Point Location;
        public Tile Tile;
        public Chest Chest;

        #endregion

        #endregion

        public ChangedTile(int x, int y, Tile tile = null) {

            Location = new Point(x, y);
            
            Tile = tile ?? new Tile(Main.tile[x, y]);

			if (Neo.IsTopLeft(x, y)) {

				if (TileID.Sets.BasicChest[Tile.type] || Main.tileContainer[Tile.type]) {

					int chestIndex = Chest.FindChest(x, y);

					if (chestIndex != -1) {
						Chest = Main.chest[chestIndex];
					}

				}

			}

		}

    }

}
