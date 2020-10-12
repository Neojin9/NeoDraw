using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

	public static class AnthillEntrance {

		public static void Place(DesertDescription description) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num = genRand.Next(2, 4);
			
			for (int i = 0; i < num; i++) {

				int holeRadius = genRand.Next(15, 18);
				int num2 = (int)((i + 1) / (num + 1) * (float)description.Surface.Width);

				num2 += description.Desert.Left;
				
				int y = description.Surface[num2];
				
				PlaceAt(description, new Point(num2, y), holeRadius);

			}

		}

		private static void PlaceAt(DesertDescription description, Point position, int holeRadius) {

			ShapeData data = new ShapeData();
			Point origin = new Point(position.X, position.Y + 6);
			
			Gen(
				origin,
				new Shapes.Tail(holeRadius * 2, new Vector2(0f, (-holeRadius) * 1.5f)),
				Actions.Chain(
					new Actions.SetTile(53).Output(data)
				)
			);
			
			GenShapeActionPair genShapeActionPair = new GenShapeActionPair(
				new Shapes.Rectangle(1, 1),
				Actions.Chain(
					new Modifiers.Blotches(),
					new Modifiers.IsSolid(),
					new Actions.Clear(),
					new Actions.PlaceWall(187)
				)
			);
			
			GenShapeActionPair genShapeActionPair2 = new GenShapeActionPair(
				new Shapes.Rectangle(1, 1),
				Actions.Chain(
					new Modifiers.IsSolid(),
					new Actions.Clear(),
					new Actions.PlaceWall(187)
				)
			);
			
			GenShapeActionPair pair = new GenShapeActionPair(
				new Shapes.Circle(2, 3),
				Actions.Chain(
					new Modifiers.IsSolid(),
					new Actions.SetTile(397),
					new Actions.PlaceWall(187)
				)
			);
			
			GenShapeActionPair pair2 = new GenShapeActionPair(
				new Shapes.Circle(holeRadius, 3),
				Actions.Chain(
					new Modifiers.SkipWalls(187),
					new Actions.SetTile(53)
				)
			);
			
			GenShapeActionPair pair3 = new GenShapeActionPair(
				new Shapes.Circle(holeRadius - 2, 3),
				Actions.Chain(
					new Actions.PlaceWall(187)
				)
			);
			
			int num = position.X;
			
			for (int i = position.Y - holeRadius - 3; i < description.Hive.Top + (position.Y - description.Desert.Top) * 2 + 12; i++) {

				Gen(new Point(num, i), (i < position.Y) ? genShapeActionPair2 : genShapeActionPair);
				
				Gen(new Point(num, i), pair);
				
				if (i % 3 == 0 && i >= position.Y) {

					num += WorldGen.genRand.Next(-1, 2);
					
					Gen(new Point(num, i), genShapeActionPair);
					
					if (i >= position.Y + 5) {

						Gen(new Point(num, i), pair2);
						Gen(new Point(num, i), pair3);

					}

					Gen(new Point(num, i), pair);

				}

			}

			Gen(
				new Point(origin.X, origin.Y - (int)(holeRadius * 1.5f) + 3),
				new Shapes.Circle(holeRadius / 2, holeRadius / 3),
				Actions.Chain(
					Actions.Chain(
						new Actions.ClearTile(),
						new Modifiers.Expand(1),
						new Actions.PlaceWall(0)
					)
				)
			);
			
			Gen(
				origin,
				new ModShapes.All(data),
				new Actions.Smooth()
			);

		}

	}

}
