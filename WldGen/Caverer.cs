using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated 8/10/2020 v1.4 Copy/Paste

		public static void Caverer(int X, int Y, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			switch (genRand.Next(2)) {

				case 0: {

						int   num4 = genRand.Next(7, 9);
						float num5 = genRand.Next(100) * 0.01f;
						float num6 = 1f - num5;

						if (genRand.Next(2) == 0)
							num5 = 0f - num5;
						
						if (genRand.Next(2) == 0)
							num6 = 0f - num6;
						
						Vector2 vector2 = new Vector2(X, Y);

						for (int j = 0; j < num4; j++) {

							vector2 = DigTunnel(vector2.X, vector2.Y, num5, num6, genRand.Next(6, 20), genRand.Next(4, 9), ref undo);

							num5 += genRand.Next(-20, 21) * 0.1f;
							num6 += genRand.Next(-20, 21) * 0.1f;

							if (num5 < -1.5)
								num5 = -1.5f;
							
							if (num5 > 1.5)
								num5 = 1.5f;
							
							if (num6 < -1.5)
								num6 = -1.5f;
							
							if (num6 > 1.5)
								num6 = 1.5f;
							
							float num7 = genRand.Next(100) * 0.01f;
							float num8 = 1f - num7;

							if (genRand.Next(2) == 0)
								num7 = 0f - num7;
							
							if (genRand.Next(2) == 0)
								num8 = 0f - num8;
							
							Vector2 vector3 = DigTunnel(vector2.X, vector2.Y, num7, num8, genRand.Next(30, 50), genRand.Next(3, 6), ref undo);

							TileRunner((int)vector3.X, (int)vector3.Y, genRand.Next(10, 20), genRand.Next(5, 10), -1, ref undo);

						}

						break;

					}
				case 1: {

						int   num  = genRand.Next(15, 30);
						float num2 = genRand.Next(100) * 0.01f;
						float num3 = 1f - num2;

						if (genRand.Next(2) == 0)
							num2 = 0f - num2;
						
						if (genRand.Next(2) == 0)
							num3 = 0f - num3;
						
						Vector2 vector = new Vector2(X, Y);

						for (int i = 0; i < num; i++) {

							vector = DigTunnel(vector.X, vector.Y, num2, num3, WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(2, 6), ref undo, Wet: true);

							num2 += genRand.Next(-20, 21) * 0.1f;
							num3 += genRand.Next(-20, 21) * 0.1f;

							if (num2 < -1.5)
								num2 = -1.5f;
							
							if (num2 > 1.5)
								num2 = 1.5f;
							
							if (num3 < -1.5)
								num3 = -1.5f;
							
							if (num3 > 1.5)
								num3 = 1.5f;
							
						}

						break;

					}

			}

			undo.ResetFrames();

		}

	}

}
