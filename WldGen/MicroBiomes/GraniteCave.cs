using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

	public class GraniteCave : MicroBiome {

		// 0: Granite - Default
		// 1: Granite - With Lava
		// 2: Granite - Without Lava
		// 3: Marble - Default
		// 4: Marble - With Lava
		// 5: Marble - Without Lava

		private readonly struct Magma {

			public readonly float Pressure;
			public readonly float Resistance;
			public readonly bool IsActive;

			private Magma(float pressure, float resistance, bool active) {

				Pressure   = pressure;
				Resistance = resistance;
				IsActive   = active;

			}

            public Magma ToFlow() => new Magma(Pressure, Resistance, active: true);

            public static Magma CreateFlow(float pressure, float resistance = 0f) => new Magma(pressure, resistance, active: true);

            public static Magma CreateEmpty(float resistance = 0f) => new Magma(0f, resistance, active: false);

        }

		private const int MAX_MAGMA_ITERATIONS = 300;

		private Magma[,] _sourceMagmaMap = new Magma[150, 150];
		private Magma[,] _targetMagmaMap = new Magma[150, 150];

		private static readonly Vector2[] _normalisedVectors = {

			Vector2.Normalize(new Vector2(-1f, -1f)),
			Vector2.Normalize(new Vector2(-1f, 0f)),
			Vector2.Normalize(new Vector2(-1f, 1f)),
			Vector2.Normalize(new Vector2(0f, -1f)),

			new Vector2(0f, 0f),

			Vector2.Normalize(new Vector2(0f, 1f)),
			Vector2.Normalize(new Vector2(1f, -1f)),
			Vector2.Normalize(new Vector2(1f, 0f)),
			Vector2.Normalize(new Vector2(1f, 1f))

		};

		public static int Style;

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			origin.X -= _sourceMagmaMap.GetLength(0) / 2;
			origin.Y -= _sourceMagmaMap.GetLength(1) / 2;

			BuildMagmaMap(origin);
			SimulatePressure(out Rectangle effectedMapArea);
			PlaceGranite(origin, effectedMapArea);
			CleanupTiles(origin, effectedMapArea);
			PlaceDecorations(origin, effectedMapArea);

			return true;

		}

		private void BuildMagmaMap(Point tileOrigin) {

			_sourceMagmaMap = new Magma[150, 150];
			_targetMagmaMap = new Magma[150, 150];
			
			for (int i = 0; i < _sourceMagmaMap.GetLength(0); i++) {

				for (int j = 0; j < _sourceMagmaMap.GetLength(1); j++) {

					int i2 = i + tileOrigin.X;
					int j2 = j + tileOrigin.Y;
					
					_sourceMagmaMap[i, j] = Magma.CreateEmpty(WorldGen.SolidTile(i2, j2) ? 4f : 1f);
					_targetMagmaMap[i, j] = _sourceMagmaMap[i, j];

				}

			}

		}

		private void SimulatePressure(out Rectangle effectedMapArea) {

			int length = _sourceMagmaMap.GetLength(0);
			int length2 = _sourceMagmaMap.GetLength(1);
			int num = length / 2;
			int num2 = length2 / 2;
			int num3 = num;
			int num4 = num3;
			int num5 = num2;
			int num6 = num5;

			for (int i = 0; i < MAX_MAGMA_ITERATIONS; i++) {

				for (int j = num3; j <= num4; j++) {

					for (int k = num5; k <= num6; k++) {

						Magma magma = _sourceMagmaMap[j, k];
						
						if (!magma.IsActive)
							continue;
						
						float num7 = 0f;

						Vector2 zero = Vector2.Zero;

						for (int l = -1; l <= 1; l++) {

							for (int m = -1; m <= 1; m++) {

								if (l == 0 && m == 0)
									continue;
								
								Vector2 value = _normalisedVectors[(l + 1) * 3 + (m + 1)];
								Magma magma2 = _sourceMagmaMap[j + l, k + m];

								if (magma.Pressure > 0.01f && !magma2.IsActive) {

									if (l == -1) {
										num3 = Utils.Clamp(j + l, 1, num3);
									}
									else {
										num4 = Utils.Clamp(j + l, num4, length - 2);
									}

									if (m == -1) {
										num5 = Utils.Clamp(k + m, 1, num5);
									}
									else {
										num6 = Utils.Clamp(k + m, num6, length2 - 2);
									}

									_targetMagmaMap[j + l, k + m] = magma2.ToFlow();

								}

								float pressure = magma2.Pressure;

								num7 += pressure;
								zero += pressure * value;

							}

						}

						num7 /= 8f;

						if (num7 > magma.Resistance) {

							float num8 = zero.Length() / 8f;
							float val = Math.Max(num7 - num8 - magma.Pressure, 0f) + num8 + magma.Pressure * 0.875f - magma.Resistance;

							val = Math.Max(0f, val);

							_targetMagmaMap[j, k] = Magma.CreateFlow(val, Math.Max(0f, magma.Resistance - val * 0.02f));

						}

					}

				}

				if (i < 2)
					_targetMagmaMap[num, num2] = Magma.CreateFlow(25f);
				
				Utils.Swap(ref _sourceMagmaMap, ref _targetMagmaMap);

			}

			effectedMapArea = new Rectangle(num3, num5, num4 - num3 + 1, num6 - num5 + 1);

		}

		private bool ShouldUseLava(Point tileOrigin) {

			if (Style == 2 || Style == 5) {
				return false;
            }
			else if (Style == 1 || Style == 4) {
				return true;
            }

			int length = _sourceMagmaMap.GetLength(0);
			int length2 = _sourceMagmaMap.GetLength(1);
			int num = length / 2;
			int num2 = length2 / 2;
			
			if (tileOrigin.Y + num2 <= WorldGen.lavaLine - 30)
				return false;
			
			for (int i = -50; i < 50; i++) {

				for (int j = -50; j < 50; j++) {

					if (_tiles[tileOrigin.X + num + i, tileOrigin.Y + num2 + j].active()) {

						ushort type = _tiles[tileOrigin.X + num + i, tileOrigin.Y + num2 + j].type;

						if (type == 147 || (uint)(type - 161) <= 2u || type == 200)
							return false;
						
					}

				}

			}

			return true;

		}

		private void PlaceGranite(Point tileOrigin, Rectangle magmaMapArea) {

			bool useLava = ShouldUseLava(tileOrigin);

			ushort type = TileID.Granite;
			ushort wall = WallID.GraniteUnsafe;

			if (Style > 2) {
				type = TileID.Marble;
				wall = WallID.MarbleUnsafe;
			}

			for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++) {

				for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++) {

					Magma magma = _sourceMagmaMap[i, j];

					if (!magma.IsActive)
						continue;
					
					Tile tile = _tiles[tileOrigin.X + i, tileOrigin.Y + j];

					float num = (float)Math.Sin((tileOrigin.Y + j) * 0.4f) * 0.7f + 1.2f;
					float num2 = 0.2f + 0.5f / (float)Math.Sqrt(Math.Max(0f, magma.Pressure - magma.Resistance));

					if (Math.Max(1f - Math.Max(0f, num * num2), magma.Pressure / 15f) > 0.35f + (WorldGen.SolidTile(tileOrigin.X + i, tileOrigin.Y + j) ? 0f : 0.5f)) {

						DrawInterface.AddChangedTile(tileOrigin.X + i, tileOrigin.Y + j);

						if (TileID.Sets.Ore[tile.type]) {
							tile.ResetToType(tile.type);
						}
						else {
							tile.ResetToType(type);
						}

						tile.wall = wall;

					}
					else if (magma.Resistance < 0.01f) {

						DrawInterface.AddChangedTile(tileOrigin.X + i, tileOrigin.Y + j);

						ClearTile(tileOrigin.X + i, tileOrigin.Y + j);
						tile.wall = wall;

					}

					if (tile.liquid > 0 && useLava) {

						DrawInterface.AddChangedTile(tileOrigin.X + i, tileOrigin.Y + j);

						tile.liquidType(1);

					}

				}

			}

		}

		private void CleanupTiles(Point tileOrigin, Rectangle magmaMapArea) {

			ushort wall = WallID.GraniteUnsafe;

			if (Style > 2)
				wall = WallID.MarbleUnsafe;
			
			List<Point16> list = new List<Point16>();

			for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++) {

				for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++) {

					if (!_sourceMagmaMap[i, j].IsActive)
						continue;
					
					int num = 0;
					int num2 = i + tileOrigin.X;
					int num3 = j + tileOrigin.Y;

					if (!WorldGen.SolidTile(num2, num3))
						continue;
					
					for (int k = -1; k <= 1; k++)
						for (int l = -1; l <= 1; l++)
							if (WorldGen.SolidTile(num2 + k, num3 + l))
								num++;
							
					if (num < 3)
						list.Add(new Point16(num2, num3));
					
				}

			}

			foreach (Point16 item in list) {

				int x = item.X;
				int y = item.Y;

				DrawInterface.AddChangedTile(x, y);

				ClearTile(x, y, frameNeighbors: true);

				_tiles[x, y].wall = wall;

			}

			list.Clear();

		}

		private void PlaceDecorations(Point tileOrigin, Rectangle magmaMapArea) {

			FastRandom fastRandom = new FastRandom(Main.ActiveWorldFileData.Seed).WithModifier(65440uL);

			for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++) {

				for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++) {

					Magma magma = _sourceMagmaMap[i, j];
					
					int num = i + tileOrigin.X;
					int num2 = j + tileOrigin.Y;
					
					if (!magma.IsActive)
						continue;

					Terraria.World.Generation.WorldUtils.TileFrame(num, num2);
					WorldGen.SquareWallFrame(num, num2);
					
					FastRandom fastRandom2 = fastRandom.WithModifier(num, num2);

					if (fastRandom2.Next(8) == 0 && _tiles[num, num2].active()) {

						if (!_tiles[num, num2 + 1].active())
							PlaceUncheckedStalactite(num, num2 + 1, fastRandom2.Next(2) == 0, fastRandom2.Next(3), spiders: false, ref DrawInterface.Undo);
						
						if (!_tiles[num, num2 - 1].active())
							PlaceUncheckedStalactite(num, num2 - 1, fastRandom2.Next(2) == 0, fastRandom2.Next(3), spiders: false, ref DrawInterface.Undo);
						
					}

					if (fastRandom2.Next(2) == 0)
						SmoothSlope(num, num2, ref DrawInterface.Undo);
					
				}

			}

		}

	}

}
