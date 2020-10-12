using NeoDraw.Undo;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace NeoDraw.WldGen { // Updated v1.4 7/25/2020

    public partial class WldGen {

		public static bool OpenDoor(int i, int j, ref UndoStep undo, int direction = 1) {

            if (Main.tile[i, j - 1] == null)
				Main.tile[i, j - 1] = new Tile();
			
			if (Main.tile[i, j - 2] == null)
				Main.tile[i, j - 2] = new Tile();
			
			if (Main.tile[i, j + 1] == null)
				Main.tile[i, j + 1] = new Tile();
			
			if (Main.tile[i, j] == null)
				Main.tile[i, j] = new Tile();
			
			Tile tile = Main.tile[i, j];

			if (TileLoader.OpenDoorID(Main.tile[i, j]) < 0)
				return false;
			
			short frameX = 0;

			int doorStyle = tile.frameY;
			int num4 = 0;

			while (doorStyle >= 54) {
				doorStyle -= 54;
				num4++;
			}

			if (tile.frameX >= 54) {

				int num5 = tile.frameX / 54;
				num4 += 36 * num5;
				frameX = (short)(frameX + (short)(72 * num5));

			}

            int yPos = j - doorStyle / 18;
            int xPos = i;

			byte color = Main.tile[xPos, yPos].color();

			if (Main.tile[xPos, yPos + 1] == null)
				Main.tile[xPos, yPos + 1] = new Tile();
			
			byte color2 = Main.tile[xPos, yPos + 1].color();

			if (Main.tile[xPos, yPos + 2] == null)
				Main.tile[xPos, yPos + 2] = new Tile();
			
			byte color3 = Main.tile[xPos, yPos + 2].color();

			int bottomOfDoor;

			if (direction == -1) {

				xPos = i - 1;
				frameX = (short)(frameX + 36);
				bottomOfDoor = i - 1;

			}
			else {

				xPos = i;
				bottomOfDoor = i + 1;

			}

			for (int y2 = yPos; y2 < yPos + 3; y2++) {

				if (Main.tile[bottomOfDoor, y2] == null)
					Main.tile[bottomOfDoor, y2] = new Tile();
				
				if (Main.tile[bottomOfDoor, y2].active()) {

					if (!Main.tileCut[Main.tile[bottomOfDoor, y2].type] && Main.tile[bottomOfDoor, y2].type != 3 && Main.tile[bottomOfDoor, y2].type != 24 && Main.tile[bottomOfDoor, y2].type != 52 && Main.tile[bottomOfDoor, y2].type != 61 && Main.tile[bottomOfDoor, y2].type != 62 && Main.tile[bottomOfDoor, y2].type != 69 && Main.tile[bottomOfDoor, y2].type != 71 && Main.tile[bottomOfDoor, y2].type != 73 && Main.tile[bottomOfDoor, y2].type != 74 && Main.tile[bottomOfDoor, y2].type != 110 && Main.tile[bottomOfDoor, y2].type != 113 && Main.tile[bottomOfDoor, y2].type != 115 && Main.tile[bottomOfDoor, y2].type != TileID.Stalactite)
						return false;
					
					KillTile(bottomOfDoor, y2, ref undo);

				}

			}

			if (Main.netMode != NetmodeID.MultiplayerClient && Wiring.running) {

				Wiring.SkipWire(xPos, yPos);
				Wiring.SkipWire(xPos, yPos + 1);
				Wiring.SkipWire(xPos, yPos + 2);
				Wiring.SkipWire(xPos + 1, yPos);
				Wiring.SkipWire(xPos + 1, yPos + 1);
				Wiring.SkipWire(xPos + 1, yPos + 2);

			}

			int frameY = num4 % 36 * 54;

			Main.PlaySound(SoundID.DoorOpen, i * 16, j * 16);

			ushort type = (ushort)TileLoader.OpenDoorID(Main.tile[i, j]);

			undo.Add(new ChangedTile(xPos, yPos));

			Main.tile[xPos, yPos].active(active: true);
			Main.tile[xPos, yPos].type = type;
			Main.tile[xPos, yPos].frameY = (short)frameY;
			Main.tile[xPos, yPos].frameX = frameX;
			Main.tile[xPos, yPos].color(color);

			if (Main.tile[xPos + 1, yPos] == null)
				Main.tile[xPos + 1, yPos] = new Tile();

			undo.Add(new ChangedTile(xPos + 1, yPos));

			Main.tile[xPos + 1, yPos].active(active: true);
			Main.tile[xPos + 1, yPos].type = type;
			Main.tile[xPos + 1, yPos].frameY = (short)frameY;
			Main.tile[xPos + 1, yPos].frameX = (short)(frameX + 18);
			Main.tile[xPos + 1, yPos].color(color);

			if (Main.tile[xPos, yPos + 1] == null)
				Main.tile[xPos, yPos + 1] = new Tile();

			undo.Add(new ChangedTile(xPos, yPos + 1));

			Main.tile[xPos, yPos + 1].active(active: true);
			Main.tile[xPos, yPos + 1].type = type;
			Main.tile[xPos, yPos + 1].frameY = (short)(frameY + 18);
			Main.tile[xPos, yPos + 1].frameX = frameX;
			Main.tile[xPos, yPos + 1].color(color2);

			if (Main.tile[xPos + 1, yPos + 1] == null)
				Main.tile[xPos + 1, yPos + 1] = new Tile();

			undo.Add(new ChangedTile(xPos + 1, yPos + 1));

			Main.tile[xPos + 1, yPos + 1].active(active: true);
			Main.tile[xPos + 1, yPos + 1].type = type;
			Main.tile[xPos + 1, yPos + 1].frameY = (short)(frameY + 18);
			Main.tile[xPos + 1, yPos + 1].frameX = (short)(frameX + 18);
			Main.tile[xPos + 1, yPos + 1].color(color2);

			if (Main.tile[xPos, yPos + 2] == null)
				Main.tile[xPos, yPos + 2] = new Tile();

			undo.Add(new ChangedTile(xPos, yPos + 2));

			Main.tile[xPos, yPos + 2].active(active: true);
			Main.tile[xPos, yPos + 2].type = type;
			Main.tile[xPos, yPos + 2].frameY = (short)(frameY + 36);
			Main.tile[xPos, yPos + 2].frameX = frameX;
			Main.tile[xPos, yPos + 2].color(color3);

			if (Main.tile[xPos + 1, yPos + 2] == null)
				Main.tile[xPos + 1, yPos + 2] = new Tile();

			undo.Add(new ChangedTile(xPos + 1, yPos + 2));

			Main.tile[xPos + 1, yPos + 2].active(active: true);
			Main.tile[xPos + 1, yPos + 2].type = type;
			Main.tile[xPos + 1, yPos + 2].frameY = (short)(frameY + 36);
			Main.tile[xPos + 1, yPos + 2].frameX = (short)(frameX + 18);
			Main.tile[xPos + 1, yPos + 2].color(color3);

			for (int x2 = xPos - 1; x2 <= xPos + 2; x2++)
				for (int y2 = yPos - 1; y2 <= yPos + 2; y2++)
					WorldGen.TileFrame(x2, y2);

			return true;

		}

	}

}
