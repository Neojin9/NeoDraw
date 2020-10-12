using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/1/2020 Copy/Paste

		public static void DungeonRoom(int i, int j, ushort tileType, ushort wallType, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			double num = genRand.Next(15, 30);

			Vector2 vector = default;
			vector.X = genRand.Next(-10, 11) * 0.1f;
			vector.Y = genRand.Next(-10, 11) * 0.1f;

			Vector2 vector2 = default;
			vector2.X = i;
			vector2.Y = j - (float)num / 2f;

			int num2 = genRand.Next(10, 20);

			double num3 = vector2.X;
			double num4 = vector2.X;
			double num5 = vector2.Y;
			double num6 = vector2.Y;

			while (num2 > 0) {

				num2--;

				int minX = (int)(vector2.X - num * 0.8f - 5.0);
				int maxX = (int)(vector2.X + num * 0.8f + 5.0);
				int minY = (int)(vector2.Y - num * 0.8f - 5.0);
				int maxY = (int)(vector2.Y + num * 0.8f + 5.0);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				for (int k = minX; k < maxX; k++) {

					for (int l = minY; l < maxY; l++) {

						if (k < dMinX)
							dMinX = k;

						if (k > dMaxX)
							dMaxX = k;

						if (l > dMaxY)
							dMaxY = l;

						Neo.SetLiquid(k, l, 0, ref undo);

						if (!Main.wallDungeon[Main.tile[k, l].wall])
							Neo.SetTile(k, l, tileType);

					}

				}

				for (int m = minX + 1; m < maxX - 1; m++)
					for (int n = minY + 1; n < maxY - 1; n++)
						Neo.SetWall(m, n, wallType, ref undo);

				minX = (int)(vector2.X - num * 0.5);
				maxX = (int)(vector2.X + num * 0.5);
				minY = (int)(vector2.Y - num * 0.5);
				maxY = (int)(vector2.Y + num * 0.5);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				if (minX < num3)
					num3 = minX;

				if (maxX > num4)
					num4 = maxX;

				if (minY < num5)
					num5 = minY;

				if (maxY > num6)
					num6 = maxY;

				for (int num11 = minX; num11 < maxX; num11++)
					for (int num12 = minY; num12 < maxY; num12++)
						Neo.SetWall(num11, num12, wallType, ref undo, false);

				vector2 += vector;
				vector.X += genRand.Next(-10, 11) * 0.05f;
				vector.Y += genRand.Next(-10, 11) * 0.05f;

				if (vector.X > 1f)
					vector.X = 1f;

				if (vector.X < -1f)
					vector.X = -1f;

				if (vector.Y > 1f)
					vector.Y = 1f;

				if (vector.Y < -1f)
					vector.Y = -1f;

			}

			dRoomX[numDRooms] = (int)vector2.X;
			dRoomY[numDRooms] = (int)vector2.Y;
			dRoomSize[numDRooms] = (int)num;
			dRoomL[numDRooms] = (int)num3;
			dRoomR[numDRooms] = (int)num4;
			dRoomT[numDRooms] = (int)num5;
			dRoomB[numDRooms] = (int)num6;
			dRoomTreasure[numDRooms] = false;
			numDRooms++;

		}
		
	}

}
