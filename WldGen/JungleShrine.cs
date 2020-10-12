using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen {

    public partial class WldGen {

        public static void MakeJungleShrine(int xPos, int yPos, ref UndoStep undo, int hutTile = -1, int hutWall = -1) {

			UnifiedRandom genRand = WorldGen.genRand;

			if (hutTile == -1)
				hutTile = genRand.Next(5);

            switch (hutTile) {
                case  0: hutTile =  45; break;
                case  1: hutTile = 119; break;
                case  2: hutTile = 120; break;
                case  3: hutTile = 158; break;
                case  4: hutTile = 175; break;
				default: hutTile =  45; break;
            }

            if (hutWall == -1) {

                switch (hutTile) {
                    case  45: hutWall = 10; break;
                    case 119: hutWall = 23; break;
                    case 120: hutWall = 24; break;
                    case 158: hutWall = 42; break;
                    case 175: hutWall = 45; break;
					default : hutWall = 10; break;
                }

            }

			int width  = genRand.Next(2, 4);
			int height = genRand.Next(2, 4);

			for (int curX = xPos - width - 1; curX <= xPos + width + 1; curX++) {

				for (int curY = yPos - height - 1; curY <= yPos + height + 1; curY++) {

					Neo.SetTile(curX, curY, (ushort)hutTile, ref undo);
					Neo.SetLiquid(curX, curY, 0, false);

				}

			}

			for (int curX = xPos - width; curX <= xPos + width; curX++)
				for (int curY = yPos - height; curY <= yPos + height; curY++)
					Neo.SetWall(curX, curY, (ushort)hutWall, ref undo, false);

			bool tilePlaced = false;

			int attempts = 0;

			while (!tilePlaced && attempts < 100) {

				attempts++;

				int randomX = genRand.Next(xPos - width, xPos + width + 1);
				int randomY = genRand.Next(yPos - height, yPos + height - 2);

				PlaceTile(randomX, randomY, 4, ref undo, mute: true, forced: false, -1, 3);

				if (Main.tile[randomX, randomY].type == 4)
					tilePlaced = true;
				
			}

			for (int curX = xPos - width - 1; curX <= xPos + width + 1; curX++)
				for (int curY = yPos + height - 2; curY <= yPos + height; curY++)
					Neo.SetActive(curX, curY, false, ref undo);

			for (int curX = xPos - width - 1; curX <= xPos + width + 1; curX++)
				for (int curY = yPos + height - 2; curY <= yPos + height - 1; curY++)
					Neo.SetActive(curX, curY, false, ref undo);

			for (int curX = xPos - width - 1; curX <= xPos + width + 1; curX++) {

				int attempts2 = 4;
				int curY = yPos + height + 2;

				while (!Main.tile[curX, curY].active() && curY < Main.maxTilesY && attempts2 > 0) {

					Neo.SetTile(curX, curY, TileID.Mud, ref undo);

					curY++;
					attempts2--;

				}

			}

			width -= genRand.Next(1, 3);

			yPos = yPos - height - 2;

			while (width > -1) {

				for (int curX = xPos - width - 1; curX <= xPos + width + 1; curX++)
					Neo.SetTile(curX, yPos, (ushort)hutTile, ref undo);

				width -= genRand.Next(1, 3);
				yPos--;

			}

			undo.ResetFrames(true);

		}

    }

}
