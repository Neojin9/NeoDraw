using System;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen {
    public partial class WldGen {

		public static void CrimVein(Vector2 position, Vector2 velocity, ref UndoStep undo, bool resetFrames = true) {

			float num = WorldGen.genRand.Next(15, 26);
			bool flag = true;
			Vector2 vector = velocity;
			Vector2 vector2 = position;
			int num2 = WorldGen.genRand.Next(100, 150);

			if (velocity.Y < 0f)
				num2 -= 25;

			while (flag) {

				num += WorldGen.genRand.Next(-50, 51) * 0.02f;

				if (num < 15f)
					num = 15f;

				if (num > 25f)
					num = 25f;

				for (int i = (int)(position.X - num / 2f); i < position.X + num / 2f; i++) {

					for (int j = (int)(position.Y - num / 2f); j < position.Y + num / 2f; j++) {

						float num3 = Math.Abs(i - position.X);
						float num4 = Math.Abs(j - position.Y);
						double num5 = Math.Sqrt(num3 * num3 + num4 * num4);

						if (num5 < num * 0.2) {

							undo.Add(new ChangedTile(i, j));
							Main.tile[i, j].active(active: false);
							Main.tile[i, j].wall = 83;

						} else if (num5 < num * 0.5 && Main.tile[i, j].wall != 83) {

							undo.Add(new ChangedTile(i, j));
							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = 203;

							if (num5 < num * 0.4)
								Main.tile[i, j].wall = 83;

						}

					}

				}

				velocity.X += WorldGen.genRand.Next(-50, 51) * 0.05f;
				velocity.Y += WorldGen.genRand.Next(-50, 51) * 0.05f;

				if (velocity.Y < vector.Y - 0.75)
					velocity.Y = vector.Y - 0.75f;

				if (velocity.Y > vector.Y + 0.75)
					velocity.Y = vector.Y + 0.75f;

				if (velocity.X < vector.X - 0.75)
					velocity.X = vector.X - 0.75f;

				if (velocity.X > vector.X + 0.75)
					velocity.X = vector.X + 0.75f;

				position += velocity;

				if (Math.Abs(position.X - vector2.X) + Math.Abs(position.Y - vector2.Y) > num2)
					flag = false;

			}

			heartPos[heartCount] = position;
			heartCount++;

			if (resetFrames)
				undo.ResetFrames(true);

		}

		/*public static void CrimVein(Vector2 position, Vector2 velocity) {

			float num = WorldGen.genRand.Next(15, 26);
			bool flag = true;
			Vector2 vector = velocity;
			Vector2 vector2 = position;
			int num2 = WorldGen.genRand.Next(100, 150);
			
			if (velocity.Y < 0f)
				num2 -= 25;
			
			while (flag) {

				num += WorldGen.genRand.Next(-50, 51) * 0.02f;

				if (num < 15f)
					num = 15f;
				
				if (num > 25f)
					num = 25f;
				
				for (int i = (int)(position.X - num / 2f); i < position.X + num / 2f; i++) {

					for (int j = (int)(position.Y - num / 2f); j < position.Y + num / 2f; j++) {

						float num3 = Math.Abs(i - position.X);
						float num4 = Math.Abs(j - position.Y);
						double num5 = Math.Sqrt(num3 * num3 + num4 * num4);

						if (num5 < num * 0.2) {

							Main.tile[i, j].active(active: false);
							Main.tile[i, j].wall = 83;

						} else if (num5 < num * 0.5 && Main.tile[i, j].wall != 83) {

							Main.tile[i, j].active(active: true);
							Main.tile[i, j].type = 203;

							if (num5 < num * 0.4)
								Main.tile[i, j].wall = 83;
							
						}

					}

				}

				velocity.X += WorldGen.genRand.Next(-50, 51) * 0.05f;
				velocity.Y += WorldGen.genRand.Next(-50, 51) * 0.05f;
				
				if (velocity.Y < vector.Y - 0.75)
					velocity.Y = vector.Y - 0.75f;
				
				if (velocity.Y > vector.Y + 0.75)
					velocity.Y = vector.Y + 0.75f;
				
				if (velocity.X < vector.X - 0.75)
					velocity.X = vector.X - 0.75f;
				
				if (velocity.X > vector.X + 0.75)
					velocity.X = vector.X + 0.75f;
				
				position += velocity;

				if (Math.Abs(position.X - vector2.X) + Math.Abs(position.Y - vector2.Y) > num2)
					flag = false;
				
			}

			heartPos[heartCount] = position;
			heartCount++;

		}
		*/

	}

}
