using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020

        public static bool DrunkWorldGen;
        public static bool GetGoodWorldGen;
		public static bool MudWall;

		public static int BeachDistance = 380;
		public static int GrassSpread;

		public static bool WillWaterPlacedHereStayPut(int x, int y) { // Updated v1.4 7/26/2020 Copy/Paste

			Tile tileBelow = Main.tile[x, y + 1];
			Tile tileLeft = Main.tile[x - 1, y];
			Tile tileRight = Main.tile[x + 1, y];

			if (
				(
					(tileBelow.active() && Main.tileSolid[tileBelow.type] && !Main.tileSolidTop[tileBelow.type]) ||
					tileBelow.liquid == byte.MaxValue)
					&&
				(
					(tileLeft.active() && Main.tileSolid[tileLeft.type] && !Main.tileSolidTop[tileLeft.type]) ||
					tileLeft.liquid == byte.MaxValue)
					&&
				(
					(tileRight.active() && Main.tileSolid[tileRight.type] && !Main.tileSolidTop[tileRight.type]) ||
					tileRight.liquid == byte.MaxValue)
				) {
				return true;

			}

			return false;

		}

	}

}
