using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 7/26/2020 Modified

		public static bool placeTrap(int x, int y, ref UndoStep undo, int type = -1) {

			UnifiedRandom genRand = WorldGen.genRand;

			if (!WorldGen.InWorld(x, y))
				return false;

			int  curY         = y;
			bool inLava       = false;
			bool inUnderworld = false;

			while (!WorldGen.SolidTile(x, curY)) {

				curY++;

				if (curY > Main.maxTilesY - 10)
					return false;

				if (curY >= Main.maxTilesY - 300)
					inUnderworld = true;

			}

			curY--;

			if (Main.tile[x, curY].wall == WallID.LihzahrdBrickUnsafe)
				return false;

			if (Main.tile[x, curY].liquid > 0 && Main.tile[x, curY].lava())
				inLava = true;

			if (type == -1 && genRand.Next(20) == 0) {
				type = 2;
			} else if (type == -1 && curY > WorldGen.lavaLine + 30 && genRand.Next(6) != 0) {
				type = 3;
			} else if (type == -1) {
				type = genRand.Next(2);
			}

			if (!WorldGen.InWorld(x, curY, 3))
				return false;

			if (inLava && type != 3)
				return false;

			if (inUnderworld && type != 3)
				return false;

			if (Main.tile[x, curY].nactive() || Main.tile[x - 1, curY].nactive() || Main.tile[x + 1, curY].nactive() || Main.tile[x, curY - 1].nactive() || Main.tile[x - 1, curY - 1].nactive() || Main.tile[x + 1, curY - 1].nactive() || Main.tile[x, curY - 2].nactive() || Main.tile[x - 1, curY - 2].nactive() || Main.tile[x + 1, curY - 2].nactive())
				return false;

			if (Main.tile[x, curY + 1].type == TileID.Spikes)
				return false;

			if (Main.tile[x, curY + 1].type == TileID.WoodenSpikes)
				return false;
			
			if (type == 1)
				for (int i = x - 3; i <= x + 3; i++)
					for (int j = curY - 3; j <= curY + 3; j++)
						if (Main.tile[i, j].type == TileID.SnowBlock || Main.tile[i, j].type == TileID.IceBlock)
							type = 0;

			switch (type) {

				case 0: { // Dart Trap

						int num15 = x;
						int num16 = curY;
						num16 -= genRand.Next(3);

						while (!WorldGen.SolidTile(num15, num16) /* && !Main.tileCracked[Main.tile[num15, num16].type]*/) {
							num15--;
							if (num15 < 0)
								return false;
						}

						int num17 = num15;
						num15 = x;

						while (!WorldGen.SolidTile(num15, num16) /* && !Main.tileCracked[Main.tile[num15, num16].type]*/) {
							num15++;
							if (num15 > Main.maxTilesX)
								return false;
						}

						int num18 = num15;
						int num19 = x - num17;
						int num20 = num18 - x;

						bool flag4 = false;
						bool flag5 = false;

						if (num19 > 5 && num19 < 50)
							flag4 = true;

						if (num20 > 5 && num20 < 50)
							flag5 = true;

						if (flag4 && !WorldGen.SolidTile(num17, num16 + 1))
							flag4 = false;

						if (flag5 && !WorldGen.SolidTile(num18, num16 + 1))
							flag5 = false;

						if (flag4 && (Main.tile[num17, num16].type == TileID.ClosedDoor || Main.tile[num17, num16].type == TileID.Spikes || Main.tile[num17, num16 + 1].type == TileID.ClosedDoor || Main.tile[num17, num16 + 1].type == TileID.Spikes))
							flag4 = false;

						if (flag5 && (Main.tile[num18, num16].type == TileID.ClosedDoor || Main.tile[num18, num16].type == TileID.Spikes || Main.tile[num18, num16 + 1].type == TileID.ClosedDoor || Main.tile[num18, num16 + 1].type == TileID.Spikes))
							flag5 = false;

						int num21;

						if (flag4 && flag5) {

							num21 = 1;
							num15 = num17;

							if (genRand.Next(2) == 0) {

								num15 = num18;
								num21 = -1;

							}

						} else if (flag5) {

							num15 = num18;
							num21 = -1;

						} else {

							if (!flag4) {
								trapDiag[type, 0]++;
								return false;
							}

							num15 = num17;
							num21 = 1;

						}

						if (Main.tile[num15, num16].type == TileID.MushroomBlock) {

							trapDiag[type, 0]++;
							return false;

						}

						if (Main.tile[x, curY].wall > 0) {
							PlaceTile(x, curY, TileID.PressurePlates, ref undo, mute: true, forced: true, -1, 2);
						} else {
							PlaceTile(x, curY, TileID.PressurePlates, ref undo, mute: true, forced: true, -1, genRand.Next(2, 4));
						}

						WorldGen.KillTile(num15, num16);
						PlaceTile(num15, num16, TileID.Traps, ref undo, mute: true, forced: true);

						if (num21 == 1)
							Main.tile[num15, num16].frameX += 18;

						int num22 = x;
						int num23 = curY;

						while (num22 != num15 || num23 != num16) {

							undo.Add(new ChangedTile(num22, num23));

							Main.tile[num22, num23].wire(wire: true);

							if (num22 > num15)
								num22--;

							if (num22 < num15)
								num22++;

							undo.Add(new ChangedTile(num22, num23));

							Main.tile[num22, num23].wire(wire: true);

							if (num23 > num16)
								num23--;

							if (num23 < num16)
								num23++;

							undo.Add(new ChangedTile(num22, num23));

							Main.tile[num22, num23].wire(wire: true);

						}

						trapDiag[type, 1]++;

						return true;

					}
				case 1: { // Boulder Trap

						int num32 = x;
						int num33 = curY - 8;
						num32 += genRand.Next(-1, 2);

						bool flag6 = true;

						while (flag6) {

							bool flag7 = true;
							int num34 = 0;

							for (int num35 = num32 - 2; num35 <= num32 + 3; num35++) {

								for (int num36 = num33; num36 <= num33 + 3; num36++) {

									if (!WorldGen.SolidTile(num35, num36))
										flag7 = false;

									if (Main.tile[num35, num36].active() && (Main.tile[num35, num36].type == TileID.Dirt || Main.tile[num35, num36].type == TileID.Stone || Main.tile[num35, num36].type == TileID.Mud))
										num34++;

								}

							}

							num33--;

							if (num33 < Main.worldSurface) {

								trapDiag[type, 0]++;
								return false;

							}

							if (flag7 && num34 > 2)
								flag6 = false;

						}

						if (curY - num33 <= 5 || curY - num33 >= 40) {

							trapDiag[type, 0]++;
							return false;

						}

						for (int num37 = num32; num37 <= num32 + 1; num37++) {

							for (int num38 = num33; num38 <= curY; num38++) {

								WorldGen.KillTile(num37, num38);

							}

						}

						for (int num39 = num32 - 2; num39 <= num32 + 3; num39++)
							for (int num40 = num33 - 2; num40 <= num33 + 3; num40++)
								if (WorldGen.SolidTile(num39, num40))
									Neo.SetTile(num39, num40, TileID.Stone, ref undo);

						PlaceTile(x,         curY,      TileID.PressurePlates,   ref undo, mute: true, forced: true, -1, WorldGen.genRand.Next(2, 4));
						PlaceTile(num32,     num33 + 2, TileID.ActiveStoneBlock, ref undo, mute: true);
						PlaceTile(num32 + 1, num33 + 2, TileID.ActiveStoneBlock, ref undo, mute: true);
						PlaceTile(num32 + 1, num33 + 1, TileID.Boulder,          ref undo, mute: true);

						num33 += 2;

						undo.Add(new ChangedTile(num32,     num33));
						undo.Add(new ChangedTile(num32 + 1, num33));

						Main.tile[num32,     num33].wire(wire: true);
						Main.tile[num32 + 1, num33].wire(wire: true);

						num33++;

						PlaceTile(num32,     num33, TileID.ActiveStoneBlock, ref undo, mute: true);
						PlaceTile(num32 + 1, num33, TileID.ActiveStoneBlock, ref undo, mute: true);

						undo.Add(new ChangedTile(num32, num33));
						undo.Add(new ChangedTile(num32 + 1, num33));

						Main.tile[num32,     num33].wire(wire: true);
						Main.tile[num32 + 1, num33].wire(wire: true);

						PlaceTile(num32,     num33 + 1, TileID.ActiveStoneBlock, ref undo, mute: true);
						PlaceTile(num32 + 1, num33 + 1, TileID.ActiveStoneBlock, ref undo, mute: true);

						undo.Add(new ChangedTile(num32, num33 + 1));
						undo.Add(new ChangedTile(num32 + 1, num33 + 1));

						Main.tile[num32,     num33 + 1].wire(wire: true);
						Main.tile[num32 + 1, num33 + 1].wire(wire: true);

						int num41 = x;
						int num42 = curY;

						while (num41 != num32 || num42 != num33) {

							undo.Add(new ChangedTile(num41, num42));

							Main.tile[num41, num42].wire(wire: true);

							if (num41 > num32)
								num41--;

							if (num41 < num32)
								num41++;

							undo.Add(new ChangedTile(num41, num42));

							Main.tile[num41, num42].wire(wire: true);

							if (num42 > num33)
								num42--;

							if (num42 < num33)
								num42++;

							undo.Add(new ChangedTile(num41, num42));

							Main.tile[num41, num42].wire(wire: true);

						}

						trapDiag[type, 1]++;

						return true;

					}
				case 2: { // Explosion

						int num24 = genRand.Next(4, 7);
						int num25 = x;

						num25 += genRand.Next(-1, 2);

						int num26 = curY;

						for (int num27 = 0; num27 < num24; num27++) {

							num26++;

							if (!WorldGen.SolidTile(num25, num26)) {

								trapDiag[type, 0]++;
								return false;

							}

						}

						for (int num28 = num25 - 2; num28 <= num25 + 2; num28++)
							for (int num29 = num26 - 2; num29 <= num26 + 2; num29++)
								if (!WorldGen.SolidTile(num28, num29))
									return false;


						WorldGen.KillTile(num25, num26);

						Neo.SetTile(num25, num26, TileID.Explosives, ref undo);
						Main.tile[num25, num26].frameX = 0;
						Main.tile[num25, num26].frameY = (short)(18 * genRand.Next(2));

						PlaceTile(x, curY, TileID.PressurePlates, ref undo, mute: true, forced: true, -1, genRand.Next(2, 4));

						int num30 = x;
						int num31 = curY;

						while (num30 != num25 || num31 != num26) {

							undo.Add(new ChangedTile(num30, num31));

							Main.tile[num30, num31].wire(wire: true);

							if (num30 > num25)
								num30--;

							if (num30 < num25)
								num30++;

							undo.Add(new ChangedTile(num30, num31));

							Main.tile[num30, num31].wire(wire: true);

							if (num31 > num26)
								num31--;

							if (num31 < num26)
								num31++;

							undo.Add(new ChangedTile(num30, num31));

							Main.tile[num30, num31].wire(wire: true);

						}

						trapDiag[type, 1]++;

						break;

					}
				case 3: { // Geyser

						int num2 = 0;
						int num3 = 0;

						for (int k = 0; k < 4; k++) {

							if (num2 < 2 && genRand.Next(5) == 0) {

								num2++;
								continue;

							}

							int num4 = x;
							int num5 = curY;
							bool flag3 = false;
							num4 = ((num3 != 0) ? (num4 + genRand.Next(-15, 16)) : (num4 + genRand.Next(-1, 2)));
							int num6 = genRand.Next(3, 6 + (num3 > 0).ToInt() * 3);

							for (int l = 0; l < num6; l++) {

								num5++;

								if (!WorldGen.SolidTile(num4, num5)) {

									trapDiag[type, 0]++;
									flag3 = true;
									break;

								}

							}

							if (flag3)
								continue;

							int num7 = 2;

							for (int m = num4 - num7; m <= num4 + num7; m++) {

								for (int n = num5 - num7; n <= num5 + num7; n++) {

									if (!WorldGen.SolidTile(m, n)) {

										trapDiag[type, 0]++;
										flag3 = true;
										break;

									}

								}

								if (flag3)
									break;

							}

							if (flag3)
								continue;

							num7 = 10;

							for (int num8 = num4; num8 <= num4 + 1; num8++) {

								int num9 = num5;

								while (num9 > num5 - 20 && WorldGen.SolidTile(num8, num9))
									num9--;

								for (int num10 = num9 - num7; num10 <= num9; num10++) {

									if (WorldGen.SolidTile(num8, num10)) {

										trapDiag[type, 0]++;
										flag3 = true;
										break;

									}

								}

								if (flag3)
									break;

							}

							if (flag3)
								continue;

							WorldGen.KillTile(num4,     num5);
							WorldGen.KillTile(num4 + 1, num5);

							int num11 = genRand.Next(2);

							for (int num12 = 0; num12 < 2; num12++) {

								undo.Add(new ChangedTile(num4 + num12, num5));

								Main.tile[num4 + num12, num5].active(active: true);
								Main.tile[num4 + num12, num5].type = TileID.GeyserTrap;
								Main.tile[num4 + num12, num5].frameX = (short)(18 * num12 + 36 * num11);
								Main.tile[num4 + num12, num5].frameY = 0;

							}

							PlaceTile(x, curY, TileID.PressurePlates, ref undo, mute: true, forced: true, -1, WorldGen.genRand.Next(2, 4));

							int num13 = x;
							int num14 = curY;

							while (num13 != num4 || num14 != num5) {

								undo.Add(new ChangedTile(num13, num14));

								Main.tile[num13, num14].wire(wire: true);

								if (num13 > num4)
									num13--;

								if (num13 < num4)
									num13++;

								undo.Add(new ChangedTile(num13, num14));

								Main.tile[num13, num14].wire(wire: true);

								if (num14 > num5)
									num14--;

								if (num14 < num5)
									num14++;

								undo.Add(new ChangedTile(num13, num14));

								Main.tile[num13, num14].wire(wire: true);

							}

							num3++;
							trapDiag[type, 1]++;

						}

						break;

					}

			}

			return false;

		}

	}

}
