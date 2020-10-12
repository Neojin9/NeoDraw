using System;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/25/2020

    public static class WldUtils {

        public static double OceanLevel => (Main.worldSurface + Main.rockLayer) / 2.0 + 40.0;

        public static bool CanPoundTile(int x, int y) {

            if (Main.tile[x, y] == null)
                Main.tile[x, y] = new Tile();
            
            if (Main.tile[x, y - 1] == null)
                Main.tile[x, y - 1] = new Tile();
            
            if (Main.tile[x, y + 1] == null)
                Main.tile[x, y + 1] = new Tile();
            
            switch (Main.tile[x, y].type) {

                case 10:
                case 48:
                case 137:
                case 138:
                case 232:
                case 380:
                case 387:
                case 388:
                case 476:
                case 484:
                    return false;

                default:

                    if (Main.tile[x, y - 1].active()) {

                        switch (Main.tile[x, y - 1].type) {

                            case 21:
                            case 26:
                            case 77:
                            case 88:
                            case 235:
                            case 237:
                            case 441:
                            case 467:
                            case 468:
                            case 470:
                            case 475:
                            case 488:
                            case 597:
                                return false;

                        }

                    }

                    return WorldGen.CanKillTile(x, y);

            }

        }

        public static Rectangle ClampToWorld(Rectangle tileRectangle) {

            int num  = Math.Max(0, Math.Min(tileRectangle.Left,   Main.maxTilesX));
            int num2 = Math.Max(0, Math.Min(tileRectangle.Top,    Main.maxTilesY));
            int num3 = Math.Max(0, Math.Min(tileRectangle.Right,  Main.maxTilesX));
            int num4 = Math.Max(0, Math.Min(tileRectangle.Bottom, Main.maxTilesY));
            
            return new Rectangle(num, num2, num3 - num, num4 - num2);

        }

        public static bool Gen(Point origin, GenShape shape, GenAction action) {
            return shape.Perform(origin, action);
        }

        public static bool Gen(Point origin, GenShapeActionPair pair) {
            return pair.Shape.Perform(origin, pair.Action);
        }

        public static bool Find(Point origin, GenSearch search, out Point result) {

            result = search.Find(origin);

            if (result == GenSearch.NOT_FOUND)
                return false;

            return true;

        }

        public static void ClearTile(int x, int y, bool frameNeighbors = false) {

            DrawInterface.AddChangedTile(x, y);
            Main.tile[x, y].ClearTile();

            if (frameNeighbors) {
                WorldGen.TileFrame(x + 1, y);
                WorldGen.TileFrame(x - 1, y);
                WorldGen.TileFrame(x, y + 1);
                WorldGen.TileFrame(x, y - 1);
            }

        }

        public static void ClearWall(int x, int y, bool frameNeighbors = false) {

            DrawInterface.AddChangedTile(x, y);
            Main.tile[x, y].wall = 0;

            if (frameNeighbors) {
                WorldGen.SquareWallFrame(x + 1, y);
                WorldGen.SquareWallFrame(x - 1, y);
                WorldGen.SquareWallFrame(x, y + 1);
                WorldGen.SquareWallFrame(x, y - 1);
            }

        }

        public static void ClearChestLocation(int x, int y) {

            ClearTile(x, y, frameNeighbors: true);
            ClearTile(x - 1, y, frameNeighbors: true);
            ClearTile(x, y - 1, frameNeighbors: true);
            ClearTile(x - 1, y - 1, frameNeighbors: true);

        }

        public static int CountWires(int x, int y, int size) {

            int num = 0;

            for (int i = x - size; i <= x + size; i++) {

                for (int j = y - size; j <= y + size; j++) {

                    if (WorldGen.InWorld(i, j)) {

                        if (Main.tile[i, j].wire())
                            num++;
                        
                        if (Main.tile[i, j].wire2())
                            num++;
                        
                        if (Main.tile[i, j].wire3())
                            num++;
                        
                        if (Main.tile[i, j].wire4())
                            num++;
                        
                    }

                }

            }

            return num;

        }

        public static void DebugRegen() {

            WorldGen.clearWorld();
            WorldGen.generateWorld(Main.ActiveWorldFileData.Seed);
            DrawInterface.SetStatusBarTempMessage("World Regen Complete.");

        }

        public static void DebugRotate() {

            int num = 0;
            int num2 = 0;
            int maxTilesY = Main.maxTilesY;

            for (int i = 0; i < Main.maxTilesX / Main.maxTilesY; i++) {

                for (int j = 0; j < maxTilesY / 2; j++) {

                    for (int k = j; k < maxTilesY - j; k++) {

                        DrawInterface.AddChangedTile(k + num, j + num2);
                        DrawInterface.AddChangedTile(j + num, maxTilesY - k + num2);
                        DrawInterface.AddChangedTile(maxTilesY - k + num, maxTilesY - j + num2);
                        DrawInterface.AddChangedTile(maxTilesY - j + num, k + num2);

                        Tile tile = Main.tile[k + num, j + num2];
                        Main.tile[k + num, j + num2] = Main.tile[j + num, maxTilesY - k + num2];
                        Main.tile[j + num, maxTilesY - k + num2] = Main.tile[maxTilesY - k + num, maxTilesY - j + num2];
                        Main.tile[maxTilesY - k + num, maxTilesY - j + num2] = Main.tile[maxTilesY - j + num, k + num2];
                        Main.tile[maxTilesY - j + num, k + num2] = tile;

                    }

                }

                num += maxTilesY;

            }

        }

        public static bool IsDungeon(int x, int y) {

            if (y < Main.worldSurface)
                return false;
            
            if (x < 0 || x > Main.maxTilesX)
                return false;
            
            if (Main.wallDungeon[Main.tile[x, y].wall])
                return true;
            
            return false;

        }

        public static bool IsTileNearby(int x, int y, int type, int distance) {

            for (int i = x - distance; i <= x + distance; i++)
                for (int j = y - distance; j <= y + distance; j++)
                    if (WorldGen.InWorld(i, j) && Main.tile[i, j].active() && Main.tile[i, j].type == type)
                        return true;
            
            return false;

        }

        public static bool IsUndergroundDesert(int x, int y) {

            if (y < Main.worldSurface)
                return false;
            
            if (x < Main.maxTilesX * 0.15 || x > Main.maxTilesX * 0.85)
                return false;
            
            int num = 15;

            for (int i = x - num; i <= x + num; i++)
                for (int j = y - num; j <= y + num; j++)
                    if (Main.tile[i, j].wall == 187 || Main.tile[i, j].wall == 216)
                        return true;
                    
            return false;

        }

        public static bool OceanDepths(int x, int y) {

            if (y > OceanLevel)
                return false;
            
            if (x < 380 || x > Main.maxTilesX - 380)
                return true;
            
            return false;

        }

        public static Vector2 RandHousePictureDesert() {

            int paintingSize = WorldGen.genRand.Next(4);
            int paintingStyle;

            if (paintingSize <= 1) {

                paintingSize = TileID.Painting3X3;
                int maxValue = 6;
                paintingStyle = 63 + WorldGen.genRand.Next(maxValue);

            }
            else if (paintingSize == 2) {

                paintingSize = TileID.Painting2X3;
                int maxValue2 = 2;
                paintingStyle = 7 + WorldGen.genRand.Next(maxValue2);

            }
            else {

                paintingSize = TileID.Painting6X4;
                int maxValue3 = 6;
                paintingStyle = 37 + WorldGen.genRand.Next(maxValue3);

            }

            return new Vector2(paintingSize, paintingStyle);

        }

        public static void TileFrame(int x, int y, bool frameNeighbors = false) {

            WorldGen.TileFrame(x, y, resetFrame: true);

            if (frameNeighbors) {
                WorldGen.TileFrame(x + 1, y, resetFrame: true);
                WorldGen.TileFrame(x - 1, y, resetFrame: true);
                WorldGen.TileFrame(x, y + 1, resetFrame: true);
                WorldGen.TileFrame(x, y - 1, resetFrame: true);
            }

        }

        public static float UnclampedSmoothStep(float min, float max, float x) {
            return (x - min) / (max - min);
        }

        public static void WireLine(Point start, Point end, ref UndoStep undo) {

            Point point = start;
            Point point2 = end;

            if (end.X < start.X)
                Utils.Swap(ref end.X, ref start.X);

            if (end.Y < start.Y)
                Utils.Swap(ref end.Y, ref start.Y);

            for (int i = start.X; i <= end.X; i++)
                PlaceWire(i, point.Y, ref undo);

            for (int j = start.Y; j <= end.Y; j++)
                PlaceWire(point2.X, j, ref undo);

        }

    }

}
