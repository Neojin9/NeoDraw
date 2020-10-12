using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer {

        public static bool PlaceTallGate(int x, int y, ushort type, ref UndoStep undo, int style = 0) {

            if (!WorldGen.InWorld(x, y, 5))
                return false;

            int height = 5;
            short frameX = 0;

            if (Main.tile[x, y - 1] == null)
                Main.tile[x, y - 1] = new Tile();

            if (!Main.tile[x, y - 1].nactive() || !Main.tileSolid[Main.tile[x, y - 1].type])
                return false;

            if (!Main.tile[x, y + height].nactive() || !Main.tileSolid[Main.tile[x, y + height].type] || Main.tileSolidTop[Main.tile[x, y - 1].type])
                return false;

            Point[] points = new Point[5];

            for (int i = 0; i < height; i++)
                points[i] = new Point(x, y + i);

            if (!Neo.TileCut(points))
                return false;

            for (int j = 0; j < height; j++)
                undo.Add(new ChangedTile(x, y + j));

            Main.tile[x, y].active(active: true);
            Main.tile[x, y].frameY = 0;
            Main.tile[x, y].frameX = frameX;
            Main.tile[x, y].type = type;

            Main.tile[x, y + 1].active(active: true);
            Main.tile[x, y + 1].frameY = 20;
            Main.tile[x, y + 1].frameX = frameX;
            Main.tile[x, y + 1].type = type;

            Main.tile[x, y + 2].active(active: true);
            Main.tile[x, y + 2].frameY = 38;
            Main.tile[x, y + 2].frameX = frameX;
            Main.tile[x, y + 2].type = type;

            Main.tile[x, y + 3].active(active: true);
            Main.tile[x, y + 3].frameY = 56;
            Main.tile[x, y + 3].frameX = frameX;
            Main.tile[x, y + 3].type = type;

            Main.tile[x, y + 4].active(active: true);
            Main.tile[x, y + 4].frameY = 74;
            Main.tile[x, y + 4].frameX = frameX;
            Main.tile[x, y + 4].type = type;

            return true;

        }

    }

}
