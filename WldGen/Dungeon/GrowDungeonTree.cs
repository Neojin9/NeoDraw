using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/2/2020 Copy/Paste

		public static bool GrowDungeonTree(int i, int j, ref UndoStep undo, bool patch = false) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num = 0;

			int[] array  = new int[1000];
			int[] array2 = new int[1000];
			int[] array3 = new int[1000];
			int[] array4 = new int[1000];

			int num2 = 0;

			int[] array5 = new int[2000];
			int[] array6 = new int[2000];

			bool[] array7 = new bool[2000];

			int num3 = i - genRand.Next(2, 3);
			int num4 = i + genRand.Next(2, 3);

			if (genRand.Next(5) == 0) {

				if (genRand.Next(2) == 0) {
					num3--;
				}
				else {
					num4++;
				}

			}

			int num5 = num4 - num3;
			int num6 = num3;
			int num7 = num4;
			int minl = num3;
			int minr = num4;

			bool flag = true;

			int num8 = genRand.Next(-8, -4);
			int num9 = genRand.Next(2);
			int num10 = j;
			int num11 = genRand.Next(5, 15);

			Main.tileSolid[TileID.Spikes] = false;

			while (flag) {

				num8++;

				if (num8 > num11) {

					num11 = genRand.Next(5, 15);
					num8 = 0;
					array2[num] = num10 + genRand.Next(5);

					if (genRand.Next(5) == 0)
						num9 = ((num9 == 0) ? 1 : 0);
					
					if (num9 == 0) {

						array3[num] = -1;
						array[num] = num3;
						array4[num] = num4 - num3;

						if (genRand.Next(2) == 0)
							num3++;
						
						num6++;
						num9 = 1;

					}
					else {

						array3[num] = 1;
						array[num] = num4;
						array4[num] = num4 - num3;

						if (genRand.Next(2) == 0)
							num4--;
						
						num7--;
						num9 = 0;

					}

					if (num6 == num7)
						flag = false;
					
					num++;

				}

				for (int k = num3; k <= num4; k++) {

					Neo.SetTile(k, num10, 191, ref undo);
					Main.tile[k, num10].color(28);

				}

				num10--;

			}

			for (int l = 0; l < num - 1; l++) {

				int num12 = array[l] + array3[l];
				int num13 = array2[l];
				int num14 = (int)(array4[l] * (1f + genRand.Next(20, 30) * 0.1f));

				Neo.SetTile(num12, num13 + 1, 191, ref undo);
				Main.tile[num12, num13 + 1].color(28);

				int num15 = genRand.Next(3, 5);

				while (num14 > 0) {

					num14--;

					Neo.SetTile(num12, num13, 191, ref undo);
					Main.tile[num12, num13].color(28);

					if (genRand.Next(10) == 0) {
						num13 = ((genRand.Next(2) != 0) ? (num13 + 1) : (num13 - 1));
					}
					else {
						num12 += array3[l];
					}

					if (num15 > 0) {
						num15--;
					}
					else if (genRand.Next(2) == 0) {

						num15 = genRand.Next(2, 5);

						if (genRand.Next(2) == 0) {

							Neo.SetTile(num12, num13, 191, ref undo);
							Main.tile[num12, num13].color(28);

							Neo.SetTile(num12, num13 - 1, 191, ref undo);
							Main.tile[num12, num13 - 1].color(28);

							array5[num2] = num12;
							array6[num2] = num13;
							num2++;

						}
						else {

							Neo.SetTile(num12, num13, 191, ref undo);
							Main.tile[num12, num13].color(28);

							Neo.SetTile(num12, num13 + 1, 191, ref undo);
							Main.tile[num12, num13 + 1].color(28);

							array5[num2] = num12;
							array6[num2] = num13;
							num2++;

						}

					}

					if (num14 == 0) {

						array5[num2] = num12;
						array6[num2] = num13;
						num2++;

					}

				}

			}

			int num16 = (num3 + num4) / 2;
			int num17 = num10;
			int num18 = genRand.Next(num5 * 3, num5 * 5);
			int num19 = 0;
			int num20 = 0;

			while (num18 > 0) {

				Neo.SetTile(num16, num17, 191, ref undo);
				Main.tile[num16, num17].color(28);

				if (num19 > 0)
					num19--;
				
				if (num20 > 0)
					num20--;
				
				for (int m = -1; m < 2; m++) {

					if (m == 0 || ((m >= 0 || num19 != 0) && (m <= 0 || num20 != 0)) || genRand.Next(2) != 0)
						continue;
					
					int num21 = num16;
					int num22 = num17;
					int num23 = genRand.Next(num5, num5 * 3);

					if (m < 0)
						num19 = genRand.Next(3, 5);
					
					if (m > 0)
						num20 = genRand.Next(3, 5);
					
					int num24 = 0;

					while (num23 > 0) {

						num23--;
						num21 += m;

						Neo.SetTile(num21, num22, 191, ref undo);
						Main.tile[num21, num22].color(28);

						if (num23 == 0) {

							array5[num2] = num21;
							array6[num2] = num22;
							array7[num2] = true;
							num2++;

						}

						if (genRand.Next(5) == 0) {

							num22 = ((genRand.Next(2) != 0) ? (num22 + 1) : (num22 - 1));

							Neo.SetTile(num21, num22, 191, ref undo);
							Main.tile[num21, num22].color(28);

						}

						if (num24 > 0) {
							num24--;
						}
						else if (genRand.Next(3) == 0) {

							num24 = genRand.Next(2, 4);

							int num25 = num21;
							int num26 = num22;

							num26 = ((genRand.Next(2) != 0) ? (num26 + 1) : (num26 - 1));

							Neo.SetTile(num25, num26, 191, ref undo);
							Main.tile[num25, num26].color(28);

							array5[num2] = num25;
							array6[num2] = num26;
							array7[num2] = true;
							num2++;

							array5[num2] = num25 + genRand.Next(-5, 6);
							array6[num2] = num26 + genRand.Next(-5, 6);
							array7[num2] = true;
							num2++;

						}

					}

				}

				array5[num2] = num16;
				array6[num2] = num17;
				num2++;

				if (genRand.Next(4) == 0) {

					num16 = ((genRand.Next(2) != 0) ? (num16 + 1) : (num16 - 1));

					Neo.SetTile(num16, num17, 191, ref undo);
					Main.tile[num16, num17].color(28);

				}

				num17--;
				num18--;

			}

			for (int n = minl; n <= minr; n++) {

				int num27 = genRand.Next(1, 6);
				int num28 = j + 1;

				while (num27 > 0) {

					if (WorldGen.SolidTile(n, num28))
						num27--;

					Neo.SetTile(n, num28, 191, ref undo);
					num28++;

				}

				int num29 = num28;
				int num30 = genRand.Next(2, num5 + 1);

				for (int num31 = 0; num31 < num30; num31++) {

					num28 = num29;
					
					int num32 = (minl + minr) / 2;
					int num33 = 0;
					int num34 = 1;
					
					num33 = ((n >= num32) ? 1 : (-1));
					
					if (n == num32 || (num5 > 6 && (n == num32 - 1 || n == num32 + 1)))
						num33 = 0;
					
					int num35 = num33;
					int num36 = n;

					num27 = genRand.Next((int)(num5 * 3.5), num5 * 6);

					while (num27 > 0) {

						num27--;
						num36 += num33;
						
						if (Main.tile[num36, num28].wall != 244)
							Neo.SetTile(num36, num28, 191, ref undo);

						num28 += num34;

						if (Main.tile[num36, num28].wall != 244)
							Neo.SetTile(num36, num28, 191, ref undo);

						if (!Main.tile[num36, num28 + 1].active()) {
							num33 = 0;
							num34 = 1;
						}

						if (genRand.Next(3) == 0)
							num33 = ((num35 < 0) ? ((num33 == 0) ? (-1) : 0) : ((num35 <= 0) ? genRand.Next(-1, 2) : ((num33 == 0) ? 1 : 0)));
						
						if (genRand.Next(3) == 0)
							num34 = ((num34 == 0) ? 1 : 0);
						
					}

				}

			}

			for (int num37 = 0; num37 < num2; num37++) {

				int num38 = genRand.Next(5, 8);

				num38 = (int)(num38 * (1f + num5 * 0.05f));
				
				if (array7[num37])
					num38 = genRand.Next(6, 12) + num5;
				
				int num39 = array5[num37] - num38 * 2;
				int num40 = array5[num37] + num38 * 2;
				int num41 = array6[num37] - num38 * 2;
				int num42 = array6[num37] + num38 * 2;

				float num43 = 2f - genRand.Next(5) * 0.1f;

				for (int num44 = num39; num44 <= num40; num44++) {

					for (int num45 = num41; num45 <= num42; num45++) {

						if (Main.tile[num44, num45].type == 191)
							continue;
						
						if (array7[num37]) {

							if ((new Vector2(array5[num37], array6[num37]) - new Vector2(num44, num45)).Length() < num38 * 0.9) {

								Neo.SetTile(num44, num45, 192, ref undo);
								Main.tile[num44, num45].color(28);

							}

						}
						else if (Math.Abs(array5[num37] - num44) + Math.Abs(array6[num37] - num45) * num43 < num38) {

							Neo.SetTile(num44, num45, 192, ref undo);
							Main.tile[num44, num45].color(28);

						}

					}

				}

			}

			GrowDungeonTree_MakePassage(j, num5, ref minl, ref minr, ref undo, patch);

			Main.tileSolid[48] = true;

			return true;

		}

	}

}
