using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/27/2020 Modified

		public static bool SandTrap(int x, int y, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

            int curY;

            for (curY = y; !Main.tile[x, curY].active() && curY < Neo.UnderworldLayer; curY++) { }

			if (!Main.tileSolid[Main.tile[x, curY].type] && !Main.tileCut[Main.tile[x, curY].type] && Main.tile[x, curY].type != TileID.Stalactite && Main.tile[x, curY].type != TileID.SmallPiles && Main.tile[x, curY].type != TileID.LargePiles && Main.tile[x, curY].type != TileID.LargePiles2) {
				DrawInterface.SetStatusBarTempMessage("Must be placed in empty area.");
				return false;
			}

			curY--;

			int ceilingHeight = -1;
			int halfWidth = genRand.Next(6, 12);
			int trapHeight = 14;
	
			for (int num8 = curY; num8 > curY - 30; num8--) {

				if (Main.tile[x, num8].active()) {

					ceilingHeight = num8;
					break;

				}

			}

			if (ceilingHeight == -1) {
				DrawInterface.SetStatusBarTempMessage("No ceiling found above.");
				return false;
			}

			if (curY - ceilingHeight < trapHeight + 4) {
				DrawInterface.SetStatusBarTempMessage("Ceiling is too close.");
				return false;
			}
			
			int yPos = (curY + ceilingHeight) / 2;

CheckIfWideEnough:;

			int num9 = 0;

			if (halfWidth < 6) {

				DrawInterface.SetStatusBarTempMessage("Space above is not wide enough.");
				return false;

			}

			for (int xPos = x - halfWidth; xPos <= x + halfWidth; xPos++) {

				if (Main.tile[xPos, yPos].active() && Main.tileSolid[Main.tile[xPos, yPos].type]) {
					halfWidth--;
					goto CheckIfWideEnough;
				}
				
				for (int yPos2 = ceilingHeight - trapHeight; yPos2 <= ceilingHeight; yPos2++)
					if (Main.tile[xPos, yPos2].active())
						if (Main.tileSolid[Main.tile[xPos, yPos2].type])
							num9++;
				
			}

			for (int num14 = x - halfWidth - 1; num14 <= x + halfWidth + 1; num14++) {

				for (int num15 = ceilingHeight - trapHeight; num15 <= ceilingHeight; num15++) {

					bool flag = false;

					if (Main.tile[num14, num15].active() && Main.tileSolid[Main.tile[num14, num15].type])
						flag = true;

					undo.Add(new ChangedTile(num14, num15));

					if (num15 == ceilingHeight) {

						Main.tile[num14, num15].slope(0);
						Main.tile[num14, num15].halfBrick(halfBrick: false);

						if (!flag) {
							Main.tile[num14, num15].active(active: true);
							Main.tile[num14, num15].type = TileID.Sandstone;
						}

					}
					else if (num15 == ceilingHeight - trapHeight) {

						Main.tile[num14, num15].ClearTile();
						Main.tile[num14, num15].active(active: true);

						if (flag && Main.tile[num14, num15 - 1].active() && Main.tileSolid[Main.tile[num14, num15 - 1].type]) {
							Main.tile[num14, num15].type = TileID.HardenedSand;
						}
						else {
							Main.tile[num14, num15].type = TileID.Sandstone;
						}

					}
					else if (num14 == x - halfWidth - 1 || num14 == x + halfWidth + 1) {

						if (!flag) {
							Main.tile[num14, num15].ClearTile();
							Main.tile[num14, num15].active(active: true);
							Main.tile[num14, num15].type = 396;
						}
						else {
							Main.tile[num14, num15].slope(0);
							Main.tile[num14, num15].halfBrick(halfBrick: false);
						}

					}
					else {

						Main.tile[num14, num15].ClearTile();
						Main.tile[num14, num15].active(active: true);
						Main.tile[num14, num15].type = 53;

					}

				}

			}

			for (int num16 = (int)(ceilingHeight - trapHeight * 0.666f); num16 <= ceilingHeight - (float)trapHeight * 0.333; num16++) {

				undo.Add(new ChangedTile(x - halfWidth - 2, num16));

				if (num16 < ceilingHeight - trapHeight * 0.4f) {
					if (Main.tile[x - halfWidth - 2, num16].bottomSlope()) {
						Main.tile[x - halfWidth - 2, num16].slope(0);
					}
				}
				else if (num16 > ceilingHeight - trapHeight * 0.6f) {
					if (Main.tile[x - halfWidth - 2, num16].topSlope()) {
						Main.tile[x - halfWidth - 2, num16].slope(0);
					}
					Main.tile[x - halfWidth - 2, num16].halfBrick(halfBrick: false);
				}
				else {
					Main.tile[x - halfWidth - 2, num16].halfBrick(halfBrick: false);
					Main.tile[x - halfWidth - 2, num16].slope(0);
				}

				if (!Main.tile[x - halfWidth - 2, num16].active() || !Main.tileSolid[Main.tile[x - halfWidth - 2, num16].type]) {
					Main.tile[x - halfWidth - 2, num16].active(active: true);
					Main.tile[x - halfWidth - 2, num16].type = 396;
				}

				if (!Main.tile[x + halfWidth + 2, num16].active() || !Main.tileSolid[Main.tile[x + halfWidth + 2, num16].type]) {
					undo.Add(new ChangedTile(x + halfWidth + 2, num16));
					Main.tile[x + halfWidth + 2, num16].active(active: true);
					Main.tile[x + halfWidth + 2, num16].type = 396;
				}

			}

			for (int num17 = ceilingHeight - trapHeight; num17 <= ceilingHeight; num17++) {

				undo.Add(new ChangedTile(x - halfWidth - 2, num17));
				undo.Add(new ChangedTile(x - halfWidth - 1, num17));
				undo.Add(new ChangedTile(x - halfWidth + 1, num17));
				undo.Add(new ChangedTile(x - halfWidth + 2, num17));

				Main.tile[x - halfWidth - 2, num17].slope(0);
				Main.tile[x - halfWidth - 2, num17].halfBrick(halfBrick: false);
				Main.tile[x - halfWidth - 1, num17].slope(0);
				Main.tile[x - halfWidth - 1, num17].halfBrick(halfBrick: false);
				Main.tile[x - halfWidth + 1, num17].slope(0);
				Main.tile[x - halfWidth + 1, num17].halfBrick(halfBrick: false);
				Main.tile[x - halfWidth + 2, num17].slope(0);
				Main.tile[x - halfWidth + 2, num17].halfBrick(halfBrick: false);

			}

			for (int num18 = x - halfWidth - 1; num18 < x + halfWidth + 1; num18++) {

				int num19 = curY - trapHeight - 1;

				undo.Add(new ChangedTile(num18, num19));

				if (Main.tile[num18, num19].bottomSlope())
					Main.tile[num18, num19].slope(0);

				Main.tile[num18, num19].halfBrick(halfBrick: false);

			}

			ushort GroundTileType = Main.tile[x, curY + 1].type;
			bool SolidGround = false;

			if (Main.tileSolid[GroundTileType])
				SolidGround = true;

			WorldGen.KillTile(x - 2, curY);
			WorldGen.KillTile(x - 1, curY);
			WorldGen.KillTile(x, curY);
			WorldGen.KillTile(x + 1, curY);
			WorldGen.KillTile(x + 2, curY);
			
			if (SolidGround) {

				undo.Add(new ChangedTile(x, curY + 1));

				Main.tile[x, curY + 1].halfBrick(halfBrick: false);
				Main.tile[x, curY + 1].slope(0);

				if (!Main.tile[x - 2, curY + 1].active() && Main.tile[x - 2, curY + 2].active() && Main.tileSolid[Main.tile[x - 2, curY + 2].type]) {

					undo.Add(new ChangedTile(x - 2, curY + 1));

					Main.tile[x - 2, curY + 1].type = GroundTileType;
					Main.tile[x - 2, curY + 1].active(active: true);
					Main.tile[x - 2, curY + 1].halfBrick(halfBrick: false);
					Main.tile[x - 2, curY + 1].slope(0);
                }

				if (!Main.tile[x - 1, curY + 1].active() && Main.tile[x - 1, curY + 2].active() && Main.tileSolid[Main.tile[x - 1, curY + 2].type]) {

					undo.Add(new ChangedTile(x - 1, curY + 1));

					Main.tile[x - 1, curY + 1].type = GroundTileType;
					Main.tile[x - 1, curY + 1].active(active: true);
					Main.tile[x - 1, curY + 1].halfBrick(halfBrick: false);
					Main.tile[x - 1, curY + 1].slope(0);
				}

				if (!Main.tile[x + 1, curY + 1].active() && Main.tile[x + 1, curY + 2].active() && Main.tileSolid[Main.tile[x + 1, curY + 2].type]) {

					undo.Add(new ChangedTile(x + 1, curY + 1));

					Main.tile[x + 1, curY + 1].type = GroundTileType;
					Main.tile[x + 1, curY + 1].active(active: true);
					Main.tile[x + 1, curY + 1].halfBrick(halfBrick: false);
					Main.tile[x + 1, curY + 1].slope(0);
				}

				if (!Main.tile[x + 2, curY + 1].active() && Main.tile[x + 2, curY + 2].active() && Main.tileSolid[Main.tile[x + 2, curY + 2].type]) {

					undo.Add(new ChangedTile(x + 2, curY + 1));

					Main.tile[x + 2, curY + 1].type = GroundTileType;
					Main.tile[x + 2, curY + 1].active(active: true);
					Main.tile[x + 2, curY + 1].halfBrick(halfBrick: false);
					Main.tile[x + 2, curY + 1].slope(0);
				}

			}

			PlaceTile(x, curY, TileID.PressurePlates, ref undo, mute: true, forced: false, -1, 7);

			for (int num20 = x - halfWidth; num20 <= x + halfWidth; num20++) {

				int num21 = curY;

				if (num20 < x - halfWidth * 0.8 || num20 > x + halfWidth * 0.8) {
					num21 = curY - 3;
				}
				else if (num20 < x - halfWidth * 0.6 || num20 > x + halfWidth * 0.6) {
					num21 = curY - 2;
				}
				else if (num20 < x - halfWidth * 0.4 || num20 > x + halfWidth * 0.4) {
					num21 = curY - 1;
				}

				for (int num22 = ceilingHeight; num22 <= curY; num22++) {

					if (num20 == x && num22 <= curY) {
						undo.Add(new ChangedTile(x, num22));
						Main.tile[x, num22].wire(wire: true);
					}

					if (Main.tile[num20, num22].active() && Main.tileSolid[Main.tile[num20, num22].type]) {

						if (num22 < ceilingHeight + 6 - 4) {
							undo.Add(new ChangedTile(num20, num22));
							Main.tile[num20, num22].actuator(actuator: true);
							Main.tile[num20, num22].wire(wire: true);
						}
						else if (num22 < num21) {
							WorldGen.KillTile(num20, num22);
						}

					}

				}

			}

			undo.ResetFrames();

			return true;

		}

	}

}
