using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/5/2020     Not sure if I can get TileCut to work with TileObject.CanPlace

		public static int PlaceChest(int x, int y, ref UndoStep undo, ushort type = TileID.Containers, bool notNearOtherChests = false, int style = 0) {

			int num = -1;

			if (!WorldGen.InWorld(x, y))
				return num;

			//if (TileID.Sets.Boulders[Main.tile[x, y + 1].type] || TileID.Sets.Boulders[Main.tile[x + 1, y + 1].type]) // TODO: Updated for v1.4
			//	return -1;

			if (Main.tile[x, y + 1].type == TileID.Boulder || Main.tile[x + 1, y + 1].type == TileID.Boulder)
				return -1;

			if (TileObject.CanPlace(x, y, type, style, 1, out TileObject objectData)) {

                bool flag = !(notNearOtherChests && Chest.NearOtherChests(x - 1, y - 1));

                if (flag) {
                    Place(objectData, ref undo);
                    num = Chest.CreateChest(objectData.xCoord, objectData.yCoord);
                }

            } else {
				num = -1;
			}

			if (num != -1 && Main.netMode == NetmodeID.MultiplayerClient && type == TileID.Containers)
				NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 0, x, y, style);

			if (num != -1 && Main.netMode == NetmodeID.MultiplayerClient && type == TileID.Containers2)
				NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 4, x, y, style);

			if (num != 1 && Main.netMode == NetmodeID.MultiplayerClient && type >= TileNames.OriginalTileCount && TileID.Sets.BasicChest[type])
				NetMessage.SendData(MessageID.ChestUpdates, -1, -1, null, 100, x, y, style, 0, type);

			return num;

		}

	}

}
