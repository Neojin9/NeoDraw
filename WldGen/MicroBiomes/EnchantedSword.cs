using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

	public class EnchantedSword : MicroBiome {


		private float _chanceOfEntrance  = 0.3333333f;
		private float _chanceOfRealSword = 0.3333333f;

		public static int RealSword;
		// 0: Random Sword
		// 1: Fake Sword
		// 2: Real Sword

		public static int Entrance;
		// 0: Random Entrance
		// 1: With Entrance
		// 2: Without Entrance

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();

			Gen(
				new Point(origin.X - 25, origin.Y - 25),
				new Shapes.Rectangle(50, 50),
				new Actions.TileScanner(0, 1).Output(dictionary)
			);
			
			Point result;

			Find(
				origin,
				Searches.Chain(
					new Searches.Up(1000),
					new Conditions.IsSolid().AreaOr(1, 50).Not()
				),
				out result
			);
			
			if (Find(
				origin,
				Searches.Chain(
					new Searches.Up(origin.Y - result.Y),
					new Conditions.IsTile(53)
				),
				out Point _)
			) {
				return false;
			}

			result.Y += 50;

			ShapeData shapeData  = new ShapeData();
			ShapeData shapeData2 = new ShapeData();
			
			Point point  = new Point(origin.X, origin.Y + 20);
			Point point2 = new Point(origin.X, origin.Y + 30);

			bool[] array = new bool[TileID.Sets.GeneralPlacementTiles.Length];
			
			for (int i = 0; i < array.Length; i++)
				array[i] = TileID.Sets.GeneralPlacementTiles[i];
			
			array[21]  = false;
			array[467] = false;

			float num = 0.8f + _random.NextFloat() * 0.5f;
			
			Gen(
				point,
				new Shapes.Slime(20, num, 1f),
				Actions.Chain(
					new Modifiers.Blotches(2, 0.4),
					new Actions.ClearTile(frameNeighbors: true).Output(shapeData)
				)
			);
			
			Gen(
				point2,
				new Shapes.Mound(14, 14),
				Actions.Chain(
					new Modifiers.Blotches(2, 1, 0.8),
					new Actions.SetTile(0),
					new Actions.SetFrames(frameNeighbors: true).Output(shapeData2)
				)
			);
			
			shapeData.Subtract(shapeData2, point, point2);
			
			Gen(
				point,
				new ModShapes.InnerOutline(shapeData),
				Actions.Chain(
					new Actions.SetTile(2),
					new Actions.SetFrames(frameNeighbors: true)
				)
			);
			
			Gen(
				point,
				new ModShapes.All(shapeData),
				Actions.Chain(
					new Modifiers.RectangleMask(-40, 40, 0, 40),
					new Modifiers.IsEmpty(),
					new Actions.SetLiquid()
				)
			);
			
			Gen(
				point,
				new ModShapes.All(shapeData),
				Actions.Chain(
					new Actions.PlaceWall(68),
					new Modifiers.OnlyTiles(2),
					new Modifiers.Offset(0, 1),
					new ActionVines(3, 5, 382)
				)
			);
			
			if ((_random.NextFloat() <= _chanceOfEntrance && Entrance != 2) || Entrance == 1) {

				ShapeData data = new ShapeData();
				
				Gen(
					new Point(origin.X, result.Y + 10),
					new Shapes.Rectangle(1, origin.Y - result.Y - 9),
					Actions.Chain(
						new Modifiers.Blotches(2, 0.2),
						new Modifiers.SkipTiles(191, 192),
						new Actions.ClearTile().Output(data),
						new Modifiers.Expand(1),
						new Modifiers.OnlyTiles(53),
						new Actions.SetTile(397).Output(data)
					)
				);
				
				Gen(
					new Point(origin.X, result.Y + 10),
					new ModShapes.All(data),
					new Actions.SetFrames(frameNeighbors: true)
				);

			}

			if ((_random.NextFloat() <= _chanceOfRealSword && RealSword != 1) || RealSword == 2) {
				PlaceTile(point2.X, point2.Y - 15, 187, ref DrawInterface.Undo, mute: true, forced: false, -1, 17);
			}
			else {
				PlaceTile(point2.X, point2.Y - 15, 186, ref DrawInterface.Undo, mute: true, forced: false, -1, 15);
			}

			Gen(
				point2,
				new ModShapes.All(shapeData2),
				Actions.Chain(
					new Modifiers.Offset(0, -1),
					new Modifiers.OnlyTiles(2),
					new Modifiers.Offset(0, -1),
					new ActionGrass()
				)
			);

			return true;

		}

	}

}
