using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/25/2020 Copy/Paste

		public static Vector2 templePather(Vector2 templePath, int destX, int destY, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num  = (int)templePath.X;
			int num2 = (int)templePath.Y;
			int num3 = genRand.Next(5, 20);
			int num4 = genRand.Next(2, 5);

			while (num3 > 0 && (num != destX || num2 != destY)) {

				num3--;

				if (num > destX)
					num--;
				
				if (num < destX)
					num++;
				
				if (num2 > destY)
					num2--;
				
				if (num2 < destY)
					num2++;
				
				for (int i = num - num4; i < num + num4; i++)
					for (int j = num2 - num4; j < num2 + num4; j++)
						Neo.SetWall(i, j, WallID.LihzahrdBrickUnsafe, ref undo, false);

			}

			return new Vector2(num, num2);

		}

	}

}
