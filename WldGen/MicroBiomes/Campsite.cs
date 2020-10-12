using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

	public class Campsite : MicroBiome { // Updated 8/11/2020

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			int size        = _random.Next(6, 10);
			int leftBlotch  = _random.Next(1, 3); // Was 5. Changed to 1 - 3.
			int upBlotch    = _random.Next(1, 3); // Was leftBlotch. Changed to 1 - 3.
			int rightBlotch = _random.Next(1, 3); // Was leftBlotch. Changed to 1 - 3.
			int downBlotch  = 1;

			ushort wallType = (byte)(196 + _random.Next(4)); // Wall Type DirtUnsafe1 - DirtUnsafe 4

			for (int i = origin.X - size; i <= origin.X + size; i++) {

				for (int j = origin.Y - size; j <= origin.Y + size; j++) {

					if (Main.tile[i, j].active()) {

						int tileType = Main.tile[i, j].type;

						if (tileType == TileID.Sand || tileType == TileID.Sandstone || tileType == TileID.HardenedSand || tileType == TileID.DesertFossil) {
							wallType = WallID.Sandstone;
						}
						else if (tileType == TileID.IceBlock || tileType == TileID.SnowBlock) {
							wallType = WallID.SnowWallUnsafe;
						}
						else if (tileType == TileID.JungleGrass) {
							wallType = (byte)(204 + _random.Next(4));
						}
						else if (tileType == TileID.Marble) {
							wallType = WallID.MarbleUnsafe;
						}
						else if (tileType == TileID.Granite) {
							wallType = WallID.GraniteUnsafe;
						}

					}

				}

			}

			ShapeData data = new ShapeData();

			Gen(
				origin,
				new Shapes.Slime(size),
				Actions.Chain(
					new Modifiers.Blotches(leftBlotch, upBlotch, rightBlotch, downBlotch).Output(data),
					new Modifiers.Offset(0, -2),
					new Modifiers.OnlyTiles(TileID.Sand, TileID.Crimsand, TileID.Ebonsand, TileID.Pearlsand),
					new Actions.SetTile(TileID.HardenedSand, setSelfFrames: true),
					new Modifiers.OnlyWalls(default(ushort)),
					new Actions.PlaceWall(wallType)
				)
			);

			Gen(
				origin,
				new ModShapes.All(data),
				Actions.Chain(
					new Actions.ClearTile(),
					new Actions.SetLiquid(0, 0),
					new Actions.SetFrames(frameNeighbors: true),
					new Modifiers.OnlyWalls(default(ushort)),
					new Actions.PlaceWall(wallType)
				)
			);
			
			if (!Find(origin, Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out Point result))
				return false;
			
			int num3 = result.Y - 1;
			bool flag = _random.Next() % 2 == 0;

			WeightedRandom<ushort> weightedRandom = new WeightedRandom<ushort>();
			weightedRandom.Add(TileID.CopperCoinPile, 50);
			weightedRandom.Add(TileID.SilverCoinPile, 35);
			weightedRandom.Add(TileID.GoldCoinPile, 14);
			weightedRandom.Add(TileID.PlatinumCoinPile, .1);

			ushort coinType = weightedRandom;

			if (_random.Next() % 10 != 0) {

				int num4 = _random.Next(1, 4);
				int num5 = flag ? 4 : (-(size >> 1));

				for (int k = 0; k < num4; k++) {

					int num6 = _random.Next(1, 3);

					for (int l = 0; l < num6; l++)
						PlaceTile(origin.X + num5 - k, num3 - l, coinType, ref undo, mute: true);
					
				}

			}

			int num7 = (size - 3) * ((!flag) ? 1 : (-1));

			if (_random.Next() % 10 != 0)
				PlaceTile(origin.X + num7, num3, TileID.LargePiles, ref undo, mute: true);

			if (_random.Next() % 10 != 0) {

				PlaceTile(origin.X, num3, TileID.Campfire, ref undo);

				if (_tiles[origin.X, num3].active() && _tiles[origin.X, num3].type == TileID.Campfire) {

					_tiles[origin.X, num3].frameY += 36;
					_tiles[origin.X - 1, num3].frameY += 36;
					_tiles[origin.X + 1, num3].frameY += 36;
					_tiles[origin.X, num3 - 1].frameY += 36;
					_tiles[origin.X - 1, num3 - 1].frameY += 36;
					_tiles[origin.X + 1, num3 - 1].frameY += 36;

				}

			}

			undo.ResetFrames();

			return true;

		}

	}

}
