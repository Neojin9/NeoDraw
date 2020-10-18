using System;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.MicroBiomes { // Updated v1.4 7/24/2020

	public class SurfaceMap {

		public readonly float Average;

		public readonly int Bottom;
		public readonly int Top;
		public readonly int X;

		private readonly short[] _heights;

		public int Width => _heights.Length;

		public short this[int absoluteX] => _heights[absoluteX - X];

		private SurfaceMap(short[] heights, int x) {

			_heights = heights;
			X = x;

			int num = 0;
			int num2 = int.MaxValue;
			int num3 = 0;
			
			foreach (short height in heights) {

				num3 += height;
				num = Math.Max(num, height);
				num2 = Math.Min(num2, height);

			}

			if (num > Main.worldSurface - 10.0)
				num = (int)Main.worldSurface - 10;
			
			Bottom = num;
			Top = num2;
			Average = num3 / _heights.Length;

		}

		public static SurfaceMap FromArea(int startX, int width) {

			int num = Main.maxTilesY / 2;
			short[] array = new short[width];
			
			for (int i = startX; i < startX + width; i++) {

				bool flag = false;
				int num2 = 0;
				
				for (int j = 50; j < 50 + num; j++) {

					if (Main.tile[i, j].active()) {

						if (Main.tile[i, j].type == TileID.Cloud || Main.tile[i, j].type == TileID.RainCloud || Main.tile[i, j].type == TileID.SnowCloud) {
							flag = false;
						}
						else if (!flag) {
							num2 = j;
							flag = true;
						}

					}

					if (!flag)
						num2 = num + 50;
					
				}

				array[i - startX] = (short)num2;

			}

			return new SurfaceMap(array, startX);

		}

	}

}
