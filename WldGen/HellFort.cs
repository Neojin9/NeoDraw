using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static void HellFort(int i, int j, ref UndoStep undo, ushort tileType = 75, byte wallType = 14, int doorStyle = 19, int platformStyle = 13) {

			UnifiedRandom genRand = WorldGen.genRand;

			const int MaxRoomsAcross = 5;
			const int MaxRoomsDown   = 10;

			int[] leftEdges   = new int[MaxRoomsAcross];
			int[] rightEdges  = new int[MaxRoomsAcross];
			int[] topEdges    = new int[MaxRoomsDown];
			int[] bottomEdges = new int[MaxRoomsDown];

			int minRoomWidth  = 8;
			int maxRoomWidth = 20;

			if (DrunkWorldGen) {

				minRoomWidth /= 2;
				maxRoomWidth *= 2;

			}

			leftEdges[2]  = i - genRand.Next(minRoomWidth / 2, maxRoomWidth / 2);
			rightEdges[2] = i + genRand.Next(minRoomWidth / 2, maxRoomWidth / 2);
			leftEdges[3]  = rightEdges[2];
			rightEdges[3] = leftEdges[3] + genRand.Next(minRoomWidth, maxRoomWidth);
			leftEdges[4]  = rightEdges[3];
			rightEdges[4] = leftEdges[4] + genRand.Next(minRoomWidth, maxRoomWidth);
			rightEdges[1] = leftEdges[2];
			leftEdges[1]  = rightEdges[1] - genRand.Next(minRoomWidth, maxRoomWidth);
			rightEdges[0] = leftEdges[1];
			leftEdges[0]  = rightEdges[0] - genRand.Next(minRoomWidth, maxRoomWidth);

			minRoomWidth  = 6;
			maxRoomWidth = 12;

			topEdges[0] = j - genRand.Next(minRoomWidth, maxRoomWidth);
			bottomEdges[0] = j;

			for (int index = 1; index < MaxRoomsDown; index++) {

				topEdges[index] = bottomEdges[index - 1];
				bottomEdges[index] = topEdges[index] + genRand.Next(minRoomWidth, maxRoomWidth);

			}

			//topEdges[3] = j - genRand.Next(minRoomWidth, maxRoomWidth);
			//bottomEdges[3] = j;
			//
			//for (int index = 4; index < MaxRoomsDown; index++) {
			//
			//	topEdges[index] = bottomEdges[index - 1];
			//	bottomEdges[index] = topEdges[index] + genRand.Next(minRoomWidth, maxRoomWidth);
			//
			//}
			//
			//for (int index = 2; index >= 0; index--) {
			//
			//	bottomEdges[index] = topEdges[index + 1];
			//	topEdges[index] = bottomEdges[index] - genRand.Next(minRoomWidth, maxRoomWidth);
			//
			//}

			bool flag  = false;
			bool flag2 = false;

			bool[,] roomExists = new bool[MaxRoomsAcross, MaxRoomsDown];

			int farLeft  = 3;
			int farRight = 3;

			for (int l = 0; l < 2; l++) {

				if (genRand.Next(3) == 0 || DrunkWorldGen) {

					flag = true;

					int roomY = genRand.Next(MaxRoomsDown);

					if (roomY < farLeft)
						farLeft = roomY;
					
					if (roomY > farRight)
						farRight = roomY;
					
					int roomX = 1;

					if (genRand.Next(2) == 0 || DrunkWorldGen) {

						roomExists[0, roomY] = true;
						roomExists[1, roomY] = true;
						roomX = 0;

					}
					else {

						roomExists[1, roomY] = true;

					}

					int direction = genRand.Next(2);

					if (direction == 0)
						direction = -1;
					
					int num9 = genRand.Next(10);

					while (num9 > 0 && roomY >= 0 && roomY < MaxRoomsDown) {

						roomExists[roomX, roomY] = true;
						roomY += direction;

					}

				}

				if (genRand.Next(3) == 0 || DrunkWorldGen) {

					flag2 = true;
					
					int roomY = genRand.Next(MaxRoomsDown);
					
					if (roomY < farLeft)
						farLeft = roomY;
					
					if (roomY > farRight)
						farRight = roomY;
					
					int roomX = 3;

					if (genRand.Next(2) == 0 || DrunkWorldGen) {

						roomExists[3, roomY] = true;
						roomExists[4, roomY] = true;
						roomX = 4;

					}
					else {

						roomExists[3, roomY] = true;

					}

					int direction = genRand.Next(2);

					if (direction == 0)
						direction = -1;
					
					int num13 = genRand.Next(10);

					while (num13 > 0 && roomY >= 0 && roomY < MaxRoomsDown) {

						roomExists[roomX, roomY] = true;
						roomY += direction;

					}

				}

			}

			for (int roomX = 0; roomX < MaxRoomsAcross; roomX++) {

				int x = leftEdges[roomX];
				bool roomValid = true;
				
				if (x < 10 || x > Main.maxTilesX - 10) {

					roomValid = false;

				}
				//else {
				//
				//	for (int y = Neo.UnderworldLayer; y < Main.maxTilesY; y++)
				//		if (Main.tile[x, y].wall > 0)
				//			roomValid = false;
				//		
				//}

				if (!roomValid)
					for (int roomY = 0; roomY < MaxRoomsDown; roomY++)
						roomExists[roomX, roomY] = false;
					
			}

			int num16 = genRand.Next(MaxRoomsDown);

			if (num16 < farLeft)
				farLeft = num16;
			
			num16 = genRand.Next(MaxRoomsDown);

			if (num16 > farRight)
				farRight = num16;
			
			if (!flag && !flag2) {

				while (farRight - farLeft < 5) {

					num16 = genRand.Next(MaxRoomsDown);

					if (num16 < farLeft)
						farLeft = num16;
					
					num16 = genRand.Next(MaxRoomsDown);

					if (num16 > farRight)
						farRight = num16;
					
				}

			}

			for (int roomY = farLeft; roomY <= farRight; roomY++)
				roomExists[2, roomY] = true;
			
			for (int roomX = 0; roomX < MaxRoomsAcross; roomX++)
				for (int roomY = 0; roomY < MaxRoomsDown; roomY++)
					if (roomExists[roomX, roomY] && (topEdges[roomY] < 20 || bottomEdges[roomY] > Main.maxTilesY - 20))
						roomExists[roomX, roomY] = false;
					
			for (int roomX = 0; roomX < MaxRoomsAcross; roomX++) {

				for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

					if (!roomExists[roomX, roomY])
						continue;
					
					for (int x = leftEdges[roomX]; x <= rightEdges[roomX]; x++) {

						for (int y = topEdges[roomY]; y <= bottomEdges[roomY]; y++) {

							if (x < 10)
								break;
							
							if (x > Main.maxTilesX - 10)
								break;

							Neo.SetLiquid(x, y, 0, ref undo);

							if (x == leftEdges[roomX] || x == rightEdges[roomX] || y == topEdges[roomY] || y == bottomEdges[roomY]) {
								Neo.SetTile(x, y, tileType);
							}
							else {
								Neo.SetWall(x, y, wallType, false);
							}

						}

					}

				}

			}

			for (int roomX = 0; roomX < MaxRoomsAcross - 1; roomX++) {

				bool[] array6 = new bool[MaxRoomsDown];
				bool flag4 = false;
				
				for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

					if (roomExists[roomX, roomY] && roomExists[roomX + 1, roomY]) {

						array6[roomY] = true;
						flag4 = true;

					}

				}

				while (flag4) {

					int roomY = genRand.Next(MaxRoomsDown);
					
					if (array6[roomY]) {

						flag4 = false;

						Neo.SetWall(rightEdges[roomX], bottomEdges[roomY] - 1, wallType, ref undo, false);
						Neo.SetWall(rightEdges[roomX], bottomEdges[roomY] - 2, wallType, ref undo, false);
						Neo.SetWall(rightEdges[roomX], bottomEdges[roomY] - 3, wallType, ref undo, false);
						PlaceTile(rightEdges[roomX], bottomEdges[roomY] - 1, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, doorStyle);

					}

				}

			}

			for (int roomX = 0; roomX < MaxRoomsAcross; roomX++) {

				for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

					if (!roomExists[roomX, roomY])
						continue;
					
					if (roomY > 0 && roomExists[roomX, roomY - 1]) {

						int startX = genRand.Next(leftEdges[roomX] + 2, rightEdges[roomX] - 1);
						int endX   = genRand.Next(leftEdges[roomX] + 2, rightEdges[roomX] - 1);

						int attempts = 0;
						
						while (endX - startX < 2 || endX - startX > 5) {

							startX = genRand.Next(leftEdges[roomX] + 2, rightEdges[roomX] - 1);
							endX   = genRand.Next(leftEdges[roomX] + 2, rightEdges[roomX] - 1);

							attempts++;
							
							if (attempts > 10000)
								break;
							
						}

						if (attempts > 10000)
							break;
						
						for (int x = startX; x <= endX && x >= 20 && x <= Main.maxTilesX - 20; x++) {

							Neo.SetWall(x, topEdges[roomY], wallType, ref undo, false);
							PlaceTile(x, topEdges[roomY], TileID.Platforms, ref undo, mute: true, forced: true, -1, platformStyle);

						}

					}

					if (roomX < 4 && roomExists[roomX + 1, roomY] && genRand.Next(3) == 0) {

						Neo.SetWall(rightEdges[roomX], bottomEdges[roomY] - 1, wallType, ref undo, false);
						Neo.SetWall(rightEdges[roomX], bottomEdges[roomY] - 2, wallType, ref undo, false);
						Neo.SetWall(rightEdges[roomX], bottomEdges[roomY] - 3, wallType, ref undo, false);
						PlaceTile(rightEdges[roomX], bottomEdges[roomY] - 1, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, doorStyle);

					}

				}

			}

			bool flag5 = false;

			for (int roomX = 0; roomX < MaxRoomsAcross; roomX++) {

				bool[] array7 = new bool[MaxRoomsDown];
				
				for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

					if (roomExists[roomX, roomY]) {

						flag5 = true;
						array7[roomY] = true;

					}

				}

				if (!flag5)
					continue;
				
				bool flag6 = false;

				for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

					if (array7[roomY]) {

						if (!Main.tile[leftEdges[roomX] - 1, bottomEdges[roomY] - 1].active() && !Main.tile[leftEdges[roomX] - 1, bottomEdges[roomY] - 2].active() && !Main.tile[leftEdges[roomX] - 1, bottomEdges[roomY] - 3].active() && Main.tile[leftEdges[roomX] - 1, bottomEdges[roomY] - 1].liquid == 0 && Main.tile[leftEdges[roomX] - 1, bottomEdges[roomY] - 2].liquid == 0 && Main.tile[leftEdges[roomX] - 1, bottomEdges[roomY] - 3].liquid == 0) {
							flag6 = true;
						}
						else {
							array7[roomY] = false;
						}

					}

				}

				while (flag6) {

					int roomY = genRand.Next(MaxRoomsDown);

					if (array7[roomY]) {

						flag6 = false;

						Neo.SetActive(leftEdges[roomX], bottomEdges[roomY] - 1, false, ref undo);
						Neo.SetActive(leftEdges[roomX], bottomEdges[roomY] - 2, false, ref undo);
						Neo.SetActive(leftEdges[roomX], bottomEdges[roomY] - 3, false, ref undo);
						PlaceTile(leftEdges[roomX], bottomEdges[roomY] - 1, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, doorStyle);

					}

				}

				break;

			}

			bool flag7 = false;

			for (int roomX = MaxRoomsAcross - 1; roomX >= 0; roomX--) {

				bool[] array8 = new bool[MaxRoomsDown];
				
				for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

					if (roomExists[roomX, roomY]) {

						flag7 = true;
						array8[roomY] = true;

					}

				}

				if (flag7) {

					bool flag8 = false;

					for (int roomY = 0; roomY < MaxRoomsDown; roomY++) {

						if (array8[roomY]) {

							if (roomX < 20 || roomX > Main.maxTilesX - 20)
								break;
							
							if (!Main.tile[rightEdges[roomX] + 1, bottomEdges[roomY] - 1].active() && !Main.tile[rightEdges[roomX] + 1, bottomEdges[roomY] - 2].active() && !Main.tile[rightEdges[roomX] + 1, bottomEdges[roomY] - 3].active() && Main.tile[rightEdges[roomX] + 1, bottomEdges[roomY] - 1].liquid == 0 && Main.tile[rightEdges[roomX] + 1, bottomEdges[roomY] - 2].liquid == 0 && Main.tile[rightEdges[roomX] + 1, bottomEdges[roomY] - 3].liquid == 0) {
								flag8 = true;
							}
							else {
								array8[roomY] = false;
							}

						}

					}

					while (flag8) {

						int roomY = genRand.Next(MaxRoomsDown);

						if (array8[roomY]) {

							flag8 = false;

							Neo.SetActive(rightEdges[roomX], bottomEdges[roomY] - 1, false, ref undo);
							Neo.SetActive(rightEdges[roomX], bottomEdges[roomY] - 2, false, ref undo);
							Neo.SetActive(rightEdges[roomX], bottomEdges[roomY] - 3, false, ref undo);
							PlaceTile(rightEdges[roomX], bottomEdges[roomY] - 1, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, doorStyle);

						}

					}

					break;

				}

			}

			bool flag9 = false;
			int roomY2 = 0;
			bool[] array9;

			while (true) {

				if (roomY2 >= MaxRoomsDown)
					return;
				
				array9 = new bool[MaxRoomsDown];

				for (int roomX = 0; roomX < MaxRoomsAcross; roomX++) {

					if (roomExists[roomX, roomY2]) {

						flag9 = true;
						array9[roomX] = true;

					}

				}

				if (flag9)
					break;
				
				roomY2++;

			}

			bool flag10 = true;

			while (flag10) {

				int num43 = genRand.Next(MaxRoomsAcross);
				
				if (!array9[num43])
					continue;
				
				int startX = genRand.Next(leftEdges[num43] + 2, rightEdges[num43] - 1);
				int endX   = genRand.Next(leftEdges[num43] + 2, rightEdges[num43] - 1);

				int attempts = 0;

				while (endX - startX < 2 || endX - startX > 5) {

					startX = genRand.Next(leftEdges[num43] + 2, rightEdges[num43] - 1);
					endX   = genRand.Next(leftEdges[num43] + 2, rightEdges[num43] - 1);
					
					attempts++;
					
					if (attempts > 10000)
						break;
					
				}

				if (attempts > 10000)
					break;
				
				for (int x = startX; x <= endX && x >= 10 && x <= Main.maxTilesX - 10; x++)
					if (Main.tile[x, topEdges[roomY2] - 1].active() || Main.tile[x, topEdges[roomY2] - 1].liquid > 0)
						flag10 = false;
					
				if (flag10) {

					for (int x = startX; x <= endX && x >= 10 && x <= Main.maxTilesX - 10; x++) {

						Neo.SetActive(x, topEdges[roomY2], false, ref undo);
						PlaceTile(x, topEdges[roomY2], TileID.Platforms, ref undo, mute: true, forced: true, -1, platformStyle);

					}

				}

				flag10 = false;

			}

			AddFurniture(leftEdges[0], rightEdges[MaxRoomsAcross - 1], topEdges[0], bottomEdges[MaxRoomsDown - 1], ref undo);

			undo.ResetFrames();

		}

		private static void AddFurniture(int minX, int maxX, int minY, int maxY, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			float modifier = 0.1f;

			float scale = Main.maxTilesX / 4200;
			scale *= modifier;

			for (int j = 0; j < 200f * scale; j++) {

				int iterations = 0;
				bool torchPlaced = false;

				while (!torchPlaced) {

					iterations++;
					
					int x = genRand.Next(minX - 1, maxX + 1);
					int y = genRand.Next(minY - 1, maxY + 1);

					if (Main.tile[x, y].active() && (Main.tile[x, y].type == 75 || Main.tile[x, y].type == 76)) {

						int num8 = 0;

						if (Main.tile[x - 1, y].wall > 0) {
							num8 = -1;
						}
						else if (Main.tile[x + 1, y].wall > 0) {
							num8 = 1;
						}

						if (!Main.tile[x + num8, y].active() && !Main.tile[x + num8, y + 1].active()) {

							bool torchFound = false;

							for (int k = x - 8; k < x + 8; k++) {

								for (int l = y - 8; l < y + 8; l++) {

									if (Main.tile[k, l].active() && Main.tile[k, l].type == 4) {

										torchFound = true;
										break;

									}

								}

							}

							if (!torchFound) {

								PlaceTile(x + num8, y, 4, ref undo, mute: true, forced: true, -1, 7);
								torchPlaced = true;

							}

						}

					}

					if (iterations > 1000)
						torchPlaced = true;
					
				}

			}

			scale = 4200000f / Main.maxTilesX;
			scale *= modifier;

			for (int m = 0; m < scale; m++) {

				int attempts = 0;

				int x = genRand.Next(minX - 1, maxX + 1);
				int y = genRand.Next(minY - 1, maxY + 1);
				
				while ((Main.tile[x, y].wall != 13 && Main.tile[x, y].wall != 14) || Main.tile[x, y].active()) {

					x = genRand.Next(minX - 1, maxX + 1);
					y = genRand.Next(minY - 1, maxY + 1);

					attempts++;

					if (attempts > 100000)
						break;
					
				}

				if (attempts > 100000 || (Main.tile[x, y].wall != 13 && Main.tile[x, y].wall != 14) || Main.tile[x, y].active())
					continue;
				
				for (; !WorldGen.SolidTile(x, y) && y < Main.maxTilesY - 20; y++) { }

				y--;

				int leftEdge  = x;
				int rightEdge = x;

				while (!Main.tile[leftEdge, y].active() && WorldGen.SolidTile(leftEdge, y + 1))
					leftEdge--;
				
				leftEdge++;

				for (; !Main.tile[rightEdge, y].active() && WorldGen.SolidTile(rightEdge, y + 1); rightEdge++) { }

				rightEdge--;

				int width  = rightEdge - leftEdge;
				int middle = (rightEdge + leftEdge) / 2;

				if (Main.tile[middle, y].active() || (Main.tile[middle, y].wall != 13 && Main.tile[middle, y].wall != 14) || !WorldGen.SolidTile(middle, y + 1))
					continue;
				
				int chairStyle = 16;
				int tableStyle = 13;
				int workbenchStyle = 14;
				int statueStyle = 49;
				int bookcaseStyle = 4;
				int bedStyle = 8;
				int pianoStyle = 15;
				int dresserStyle = 9;
				int benchStyle = 10;
				int clockStyle = 17;
				int candelabraStyle = 25;
				int candleStyle = 25;
				int lampStyle = 23;
				int style14 = 25;

				int xOffset = 0;
				int yOffset = 0;

				int furnitureItem = genRand.Next(13);

				if (furnitureItem == 0) {
					xOffset = 5;
					yOffset = 4;
				}

				if (furnitureItem == 1) {
					xOffset = 4;
					yOffset = 3;
				}

				if (furnitureItem == 2) {
					xOffset = 3;
					yOffset = 5;
				}

				if (furnitureItem == 3) {
					xOffset = 4;
					yOffset = 6;
				}

				if (furnitureItem == 4) {
					xOffset = 3;
					yOffset = 3;
				}

				if (furnitureItem == 5) {
					xOffset = 5;
					yOffset = 3;
				}

				if (furnitureItem == 6) {
					xOffset = 5;
					yOffset = 4;
				}

				if (furnitureItem == 7) {
					xOffset = 5;
					yOffset = 4;
				}

				if (furnitureItem == 8) {
					xOffset = 5;
					yOffset = 4;
				}

				if (furnitureItem == 9) {
					xOffset = 3;
					yOffset = 5;
				}

				if (furnitureItem == 10) {
					xOffset = 5;
					yOffset = 3;
				}

				if (furnitureItem == 11) {
					xOffset = 2;
					yOffset = 4;
				}

				if (furnitureItem == 12) {
					xOffset = 3;
					yOffset = 3;
				}

				for (int x2 = middle - xOffset; x2 <= middle + xOffset; x2++) {

					for (int y2 = y - yOffset; y2 <= y; y2++) {

						if (Main.tile[x2, y2].active()) {

							furnitureItem = -1;
							break;

						}

					}

				}

				if (width < xOffset * 1.75)
					furnitureItem = -1;
				
				switch (furnitureItem) {

					case 0: {

							PlaceTile(middle, y, TileID.Tables, ref undo, mute: true, forced: false, -1, tableStyle);

							int num22 = genRand.Next(6);

							if (num22 < 3)
								PlaceTile(middle + num22, y - 2, TileID.Candles, ref undo, mute: true, forced: false, -1, candleStyle);
							
							if (!Main.tile[middle, y].active())
								break;
							
							if (!Main.tile[middle - 2, y].active()) {

								PlaceTile(middle - 2, y, TileID.Chairs, ref undo, mute: true, forced: false, -1, chairStyle);

								if (Main.tile[middle - 2, y].active()) {

									Main.tile[middle - 2, y].frameX += 18;
									Main.tile[middle - 2, y - 1].frameX += 18;

								}

							}

							if (!Main.tile[middle + 2, y].active())
								PlaceTile(middle + 2, y, TileID.Chairs, ref undo, mute: true, forced: false, -1, chairStyle);
							
							break;

						} // Table and Chairs
					case 1: {

							PlaceTile(middle, y, TileID.WorkBenches, ref undo, mute: true, forced: false, -1, workbenchStyle);

							int num21 = genRand.Next(4);

							if (num21 < 2)
								PlaceTile(middle + num21, y - 1, TileID.Candles, ref undo, mute: true, forced: false, -1, candleStyle);
							
							if (!Main.tile[middle, y].active())
								break;
							
							if (genRand.Next(2) == 0) {

								if (!Main.tile[middle - 1, y].active()) {

									PlaceTile(middle - 1, y, TileID.Chairs, ref undo, mute: true, forced: false, -1, chairStyle);

									if (Main.tile[middle - 1, y].active()) {

										Main.tile[middle - 1, y].frameX += 18;
										Main.tile[middle - 1, y - 1].frameX += 18;

									}

								}

							}
							else if (!Main.tile[middle + 2, y].active()) {
								PlaceTile(middle + 2, y, TileID.Chairs, ref undo, mute: true, forced: false, -1, chairStyle);
							}

							break;

						} // Workbench and Chairs
					case 2: {

							PlaceTile(middle, y, TileID.Statues, ref undo, mute: true, forced: false, -1, statueStyle);
							break;

						} // Statue
					case 3: {

							PlaceTile(middle, y, TileID.Bookcases, ref undo, mute: true, forced: false, -1, bookcaseStyle);
							break;

						} // Bookcase
					case 4: {

							if (genRand.Next(2) == 0) {

								PlaceTile(middle, y, TileID.Chairs, ref undo, mute: true, forced: false, -1, chairStyle);
								Main.tile[middle, y].frameX += 18;
								Main.tile[middle, y - 1].frameX += 18;

							}
							else {
								PlaceTile(middle, y, TileID.Chairs, ref undo, mute: true, forced: false, -1, chairStyle);
							}

							break;

						} // Chair
					case 5: {

							Place4x2(middle, y, TileID.Beds, ref undo, genRand.Next(2) == 0 ? 1 : -1, bedStyle);
							break;

						} // Bed
					case 6: {

							PlaceTile(middle, y, TileID.Pianos, ref undo, mute: true, forced: false, -1, pianoStyle);
							break;

						} // Piano
					case 7: {

							PlaceTile(middle, y, TileID.Dressers, ref undo, mute: true, forced: false, -1, dresserStyle);
							break;

						} // Dresser
					case 8: {

							PlaceTile(middle, y, TileID.Benches, ref undo, mute: true, forced: false, -1, benchStyle);
							break;

						} // Bench
					case 9: {

							PlaceTile(middle, y, TileID.GrandfatherClocks, ref undo, mute: true, forced: false, -1, clockStyle);
							break;

						} // Grandfather Clock
					case 10: {

							Place4x2(middle, y, TileID.Bathtubs, ref undo, genRand.Next(2) == 0 ? 1 : -1, style14);
							break;

						} // Bathtub
					case 11: {

							PlaceTile(middle, y, TileID.Lamps, ref undo, mute: true, forced: false, -1, lampStyle);
							break;

						} // Lamp
					case 12: {

							PlaceTile(middle, y, TileID.Candelabras, ref undo, mute: true, forced: false, -1, candelabraStyle);
							break;

						} // Candelabra

				}

			}

			scale = 420000f / Main.maxTilesX;
			scale *= modifier;

			for (int num23 = 0; num23 < scale; num23++) {

				int attempts = 0;

				int x = genRand.Next(minX - 1, maxX + 1);
				int y = genRand.Next(minY - 1, maxY + 1);
				
				while ((Main.tile[x, y].wall != 13 && Main.tile[x, y].wall != 14) || Main.tile[x, y].active()) {

					x = genRand.Next(minX - 1, maxX + 1);
					y = genRand.Next(minY - 1, maxY + 1);

					attempts++;

					if (attempts > 100000)
						break;
					
				}

				if (attempts > 100000)
					continue;
				
				int leftEdge;
				int rightEdge;
				int topEdge;
				int bottomEdge;
				int height;

				for (int num32 = 0; num32 < 2; num32++) {

					leftEdge  = x;
					rightEdge = x;

					while (!Main.tile[leftEdge, y].active() && (Main.tile[leftEdge, y].wall == 13 || Main.tile[leftEdge, y].wall == 14))
						leftEdge--;
					
					leftEdge++;

					for (; !Main.tile[rightEdge, y].active() && (Main.tile[rightEdge, y].wall == 13 || Main.tile[rightEdge, y].wall == 14); rightEdge++) { }

					rightEdge--;

					x = (leftEdge + rightEdge) / 2;

					topEdge    = y;
					bottomEdge = y;

					while (!Main.tile[x, topEdge].active() && (Main.tile[x, topEdge].wall == 13 || Main.tile[x, topEdge].wall == 14))
						topEdge--;
					
					topEdge++;

					for (; !Main.tile[x, bottomEdge].active() && (Main.tile[x, bottomEdge].wall == 13 || Main.tile[x, bottomEdge].wall == 14); bottomEdge++) { }

					bottomEdge--;

					y = (topEdge + bottomEdge) / 2;

				}

				leftEdge  = x;
				rightEdge = x;
				
				while (!Main.tile[leftEdge, y].active() && !Main.tile[leftEdge, y - 1].active() && !Main.tile[leftEdge, y + 1].active())
					leftEdge--;
				
				leftEdge++;

				for (; !Main.tile[rightEdge, y].active() && !Main.tile[rightEdge, y - 1].active() && !Main.tile[rightEdge, y + 1].active(); rightEdge++) { }

				rightEdge--;
				
				topEdge    = y;
				bottomEdge = y;

				while (!Main.tile[x, topEdge].active() && !Main.tile[x - 1, topEdge].active() && !Main.tile[x + 1, topEdge].active())
					topEdge--;
				
				topEdge++;

				for (; !Main.tile[x, bottomEdge].active() && !Main.tile[x - 1, bottomEdge].active() && !Main.tile[x + 1, bottomEdge].active(); bottomEdge++) { }

				bottomEdge--;
				
				x = (leftEdge + rightEdge) / 2;
				y = (topEdge + bottomEdge) / 2;
				
				int width = rightEdge - leftEdge;
				height = bottomEdge - topEdge;

				if (width <= 7 || height <= 5)
					continue;
				
				bool placePicture = true;

				if (WorldGen.nearPicture2(x, y))
					placePicture = false;
				
				if (placePicture) {

					Vector2 vector = WorldGen.randHellPicture();

					int picType  = (int)vector.X;
					int picStyle = (int)vector.Y;

					if (!WorldGen.nearPicture(x, y))
						PlaceTile(x, y, picType, ref undo, mute: true, forced: false, -1, picStyle);
					
				}

			}

			int[] bannerStyle = new int[3] {

				genRand.Next(16, 22),
				genRand.Next(16, 22),
				genRand.Next(16, 22)

			};

			while (bannerStyle[1] == bannerStyle[0])
				bannerStyle[1] = genRand.Next(16, 22);
			
			while (bannerStyle[2] == bannerStyle[0] || bannerStyle[2] == bannerStyle[1])
				bannerStyle[2] = genRand.Next(16, 22);
			
			scale = 420000f / Main.maxTilesX;
			scale *= modifier;

			for (int num35 = 0; num35 < scale; num35++) {

				int attempts = 0;
				int x;
				int y;

				do {

					x = genRand.Next(minX - 1, maxX + 1);
					y = genRand.Next(minY - 1, maxY + 1);

					attempts++;

				}
				while (attempts <= 100000 && ((Main.tile[x, y].wall != 13 && Main.tile[x, y].wall != 14) || Main.tile[x, y].active()));

				if (attempts > 100000)
					continue;
				
				while (!WorldGen.SolidTile(x, y) && y > 10)
					y--;
				
				y++;

				if (Main.tile[x, y].wall != 13 && Main.tile[x, y].wall != 14)
					continue;
				
				int styleChoice = genRand.Next(3);
				int chandelierStyle = 32;
				int lanternStyle = 32;
				int num40;
				int num41;

				switch (styleChoice) {

					default:
						num40 = 1;
						num41 = 3;
						break;

					case 1:
						num40 = 3;
						num41 = 3;
						break;

					case 2:
						num40 = 1;
						num41 = 2;
						break;

				}

				for (int num42 = x - 1; num42 <= x + num40; num42++) {

					for (int num43 = y; num43 <= y + num41; num43++) {

						Tile tile = Main.tile[x, y];

						if (num42 < x || num42 == x + num40) {

							if (tile.active()) {

								switch (tile.type) {

									case TileID.ClosedDoor:
									case TileID.OpenDoor:
									case TileID.Chandeliers:
									case TileID.HangingLanterns:
									case TileID.Banners:
										styleChoice = -1;
										break;

								}

							}

						}
						else if (tile.active()) {
							styleChoice = -1;
						}

					}

				}

				switch (styleChoice) {

					case 0: {

							PlaceTile(x, y, TileID.Banners, ref undo, mute: true, forced: false, -1, bannerStyle[genRand.Next(3)]);
							break;

						} // Banner
					case 1: {

							PlaceTile(x, y, TileID.Chandeliers, ref undo, mute: true, forced: false, -1, chandelierStyle);
							break;

						} // Chandelier
					case 2: {

							PlaceTile(x, y, TileID.HangingLanterns, ref undo, mute: true, forced: false, -1, lanternStyle);
							break;

						} // Hanging Lantern

				}

			}

		}

	}

}
