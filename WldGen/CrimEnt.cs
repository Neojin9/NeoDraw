using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static void CrimEnt(Vector2 position, int crimDir, ref UndoStep undo, bool resetFrames = true) {

			float num = 0f;
			float num2 = WorldGen.genRand.Next(6, 11);
			bool flag = true;
			Vector2 vector = new Vector2(2f, WorldGen.genRand.Next(-20, 0) * 0.01f);
			vector.X *= -crimDir;

			while (flag) {

				num += 1f;

				if (num >= 20f)
					flag = false;

				num2 += WorldGen.genRand.Next(-10, 11) * 0.02f;

				if (num2 < 6f)
					num2 = 6f;

				if (num2 > 10f)
					num2 = 10f;

				for (int i = (int)(position.X - num2 / 2f); i < position.X + num2 / 2f; i++) {

					for (int j = (int)(position.Y - num2 / 2f); j < position.Y + num2 / 2f; j++) {

						float num3 = Math.Abs(i - position.X);
						float num4 = Math.Abs(j - position.Y);

						if (Math.Sqrt(num3 * num3 + num4 * num4) < num2 * 0.5 && Main.tile[i, j].active() && Main.tile[i, j].type == 203) {

							undo.Add(new ChangedTile(i, j));
							Main.tile[i, j].active(active: false);
							flag = true;
							num = 0f;

						}

					}

				}

				position += vector;

			}

			if (resetFrames)
				undo.ResetFrames();

		}

		/*public static void CrimEnt(Vector2 position, int crimDir) {

			float num = 0f;
			float num2 = WorldGen.genRand.Next(6, 11);
			bool flag = true;
			Vector2 vector = new Vector2(2f, WorldGen.genRand.Next(-20, 0) * 0.01f);
			vector.X *= -crimDir;
			
			while (flag) {

				num += 1f;

				if (num >= 20f)
					flag = false;
				
				num2 += WorldGen.genRand.Next(-10, 11) * 0.02f;

				if (num2 < 6f)
					num2 = 6f;
				
				if (num2 > 10f)
					num2 = 10f;
				
				for (int i = (int)(position.X - num2 / 2f); i < position.X + num2 / 2f; i++) {

					for (int j = (int)(position.Y - num2 / 2f); j < position.Y + num2 / 2f; j++) {

						float num3 = Math.Abs(i - position.X);
						float num4 = Math.Abs(j - position.Y);
						
						if (Math.Sqrt(num3 * num3 + num4 * num4) < num2 * 0.5 && Main.tile[i, j].active() && Main.tile[i, j].type == 203) {

							Main.tile[i, j].active(active: false);
							flag = true;
							num = 0f;

						}

					}

				}

				position += vector;

			}

		}
		*/

	}

}
