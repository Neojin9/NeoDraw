using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using NeoDraw.WldGen.Place;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static void ChasmRunnerSideways(int i, int j, int direction, int steps, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			float num = steps;

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			Vector2 vector2 = default;
			vector2.X = genRand.Next(10, 21) * 0.1f * direction;
			vector2.Y = genRand.Next(-10, 10) * 0.01f;

			double num2 = genRand.Next(5) + 7;
			
			while (num2 > 0.0) {

				if (num > 0f) {

					num2 += genRand.Next(3);
					num2 -= genRand.Next(3);

					if (num2 < 7.0)
						num2 = 7.0;
					
					if (num2 > 20.0)
						num2 = 20.0;
					
					if (num == 1f && num2 < 10.0)
						num2 = 10.0;
					
				}
				else {

					num2 -= genRand.Next(4);

				}

				if (vector.Y > Main.rockLayer && num > 0f)
					num = 0f;
				
				num -= 1f;

				int minX = (int)(vector.X - num2 * 0.5);
				int maxX = (int)(vector.X + num2 * 0.5);
				int minY = (int)(vector.Y - num2 * 0.5);
				int maxY = (int)(vector.Y + num2 * 0.5);
				
				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY, 1);

				for (int k = minX; k < maxX; k++)
					for (int l = minY; l < maxY; l++)
						if (Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < num2 * 0.5 * (1.0 + genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != TileID.ShadowOrbs && Main.tile[k, l].type != TileID.Demonite)
							Neo.SetActive(k, l, false, ref undo);
				
				vector += vector2;
				vector2.Y += genRand.Next(-10, 10) * 0.1f;
				
				if (vector.Y < j - 20)
					vector2.Y += genRand.Next(20) * 0.01f;
				
				if (vector.Y > j + 20)
					vector2.Y -= genRand.Next(20) * 0.01f;
				
				if (vector2.Y < -0.5)
					vector2.Y = -0.5f;
				
				if (vector2.Y > 0.5)
					vector2.Y = 0.5f;
				
				vector2.X += genRand.Next(-10, 11) * 0.01f;

				switch (direction) {

					case -1: {

							if (vector2.X > -0.5)
								vector2.X = -0.5f;

							if (vector2.X < -2f)
								vector2.X = -2f;

							break;
						}
					case 1: {

							if (vector2.X < 0.5)
								vector2.X = 0.5f;

							if (vector2.X > 2f)
								vector2.X = 2f;

							break;
						}

				}

				minX = (int)(vector.X - num2 * 1.1);
				maxX = (int)(vector.X + num2 * 1.1);
				minY = (int)(vector.Y - num2 * 1.1);
				maxY = (int)(vector.Y + num2 * 1.1);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY, 1);

				for (int m = minX; m < maxX; m++) {

					for (int n = minY; n < maxY; n++) {

						if (Math.Abs(m - vector.X) + Math.Abs(n - vector.Y) < num2 * 1.1 * (1.0 + genRand.Next(-10, 11) * 0.015) && Main.tile[m, n].wall != 3) {

							Neo.SetActive(m, n, true, ref undo);
							
							if (Main.tile[m, n].type != TileID.ShadowOrbs && Main.tile[m, n].type != TileID.Demonite)
								Neo.SetTile(m, n, TileID.Ebonstone);

							if (Main.tile[m, n].wall == 2)
								Neo.SetWall(m, n, 0);
							
						}

					}

				}

				for (int num7 = minX; num7 < maxX; num7++) {

					for (int num8 = minY; num8 < maxY; num8++) {

						if (Math.Abs(num7 - vector.X) + Math.Abs(num8 - vector.Y) < num2 * 1.1 * (1.0 + genRand.Next(-10, 11) * 0.015) && Main.tile[num7, num8].wall != 3) {

							Neo.SetActive(num7, num8, true, ref undo);

							if (Main.tile[num7, num8].type != TileID.ShadowOrbs && Main.tile[num7, num8].type != TileID.Demonite)
								Neo.SetTile(num7, num8, TileID.Ebonstone);
							
							TilePlacer.PlaceWall(num7, num8, 3, ref undo, mute: true);

						}

					}

				}

			}

			/*if (genRand.Next(3) == 0) {

				int num9 = (int)vector.X;
				int num10;

				for (num10 = (int)vector.Y; !Main.tile[num9, num10].active(); num10++) { }

				TileRunner(num9, num10, genRand.Next(2, 6), genRand.Next(3, 7), TileID.Demonite, ref undo);

			}*/

		}

	}

}
