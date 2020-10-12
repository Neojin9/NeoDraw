using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020 Copy/Paste

		public static void SonOfLakinater(int i, int j, ref UndoStep undo, float strengthMultiplier = 1f, bool lava = false) {

			GrassSpread = 0;

			UnifiedRandom genRand = WorldGen.genRand;

			double num = genRand.Next(15, 31) * strengthMultiplier;
			float num2 = genRand.Next(30, 61);

			if (genRand.Next(5) == 0) {
				num  *= 1.3f;
				num2 *= 1.3f;
			}

			if (DrunkWorldGen) {
				num  *= 1.3f;
				num2 *= 1.3f;
			}

			Vector2 vector = default;

			vector.X = i;
			vector.Y = j;

			float num3 = genRand.NextFloat() * 0.002f;

			Vector2 vector2 = default;

			if (genRand.Next(4) != 0) {
				vector2.X = genRand.Next(-15, 16) * 0.01f;
			}
			else {
				vector2.X = genRand.Next(-50, 51) * 0.01f;
				num3 = genRand.NextFloat() * 0.004f + 0.001f;
			}

			vector2.Y = genRand.Next(101) * 0.01f;
			double num4;
			float num5 = num2;
			
			while (num > 3.0 && num2 > 0f) {

				num -= genRand.Next(11) * 0.1f;
				num2 -= 1f;
				
				int tileLeft  = (int)(vector.X - num * 4.0);
				int tileRight = (int)(vector.X + num * 4.0);
				int tileAbove = (int)(vector.Y - num * 3.0);
				int tileBelow = (int)(vector.Y + num * 2.0);
				
				if (tileLeft < 0)
					tileLeft = 0;
				
				if (tileRight > Main.maxTilesX)
					tileRight = Main.maxTilesX;
				
				if (tileAbove < 0)
					tileAbove = 0;
				
				if (tileBelow > Main.maxTilesY)
					tileBelow = Main.maxTilesY;
				
				num4 = num;

				for (int xPos = tileLeft; xPos < tileRight; xPos++) {

					for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

						float value  = Math.Abs(xPos - vector.X) * 0.6f;
						float value2 = Math.Abs(yPos - vector.Y) * 1.4f;
						float value3 = Math.Abs(xPos - vector.X) * 0.3f;
						float value4 = Math.Abs(yPos - vector.Y) * 5f;

						value  = MathHelper.Lerp(value, value3, num2 / num5);
						value2 = MathHelper.Lerp(value2, value4, num2 / num5);

						double num10 = Math.Sqrt(value * value + value2 * value2);

						int num11 = j + 5;

						if (num10 < num4 * 0.4) {

							undo.Add(new ChangedTile(xPos, yPos));

							if (yPos >= j) {

								if (yPos <= j + 1) {
									
									if (WillWaterPlacedHereStayPut(xPos, yPos)) {
										Main.tile[xPos, yPos].liquid = byte.MaxValue;
										Main.tile[xPos, yPos].lava(lava);
									}

								}
								else {

									Main.tile[xPos, yPos].liquid = byte.MaxValue;
									Main.tile[xPos, yPos].lava(lava);

								}

							}

							Main.tile[xPos, yPos].active(active: false);

							WorldGen.SquareTileFrame(xPos, yPos);

							if (Main.tile[xPos, yPos].type == TileID.Dirt || Main.tile[xPos, yPos].type == TileID.Grass) {

								SpreadGrass(xPos - 1, yPos, ref undo);
								SpreadGrass(xPos + 1, yPos, ref undo);
								SpreadGrass(xPos, yPos + 1, ref undo);

							}
							else if (Main.tile[xPos, yPos].type == TileID.Mud || Main.tile[xPos, yPos].type == TileID.JungleGrass) {

								SpreadGrass(xPos - 1, yPos, ref undo, TileID.Mud, TileID.JungleGrass, repeat: true, 0);
								SpreadGrass(xPos + 1, yPos, ref undo, TileID.Mud, TileID.JungleGrass, repeat: true, 0);
								SpreadGrass(xPos, yPos + 1, ref undo, TileID.Mud, TileID.JungleGrass, repeat: true, 0);

							}

						}
						else if (yPos > j + 1 && num10 < num4 && Main.tile[xPos, yPos].liquid == 0) {

							if (Math.Abs(xPos - vector.X) * 0.8 < num4 && Main.tile[xPos, yPos].wall > 0 && Main.tile[xPos - 1, yPos].wall > 0 && Main.tile[xPos + 1, yPos].wall > 0 && Main.tile[xPos, yPos + 1].wall > 0) {

								undo.Add(new ChangedTile(xPos, yPos));
								Main.tile[xPos, yPos].active(active: true);

							}

						}
						else {

							if (yPos >= j || num2 != num5 - 1f || !(yPos > WorldGen.worldSurfaceLow - 20.0)/* || TileID.Sets.Clouds[Main.tile[k, l].type]*/)
								continue;
							
							value = Math.Abs(xPos - i) * 0.7f;

							float num12 = (float)num4 * 0.4f;
							float num13 = Math.Abs(xPos - i) / (float)(tileRight - i);
							
							num13 = 1f - num13;
							num13 *= 2.3f;
							num13 *= num13;
							num13 *= num13;
							
							if (yPos < num11 && value < num12 + Math.Abs(yPos - num11) * 0.5 * num13) {

								undo.Add(new ChangedTile(xPos, yPos));

								Main.tile[xPos, yPos].active(active: false);

								if (Main.tile[xPos, yPos].type == TileID.Dirt || Main.tile[xPos, yPos].type == TileID.Grass) {

									SpreadGrass(xPos - 1, yPos, ref undo);
									SpreadGrass(xPos + 1, yPos, ref undo);
									SpreadGrass(xPos, yPos + 1, ref undo);

								}
								else if (Main.tile[xPos, yPos].type == TileID.Mud || Main.tile[xPos, yPos].type == TileID.JungleGrass) {

									SpreadGrass(xPos - 1, yPos, ref undo, TileID.Mud, TileID.JungleGrass, repeat: true, 0);
									SpreadGrass(xPos + 1, yPos, ref undo, TileID.Mud, TileID.JungleGrass, repeat: true, 0);
									SpreadGrass(xPos, yPos + 1, ref undo, TileID.Mud, TileID.JungleGrass, repeat: true, 0);

								}

							}

						}

					}

				}

				vector += vector2;
				vector2.X += genRand.Next(-100, 101) * num3;
				vector2.Y += genRand.Next(-100, 101) * 0.01f;

				if (vector2.X > 1f)
					vector2.X = 1f;
				
				if (vector2.X < -1f)
					vector2.X = -1f;
				
				if (vector2.Y > 1f)
					vector2.Y = 1f;
				
				float num14 = 0.5f * (1f - num2 / num5);

				if (vector2.Y < num14)
					vector2.Y = num14;
				
			}

		}

	}

}
