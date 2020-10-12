using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public const int TileCounterMax = 20;

		public static int TileCounterNum;
		public static int[] TileCounterX = new int[TileCounterMax];
		public static int[] TileCounterY = new int[TileCounterMax];

		public static void TileCountAndDestroy(int minX, int maxX, int minY, int maxY, ref UndoStep undo, bool resetFrames = true) {

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY, 10, 10);

			for (int i = minX; i < maxX; i++)
				for (int j = minY; j < maxY; j++)
					if (Main.tile[i, j].active() && TileCounter(i, j) < TileCounterMax)
						TileCounterKill(ref undo);

			if (resetFrames)
				undo.ResetFrames();

		}

		public static int TileCounter(int x, int y) {

			TileCounterNum = 0;
			TileCounterNext(x, y);

			return TileCounterNum;

		}

		public static void TileCounterNext(int x, int y) {

			if (TileCounterNum >= TileCounterMax || x < 5 || x > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5 || !Main.tile[x, y].active() || !Main.tileSolid[Main.tile[x, y].type])
				return;
			
			for (int i = 0; i < TileCounterNum; i++)
				if (TileCounterX[i] == x && TileCounterY[i] == y)
					return;
				
			TileCounterX[TileCounterNum] = x;
			TileCounterY[TileCounterNum] = y;

			TileCounterNum++;

			TileCounterNext(x - 1, y);
			TileCounterNext(x + 1, y);
			TileCounterNext(x, y - 1);
			TileCounterNext(x, y + 1);

		}

		public static void TileCounterKill(ref UndoStep undo) {

			for (int i = 0; i < TileCounterNum; i++)
				Neo.SetActive(TileCounterX[i], TileCounterY[i], false, ref undo);

		}

	}

}
