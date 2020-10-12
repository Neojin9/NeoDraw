using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldUtils.WldUtils;


namespace NeoDraw.WldGen.MicroBiomes { // Updated v1.4 7/25/2020

    class MiningExplosives : MicroBiome {

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			ushort type = Utils.SelectRandom(_random, (WorldGen.goldBar == 19) ? TileID.Gold : TileID.Platinum, (WorldGen.silverBar == 21) ? TileID.Silver : TileID.Tungsten, (WorldGen.ironBar == 22) ? TileID.Iron : TileID.Lead, (WorldGen.copperBar == 20) ? TileID.Copper : TileID.Tin);
			double num = _random.NextDouble() * 2.0 - 1.0;
			
			if (!Find(origin, Searches.Chain((num > 0.0) ? new Searches.Right(40) : ((GenSearch)new Searches.Left(40)), new Conditions.IsSolid()), out origin))
				return false;
			
			if (!Find(origin, Searches.Chain(new Searches.Down(80), new Conditions.IsSolid()), out origin))
				return false;
			
			ShapeData shapeData = new ShapeData();
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);

			Gen(
				origin,
				new Shapes.Runner(10f, 20, new Vector2((float)num, 1f)).Output(shapeData),
				Actions.Chain(
					new Modifiers.Blotches(),
					new Actions.Scanner(@ref),
					new Modifiers.IsSolid(),
					new Actions.Scanner(ref2)
				)
			);
			
			if (ref2.Value < @ref.Value / 2)
				return false;
			
			Gen(
				origin,
				new ModShapes.All(shapeData),
				new Actions.SetTile(type, setSelfFrames: true)
			);
			
			Gen(
				new Point(origin.X - (int)(num * -5.0), origin.Y - 5),
				new Shapes.Circle(5),
				Actions.Chain(
					new Modifiers.Blotches(),
					new Actions.ClearTile(frameNeighbors: true)
				)
			);
			
			int num2 = 1 & (
				Find(
					new Point(origin.X - ((num > 0.0) ? 3 : (-3)), origin.Y - 3),
					Searches.Chain(
						new Searches.Down(10),
						new Conditions.IsSolid()
					),
					out Point result) ? 1 : 0);
			
			int num3 = (_random.Next(4) == 0) ? 3 : 7;

			if ((num2 & (
				Find(
					new Point(origin.X - ((num > 0.0) ? (-num3) : num3), origin.Y - 3),
					Searches.Chain(
						new Searches.Down(10),
						new Conditions.IsSolid()
					),
					out Point result2) ? 1 : 0)) == 0
				) {

				return false;

			}

			result2.Y--;

			Tile tile = _tiles[result.X, result.Y];

			DrawInterface.AddChangedTile(result.X, result.Y);

			tile.slope(0);
			tile.halfBrick(halfBrick: false);
			
			for (int i = -1; i <= 1; i++) {

				DrawInterface.AddChangedTile(result2.X + i, result2.Y);
				DrawInterface.AddChangedTile(result2.X + i, result2.Y - 1);
				DrawInterface.AddChangedTile(result2.X + i, result2.Y - 2);

				ClearTile(result2.X + i, result2.Y);
				ClearTile(result2.X + i, result2.Y - 1);
				ClearTile(result2.X + i, result2.Y - 2);

				Tile tile2 = _tiles[result2.X + i, result2.Y + 1];

				DrawInterface.AddChangedTile(result2.X + i, result2.Y + 1);

				if (!WorldGen.SolidOrSlopedTile(tile2)) {
					tile2.ResetToType(1);
					tile2.active(active: true);
				}

				tile2.slope(0);
				tile2.halfBrick(halfBrick: false);

				Terraria.World.Generation.WorldUtils.TileFrame(result2.X + i, result2.Y + 1, frameNeighbors: true);

			}

			result.Y--;

			DrawInterface.AddChangedTile(result.X, result.Y);
			Main.tile[result.X, result.Y].ClearEverything();

			PlaceTile(result.X, result.Y, TileID.Explosives, ref DrawInterface.Undo);

			PlaceTile(result2.X, result2.Y, TileID.Detonator, ref DrawInterface.Undo, mute: true, forced: true);

			WireLine(result, result2, ref DrawInterface.Undo);

			return true;

		}

	}

}
