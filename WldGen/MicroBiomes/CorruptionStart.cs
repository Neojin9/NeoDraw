using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.MicroBiomes {

    public class CorruptionStart : MicroBiome {

        public static List<Vector2> OrbLocations = new List<Vector2>();

        public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			OrbLocations.Clear();

			UnifiedRandom genRand = WorldGen.genRand;

			int leftEdge  = origin.X - genRand.Next(200) - 100;
			int rightEdge = origin.X + genRand.Next(200) + 100;

			int stepsBetweenChasms = 0;
			
			for (int xPos = leftEdge; xPos < rightEdge; xPos++) {

				if (stepsBetweenChasms > 0)
					stepsBetweenChasms--;
				
				if (xPos == origin.X || stepsBetweenChasms == 0) {

					for (int yPos = origin.Y; yPos < Main.worldSurface - 1.0; yPos++) { // for (int yPos = (int)WorldGen.worldSurfaceLow; yPos < Main.worldSurface - 1.0; yPos++) {

						if (_tiles[xPos, yPos].active() || _tiles[xPos, yPos].wall > 0) {

							if (xPos == origin.X) {

								stepsBetweenChasms = 20;
								WldGen.ChasmRunner(xPos, yPos, genRand.Next(150) + 150, ref undo, makeOrb: true);

							}
							else if (genRand.Next(35) == 0 && stepsBetweenChasms == 0) {

								stepsBetweenChasms = 30;
								WldGen.ChasmRunner(xPos, yPos, genRand.Next(50) + 50, ref undo, makeOrb: true);

							}

							break;

						}

					}

				}

                #region Convert Jungle to Dirt
                /*for (int yPos = (int)WorldGen.worldSurfaceLow; yPos < Main.worldSurface - 1.0; yPos++) {

					if (_tiles[xPos, yPos].active()) {

						int bottomY = yPos + genRand.Next(10, 14);

						for (int currentY = yPos; currentY < bottomY; currentY++)
							if ((_tiles[xPos, currentY].type == TileID.Mud || _tiles[xPos, currentY].type == TileID.JungleGrass) && xPos >= leftEdge + genRand.Next(5) && xPos < rightEdge - genRand.Next(5))
								Neo.SetTile(xPos, currentY, TileID.Dirt, ref undo);
						
						break;

					}

				}*/
                #endregion

            }

            #region Convert adjacent area
            /*double curY = Main.worldSurface + 40.0;

			for (int xPos = leftEdge; xPos < rightEdge; xPos++) {

				curY += genRand.Next(-2, 3);

				if (curY < Main.worldSurface + 30.0)
					curY = Main.worldSurface + 30.0;

				if (curY > Main.worldSurface + 50.0)
					curY = Main.worldSurface + 50.0;

				bool flag51 = false;

				for (int yPos2 = (int)WorldGen.worldSurfaceLow; yPos2 < curY; yPos2++) {

					if (_tiles[xPos, yPos2].active()) {

						if (_tiles[xPos, yPos2].type == 53 && xPos >= leftEdge + genRand.Next(5) && xPos <= rightEdge - genRand.Next(5))
							Neo.SetTile(xPos, yPos2, 112, ref undo);
						
						if (_tiles[xPos, yPos2].type == TileID.Dirt && yPos2 < Main.worldSurface - 1.0 && !flag51) {
							WldGen.GrassSpread = 0;
							WldGen.SpreadGrass(xPos, yPos2, ref undo, 0, 23, repeat: true, 0);
						}

						flag51 = true;

						if (_tiles[xPos, yPos2].type == 1 && xPos >= leftEdge + genRand.Next(5) && xPos <= rightEdge - genRand.Next(5))
							Neo.SetTile(xPos, yPos2, 25, ref undo);

						if (_tiles[xPos, yPos2].wall == 216) {
							Neo.SetWall(xPos, yPos2, 217, ref undo);
						}
						else if (_tiles[xPos, yPos2].wall == 187) {
							Neo.SetWall(xPos, yPos2, 220, ref undo);
						}

						if (_tiles[xPos, yPos2].type == 2)
							Neo.SetTile(xPos, yPos2, 23, ref undo);

						if (_tiles[xPos, yPos2].type == 161) {
							Neo.SetTile(xPos, yPos2, 163, ref undo);
						}
						else if (_tiles[xPos, yPos2].type == 396) {
							Neo.SetTile(xPos, yPos2, 400, ref undo);
						}
						else if (_tiles[xPos, yPos2].type == 397) {
							Neo.SetTile(xPos, yPos2, 398, ref undo);
						}

					}

				}

			}*/
            #endregion

            #region Clear area around orbs
            for (int i = 0; i < OrbLocations.Count; i++) {

				int orbX = (int)OrbLocations[i].X;
				int orbY = (int)OrbLocations[i].Y;

				for (int k = orbX; k <= orbX + 1; k++) {

					for (int l = orbY; l <= orbY + 1; l++) {

						int leftSide  = k - 13;
						int rightSide = k + 13;
						int top       = l - 13;
						int bottom    = l + 13;

						for (int tileX = leftSide; tileX < rightSide; tileX++) {

							if (tileX > 10 && tileX < Main.maxTilesX - 10) {

								for (int tileY = top; tileY < bottom; tileY++) {

									if (Math.Abs(tileX - k) + Math.Abs(tileY - l) < 9 + genRand.Next(11) && genRand.Next(3) != 0 && _tiles[tileX, tileY].type != 31) {

										Neo.SetTile(tileX, tileY, TileID.Ebonstone, ref undo, active: true);

										if (Math.Abs(tileX - k) <= 1 && Math.Abs(tileY - l) <= 1)
											Neo.SetActive(tileX, tileY, false);

									}

									if (_tiles[tileX, tileY].type != 31 && Math.Abs(tileX - k) <= 2 + genRand.Next(3) && Math.Abs(tileY - l) <= 2 + genRand.Next(3))
										Neo.SetActive(tileX, tileY, false, ref undo);

								}

							}

						}

					}

				}

			}
            #endregion

            undo.ResetFrames(true);

			return true;

		}

	}

}
