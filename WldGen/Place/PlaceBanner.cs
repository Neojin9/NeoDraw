using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool PlaceBanner(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

            if (!WorldGen.InWorld(x, y))
                return false;

            int frameX = style * 18;
            int frameY = 0;

            if (style >= 90) {

                frameX -= 1620;
                frameY += 54;

            }

            if (Main.tile[x, y - 1] == null)
                Main.tile[x, y - 1] = new Tile();

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();

            if (Main.tile[x, y + 1] == null)
                Main.tile[x, y + 1] = new Tile();

            if (Main.tile[x, y + 2] == null)
                Main.tile[x, y + 2] = new Tile();

            if (!Main.tile[x, y - 1].nactive() || !Main.tileSolid[Main.tile[x, y - 1].type] || Main.tileSolidTop[Main.tile[x, y - 1].type])
                return false;

            if (!Neo.TileCut(new Point[] { new Point(x, y), new Point(x, y + 1), new Point(x, y + 2) }))
                return false;

            undo.Add(new ChangedTile(x, y));
            undo.Add(new ChangedTile(x, y + 1));
            undo.Add(new ChangedTile(x, y + 2));

            Main.tile[x, y].active(active: true);
            Main.tile[x, y].frameY = (short)frameY;
            Main.tile[x, y].frameX = (short)frameX;
            Main.tile[x, y].type = type;

            Main.tile[x, y + 1].active(active: true);
            Main.tile[x, y + 1].frameY = (short)(frameY + 18);
            Main.tile[x, y + 1].frameX = (short)frameX;
            Main.tile[x, y + 1].type = type;

            Main.tile[x, y + 2].active(active: true);
            Main.tile[x, y + 2].frameY = (short)(frameY + 36);
            Main.tile[x, y + 2].frameX = (short)frameX;
            Main.tile[x, y + 2].type = type;

            return true;

        }

    }

}
