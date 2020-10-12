using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/29/2020 Copy/Paste
		// TODO: Make customizable
		public static void Mountinater(int i, int j, ref UndoStep undo, int size = -1, int tileType = -1, int grassType = -1) {

			if (tileType == -1)
				tileType = TileID.Dirt;

			if (grassType == -1)
				grassType = TileID.Grass;

			UnifiedRandom genRand = WorldGen.genRand;

			double width = genRand.Next(80, 120);
            float num3 = genRand.Next(40, 55);

			Vector2 vector = default;
			vector.X = i;
			vector.Y = j + num3 / 2f;

			Vector2 vector2 = default;
			vector2.X = genRand.Next(-10, 11) * 0.1f;
			vector2.Y = genRand.Next(-20, -10) * 0.1f;

			while (width > 0.0 && num3 > 0f) {

				width -= genRand.Next(4);
				num3 -= 1f;

				int tileLeft  = (int)(vector.X - width * 0.5);
				int tileRight = (int)(vector.X + width * 0.5);
				int tileAbove = (int)(vector.Y - width * 0.5);
				int tileBelow = (int)(vector.Y + width * 0.5);

				if (tileLeft < 0)
					tileLeft = 0;
				
				if (tileRight > Main.maxTilesX)
					tileRight = Main.maxTilesX;
				
				if (tileAbove < 0)
					tileAbove = 0;
				
				if (tileBelow > Main.maxTilesY)
					tileBelow = Main.maxTilesY;

                double num2 = width * genRand.Next(80, 120) * 0.01;

                for (int x = tileLeft; x < tileRight; x++) {

					for (int y = tileAbove; y < tileBelow; y++) {

						float num8 = Math.Abs(x - vector.X);
						float num9 = Math.Abs(y - vector.Y);

						if (Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4 && !Main.tile[x, y].active()) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].active(active: true);
							Main.tile[x, y].type = (ushort)tileType;

							// Trying to put grass on top of the mountain is not working.
							//if (Main.tile[x, y - 1].active() && Main.tile[x, y - 1].type == tileType) {
							//	Main.tile[x, y].type = (ushort)grassType;
                            //}
							//
							//if (Main.tile[x, y + 1].active() && Main.tile[x, y + 1].type == grassType) {
							//	undo.Add(new ChangedTile(x, y + 1));
							//	Main.tile[x, y + 1].type = (ushort)tileType;
                            //}

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
				
				if (vector2.Y > -0.5)
					vector2.Y = -0.5f;
				
				if (vector2.Y < -1.5)
					vector2.Y = -1.5f;
				
			}

			undo.ResetFrames();

		}

	}

}
