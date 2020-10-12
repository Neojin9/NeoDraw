using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020

		public static void ShroomPatch(int i, int j, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			double num = genRand.Next(80, 100);
			float num2 = genRand.Next(20, 26);
			float num3 = Main.maxTilesX / 4200;

			if (GetGoodWorldGen)
				num3 *= 2f;
			
			num  *= num3;
			num2 *= num3;

			float num4 = num2 - 1f;

            Vector2 vector = default;
			vector.X = i;
			vector.Y = j - num2 * 0.3f;

			Vector2 vector2 = default;
			vector2.X = genRand.Next(-100, 101) * 0.005f;
			vector2.Y = genRand.Next(-200, -100) * 0.005f;

			while (num > 0.0 && num2 > 0f) {

				num  -= genRand.Next(3);
				num2 -= 1f;

				int tileLeft  = (int)(vector.X - num * 0.5);
				int tileRight = (int)(vector.X + num * 0.5);
				int tileAbove = (int)(vector.Y - num * 0.5);
				int tileBelow = (int)(vector.Y + num * 0.5);

				if (tileLeft < 0)
					tileLeft = 0;
				
				if (tileRight > Main.maxTilesX)
					tileRight = Main.maxTilesX;
				
				if (tileAbove < 0)
					tileAbove = 0;
				
				if (tileBelow > Main.maxTilesY)
					tileBelow = Main.maxTilesY;
				
                double num5 = num * genRand.Next(80, 120) * 0.01;

                for (int xPos = tileLeft; xPos < tileRight; xPos++) {

					for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

						float  num10 = Math.Abs(xPos - vector.X);
						float  num11 = Math.Abs((yPos - vector.Y) * 2.3f);
						double num12 = Math.Sqrt(num10 * num10 + num11 * num11);

						if (num12 < num5 * 0.8 && Main.tile[xPos, yPos].lava()) {

							undo.Add(new ChangedTile(xPos, yPos));
							Main.tile[xPos, yPos].liquid = 0;
							
						}

						if (num12 < num5 * 0.2 && yPos < vector.Y) {

							undo.Add(new ChangedTile(xPos, yPos));
							Main.tile[xPos, yPos].active(active: false);

							if (Main.tile[xPos, yPos].wall > 0)
								Main.tile[xPos, yPos].wall = WallID.MushroomUnsafe;

						}
						else if (num12 < num5 * 0.4 * (0.95 + genRand.NextFloat() * 0.1)) {

							undo.Add(new ChangedTile(xPos, yPos));
							Main.tile[xPos, yPos].type = TileID.Mud;

							if (num2 == num4 && yPos > vector.Y)
								Main.tile[xPos, yPos].active(active: true);
							
							if (Main.tile[xPos, yPos].wall > 0)
								Main.tile[xPos, yPos].wall = WallID.MushroomUnsafe;

						}

					}

				}

				vector += vector2;
				vector.X += vector2.X;
				vector2.X += genRand.Next(-100, 110) * 0.005f;
				vector2.Y -= genRand.Next(110) * 0.005f;

				if (vector2.X > -0.5 && vector2.X < 0.5) {

					if (vector2.X < 0f) {
						vector2.X = -0.5f;
					}
					else {
						vector2.X = 0.5f;
					}

				}

				if (vector2.X > 0.5)
					vector2.X = 0.5f;
				
				if (vector2.X < -0.5)
					vector2.X = -0.5f;
				
				if (vector2.Y > 0.5)
					vector2.Y = 0.5f;
				
				if (vector2.Y < -0.5)
					vector2.Y = -0.5f;
				
				for (int m = 0; m < 2; m++) {

					int num13 = (int)vector.X + genRand.Next(-20, 20);
					int num14 = (int)vector.Y + genRand.Next(0, 20);

					while (!Main.tile[num13, num14].active() && Main.tile[num13, num14].type != TileID.Mud) {

						num13 = (int)vector.X + genRand.Next(-20, 20);
						num14 = (int)vector.Y + genRand.Next(0, 20);

					}

					int num15 = genRand.Next(10, 20);
					int steps = genRand.Next(10, 20);

					TileRunner(num13, num14, num15, steps, TileID.Mud, ref undo, true, addTile: false, 0f, 2f, noYChange: true);

				}

			}

		}

	}

}
