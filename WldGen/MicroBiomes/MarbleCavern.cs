﻿using System;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.MicroBiomes {

    class MarbleCavern : MicroBiome {

		public static bool SwitchToGranite;

		private delegate bool SlabState(int x, int y, int scale);

		private static class SlabStates {

            public static bool Empty(int x, int y, int scale) => false;

            public static bool Solid(int x, int y, int scale) => true;

            public static bool HalfBrick(int x, int y, int scale) => y >= scale / 2;

            public static bool BottomRightFilled(int x, int y, int scale) => x >= scale - y;

            public static bool BottomLeftFilled(int x, int y, int scale) => x < y;

            public static bool TopRightFilled(int x, int y, int scale) => x > y;

            public static bool TopLeftFilled(int x, int y, int scale) => x < scale - y;

        }

		private struct Slab {

			public readonly SlabState State;

			public readonly bool HasWall;

			public bool IsSolid => State != new SlabState(SlabStates.Empty);

			private Slab(SlabState state, bool hasWall) {
				State = state;
				HasWall = hasWall;
			}

            public Slab WithState(SlabState state) => new Slab(state, HasWall);

            public static Slab Create(SlabState state, bool hasWall) => new Slab(state, hasWall);

        }

		private const int SCALE = 3;

		private Slab[,] _slabs;

		private void SmoothSlope(int x, int y) {

			Slab slab = _slabs[x, y];

			if (slab.IsSolid) {

				bool isSolid  = _slabs[x, y - 1].IsSolid;
				bool isSolid2 = _slabs[x, y + 1].IsSolid;
				bool isSolid3 = _slabs[x - 1, y].IsSolid;
				bool isSolid4 = _slabs[x + 1, y].IsSolid;

				switch (((isSolid ? 1 : 0) << 3) | ((isSolid2 ? 1 : 0) << 2) | ((isSolid3 ? 1 : 0) << 1) | (isSolid4 ? 1 : 0)) {

					case 10:
						_slabs[x, y] = slab.WithState(SlabStates.TopLeftFilled);
						break;
					case 9:
						_slabs[x, y] = slab.WithState(SlabStates.TopRightFilled);
						break;
					case 6:
						_slabs[x, y] = slab.WithState(SlabStates.BottomLeftFilled);
						break;
					case 5:
						_slabs[x, y] = slab.WithState(SlabStates.BottomRightFilled);
						break;
					case 4:
						_slabs[x, y] = slab.WithState(SlabStates.HalfBrick);
						break;
					default:
						_slabs[x, y] = slab.WithState(SlabStates.Solid);
						break;

				}

			}

		}

		private void PlaceSlab(Slab slab, int originX, int originY, int scale) {

			ushort num = TileID.Marble;
			ushort wall = WallID.MarbleUnsafe;

			if (SwitchToGranite) {
				num = TileID.Granite;
				wall = WallID.GraniteUnsafe;
			}

			int num2 = -1;
			int num3 = scale + 1;
			int num4 = 0;
			int num5 = scale;

			for (int i = num2; i < num3; i++) {

				if ((i == num2 || i == num3 - 1) && WorldGen.genRand.Next(2) == 0)
					continue;
				
				if (WorldGen.genRand.Next(2) == 0)
					num4--;
				
				if (WorldGen.genRand.Next(2) == 0)
					num5++;
				
				for (int j = num4; j < num5; j++) {

					Tile tile = _tiles[originX + i, originY + j];

					DrawInterface.AddChangedTile(originX + i, originY + j);

					tile.ResetToType(TileID.Sets.Ore[tile.type] ? tile.type : num);

					bool active = slab.State(i, j, scale);

					tile.active(active);

					if (slab.HasWall)
						tile.wall = wall;
					
					Terraria.World.Generation.WorldUtils.TileFrame(originX + i, originY + j, frameNeighbors: true);
					WorldGen.SquareWallFrame(originX + i, originY + j);
					WldGen.SmoothSlope(originX + i, originY + j, ref DrawInterface.Undo);

					if (WorldGen.SolidTile(originX + i, originY + j - 1) && _random.Next(4) == 0)
						PlaceTight(originX + i, originY + j, ref DrawInterface.Undo);
					
					if (WorldGen.SolidTile(originX + i, originY + j) && _random.Next(4) == 0)
						PlaceTight(originX + i, originY + j - 1, ref DrawInterface.Undo);
					
				}

			}

		}

		private static bool IsGroupSolid(int x, int y, int scale) {

			int num = 0;
			
			for (int i = 0; i < scale; i++)
				for (int j = 0; j < scale; j++)
					if (WorldGen.SolidOrSlopedTile(x + i, y + j))
						num++;
					
			return num > scale / 4 * 3;

		}

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			if (_slabs == null)
				_slabs = new Slab[56, 26];
			
			int num  = _random.Next(80, 150) / 3;
			int num2 = _random.Next(40, 60) / 3;
			int num3 = (num2 * 3 - _random.Next(20, 30)) / 3;

			origin.X -= num * 3 / 2;
			origin.Y -= num2 * 3 / 2;

			for (int i = -1; i < num + 1; i++) {

				double num4 = (i - num / 2) / (double)num + 0.5;
				int num5 = (int)((0.5 - Math.Abs(num4 - 0.5)) * 5.0) - 2;

				for (int j = -1; j < num2 + 1; j++) {

					bool hasWall = true;
					bool flag = false;
					bool flag2 = IsGroupSolid(i * 3 + origin.X, j * 3 + origin.Y, 3);
					
					int num6 = Math.Abs(j - num2 / 2) - num3 / 4 + num5;
					
					if (num6 > 3) {
						flag = flag2;
						hasWall = false;
					}
					else if (num6 > 0) {
						flag = (j - num2 / 2 > 0 || flag2);
						hasWall = (j - num2 / 2 < 0 || num6 <= 2);
					}
					else if (num6 == 0) {
						flag = (_random.Next(2) == 0 && (j - num2 / 2 > 0 || flag2));
					}

					if (Math.Abs(num4 - 0.5) > 0.35f + _random.NextFloat() * 0.1f && !flag2) {
						hasWall = false;
						flag = false;
					}

					_slabs[i + 1, j + 1] = Slab.Create(flag ? new SlabState(SlabStates.Solid) : new SlabState(SlabStates.Empty), hasWall);

				}

			}

			for (int k = 0; k < num; k++)
				for (int l = 0; l < num2; l++)
					SmoothSlope(k + 1, l + 1);
				
			int num7 = num / 2;
			int num8 = num2 / 2;
			int num9 = (num8 + 1) * (num8 + 1);

			float value  = _random.NextFloat() * 2f - 1f;
			float num10  = _random.NextFloat() * 2f - 1f;
			float value2 = _random.NextFloat() * 2f - 1f;
			float num11 = 0f;

			for (int m = 0; m <= num; m++) {

				float num12 = num8 / (float)num7 * (m - num7);
				int num13 = Math.Min(num8, (int)Math.Sqrt(Math.Max(0f, num9 - num12 * num12)));
				
				num11 = ((m >= num / 2) ? (num11 + MathHelper.Lerp(num10, value2, m / (float)(num / 2) - 1f)) : (num11 + MathHelper.Lerp(value, num10, m / (float)(num / 2))));
				
				for (int n = num8 - num13; n <= num8 + num13; n++)
					PlaceSlab(_slabs[m + 1, n + 1], m * 3 + origin.X, n * 3 + origin.Y + (int)num11, 3);
				
			}

			return true;

		}

	}

}
