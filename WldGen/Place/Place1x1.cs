using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 - TileCut

		public static bool Place1x1(int x, int y, int type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			Tile tile = Main.tile[x, y];

			if (Main.tile[x, y] == null) {
				tile = new Tile();
				Main.tile[x, y] = tile;
			}

			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();

			if (type == TileID.BeachPiles) {

				if (WorldGen.SolidTile2(x, y + 1) || (Main.tile[x, y + 1].nactive() && Main.tileTable[Main.tile[x, y + 1].type])) {

					if (!Neo.TileCut(x, y))
						return false;

					undo.Add(new ChangedTile(x, y));

					int frameX = style;
					int frameY = 0;

					if (style > 5) {
						frameX = 0;
						frameY = style - 4;
                    }
					else if (style > 2) {
						frameX -= 3;
						frameY = 1;
                    }

					tile.active(active: true);
					tile.type = (ushort)type;
					tile.frameX = (short)(22 * frameX);
					tile.frameY = (short)(22 * frameY);

					return true;

				}

			}
			else if (type == TileID.HoneyDrip || type == TileID.LavaDrip || type == TileID.SandDrip || type == TileID.WaterDrip) {

				if (!Neo.TileCut(x, y))
					return false;

				if (!Main.tile[x, y].active() && Main.tile[x, y - 1].active() && WorldGen.SolidTile(x, y - 1)) {

					undo.Add(new ChangedTile(x, y));

					Main.tile[x, y].type = (ushort)type;
					Main.tile[x, y].frameX = 0;
					Main.tile[x, y].frameY = 0;
					Main.tile[x, y].active(active: true);

				}

            }
			else if (WorldGen.SolidTile2(x, y + 1)) {

				if (!Neo.TileCut(x, y))
					return false;

				undo.Add(new ChangedTile(x, y));

				tile.active(active: true);
				tile.type = (ushort)type;
				
				switch (type) {

					case TileID.BeachPiles:
						tile.frameX = (short)(22 * WorldGen.genRand.Next(2));
						tile.frameY = (short)(22 * style);
						break;

					case TileID.Presents:
					case TileID.Timers:
					case TileID.MetalBars:
						tile.frameX = (short)(style * 18);
						tile.frameY = 0;
						break;

					default:
						tile.frameX = 0;
						tile.frameY = (short)(style * 18);
						break;

				}

				return true;

			}

			return false;

		}

	}

}
