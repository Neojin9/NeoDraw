using NeoDraw.Undo;

namespace NeoDraw.WldGen {

    public partial class WldGen {

        public static void SquareTileFrame(int i, int j, ref UndoStep undo, bool resetFrame = true) {

            TileFrame(i - 1, j - 1, ref undo);
            TileFrame(i - 1, j, ref undo);
            TileFrame(i - 1, j + 1, ref undo);
            TileFrame(i, j - 1, ref undo);
            TileFrame(i, j, ref undo, resetFrame);
            TileFrame(i, j + 1, ref undo);
            TileFrame(i + 1, j - 1, ref undo);
            TileFrame(i + 1, j, ref undo);
            TileFrame(i + 1, j + 1, ref undo);

        }

    }

}
