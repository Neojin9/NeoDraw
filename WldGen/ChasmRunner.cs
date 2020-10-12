using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using NeoDraw.WldGen.MicroBiomes;
using NeoDraw.WldGen.Place;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static void ChasmRunner(int i, int j, int steps, ref UndoStep undo, bool makeOrb = false) {

			UnifiedRandom genRand = WorldGen.genRand;

			bool flag  = false;
			bool flag2 = false;
			bool flag3 = false;

			if (!makeOrb)
				flag2 = true;
			
			float num = steps;

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			Vector2 vector2 = default;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(11) * 0.2f + 0.5f;

            double num3 = genRand.Next(5) + 7;

			while (num3 > 0.0) {

				if (num > 0f) {

					num3 += genRand.Next(3);
					num3 -= genRand.Next(3);

					if (num3 < 7.0)
						num3 = 7.0;
					
					if (num3 > 20.0)
						num3 = 20.0;
					
					if (num == 1f && num3 < 10.0)
						num3 = 10.0;
					
				}
				else if (vector.Y > Main.worldSurface + 45.0) {

					num3 -= genRand.Next(4);

				}

				if (vector.Y > Main.rockLayer && num > 0f)
					num = 0f;
				
				num -= 1f;

				if (!flag && vector.Y > Main.worldSurface + 20.0) {

					flag = true;

					ChasmRunnerSideways((int)vector.X, (int)vector.Y, -1, genRand.Next(20, 40), ref undo);
					ChasmRunnerSideways((int)vector.X, (int)vector.Y, 1, genRand.Next(20, 40), ref undo);

				}
				
				int minX;
				int maxX;
				int minY;
				int maxY;

				if (num > 5) {

					minX = (int)(vector.X - num3 * 0.5);
					maxX = (int)(vector.X + num3 * 0.5);
					minY = (int)(vector.Y - num3 * 0.5);
					maxY = (int)(vector.Y + num3 * 0.5);

					Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

					for (int k = minX; k < maxX; k++)
						for (int l = minY; l < maxY; l++)
							if (Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < num3 * 0.5 * (1.0 + genRand.Next(-10, 11) * 0.015) && Main.tile[k, l].type != TileID.ShadowOrbs && Main.tile[k, l].type != TileID.Demonite)
								Neo.SetActive(k, l, false, ref undo);
					
				}

				if (num <= 2f && vector.Y < Main.worldSurface + 45.0)
					num = 2f;
				
				if (num <= 0f) {

					if (!flag2) {

						flag2 = true;
						CorruptionStart.OrbLocations.Add(vector);
						AddShadowOrb((int)vector.X, (int)vector.Y, ref undo);

					}
					else if (!flag3) {

						flag3 = false;
						bool flag4 = false;
						int num8 = 0;

						while (!flag4) {

							int num9  = genRand.Next((int)vector.X - 25, (int)vector.X + 25);
							int num10 = genRand.Next((int)vector.Y - 50, (int)vector.Y);

							if (num9 < 5) {
								num9 = 5;
							}
							if (num9 > Main.maxTilesX - 5) {
								num9 = Main.maxTilesX - 5;
							}
							if (num10 < 5) {
								num10 = 5;
							}
							if (num10 > Main.maxTilesY - 5) {
								num10 = Main.maxTilesY - 5;
							}

							if (num10 > Main.worldSurface) {

								if (!IsTileNearby(num9, num10, TileID.DemonAltar, 3))
									TilePlacer.Place3x2(num9, num10, TileID.DemonAltar, ref undo);
								
								if (Main.tile[num9, num10].type == TileID.DemonAltar) {
									flag4 = true;
									continue;
								}

								num8++;

								if (num8 >= 10000)
									flag4 = true;
								
							}
							else {

								flag4 = true;

							}

						}

					}

				}
				
				vector += vector2;
				vector2.X += genRand.Next(-10, 11) * 0.01f;

				if (vector2.X > 0.3)
					vector2.X = 0.3f;
				
				if (vector2.X < -0.3)
					vector2.X = -0.3f;
				
				minX = (int)(vector.X - num3 * 1.1);
				maxX = (int)(vector.X + num3 * 1.1);
				minY = (int)(vector.Y - num3 * 1.1);
				maxY = (int)(vector.Y + num3 * 1.1);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY, 1);

				for (int curX = minX; curX < maxX; curX++) {

					for (int curY = minY; curY < maxY; curY++) {

						if (Math.Abs(curX - vector.X) + Math.Abs(curY - vector.Y) < num3 * 1.1 * (1.0 + genRand.Next(-10, 11) * 0.015)) {

							if (steps <= 5)
								Neo.SetActive(curX, curY, true, ref undo);

							if (Main.tile[curX, curY].type != TileID.ShadowOrbs)
								Neo.SetTile(curX, curY, TileID.Ebonstone, ref undo, null);

							if (Main.tile[curX, curY].type != TileID.Ebonstone && curY > j + genRand.Next(3, 20))
								Neo.SetActive(curX, curY, true, ref undo);

						}

					}

				}
				
				for (int curX = minX; curX < maxX; curX++) {

					for (int curY = minY; curY < maxY; curY++) {

						if (Math.Abs(curX - vector.X) + Math.Abs(curY - vector.Y) < num3 * 1.1 * (1.0 + genRand.Next(-10, 11) * 0.015)) {

							if (steps <= 5)
								Neo.SetActive(curX, curY, true, ref undo);

							if (Main.tile[curX, curY].type != TileID.ShadowOrbs)
								Neo.SetTile(curX, curY, TileID.Ebonstone, ref undo, null);
							
							if (curY > j + genRand.Next(3, 20))
								Neo.SetWall(curX, curY, 3, ref undo);
							
						}

					}

				}

			}

		}

	}

}
