using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 TileCut

		public static bool Place1x2Top(int x, int y, ushort type, int style, ref UndoStep undo) {

            if (!WorldGen.InWorld(x, y))
                return false;

            short frameX = 0;

            if (type == Terraria.ID.TileID.HangingLanterns) {

                if (Main.keyState.PressingAlt())
                    frameX += 18;

            }

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y - 1] == null)
                Main.tile[x, y - 1] = new Tile();

            if (Main.tile[x, y + 1] == null)
                Main.tile[x, y + 1] = new Tile();

            if (!Main.tile[x, y - 1].nactive() || !Main.tileSolid[Main.tile[x, y - 1].type] || Main.tileSolidTop[Main.tile[x, y - 1].type])
                return false;

            if (!Neo.TileCut(new[] { new Point(x, y), new Point(x, y + 1) }))
                return false;

            short num = (short)(style * 36);

            undo.Add(new ChangedTile(x, y));
            undo.Add(new ChangedTile(x, y + 1));

            Main.tile[x, y].active(true);
            Main.tile[x, y].frameY = num;
            Main.tile[x, y].frameX = frameX;
            Main.tile[x, y].type = type;

            Main.tile[x, y + 1].active(true);
            Main.tile[x, y + 1].frameY = (short)(num + 18);
            Main.tile[x, y + 1].frameX = frameX;
            Main.tile[x, y + 1].type = type;

            return true;

        }

    }

}
