using NeoDraw.Undo;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using NeoDraw.Core;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool Place3x2(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y, 5))
				return false;

			bool isDynastyTable = false;

			if (type == TileID.Tables && style == 25) // Dynasty Table
				isDynastyTable = true;
			
			int num = isDynastyTable ? y : y - 1;

			Point[] points = new Point[6];
			int k = 0;

			for (int i = x - 1; i < x + 2; i++) {

				for (int j = num; j < y + 1; j++) {

					if (Main.tile[i, j] == null)
						Main.tile[i, j] = new Tile();

					points[k++] = new Point(i, j);

					if (type == TileID.Campfire && Main.tile[i, j].liquid > 0)
						return false;
					
				}

				if (Main.tile[i, y + 1] == null)
					Main.tile[i, y + 1] = new Tile();
				
				switch (type) {

					case TileID.SnailCage:
					case TileID.GlowingSnailCage:
					case TileID.FrogCage:
					case TileID.MouseCage:
					case TileID.WormCage:
					case TileID.GoldFrogCage:
					case TileID.GoldGrasshopperCage:
					case TileID.GoldMouseCage:
					case TileID.GoldWormCage:
					case 582:
					case 619:

						if (!WorldGen.SolidTile2(i, y + 1) && (!Main.tile[i, y + 1].nactive() || !Main.tileSolidTop[Main.tile[i, y + 1].type] || Main.tile[i, y + 1].frameY != 0))
							return false;
						
						break;

					default:

						if (type == TileID.DemonAltar && Main.tile[i, y + 1].type == 484)
							return false;

						if (!WorldGen.SolidTile2(i, y + 1))
							return false;
						
						break;

				}

			}

			if (TileLoader.IsDresser(type)) {

				if (Chest.CreateChest(x - 1, y - 1) == -1) {
					return false;
				} else if (Main.netMode == NetmodeID.MultiplayerClient) {
					NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 2, x, y, style);
				}

			}

			short frameY = 0;

			if (type == TileID.LargePiles2) {
				if (style > 34) {
					style -= 34;
					frameY += 36;
                }
            }

			short frameX = (short)(54 * style);

			if (!Neo.TileCut(points))
				return false;

			if (isDynastyTable) {

				undo.Add(new ChangedTile(x - 1, y));
				undo.Add(new ChangedTile(x,     y));
				undo.Add(new ChangedTile(x + 1, y));

				Main.tile[x - 1, y].active(active: true);
				Main.tile[x - 1, y].frameY = frameY;
				Main.tile[x - 1, y].frameX = frameX;
				Main.tile[x - 1, y].type = type;

				Main.tile[x, y].active(active: true);
				Main.tile[x, y].frameY = frameY;
				Main.tile[x, y].frameX = (short)(frameX + 18);
				Main.tile[x, y].type = type;

				Main.tile[x + 1, y].active(active: true);
				Main.tile[x + 1, y].frameY = frameY;
				Main.tile[x + 1, y].frameX = (short)(frameX + 36);
				Main.tile[x + 1, y].type = type;

				return true;

			}

			undo.Add(new ChangedTile(x - 1, y - 1));
			undo.Add(new ChangedTile(x,     y - 1));
			undo.Add(new ChangedTile(x + 1, y - 1));
			undo.Add(new ChangedTile(x - 1, y    ));
			undo.Add(new ChangedTile(x,     y    ));
			undo.Add(new ChangedTile(x + 1, y    ));

			Main.tile[x - 1, y - 1].active(active: true);
			Main.tile[x - 1, y - 1].frameY = frameY;
			Main.tile[x - 1, y - 1].frameX = frameX;
			Main.tile[x - 1, y - 1].type = type;

			Main.tile[x, y - 1].active(active: true);
			Main.tile[x, y - 1].frameY = frameY;
			Main.tile[x, y - 1].frameX = (short)(frameX + 18);
			Main.tile[x, y - 1].type = type;

			Main.tile[x + 1, y - 1].active(active: true);
			Main.tile[x + 1, y - 1].frameY = frameY;
			Main.tile[x + 1, y - 1].frameX = (short)(frameX + 36);
			Main.tile[x + 1, y - 1].type = type;

			Main.tile[x - 1, y].active(active: true);
			Main.tile[x - 1, y].frameY = (short)(frameY + 18);
			Main.tile[x - 1, y].frameX = frameX;
			Main.tile[x - 1, y].type = type;

			Main.tile[x, y].active(active: true);
			Main.tile[x, y].frameY = (short)(frameY + 18);
			Main.tile[x, y].frameX = (short)(frameX + 18);
			Main.tile[x, y].type = type;

			Main.tile[x + 1, y].active(active: true);
			Main.tile[x + 1, y].frameY = (short)(frameY + 18);
			Main.tile[x + 1, y].frameX = (short)(frameX + 36);
			Main.tile[x + 1, y].type = type;

			return true;

		}

	}

}
