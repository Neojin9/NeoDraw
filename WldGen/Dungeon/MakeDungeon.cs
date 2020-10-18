using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated 8/1/2020

        #region Variables

        private static int[,] trapDiag = new int[4, 2];

		public static int dungeonX;
        public static int dungeonY;

        public static int numDPlats;
        public static int[] DPlatX = new int[300];
        public static int[] DPlatY = new int[300];

        public static int numDDoors;
        public static int[] DDoorX = new int[300];
        public static int[] DDoorY = new int[300];
        public static int[] DDoorPos = new int[300];

        public static int numDRooms;
        public static int[] dRoomX = new int[WorldGen.maxDRooms];
        public static int[] dRoomY = new int[WorldGen.maxDRooms];
        public static int[] dRoomSize = new int[WorldGen.maxDRooms];

        public static int[] dRoomL = new int[WorldGen.maxDRooms];
        public static int[] dRoomR = new int[WorldGen.maxDRooms];
        public static int[] dRoomT = new int[WorldGen.maxDRooms];
        public static int[] dRoomB = new int[WorldGen.maxDRooms];

        public static bool[] dRoomTreasure = new bool[WorldGen.maxDRooms];

        public static int dEnteranceX;
        public static bool dSurface;

        public static double dxStrength1;
        public static double dyStrength1;
        public static double dxStrength2;
        public static double dyStrength2;

        public static int dMinX;
        public static int dMaxX;
        public static int dMinY;
        public static int dMaxY;

        public static Vector2 lastDungeonHall = Vector2.Zero;

        public static int dWallCount;
        public static bool dWallBroke;

        public static bool dungeonLake;
        
		#endregion

        private static void DungeonSetup() {

			UnifiedRandom genRand = WorldGen.genRand;

			Count = 0;

            Main.tileSolid[CrackedType] = false;

            dungeonLake = true;

            trapDiag = new int[4, 2];

			StyleArray = new int[3];

			numDPlats = 0;
			DPlatX = new int[300];
			DPlatY = new int[300];

			numDDoors = 0;
			DDoorX = new int[300];
			DDoorY = new int[300];
			DDoorPos = new int[300];

			numDRooms = 0;
			dRoomX = new int[WorldGen.maxDRooms];
			dRoomY = new int[WorldGen.maxDRooms];
			dRoomSize = new int[WorldGen.maxDRooms];

			dRoomL = new int[WorldGen.maxDRooms];
			dRoomR = new int[WorldGen.maxDRooms];
			dRoomT = new int[WorldGen.maxDRooms];
			dRoomB = new int[WorldGen.maxDRooms];

			dRoomTreasure = new bool[WorldGen.maxDRooms];

			dEnteranceX = 0;
			dSurface = false;

			dxStrength1 = genRand.Next(25, 30);
			dyStrength1 = genRand.Next(20, 25);
			dxStrength2 = genRand.Next(35, 50);
			dyStrength2 = genRand.Next(10, 15);

			lastDungeonHall = Vector2.Zero;

			dWallCount = 0;
			dWallBroke = false;

        }

    }

}
