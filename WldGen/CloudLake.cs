using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 8/11/2020 Copy/Paste

		public static void CloudLake(int i, int j, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			double num = genRand.Next(100, 150);
			float num3 = genRand.Next(20, 30);

			int num4 = i;
			int num5 = i;
			int num6 = i;
			int num7 = j;

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;
			
			Vector2 vector2 = default;
			vector2.X = genRand.Next(-20, 21) * 0.2f;

			while (vector2.X > -2f && vector2.X < 2f)
				vector2.X = genRand.Next(-20, 21) * 0.2f;

			vector2.Y = genRand.Next(-20, -10) * 0.02f;

			double num2;

			while (num > 0.0 && num3 > 0f) {

				num -= genRand.Next(4);
				num3 -= 1f;

				int minX = (int)(vector.X - num * 0.5);
				int maxX = (int)(vector.X + num * 0.5);
				int minY = (int)(vector.Y - num * 0.5);
				int maxY = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

				num2 = num * genRand.Next(80, 120) * 0.01;

				float num12 = vector.Y + 1f;

				for (int k = minX; k < maxX; k++) {

					if (genRand.Next(2) == 0)
						num12 += genRand.Next(-1, 2);

					if (num12 < vector.Y)
						num12 = vector.Y;

					if (num12 > vector.Y + 2f)
						num12 = vector.Y + 2f;

					for (int l = minY; l < maxY; l++) {

						if (!(l > num12))
							continue;

						float num13 = Math.Abs(k - vector.X);
						float num14 = Math.Abs(l - vector.Y) * 3f;

						if (Math.Sqrt(num13 * num13 + num14 * num14) < num2 * 0.4) {

							if (k < num4)
								num4 = k;

							if (k > num5)
								num5 = k;

							if (l < num6)
								num6 = l;

							if (l > num7)
								num7 = l;

							Neo.SetTile(k, l, TileID.Cloud, ref undo);
							WorldGen.SquareTileFrame(k, l);

						}

					}

				}

				vector += vector2;
				vector2.X += genRand.Next(-20, 21) * 0.05f;

				if (vector2.X > 1f)
					vector2.X = 1f;

				if (vector2.X < -1f)
					vector2.X = -1f;

				if (vector2.Y > 0.2)
					vector2.Y = -0.2f;

				if (vector2.Y < -0.2)
					vector2.Y = -0.2f;

			}

			int num15 = num4;
			int num17;

			for (num15 += genRand.Next(5); num15 < num5; num15 += genRand.Next(num17, (int)(num17 * 1.5))) {

				int num16 = num7;

				while (!Main.tile[num15, num16].active())
					num16--;

				num16 += genRand.Next(-3, 4);
				num17 = genRand.Next(4, 8);
				ushort num18 = TileID.Cloud;

				if (genRand.Next(4) == 0)
					num18 = TileID.RainCloud;

				for (int m = num15 - num17; m <= num15 + num17; m++) {

					for (int n = num16 - num17; n <= num16 + num17; n++) {

						if (n > num6) {

							float num19 = Math.Abs(m - num15);
							float num20 = Math.Abs(n - num16) * 2;

							if (Math.Sqrt(num19 * num19 + num20 * num20) < (num17 + genRand.Next(2))) {

								Neo.SetTile(m, n, num18, ref undo);
								WorldGen.SquareTileFrame(m, n);

							}

						}

					}

				}

			}

			num  = genRand.Next(80, 95);
			num3 = genRand.Next(10, 15);

			vector.X = i;
			vector.Y = num6;

			vector2.X = genRand.Next(-20, 21) * 0.2f;

			while (vector2.X > -2f && vector2.X < 2f)
				vector2.X = genRand.Next(-20, 21) * 0.2f;

			vector2.Y = genRand.Next(-20, -10) * 0.02f;

			while (num > 0.0 && num3 > 0f) {

				num -= genRand.Next(4);
				num3 -= 1f;

				int num21 = (int)(vector.X - num * 0.5);
				int num22 = (int)(vector.X + num * 0.5);
				int num23 = num6 - 1;
				int num24 = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref num21, ref num22, ref num23, ref num24);

				num2 = num * genRand.Next(80, 120) * 0.01;
				float num25 = vector.Y + 1f;

				for (int num26 = num21; num26 < num22; num26++) {

					if (genRand.Next(2) == 0)
						num25 += genRand.Next(-1, 2);

					if (num25 < vector.Y)
						num25 = vector.Y;

					if (num25 > vector.Y + 2f)
						num25 = vector.Y + 2f;

					for (int num27 = num23; num27 < num24; num27++) {

						if (num27 > num25) {

							float num28 = Math.Abs(num26 - vector.X);
							float num29 = Math.Abs(num27 - vector.Y) * 3f;

							if (Math.Sqrt(num28 * num28 + num29 * num29) < num2 * 0.4 && Main.tile[num26, num27].type == TileID.Cloud) {

								Neo.SetActive(num26, num27, false, ref undo);

								if (WillWaterPlacedHereStayPut(num26, num27))
									Neo.SetLiquid(num26, num27, byte.MaxValue, false, false);

							}

						}

					}

				}

				vector += vector2;
				vector2.X += genRand.Next(-20, 21) * 0.05f;

				if (vector2.X > 1f)
					vector2.X = 1f;

				if (vector2.X < -1f)
					vector2.X = -1f;

				if (vector2.Y > 0.2)
					vector2.Y = -0.2f;

				if (vector2.Y < -0.2)
					vector2.Y = -0.2f;

			}

			for (int num30 = num4 - 20; num30 <= num5 + 20; num30++) {

				for (int num31 = num6 - 20; num31 <= num7 + 20; num31++) {

					bool flag = true;

					for (int num32 = num30 - 1; num32 <= num30 + 1; num32++)
						for (int num33 = num31 - 1; num33 <= num31 + 1; num33++)
							if (!Main.tile[num32, num33].active())
								flag = false;

					if (flag)
						Neo.SetWall(num30, num31, WallID.Cloud, ref undo);

				}

			}

			for (int num34 = num4; num34 <= num5; num34++) {

				int num35;

				for (num35 = num6 - 10; !Main.tile[num34, num35 + 1].active(); num35++) { }

				if (num35 >= num7 || Main.tile[num34, num35 + 1].type != TileID.Cloud)
					continue;

				if (genRand.Next(10) == 0) {

					int num36 = genRand.Next(1, 3);

					for (int num37 = num34 - num36; num37 <= num34 + num36; num37++) {

						if (Main.tile[num37, num35].type == TileID.Cloud && WillWaterPlacedHereStayPut(num37, num35)) {

							Neo.SetActive(num37, num35, false, ref undo);
							Neo.SetLiquid(num37, num35, byte.MaxValue, false);
							WorldGen.SquareTileFrame(num37, num35);

						}

						if (Main.tile[num37, num35 + 1].type == TileID.Cloud && WillWaterPlacedHereStayPut(num37, num35 + 1)) {

							Neo.SetActive(num37, num35 + 1, false, ref undo);
							Neo.SetLiquid(num37, num35 + 1, byte.MaxValue, false);
							WorldGen.SquareTileFrame(num37, num35 + 1);

						}

						if (num37 > num34 - num36 && num37 < num34 + 2 && Main.tile[num37, num35 + 2].type == TileID.Cloud && WillWaterPlacedHereStayPut(num37, num35 + 2)) {

							Neo.SetActive(num37, num35 + 2, false, ref undo);
							Neo.SetLiquid(num37, num35 + 2, byte.MaxValue, false);
							WorldGen.SquareTileFrame(num37, num35 + 2);

						}

					}

				}

				undo.Add(new ChangedTile(num34, num35));

				if (genRand.Next(5) == 0 && WillWaterPlacedHereStayPut(num34, num35))
					Main.tile[num34, num35].liquid = byte.MaxValue;

				Main.tile[num34, num35].lava(lava: false);
				WorldGen.SquareTileFrame(num34, num35);

			}

			int num38 = genRand.Next(1, 4);

			for (int num39 = 0; num39 <= num38; num39++) {

				int num40 = genRand.Next(num4 - 5, num5 + 5);
				int num41 = num6 - genRand.Next(20, 40);
				int num42 = genRand.Next(4, 8);
				ushort num43 = TileID.Cloud;

				if (genRand.Next(4) != 0)
					num43 = TileID.RainCloud;

				for (int num44 = num40 - num42; num44 <= num40 + num42; num44++) {

					for (int num45 = num41 - num42; num45 <= num41 + num42; num45++) {

						float num46 = Math.Abs(num44 - num40);
						float num47 = Math.Abs(num45 - num41) * 2;

						if (Math.Sqrt(num46 * num46 + num47 * num47) < (num42 + WorldGen.genRand.Next(-1, 2))) {

							Neo.SetTile(num44, num45, num43, ref undo);
							WorldGen.SquareTileFrame(num44, num45);

						}

					}

				}

				for (int num48 = num40 - num42 + 2; num48 <= num40 + num42 - 2; num48++) {

					int num49;

					for (num49 = num41 - num42; !Main.tile[num48, num49].active(); num49++) { }

					if (WillWaterPlacedHereStayPut(num48, num49)) {

						Neo.SetActive(num48, num49, false, ref undo);
						Neo.SetLiquid(num48, num49, byte.MaxValue);
						WorldGen.SquareTileFrame(num48, num49);

					}

				}

			}

			undo.ResetFrames(true);

		}

	}

}
