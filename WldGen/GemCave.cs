using NeoDraw.Core;
using NeoDraw.Undo;
using NeoDraw.WldGen.Place;
using Terraria;

namespace NeoDraw.WldGen {

    public partial class WldGen {

        public static bool[] Gem = new bool[6];
        public static int GemCaveSafetyCount;

        public static void GemCave(int x, int y, ref UndoStep undo, int gem = -1) {

            if (WorldGen.SolidTile(x, y))
                return;

            for (int i = 0; i < 6; i++)
                Gem[i] = false;

            if (gem < 0) {

                Gem[Main.rand.Next(6)] = true;

                for (int j = 0; j < 6; j++)
                    if (Main.rand.Next(6) == 0)
                        Gem[j] = true;

            }
            else if (gem == 6) {

                for (int i = 0; i < 6; i++)
                    Gem[i] = true;

            }
            else {

                Gem[gem] = true;

            }

            GemCaveSafetyCount = 0;
            GemOut(x, y, ref undo);

            undo.ResetFrames(true);

        }

        public static void GemOut(int x, int y, ref UndoStep undo) {

            if (x < 0 || (x < Main.screenPosition.X / 16f && !Main.keyState.PressingAlt()))
                return;

            if (y < 0 || (y < Main.screenPosition.Y / 16f && !Main.keyState.PressingAlt()))
                return;

            if (x > Main.maxTilesX || (x > (Main.screenPosition.X + Main.screenWidth) / 16f && !Main.keyState.PressingAlt()))
                return;

            if (y > Main.maxTilesY || (y > (Main.screenPosition.Y + Main.screenHeight) / 16f && !Main.keyState.PressingAlt()))
                return;

            if (GemCaveSafetyCount > 3500)
                return;

            GemCaveSafetyCount++;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (!WorldGen.SolidTile(x, y) && (Main.tile[x, y].wall < 48 || Main.tile[x, y].wall > 53)) {

                Neo.SetWall(x, y, (byte)(48 + RandGem()), ref undo);

                if (!Main.tile[x, y].active() && Main.rand.Next(2) == 0)
                    TilePlacer.PlaceTile(x, y, 178, ref undo, true, true, -1, RandGem());

                GemOut(x - 1, y, ref undo);
                GemOut(x + 1, y, ref undo);
                GemOut(x, y - 1, ref undo);
                GemOut(x, y + 1, ref undo);

                return;

            }
            
            if (Main.tile[x, y].active() && WorldGen.genRand.Next(4) == 0) {

                int type = Main.tile[x, y].type;

                if (type == 0 || type == 1 || type == 40 || type == 59 || type == 60 || type == 70 || type == 147 || type == 161)
                    Neo.SetTile(x, y, RandGemTile(), ref undo);

                for (int i = 0; i < 4; i++) {

                    int num = x;
                    int num2 = y;

                    if (i == 0)
                        num--;

                    if (i == 1)
                        num++;

                    if (i == 2)
                        num2--;

                    if (i == 3)
                        num2++;

                    if (!Main.tile[num, num2].active() || WorldGen.genRand.Next(2) != 0)
                        continue;

                    type = Main.tile[num, num2].type;

                    if (type == 0 || type == 1 || type == 40 || type == 59 || type == 60 || type == 70 || type == 147 || type == 161)
                        Neo.SetTile(num, num2, RandGemTile(), ref undo);

                }

            }
            
        }

        public static int RandGem() {

            int num = Main.rand.Next(6);

            while (!Gem[num])
                num = Main.rand.Next(6);

            return num;

        }

        public static ushort RandGemTile() {

            if (Main.rand.Next(20) != 0)
                return 1;

            ushort num = (ushort)RandGem();

            if (num == 0)
                return 67;

            if (num == 1)
                return 66;

            if (num == 2)
                return 63;

            if (num == 3)
                return 65;

            if (num == 4)
                return 64;

            return 68;

        }

    }

}
