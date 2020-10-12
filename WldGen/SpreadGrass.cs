using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/26/2020 Copy/Paste

		public static void SpreadGrass(int i, int j, ref UndoStep undo, int dirt = 0, int grass = 2, bool repeat = true, byte color = 0) {

			try {

                int tileLeft = i - 1;
                int tileRight = i + 2;
                int tileAbove = j - 1;
                int tileBelow = j + 2;

                if (tileLeft < 0)
                    tileLeft = 0;

                if (tileRight > Main.maxTilesX)
                    tileRight = Main.maxTilesX;

                if (tileAbove < 0)
                    tileAbove = 0;

                if (tileBelow > Main.maxTilesY)
                    tileBelow = Main.maxTilesY;

                bool badPlace = true;

                for (int xPos = tileLeft; xPos < tileRight; xPos++) {

                    for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

                        if (!Main.tile[xPos, yPos].active() || !Main.tileSolid[Main.tile[xPos, yPos].type])
                            badPlace = false;

                        if (Main.tile[xPos, yPos].lava() && Main.tile[xPos, yPos].liquid > 0) {
                            badPlace = true;
                            break;
                        }

                    }

                }

                if (badPlace)
                    return;

                undo.Add(new ChangedTile(i, j));

                //Main.tile[i, j].frameX = 0;
                //Main.tile[i, j].frameY = 0;
                Main.tile[i, j].type = (ushort)grass;
                Main.tile[i, j].color(color);

                for (int xPos = tileLeft; xPos < tileRight; xPos++) {

                    for (int yPos = tileAbove; yPos < tileBelow; yPos++) {

                        if (Main.tile[xPos, yPos].active() && Main.tile[xPos, yPos].type == dirt) {

                            try {

                                if (repeat && GrassSpread < 1000) {

                                    GrassSpread++;
                                    SpreadGrass(xPos, yPos, ref undo, dirt, grass, repeat: true, 0);
                                    GrassSpread--;

                                }

                            }
                            catch { }

                        }

                    }

                }

            }
            catch { }

		}

	}

}
