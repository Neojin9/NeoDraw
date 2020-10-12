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

        public ChangedTile(Point location, Tile tile = null) {
            
            Location = location;

            if (tile == null) {
                Tile = new Tile(Main.tile[location.X, location.Y]);
            } else {
                Tile = tile;
            }

            if (Neo.IsTopLeft(location)) {

                if (TileID.Sets.BasicChest[Tile.type] || Main.tileContainer[Tile.type]) {

                    int chestIndex = Chest.FindChest(location.X, location.Y);

                    if (chestIndex != -1) {
                        Chest = Main.chest[chestIndex];
                    }

                }

            }

        }

        public ChangedTile(int x, int y, Tile tile = null) {

            Location = new Point(x, y);
            
            if (tile == null) {
                Tile = new Tile(Main.tile[x, y]);
            } else {
                Tile = tile;
            }

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
