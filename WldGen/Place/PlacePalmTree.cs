using System;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Update 7/12/2020

        public static bool PlacePalmTree(int x, int y, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			const int MAX_HEIGHT = 21;
			const int MIN_HEIGHT = 10;

			UnifiedRandom genRand = WorldGen.genRand;

			while (
				y < Main.maxTilesY &&
				(
					!Main.tile[x, y].active() ||
					(
						Main.tile[x, y].active() &&
						Main.tileCut[Main.tile[x, y].type]
					)
				)
			)
				y++;

			Tile groundTile = Main.tile[x, y];

			if (!groundTile.active() || groundTile.halfBrick() || groundTile.slope() != 0)
				return false;
			
			if (groundTile.type != TileID.Sand && groundTile.type != TileID.Crimsand && groundTile.type != TileID.Pearlsand && groundTile.type != TileID.Ebonsand && !TileLoader.CanGrowModPalmTree(groundTile.type))
				return false;
			
			if (!EmptyTileCheck(x - 1, x + 1, y - (MAX_HEIGHT + 1), y - 1, 20, true))
				return false;

			if (!Neo.RangeTileCut(x - 1, x + 1, y - (MAX_HEIGHT + 1), y - 1))
				return false;

			int height = genRand.Next(MIN_HEIGHT, MAX_HEIGHT);
			int num2   = genRand.Next(-8, 9);

			num2 *= 2;

			short num3 = 0;

			for (int k = 0; k < height; k++) {

				undo.Add(new ChangedTile(x, y - 1 - k));

				groundTile = Main.tile[x, y - 1 - k];

				if (k == 0) {

					groundTile.active(true);
					groundTile.type = TileID.PalmTree;
					groundTile.frameX = 66;
					groundTile.frameY = 0;

					continue;

				}

				if (k == height - 1) {

					groundTile.active(true);
					groundTile.type = TileID.PalmTree;
					groundTile.frameX = (short)(22 * genRand.Next(4, 7));
					groundTile.frameY = num3;

					continue;

				}

				if (num3 != num2) {

					float num4 = k / height;

					if (!(num4 < 0.25f)) {

						if ((!(num4 < 0.5f) || genRand.Next(13) != 0) && (!(num4 < 0.7f) || genRand.Next(9) != 0) && num4 < 0.95f) {
							genRand.Next(5);
						}

						short num5 = (short)Math.Sign(num2);
						num3 = (short)(num3 + (short)(num5 * 2));

					}

				}

				groundTile.active(true);
				groundTile.type = TileID.PalmTree;
				groundTile.frameX = (short)(22 * genRand.Next(0, 3));
				groundTile.frameY = num3;

			}

			WorldGen.RangeFrame(x - 2, y - height - 1, x + 2, y + 1);

			if (Main.netMode == NetmodeID.Server)
				NetMessage.SendTileSquare(-1, x, (int)(y - height * 0.5), height + 1);

			return true;

		}

    }

}
