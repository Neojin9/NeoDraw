using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen { // Updated v1.4 7/23/2020

    public partial class WldGen {

		public static void SmoothSlope(int x, int y, ref UndoStep undo, bool applyToNeighbors = true, bool sync = false) {

			if (applyToNeighbors) {
				SmoothSlope(x + 1, y, ref undo, applyToNeighbors: false, sync);
				SmoothSlope(x - 1, y, ref undo, applyToNeighbors: false, sync);
				SmoothSlope(x, y + 1, ref undo, applyToNeighbors: false, sync);
				SmoothSlope(x, y - 1, ref undo, applyToNeighbors: false, sync);
			}

			Tile tile = Main.tile[x, y];
			
			if (!WldUtils.WldUtils.CanPoundTile(x, y) || !WorldGen.SolidOrSlopedTile(x, y))
				return;

			bool tilePresent  = !WorldGen.TileEmpty(x, y - 1);

			bool topSloped    = !WorldGen.SolidOrSlopedTile(x, y - 1) && tilePresent;
			bool bottomSloped = WorldGen.SolidOrSlopedTile(x, y + 1);
			bool leftSloped   = WorldGen.SolidOrSlopedTile(x - 1, y);
			bool rightSloped  = WorldGen.SolidOrSlopedTile(x + 1, y);

			bool isHalfBrick = tile.halfBrick();
			int slopeStyle   = tile.slope();

			undo.Add(new ChangedTile(x, y));

			switch (((tilePresent ? 1 : 0) << 3) | ((bottomSloped ? 1 : 0) << 2) | ((leftSloped ? 1 : 0) << 1) | (rightSloped ? 1 : 0)) {

				case 10:
					if (!topSloped) {
						tile.halfBrick(halfBrick: false);
						tile.slope(3);
					}
					break;

				case 9:
					if (!topSloped) {
						tile.halfBrick(halfBrick: false);
						tile.slope(4);
					}
					break;

				case 6:
					tile.halfBrick(halfBrick: false);
					tile.slope(1);
					break;

				case 5:
					tile.halfBrick(halfBrick: false);
					tile.slope(2);
					break;

				case 4:
					tile.slope(0);
					tile.halfBrick(halfBrick: true);
					break;

				default:
					tile.halfBrick(halfBrick: false);
					tile.slope(0);
					break;

			}

			if (sync) {

				int num3 = tile.slope();
				bool flag7 = isHalfBrick != tile.halfBrick();
				bool flag8 = slopeStyle != num3;

				if (flag7 && flag8) {
					NetMessage.SendData(MessageID.TileChange, -1, -1, null, 23, x, y, num3);
				}
				else if (flag7) {
					NetMessage.SendData(MessageID.TileChange, -1, -1, null, 7, x, y, 1f);
				}
				else if (flag8) {
					NetMessage.SendData(MessageID.TileChange, -1, -1, null, 14, x, y, num3);
				}

			}

		}

	}

}
