using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;


namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static Vector2 DigTunnel(float X, float Y, float xDir, float yDir, int Steps, int Size, ref UndoStep undo, bool Wet = false, bool resetFrames = true) {

			float num = X;
			float num2 = Y;

			try {

				float num3 = 0f;
				float num4 = 0f;
				float num5 = Size;

				num = MathHelper.Clamp(num, num5 + 1f, Main.maxTilesX - num5 - 1f);
				num2 = MathHelper.Clamp(num2, num5 + 1f, Main.maxTilesY - num5 - 1f);

				for (int i = 0; i < Steps; i++) {

					for (int j = (int)(num - num5); j <= num + num5; j++) {

						for (int k = (int)(num2 - num5); k <= num2 + num5; k++) {

							if ((Math.Abs(j - num) + Math.Abs(k - num2)) < num5 * (1.0 + WorldGen.genRand.Next(-10, 11) * 0.005) && j >= 0 && j < Main.maxTilesX && k >= 0 && k < Main.maxTilesY) {

								undo.Add(new ChangedTile(j, k));
								Main.tile[j, k].active(active: false);

								if (Wet)
									Main.tile[j, k].liquid = byte.MaxValue;

							}

						}

					}

					num5 += WorldGen.genRand.Next(-50, 51) * 0.03f;

					if (num5 < Size * 0.6)
						num5 = Size * 0.6f;

					if (num5 > (Size * 2))
						num5 = Size * 2f;

					num3 += WorldGen.genRand.Next(-20, 21) * 0.01f;
					num4 += WorldGen.genRand.Next(-20, 21) * 0.01f;

					if (num3 < -1f)
						num3 = -1f;

					if (num3 > 1f)
						num3 = 1f;

					if (num4 < -1f)
						num4 = -1f;

					if (num4 > 1f)
						num4 = 1f;

					num += (xDir + num3) * 0.6f;
					num2 += (yDir + num4) * 0.6f;

				}

			} catch { } finally {

				if (resetFrames)
					undo.ResetFrames();

            }

			return new Vector2(num, num2);

		}

		/*public static Vector2 DigTunnel(float X, float Y, float xDir, float yDir, int Steps, int Size, bool Wet = false) {

			float num = X;
			float num2 = Y;

			try {

				float num3 = 0f;
				float num4 = 0f;
				float num5 = Size;

				num = MathHelper.Clamp(num, num5 + 1f, Main.maxTilesX - num5 - 1f);
				num2 = MathHelper.Clamp(num2, num5 + 1f, Main.maxTilesY - num5 - 1f);

				for (int i = 0; i < Steps; i++) {

					for (int j = (int)(num - num5); j <= num + num5; j++) {

						for (int k = (int)(num2 - num5); k <= num2 + num5; k++) {

							if ((Math.Abs(j - num) + Math.Abs(k - num2)) < num5 * (1.0 + WorldGen.genRand.Next(-10, 11) * 0.005) && j >= 0 && j < Main.maxTilesX && k >= 0 && k < Main.maxTilesY) {

								Main.tile[j, k].active(active: false);

								if (Wet)
									Main.tile[j, k].liquid = byte.MaxValue;

							}

						}

					}

					num5 += WorldGen.genRand.Next(-50, 51) * 0.03f;

					if (num5 < Size * 0.6)
						num5 = Size * 0.6f;

					if (num5 > (Size * 2))
						num5 = Size * 2f;

					num3 += WorldGen.genRand.Next(-20, 21) * 0.01f;
					num4 += WorldGen.genRand.Next(-20, 21) * 0.01f;

					if (num3 < -1f)
						num3 = -1f;

					if (num3 > 1f)
						num3 = 1f;

					if (num4 < -1f)
						num4 = -1f;

					if (num4 > 1f)
						num4 = 1f;

					num += (xDir + num3) * 0.6f;
					num2 += (yDir + num4) * 0.6f;

				}

			} catch { } finally { }

			return new Vector2(num, num2);

		}
		*/

	}

}
