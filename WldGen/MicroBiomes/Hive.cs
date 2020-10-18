using System;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen.MicroBiomes { // Updated v1.4 7/25/2020

	public class Hive : MicroBiome {

		public static bool DrunkWorldGen;

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			int index = 0;

			int[] tunnelStartX = new int[1000];
			int[] tunnelStartY = new int[1000];

			Vector2 vector = origin.ToVector2();

			int tunnelsToCreate = WorldGen.genRand.Next(2, 5);

			if (DrunkWorldGen)
				tunnelsToCreate += WorldGen.genRand.Next(7, 10);
			
			for (int i = 0; i < tunnelsToCreate; i++) {

				Vector2 vector2 = vector;

				int num3 = WorldGen.genRand.Next(2, 5);

				for (int j = 0; j < num3; j++)
					vector2 = CreateHiveTunnel((int)vector.X, (int)vector.Y, WorldGen.genRand);
				
				vector = vector2;

				tunnelStartX[index] = (int)vector.X;
				tunnelStartY[index] = (int)vector.Y;

				index++;

			}

			FrameOutAllHiveContents(origin, 50);

			for (int k = 0; k < index; k++) {

				int x = tunnelStartX[k];
				int y = tunnelStartY[k];

				int num5 = 1;

				if (WorldGen.genRand.Next(2) == 0)
					num5 = -1;
				
				bool spotFound = false;

				while (WorldGen.InWorld(x, y, 10) && BadSpotForHoneyFall(x, y)) {

					x += num5;

					if (Math.Abs(x - tunnelStartX[k]) > 50) {
						spotFound = true;
						break;
					}

				}

				if (!spotFound) {

					x += num5;

					if (!SpotActuallyNotInHive(x, y)) {
						CreateBlockedHoneyCube(x, y);
						CreateDentForHoneyFall(x, y, num5);
					}

				}

			}

			CreateStandForLarva(vector);

			if (DrunkWorldGen) {

				for (int l = 0; l < 1000; l++) {

					Vector2 vector3 = vector;

					vector3.X += WorldGen.genRand.Next(-50, 51);
					vector3.Y += WorldGen.genRand.Next(-50, 51);

					if (WorldGen.InWorld((int)vector3.X, (int)vector3.Y) && Vector2.Distance(vector, vector3) > 10f && !_tiles[(int)vector3.X, (int)vector3.Y].active() && _tiles[(int)vector3.X, (int)vector3.Y].wall == 86) {
						CreateStandForLarva(vector3);
						break;
					}

				}

			}

			return true;

		}

		private static void FrameOutAllHiveContents(Point origin, int squareHalfWidth) {

			int num  = Math.Max(10, origin.X - squareHalfWidth);
			int num2 = Math.Min(Main.maxTilesX - 10, origin.X + squareHalfWidth);
			int num3 = Math.Max(10, origin.Y - squareHalfWidth);
			int num4 = Math.Min(Main.maxTilesY - 10, origin.Y + squareHalfWidth);
			
			for (int i = num; i < num2; i++) {

				for (int j = num3; j < num4; j++) {

					Tile tile = _tiles[i, j];

					if (tile.active() && tile.type == 225)
						WorldGen.SquareTileFrame(i, j);
					
					if (tile.wall == 86)
						WorldGen.SquareWallFrame(i, j);
					
				}

			}

		}

		private static Vector2 CreateHiveTunnel(int i, int j, UnifiedRandom random) {

			double num = random.Next(12, 21);
			float num2 = random.Next(10, 21);

			if (DrunkWorldGen) {

				num  = random.Next(8, 26);
				num2 = random.Next(10, 41);

				float num3 = Main.maxTilesX / 4200;

				num3 = (num3 + 1f) / 2f;
				num *= num3;
				num2 *= num3;

			}

			double num4 = num;

			Vector2 result = default;
			result.X = i;
			result.Y = j;

			Vector2 vector = default;
			vector.X = random.Next(-10, 11) * 0.2f;
			vector.Y = random.Next(-10, 11) * 0.2f;

			while (num > 0.0 && num2 > 0f) {

				if (result.Y > Main.maxTilesY - 250)
					num2 = 0f;
				
				num = num4 * (1f + random.Next(-20, 20) * 0.01f);
				num2 -= 1f;

				int num5 = (int)(result.X - num);
				int num6 = (int)(result.X + num);
				int num7 = (int)(result.Y - num);
				int num8 = (int)(result.Y + num);

				if (num5 < 1)
					num5 = 1;
				
				if (num6 > Main.maxTilesX - 1)
					num6 = Main.maxTilesX - 1;
				
				if (num7 < 1)
					num7 = 1;
				
				if (num8 > Main.maxTilesY - 1)
					num8 = Main.maxTilesY - 1;
				
				for (int k = num5; k < num6; k++) {

					for (int l = num7; l < num8; l++) {

						if (!WorldGen.InWorld(k, l, 50)) {

							num2 = 0f;

						}
						else {

							if (_tiles[k - 10, l].wall == 87)
								num2 = 0f;
							
							if (_tiles[k + 10, l].wall == 87)
								num2 = 0f;
							
							if (_tiles[k, l - 10].wall == 87)
								num2 = 0f;
							
							if (_tiles[k, l + 10].wall == 87)
								num2 = 0f;
							
						}

						if (l < Main.worldSurface && _tiles[k, l - 5].wall == 0)
							num2 = 0f;
						
						float num9   = Math.Abs(k - result.X);
						float num10  = Math.Abs(l - result.Y);
						double num11 = Math.Sqrt(num9 * num9 + num10 * num10);

						if (num11 < num4 * 0.4 * (1.0 + random.Next(-10, 11) * 0.005)) {

							DrawInterface.AddChangedTile(k, l);

							if (random.Next(3) == 0)
								_tiles[k, l].liquid = byte.MaxValue;
							
							if (DrunkWorldGen)
								_tiles[k, l].liquid = byte.MaxValue;
							
							_tiles[k, l].honey(true);
							_tiles[k, l].wall = 86;
							_tiles[k, l].active(false);
							_tiles[k, l].halfBrick(false);
							_tiles[k, l].slope(0);

						}
						else if (num11 < num4 * 0.75 * (1.0 + random.Next(-10, 11) * 0.005)) {

							DrawInterface.AddChangedTile(k, l);

							_tiles[k, l].liquid = 0;

							if (_tiles[k, l].wall != 86) {

								_tiles[k, l].active(true);
								_tiles[k, l].halfBrick(false);
								_tiles[k, l].slope(0);
								_tiles[k, l].type = 225;

							}

						}

						if (num11 < num4 * 0.6 * (1.0 + random.Next(-10, 11) * 0.005)) {

							DrawInterface.AddChangedTile(k, l);

							_tiles[k, l].wall = 86;

							if (DrunkWorldGen && random.Next(2) == 0) {

								_tiles[k, l].liquid = byte.MaxValue;
								_tiles[k, l].honey(true);

							}

						}

					}

				}

				result += vector;
				num2 -= 1f;
				vector.Y += random.Next(-10, 11) * 0.05f;
				vector.X += random.Next(-10, 11) * 0.05f;

			}

			return result;

		}

		private static void CreateDentForHoneyFall(int x, int y, int dir) {

			dir *= -1;
			y++;

			int num = 0;

			while ((num < 4 || WorldGen.SolidTile(x, y)) && x > 10 && x < Main.maxTilesX - 10) {

				num++;
				x += dir;
				
				if (WorldGen.SolidTile(x, y)) {

					DrawInterface.AddChangedTile(x, y);

					WorldGen.PoundTile(x, y);

					if (!_tiles[x, y + 1].active()) {

						DrawInterface.AddChangedTile(x, y + 1);

						_tiles[x, y + 1].active(true);
						_tiles[x, y + 1].type = 225;

					}

				}

			}

		}

		private static void CreateBlockedHoneyCube(int x, int y) {

			for (int i = x - 1; i <= x + 2; i++) {

				for (int j = y - 1; j <= y + 2; j++) {

					DrawInterface.AddChangedTile(i, j);

					if (i >= x && i <= x + 1 && j >= y && j <= y + 1) {

						_tiles[i, j].active(false);
						_tiles[i, j].liquid = byte.MaxValue;
						_tiles[i, j].honey(true);

					}
					else {

						_tiles[i, j].active(true);
						_tiles[i, j].type = 225;

					}

				}

			}

		}

		private static bool SpotActuallyNotInHive(int x, int y) {

			for (int i = x - 1; i <= x + 2; i++) {

				for (int j = y - 1; j <= y + 2; j++) {

					if (i < 10 || i > Main.maxTilesX - 10)
						return true;
					
					if (_tiles[i, j].active() && _tiles[i, j].type != 225)
						return true;
					
				}

			}

			return false;

		}

		private static bool BadSpotForHoneyFall(int x, int y) {

			if (_tiles[x, y].active() && _tiles[x, y + 1].active() && _tiles[x + 1, y].active())
				return !_tiles[x + 1, y + 1].active();
			
			return true;

		}

		public static void CreateStandForLarva(Vector2 position) {

			int startX = (int)position.X;
			int startY = (int)position.Y;

			for (int x = startX - 1; x <= startX + 1 && x > 0 && x < Main.maxTilesX; x++) {

				for (int y = startY - 2; y <= startY + 1 && y > 0 && y < Main.maxTilesY; y++) {

					DrawInterface.AddChangedTile(x, y);

					if (y != startY + 1) {

						_tiles[x, y].active(false);
						continue;

					}

					_tiles[x, y].active(true);
					_tiles[x, y].type = 225;
					_tiles[x, y].slope(0);
					_tiles[x, y].halfBrick(false);

				}

			}

			PlaceLarva(position);

		}

		private static void PlaceLarva(Vector2 position) {

			int x = (int)position.X;
			int y = (int)position.Y;

			for (int curX = x - 1; curX <= x + 1; curX++) {

				for (int curY = y - 2; curY <= y + 1; curY++) {

					DrawInterface.AddChangedTile(curX, curY);

					if (curY != y + 1) {

						_tiles[curX, curY].active(false);

					}
					else {

						_tiles[curX, curY].active(true);
						_tiles[curX, curY].type = 225;
						_tiles[curX, curY].slope(0);
						_tiles[curX, curY].halfBrick(false);

					}

				}

			}

			PlaceTile(x, y, 231, ref DrawInterface.Undo, true);

		}

	}

}
