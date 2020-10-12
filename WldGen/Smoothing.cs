using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using Terraria;


namespace NeoDraw.WldGen {

    public partial class WldGen {

        public static void Smoothing(int minX, int maxX, int minY, int maxY, ref UndoStep undo, bool resetFrames = true) {

            minX = (int)MathHelper.Clamp(minX, 20, Main.maxTilesX - 20);
            maxX = (int)MathHelper.Clamp(maxX, 20, Main.maxTilesX - 20);
            minY = (int)MathHelper.Clamp(minY, 20, Main.maxTilesY - 20);
            maxY = (int)MathHelper.Clamp(maxY, 20, Main.maxTilesY - 20);

            Main.tileSolid[137] = false;

            for (int i = minX; i < maxX; i++) {

                for (int j = minY; j < maxY; j++) {

                    SmoothSlope(i, j, ref undo);

                }

            }

            Main.tileSolid[137] = true;

            if (resetFrames)
                undo.ResetFrames();

        }

        /*public static void Smoothing(int minX, int maxX, int minY, int maxY) {

            minX = (int)MathHelper.Clamp(minX, 20, Main.maxTilesX - 20);
            maxX = (int)MathHelper.Clamp(maxX, 20, Main.maxTilesX - 20);
            minY = (int)MathHelper.Clamp(minY, 20, Main.maxTilesY - 20);
            maxY = (int)MathHelper.Clamp(maxY, 20, Main.maxTilesY - 20);

            Main.tileSolid[137] = false;

            for (int i = minX; i < maxX; i++) {

                for (int j = minY; j < maxY; j++) {

                    SmoothSlope(i, j);

                }

            }

            Main.tileSolid[137] = true;

        }
        */

    }

}
