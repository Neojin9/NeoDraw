using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen { // Updated v1.4 7/28/2020 Copy/Paste Modified Heavily

    public partial class WldGen {

		public static int LavaTrapVolumeCheckWidth = 5;
		public static int MaxLavaTrapHeight = 40;
		public static int MinLavaTrapHeight = 4;
		public static int MinLavaTrapVolume = 25;

		public static bool LavaTrap(int x, int y, ref UndoStep undo) {

			if (x - LavaTrapVolumeCheckWidth < 0 || x + LavaTrapVolumeCheckWidth > Main.maxTilesX) {
				DrawInterface.SetStatusBarTempMessage("Trap too close to edge of world.");
				return false;
            }

			int startY = y;

			while (!Main.tile[x, y].active() || !Main.tileSolid[Main.tile[x, y].type]) {

				y++;

				if (y > Main.maxTilesY) {
					DrawInterface.SetStatusBarTempMessage("Trap too close to bottom of world.");
					return false;
                }

				if (y - startY > 20) {
					DrawInterface.SetStatusBarTempMessage("Gound not found below.");
					return false;
				}

            }

			y--;

			if (Main.tile[x, y].active() && !Main.tileCut[Main.tile[x, y].type] && Main.tile[x, y].type != TileID.Stalactite && Main.tile[x, y].type != TileID.SmallPiles && Main.tile[x, y].type != TileID.LargePiles && Main.tile[x, y].type != TileID.LargePiles2) {

				DrawInterface.SetStatusBarTempMessage("Starting area blocked.");
				return false;

            }

			int pressurePlateY = y;
			startY = y;

			y--;

			while (!Main.tile[x, y].active() || !Main.tileSolid[Main.tile[x, y].type]) {

				y--;

				if (y < 10) {
					DrawInterface.SetStatusBarTempMessage("Trap too close to top of world.");
					return false;
                }

				if (startY - y > MaxLavaTrapHeight) {
					DrawInterface.SetStatusBarTempMessage("Ceiling not close enough.");
					return false;
                }

            }

			int ceilingStartY = y;

			if (pressurePlateY - ceilingStartY < MinLavaTrapHeight) {
				DrawInterface.SetStatusBarTempMessage("Not enough clearance between floor and ceiling.");
				return false;
            }

			startY = y;

			y--;

			while (Main.tile[x, y].liquid < 255 || !Main.tile[x, y].lava()) {
				
				y--;

				if (y < 10) {
					DrawInterface.SetStatusBarTempMessage("Trap too close to top of world.");
					return false;
				}


				if (startY - y > 20) {
					DrawInterface.SetStatusBarTempMessage("No lava found above ceiling.");
					return false;
                }

            }

			int lavaPoolStartY = y;

			int lavaVolume = 0;

			for (int i = x - LavaTrapVolumeCheckWidth; i <= x + LavaTrapVolumeCheckWidth; i++)
				for (int j = y - LavaTrapVolumeCheckWidth * 2; j <= y; j++)
					if (!Main.tile[i, j].active() && Main.tile[i, j].lava())
						lavaVolume++;

			if (lavaVolume < MinLavaTrapVolume) {
				DrawInterface.SetStatusBarTempMessage("Trap requires more lava.");
				return false;
			}

			if (Main.tile[x, pressurePlateY].active())
				WorldGen.KillTile(x, pressurePlateY);
			
			if (Main.tile[x, pressurePlateY].active()) {
				DrawInterface.SetStatusBarTempMessage("Unable to place Pressure Plate on floor.");
				return false;
            }

			bool wire1 = true;
			bool wire2 = true;
			bool wire3 = true;
			bool wire4 = true;

			for (int x2 = x - 2; x2 <= x + 2; x2++) {
				for (int y2 = y - 2; y2 <= pressurePlateY + 2; y2++) {
					if (Main.tile[x2, y2].wire())
						wire1 = false;
					if (Main.tile[x2, y2].wire2())
						wire2 = false;
					if (Main.tile[x2, y2].wire3())
						wire3 = false;
					if (Main.tile[x2, y2].wire4())
						wire4 = false;
                }
            }

			if (!wire1 && !wire2 && !wire3 && !wire4) {
				DrawInterface.SetStatusBarTempMessage("All wire types in use.");
				return false;
            }

			undo.Add(new ChangedTile(x, pressurePlateY + 1));

			Main.tile[x, pressurePlateY + 1].halfBrick(halfBrick: false);
			Main.tile[x, pressurePlateY + 1].slope(0);

			PlaceTile(x, pressurePlateY, TileID.PressurePlates, ref undo, mute: false, forced: true, -1, 7);

			y++;

			while (y <= pressurePlateY) {

				undo.Add(new ChangedTile(x, y));

				if (wire1) {
					Main.tile[x, y].wire(wire: true);
				}
				else if (wire2) {
					Main.tile[x, y].wire2(wire2: true);
				}
				else if (wire3) {
					Main.tile[x, y].wire3(wire3: true);
				}
				else if (wire4) {
					Main.tile[x, y].wire4(wire4: true);
				}

				if (y <= ceilingStartY && Main.tile[x, y].active() && Main.tileSolid[Main.tile[x, y].type]) {
					Main.tile[x, y].slope(0);
					Main.tile[x, y].halfBrick(halfBrick: false);
					Main.tile[x, y].actuator(actuator: true);
				}

				y++;

			}

			return true;

		}

	}

}
