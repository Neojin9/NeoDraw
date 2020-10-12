using System;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace NeoDraw.WldGen { // TODO: Change to MicroBiome

    public partial class WldGen { // Updated v1.4 7/29/2020 Copy/Paste

		public static bool Meteor(int i, int j, ref UndoStep undo, float scale = 1f, bool ignorePlayers = true) {

			if (!WorldGen.InWorld(i, j, 50)) {
				DrawInterface.SetStatusBarTempMessage("Too close to edge of world.");
				return false;
            }

			int num = (int)(35 * scale);

			Rectangle rectangle = new Rectangle((i - num) * 16, (j - num) * 16, num * 2 * 16, num * 2 * 16);

			for (int k = 0; k < 255; k++) {

				if (Main.player[k].active && !ignorePlayers) {

					Rectangle value = new Rectangle((int)(Main.player[k].position.X + Main.player[k].width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Main.player[k].position.Y + Main.player[k].height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);

					if (rectangle.Intersects(value)) {
						DrawInterface.SetStatusBarTempMessage("Player too close.");
						return false;
					}

				}

			}

			for (int l = 0; l < 200; l++) {

				if (Main.npc[l].active) {

					Rectangle value2 = new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height);

					if (rectangle.Intersects(value2)) {
						DrawInterface.SetStatusBarTempMessage("NPC too close.");
						return false;
					}

				}

			}

			for (int m = i - num; m < i + num; m++) {

				for (int n = j - num; n < j + num; n++) {

					if (Main.tile[m, n].active()) {

						if (TileID.Sets.BasicChest[Main.tile[m, n].type]) {
							DrawInterface.SetStatusBarTempMessage("Too close to chest.");
							return false;
						}

						if (Main.tileDungeon[Main.tile[m, n].type]) {
							DrawInterface.SetStatusBarTempMessage("Too close to dungeon.");
							return false;
						}

						switch (Main.tile[m, n].type) {
							case 226:
							case 470:
							case 475:
							case 488:
							case 597:
								DrawInterface.SetStatusBarTempMessage("Invalid tiles nearby.");
								return false;
						}

					}

				}

			}

			stopDrops = true;

			num = (int)(WorldGen.genRand.Next(17, 23) * scale);

			for (int num2 = i - num; num2 < i + num; num2++) {

				for (int num3 = j - num; num3 < j + num; num3++) {

					if (num3 <= j + Main.rand.Next(-2, 3) - 5)
						continue;

					float num4 = Math.Abs(i - num2);
					float num5 = Math.Abs(j - num3);

					if ((float)Math.Sqrt(num4 * num4 + num5 * num5) < num * 0.9 + Main.rand.Next(-4, 5)) {

						undo.Add(new ChangedTile(num2, num3));

						if (!Main.tileSolid[Main.tile[num2, num3].type])
							Main.tile[num2, num3].active(active: false);

						Main.tile[num2, num3].type = 37;

					}

				}

			}

			num = (int)(WorldGen.genRand.Next(8, 14) * scale);

			for (int num6 = i - num; num6 < i + num; num6++) {

				for (int num7 = j - num; num7 < j + num; num7++) {

					if (num7 > j + Main.rand.Next(-2, 3) - 4) {

						float num8 = Math.Abs(i - num6);
						float num9 = Math.Abs(j - num7);

						if ((float)Math.Sqrt(num8 * num8 + num9 * num9) < num * 0.8 + Main.rand.Next(-3, 4)) {
							undo.Add(new ChangedTile(num6, num7));
							Main.tile[num6, num7].active(active: false);
						}

					}

				}

			}

			num = (int)(WorldGen.genRand.Next(25, 35) * scale);

			for (int num10 = i - num; num10 < i + num; num10++) {

				for (int num11 = j - num; num11 < j + num; num11++) {

					float num12 = Math.Abs(i - num10);
					float num13 = Math.Abs(j - num11);

					if ((float)Math.Sqrt(num12 * num12 + num13 * num13) < num * 0.7) {

						undo.Add(new ChangedTile(num10, num11));

						if (Main.tile[num10, num11].type == 5 || Main.tile[num10, num11].type == 32 || Main.tile[num10, num11].type == 352) // if (TileID.Sets.GetsDestroyedForMeteors[Main.tile[num10, num11].type])
							WorldGen.KillTile(num10, num11);

						Main.tile[num10, num11].liquid = 0;

					}

					if (Main.tile[num10, num11].type == 37) {

						if (!WorldGen.SolidTile(num10 - 1, num11) && !WorldGen.SolidTile(num10 + 1, num11) && !WorldGen.SolidTile(num10, num11 - 1) && !WorldGen.SolidTile(num10, num11 + 1)) {

							undo.Add(new ChangedTile(num10, num11));
							Main.tile[num10, num11].active(active: false);

						} else if ((Main.tile[num10, num11].halfBrick() || Main.tile[num10 - 1, num11].topSlope()) && !WorldGen.SolidTile(num10, num11 + 1)) {

							undo.Add(new ChangedTile(num10, num11));
							Main.tile[num10, num11].active(active: false);

						}

					}

					WorldGen.SquareTileFrame(num10, num11);
					WorldGen.SquareWallFrame(num10, num11);

				}

			}

			num = (int)(WorldGen.genRand.Next(23, 32) * scale);

			for (int num14 = i - num; num14 < i + num; num14++) {

				for (int num15 = j - num; num15 < j + num; num15++) {

					if (num15 <= j + WorldGen.genRand.Next(-3, 4) - 3 || !Main.tile[num14, num15].active() || Main.rand.Next(10) != 0)
						continue;

					float num16 = Math.Abs(i - num14);
					float num17 = Math.Abs(j - num15);

					if ((float)Math.Sqrt(num16 * num16 + num17 * num17) < num * 0.8) {

						undo.Add(new ChangedTile(num14, num15));

						if (Main.tile[num14, num15].type == 5 || Main.tile[num14, num15].type == 32 || Main.tile[num14, num15].type == 352) // if (TileID.Sets.GetsDestroyedForMeteors[Main.tile[num14, num15].type])
							WorldGen.KillTile(num14, num15);

						Main.tile[num14, num15].type = 37;
						WorldGen.SquareTileFrame(num14, num15);

					}

				}

			}

			num = (int)(WorldGen.genRand.Next(30, 38) * scale);

			for (int num18 = i - num; num18 < i + num; num18++) {

				for (int num19 = j - num; num19 < j + num; num19++) {

					if (num19 <= j + WorldGen.genRand.Next(-2, 3) || !Main.tile[num18, num19].active() || Main.rand.Next(20) != 0)
						continue;

					float num20 = Math.Abs(i - num18);
					float num21 = Math.Abs(j - num19);

					if ((float)Math.Sqrt(num20 * num20 + num21 * num21) < num * 0.85) {

						undo.Add(new ChangedTile(num18, num19));

						if (Main.tile[num18, num19].type == 5 || Main.tile[num18, num19].type == 32 || Main.tile[num18, num19].type == 352) // if (TileID.Sets.GetsDestroyedForMeteors[Main.tile[num18, num19].type])
							WorldGen.KillTile(num18, num19);

						Main.tile[num18, num19].type = 37;
						WorldGen.SquareTileFrame(num18, num19);

					}

				}

			}

			undo.ResetFrames();

			stopDrops = false;

			if (Main.netMode == NetmodeID.SinglePlayer) {
				DrawInterface.SetStatusBarTempMessage(Lang.gen[59].Value);
				//Main.NewText(Lang.gen[59].Value, 50, byte.MaxValue, 130);
			}
			else if (Main.netMode == NetmodeID.Server) {
				NetMessage.BroadcastChatMessage(NetworkText.FromKey(Lang.gen[59].Key), new Color(50, 255, 130));
			}

			if (Main.netMode != NetmodeID.MultiplayerClient)
				NetMessage.SendTileSquare(-1, i, j, 40);
			
			return true;

		}

	}

}
