using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020 Copy/Paste

		public static void CloudIsland(int i, int j, ref UndoStep undo, int style) {

			bool SwitchToSand = false;
			bool SwitchToSnow = false;

			if (style == CloudIslandStyle.Desert) {
				SwitchToSand = true;
			}
			else if (style == CloudIslandStyle.Snow) {
				SwitchToSnow = true;
			}
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

				int tileLeft  = (int)(vector.X - num * 0.5);
				int tileRight = (int)(vector.X + num * 0.5);
				int tileAbove = (int)(vector.Y - num * 0.5);
				int tileBelow = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref tileLeft, ref tileRight, ref tileAbove, ref tileBelow);

				num2 = num * genRand.Next(80, 120) * 0.01;

				float num12 = vector.Y + 1f;

				for (int xPos = tileLeft; xPos < tileRight; xPos++) {

					if (genRand.Next(2) == 0)
						num12 += genRand.Next(-1, 2);

					if (num12 < vector.Y)
						num12 = vector.Y;

					if (num12 > vector.Y + 2f)
						num12 = vector.Y + 2f;

					for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

						if (!(yPos > num12))
							continue;

						float num13 = Math.Abs(xPos - vector.X);
						float num14 = Math.Abs(yPos - vector.Y) * 3f;

						if (Math.Sqrt(num13 * num13 + num14 * num14) < num2 * 0.4) {

							if (xPos < num4)
								num4 = xPos;

							if (xPos > num5)
								num5 = xPos;

							if (yPos < num6)
								num6 = yPos;

							if (yPos > num7)
								num7 = yPos;

							Neo.SetTile(xPos, yPos, TileID.Cloud, ref undo);
							WorldGen.SquareTileFrame(xPos, yPos);

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
				num17  = genRand.Next(4, 8);

				ushort tileType = TileID.Cloud;

				if (genRand.Next(4) == 0)
					tileType = SwitchToSnow ? TileID.SnowCloud : TileID.RainCloud;

				for (int xPos = num15 - num17; xPos <= num15 + num17; xPos++) {

					for (int yPos = num16 - num17; yPos <= num16 + num17; yPos++) {

						if (yPos > num6) {

							float num19 = Math.Abs(xPos - num15);
							float num20 = Math.Abs(yPos - num16) * 2;

							if (Math.Sqrt(num19 * num19 + num20 * num20) < (num17 + genRand.Next(2))) {

								Neo.SetTile(xPos, yPos, tileType, ref undo);
								WorldGen.SquareTileFrame(xPos, yPos);

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

				int tileLeft  = (int)(vector.X - num * 0.5);
				int tileRight = (int)(vector.X + num * 0.5);
				int tileAbove = num6 - 1;
				int tileBelow = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref tileLeft, ref tileRight, ref tileAbove, ref tileBelow);

				num2 = num * genRand.Next(80, 120) * 0.01;

				float num25 = vector.Y + 1f;

				for (int xPos = tileLeft; xPos < tileRight; xPos++) {

					if (genRand.Next(2) == 0)
						num25 += genRand.Next(-1, 2);

					if (num25 < vector.Y)
						num25 = vector.Y;

					if (num25 > vector.Y + 2f)
						num25 = vector.Y + 2f;

					for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

						if (yPos > num25) {

							float num28 = Math.Abs(xPos - vector.X);
							float num29 = Math.Abs(yPos - vector.Y) * 3f;

							if (Math.Sqrt(num28 * num28 + num29 * num29) < num2 * 0.4 && Main.tile[xPos, yPos].type == TileID.Cloud) {

								Neo.SetTile(xPos, yPos, (SwitchToSnow ? TileID.SnowBlock : SwitchToSand ? TileID.Sand : TileID.Dirt), ref undo);
								WorldGen.SquareTileFrame(xPos, yPos);

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

			int num30 = num4;

			num30 += genRand.Next(5);

			while (num30 < num5) {

				int num31 = num7;

				while ((!Main.tile[num30, num31].active() || Main.tile[num30, num31].type != TileID.Dirt) && num30 < num5) {

					num31--;

					if (num31 < num6) {
						num31 = num7;
						num30 += genRand.Next(1, 4);
					}

				}

				if (num30 >= num5)
					continue;

				num31 += genRand.Next(0, 4);

				int num32 = genRand.Next(2, 5);
				ushort tileType = TileID.Cloud;

				for (int num34 = num30 - num32; num34 <= num30 + num32; num34++) {

					for (int num35 = num31 - num32; num35 <= num31 + num32; num35++) {

						if (num35 > num6) {

							float num36 = Math.Abs(num34 - num30);
							float num37 = Math.Abs(num35 - num31) * 2;

							if (Math.Sqrt(num36 * num36 + num37 * num37) < num32) {

								Neo.SetTile(num34, num35, tileType, ref undo);
								WorldGen.SquareTileFrame(num34, num35);

							}

						}

					}

				}

				num30 += genRand.Next(num32, (int)(num32 * 1.5));

			}

			for (int num38 = num4 - 20; num38 <= num5 + 20; num38++) {

				for (int num39 = num6 - 20; num39 <= num7 + 20; num39++) {

					bool flag = true;

					for (int num40 = num38 - 1; num40 <= num38 + 1; num40++)
						for (int num41 = num39 - 1; num41 <= num39 + 1; num41++)
							if (!Main.tile[num40, num41].active())
								flag = false;

					if (flag) {

						Neo.SetWall(num38, num39, WallID.Cloud, ref undo);
						WorldGen.SquareWallFrame(num38, num39);

					}

				}

			}

			for (int num42 = num4; num42 <= num5; num42++) {

				int num43;

				for (num43 = num6 - 10; !Main.tile[num42, num43 + 1].active(); num43++) { }

				if (num43 >= num7 || Main.tile[num42, num43 + 1].type != TileID.Cloud)
					continue;

				if (genRand.Next(10) == 0) {

					int num44 = genRand.Next(1, 3);

					for (int num45 = num42 - num44; num45 <= num42 + num44; num45++) {

						if (Main.tile[num45, num43].type == TileID.Cloud) {

							Neo.SetActive(num45, num43, false, ref undo);
							Neo.SetLiquid(num45, num43, byte.MaxValue, false);

							WorldGen.SquareTileFrame(num42, num43);

						}

						if (Main.tile[num45, num43 + 1].type == TileID.Cloud && WillWaterPlacedHereStayPut(num45, num43 + 1)) {

							Neo.SetActive(num45, num43 + 1, false, ref undo);
							Neo.SetLiquid(num45, num43 + 1, byte.MaxValue, false);

							WorldGen.SquareTileFrame(num42, num43 + 1);

						}

						if (num45 > num42 - num44 && num45 < num42 + 2 && Main.tile[num45, num43 + 2].type == TileID.Cloud && WillWaterPlacedHereStayPut(num45, num43 + 2)) {

							Neo.SetActive(num45, num43 + 2, false, ref undo);
							Neo.SetLiquid(num45, num43 + 2, byte.MaxValue, false);

							WorldGen.SquareTileFrame(num42, num43 + 2);

						}

					}

				}

				if (genRand.Next(5) == 0 && WillWaterPlacedHereStayPut(num42, num43))
					Neo.SetLiquid(num42, num43, byte.MaxValue, ref undo);

				undo.Add(new ChangedTile(num42, num43));
				Main.tile[num42, num43].lava(lava: false);
				WorldGen.SquareTileFrame(num42, num43);

			}

			int num46 = genRand.Next(4);

			for (int num47 = 0; num47 <= num46; num47++) {

				int num48 = genRand.Next(num4 - 5, num5 + 5);
				int num49 = num6 - genRand.Next(20, 40);
				int num50 = genRand.Next(4, 8);
				ushort tileType = TileID.Cloud;

				if (genRand.Next(2) == 0)
					tileType = SwitchToSnow ? TileID.SnowCloud : TileID.RainCloud;

				for (int num52 = num48 - num50; num52 <= num48 + num50; num52++) {

					for (int num53 = num49 - num50; num53 <= num49 + num50; num53++) {

						float num54 = Math.Abs(num52 - num48);
						float num55 = Math.Abs(num53 - num49) * 2;

						if (Math.Sqrt(num54 * num54 + num55 * num55) < (num50 + genRand.Next(-1, 2))) {

							Neo.SetTile(num52, num53, tileType, ref undo);

							WorldGen.SquareTileFrame(num52, num53);

						}

					}

				}

				for (int num56 = num48 - num50 + 2; num56 <= num48 + num50 - 2; num56++) {

					int num57;

					for (num57 = num49 - num50; !Main.tile[num56, num57].active(); num57++) { }

					if (WillWaterPlacedHereStayPut(num56, num57)) {

						Neo.SetActive(num56, num57, false, ref undo);
						Neo.SetLiquid(num56, num57, byte.MaxValue);

						WorldGen.SquareTileFrame(num56, num57);

					}

				}

			}

			undo.ResetFrames(true);

		}

		public struct CloudIslandStyle {
			public static byte Default = 0;
			public static byte Desert = 1;
			public static byte Snow = 2;
        }

	}

}
