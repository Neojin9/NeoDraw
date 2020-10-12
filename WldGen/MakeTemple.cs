using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/31/2020 Half updated, half copy/paste.

		public static int lAltarX;
		public static int lAltarY;
        public static int tLeft;
        public static int tRight;
        public static int tTop;
        public static int tBottom;
        public static int tRooms;

		public static void makeTemple(int x, int y, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			Rectangle[] array = new Rectangle[100];

			float num = Main.maxTilesX / 4200; // Scale with World Size
			int num2 = genRand.Next((int)(num * 10f), (int)(num * 16f));

			if (DrunkWorldGen)
				num2 *= 3;

			if (GetGoodWorldGen)
				num2 *= 3;

			int num3 = 1;

			if (genRand.Next(2) == 0) // Direction?
				num3 = -1;
			
			int num4 = num3;
			int num5 = x;
			int num6 = y;
			int num7 = x;
			int num8 = y;
			int num9 = genRand.Next(1, 3);
			int num10 = 0;

			for (int i = 0; i < num2; i++) {

				num10++;

				int num11 = num3;
				int num12 = num7;
				int num13 = num8;
				bool flag = true;
				int num14 = 0;
				int num15 = 0;
				int num16 = -10;

				Rectangle rectangle = new Rectangle(num12 - num14 / 2, num13 - num15 / 2, num14, num15);

				while (flag) {

					num12 = num7;
					num13 = num8;
					num14 = genRand.Next(25, 50);
					num15 = genRand.Next(20, 35);

					if (num15 > num14)
						num15 = num14;
					
					if (i == num2 - 1) {

						num14 = genRand.Next(55, 65);
						num15 = genRand.Next(45, 50);

						if (num15 > num14)
							num15 = num14;
						
						num14 = (int)(num14 * 1.6);
						num15 = (int)(num15 * 1.35);
						num13 += genRand.Next(5, 10);

					}

					if (num10 > num9) {

						num13 += genRand.Next(num15 + 1, num15 + 3) + num16;
						num12 += genRand.Next(-5, 6);
						num11 = num3 * -1;

					}
					else {

						num12 += (genRand.Next(num14 + 1, num14 + 3) + num16) * num11;
						num13 += genRand.Next(-5, 6);

					}

					flag = false;
					rectangle = new Rectangle(num12 - num14 / 2, num13 - num15 / 2, num14, num15);

					for (int j = 0; j < i; j++) {

						if (rectangle.Intersects(array[j]))
							flag = true;
						
						if (genRand.Next(100) == 0)
							num16++;
						
					}

				}

				if (num10 > num9) {
					num9++;
					num10 = 1;
				}

				array[i] = rectangle;
				num3 = num11;
				num7 = num12;
				num8 = num13;

			}

			for (int k = 0; k < num2; k++) {

				for (int l = 0; l < 2; l++) {

					for (int m = 0; m < num2; m++) {

						for (int n = 0; n < 2; n++) {

							int num17 = array[k].X;
							
							if (l == 1)
								num17 += array[k].Width - 1;
							
							int num18 = array[k].Y;
							int num19 = num18 + array[k].Height;
							int num20 = array[m].X;
							
							if (n == 1)
								num20 += array[m].Width - 1;
							
							int y2 = array[m].Y;
							int num21 = y2 + array[m].Height;
							
							while (num17 != num20 || num18 != y2 || num19 != num21) {

								if (num17 < num20)
									num17++;
								
								if (num17 > num20)
									num17--;
								
								if (num18 < y2)
									num18++;
								
								if (num18 > y2)
									num18--;
								
								if (num19 < num21)
									num19++;
								
								if (num19 > num21)
									num19--;
								
								int num22 = num17;

								for (int num23 = num18; num23 < num19; num23++) {

									undo.Add(new ChangedTile(num22, num23));

									Main.tile[num22, num23].active(active: true);
									Main.tile[num22, num23].type = TileID.LihzahrdBrick;
									Main.tile[num22, num23].liquid = 0;
									Main.tile[num22, num23].slope(0);
									Main.tile[num22, num23].halfBrick(halfBrick: false);

								}

							}

						}

					}

				}

			}

			for (int num24 = 0; num24 < num2; num24++) {

				//if (genRand.Next(1) != 0)
				//	continue;

				if (false)
					continue;

				for (int num25 = array[num24].X; num25 < array[num24].X + array[num24].Width; num25++) {

					for (int num26 = array[num24].Y; num26 < array[num24].Y + array[num24].Height; num26++) {

						undo.Add(new ChangedTile(num25, num26));

						Main.tile[num25, num26].active(active: true);
						Main.tile[num25, num26].type = TileID.LihzahrdBrick;
						Main.tile[num25, num26].liquid = 0;
						Main.tile[num25, num26].slope(0);
						Main.tile[num25, num26].halfBrick(halfBrick: false);

					}

				}

				int x2 = array[num24].X;
				int num27 = x2 + array[num24].Width;
				int y3 = array[num24].Y;
				int num28 = y3 + array[num24].Height;

				x2 += genRand.Next(3, 8);
				num27 -= genRand.Next(3, 8);
				y3 += genRand.Next(3, 8);
				num28 -= genRand.Next(3, 8);

				int num29 = x2;
				int num30 = num27;
				int num31 = y3;
				int num32 = num28;
				int num33 = (x2 + num27) / 2;
				int num34 = (y3 + num28) / 2;

				for (int num35 = x2; num35 < num27; num35++) {

					for (int num36 = y3; num36 < num28; num36++) {

						if (genRand.Next(20) == 0)
							num31 += genRand.Next(-1, 2);
						
						if (genRand.Next(20) == 0)
							num32 += genRand.Next(-1, 2);
						
						if (genRand.Next(20) == 0)
							num29 += genRand.Next(-1, 2);
						
						if (genRand.Next(20) == 0)
							num30 += genRand.Next(-1, 2);
						
						if (num29 < x2)
							num29 = x2;
						
						if (num30 > num27)
							num30 = num27;
						
						if (num31 < y3)
							num31 = y3;
						
						if (num32 > num28)
							num32 = num28;
						
						if (num29 > num33)
							num29 = num33;
						
						if (num30 < num33)
							num30 = num33;
						
						if (num31 > num34)
							num31 = num34;
						
						if (num32 < num34)
							num32 = num34;
						
						if (num35 >= num29 && num35 < num30 && num36 >= num31 && num36 <= num32) {

							undo.Add(new ChangedTile(num35, num36));

							Main.tile[num35, num36].active(active: false);
							Main.tile[num35, num36].wall = WallID.LihzahrdBrickUnsafe;

						}

					}

				}

				for (int num37 = num28; num37 > y3; num37--) {

					for (int num38 = num27; num38 > x2; num38--) {

						if (genRand.Next(20) == 0)
							num31 += genRand.Next(-1, 2);
						
						if (genRand.Next(20) == 0)
							num32 += genRand.Next(-1, 2);
						
						if (genRand.Next(20) == 0)
							num29 += genRand.Next(-1, 2);
						
						if (genRand.Next(20) == 0)
							num30 += genRand.Next(-1, 2);
						
						if (num29 < x2)
							num29 = x2;
						
						if (num30 > num27)
							num30 = num27;
						
						if (num31 < y3)
							num31 = y3;
						
						if (num32 > num28)
							num32 = num28;
						
						if (num29 > num33)
							num29 = num33;
						
						if (num30 < num33)
							num30 = num33;
						
						if (num31 > num34)
							num31 = num34;
						
						if (num32 < num34)
							num32 = num34;
						
						if (num38 >= num29 && num38 < num30 && num37 >= num31 && num37 <= num32) {

							undo.Add(new ChangedTile(num38, num37));

							Main.tile[num38, num37].active(active: false);
							Main.tile[num38, num37].wall = WallID.LihzahrdBrickUnsafe;

						}

					}

				}

			}

			Vector2 templePath = new Vector2(num5, num6);
			
			for (int num39 = 0; num39 < num2; num39++) {

				Rectangle rectangle2 = array[num39];
				rectangle2.X += 8;
				rectangle2.Y += 8;
				rectangle2.Width -= 16;
				rectangle2.Height -= 16;

				bool flag2 = true;

				while (flag2) {

					int num40 = genRand.Next(rectangle2.X, rectangle2.X + rectangle2.Width);
					int num41 = genRand.Next(rectangle2.Y, rectangle2.Y + rectangle2.Height);

					if (num39 == num2 - 1) {

						num40 = rectangle2.X + rectangle2.Width / 2 + genRand.Next(-10, 10);
						num41 = rectangle2.Y + rectangle2.Height / 2 + genRand.Next(-10, 10);

					}

					templePath = templePather(templePath, num40, num41, ref undo);
					
					if (templePath.X == num40 && templePath.Y == num41)
						flag2 = false;
					
				}

				if (num39 >= num2 - 1)
					continue;
				
				if (genRand.Next(3) != 0) {

					int num42 = num39 + 1;

					if (array[num42].Y >= array[num39].Y + array[num39].Height) {

						rectangle2.X = array[num42].X;

						if (num39 == 0) {

							if (num3 > 0) {

								rectangle2.X += (int)(array[num42].Width * 0.8);

							}
							else {

								rectangle2.X += (int)(array[num42].Width * 0.2);

							}

						}
						else if (array[num42].X < array[num39].X) {

							rectangle2.X += (int)(array[num42].Width * 0.2);

						}
						else {

							rectangle2.X += (int)(array[num42].Width * 0.8);

						}

						rectangle2.Y = array[num42].Y;

					}
					else {

						rectangle2.X = (array[num39].X + array[num39].Width / 2 + (array[num42].X + array[num42].Width / 2)) / 2;
						rectangle2.Y = (int)(array[num42].Y + array[num42].Height * 0.8);

					}

					int x3 = rectangle2.X;
					int y4 = rectangle2.Y;

					flag2 = true;
					
					while (flag2) {

						int num43 = genRand.Next(x3 - 6, x3 + 7);
						int num44 = genRand.Next(y4 - 6, y4 + 7);

						templePath = templePather(templePath, num43, num44, ref undo);

						if (templePath.X == num43 && templePath.Y == num44)
							flag2 = false;
						
					}

					continue;

				}

				int num45 = num39 + 1;
				int num46 = (array[num39].X + array[num39].Width / 2 + (array[num45].X + array[num45].Width / 2)) / 2;
				int num47 = (array[num39].Y + array[num39].Height / 2 + (array[num45].Y + array[num45].Height / 2)) / 2;
				
				flag2 = true;
				
				while (flag2) {

					int num48 = genRand.Next(num46 - 6, num46 + 7);
					int num49 = genRand.Next(num47 - 6, num47 + 7);
					
					templePath = templePather(templePath, num48, num49, ref undo);
					
					if (templePath.X == num48 && templePath.Y == num49)
						flag2 = false;
					
				}

			}

			int num50 = Main.maxTilesX - 20;
			int num51 = 20;
			int num52 = Main.maxTilesY - 20;
			int num53 = 20;

			for (int num54 = 0; num54 < num2; num54++) {

				if (array[num54].X < num50)
					num50 = array[num54].X;
				
				if (array[num54].X + array[num54].Width > num51)
					num51 = array[num54].X + array[num54].Width;
				
				if (array[num54].Y < num52)
					num52 = array[num54].Y;
				
				if (array[num54].Y + array[num54].Height > num53)
					num53 = array[num54].Y + array[num54].Height;
				
			}

			num50 -= 10;
			num51 += 10;
			num52 -= 10;
			num53 += 10;

			for (int num55 = num50; num55 < num51; num55++)
				for (int num56 = num52; num56 < num53; num56++)
					outerTempled(num55, num56, ref undo);

			for (int num57 = num51; num57 >= num50; num57--)
				for (int num58 = num52; num58 < num53 / 2; num58++)
					outerTempled(num57, num58, ref undo);
				
			for (int num59 = num52; num59 < num53; num59++)
				for (int num60 = num50; num60 < num51; num60++)
					outerTempled(num60, num59, ref undo);
				
			for (int num61 = num53; num61 >= num52; num61--)
				for (int num62 = num50; num62 < num51; num62++)
					outerTempled(num62, num61, ref undo);
				

			num3 = -num4;

			Vector2 vector = new Vector2(num5, num6);

			int num63 = genRand.Next(2, 5);
			bool flag3 = true;
			int num64 = 0;
			int num65 = genRand.Next(9, 14);

			while (flag3) {

				num64++;

				if (num64 >= num65) {
					num64 = 0;
					vector.Y -= 1f;
				}

				vector.X += num3;
				int num66 = (int)vector.X;
				flag3 = false;
				
				for (int num67 = (int)vector.Y - num63; num67 < vector.Y + num63; num67++) {
					
					if (Main.tile[num66, num67].wall == WallID.LihzahrdBrickUnsafe || (Main.tile[num66, num67].active() && Main.tile[num66, num67].type == TileID.LihzahrdBrick))
						flag3 = true;
					
					if (Main.tile[num66, num67].active() && Main.tile[num66, num67].type == TileID.LihzahrdBrick) {

						undo.Add(new ChangedTile(num66, num67));

						Main.tile[num66, num67].active(active: false);
						Main.tile[num66, num67].wall = WallID.LihzahrdBrickUnsafe;

					}

				}

			}

			int num68 = num5;
			int num69;

			for (num69 = num6; !Main.tile[num68, num69].active(); num69++) { }

			num69 -= 4;
			int num70 = num69;

			while ((Main.tile[num68, num70].active() && Main.tile[num68, num70].type == TileID.LihzahrdBrick) || Main.tile[num68, num70].wall == WallID.LihzahrdBrickUnsafe)
				num70--;

			num70 += 2;

			for (int num71 = num68 - 1; num71 <= num68 + 1; num71++) {

				for (int num72 = num70; num72 <= num69; num72++) {

					undo.Add(new ChangedTile(num71, num72));

					Main.tile[num71, num72].active(active: true);
					Main.tile[num71, num72].type = TileID.LihzahrdBrick;
					Main.tile[num71, num72].liquid = 0;
					Main.tile[num71, num72].slope(0);
					Main.tile[num71, num72].halfBrick(halfBrick: false);

				}

			}

			for (int num73 = num68 - 4; num73 <= num68 + 4; num73++) {

				for (int num74 = num69 - 1; num74 < num69 + 3; num74++) {

					undo.Add(new ChangedTile(num73, num74));

					Main.tile[num73, num74].active(active: false);
					Main.tile[num73, num74].wall = WallID.LihzahrdBrickUnsafe;

				}

			}

			for (int num75 = num68 - 1; num75 <= num68 + 1; num75++) {

				for (int num76 = num69 - 5; num76 <= num69 + 8; num76++) {

					undo.Add(new ChangedTile(num75, num76));

					Main.tile[num75, num76].active(active: true);
					Main.tile[num75, num76].type = TileID.LihzahrdBrick;
					Main.tile[num75, num76].liquid = 0;
					Main.tile[num75, num76].slope(0);
					Main.tile[num75, num76].halfBrick(halfBrick: false);

				}

			}

			for (int num77 = num68 - 1; num77 <= num68 + 1; num77++) {

				for (int num78 = num69; num78 < num69 + 3; num78++) {

					undo.Add(new ChangedTile(num77, num78));

					Main.tile[num77, num78].active(active: false);
					Main.tile[num77, num78].wall = WallID.LihzahrdBrickUnsafe;

				}

			}

			PlaceTile(num68, num69, TileID.ClosedDoor, ref undo, mute: true, forced: false, -1, 11);

			for (int num79 = num50; num79 < num51; num79++)
				for (int num80 = num52; num80 < num53; num80++)
					templeCleaner(num79, num80, ref undo);

			for (int num81 = num53; num81 >= num52; num81--)
				for (int num82 = num51; num82 >= num50; num82--)
					templeCleaner(num82, num81, ref undo);

			for (int num83 = num50; num83 < num51; num83++) {

				for (int num84 = num52; num84 < num53; num84++) {

					bool flag4 = true;
					
					for (int num85 = num83 - 1; num85 <= num83 + 1; num85++) {

						for (int num86 = num84 - 1; num86 <= num84 + 1; num86++) {

							if ((!Main.tile[num85, num86].active() || Main.tile[num85, num86].type != TileID.LihzahrdBrick) && Main.tile[num85, num86].wall != WallID.LihzahrdBrickUnsafe) {
								flag4 = false;
								break;
							}

						}

					}

					if (flag4) {

						undo.Add(new ChangedTile(num83, num84));

						Main.tile[num83, num84].wall = WallID.LihzahrdBrickUnsafe;

					}

				}

			}

			int num87 = 0;

			Rectangle rectangle3 = array[num2 - 1];

			int num88 = rectangle3.Width / 2;
			int num89 = rectangle3.Height / 2;

			while (true) {

				num87++;

				int num90 = rectangle3.X + num88 + 15 - genRand.Next(30);
				int num91 = rectangle3.Y + num89 + 15 - genRand.Next(30);

				PlaceTile(num90, num91, TileID.LihzahrdAltar, ref undo);

				if (Main.tile[num90, num91].type == TileID.LihzahrdAltar) {

					lAltarX = num90 - Main.tile[num90, num91].frameX / 18;
					lAltarY = num91 - Main.tile[num90, num91].frameY / 18;

					break;

				}

				if (num87 < 1000)
					continue;
				
				num90 = rectangle3.X + num88;
				num91 = rectangle3.Y + num89;
				num90 += genRand.Next(-10, 11);

				for (num91 += genRand.Next(-10, 11); !Main.tile[num90, num91].active(); num91++) { }

				undo.Add(new ChangedTile(num90 - 1, num91));
				undo.Add(new ChangedTile(num90, num91));
				undo.Add(new ChangedTile(num90 + 1, num91));

				Main.tile[num90 - 1, num91].active(active: true);
				Main.tile[num90 - 1, num91].slope(0);
				Main.tile[num90 - 1, num91].halfBrick(halfBrick: false);
				Main.tile[num90 - 1, num91].type = TileID.LihzahrdBrick;

				Main.tile[num90, num91].active(active: true);
				Main.tile[num90, num91].slope(0);
				Main.tile[num90, num91].halfBrick(halfBrick: false);
				Main.tile[num90, num91].type = TileID.LihzahrdBrick;

				Main.tile[num90 + 1, num91].active(active: true);
				Main.tile[num90 + 1, num91].slope(0);
				Main.tile[num90 + 1, num91].halfBrick(halfBrick: false);
				Main.tile[num90 + 1, num91].type = TileID.LihzahrdBrick;

				num91 -= 2;
				num90--;
				
				for (int num92 = -1; num92 <= 3; num92++) {

					for (int num93 = -1; num93 <= 1; num93++) {

						x = num90 + num92;
						y = num91 + num93;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].active(active: false);

					}

				}

				lAltarX = num90;
				lAltarY = num91;

				for (int num94 = 0; num94 <= 2; num94++) {

					for (int num95 = 0; num95 <= 1; num95++) {

						x = num90 + num94;
						y = num91 + num95;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].active(active: true);
						Main.tile[x, y].type = TileID.LihzahrdAltar;
						Main.tile[x, y].frameX = (short)(num94 * 18);
						Main.tile[x, y].frameY = (short)(num95 * 18);

					}

				}

				for (int num96 = 0; num96 <= 2; num96++) {

					for (int num97 = 0; num97 <= 1; num97++) {

						x = num90 + num96;
						y = num91 + num97;

						WorldGen.SquareTileFrame(x, y);

					}

				}

				break;

			}

			float num98 = (float)num2 * 1.1f;
			num98 *= 1f + (float)genRand.Next(-25, 26) * 0.01f;

			if (DrunkWorldGen)
				num98 *= 1.5f;
			
			int num99 = 0;

			while (num98 > 0f) {

				num99++;
				
				int num100 = genRand.Next(num2);
				int num101 = genRand.Next(array[num100].X, array[num100].X + array[num100].Width);
				int num102 = genRand.Next(array[num100].Y, array[num100].Y + array[num100].Height);

				if (Main.tile[num101, num102].wall == WallID.LihzahrdBrickUnsafe && !Main.tile[num101, num102].active()) {

					bool flag5 = false;

					if (genRand.Next(2) == 0) {

						int num103 = 1;

						if (genRand.Next(2) == 0)
							num103 = -1;
						
						for (; !Main.tile[num101, num102].active(); num102 += num103) { }

						num102 -= num103;
						
						int num104 = genRand.Next(2);
						int num105 = genRand.Next(3, 10);
						bool flag6 = true;
						
						for (int num106 = num101 - num105; num106 < num101 + num105; num106++) {

							for (int num107 = num102 - num105; num107 < num102 + num105; num107++) {

								if (Main.tile[num106, num107].active() && (Main.tile[num106, num107].type == TileID.ClosedDoor || Main.tile[num106, num107].type == TileID.LihzahrdAltar)) {

									flag6 = false;
									break;

								}

							}

						}

						if (flag6) {

							for (int num108 = num101 - num105; num108 < num101 + num105; num108++) {

								for (int num109 = num102 - num105; num109 < num102 + num105; num109++) {

									if (!WorldGen.SolidTile(num108, num109) || Main.tile[num108, num109].type == TileID.WoodenSpikes || WorldGen.SolidTile(num108, num109 - num103))
										continue;

									undo.Add(new ChangedTile(num108, num109));

									Main.tile[num108, num109].type = TileID.WoodenSpikes;

									flag5 = true;

									if (num104 == 0) {

										undo.Add(new ChangedTile(num108, num109 - 1));

										Main.tile[num108, num109 - 1].type = TileID.WoodenSpikes;
										Main.tile[num108, num109 - 1].active(active: true);

										if (DrunkWorldGen) {

											undo.Add(new ChangedTile(num108, num109 - 2));

											Main.tile[num108, num109 - 2].type = TileID.WoodenSpikes;
											Main.tile[num108, num109 - 2].active(active: true);

										}

									}
									else {

										undo.Add(new ChangedTile(num108, num109 + 1));

										Main.tile[num108, num109 + 1].type = TileID.WoodenSpikes;
										Main.tile[num108, num109 + 1].active(active: true);

										if (DrunkWorldGen) {

											undo.Add(new ChangedTile(num108, num109 + 2));

											Main.tile[num108, num109 + 2].type = TileID.WoodenSpikes;
											Main.tile[num108, num109 + 2].active(active: true);

										}

									}

									num104++;

									if (num104 > 1)
										num104 = 0;
									
								}

							}

						}

						if (flag5) {

							num99 = 0;
							num98 -= 1f;

						}

					}
					else {

						int num110 = 1;
						
						if (genRand.Next(2) == 0)
							num110 = -1;
						
						for (; !Main.tile[num101, num102].active(); num101 += num110) { }

						num101 -= num110;
						
						int num111 = genRand.Next(2);
						int num112 = genRand.Next(3, 10);
						bool flag7 = true;
						
						for (int num113 = num101 - num112; num113 < num101 + num112; num113++) {

							for (int num114 = num102 - num112; num114 < num102 + num112; num114++) {

								if (Main.tile[num113, num114].active() && Main.tile[num113, num114].type == TileID.ClosedDoor) {

									flag7 = false;
									break;

								}

							}

						}

						if (flag7) {

							for (int num115 = num101 - num112; num115 < num101 + num112; num115++) {

								for (int num116 = num102 - num112; num116 < num102 + num112; num116++) {

									if (!WorldGen.SolidTile(num115, num116) || Main.tile[num115, num116].type == TileID.WoodenSpikes || WorldGen.SolidTile(num115 - num110, num116))
										continue;

									undo.Add(new ChangedTile(num115, num116));

									Main.tile[num115, num116].type = TileID.WoodenSpikes;

									flag5 = true;

									if (num111 == 0) {

										undo.Add(new ChangedTile(num115 - 1, num116));

										Main.tile[num115 - 1, num116].type = TileID.WoodenSpikes;
										Main.tile[num115 - 1, num116].active(active: true);

										if (DrunkWorldGen) {

											undo.Add(new ChangedTile(num115 - 2, num116));

											Main.tile[num115 - 2, num116].type = TileID.WoodenSpikes;
											Main.tile[num115 - 2, num116].active(active: true);

										}

									}
									else {

										undo.Add(new ChangedTile(num115 + 1, num116));

										Main.tile[num115 + 1, num116].type = TileID.WoodenSpikes;
										Main.tile[num115 + 1, num116].active(active: true);
										
										if (DrunkWorldGen) {

											undo.Add(new ChangedTile(num115 - 2, num116));

											Main.tile[num115 - 2, num116].type = TileID.WoodenSpikes;
											Main.tile[num115 - 2, num116].active(active: true);

										}

									}

									num111++;

									if (num111 > 1)
										num111 = 0;
									
								}

							}

						}

						if (flag5) {

							num99 = 0;
							num98 -= 1f;

						}

					}

				}

				if (num99 > 1000) {

					num99 = 0;
					num98 -= 1f;

				}

			}

			tLeft   = num50;
			tRight  = num51;
			tTop    = num52;
			tBottom = num53;
			tRooms  = num2;

		}

	}

}
