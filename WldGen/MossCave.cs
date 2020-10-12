using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen {
    public partial class WldGen {

        private static byte _mossTile;
        private static byte _mossWall;
        public static int MossCaveSafetyCount;

        public static void MossCave(int x, int y, ref UndoStep undo, int mossType = -1) {

            if (WorldGen.SolidTile(x, y))
                return;

            if (mossType == -2)
                mossType = Main.rand.Next(5);

            if (mossType < 0 || mossType > 4) {

                WorldGen.setMoss(x, y);
                _mossTile = WorldGen.mossTile;
                _mossWall = WorldGen.mossWall;

            }
            else {

                _mossWall = (byte)(54 + mossType);
                _mossTile = (byte)(179 + mossType);

            }

            MossCaveSafetyCount = 0;
            MossOut(x, y, ref undo);

            undo.ResetFrames(true);

        }

        public static void MossOut(int x, int y, ref UndoStep undo) {

            if (x < 0 || (x < Main.screenPosition.X / 16f && !Main.keyState.PressingAlt()))
                return;

            if (y < 0 || (y < Main.screenPosition.Y / 16f && !Main.keyState.PressingAlt()))
                return;

            if (x > Main.maxTilesX || (x > (Main.screenPosition.X + Main.screenWidth) / 16f && !Main.keyState.PressingAlt()))
                return;

            if (y > Main.maxTilesY || (y > (Main.screenPosition.Y + Main.screenHeight) / 16f && !Main.keyState.PressingAlt()))
                return;

            if (MossCaveSafetyCount > 3500)
                return;

            MossCaveSafetyCount++;

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (!WorldGen.SolidTile(x, y) && Main.tile[x, y].wall != _mossWall) {

                Neo.SetWall(x, y, _mossWall, ref undo);

                MossOut(x - 1, y, ref undo);
                MossOut(x + 1, y, ref undo);
                MossOut(x, y - 1, ref undo);
                MossOut(x, y + 1, ref undo);

                return;

            }

            if (Main.tile[x, y].active()) {

                Neo.SetWall(x, y, _mossWall, ref undo);

                ushort type = Main.tile[x, y].type;

                if (type != _mossTile && (type == TileID.Stone || type == TileID.BlueMoss || type == TileID.BrownMoss || type == TileID.GreenMoss || type == TileID.PurpleMoss || type == TileID.RedMoss))
                    Main.tile[x, y].type = _mossTile;

            }

        }

    }

}
