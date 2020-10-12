using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/23/2020

    public class GenBase {

        public delegate bool CustomPerUnitAction(int x, int y, params object[] args);

        protected static UnifiedRandom _random => WorldGen.genRand;

        protected static Tile[,] _tiles => Main.tile;

        protected static int _worldWidth => Main.maxTilesX;

        protected static int _worldHeight => Main.maxTilesY;

    }

}
