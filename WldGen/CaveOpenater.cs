using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated 8/10/2020 v1.4 Copy/Paste

		public static void CaveOpenater(int i, int j, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			double num = genRand.Next(7, 12);
			int num3   = genRand.Next(2) == 0 ? -1 : 1;

			if (genRand.Next(10) != 0)
				num3 = ((i < Main.maxTilesX / 2) ? 1 : (-1));

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			int num4 = 100;

			Vector2 vector2 = default;
			vector2.Y = 0f;
			vector2.X = num3;

			while (num4 > 0) {

				Tile tile = Main.tile[(int)vector.X, (int)vector.Y];

				if (tile.wall == 0 /*|| (tile.active() && !TileID.Sets.CanBeClearedDuringGeneration[tile.type])*/) // TODO: Uncomment for v1.4
					num4 = 0;

				num4--;

				int minX = (int)(vector.X - num * 0.5);
				int maxX = (int)(vector.X + num * 0.5);
				int minY = (int)(vector.Y - num * 0.5);
				int maxY = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

                double num2 = num * genRand.Next(80, 120) * 0.01;

                for (int k = minX; k < maxX; k++) {

					for (int l = minY; l < maxY; l++) {

						float num9  = Math.Abs(k - vector.X);
						float num10 = Math.Abs(l - vector.Y);

						if (Math.Sqrt(num9 * num9 + num10 * num10) < num2 * 0.4 /*&& TileID.Sets.CanBeClearedDuringGeneration[Main.tile[k, l].type]*/)
							Neo.SetActive(k, l, false, ref undo);
						
					}

				}

				vector += vector2;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;
				
				if (vector2.X > num3 + 0.5f)
					vector2.X = num3 + 0.5f;
				
				if (vector2.X < num3 - 0.5f)
					vector2.X = num3 - 0.5f;
				
				if (vector2.Y > 0f)
					vector2.Y = 0f;
				
				if (vector2.Y < -0.5)
					vector2.Y = -0.5f;

			}

			undo.ResetFrames();

		}

	}

}
