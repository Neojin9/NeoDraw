using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020 Copy/Paste

		public static bool StonePatch(int X, int Y, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			while (!WorldGen.SolidTile(X, Y)) {

				Y++;

				if (Y > Main.worldSurface)
					return false;
				
			}

			Vector2 value = new Vector2(X, Y);
			Vector2 vector = default;

			vector.X = genRand.NextFloat() * 0.6f - 0.3f;
			vector.Y = genRand.NextFloat() * 0.5f + 0.5f;

			float num2 = genRand.Next(13, 18);
			int num3 = genRand.Next(13, 19);

			if (genRand.Next(3) == 0)
				num2 += genRand.Next(3);
			
			if (genRand.Next(3) == 0)
				num3 += genRand.Next(3);
			
			while (num3 > 0) {

				num3--;
				
				for (int k = X - (int)num2 * 4; k <= X + num2 * 4f; k++) {

					for (int l = Y - (int)num2 * 4; l <= Y + num2 * 4f; l++) {

						float num4 = num2 * (0.7f + genRand.NextFloat() * 0.6f) * 0.3f;
						
						if (genRand.Next(8) == 0)
							num4 *= 2f;
						
						Vector2 vector2 = value - new Vector2(k, l);

						if (vector2.Length() < num4 * 2f && !Main.tile[k, l].active() && Main.tile[k, l + 1].active() && Main.tile[k, l + 1].type == TileID.Stone && genRand.Next(7) == 0 && WorldGen.SolidTile(k - 1, l + 1) && WorldGen.SolidTile(k + 1, l + 1)) {
							
							if (genRand.Next(3) != 0)
								PlaceTile(k, l, TileID.LargePiles, ref undo, mute: true, forced: false, -1, genRand.Next(7, 13));
							
							if (genRand.Next(3) != 0)
								PlaceSmallPile(k, l, genRand.Next(6), 1, ref undo, TileID.SmallPiles);
							
							PlaceSmallPile(k, l, genRand.Next(6), 0, ref undo, TileID.SmallPiles);

						}

						if (vector2.Length() < num4) {

							if (Main.tileSolid[Main.tile[k, l].type]) {

								undo.Add(new ChangedTile(k, l));
								Main.tile[k, l].type = TileID.Stone;

							}
							
							WorldGen.SquareTileFrame(k, l);

						}

					}

				}

				value += vector;
				vector.X += genRand.NextFloat() * 0.2f - 0.1f;
				vector.Y += genRand.NextFloat() * 0.2f - 0.1f;
				
				MathHelper.Clamp(vector.X, -0.3f, 0.3f);
				MathHelper.Clamp(vector.Y, 0.5f, 1f);

			}

			return true;

		}

	}

}
