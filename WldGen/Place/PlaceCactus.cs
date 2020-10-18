using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut Heavily/Modified

		public static bool PlaceCactus(int x, int y, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			bool tileChanged = false;

			while (y < Main.maxTilesY && (!Main.tile[x, y].active() || (Main.tile[x, y].active() && Main.tileCut[Main.tile[x, y].type])))
				y++;

			if (Main.tile[x, y - 1].active() && !Main.tileCut[Main.tile[x, y - 1].type])
				return false;

			if (!GrowCactus(x, y, ref undo, ref tileChanged))
				return tileChanged;

			for (int k = 0; k < 150; k++) {

				int nextX = WorldGen.genRand.Next(x - 1, x + 2);
				int nextY = WorldGen.genRand.Next(y - 10, y + 2);

				GrowCactus(nextX, nextY, ref undo, ref tileChanged);

			}

			return tileChanged;

		}

		public static bool GrowCactus(int x, int y, ref UndoStep undo, ref bool tileChanged) {

			int curX = x;
			int curY = y;

			if (!Main.tile[x, y].nactive())
				return false;

			if (Main.tile[x, y].halfBrick() || Main.tile[x, y].slope() != 0)
				return false;

			if (
				Main.tile[x, y].type != TileID.Sand &&
				Main.tile[x, y].type != TileID.Cactus &&
				Main.tile[x, y].type != TileID.Ebonsand &&
				Main.tile[x, y].type != TileID.Pearlsand &&
				Main.tile[x, y].type != TileID.Crimsand
				
			) {
				return false;
			}

			if (
				Main.tile[x, y].type == TileID.Sand      || 
				Main.tile[x, y].type == TileID.Ebonsand  || 
				Main.tile[x, y].type == TileID.Pearlsand || 
				Main.tile[x, y].type == TileID.Crimsand  || 
				TileLoader.CanGrowModCactus(Main.tile[x, y].type)
			) {

				if (!Neo.TileCut(new[] { new Point(x, y - 1), new Point(x - 1, y - 1), new Point(x + 1, y - 1) }))
					return false;

				undo.Add(new ChangedTile(x, y - 1));

				Main.tile[x, y - 1].active(true);
				Main.tile[x, y - 1].type = TileID.Cactus;

				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendTileSquare(-1, x, y - 1, 1);

				WorldGen.SquareTileFrame(curX, curY - 1);

				tileChanged = true;

			}
			else {

				if (Main.tile[x, y].type != TileID.Cactus)
					return true;
				
				while (Main.tile[curX, curY].active() && Main.tile[curX, curY].type == TileID.Cactus) {

					curY++;

					if (!Main.tile[curX, curY].active() || Main.tile[curX, curY].type != TileID.Cactus) {

						if (Main.tile[curX - 1, curY].active() && Main.tile[curX - 1, curY].type == TileID.Cactus && Main.tile[curX - 1, curY - 1].active() && Main.tile[curX - 1, curY - 1].type == TileID.Cactus && curX >= x) {
							curX--;
						}

						if (Main.tile[curX + 1, curY].active() && Main.tile[curX + 1, curY].type == TileID.Cactus && Main.tile[curX + 1, curY - 1].active() && Main.tile[curX + 1, curY - 1].type == TileID.Cactus && curX <= x) {
							curX++;
						}

					}

				}

				curY--;

				int num5 = curY - y;
				int num6 = x - curX;

				curX = x - num6;
				curY = y;

				int num7 = 11 - num5;
				int num8 = 0;

				for (int m = curX - 2; m <= curX + 2; m++)
					for (int n = curY - num7; n <= curY + num5; n++)
						if (Main.tile[m, n].active() && Main.tile[m, n].type == TileID.Cactus)
							num8++;

				if (WldGen.DrunkWorldGen) { // Allow taller cacti

					if (num8 >= WorldGen.genRand.Next(11, 20))
						return true;

				}
				else if (num8 >= WorldGen.genRand.Next(11, 13)) {

					return true;

				}

				curX = x;
				curY = y;

				if (num6 == 0) {

					if (num5 == 0) {

						if (!Main.tile[curX, curY - 1].active()) {

							undo.Add(new ChangedTile(curX, curY - 1));

							Main.tile[curX, curY - 1].active(active: true);
							Main.tile[curX, curY - 1].type = TileID.Cactus;

							WorldGen.SquareTileFrame(curX, curY - 1);

							tileChanged = true;

							if (Main.netMode == NetmodeID.Server)
								NetMessage.SendTileSquare(-1, curX, curY - 1, 1);
							
						}

						return true;

					}

					bool flag  = false;
					bool flag2 = false;

					if (Main.tile[curX, curY - 1].active() && Main.tile[curX, curY - 1].type == TileID.Cactus) {

						if (!Main.tile[curX - 1, curY].active() && !Main.tile[curX - 2, curY + 1].active() && !Main.tile[curX - 1, curY - 1].active() && !Main.tile[curX - 1, curY + 1].active() && !Main.tile[curX - 2, curY].active()) {
							flag = true;
						}

						if (!Main.tile[curX + 1, curY].active() && !Main.tile[curX + 2, curY + 1].active() && !Main.tile[curX + 1, curY - 1].active() && !Main.tile[curX + 1, curY + 1].active() && !Main.tile[curX + 2, curY].active()) {
							flag2 = true;
						}

					}

					int num9 = WorldGen.genRand.Next(3);

					if (num9 == 0 && flag) {

						undo.Add(new ChangedTile(curX - 1, curY));

						Main.tile[curX - 1, curY].active(active: true);
						Main.tile[curX - 1, curY].type = TileID.Cactus;

						WorldGen.SquareTileFrame(curX - 1, curY);

						tileChanged = true;

						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, curX - 1, curY, 1);
						
					}
					else if (num9 == 1 && flag2) {

						undo.Add(new ChangedTile(curX + 1, curY));

						Main.tile[curX + 1, curY].active(active: true);
						Main.tile[curX + 1, curY].type = TileID.Cactus;

						WorldGen.SquareTileFrame(curX + 1, curY);

						tileChanged = true;

						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, curX + 1, curY, 1);
						
					}
					else {

						if (num5 >= WorldGen.genRand.Next(2, 8))
							return true;
						
						if (Main.tile[curX - 1, curY - 1].active()) {
							_ = Main.tile[curX - 1, curY - 1].type;
							_ = 80;
						}

						if ((!Main.tile[curX + 1, curY - 1].active() || Main.tile[curX + 1, curY - 1].type != TileID.Cactus) && !Main.tile[curX, curY - 1].active()) {

							undo.Add(new ChangedTile(curX, curY - 1));

							Main.tile[curX, curY - 1].active(true);
							Main.tile[curX, curY - 1].type = TileID.Cactus;
							
							WorldGen.SquareTileFrame(curX, curY - 1);

							tileChanged = true;

							if (Main.netMode == NetmodeID.Server)
								NetMessage.SendTileSquare(-1, curX, curY - 1, 1);
							
						}

					}

				}
				else if (!Main.tile[curX, curY - 1].active() && !Main.tile[curX, curY - 2].active() && !Main.tile[curX + num6, curY - 1].active() && Main.tile[curX - num6, curY - 1].active() && Main.tile[curX - num6, curY - 1].type == TileID.Cactus) {

					undo.Add(new ChangedTile(curX, curY - 1));

					Main.tile[curX, curY - 1].active(true);
					Main.tile[curX, curY - 1].type = TileID.Cactus;
					
					WorldGen.SquareTileFrame(curX, curY - 1);

					tileChanged = true;

					if (Main.netMode == NetmodeID.Server)
						NetMessage.SendTileSquare(-1, curX, curY - 1, 1);
					
				}

			}

			return true;

		}

	}
}
