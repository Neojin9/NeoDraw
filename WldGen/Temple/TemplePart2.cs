using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/25/2020 Copy/Paste

		public static void templePart2(ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			int minValueX = tLeft;
			int maxValueX = tRight;
			int minValueY = tTop;
			int maxValueY = tBottom;

			int templeRooms = tRooms;

			float trapsToPlace = templeRooms * 1.9f;
			trapsToPlace *= 1f + genRand.Next(-15, 16) * 0.01f;

			int attampts = 0;

			while (trapsToPlace > 0f) {

				int xPos = genRand.Next(minValueX, maxValueX);
				int yPos = genRand.Next(minValueY, maxValueY);

				if (Main.tile[xPos, yPos].wall == WallID.LihzahrdBrickUnsafe && !Main.tile[xPos, yPos].active()) {

					if (mayanTrap(xPos, yPos, ref undo)) {

						trapsToPlace -= 1f;
						attampts = 0;

					}
					else {

						attampts++;

					}

				}
				else {

					attampts++;

				}

				if (attampts > 100) {

					attampts = 0;
					trapsToPlace -= 1f;

				}

			}

			Main.tileSolid[TileID.WoodenSpikes] = false;

			float chestsToPlace = templeRooms * 0.35f;
			chestsToPlace *= 1f + genRand.Next(-15, 16) * 0.01f;
			
			int contain = ItemID.EmeraldHook;

			attampts = 0;
			
			while (chestsToPlace > 0f) {

				int xPos = genRand.Next(minValueX, maxValueX);
				int yPos = genRand.Next(minValueY, maxValueY);

				if (Main.tile[xPos, yPos].wall == WallID.LihzahrdBrickUnsafe && !Main.tile[xPos, yPos].active() && AddBuriedChest(xPos, yPos, ref undo, contain, notNearOtherChests: true, 16, trySlope: false, 0)) {

					chestsToPlace -= 1f;
					attampts = 0;

				}

				attampts++;

				if (attampts > 10000)
					break;
				
			}

			float statuesToPlace = templeRooms * 1.25f;
			statuesToPlace *= 1f + genRand.Next(-25, 36) * 0.01f;

			attampts = 0;

			while (statuesToPlace > 0f) {

				attampts++;

				int xPos = genRand.Next(minValueX, maxValueX);
				int yPos = genRand.Next(minValueY, maxValueY);

				if (Main.tile[xPos, yPos].wall != WallID.LihzahrdBrickUnsafe || Main.tile[xPos, yPos].active())
					continue;

                int yPos2 = yPos;

				while (!Main.tile[xPos, yPos2].active()) {

					yPos2++;

					if (yPos2 > maxValueY)
						break;
					
				}

				yPos2--;

				if (yPos2 <= maxValueY) {

                    PlaceTile(xPos, yPos2, TileID.Statues, ref undo, mute: true, forced: false, -1, genRand.Next(43, 46));

					if (Main.tile[xPos, yPos2].type == TileID.Statues)
						statuesToPlace -= 1f;
					
				}
			}

			float furnitureToPlace = templeRooms * 1.35f;
			furnitureToPlace *= 1f + genRand.Next(-15, 26) * 0.01f;

			attampts = 0;

			while (furnitureToPlace > 0f) {

				attampts++;

				int xPos = genRand.Next(minValueX, maxValueX);
				int yPos = genRand.Next(minValueY, maxValueY);

				if (Main.tile[xPos, yPos].wall == WallID.LihzahrdBrickUnsafe && !Main.tile[xPos, yPos].active()) {

                    int yPos2 = yPos;

					while (!Main.tile[xPos, yPos2].active()) {

						yPos2++;

						if (yPos2 > maxValueY)
							break;
						
					}

					yPos2--;

					if (yPos2 <= maxValueY) {

						switch (genRand.Next(3)) {

							case 0:

                                PlaceTile(xPos, yPos2, TileID.WorkBenches, ref undo, mute: true, forced: false, -1, 10);

								if (Main.tile[xPos, yPos2].type == TileID.WorkBenches)
									furnitureToPlace -= 1f;
								
								break;

							case 1:

                                PlaceTile(xPos, yPos2, TileID.Tables, ref undo, mute: true, forced: false, -1, 9);

								if (Main.tile[xPos, yPos2].type == TileID.Tables)
									furnitureToPlace -= 1f;
								
								break;

							case 2:

                                PlaceTile(xPos, yPos2, TileID.Chairs, ref undo, mute: true, forced: false, -1, 12);

								if (Main.tile[xPos, yPos2].type == TileID.Chairs)
									furnitureToPlace -= 1f;
								
								break;

						}

					}

				}

				if (attampts > 10000)
					break;
				
			}

			Main.tileSolid[TileID.WoodenSpikes] = true;

		}

	}

}
