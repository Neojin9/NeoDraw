using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // Updated v1.4 7/23/2020

	public class CorruptionPit : MicroBiome {

		public static bool[] ValidTiles = TileID.Sets.Factory.CreateBoolSet(true, 21, 31, 26);

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			if (!Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid()), out origin))
				return false;
			
			ShapeData shapeData  = new ShapeData();
			ShapeData shapeData2 = new ShapeData();
			ShapeData shapeData3 = new ShapeData();

			for (int i = 0; i < 6; i++) {

				Gen(
					origin,
					new Shapes.Circle(_random.Next(10, 12) + i),
					Actions.Chain(
						new Modifiers.Offset(0, 5 * i + 5),
						new Modifiers.Blotches(3).Output(shapeData)
					)
				);

			}

			for (int j = 0; j < 6; j++) {

				Gen(
					origin,
					new Shapes.Circle(_random.Next(5, 7) + j),
					Actions.Chain(
						new Modifiers.Offset(0, 2 * j + 18),
						new Modifiers.Blotches(3).Output(shapeData2)
					)
				);

			}

			for (int k = 0; k < 6; k++) {

				Gen(
					origin,
					new Shapes.Circle(_random.Next(4, 6) + k / 2),
					Actions.Chain(
						new Modifiers.Offset(0, (int)(7.5f * k) - 10),
						new Modifiers.Blotches(3).Output(shapeData3)
					)
				);

			}

			ShapeData shapeData4 = new ShapeData(shapeData2);

			shapeData2.Subtract(shapeData3, origin, origin);
			shapeData4.Subtract(shapeData2, origin, origin);
			
			Rectangle bounds = ShapeData.GetBounds(origin, shapeData, shapeData3);
			
			if (!structures.CanPlace(bounds, ValidTiles, 2))
				return false;
			
			Gen(
				origin,
				new ModShapes.All(shapeData),
				Actions.Chain(
					new Actions.SetTile(25, setSelfFrames: true),
					new Actions.PlaceWall(3)
				)
			);
			
			Gen(
				origin,
				new ModShapes.All(shapeData2),
				new Actions.SetTile(0, setSelfFrames: true)
			);
			
			Gen(
				origin,
				new ModShapes.All(shapeData3),
				new Actions.ClearTile(frameNeighbors: true)
			);
			
			Gen(
				origin,
				new ModShapes.All(shapeData2),
				Actions.Chain(
					new Modifiers.IsTouchingAir(useDiagonals: true),
					new Modifiers.NotTouching(false, 25),
					new Actions.SetTile(23, setSelfFrames: true)
				)
			);
			
			Gen(
				origin,
				new ModShapes.All(shapeData4),
				new Actions.PlaceWall(69)
			);
			
			return true;

		}

	}

}
