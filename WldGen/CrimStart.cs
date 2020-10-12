using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static bool crimson;
		public static int heartCount;
		public static Vector2[] heartPos;

		public static void CrimStart(int i, int j, int crimDir, ref UndoStep undo) {

			heartPos = new Vector2[100];

			heartCount = 0;
			crimson = true;
			int k = j;

			if (k > Main.worldSurface)
				k = (int)Main.worldSurface;

			for (; !WorldGen.SolidTile(i, k); k++) {
			}

			int num = k;
			Vector2 position = new Vector2(i, k);
			Vector2 vector = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.1f, WorldGen.genRand.Next(20, 201) * 0.01f);

			if (crimDir == 0) {

				crimDir = 1;

				if (vector.X < 0f)
					crimDir = -1;

			}

			float num2 = WorldGen.genRand.Next(15, 26);
			bool flag = true;
			int num3 = 0;

			while (flag) {

				num2 += WorldGen.genRand.Next(-50, 51) * 0.01f;

				if (num2 < 15f)
					num2 = 15f;

				if (num2 > 25f)
					num2 = 25f;

				for (int l = (int)(position.X - num2 / 2f); l < position.X + num2 / 2f; l++) {

					for (int m = (int)(position.Y - num2 / 2f); m < position.Y + num2 / 2f; m++) {

						if (m > num) {

							if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.3) {

								undo.Add(new ChangedTile(l, m));
								Main.tile[l, m].active(active: false);
								Main.tile[l, m].wall = 83;

							} else if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.8 && Main.tile[l, m].wall != 83) {

								undo.Add(new ChangedTile(l, m));
								Main.tile[l, m].active(active: true);
								Main.tile[l, m].type = 203;

								if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.6)
									Main.tile[l, m].wall = 83;

							}

						} else if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.3 && Main.tile[l, m].active()) {

							undo.Add(new ChangedTile(l, m));
							Main.tile[l, m].active(active: false);
							Main.tile[l, m].wall = 83;

						}

					}

				}

				if (position.X > i + 50)
					num3 = -100;

				if (position.X < i - 50)
					num3 = 100;

				if (num3 < 0) {
					vector.X -= WorldGen.genRand.Next(20, 51) * 0.01f;
				} else if (num3 > 0) {
					vector.X += WorldGen.genRand.Next(20, 51) * 0.01f;
				} else {
					vector.X += WorldGen.genRand.Next(-50, 51) * 0.01f;
				}

				vector.Y += WorldGen.genRand.Next(-50, 51) * 0.01f;

				if (vector.Y < 0.25)
					vector.Y = 0.25f;

				if (vector.Y > 2f)
					vector.Y = 2f;

				if (vector.X < -2f)
					vector.X = -2f;

				if (vector.X > 2f)
					vector.X = 2f;

				position += vector;

				if (position.Y > Main.worldSurface + 100.0)
					flag = false;

			}

			num2 = WorldGen.genRand.Next(40, 55);

			for (int n = 0; n < 50; n++) {

				int num4 = (int)position.X + WorldGen.genRand.Next(-20, 21);
				int num5 = (int)position.Y + WorldGen.genRand.Next(-20, 21);

				for (int num6 = (int)(num4 - num2 / 2f); num6 < num4 + num2 / 2f; num6++) {

					for (int num7 = (int)(num5 - num2 / 2f); num7 < num5 + num2 / 2f; num7++) {

						float num8 = Math.Abs(num6 - num4);
						float num9 = Math.Abs(num7 - num5);
						float num10 = 1f + WorldGen.genRand.Next(-20, 21) * 0.01f;
						float num11 = 1f + WorldGen.genRand.Next(-20, 21) * 0.01f;
						float num12 = num8 * num10;
						num9 *= num11;
						double num13 = Math.Sqrt(num12 * num12 + num9 * num9);

						if (num13 < num2 * 0.25) {

							undo.Add(new ChangedTile(num6, num7));
							Main.tile[num6, num7].active(active: false);
							Main.tile[num6, num7].wall = 83;

						} else if (num13 < num2 * 0.4 && Main.tile[num6, num7].wall != 83) {

							undo.Add(new ChangedTile(num6, num7));
							Main.tile[num6, num7].active(active: true);
							Main.tile[num6, num7].type = 203;

							if (num13 < num2 * 0.35)
								Main.tile[num6, num7].wall = 83;
							

						}

					}

				}

			}

			int num14 = WorldGen.genRand.Next(5, 9);
			Vector2[] array = new Vector2[num14];

			for (int num15 = 0; num15 < num14; num15++) {

				int num16 = (int)position.X;
				int num17 = (int)position.Y;
				int num18 = 0;
				bool flag2 = true;
				Vector2 vector2 = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.15f, WorldGen.genRand.Next(0, 21) * 0.15f);

				while (flag2) {

					vector2 = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.15f, WorldGen.genRand.Next(0, 21) * 0.15f);

					while (Math.Abs(vector2.X) + Math.Abs(vector2.Y) < 1.5)
						vector2 = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.15f, WorldGen.genRand.Next(0, 21) * 0.15f);

					flag2 = false;

					for (int num19 = 0; num19 < num15; num19++) {

						if (vector.X > array[num19].X - 0.75 && vector.X < array[num19].X + 0.75 && vector.Y > array[num19].Y - 0.75 && vector.Y < array[num19].Y + 0.75) {

							flag2 = true;
							num18++;
							break;

						}

					}

					if (num18 > 10000)
						break;

				}

				array[num15] = vector2;
				CrimVein(new Vector2(num16, num17), vector2, ref undo, false);

			}

			for (int num20 = 0; num20 < heartCount; num20++) {

				num2 = WorldGen.genRand.Next(16, 21);
				int num21 = (int)heartPos[num20].X;
				int num22 = (int)heartPos[num20].Y;

				for (int num23 = (int)(num21 - num2 / 2f); num23 < num21 + num2 / 2f; num23++) {

					for (int num24 = (int)(num22 - num2 / 2f); num24 < num22 + num2 / 2f; num24++) {

						float num25 = Math.Abs(num23 - num21);
						float num26 = Math.Abs(num24 - num22);

						if (Math.Sqrt(num25 * num25 + num26 * num26) < num2 * 0.4) {

							undo.Add(new ChangedTile(num23, num24));
							Main.tile[num23, num24].active(active: true);
							Main.tile[num23, num24].type = 203;
							Main.tile[num23, num24].wall = 83;

						}

					}

				}

			}

			for (int num27 = 0; num27 < heartCount; num27++) {

				num2 = WorldGen.genRand.Next(10, 14);
				int num28 = (int)heartPos[num27].X;
				int num29 = (int)heartPos[num27].Y;

				for (int num30 = (int)(num28 - num2 / 2f); num30 < num28 + num2 / 2f; num30++) {

					for (int num31 = (int)(num29 - num2 / 2f); num31 < num29 + num2 / 2f; num31++) {

						float num32 = Math.Abs(num30 - num28);
						float num33 = Math.Abs(num31 - num29);

						if (Math.Sqrt(num32 * num32 + num33 * num33) < num2 * 0.3) {

							undo.Add(new ChangedTile(num30, num31));
							Main.tile[num30, num31].active(active: false);
							Main.tile[num30, num31].wall = 83;

						}

					}

				}

			}

			for (int num34 = 0; num34 < heartCount; num34++)
				AddShadowOrb((int)heartPos[num34].X, (int)heartPos[num34].Y, ref undo);

			int num35 = Main.maxTilesX;
			int num36 = 0;
			position.X = i;
			position.Y = num;
			num2 = WorldGen.genRand.Next(25, 35);
			float num37 = WorldGen.genRand.Next(0, 6);

			for (int num38 = 0; num38 < 50; num38++) {

				if (num37 > 0f) {
					float num39 = WorldGen.genRand.Next(10, 30) * 0.01f;
					num37 -= num39;
					position.Y -= num39;
				}

				int num40 = (int)position.X + WorldGen.genRand.Next(-2, 3);
				int num41 = (int)position.Y + WorldGen.genRand.Next(-2, 3);

				for (int num42 = (int)(num40 - num2 / 2f); num42 < num40 + num2 / 2f; num42++) {

					for (int num43 = (int)(num41 - num2 / 2f); num43 < num41 + num2 / 2f; num43++) {

						float num44 = Math.Abs(num42 - num40);
						float num45 = Math.Abs(num43 - num41);
						float num46 = 1f + WorldGen.genRand.Next(-20, 21) * 0.005f;
						float num47 = 1f + WorldGen.genRand.Next(-20, 21) * 0.005f;
						float num48 = num44 * num46;
						num45 *= num47;
						double num49 = Math.Sqrt(num48 * num48 + num45 * num45);

						if (num49 < num2 * 0.2 * (WorldGen.genRand.Next(90, 111) * 0.01)) {

							undo.Add(new ChangedTile(num42, num43));
							Main.tile[num42, num43].active(active: false);
							Main.tile[num42, num43].wall = 83;

						} else {

							if (!(num49 < num2 * 0.45))
								continue;

							if (num42 < num35)
								num35 = num42;

							if (num42 > num36)
								num36 = num42;

							if (Main.tile[num42, num43].wall != 83) {

								undo.Add(new ChangedTile(num42, num43));
								Main.tile[num42, num43].active(active: true);
								Main.tile[num42, num43].type = 203;

								if (num49 < num2 * 0.35) {
									Main.tile[num42, num43].wall = 83;
								}

							}

						}

					}

				}

			}

			for (int num50 = num35; num50 <= num36; num50++) {

				int num51;

				for (num51 = num; (Main.tile[num50, num51].type == 203 && Main.tile[num50, num51].active()) || Main.tile[num50, num51].wall == 83; num51++) {
				}

				int num52 = WorldGen.genRand.Next(15, 20);

				for (; !Main.tile[num50, num51].active(); num51++) {

					if (num52 <= 0)
						break;

					if (Main.tile[num50, num51].wall == 83)
						break;

					num52--;

					undo.Add(new ChangedTile(num50, num51));
					Main.tile[num50, num51].type = 203;
					Main.tile[num50, num51].active(active: true);

				}

			}

			CrimEnt(position, crimDir, ref undo, false);

			undo.ResetFrames(true);

		}

		/*public static void CrimStart(int i, int j) {

			heartPos = new Vector2[100];

			int crimDir = 1;
			heartCount = 0;
			crimson = true;
			int k = j;

			if (k > Main.worldSurface)
				k = (int)Main.worldSurface;
			
			for (; !WorldGen.SolidTile(i, k); k++) {
			}

			int num = k;
			Vector2 position = new Vector2(i, k);
			Vector2 vector = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.1f, WorldGen.genRand.Next(20, 201) * 0.01f);

			if (vector.X < 0f)
				crimDir = -1;
			
			float num2 = WorldGen.genRand.Next(15, 26);
			bool flag = true;
			int num3 = 0;

			while (flag) {

				num2 += WorldGen.genRand.Next(-50, 51) * 0.01f;

				if (num2 < 15f)
					num2 = 15f;
				
				if (num2 > 25f)
					num2 = 25f;
				
				for (int l = (int)(position.X - num2 / 2f); l < position.X + num2 / 2f; l++) {

					for (int m = (int)(position.Y - num2 / 2f); m < position.Y + num2 / 2f; m++) {

						if (m > num) {

							if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.3) {

								Main.tile[l, m].active(active: false);
								Main.tile[l, m].wall = 83;

							} else if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.8 && Main.tile[l, m].wall != 83) {

								Main.tile[l, m].active(active: true);
								Main.tile[l, m].type = 203;

								if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.6)
									Main.tile[l, m].wall = 83;
								
							}

						} else if (Math.Abs(l - position.X) + Math.Abs(m - position.Y) < num2 * 0.3 && Main.tile[l, m].active()) {

							Main.tile[l, m].active(active: false);
							Main.tile[l, m].wall = 83;

						}

					}

				}

				if (position.X > i + 50)
					num3 = -100;
				
				if (position.X < i - 50)
					num3 = 100;
				
				if (num3 < 0) {
					vector.X -= WorldGen.genRand.Next(20, 51) * 0.01f;
				} else if (num3 > 0) {
					vector.X += WorldGen.genRand.Next(20, 51) * 0.01f;
				} else {
					vector.X += WorldGen.genRand.Next(-50, 51) * 0.01f;
				}

				vector.Y += WorldGen.genRand.Next(-50, 51) * 0.01f;

				if (vector.Y < 0.25)
					vector.Y = 0.25f;
				
				if (vector.Y > 2f)
					vector.Y = 2f;
				
				if (vector.X < -2f)
					vector.X = -2f;
				
				if (vector.X > 2f)
					vector.X = 2f;
				
				position += vector;

				if (position.Y > Main.worldSurface + 100.0)
					flag = false;
				
			}

			num2 = WorldGen.genRand.Next(40, 55);
			
			for (int n = 0; n < 50; n++) {

				int num4 = (int)position.X + WorldGen.genRand.Next(-20, 21);
				int num5 = (int)position.Y + WorldGen.genRand.Next(-20, 21);
				
				for (int num6 = (int)(num4 - num2 / 2f); num6 < num4 + num2 / 2f; num6++) {

					for (int num7 = (int)(num5 - num2 / 2f); num7 < num5 + num2 / 2f; num7++) {

						float num8 = Math.Abs(num6 - num4);
						float num9 = Math.Abs(num7 - num5);
						float num10 = 1f + WorldGen.genRand.Next(-20, 21) * 0.01f;
						float num11 = 1f + WorldGen.genRand.Next(-20, 21) * 0.01f;
						float num12 = num8 * num10;
						num9 *= num11;
						double num13 = Math.Sqrt(num12 * num12 + num9 * num9);
						
						if (num13 < num2 * 0.25) {

							Main.tile[num6, num7].active(active: false);
							Main.tile[num6, num7].wall = 83;

						} else if (num13 < num2 * 0.4 && Main.tile[num6, num7].wall != 83) {

							Main.tile[num6, num7].active(active: true);
							Main.tile[num6, num7].type = 203;
							
							if (num13 < num2 * 0.35) {
								Main.tile[num6, num7].wall = 83;
							}

						}

					}

				}

			}

			int num14 = WorldGen.genRand.Next(5, 9);
			Vector2[] array = new Vector2[num14];

			for (int num15 = 0; num15 < num14; num15++) {

				int num16 = (int)position.X;
				int num17 = (int)position.Y;
				int num18 = 0;
				bool flag2 = true;
				Vector2 vector2 = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.15f, WorldGen.genRand.Next(0, 21) * 0.15f);
				
				while (flag2) {

					vector2 = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.15f, WorldGen.genRand.Next(0, 21) * 0.15f);
					
					while (Math.Abs(vector2.X) + Math.Abs(vector2.Y) < 1.5)
						vector2 = new Vector2(WorldGen.genRand.Next(-20, 21) * 0.15f, WorldGen.genRand.Next(0, 21) * 0.15f);
					
					flag2 = false;

					for (int num19 = 0; num19 < num15; num19++) {

						if (vector.X > array[num19].X - 0.75 && vector.X < array[num19].X + 0.75 && vector.Y > array[num19].Y - 0.75 && vector.Y < array[num19].Y + 0.75) {

							flag2 = true;
							num18++;
							break;

						}

					}

					if (num18 > 10000)
						break;
					
				}

				array[num15] = vector2;
				CrimVein(new Vector2(num16, num17), vector2);

			}

			for (int num20 = 0; num20 < heartCount; num20++) {

				num2 = WorldGen.genRand.Next(16, 21);
				int num21 = (int)heartPos[num20].X;
				int num22 = (int)heartPos[num20].Y;

				for (int num23 = (int)(num21 - num2 / 2f); num23 < num21 + num2 / 2f; num23++) {

					for (int num24 = (int)(num22 - num2 / 2f); num24 < num22 + num2 / 2f; num24++) {

						float num25 = Math.Abs(num23 - num21);
						float num26 = Math.Abs(num24 - num22);

						if (Math.Sqrt(num25 * num25 + num26 * num26) < num2 * 0.4) {

							Main.tile[num23, num24].active(active: true);
							Main.tile[num23, num24].type = 203;
							Main.tile[num23, num24].wall = 83;

						}

					}

				}

			}

			for (int num27 = 0; num27 < heartCount; num27++) {

				num2 = WorldGen.genRand.Next(10, 14);
				int num28 = (int)heartPos[num27].X;
				int num29 = (int)heartPos[num27].Y;

				for (int num30 = (int)(num28 - num2 / 2f); num30 < num28 + num2 / 2f; num30++) {

					for (int num31 = (int)(num29 - num2 / 2f); num31 < num29 + num2 / 2f; num31++) {

						float num32 = Math.Abs(num30 - num28);
						float num33 = Math.Abs(num31 - num29);
						
						if (Math.Sqrt(num32 * num32 + num33 * num33) < num2 * 0.3) {

							Main.tile[num30, num31].active(active: false);
							Main.tile[num30, num31].wall = 83;

						}

					}

				}

			}

			for (int num34 = 0; num34 < heartCount; num34++)
				AddShadowOrb((int)heartPos[num34].X, (int)heartPos[num34].Y);
			
			int num35 = Main.maxTilesX;
			int num36 = 0;
			position.X = i;
			position.Y = num;
			num2 = WorldGen.genRand.Next(25, 35);
			float num37 = WorldGen.genRand.Next(0, 6);

			for (int num38 = 0; num38 < 50; num38++) {

				if (num37 > 0f) {
					float num39 = WorldGen.genRand.Next(10, 30) * 0.01f;
					num37 -= num39;
					position.Y -= num39;
				}

				int num40 = (int)position.X + WorldGen.genRand.Next(-2, 3);
				int num41 = (int)position.Y + WorldGen.genRand.Next(-2, 3);
				
				for (int num42 = (int)(num40 - num2 / 2f); num42 < num40 + num2 / 2f; num42++) {

					for (int num43 = (int)(num41 - num2 / 2f); num43 < num41 + num2 / 2f; num43++) {

						float num44 = Math.Abs(num42 - num40);
						float num45 = Math.Abs(num43 - num41);
						float num46 = 1f + WorldGen.genRand.Next(-20, 21) * 0.005f;
						float num47 = 1f + WorldGen.genRand.Next(-20, 21) * 0.005f;
						float num48 = num44 * num46;
						num45 *= num47;
						double num49 = Math.Sqrt(num48 * num48 + num45 * num45);
						
						if (num49 < num2 * 0.2 * (WorldGen.genRand.Next(90, 111) * 0.01)) {

							Main.tile[num42, num43].active(active: false);
							Main.tile[num42, num43].wall = 83;

						} else {

							if (!(num49 < num2 * 0.45))
								continue;
							
							if (num42 < num35)
								num35 = num42;
							
							if (num42 > num36)
								num36 = num42;
							
							if (Main.tile[num42, num43].wall != 83) {

								Main.tile[num42, num43].active(active: true);
								Main.tile[num42, num43].type = 203;

								if (num49 < num2 * 0.35) {
									Main.tile[num42, num43].wall = 83;
								}

							}

						}

					}

				}

			}

			for (int num50 = num35; num50 <= num36; num50++) {

				int num51;
				
				for (num51 = num; (Main.tile[num50, num51].type == 203 && Main.tile[num50, num51].active()) || Main.tile[num50, num51].wall == 83; num51++) {
				}
				
				int num52 = WorldGen.genRand.Next(15, 20);
				
				for (; !Main.tile[num50, num51].active(); num51++) {

					if (num52 <= 0)
						break;
					
					if (Main.tile[num50, num51].wall == 83)
						break;
					
					num52--;
					Main.tile[num50, num51].type = 203;
					Main.tile[num50, num51].active(active: true);

				}

			}

			CrimEnt(position, crimDir);

		}
		*/

	}

}
