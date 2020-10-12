using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using Terraria;

namespace NeoDraw.WldGen.MicroBiomes {

	public static class SandMound {

		public static void Place(DesertDescription description) {

			Rectangle desert = description.Desert;
			
			desert.Height = Math.Min(description.Desert.Height, description.Hive.Height / 2);
			
			Rectangle desert2 = description.Desert;
			
			desert2.Y = desert.Bottom;
			desert2.Height = Math.Max(0, description.Desert.Bottom - desert.Bottom);
			
			SurfaceMap surface = description.Surface;
			
			int num = 0;
			int num2 = 0;
			
			for (int i = -5; i < desert.Width + 5; i++) {

				float value = Math.Abs((i + 5) / (desert.Width + 10)) * 2f - 1f;
				value = MathHelper.Clamp(value, -1f, 1f);

				if (i % 3 == 0) {

					num += WorldGen.genRand.Next(-1, 2);
					num = Utils.Clamp(num, -10, 10);

				}

				num2 += WorldGen.genRand.Next(-1, 2);
				num2 = Utils.Clamp(num2, -10, 10);

				float num3 = (float)Math.Sqrt(1f - value * value * value * value);
				int num4 = desert.Bottom - (int)(num3 * desert.Height) + num;
				
				if (Math.Abs(value) < 1f) {

					float num5 = WldUtils.WldUtils.UnclampedSmoothStep(0.5f, 0.8f, Math.Abs(value));
					num5 = num5 * num5 * num5;
					
					int val = 10 + (int)(desert.Top - num5 * 20f) + num2;
					val = Math.Min(val, num4);
					
					for (int j = surface[i + desert.X] - 1; j < val; j++) {

						int num6 = i + desert.X;
						int num7 = j;

						Neo.SetWall(num6, num7, 0, ref DrawInterface.Undo, false);

					}

				}

				PlaceSandColumn(i + desert.X, num4, desert2.Bottom - num4);

			}

		}

		private static void PlaceSandColumn(int startX, int startY, int height) {

			for (int num = startY + height - 1; num >= startY; num--) {

				int num2 = num;

				Neo.SetTile(startX, num2, 53, ref DrawInterface.Undo);
				Neo.SetLiquid(startX, num2, 0);

				if (num < startY)
					Neo.SetActive(startX, num2, false);

				WorldGen.SquareWallFrame(startX, num2);

			}

		}

	}

}
