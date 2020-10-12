using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated 8/10/2020 v1.4 Copy/Paste

		public static void Cavinator(int i, int j, int steps, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			double num  = genRand.Next(7, 15);
			int    num3 = (genRand.Next(2) == 0) ? -1 : 1;

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j;

			int num4 = genRand.Next(20, 40);

			Vector2 vector2 = default;
			vector2.Y = genRand.Next(10, 20) * 0.01f;
			vector2.X = num3;

			while (num4 > 0) {

				num4--;
				int num5 = (int)(vector.X - num * 0.5);
				int num6 = (int)(vector.X + num * 0.5);
				int num7 = (int)(vector.Y - num * 0.5);
				int num8 = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref num5, ref num6, ref num7, ref num8);

				double num2 = num * genRand.Next(80, 120) * 0.01;

				for (int k = num5; k < num6; k++) {

					for (int l = num7; l < num8; l++) {

						float num9  = Math.Abs(k - vector.X);
						float num10 = Math.Abs(l - vector.Y);

						if (Math.Sqrt(num9 * num9 + num10 * num10) < num2 * 0.4 && TileID.Sets.CanBeClearedDuringGeneration[Main.tile[k, l].type] && Main.tile[k, l].type != TileID.Sand)
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

				if (vector2.Y > 2f)
					vector2.Y = 2f;

				if (vector2.Y < 0f)
					vector2.Y = 0f;

			}

			undo.ResetFrames();

			if (steps > 0 && (int)vector.Y < Main.rockLayer + 50.0)
				Cavinator((int)vector.X, (int)vector.Y, steps - 1, ref undo);

		}

	}

}
