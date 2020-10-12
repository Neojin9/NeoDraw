using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated 8/6/2020 Heavily Modified TileCut

		public static bool PlaceDye(int x, int y, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			if (style == 7) { // Orange Bloodroot
				
				if (!WorldGen.SolidTile(x, y - 1))
					return false;

				if (!Neo.TileCut(new Point[] { new Point(x, y), new Point(x, y + 1) }))
					return false;

			} else {

				if (style == 6) { // Pink Prickly Pear

					if (!Main.tile[x, y + 1].nactive() || Main.tile[x, y + 1].type != TileID.Cactus)
						return false;

					if (!Neo.TileCut(new Point[] { new Point(x, y), new Point(x, y - 1), new Point(x - 1, y), new Point(x + 1, y) }))
						return false;

				} else if (WorldGen.SolidTile(x, y + 1)) {

					switch (style) {

                        case 5:

							if (Main.tile[x, y].liquid != byte.MaxValue)
								return false;

                            break;

                        case 8:
						case 9:
						case 10:
						case 11:

							break;

						default:

							if (Main.tile[x, y].liquid != 0)
								return false;
							
							if (style == 3 || style == 4)
								if (Main.tile[x, y].wall != 0)
									return false;

							break;

					}

					if (!Neo.TileCut(new Point[] { new Point(x, y), new Point(x, y - 1) }))
						return false;

				}
				else {
					return false;
                }

			}

			Neo.SetTile(x, y, 227, ref undo);
			Main.tile[x, y].frameY = 0;
			Main.tile[x, y].frameX = (short)(34 * style);

			return true;

		}

	}

}
