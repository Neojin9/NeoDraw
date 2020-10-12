using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 8/1/2020 Copy/Paste

		public static void Lakinater(int i, int j, ref UndoStep undo, float strengthMultiplier = 1f) {

			UnifiedRandom genRand = WorldGen.genRand;

			double num  = genRand.Next(25, 50) * strengthMultiplier;
			double num2 = num;
			float depth = genRand.Next(30, 80);

			if (genRand.Next(5) == 0) {

				num  *= 1.5;
				num2 *= 1.5;
				depth *= 1.2f;

			}

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j - depth * 0.3f;

			Vector2 vector2 = default;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-20, -10) * 0.1f;

			while (num > 0.0 && depth > 0f) {

				if (vector.Y + num2 * 0.5 > Main.worldSurface)
					depth = 0f;
				
				num -= genRand.Next(3);
				depth -= 1f;

				int startX = (int)(vector.X - num * 0.5);
				int endX   = (int)(vector.X + num * 0.5);
				int startY = (int)(vector.Y - num * 0.5);
				int endY   = (int)(vector.Y + num * 0.5);

				Neo.WorldRestrain(ref startX, ref endX, ref startY, ref endY);

				num2 = num * genRand.Next(80, 120) * 0.01;

				for (int curX = startX; curX < endX; curX++) {

					for (int curY = startY; curY < endY; curY++) {

						float num8 = Math.Abs(curX - vector.X);
						float num9 = Math.Abs(curY - vector.Y);

						if (Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4) {

							undo.Add(new ChangedTile(curX, curY));

							if (Main.tile[curX, curY].active())
								Main.tile[curX, curY].liquid = byte.MaxValue;

							Main.tile[curX, curY].active(active: false);

						}

					}

				}

				vector += vector2;
				vector2.X += genRand.Next(-10, 11) * 0.05f;
				vector2.Y += genRand.Next(-10, 11) * 0.05f;

				if (vector2.X > 0.5)
					vector2.X = 0.5f;
				
				if (vector2.X < -0.5)
					vector2.X = -0.5f;
				
				if (vector2.Y > 1.5)
					vector2.Y = 1.5f;
				
				if (vector2.Y < 0.5)
					vector2.Y = 0.5f;

			}

			undo.ResetFrames();

		}

	}

}
