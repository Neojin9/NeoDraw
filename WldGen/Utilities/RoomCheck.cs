using Terraria;
using Terraria.ID;
using NeoDraw.Core;
using NeoDraw.UI;

namespace NeoDraw.WldGen {

    public partial class WldGen {

        private static bool _canSpawn;

        private static int _numRoomTiles;
        private static int _roomCeilingsCount;

        private static int _roomX1;
        private static int _roomX2;
        private static int _roomY1;
        private static int _roomY2;

        public static int[] RoomX = new int[WorldGen.maxRoomTiles];
        public static int[] RoomY = new int[WorldGen.maxRoomTiles];
        public static int[] RoomCeilingX = new int[WorldGen.maxRoomTiles];
        public static int[] RoomCeilingY = new int[WorldGen.maxRoomTiles];

        private static bool[] HouseTile;

        public static bool StartRoomCheck(int x, int y) {

            _roomX1 = x;
            _roomX2 = x;
            _roomY1 = y;
            _roomY2 = y;
            _numRoomTiles = 0;
            _roomCeilingsCount = 0;

            HouseTile = new bool[TileNames.TileCount];

            for (int i = 0; i < TileNames.TileCount; i++)
                HouseTile[i] = false;

            _canSpawn = true;

            if (Main.tile[x, y].nactive() && Main.tileSolid[Main.tile[x, y].type]) {

                DrawInterface.SetStatusBarTempMessage("This is a solid block!");
                _canSpawn = false;
                return false;

            }

            CheckRoom(x, y);

            if (_canSpawn && _numRoomTiles < 60) {

                DrawInterface.SetStatusBarTempMessage("This room is too small.");
                _canSpawn = false;
                return false;

            }

            return _canSpawn;

        }

        public static void CheckRoom(int x, int y) {

            if (!_canSpawn)
                return;

            if (x < 10 || y < 10 || x >= Main.maxTilesX - 10 || y >= Main.maxTilesY - 10) {

                DrawInterface.SetStatusBarTempMessage("House extends outside World boundaries.");
                _canSpawn = false;
                return;

            }

            for (int i = 0; i < _numRoomTiles; i++)
                if (RoomX[i] == x && RoomY[i] == y)
                    return;

            RoomX[_numRoomTiles] = x;
            RoomY[_numRoomTiles] = y;

            bool flag3 = false;

            for (int j = 0; j < _roomCeilingsCount; j++) {

                if (RoomCeilingX[j] == x) {

                    flag3 = true;

                    if (RoomCeilingY[j] > y)
                        RoomCeilingY[j] = y;
                    
                    break;

                }

            }

            if (!flag3) {

                RoomCeilingX[_roomCeilingsCount] = x;
                RoomCeilingY[_roomCeilingsCount] = y;
                _roomCeilingsCount++;

            }

            _numRoomTiles++;

            if (_numRoomTiles >= WorldGen.maxRoomTiles) {

                DrawInterface.SetStatusBarTempMessage("This room is too big.");
                _canSpawn = false;
                return;

            }

            if (Main.tile[x, y].nactive()) {

                HouseTile[Main.tile[x, y].type] = true;

                if (Main.tileSolid[Main.tile[x, y].type])
                    return;

                if (Main.tile[x, y].type == 11 && (Main.tile[x, y].frameX == 0 || Main.tile[x, y].frameX == 54 || Main.tile[x, y].frameX == 72 || Main.tile[x, y].frameX == 126))
                    return;

                if (Main.tile[x, y].type == 389)
                    return;

                if (Main.tile[x, y].type == 386 && ((Main.tile[x, y].frameX < 36 && Main.tile[x, y].frameY == 18) || (Main.tile[x, y].frameX >= 36 && Main.tile[x, y].frameY == 0)))
                    return;

            }

            if (x < _roomX1)
                _roomX1 = x;

            if (x > _roomX2)
                _roomX2 = x;

            if (y < _roomY1)
                _roomY1 = y;

            if (y > _roomY2)
                _roomY2 = y;

            bool flag = false;
            bool flag2 = false;

            for (int j = -2; j < 3; j++) {

                if (Main.wallHouse[Main.tile[x + j, y].wall])
                    flag = true;

                if (Main.tile[x + j, y].nactive() && (Main.tileSolid[Main.tile[x + j, y].type] || TileID.Sets.HousingWalls[Main.tile[x + j, y].type]))
                    flag = true;

                if (Main.wallHouse[Main.tile[x, y + j].wall])
                    flag2 = true;

                if (Main.tile[x, y + j].nactive() && (Main.tileSolid[Main.tile[x, y + j].type] || TileID.Sets.HousingWalls[Main.tile[x, y + j].type]))
                    flag2 = true;

            }

            if (!flag || !flag2) {

                DrawInterface.SetStatusBarTempMessage("This room is missing a wall.");
                _canSpawn = false;
                return;

            }

            for (int k = x - 1; k < x + 2; k++)
                for (int l = y - 1; l < y + 2; l++)
                    if ((k != x || l != y) && _canSpawn)
                        CheckRoom(k, l);

        }

    }

}
