using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place1xX(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			int frameX = 0;
			int frameY = style * 18;

			int height = 3;

            switch (type) {

                case TileID.Lampposts: {

                        height = 6;

                        if (Main.keyState.PressingAlt())
                            frameX += 18;

                        break;

                    }
                case TileID.TallGateClosed:
                case TileID.TallGateOpen: {

                        height = 5;

                        break;

                    }
                case TileID.SillyBalloonTile: {
                        
                        frameX = style * 2 * 18;

                        if (Main.keyState.PressingAlt())
                            frameX += 18;

                        frameY = 0;

                        break;

                    }
                case TileID.Lamps: {

                        if (Main.keyState.PressingAlt())
                            frameX += 18;

                        break;

                    }

            }

            if (!WorldGen.SolidTile2(x, y + 1))
				return false;

			Point[] points = new Point[height];
			int k = 0;

			for (int i = y - height + 1; i < y + 1; i++)
				points[k++] = new Point(x, i);

			if (!Neo.TileCut(points))
				return false;

			for (int j = 0; j < height; j++) {

				undo.Add(new ChangedTile(x, y - height + 1 + j));

				Main.tile[x, y - height + 1 + j].active(true);

                Main.tile[x, y - height + 1 + j].type   = type;
                Main.tile[x, y - height + 1 + j].frameX = (short)frameX;
                Main.tile[x, y - height + 1 + j].frameY = (short)(j * 18 + height * frameY);

			}

			return true;

		}

	}

}
