using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Undo;

namespace NeoDraw.WldGen {

    public partial class WldGen {

        public static void MakeHole(int x, int y, int strength, int steps, ref UndoStep undo, bool countAndDestroy = true, bool wet = false) {

            TileRunner(x, y, strength, steps, wet ? -2 : -1, ref undo, false);

            if (countAndDestroy) {

                IList<Point> tileList = undo.GetChangedTilesList();

                int minX = x;
                int maxX = x;
                int minY = y;
                int maxY = y;

                for (int i = 0; i < tileList.Count; i++) {

                    Point p = tileList[i];

                    if (p.X < minX)
                        minX = p.X;

                    if (p.X > maxX)
                        maxX = p.X;

                    if (p.Y < minY)
                        minY = p.Y;

                    if (p.Y > maxY)
                        maxY = p.Y;

                }

                TileCountAndDestroy(minX, maxX, minY, maxY, ref undo, false);

            }

            undo.ResetFrames();

        }

    }

}
