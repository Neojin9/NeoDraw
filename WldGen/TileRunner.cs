using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020 Copy/Paste

		public static void TileRunner(int i, int j, double strength, int steps, int type, ref UndoStep undo, bool resetFrames = false, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true, int ignoreTileType = -1) {

			UnifiedRandom genRand = WorldGen.genRand;

			if (DrunkWorldGen) {
				strength *= 1f + genRand.Next(-80, 81) * 0.01f;
				steps = (int)(steps * (1f + genRand.Next(-80, 81) * 0.01f));
			}

			if (GetGoodWorldGen && type != 57) {
				strength *= 1f + genRand.Next(-80, 81) * 0.015f;
				steps += genRand.Next(3);
			}

			double num = strength;
			float num2 = steps;

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			Vector2 vector2 = default;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-10, 11) * 0.1f;

			if (speedX != 0f || speedY != 0f) {
				vector2.X = speedX;
				vector2.Y = speedY;
			}

			bool flag = type == 368;
			bool flag2 = type == 367;
			bool lava = false;

			if (GetGoodWorldGen && genRand.Next(4) == 0)
				lava = true;
			
			while (num > 0.0 && num2 > 0f) {

				if (DrunkWorldGen && genRand.Next(30) == 0) {

					vector.X += genRand.Next(-100, 101) * 0.05f;
					vector.Y += genRand.Next(-100, 101) * 0.05f;

				}

				if (vector.Y < 0f && num2 > 0f && type == 59)
					num2 = 0f;
				
				num = strength * (num2 / steps);
				num2 -= 1f;

				int num3 = (int)(vector.X - num * 0.5);
				int num4 = (int)(vector.X + num * 0.5);
				int num5 = (int)(vector.Y - num * 0.5);
				int num6 = (int)(vector.Y + num * 0.5);

				if (num3 < 1)
					num3 = 1;
				
				if (num4 > Main.maxTilesX - 1)
					num4 = Main.maxTilesX - 1;
				
				if (num5 < 1)
					num5 = 1;
				
				if (num6 > Main.maxTilesY - 1)
					num6 = Main.maxTilesY - 1;
				
				for (int k = num3; k < num4; k++) {

					if (k < BeachDistance + 50 || k >= Main.maxTilesX - BeachDistance - 50)
						lava = false;
					
					for (int l = num5; l < num6; l++) {

						if ((DrunkWorldGen && l < Main.maxTilesY - 300 && type == 57) ||
							(ignoreTileType >= 0 && Main.tile[k, l].active() && Main.tile[k, l].type == ignoreTileType) ||
							!(Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < strength * 0.5 * (1.0 + genRand.Next(-10, 11) * 0.015))) {
							continue;
						}

						if (MudWall && l > Main.worldSurface && Main.tile[k, l - 1].wall != 2 && l < Main.maxTilesY - 210 - genRand.Next(3) && Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < strength * 0.45 * (1.0 + genRand.Next(-10, 11) * 0.01)) {

							if (l > WorldGen.lavaLine - genRand.Next(0, 4) - 50) {

								if (Main.tile[k, l - 1].wall != 64 && Main.tile[k, l + 1].wall != 64 && Main.tile[k - 1, l].wall != 64 && Main.tile[k, l + 1].wall != 64)
									PlaceWall(k, l, 15, ref undo, mute: true);
								
							}
							else if (Main.tile[k, l - 1].wall != 15 && Main.tile[k, l + 1].wall != 15 && Main.tile[k - 1, l].wall != 15 && Main.tile[k, l + 1].wall != 15) {
								PlaceWall(k, l, 64, ref undo, mute: true);
							}

						}

						if (type < 0) {

							if (Main.tile[k, l].type == TileID.Sand)
								continue;

							undo.Add(new ChangedTile(k, l));

							if (type == -2 && Main.tile[k, l].active() && (l < WorldGen.waterLine || l > WorldGen.lavaLine)) {

								Neo.SetLiquid(k, l, byte.MaxValue, lava);

								if (l > WorldGen.lavaLine)
									Main.tile[k, l].lava(lava: true);
								
							}

							Neo.SetActive(k, l, false);
							continue;

						}

						if (flag && Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < strength * 0.3 * (1.0 + genRand.Next(-10, 11) * 0.01))
							PlaceWall(k, l, 180, ref undo, mute: true);
						
						if (flag2 && Math.Abs(k - vector.X) + Math.Abs(l - vector.Y) < strength * 0.3 * (1.0 + genRand.Next(-10, 11) * 0.01))
							PlaceWall(k, l, 178, ref undo, mute: true);
						
						if (overRide || !Main.tile[k, l].active()) {

							Tile tile = Main.tile[k, l];

							bool flag3 = (Main.tileStone[type] && tile.type != TileID.Stone);

							if (!TileID.Sets.CanBeClearedDuringGeneration[tile.type])
								flag3 = true;
							
							switch (tile.type) {

								case 53:

									if (type == TileID.Mud && WorldGen.UndergroundDesertLocation.Contains(k, l))
										flag3 = true;
									
									if (type == TileID.ClayBlock)
										flag3 = true;
									
									if (l < Main.worldSurface && type != TileID.Mud)
										flag3 = true;
									
									break;

								case 45:
								case 147:
								case 189:
								case 190:
								case 196:
								case 460:

									flag3 = true;

									break;

								case 396:
								case 397:

									flag3 = !TileID.Sets.Ore[type];

									break;

								case 1:

									if (type == TileID.Mud && l < Main.worldSurface + genRand.Next(-50, 50))
										flag3 = true;
									
									break;

								case 367:
								case 368:

									if (type == TileID.Mud)
										flag3 = true;
									
									break;

							}

							if (!flag3)
								Neo.SetTile(k, l, (ushort)type, ref undo);

						}

						if (addTile) {

							Neo.SetLiquid(k, l, 0, ref undo, false);
							Neo.SetActive(k, l, true);

						}

						if (noYChange && l < Main.worldSurface && type != TileID.Mud)
							Neo.SetWall(k, l, 2, ref undo);

						if (type == TileID.Mud && l > WorldGen.waterLine && Main.tile[k, l].liquid > 0)
							Neo.SetLiquid(k, l, 0, ref undo, false);

					}

				}

				vector += vector2;

				if ((!DrunkWorldGen || genRand.Next(3) != 0) && num > 50.0) {

					vector += vector2;
					num2 -= 1f;
					vector2.Y += genRand.Next(-10, 11) * 0.05f;
					vector2.X += genRand.Next(-10, 11) * 0.05f;

					if (num > 100.0) {

						vector += vector2;
						num2 -= 1f;
						vector2.Y += genRand.Next(-10, 11) * 0.05f;
						vector2.X += genRand.Next(-10, 11) * 0.05f;

						if (num > 150.0) {

							vector += vector2;
							num2 -= 1f;
							vector2.Y += genRand.Next(-10, 11) * 0.05f;
							vector2.X += genRand.Next(-10, 11) * 0.05f;

							if (num > 200.0) {

								vector += vector2;
								num2 -= 1f;
								vector2.Y += genRand.Next(-10, 11) * 0.05f;
								vector2.X += genRand.Next(-10, 11) * 0.05f;

								if (num > 250.0) {

									vector += vector2;
									num2 -= 1f;
									vector2.Y += genRand.Next(-10, 11) * 0.05f;
									vector2.X += genRand.Next(-10, 11) * 0.05f;

									if (num > 300.0) {

										vector += vector2;
										num2 -= 1f;
										vector2.Y += genRand.Next(-10, 11) * 0.05f;
										vector2.X += genRand.Next(-10, 11) * 0.05f;

										if (num > 400.0) {

											vector += vector2;
											num2 -= 1f;
											vector2.Y += genRand.Next(-10, 11) * 0.05f;
											vector2.X += genRand.Next(-10, 11) * 0.05f;

											if (num > 500.0) {

												vector += vector2;
												num2 -= 1f;
												vector2.Y += genRand.Next(-10, 11) * 0.05f;
												vector2.X += genRand.Next(-10, 11) * 0.05f;

												if (num > 600.0) {

													vector += vector2;
													num2 -= 1f;
													vector2.Y += genRand.Next(-10, 11) * 0.05f;
													vector2.X += genRand.Next(-10, 11) * 0.05f;

													if (num > 700.0) {

														vector += vector2;
														num2 -= 1f;
														vector2.Y += genRand.Next(-10, 11) * 0.05f;
														vector2.X += genRand.Next(-10, 11) * 0.05f;

														if (num > 800.0) {

															vector += vector2;
															num2 -= 1f;
															vector2.Y += genRand.Next(-10, 11) * 0.05f;
															vector2.X += genRand.Next(-10, 11) * 0.05f;

															if (num > 900.0) {

																vector += vector2;
																num2 -= 1f;
																vector2.Y += genRand.Next(-10, 11) * 0.05f;
																vector2.X += genRand.Next(-10, 11) * 0.05f;

															}

														}

													}

												}

											}

										}

									}

								}

							}

						}

					}

				}

				vector2.X += genRand.Next(-10, 11) * 0.05f;

				if (DrunkWorldGen)
					vector2.X += genRand.Next(-10, 11) * 0.25f;
				
				if (vector2.X > 1f)
					vector2.X = 1f;
				
				if (vector2.X < -1f)
					vector2.X = -1f;
				
				if (!noYChange) {

					vector2.Y += genRand.Next(-10, 11) * 0.05f;

					if (vector2.Y > 1f)
						vector2.Y = 1f;
					
					if (vector2.Y < -1f)
						vector2.Y = -1f;
					
				}
				else if (type != TileID.Mud && num < 3.0) {

					if (vector2.Y > 1f)
						vector2.Y = 1f;
					
					if (vector2.Y < -1f)
						vector2.Y = -1f;
					
				}

				if (type == TileID.Mud && !noYChange) {

					if (vector2.Y > 0.5)
						vector2.Y = 0.5f;
					
					if (vector2.Y < -0.5)
						vector2.Y = -0.5f;
					
					if (vector.Y < Main.rockLayer + 100.0)
						vector2.Y = 1f;
					
					if (vector.Y > Main.maxTilesY - 300)
						vector2.Y = -1f;
					
				}

			}

			if (resetFrames)
				undo.ResetFrames(true);

		}

	}

}
