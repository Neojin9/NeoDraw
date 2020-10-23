using System.Linq;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place2x2(int x, int y, ushort type, int style, ref UndoStep undo) {

			if (type == TileID.ChineseLanterns || type == TileID.DiscoBall || type == TileID.BeeHive)
				y++;

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			if (type == TileID.Sinks) {

				if (!CanPlaceSink(x, y, type, style))
					return false;
				
			} else {

				if (type == TileID.TrapdoorOpen)
					x++;

				for (int i = x - 1; i < x + 1; i++) {

					for (int j = y - 1; j < y + 1; j++) {

						Tile tileSafely = Framing.GetTileSafely(i, j);

						if ((tileSafely.active() && !Main.tileCut[tileSafely.type]) || (type == TileID.SkullLanterns && tileSafely.liquid > 0))
							return false;
						
					}

					switch (type) {

						case TileID.BeeHive:
						case TileID.ChineseLanterns:
						case TileID.DiscoBall: {

								Tile tileSafely3 = Framing.GetTileSafely(i, y - 2);

								if (!tileSafely3.nactive() || !Main.tileSolid[tileSafely3.type] || Main.tileSolidTop[tileSafely3.type])
									return false;
								
								break;

							}
						case TileID.Lever: case TileID.TrapdoorOpen: {
								break;
                            }
						default: {

								Tile tileSafely2 = Framing.GetTileSafely(i, y + 1);
								
								if (!tileSafely2.nactive() || (!WorldGen.SolidTile2(tileSafely2) && !Main.tileTable[tileSafely2.type]))
									return false;
								
								break;

							}

					}

				}

			}

			if (type == TileID.Lever) {

				bool flag = true;

				if (Main.tile[x - 1, y + 1] == null)
					Main.tile[x - 1, y + 1] = new Tile();
				
				if (Main.tile[x, y + 1] == null)
					Main.tile[x, y + 1] = new Tile();
				
				if (!Main.tile[x - 1, y + 1].nactive() || (!WorldGen.SolidTile2(x - 1, y + 1) && !Main.tileTable[Main.tile[x - 1, y + 1].type]))
					flag = false;
				
				if (!Main.tile[x, y + 1].nactive() || (!WorldGen.SolidTile2(x, y + 1) && !Main.tileTable[Main.tile[x, y + 1].type]))
					flag = false;
				
				if (!flag && (Main.tile[x - 1, y - 1].wall < 1 || Main.tile[x, y - 1].wall < 1 || Main.tile[x - 1, y].wall < 1 || Main.tile[x - 1, y].wall < 1))
					return false;
				
			}

			x--;
			y--;

			if (type == TileID.TrapdoorOpen) {

				y++;

				Tile tileSafelyLeft = Framing.GetTileSafely(x - 1, y);
				Tile tileSafelyRight = Framing.GetTileSafely(x + 2, y);

				if (!tileSafelyLeft.nactive() || !tileSafelyRight.nactive() || !Main.tileSolid[tileSafelyLeft.type] || !Main.tileSolid[tileSafelyRight.type])
					return false;

				if (Main.keyState.PressingAlt()) {
					style = 1;
                }
				else {
					y--;
				}

			}

			int frameHeight = (type == TileID.Sinks) ? 38 : 36;

			Point[] tiles = new Point[4];
			int o = 0;

			for (int m = 0; m < 2; m++)
				for (int n = 0; n < 2; n++)
					tiles[o++] = new Point(x + m, y + n);

			if (!Neo.TileCut(tiles))
				return false;

			if (type == TileID.FakeContainers || type == TileID.TrapdoorOpen) {

				for (int k = 0; k < 2; k++) {

					for (int l = 0; l < 2; l++) {

						undo.Add(new ChangedTile(x + k, y + l));

						Main.tile[x + k, y + l].active(true);
						Main.tile[x + k, y + l].frameX = (short)(style * frameHeight + k * 18);
						Main.tile[x + k, y + l].frameY = (short)(l * 18);
						Main.tile[x + k, y + l].type = type;

					}

				}

				return true;

			}

			int frameX = 0;

			if (DrawInterface.Lightable.Contains(type) || DrawInterface.MultipleStates.Contains(type)) {

				if (Main.keyState.PressingAlt())
					frameX += 36;

            }

			for (int k = 0; k < 2; k++) {

				for (int l = 0; l < 2; l++) {

					undo.Add(new ChangedTile(x + k, y + l));

                    Main.tile[x + k, y + l].active(true);

					Main.tile[x + k, y + l].type   = type;
					Main.tile[x + k, y + l].frameX = (short)(frameX + k * 18);
                    Main.tile[x + k, y + l].frameY = (short)(style * frameHeight + l * 18);

				}

			}

			return true;

		}

		public static bool CanPlaceSink(int x, int y, ushort type, int style) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;
			
			bool result = true;

			x--;
			y--;

			for (int i = 0; i < 2; i++) {

				Tile tileSafely;

				for (int j = 0; j < 2; j++) {

					tileSafely = Framing.GetTileSafely(x + i, y + j);

					if (tileSafely.active() && !Main.tileCut[tileSafely.type])
						result = false;
					
				}

				tileSafely = Framing.GetTileSafely(x + i, y + 2);

				if (!tileSafely.nactive() || !WorldGen.SolidTile(tileSafely))
					result = false;
				
			}

			return result;

		}

	}

}
