using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020 Copy/Paste

		public static bool Pyramid(int i, int j, ref UndoStep undo, ushort tileType = TileID.SandstoneBrick, ushort wallType = WallID.SandstoneBrick, byte potType = 25) {

			UnifiedRandom genRand = WorldGen.genRand;

			int tileTop    = j - genRand.Next(0, 7);
			int tileBottom = j + genRand.Next(75, 125);
			int widthHalf  = genRand.Next(9, 13);
			int stepWidth  = 1;

			int counter = 1;

			if (tileType == TileID.LihzahrdBrick) {
				stepWidth = 4;
				tileBottom = j + genRand.Next(56, 94);
			}

			for (int yPos = tileTop; yPos < tileBottom; yPos++) {

				for (int xPos = i - stepWidth; xPos < i + stepWidth - 1; xPos++) {

					undo.Add(new ChangedTile(xPos, yPos));
					
					Main.tile[xPos, yPos].type = tileType;
					Main.tile[xPos, yPos].active(active: true);
					Main.tile[xPos, yPos].halfBrick(halfBrick: false);
					Main.tile[xPos, yPos].slope(0);

				}

				if (tileType == TileID.LihzahrdBrick && counter % 4 == 0)
					stepWidth += 4;

				if (tileType != TileID.LihzahrdBrick)
					stepWidth++;

				counter++;

			}

			for (int xPos = i - stepWidth - 5; xPos <= i + stepWidth + 5; xPos++) {

				for (int yPos = j - 1; yPos <= tileBottom + 1; yPos++) {

					bool flag = true;

					for (int num6 = xPos - 1; num6 <= xPos + 1; num6++)
						for (int num7 = yPos - 1; num7 <= yPos + 1; num7++)
							if (Main.tile[num6, num7].type != tileType)
								flag = false;

					if (flag) {

						undo.Add(new ChangedTile(xPos, yPos));

						Main.tile[xPos, yPos].wall = wallType;
						WorldGen.SquareWallFrame(xPos, yPos);

					}

				}

			}

			int direction = 1;

			if (genRand.Next(2) == 0)
				direction = -1;
			
			int xPos2  = i - widthHalf * direction;
			int startY = j + widthHalf;
			int height = genRand.Next(5, 8);
			bool flag2 = true;
			int num12  = genRand.Next(20, 30);

			while (flag2) {

				flag2 = false;
				bool flag3 = false;

				for (int yPos = startY; yPos <= startY + height; yPos++) {


                    if (Main.tile[xPos2, yPos - 1].type == (tileType == TileID.LihzahrdBrick ? TileID.Mud : TileID.Sand))
						flag3 = true;
					
					if (Main.tile[xPos2, yPos].type == tileType) {

						undo.Add(new ChangedTile(xPos2, yPos + 1));
						undo.Add(new ChangedTile(xPos2 + direction, yPos));
						undo.Add(new ChangedTile(xPos2, yPos));

                        Main.tile[xPos2            , yPos + 1].wall = wallType;
                        Main.tile[xPos2 + direction, yPos    ].wall = wallType;
                        Main.tile[xPos2            , yPos    ].active(active: false);

						flag2 = true;

					}

					if (flag3) {

						undo.Add(new ChangedTile(xPos2, yPos));

                        Main.tile[xPos2, yPos].type = (tileType == TileID.LihzahrdBrick ? TileID.Mud : TileID.Sand);
                        Main.tile[xPos2, yPos].active(active: true);
                        Main.tile[xPos2, yPos].halfBrick(halfBrick: false);
                        Main.tile[xPos2, yPos].slope(0);

					}

				}

				xPos2 -= direction;

			}

			xPos2 = i - widthHalf * direction;

			bool flag4 = true;
			bool flag5 = false;

			flag2 = true;
			
			while (flag2) {

				for (int yPos = startY; yPos <= startY + height; yPos++) {

					int xPos = xPos2;

					undo.Add(new ChangedTile(xPos, yPos));

					Main.tile[xPos, yPos].active(active: false);

				}

				xPos2 += direction;
				startY++;
				num12--;

				if (startY >= tileBottom - height * 2)
					num12 = 10;
				
				if (num12 <= 0) {

					bool flag6 = false;

					if (!flag4 && !flag5) {

						flag5 = true;
						flag6 = true;

						int num17 = genRand.Next(7, 13);
						int num18 = genRand.Next(23, 28);
						int num19 = num18;
						int num20 = xPos2;

						while (num18 > 0) {

							for (int num21 = startY - num17 + height; num21 <= startY + height; num21++) {
								
								if (num18 == num19 || num18 == 1) {

									if (num21 >= startY - num17 + height + 2) {

										undo.Add(new ChangedTile(xPos2, num21));

										Main.tile[xPos2, num21].active(active: false);

									}
									
								}
								else if (num18 == num19 - 1 || num18 == 2 || num18 == num19 - 2 || num18 == 3) {

									if (num21 >= startY - num17 + height + 1) {

										undo.Add(new ChangedTile(xPos2, num21));

										Main.tile[xPos2, num21].active(active: false);

									}
									
								}
								else {

									undo.Add(new ChangedTile(xPos2, num21));

									Main.tile[xPos2, num21].active(active: false);

								}

							}

							num18--;
							xPos2 += direction;

						}

						int num22 = xPos2 - direction;
						int num23 = num22;
						int num24 = num20;

						if (num22 > num20) {
							num23 = num20;
							num24 = num22;
						}

						int num25 = genRand.Next(3);
						
						switch (num25) {
							case 0:
								num25 = ItemID.SandstorminaBottle;
								break;
							case 1:
								num25 = ItemID.PharaohsMask;
								break;
							case 2:
								num25 = ItemID.FlyingCarpet;
								break;
						}

						AddBuriedChest((num23 + num24) / 2, startY, ref undo, num25, notNearOtherChests: false, tileType == TileID.LihzahrdBrick ? 16 : 1, trySlope: false, 0);
						
						int num26 = genRand.Next(1, 10);
						
						for (int num27 = 0; num27 < num26; num27++) {

							int i2 = genRand.Next(num23, num24);
							int j2 = startY + height;

							PlaceSmallPile(i2, j2, genRand.Next(16, 19), 1, ref undo, TileID.SmallPiles);

						}

						if (tileType != 226) {

							PlaceTile(num23 + 2, startY - num17 + height + 1, TileID.Banners, ref undo, mute: true, forced: false, -1, genRand.Next(4, 7));
							PlaceTile(num23 + 3, startY - num17 + height,     TileID.Banners, ref undo, mute: true, forced: false, -1, genRand.Next(4, 7));
							PlaceTile(num24 - 2, startY - num17 + height + 1, TileID.Banners, ref undo, mute: true, forced: false, -1, genRand.Next(4, 7));
							PlaceTile(num24 - 3, startY - num17 + height,     TileID.Banners, ref undo, mute: true, forced: false, -1, genRand.Next(4, 7));

						}

						for (int num28 = num23; num28 <= num24; num28++)
							PlacePot(num28, startY + height, ref undo, 28, genRand.Next(potType, potType + 3));
						
					}

					if (flag4) {

						flag4 = false;
						direction *= -1;
						num12 = genRand.Next(15, 20);

					}
					else if (flag6) {

						num12 = genRand.Next(10, 15);

					}
					else {

						direction *= -1;
						num12 = genRand.Next(20, 40);

					}

				}

				if (startY >= tileBottom - height)
					flag2 = false;
				
			}

			int num29 = genRand.Next(100, 200);
			int num30 = genRand.Next(500, 800);

			flag2 = true;

			int num31 = height;

			num12 = genRand.Next(10, 50);
			
			if (direction == 1)
				xPos2 -= num31;
			
			int num32 = genRand.Next(5, 10);

			while (flag2) {

				num29--;
				num30--;
				num12--;

				for (int num33 = xPos2 - num32 - genRand.Next(0, 2); num33 <= xPos2 + num31 + num32 + genRand.Next(0, 2); num33++) {
					
					int num34 = startY;
					
					if (num33 >= xPos2 && num33 <= xPos2 + num31) {

						undo.Add(new ChangedTile(num33, num34));

						Main.tile[num33, num34].active(active: false);

					}
					else {

						undo.Add(new ChangedTile(num33, num34));

						Main.tile[num33, num34].type = tileType;
						Main.tile[num33, num34].active(active: true);
						Main.tile[num33, num34].halfBrick(halfBrick: false);
						Main.tile[num33, num34].slope(0);

					}

					if (num33 >= xPos2 - 1 && num33 <= xPos2 + 1 + num31) {

						undo.Add(new ChangedTile(num33, num34));

						Main.tile[num33, num34].wall = wallType;

					}
					
				}

				startY++;
				xPos2 += direction;

				if (num29 <= 0) {

					flag2 = false;

					for (int num35 = xPos2 + 1; num35 <= xPos2 + num31 - 1; num35++)
						if (Main.tile[num35, startY].active())
							flag2 = true;

				}

				if (num12 < 0) {
					num12 = genRand.Next(10, 50);
					direction *= -1;
				}

				if (num30 <= 0)
					flag2 = false;
				
			}

			undo.ResetFrames();

			return true;

		}

	}

}
