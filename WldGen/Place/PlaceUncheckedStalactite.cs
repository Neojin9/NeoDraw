using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place { // Updated v1.4 7/25/2020

	public partial class TilePlacer {

		public static void PlaceUncheckedStalactite(int x, int y, bool preferSmall, int variation, bool spiders, ref UndoStep undo) {

			ushort type = TileID.Stalactite;

			variation = Utils.Clamp(variation, 0, 2);

			if (WorldGen.SolidTile(x, y - 1) && !Main.tile[x, y].active() && !Main.tile[x, y + 1].active()) {

				if (spiders) {

					int num = 108 + variation * 18;

					undo.Add(new ChangedTile(x, y));
					undo.Add(new ChangedTile(x, y + 1));

					Main.tile[x, y].type = type;
					Main.tile[x, y].active(active: true);
					Main.tile[x, y].frameX = (short)num;
					Main.tile[x, y].frameY = 0;

					Main.tile[x, y + 1].type = type;
					Main.tile[x, y + 1].active(active: true);
					Main.tile[x, y + 1].frameX = (short)num;
					Main.tile[x, y + 1].frameY = 18;

					return;

				}

				if (Main.tile[x, y - 1].type == 147 || Main.tile[x, y - 1].type == 161 || Main.tile[x, y - 1].type == 163 || Main.tile[x, y - 1].type == 164 || Main.tile[x, y - 1].type == 200) {

					if (preferSmall) {

						int num2 = variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num2;
						Main.tile[x, y].frameY = 72;

					}
					else {

						int num3 = variation * 18;

						undo.Add(new ChangedTile(x, y));
						undo.Add(new ChangedTile(x, y + 1));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num3;
						Main.tile[x, y].frameY = 0;

						Main.tile[x, y + 1].type = type;
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].frameX = (short)num3;
						Main.tile[x, y + 1].frameY = 18;

					}

				}

				if (Main.tile[x, y - 1].type == 1 || Main.tileMoss[Main.tile[x, y - 1].type] || Main.tile[x, y - 1].type == 117 || Main.tile[x, y - 1].type == 25 || Main.tile[x, y - 1].type == 203) {

					if (preferSmall) {

						int num4 = 54 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num4;
						Main.tile[x, y].frameY = 72;

					}
					else {

						int num5 = 54 + variation * 18;

						undo.Add(new ChangedTile(x, y));
						undo.Add(new ChangedTile(x, y + 1));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num5;
						Main.tile[x, y].frameY = 0;

						Main.tile[x, y + 1].type = type;
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].frameX = (short)num5;
						Main.tile[x, y + 1].frameY = 18;

					}

				}

				if (Main.tile[x, y - 1].type == 225) {

					int num6 = 162 + variation * 18;

					undo.Add(new ChangedTile(x, y));

					Main.tile[x, y].type = type;
					Main.tile[x, y].active(active: true);
					Main.tile[x, y].frameX = (short)num6;
					Main.tile[x, y].frameY = 72;

				}

				if (Main.tile[x, y - 1].type == 396 || Main.tile[x, y - 1].type == 397) {

					if (preferSmall) {

						int num7 = 378 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num7;
						Main.tile[x, y].frameY = 72;

					}
					else {

						int num8 = 378 + variation * 18;

						undo.Add(new ChangedTile(x, y));
						undo.Add(new ChangedTile(x, y + 1));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num8;
						Main.tile[x, y].frameY = 0;

						Main.tile[x, y + 1].type = type;
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].frameX = (short)num8;
						Main.tile[x, y + 1].frameY = 18;

					}

				}

				if (Main.tile[x, y - 1].type == 368) {

					if (preferSmall) {

						int num9 = 432 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num9;
						Main.tile[x, y].frameY = 72;

					}
					else {

						int num10 = 432 + variation * 18;

						undo.Add(new ChangedTile(x, y));
						undo.Add(new ChangedTile(x, y + 1));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num10;
						Main.tile[x, y].frameY = 0;

						Main.tile[x, y + 1].type = type;
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].frameX = (short)num10;
						Main.tile[x, y + 1].frameY = 18;

					}

				}

				if (Main.tile[x, y - 1].type == 367) {

					if (preferSmall) {

						int num11 = 486 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num11;
						Main.tile[x, y].frameY = 72;

					}
					else {

						int num12 = 486 + variation * 18;

						undo.Add(new ChangedTile(x, y));
						undo.Add(new ChangedTile(x, y + 1));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num12;
						Main.tile[x, y].frameY = 0;

						Main.tile[x, y + 1].type = type;
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].frameX = (short)num12;
						Main.tile[x, y + 1].frameY = 18;

					}

				}

			}
			else {

				if (spiders || !WorldGen.SolidTile(x, y + 1) || Main.tile[x, y].active() || Main.tile[x, y - 1].active())
					return;
				
				if (Main.tile[x, y + 1].type == 1 || Main.tileMoss[Main.tile[x, y + 1].type] || Main.tile[x, y - 1].type == 117 || Main.tile[x, y - 1].type == 25 || Main.tile[x, y - 1].type == 203) {

					if (preferSmall) {

						int num13 = 54 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num13;
						Main.tile[x, y].frameY = 90;

					}
					else {

						int num14 = 54 + variation * 18;

						undo.Add(new ChangedTile(x, y - 1));
						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y - 1].type = type;
						Main.tile[x, y - 1].active(active: true);
						Main.tile[x, y - 1].frameX = (short)num14;
						Main.tile[x, y - 1].frameY = 36;

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num14;
						Main.tile[x, y].frameY = 54;

					}

				}

				if (Main.tile[x, y + 1].type == 225) {

					int num15 = 162 + variation * 18;

					undo.Add(new ChangedTile(x, y));

					Main.tile[x, y].type = type;
					Main.tile[x, y].active(active: true);
					Main.tile[x, y].frameX = (short)num15;
					Main.tile[x, y].frameY = 90;

				}

				if (Main.tile[x, y + 1].type == 396 || Main.tile[x, y + 1].type == 397) {

					if (preferSmall) {

						int num16 = 378 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num16;
						Main.tile[x, y].frameY = 90;

					}
					else {

						int num17 = 378 + variation * 18;

						undo.Add(new ChangedTile(x, y - 1));
						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y - 1].type = type;
						Main.tile[x, y - 1].active(active: true);
						Main.tile[x, y - 1].frameX = (short)num17;
						Main.tile[x, y - 1].frameY = 36;

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num17;
						Main.tile[x, y].frameY = 54;

					}

				}

				if (Main.tile[x, y + 1].type == 368) {

					if (preferSmall) {

						int num18 = 432 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num18;
						Main.tile[x, y].frameY = 90;

					}
					else {

						int num19 = 432 + variation * 18;

						undo.Add(new ChangedTile(x, y - 1));
						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y - 1].type = type;
						Main.tile[x, y - 1].active(active: true);
						Main.tile[x, y - 1].frameX = (short)num19;
						Main.tile[x, y - 1].frameY = 36;

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num19;
						Main.tile[x, y].frameY = 54;

					}

				}

				if (Main.tile[x, y + 1].type == 367) {

					if (preferSmall) {

						int num20 = 486 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num20;
						Main.tile[x, y].frameY = 90;

					}
					else {

						int num21 = 486 + variation * 18;

						undo.Add(new ChangedTile(x, y - 1));
						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y - 1].type = type;
						Main.tile[x, y - 1].active(active: true);
						Main.tile[x, y - 1].frameX = (short)num21;
						Main.tile[x, y - 1].frameY = 36;

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)num21;
						Main.tile[x, y].frameY = 54;

					}

				}

			}

		}

	}

}
