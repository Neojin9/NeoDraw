using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader;


namespace NeoDraw.WldGen {

    public partial class WldGen {
		
		public static bool stopDrops = true;

		public static void KillTile(int x, int yCoord, ref UndoStep undo, bool fail = false, bool effectOnly = false, bool noItem = true) {

			if (x < 0 || yCoord < 0 || x >= Main.maxTilesX || yCoord >= Main.maxTilesY)
				return;

			stopDrops = true;

			Tile tile = Main.tile[x, yCoord];

			if (tile == null) {
				tile = new Tile();
				Main.tile[x, yCoord] = tile;
			}

			if (!tile.active())
				return;

			if (yCoord >= 1 && Main.tile[x, yCoord - 1] == null)
				Main.tile[x, yCoord - 1] = new Tile();

			//if (yCoord >= 1 && Main.tile[x, yCoord - 1].active() && ((Main.tile[x, yCoord - 1].type == 5 && tile.type != 5) || (Main.tile[x, yCoord - 1].type == 323 && tile.type != 323) || (TileID.Sets.BasicChest[Main.tile[x, yCoord - 1].type] && !TileID.Sets.BasicChest[tile.type]) || (Main.tile[x, yCoord - 1].type == 323 && tile.type != 323) || (TileLoader.IsDresser(Main.tile[x, yCoord - 1].type) && !TileLoader.IsDresser(tile.type)) || (Main.tile[x, yCoord - 1].type == 26 && tile.type != 26) || (Main.tile[x, yCoord - 1].type == 72 && tile.type != 72))) {
			//	if (Main.tile[x, yCoord - 1].type == 5) {
			//		if ((Main.tile[x, yCoord - 1].frameX != 66 || Main.tile[x, yCoord - 1].frameY < 0 || Main.tile[x, yCoord - 1].frameY > 44) && (Main.tile[x, yCoord - 1].frameX != 88 || Main.tile[x, yCoord - 1].frameY < 66 || Main.tile[x, yCoord - 1].frameY > 110) && Main.tile[x, yCoord - 1].frameY < 198) {
			//			return;
			//		}
			//	} else if (Main.tile[x, yCoord - 1].type != 323 || Main.tile[x, yCoord - 1].frameX == 66 || Main.tile[x, yCoord - 1].frameX == 220) {
			//		return;
			//	}
			//}

			//if (tile.type == TileID.ClosedDoor && tile.frameY >= 594 && tile.frameY <= 646) {
			//	fail = true;
			//}

			//if (tile.type == TileID.Boulder)
			//	fail = WorldGen.CheckBoulderChest(x, yCoord);

			//if (tile.type == 235) {
			//	int frameX = tile.frameX;
			//	int num = x - frameX % 54 / 18;
			//	for (int k = 0; k < 3; k++) {
			//		if (Main.tile[num + k, yCoord - 1].active() && (TileID.Sets.BasicChest[Main.tile[num + k, yCoord - 1].type] || TileID.Sets.BasicChestFake[Main.tile[num + k, yCoord - 1].type] || TileLoader.IsDresser(Main.tile[num + k, yCoord - 1].type))) {
			//			fail = true;
			//			break;
			//		}
			//	}
			//}

			//if (!effectOnly && !stopDrops) {
			//	if (!noItem && FixExploitManEaters.SpotProtected(x, yCoord)) {
			//		return;
			//	}
			//}

			if (TileID.Sets.BasicChest[tile.type] && Main.netMode != NetmodeID.MultiplayerClient) {
			
				int num15 = tile.frameX / 18;
				int y = yCoord - tile.frameY / 18;
				while (num15 > 1) {
					num15 -= 2;
				}
				num15 = x - num15;
			
				if (!DestroyChest(num15, y))
					return;
			
			}

			if (TileLoader.IsDresser(tile.type) && Main.netMode != NetmodeID.MultiplayerClient) {
			
				int num16 = tile.frameX / 18;
				int y2 = yCoord - tile.frameY / 18;
				num16 %= 3;
				num16 = x - num16;
			
				if (!DestroyChest(num16, y2))
					return;
			
			}

			if (!noItem && !stopDrops && Main.netMode != NetmodeID.MultiplayerClient) {

				if (tile.type == 428) {
					PressurePlateHelper.DestroyPlate(new Point(x, yCoord));
				} else if (tile.type == 423) {
					TELogicSensor.Kill(x, yCoord);
				}

			}

			undo.Add(new ChangedTile(x, yCoord));

			tile.active(active: false);
			tile.halfBrick(halfBrick: false);
			tile.frameX = -1;
			tile.frameY = -1;
			tile.color(0);
			tile.frameNumber(0);

			if (tile.type == 419) {
				Wiring.PokeLogicGate(x, yCoord + 1);
			} else if (tile.type == 54) {
				WorldGen.SquareWallFrame(x, yCoord);
			}

			tile.type = 0;
			tile.inActive(inActive: false);
			SquareTileFrame(x, yCoord, ref undo);

		}

	}

}
