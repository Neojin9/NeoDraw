using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.WldGen;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

    class MahoganyTree : MicroBiome {

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			if (!Find(new Point(origin.X - 3, origin.Y), Searches.Chain(new Searches.Down(200), new Conditions.IsSolid().AreaAnd(6, 1)), out Point result))
				return false;

			int height = WorldGen.genRand.Next(30, 60);
			int num3 = (height - 9) / 5;
			int num4 = num3 * 5;
			int num5 = 0;

			double num6 = _random.NextDouble() + 1.0;
			double num7 = _random.NextDouble() + 2.0;

			if (_random.Next(2) == 0)
				num7 = 0.0 - num7;
			
			for (int i = 0; i < num3; i++) {

				int num8 = (int)(Math.Sin((i + 1) / 12.0 * num6 * 3.1415927410125732) * num7);
				int num9 = (num8 < num5) ? (num8 - num5) : 0;
				
				Gen(
					new Point(result.X + num5 + num9, result.Y - (i + 1) * 5),
					new Shapes.Rectangle(6 + Math.Abs(num8 - num5), 7),
					Actions.Chain(
						new Actions.RemoveWall(),
						new Actions.SetTile(TileID.LivingMahogany),
						new Actions.SetFrames()
					)
				);
				
				Gen(
					new Point(result.X + num5 + num9 + 2, result.Y - (i + 1) * 5),
					new Shapes.Rectangle(2 + Math.Abs(num8 - num5), 5),
					Actions.Chain(
						new Actions.ClearTile(true),
						new Actions.PlaceWall(78)
					)
				);
				
				Gen(
					new Point(result.X + num5 + 2, result.Y - i * 5),
					new Shapes.Rectangle(2, 2),
					Actions.Chain(
						new Actions.ClearTile(true),
						new Actions.PlaceWall(78)
					)
				);
				
				num5 = num8;

			}

			int num10 = 6;

			if (num7 < 0.0)
				num10 = 0;
			
			List<Point> points = new List<Point>();

			for (int j = 0; j < 2; j++) {

				double num11 = (j + 1.0) / 3.0;
				int num12 = num10 + (int)(Math.Sin(num3 * num11 / 12.0 * num6 * 3.1415927410125732) * num7);
				double num13 = _random.NextDouble() * -0.2;
				
				if (num10 == 0)
					num13 -= 1.5707963705062866;
				
				Gen(
					new Point(result.X + num12, result.Y - (int)(num3 * 5 * num11)),
					new Shapes.Branch(num13, _random.Next(12, 16)).OutputEndpoints(points),
					Actions.Chain(
						new Actions.SetTile(TileID.LivingMahogany),
						new Actions.SetFrames(true)
					)
				);
				
				num10 = 6 - num10;

			}

			int num14 = (int)(Math.Sin(num3 / 12.0 * num6 * 3.1415927410125732) * num7);

			Gen(
				new Point(result.X + 6 + num14, result.Y - num4),
				new Shapes.Branch(-0.68539818525314333, _random.Next(16, 22)).OutputEndpoints(points),
				Actions.Chain(
					new Actions.SetTile(TileID.LivingMahogany),
					new Actions.SetFrames(true)
				)
			);
			
			Gen(
				new Point(result.X + num14, result.Y - num4),
				new Shapes.Branch(-2.4561944961547852, _random.Next(16, 22)).OutputEndpoints(points),
				Actions.Chain(
					new Actions.SetTile(TileID.LivingMahogany),
					new Actions.SetFrames(true)
				)
			);
			
			foreach (Point item in points) {

				Gen(
					item,
					new Shapes.Circle(4),
					Actions.Chain(
						new Modifiers.Blotches(4, 2),
						new Modifiers.SkipTiles(TileID.LivingMahogany),
						new Modifiers.SkipWalls(78),
						new Actions.SetTile(TileID.LivingMahoganyLeaves),
						new Actions.SetFrames(true)
					)
				);

			}

			for (int k = 0; k < 4; k++) {

				float angle = k / 3f * 2f + 0.57075f;

				Gen(
					result,
					new Shapes.Root(angle, _random.Next(40, 60)),
					new Actions.SetTile(TileID.LivingMahogany, true)
				);

			}

			AddBuriedChest(result.X + 3, result.Y - 1, ref DrawInterface.Undo, (_random.Next(4) != 0) ? WorldGen.GetNextJungleChestItem() : 0, false, 10, false, 0);

			return true;

		}

	}

}
