using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;

namespace NeoDraw.WldGen.MicroBiomes { // Updated v1.4 7/23/2020

	public class Desert : MicroBiome {

		public float ChanceOfEntrance = 0.3333f;

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {
			
			DesertDescription desertDescription = DesertDescription.CreateFromPlacement(origin);
			
			if (!desertDescription.IsValid)
				return false;
			
			SandMound.Place(desertDescription);
			
			desertDescription.UpdateSurfaceMap();
			
			if (_random.NextFloat() <= ChanceOfEntrance) {

				switch (_random.Next(4)) {

					case 0: ChambersEntrance.Place(desertDescription); break;
					case 1: AnthillEntrance.Place(desertDescription); break;
					case 2: LarvaHoleEntrance.Place(desertDescription); break;
					case 3: PitEntrance.Place(desertDescription); break;

				}

			}

			DesertHive.Place(desertDescription);

			CleanupArea(desertDescription.Hive);

			return true;

		}

		private static void CleanupArea(Rectangle area) {

			for (int i = -20 + area.Left; i < area.Right + 20; i++) {

				for (int j = -20 + area.Top; j < area.Bottom + 20; j++) {

					if (i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1) {

						WorldGen.SquareWallFrame(i, j);
						Terraria.World.Generation.WorldUtils.TileFrame(i, j, true);

					}

				}

			}

		}

	}

}
