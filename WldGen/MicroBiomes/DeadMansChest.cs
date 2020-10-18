using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

	public class DeadMansChest : MicroBiome {

		private class DartTrapPlacementAttempt {

			public int directionX;
			public int xPush;
			public int x;
			public int y;

			public Point position;

			public Tile t;

			public DartTrapPlacementAttempt(Point position, int directionX, int x, int y, int xPush, Tile t) {

				this.position = position;
				this.directionX = directionX;
				this.x = x;
				this.y = y;
				this.xPush = xPush;
				this.t = t;

			}

		}

		private class BoulderPlacementAttempt {

			public Point position;

			public int yPush;
			public int requiredHeight;
			public int bestType;

			public BoulderPlacementAttempt(Point position, int yPush, int requiredHeight, int bestType) {

				this.position = position;
				this.yPush = yPush;
				this.requiredHeight = requiredHeight;
				this.bestType = bestType;

			}

		}

		private class WirePlacementAttempt {

			public Point position;

			public int dirX;
			public int dirY;
			public int steps;

			public WirePlacementAttempt(Point position, int dirX, int dirY, int steps) {

				this.position = position;
				this.dirX = dirX;
				this.dirY = dirY;
				this.steps = steps;

			}

		}

		private class ExplosivePlacementAttempt {

			public Point position;

			public ExplosivePlacementAttempt(Point position) {
				this.position = position;
			}

		}

		private List<DartTrapPlacementAttempt> _dartTrapPlacementSpots = new List<DartTrapPlacementAttempt>();

		private List<WirePlacementAttempt> _wirePlacementSpots = new List<WirePlacementAttempt>();

		private List<BoulderPlacementAttempt> _boulderPlacementSpots = new List<BoulderPlacementAttempt>();

		private List<ExplosivePlacementAttempt> _explosivePlacementAttempt = new List<ExplosivePlacementAttempt>();

		private IntRange _numberOfDartTraps = new IntRange(3, 6);
		private IntRange _numberOfBoulderTraps = new IntRange(2, 4);
		private IntRange _numberOfStepsBetweenBoulderTraps = new IntRange(2, 4);

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			if (!IsAGoodSpot(origin))
				return false;
			
			ClearCaches();

			Point position = new Point(origin.X, origin.Y + 1);
			
			FindBoulderTrapSpots(position);
			FindDartTrapSpots(position);
			FindExplosiveTrapSpots(position);
			
			if (!AreThereEnoughTraps())
				return false;
			
			//TurnGoldChestIntoDeadMansChest(origin);
			
			foreach (DartTrapPlacementAttempt dartTrapPlacementSpot in _dartTrapPlacementSpots)
				ActuallyPlaceDartTrap(dartTrapPlacementSpot.position, dartTrapPlacementSpot.directionX, dartTrapPlacementSpot.x, dartTrapPlacementSpot.y, dartTrapPlacementSpot.xPush, dartTrapPlacementSpot.t);
			
			foreach (WirePlacementAttempt wirePlacementSpot in _wirePlacementSpots)
				PlaceWireLine(wirePlacementSpot.position, wirePlacementSpot.dirX, wirePlacementSpot.dirY, wirePlacementSpot.steps);
			
			foreach (BoulderPlacementAttempt boulderPlacementSpot in _boulderPlacementSpots)
				ActuallyPlaceBoulderTrap(boulderPlacementSpot.position, boulderPlacementSpot.yPush, boulderPlacementSpot.requiredHeight, boulderPlacementSpot.bestType);
			
			foreach (ExplosivePlacementAttempt item in _explosivePlacementAttempt)
				ActuallyPlaceExplosive(item.position);
			
			PlaceWiresForExplosives(origin);

			return true;

		}

		private void PlaceWiresForExplosives(Point origin) {

			if (_explosivePlacementAttempt.Count <= 0)
				return;
			
			PlaceWireLine(origin, 0, 1, _explosivePlacementAttempt[0].position.Y - origin.Y);

			int num = _explosivePlacementAttempt[0].position.X;
			int num2 = _explosivePlacementAttempt[0].position.X;
			int y = _explosivePlacementAttempt[0].position.Y;

			for (int i = 0; i < _explosivePlacementAttempt.Count; i++) {

				int x = _explosivePlacementAttempt[i].position.X;

				if (i == 0)
					x = origin.X;
				
				if (num > x)
					num = x;
				
				if (num2 < x)
					num2 = x;
				
			}

			PlaceWireLine(new Point(num, y), 1, 0, num2 - num);

		}

		private bool AreThereEnoughTraps() {

			if (_boulderPlacementSpots.Count < 1 && _explosivePlacementAttempt.Count < 1) {
				DrawInterface.SetStatusBarTempMessage("Unable to place enough Boulder/Explosive traps");
            } else if (_dartTrapPlacementSpots.Count < 1) {
				DrawInterface.SetStatusBarTempMessage("Unable to place enough Dart traps");
            }

			if (_boulderPlacementSpots.Count >= 1 || _explosivePlacementAttempt.Count >= 1)
				return _dartTrapPlacementSpots.Count >= 1;
			
			return false;

		}

		private void ClearCaches() {

			_dartTrapPlacementSpots.Clear();
			_wirePlacementSpots.Clear();
			_boulderPlacementSpots.Clear();
			_explosivePlacementAttempt.Clear();

		}

		private void FindBoulderTrapSpots(Point position) {

			int x = position.X;
			int num = _random.Next(_numberOfBoulderTraps);
			int num2 = _random.Next(_numberOfStepsBetweenBoulderTraps);
			
			x -= num / 2 * num2;
			
			int num3 = position.Y - 6;
			
			for (int i = 0; i <= num; i++) {
				FindBoulderTrapSpot(new Point(x, num3));
				x += num2;
			}

			if (_boulderPlacementSpots.Count <= 0)
				return;
			
			int num4 = _boulderPlacementSpots[0].position.X;
			int num5 = _boulderPlacementSpots[0].position.X;

			for (int j = 1; j < _boulderPlacementSpots.Count; j++) {

				int x2 = _boulderPlacementSpots[j].position.X;
				
				if (num4 > x2)
					num4 = x2;
				
				if (num5 < x2)
					num5 = x2;
				
			}

			if (num4 > position.X)
				num4 = position.X;
			
			if (num5 < position.X)
				num5 = position.X;
			
			_wirePlacementSpots.Add(new WirePlacementAttempt(new Point(num4, num3 - 1), 1, 0, num5 - num4));
			_wirePlacementSpots.Add(new WirePlacementAttempt(position, 0, -1, 7));

		}

		private void FindBoulderTrapSpot(Point position) {

			int x = position.X;
			int y = position.Y;
			int num = 0;
			
			while (true) {

				if (num < 50) {

					if (_tiles[x, y - num].active())
						break;
					
					num++;

					continue;

				}

				return;

			}

			PlaceBoulderTrapSpot(new Point(x, y - num), num);

		}

		private void PlaceBoulderTrapSpot(Point position, int yPush) {

			int[] array = new int[623];
			
			for (int i = position.X; i < position.X + 2; i++) {

				for (int j = position.Y - 4; j <= position.Y; j++) {

					Tile tile = _tiles[i, j];
					
					if (tile.active() && !Main.tileFrameImportant[tile.type] && Main.tileSolid[tile.type])
						array[tile.type]++;
					
					if (tile.active() && !TileID.Sets.CanBeClearedDuringGeneration[tile.type])
						return;
					
				}

			}

			for (int k = position.X - 1; k < position.X + 2 + 1; k++)
				for (int l = position.Y - 4 - 1; l <= position.Y - 4 + 2; l++)
					if (!_tiles[k, l].active())
						return;
					
			int num = -1;

			for (int m = 0; m < array.Length; m++)
				if (num == -1 || array[num] < array[m])
					num = m;
				
			_boulderPlacementSpots.Add(new BoulderPlacementAttempt(position, yPush - 1, 4, num));

		}

		private void FindDartTrapSpots(Point position) {

			int num = _random.Next(_numberOfDartTraps);
			int num2 = (_random.Next(2) != 0) ? 1 : (-1);
			int steps = -1;

			for (int i = 0; i < num; i++) {

				bool num3 = FindDartTrapSpotSingle(position, num2);
				num2 *= -1;
				position.Y--;
				
				if (num3)
					steps = i;
				
			}

			_wirePlacementSpots.Add(new WirePlacementAttempt(new Point(position.X, position.Y + num), 0, -1, steps));

		}

		private bool FindDartTrapSpotSingle(Point position, int directionX) {

			int x = position.X;
			int y = position.Y;

			for (int i = 0; i < 20; i++) {

				Tile tile = _tiles[x + i * directionX, y];
				
				if (tile.type != 467 && tile.active() && Main.tileSolid[tile.type]) {

					if (i >= 5 && !tile.actuator() && !Main.tileFrameImportant[tile.type] && TileID.Sets.CanBeClearedDuringGeneration[tile.type]) {
						_dartTrapPlacementSpots.Add(new DartTrapPlacementAttempt(position, directionX, x, y, i, tile));
						return true;
					}

					return false;

				}

			}

			return false;

		}

		private void FindExplosiveTrapSpots(Point position) {

			int x = position.X;
			int y = position.Y + 3;
			
			List<int> list = new List<int>();
			
			if (IsGoodSpotsForExplosive(x, y))
				list.Add(x);
			
			x++;

			if (IsGoodSpotsForExplosive(x, y))
				list.Add(x);
			
			int num = -1;

			if (list.Count > 0)
				num = list[_random.Next(list.Count)];
			
			list.Clear();

			x += _random.Next(2, 6);
			
			int num2 = 4;

			for (int i = x; i < x + num2; i++)
				if (IsGoodSpotsForExplosive(i, y))
					list.Add(i);
				
			int num3 = -1;

			if (list.Count > 0)
				num3 = list[_random.Next(list.Count)];
			
			x = position.X - num2 - _random.Next(2, 6);

			for (int j = x; j < x + num2; j++)
				if (IsGoodSpotsForExplosive(j, y))
					list.Add(j);
				
			int num4 = -1;

			if (list.Count > 0)
				num4 = list[_random.Next(list.Count)];
			
			if (num4 != -1)
				_explosivePlacementAttempt.Add(new ExplosivePlacementAttempt(new Point(num4, y)));
			
			if (num != -1)
				_explosivePlacementAttempt.Add(new ExplosivePlacementAttempt(new Point(num, y)));
			
			if (num3 != -1)
				_explosivePlacementAttempt.Add(new ExplosivePlacementAttempt(new Point(num3, y)));
			
		}

		private bool IsGoodSpotsForExplosive(int x, int y) {

			Tile tile = _tiles[x, y];
			
			if (tile.active() && Main.tileSolid[tile.type] && !Main.tileFrameImportant[tile.type] && !Main.tileSolidTop[tile.type])
				return true;
			
			return false;

		}

		private static bool IsAGoodSpot(Point position) {
			
			if (!WorldGen.InWorld(position.X, position.Y, 50))
				return false;

			if (OceanDepths(position.X, position.Y)) {
				DrawInterface.SetStatusBarTempMessage("Too close to edge of world");
				return false;
			}
			
			Tile tile = _tiles[position.X, position.Y];

			//if (tile.type != TileID.Containers && tile.type != TileID.FakeContainers && tile.type != TileID.Containers2 && tile.type != TileID.FakeContainers2) {
			//	DrawInterface.SetStatusBarTempMessage("Must place over a Chest", 150);
			//	return false;
			//}
			
			//if (tile.frameX / 36 != 1)
			//	return false;
			
			return true;

		}

		private void TurnGoldChestIntoDeadMansChest(Point position) {

			for (int i = 0; i < 2; i++) {

				for (int j = 0; j < 2; j++) {

					int num = position.X + i;
					int num2 = position.Y + j;
					
					Tile tile = _tiles[num, num2];

					DrawInterface.AddChangedTile(num, num2);

					tile.type = 467;
					tile.frameX = (short)(144 + i * 18);
					tile.frameY = (short)(j * 18);

				}

			}

		}

		private void ActuallyPlaceDartTrap(Point position, int directionX, int x, int y, int xPush, Tile t) {

			DrawInterface.AddChangedTile(position.X + xPush * directionX, position.Y);

			t.type = 137;
			t.frameY = 0;

			if (directionX == -1) {
				t.frameX = 18;
			}
			else {
				t.frameX = 0;
			}

			t.slope(0);
			t.halfBrick(false);

			WorldGen.TileFrame(x, y, true);
			
			PlaceWireLine(position, directionX, 0, xPush);

		}

		private void PlaceWireLine(Point start, int offsetX, int offsetY, int steps) {

			for (int i = 0; i <= steps; i++) {

				DrawInterface.AddChangedTile(start.X + offsetX * i, start.Y + offsetY * i);
				_tiles[start.X + offsetX * i, start.Y + offsetY * i].wire(true);

			}

		}

		private void ActuallyPlaceBoulderTrap(Point position, int yPush, int requiredHeight, int bestType) {

			for (int i = position.X; i < position.X + 2; i++) {

				for (int j = position.Y - requiredHeight; j <= position.Y + 2; j++) {

					Tile tile = _tiles[i, j];

					DrawInterface.AddChangedTile(i, j);

					if (j < position.Y - requiredHeight + 2) {
						tile.ClearTile();
					}
					else if (j <= position.Y) {

						if (!tile.active()) {

							tile.active(true);
							tile.type = (ushort)bestType;

						}

						tile.slope(0);
						tile.halfBrick(false);
						tile.actuator(true);
						tile.wire(true);

						WorldGen.TileFrame(i, j, true);

					}
					else {
						tile.ClearTile();
					}

				}

			}

			int num  = position.X + 1;
			int num2 = position.Y - requiredHeight + 1;
			int num3 = 3;
			int num4 = num - num3;
			int num5 = num2 - num3;
			int num6 = num + num3 - 1;
			int num7 = num2 + num3 - 1;

			for (int k = num4; k <= num6; k++) {

				for (int l = num5; l <= num7; l++) {

					if (_tiles[k, l].type != 138) {

						DrawInterface.AddChangedTile(k, l);
						_tiles[k, l].type = 1;

					}

				}

			}

			PlaceTile(num, num2, 138, ref DrawInterface.Undo);
			PlaceWireLine(position, 0, 1, yPush);

		}

		private void ActuallyPlaceExplosive(Point position) {

			Tile tile = _tiles[position.X, position.Y];

			DrawInterface.AddChangedTile(position.X, position.Y);

			tile.type = 141;
			tile.frameX = tile.frameY = 0;
			tile.slope(0);
			tile.halfBrick(false);
			
			WorldGen.TileFrame(position.X, position.Y, true);

		}

	}

}
