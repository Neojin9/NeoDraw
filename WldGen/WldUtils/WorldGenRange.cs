using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/25/2020

	public class WorldGenRange {

		public enum ScalingMode {
			None,
			WorldArea,
			WorldWidth
		}

		public static readonly WorldGenRange Empty = new WorldGenRange(0, 0);

		public readonly int Minimum;
		public readonly int Maximum;

		public readonly ScalingMode ScaleWith;

		public int ScaledMinimum => ScaleValue(Minimum);

		public int ScaledMaximum => ScaleValue(Maximum);

		public WorldGenRange(int minimum, int maximum) {
			Minimum = minimum;
			Maximum = maximum;
		}

        public int GetRandom(UnifiedRandom random) => random.Next(ScaledMinimum, ScaledMaximum + 1);

        private int ScaleValue(int value) {

			float num = 1f;
			
			switch (ScaleWith) {

				case ScalingMode.WorldArea:
					num = (float)(Main.maxTilesX * Main.maxTilesY) / 5040000f;
					break;

				case ScalingMode.WorldWidth:
					num = (float)Main.maxTilesX / 4200f;
					break;

				case ScalingMode.None:
					num = 1f;
					break;

			}

			return (int)(num * value);

		}

	}

}
