using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 7/27/2020 Copy/Paste

		public static bool PlaceMan(int i, int j, ushort type, int dir, ref UndoStep undo) {

            if (!WorldGen.InWorld(i, j))
                return false;

            Point[] points = new Point[6];
            int m = 0;

            for (int k = i; k <= i + 1; k++)
                for (int l = j - 2; l <= j; l++)
                    points[m++] = new Point(k, l);

            if (!WorldGen.SolidTile2(i, j + 1) || !WorldGen.SolidTile2(i + 1, j + 1))
                return false;

            if (!Neo.TileCut(points))
                return false;

            byte frameX = (byte)(dir == 1 ? 36 : 0);

            undo.Add(new ChangedTile(i,     j - 2));
            undo.Add(new ChangedTile(i,     j - 1));
            undo.Add(new ChangedTile(i,     j    ));
            undo.Add(new ChangedTile(i + 1, j - 2));
            undo.Add(new ChangedTile(i + 1, j - 1));
            undo.Add(new ChangedTile(i + 1, j    ));

            Main.tile[i, j - 2].active(true);
            Main.tile[i, j - 2].frameY = 0;
            Main.tile[i, j - 2].frameX = frameX;
            Main.tile[i, j - 2].type = type;

            Main.tile[i, j - 1].active(true);
            Main.tile[i, j - 1].frameY = 18;
            Main.tile[i, j - 1].frameX = frameX;
            Main.tile[i, j - 1].type = type;

            Main.tile[i, j].active(true);
            Main.tile[i, j].frameY = 36;
            Main.tile[i, j].frameX = frameX;
            Main.tile[i, j].type = type;

            Main.tile[i + 1, j - 2].active(true);
            Main.tile[i + 1, j - 2].frameY = 0;
            Main.tile[i + 1, j - 2].frameX = (byte)(18 + frameX);
            Main.tile[i + 1, j - 2].type = type;

            Main.tile[i + 1, j - 1].active(true);
            Main.tile[i + 1, j - 1].frameY = 18;
            Main.tile[i + 1, j - 1].frameX = (byte)(18 + frameX);
            Main.tile[i + 1, j - 1].type = type;

            Main.tile[i + 1, j].active(true);
            Main.tile[i + 1, j].frameY = 36;
            Main.tile[i + 1, j].frameX = (byte)(18 + frameX);
            Main.tile[i + 1, j].type = type;

            return true;

        }

        public static bool PlaceWoman(int i, int j, int dir, ref UndoStep undo) {

            if (!WorldGen.InWorld(i, j))
                return false;

            Point[] points = new Point[6];
            int m = 0;

            for (int k = i; k <= i + 1; k++)
                for (int l = j - 2; l <= j; l++)
                    points[m++] = new Point(k, l);

            if (!WorldGen.SolidTile2(i, j + 1) || !WorldGen.SolidTile2(i + 1, j + 1))
                return false;

            if (!Neo.TileCut(points))
                return false;

            byte frameX = (byte)(dir == 1 ? 36 : 0);

            undo.Add(new ChangedTile(i, j - 2));
            undo.Add(new ChangedTile(i, j - 1));
            undo.Add(new ChangedTile(i, j));
            undo.Add(new ChangedTile(i + 1, j - 2));
            undo.Add(new ChangedTile(i + 1, j - 1));
            undo.Add(new ChangedTile(i + 1, j));


            Main.tile[i, j - 2].active(true);
            Main.tile[i, j - 2].frameY = 0;
            Main.tile[i, j - 2].frameX = frameX;
            Main.tile[i, j - 2].type = TileID.Womannequin;

            Main.tile[i, j - 1].active(true);
            Main.tile[i, j - 1].frameY = 18;
            Main.tile[i, j - 1].frameX = frameX;
            Main.tile[i, j - 1].type = TileID.Womannequin;

            Main.tile[i, j].active(true);
            Main.tile[i, j].frameY = 36;
            Main.tile[i, j].frameX = frameX;
            Main.tile[i, j].type = TileID.Womannequin;

            Main.tile[i + 1, j - 2].active(true);
            Main.tile[i + 1, j - 2].frameY = 0;
            Main.tile[i + 1, j - 2].frameX = (byte)(18 + frameX);
            Main.tile[i + 1, j - 2].type = TileID.Womannequin;

            Main.tile[i + 1, j - 1].active(true);
            Main.tile[i + 1, j - 1].frameY = 18;
            Main.tile[i + 1, j - 1].frameX = (byte)(18 + frameX);
            Main.tile[i + 1, j - 1].type = TileID.Womannequin;

            Main.tile[i + 1, j].active(true);
            Main.tile[i + 1, j].frameY = 36;
            Main.tile[i + 1, j].frameX = (byte)(18 + frameX);
            Main.tile[i + 1, j].type = TileID.Womannequin;

            return true;

        }

    }

}
