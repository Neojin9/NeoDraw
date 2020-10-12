using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static void Spider(int x, int y, ref UndoStep undo, int maxLocations = -1, byte wall = 62) {

			if (!WorldGen.InWorld(x, y))
				return;

			UnifiedRandom genRand = WorldGen.genRand;

			int locationCount = 0;

			List<Point> locations  = new List<Point>();
			List<Point> nextLocations = new List<Point>();

			HashSet<Point> hashSet = new HashSet<Point>();

			nextLocations.Add(new Point(x, y));

			while (nextLocations.Count > 0 && (maxLocations == -1 || locationCount < maxLocations)) {

				locations.Clear();
				locations.AddRange(nextLocations);
				nextLocations.Clear();

				while (locations.Count > 0) {

					Point location = locations[0];

					if (!WorldGen.InWorld(location.X, location.Y, 1)) {

						locations.Remove(location);
						continue;

					}

					locationCount++;

					hashSet.Add(location);
					locations.Remove(location);

					Tile tile = Main.tile[location.X, location.Y];

					if (WorldGen.SolidTile(location.X, location.Y) || tile.wall != 0) {

						if (tile.active() && tile.wall == 0)
							Neo.SetWall(location.X, location.Y, wall, ref undo);

						continue;

					}

					Neo.SetWall(location.X, location.Y, wall, ref undo);

					WorldGen.SquareWallFrame(location.X, location.Y);

					if (!tile.active()) {

						Neo.SetLiquid(location.X, location.Y, 0, false);

						if (WorldGen.SolidTile(location.X, location.Y + 1) && genRand.Next(3) == 0) {

							if (genRand.Next(15) == 0) {

								AddBuriedChest(location.X, location.Y, ref undo, 939, notNearOtherChests: true, 15, trySlope: false, 0);

							}
							else {

								PlacePot(location.X, location.Y, ref undo, 28, genRand.Next(19, 21));

							}

						}

						if (!tile.active()) {

							if (WorldGen.SolidTile(location.X, location.Y - 1) && genRand.Next(3) == 0) {

								PlaceTight(location.X, location.Y, ref undo, spiders: true);

							}
							else if (WorldGen.SolidTile(location.X, location.Y + 1)) {

								PlaceTile(location.X, location.Y, 187, ref undo, mute: true, forced: false, -1, 9 + genRand.Next(5));

								if (genRand.Next(3) == 0) {

									if (!tile.active())
										PlaceSmallPile(location.X, location.Y, 34 + genRand.Next(4), 1, ref undo, 185);
									
									if (!tile.active())
										PlaceSmallPile(location.X, location.Y, 48 + genRand.Next(6), 0, ref undo, 185);
									
								}

							}

						}

					}

					Point nextLocation = new Point(location.X - 1, location.Y);

					if (!hashSet.Contains(nextLocation))
						nextLocations.Add(nextLocation);
					
					nextLocation = new Point(location.X + 1, location.Y);

					if (!hashSet.Contains(nextLocation))
						nextLocations.Add(nextLocation);
					
					nextLocation = new Point(location.X, location.Y - 1);

					if (!hashSet.Contains(nextLocation))
						nextLocations.Add(nextLocation);
					
					nextLocation = new Point(location.X, location.Y + 1);

					if (!hashSet.Contains(nextLocation))
						nextLocations.Add(nextLocation);
					
				}

			}

		}

	}

}
