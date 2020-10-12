using System;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/30/2020 Copy/Paste

        public static bool mayanTrap(int x2, int y2, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num = 1;

			if (genRand.Next(3) == 0)
				num = 0;
			
			int num2 = y2;

			while (!WorldGen.SolidOrSlopedTile(x2, num2)) {

				num2++;

				if (num2 >= Main.maxTilesY - 300)
					return false;

			}

			if (Main.tile[x2, num2].type == TileID.WoodenSpikes || Main.tile[x2, num2].type == TileID.ClosedDoor)
				return false;

			num2--;
			
			if (Main.tile[x2, num2].liquid > 0 && Main.tile[x2, num2].lava()) 
				return false;

			if (num == -1 && genRand.Next(20) == 0) {
				num = 2;
			}
			else if (num == -1) {
				num = genRand.Next(2);
			}

			if (Main.tile[x2, num2].nactive() || Main.tile[x2 - 1, num2].nactive() || Main.tile[x2 + 1, num2].nactive() || Main.tile[x2, num2 - 1].nactive() || Main.tile[x2 - 1, num2 - 1].nactive() || Main.tile[x2 + 1, num2 - 1].nactive() || Main.tile[x2, num2 - 2].nactive() || Main.tile[x2 - 1, num2 - 2].nactive() || Main.tile[x2 + 1, num2 - 2].nactive())
				return false;

			if (Main.tile[x2, num2 + 1].type == TileID.ClosedDoor)
				return false;

			if (Main.tile[x2, num2 + 1].type == TileID.Spikes)
				return false;

			if (Main.tile[x2, num2 + 1].type == TileID.WoodenSpikes)
				return false;

			switch (num) {

				case 0: {

						int num12 = x2;
						int num13 = num2;

						num13 -= genRand.Next(3);

						while (!WorldGen.SolidOrSlopedTile(num12, num13))
							num12--;
						
						int num14 = num12;

						for (num12 = x2; !WorldGen.SolidOrSlopedTile(num12, num13); num12++) { }

						int num15 = num12;
						int num16 = x2 - num14;
						int num17 = num15 - x2;

						bool flag = false;
						bool flag2 = false;

						if (num16 > 5 && num16 < 50)
							flag = true;
						
						if (num17 > 5 && num17 < 50)
							flag2 = true;
						
						if (flag && !WorldGen.SolidOrSlopedTile(num14, num13 + 1))
							flag = false;
						
						if (flag2 && !WorldGen.SolidOrSlopedTile(num15, num13 + 1))
							flag2 = false;
						
						if (flag && (Main.tile[num14, num13].type == TileID.ClosedDoor || Main.tile[num14, num13].type == TileID.Spikes || Main.tile[num14, num13 + 1].type == TileID.ClosedDoor || Main.tile[num14, num13 + 1].type == TileID.Spikes))
							flag = false;
						
						if (flag2 && (Main.tile[num15, num13].type == TileID.ClosedDoor || Main.tile[num15, num13].type == TileID.Spikes || Main.tile[num15, num13 + 1].type == TileID.ClosedDoor || Main.tile[num15, num13 + 1].type == TileID.Spikes))
							flag2 = false;
						
						int num18 = 0;

						if (flag && flag2) {

							num18 = 1;
							num12 = num14;

							if (genRand.Next(2) == 0) {

								num12 = num15;
								num18 = -1;

							}

						}
						else if (flag2) {

							num12 = num15;
							num18 = -1;

						}
						else {

							if (!flag)
								return false;
							
							num12 = num14;
							num18 = 1;

						}

						if (Main.tile[num12, num13].wall != WallID.LihzahrdBrickUnsafe)
							return false;
						
						if (Main.tile[num12, num13].type == TileID.MushroomBlock)
							return false;
						
						if (Main.tile[num12, num13].type == TileID.PressurePlates)
							return false;
						
						if (Main.tile[num12, num13].type == TileID.Traps)
							return false;
						
						if (Main.tile[num12, num13].type == TileID.WoodenSpikes)
							return false;
						
						if (Main.tile[num12, num13].type == TileID.LihzahrdAltar)
							return false;
						
						if (Main.tile[num12, num13].type == TileID.ClosedDoor)
							return false;
						
						PlaceTile(x2, num2, TileID.PressurePlates, ref undo, mute: true, forced: true, -1, 6);

						WorldGen.KillTile(num12, num13);
						
						int num19 = genRand.Next(4);
						
						if (Main.tile[x2, num2].wire())
							num19 = 0;
						
						if (Main.tile[x2, num2].wire2())
							num19 = 1;
						
						if (Main.tile[x2, num2].wire3())
							num19 = 2;
						
						if (Main.tile[x2, num2].wire4())
							num19 = 3;
						
						int num20 = Math.Abs(num12 - x2);
						int style2 = 1;
						
						if (num20 < 10 && genRand.Next(3) != 0)
							style2 = 2;
						
						PlaceTile(num12, num13, TileID.Traps, ref undo, mute: true, forced: true, -1, style2);

						if (num18 == 1) {
							Main.tile[num12, num13].frameX += 18;
						}

						int num21 = genRand.Next(5);
						int num22 = num13;

						while (num21 > 0) {

							num21--;
							num22--;
							
							if (!WorldGen.SolidTile(num12, num22) || !WorldGen.SolidTile(num12 - num18, num22) || WorldGen.SolidOrSlopedTile(num12 + num18, num22))
								break;
							
							PlaceTile(num12, num22, TileID.Traps, ref undo, mute: true, forced: true, -1, style2);

							if (num18 == 1) {
								Main.tile[num12, num22].frameX += 18;
							}

							switch (num19) {

								case 0:
									Main.tile[num12, num22].wire(wire: true);
									break;
								case 1:
									Main.tile[num12, num22].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num12, num22].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num12, num22].wire4(wire4: true);
									break;

							}

						}

						int num23 = x2;
						int num24 = num2;

						while (num23 != num12 || num24 != num13) {

							undo.Add(new ChangedTile(num23, num24));

							switch (num19) {

								case 0:
									Main.tile[num23, num24].wire(wire: true);
									break;
								case 1:
									Main.tile[num23, num24].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num23, num24].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num12, num22].wire4(wire4: true);
									break;

							}

							if (num23 > num12)
								num23--;
							
							if (num23 < num12)
								num23++;

							undo.Add(new ChangedTile(num23, num24));

							switch (num19) {

								case 0:
									Main.tile[num23, num24].wire(wire: true);
									break;
								case 1:
									Main.tile[num23, num24].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num23, num24].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num12, num22].wire4(wire4: true);
									break;

							}

							if (num24 > num13)
								num24--;
							
							if (num24 < num13)
								num24++;

							undo.Add(new ChangedTile(num23, num24));

							switch (num19) {

								case 0:
									Main.tile[num23, num24].wire(wire: true);
									break;
								case 1:
									Main.tile[num23, num24].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num23, num24].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num12, num22].wire4(wire4: true);
									break;

							}

						}

						return true;

					}
				case 1: {

						int num3 = x2;
						int num4 = num2;

						while (!WorldGen.SolidOrSlopedTile(num3, num4)) {

							num4--;

							if (num4 < Main.worldSurface)
								return false;
							
						}

						int num5 = Math.Abs(num4 - num2);
						
						if (num5 < 3)
							return false;
						
						int num6 = genRand.Next(4);

						if (Main.tile[x2, num2].wire())
							num6 = 0;
						
						if (Main.tile[x2, num2].wire2())
							num6 = 1;
						
						if (Main.tile[x2, num2].wire3())
							num6 = 2;

						if (Main.tile[x2, num2].wire4())
							num6 = 3;

						int style = 3;

						if (num5 < 16 && genRand.Next(3) != 0)
							style = 4;
						
						if (Main.tile[num3, num4].type == TileID.PressurePlates)
							return false;
						
						if (Main.tile[num3, num4].type == TileID.Traps)
							return false;
						
						if (Main.tile[num3, num4].type == TileID.WoodenSpikes)
							return false;
						
						if (Main.tile[num3, num4].type == TileID.LihzahrdAltar)
							return false;
						
						if (Main.tile[num3, num4].type == TileID.ClosedDoor)
							return false;
						
						if (Main.tile[num3, num4].wall != WallID.LihzahrdBrickUnsafe)
							return false;
						
						PlaceTile(x2, num2, TileID.PressurePlates, ref undo, mute: true, forced: true, -1, 6);
						PlaceTile(num3, num4, TileID.Traps, ref undo, mute: true, forced: true, -1, style);

						for (int i = 0; i < 2; i++) {

							int num7 = genRand.Next(1, 5);
							int num8 = num3;
							int num9 = -1;

							if (i == 1)
								num9 = 1;
							
							while (num7 > 0) {

								num7--;
								num8 += num9;

								if (!WorldGen.SolidTile(num8, num4 - 1) || WorldGen.SolidOrSlopedTile(num8, num4 + 1))
									break;
								
								PlaceTile(num8, num4, TileID.Traps, ref undo, mute: true, forced: true, -1, style);

								switch (num6) {

									case 0:
										Main.tile[num8, num4].wire(wire: true);
										break;
									case 1:
										Main.tile[num8, num4].wire2(wire2: true);
										break;
									case 2:
										Main.tile[num8, num4].wire3(wire3: true);
										break;
									case 3:
										Main.tile[num8, num4].wire4(wire4: true);
										break;

								}

							}

						}

						int num10 = x2;
						int num11 = num2;
						
						while (num10 != num3 || num11 != num4) {

							undo.Add(new ChangedTile(num10, num11));

							switch (num6) {

								case 0:
									Main.tile[num10, num11].wire(wire: true);
									break;
								case 1:
									Main.tile[num10, num11].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num10, num11].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num10, num11].wire4(wire4: true);
									break;

							}

							if (num10 > num3)
								num10--;
							
							if (num10 < num3)
								num10++;

							undo.Add(new ChangedTile(num10, num11));

							switch (num6) {

								case 0:
									Main.tile[num10, num11].wire(wire: true);
									break;
								case 1:
									Main.tile[num10, num11].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num10, num11].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num10, num11].wire4(wire4: true);
									break;

							}

							if (num11 > num4)
								num11--;
							
							if (num11 < num4)
								num11++;

							undo.Add(new ChangedTile(num10, num11));

							switch (num6) {

								case 0:
									Main.tile[num10, num11].wire(wire: true);
									break;
								case 1:
									Main.tile[num10, num11].wire2(wire2: true);
									break;
								case 2:
									Main.tile[num10, num11].wire3(wire3: true);
									break;
								case 3:
									Main.tile[num10, num11].wire4(wire4: true);
									break;

							}

						}

						return true;

					}
				default: {
						return false;
					}

			}

		}

    }

}
