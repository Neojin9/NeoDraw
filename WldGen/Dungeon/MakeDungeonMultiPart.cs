using System;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated 7/12/2020

        public static ushort CrackedType;
        public static ushort TileType;
        public static ushort WallType;

        public static UndoStep Undo;

        public static bool FramesReset;
        public static bool MakingDungeon;

        public static float Count;

        public static int DungeonStep = -1;

        public static int StartX;
        public static int StartY;
        public static int DungeonScale;
        public static int DungeonStyle;

        public static int CurrentY;

        public static int[] StyleArray;

        public static void StartDungeonCreation(int x, int y, int dungeonStyle = -1) {

            MakingDungeon = true;
            FramesReset = false;
            DungeonStep = 0;
            Undo = new UndoStep("Make Dungeon");
            StartX = x;
            StartY = y;
            DungeonStyle = dungeonStyle;
            CurrentY = -1;

        }

        public static void EndDungeonCreation() {

            Main.tileSolid[CrackedType] = true;
            MakingDungeon = false;
            DungeonStep = -1;
            DrawInterface.SetUndo(Undo);

        }

        public static void MultiPartDungeon() {

            switch (DungeonStep) {

                case 0:
                    DrawInterface.SetStatusBarTempMessage("Dungeon Creation Starting");
                    Part0(StartX, StartY, DungeonStyle);
                    DungeonStep++;
                    break;

                case 1:
                    DrawInterface.SetStatusBarTempMessage("Part 1");
                    Part1();
                    DungeonStep++;
                    break;
                    
                case 2:
                    DrawInterface.SetStatusBarTempMessage("Part 2");
                    Part2();
                    DungeonStep++;
                    break;
                    
                case 3:
                    DrawInterface.SetStatusBarTempMessage("Part 3");
                    Part3();
                    DungeonStep++;
                    break;

                case 4:
                    DrawInterface.SetStatusBarTempMessage("Part 4");
                    Part4();
                    DungeonStep++;
                    break;

                case 5:
                    DrawInterface.SetStatusBarTempMessage("Part 5");
                    Part5();
                    DungeonStep++;
                    break;
                    
                case 6:
                    DrawInterface.SetStatusBarTempMessage("Part 6");
                    Part6();
                    DungeonStep++;
                    break;

                case 7:
                    DrawInterface.SetStatusBarTempMessage("Part 7");
                    Part7();
                    DungeonStep++;
                    break;

                case 8:
                    DrawInterface.SetStatusBarTempMessage("Part 8");
                    Part8();
                    DungeonStep++;
                    break;
                    
                case 9:
                    DrawInterface.SetStatusBarTempMessage("Part 9");
                    Part9();
                    DungeonStep++;
                    break;

                case 10:
                    DrawInterface.SetStatusBarTempMessage("Part 10");
                    Part10();
                    DungeonStep++;
                    break;

                case 11:
                    DrawInterface.SetStatusBarTempMessage("Part 11");
                    Part11();
                    DungeonStep++;
                    break;

                case 12:
                    DrawInterface.SetStatusBarTempMessage("Part 12");
                    Part12();
                    DungeonStep++;
                    break;

                case 13:
                    DrawInterface.SetStatusBarTempMessage("Part 13");
                    Part13();
                    DungeonStep++;
                    break;
                    
                case 14:
                    DrawInterface.SetStatusBarTempMessage("Part 14");
                    Part14();
                    DungeonStep++;
                    break;

                case 15:
                    DrawInterface.SetStatusBarTempMessage("Part 15");
                    Part15();
                    DungeonStep++;
                    break;

                case 16:
                    DrawInterface.SetStatusBarTempMessage("Part 16");
                    Part16();
                    DungeonStep++;
                    break;
                    
                case 17:
                    float percent = CurrentY / (float)dMaxY;
                    DrawInterface.SetStatusBarTempMessage($"{percent:P1}");
                    Part17();
                    if (FramesReset)
                        DungeonStep++;
                    break;

                default:
                    DrawInterface.SetStatusBarTempMessage("Dungeon Creation Complete");
                    EndDungeonCreation();
                    break;

            }

        }

        public static void Part0(int x, int y, int dungeonStyle = -1) {

            UnifiedRandom genRand = WorldGen.genRand;

            int num = (dungeonStyle == -1 ? genRand.Next(3) : dungeonStyle);

            trapDiag = new int[4, 2];

            switch (num) {

                case 0: // Blue
                    TileType = 41;
                    WallType = 7;
                    CrackedType = 481;
                    break;

                case 1: // Green
                    TileType = 43;
                    WallType = 8;
                    CrackedType = 482;
                    break;

                default: // Pink
                    TileType = 44;
                    WallType = 9;
                    CrackedType = 483;
                    break;

            }

            dungeonX = x;
            dungeonY = y;

            DungeonSetup();

            dMinX = x;
            dMaxX = x;
            dMinY = y;
            dMaxY = y;

        }

        public static void Part1() {

            UnifiedRandom genRand = WorldGen.genRand;

            float num4 = Main.maxTilesX / 60;
            num4 += genRand.Next(0, (int)(num4 / 3f));
            int num6 = 5;

            DungeonRoom(dungeonX, dungeonY, TileType, WallType, ref Undo);

            while (num4 > 0f) {

                if (dungeonX < dMinX)
                    dMinX = dungeonX;

                if (dungeonX > dMaxX)
                    dMaxX = dungeonX;

                if (dungeonY > dMaxY)
                    dMaxY = dungeonY;

                num4 -= 1f;
                
                if (num6 > 0)
                    num6--;

                if ((num6 == 0) & (genRand.Next(3) == 0)) {

                    num6 = 5;

                    if (genRand.Next(2) == 0) {

                        int num7 = dungeonX;
                        int num8 = dungeonY;

                        DungeonHalls(dungeonX, dungeonY, TileType, WallType, ref Undo);

                        if (genRand.Next(2) == 0)
                            DungeonHalls(dungeonX, dungeonY, TileType, WallType, ref Undo);

                        DungeonRoom(dungeonX, dungeonY, TileType, WallType, ref Undo);

                        dungeonX = num7;
                        dungeonY = num8;

                    } else {
                        DungeonRoom(dungeonX, dungeonY, TileType, WallType, ref Undo);
                    }

                } else {
                    DungeonHalls(dungeonX, dungeonY, TileType, WallType, ref Undo);
                }

            }

            DungeonRoom(dungeonX, dungeonY, TileType, WallType, ref Undo);

            int num9 = dRoomX[0];
            int num10 = dRoomY[0];

            for (int i = 0; i < numDRooms; i++) {

                if (dRoomY[i] < num10) {

                    num9 = dRoomX[i];
                    num10 = dRoomY[i];

                }

            }

            dungeonX = num9;
            dungeonY = num10;
            dEnteranceX = num9;
            dSurface = false;
            num6 = 5;

            while (!dSurface) {

                if (num6 > 0)
                    num6--;

                if (num6 == 0 && genRand.Next(5) == 0 && dungeonY > Main.worldSurface + 100.0) {

                    num6 = 10;
                    int num11 = dungeonX;
                    int num12 = dungeonY;

                    DungeonHalls(dungeonX, dungeonY, TileType, WallType, ref Undo, forceX: true);
                    DungeonRoom(dungeonX, dungeonY, TileType, WallType, ref Undo);

                    dungeonX = num11;
                    dungeonY = num12;

                }

                DungeonStairs(dungeonX, dungeonY, TileType, WallType, ref Undo);

            }

        }

        public static void Part2() {

            DungeonEnt(dungeonX, dungeonY, TileType, WallType, ref Undo);

        }

        public static void Part2B() { // TODO: Add when updated to v1.4

            UnifiedRandom genRand = WorldGen.genRand;

            int num13 = Main.maxTilesX * 2;

            for (int num14 = 0; num14 < num13; num14++) {

                int i2 = genRand.Next(dMinX, dMaxX);
                int num15 = dMinY;

                if (num15 < Main.worldSurface)
                    num15 = (int)Main.worldSurface;
                
                int j = genRand.Next(num15, dMaxY);

                num14 = ((!DungeonPitTrap(i2, j, TileType, WallType, ref Undo)) ? (num14 + 1) : (num14 + 1500));

            }

        }

        public static void Part3() {

            for (int j = 0; j < numDRooms; j++) {

                for (int k = dRoomL[j]; k <= dRoomR[j]; k++) {

                    if (!Main.tile[k, dRoomT[j] - 1].active()) {

                        DPlatX[numDPlats] = k;
                        DPlatY[numDPlats] = dRoomT[j] - 1;
                        numDPlats++;
                        break;

                    }

                }

                for (int l = dRoomL[j]; l <= dRoomR[j]; l++) {

                    if (!Main.tile[l, dRoomB[j] + 1].active()) {

                        DPlatX[numDPlats] = l;
                        DPlatY[numDPlats] = dRoomB[j] + 1;
                        numDPlats++;
                        break;

                    }

                }

                for (int m = dRoomT[j]; m <= dRoomB[j]; m++) {

                    if (!Main.tile[dRoomL[j] - 1, m].active()) {

                        DDoorX[numDDoors] = dRoomL[j] - 1;
                        DDoorY[numDDoors] = m;
                        DDoorPos[numDDoors] = -1;
                        numDDoors++;
                        break;

                    }

                }

                for (int n = dRoomT[j]; n <= dRoomB[j]; n++) {

                    if (!Main.tile[dRoomR[j] + 1, n].active()) {

                        DDoorX[numDDoors] = dRoomR[j] + 1;
                        DDoorY[numDDoors] = n;
                        DDoorPos[numDDoors] = 1;
                        numDDoors++;
                        break;

                    }

                }

            }

        }

        public static void Part4() {

            UnifiedRandom genRand = WorldGen.genRand;

            int num13 = 0;
            int num14 = 1000;
            int num15 = 0;

            DungeonScale = Main.maxTilesX / 100;

            if (GetGoodWorldGen)
                DungeonScale *= 3;

            while (num15 < DungeonScale) {

                num13++;
                int num16 = genRand.Next(dMinX, dMaxX);
                int num17 = genRand.Next((int)Main.worldSurface + 25, dMaxY);

                if (DrunkWorldGen)
                    num17 = genRand.Next(dungeonY + 25, dMaxY);

                int num18 = num16;

                if (Main.tile[num16, num17].wall == WallType && !Main.tile[num16, num17].active()) {

                    int num19 = 1;

                    if (genRand.Next(2) == 0)
                        num19 = -1;

                    for (; !Main.tile[num16, num17].active(); num17 += num19) { }

                    if (Main.tile[num16 - 1, num17].active() && Main.tile[num16 + 1, num17].active() && Main.tile[num16 - 1, num17].type != CrackedType  && !Main.tile[num16 - 1, num17 - num19].active() && !Main.tile[num16 + 1, num17 - num19].active()) {

                        num15++;
                        int num20 = genRand.Next(5, 13);

                        while (Main.tile[num16 - 1, num17].active() && Main.tile[num16 - 1, num17].type != CrackedType && Main.tile[num16, num17 + num19].active() && Main.tile[num16, num17].active() && !Main.tile[num16, num17 - num19].active() && num20 > 0) {

                            Neo.SetTile(num16, num17, TileID.Spikes, ref Undo);

                            if (!Main.tile[num16 - 1, num17 - num19].active() && !Main.tile[num16 + 1, num17 - num19].active()) {

                                Neo.SetTile(num16, num17 - num19, TileID.Spikes, ref Undo);
                                Neo.SetTile(num16, num17 - num19 * 2, TileID.Spikes, ref Undo);

                            }

                            num16--;
                            num20--;

                        }

                        num20 = genRand.Next(5, 13);
                        num16 = num18 + 1;

                        while (Main.tile[num16 + 1, num17].active() && Main.tile[num16 + 1, num17].type != CrackedType && Main.tile[num16, num17 + num19].active() && Main.tile[num16, num17].active() && !Main.tile[num16, num17 - num19].active() && num20 > 0) {

                            Neo.SetTile(num16, num17, TileID.Spikes, ref Undo);

                            if (!Main.tile[num16 - 1, num17 - num19].active() && !Main.tile[num16 + 1, num17 - num19].active()) {

                                Neo.SetTile(num16, num17 - num19, TileID.Spikes, ref Undo);
                                Neo.SetTile(num16, num17 - num19 * 2, TileID.Spikes, ref Undo);

                            }

                            num16++;
                            num20--;

                        }

                    }

                }

                if (num13 > num14) {

                    num13 = 0;
                    num15++;

                }

            }

        }

        public static void Part5() {

            UnifiedRandom genRand = WorldGen.genRand;

            int num13 = 0;
            int num14 = 1000;
            int num15 = 0;

            while (num15 < DungeonScale) {

                num13++;
                int num21 = genRand.Next(dMinX, dMaxX);
                int num22 = genRand.Next((int)Main.worldSurface + 25, dMaxY);
                int num23 = num22;

                if (Main.tile[num21, num22].wall == WallType && !Main.tile[num21, num22].active()) {

                    int num24 = 1;

                    if (genRand.Next(2) == 0)
                        num24 = -1;

                    for (; num21 > 5 && num21 < Main.maxTilesX - 5 && !Main.tile[num21, num22].active(); num21 += num24) { }

                    if (Main.tile[num21, num22 - 1].active() && Main.tile[num21, num22 + 1].active() && Main.tile[num21, num22 - 1].type != CrackedType && !Main.tile[num21 - num24, num22 - 1].active() && !Main.tile[num21 - num24, num22 + 1].active()) {

                        num15++;
                        int num25 = genRand.Next(5, 13);

                        while (Main.tile[num21, num22 - 1].active() && Main.tile[num21, num22 - 1].type != CrackedType && Main.tile[num21 + num24, num22].active() && Main.tile[num21, num22].active() && !Main.tile[num21 - num24, num22].active() && num25 > 0) {

                            Neo.SetTile(num21, num22, TileID.Spikes, ref Undo);

                            if (!Main.tile[num21 - num24, num22 - 1].active() && !Main.tile[num21 - num24, num22 + 1].active()) {

                                Neo.SetTile(num21 - num24, num22, TileID.Spikes, ref Undo);
                                Neo.SetTile(num21 - num24 * 2, num22, TileID.Spikes, ref Undo);

                            }

                            num22--;
                            num25--;

                        }

                        num25 = genRand.Next(5, 13);
                        num22 = num23 + 1;

                        while (Main.tile[num21, num22 + 1].active() && Main.tile[num21, num22 + 1].type != CrackedType && Main.tile[num21 + num24, num22].active() && Main.tile[num21, num22].active() && !Main.tile[num21 - num24, num22].active() && num25 > 0) {

                            Neo.SetTile(num21, num22, TileID.Spikes, ref Undo);

                            if (!Main.tile[num21 - num24, num22 - 1].active() && !Main.tile[num21 - num24, num22 + 1].active()) {

                                Neo.SetTile(num21 - num24, num22, TileID.Spikes, ref Undo);
                                Neo.SetTile(num21 - num24 * 2, num22, TileID.Spikes, ref Undo);

                            }

                            num22++;
                            num25--;

                        }

                    }

                }

                if (num13 > num14) {

                    num13 = 0;
                    num15++;

                }

            }

        }

        public static void Part6() {

            UnifiedRandom genRand = WorldGen.genRand;

            for (int num26 = 0; num26 < numDDoors; num26++) {

                int num27 = DDoorX[num26] - 10;
                int num28 = DDoorX[num26] + 10;
                int num29 = 100;
                int num30 = 0;

                for (int num33 = num27; num33 < num28; num33++) {

                    bool flag = true;
                    int num34 = DDoorY[num26];

                    while (num34 > 10 && !Main.tile[num33, num34].active())
                        num34--;

                    if (!Main.tileDungeon[Main.tile[num33, num34].type])
                        flag = false;

                    int num31 = num34;

                    for (num34 = DDoorY[num26]; !Main.tile[num33, num34].active(); num34++) { }

                    if (!Main.tileDungeon[Main.tile[num33, num34].type])
                        flag = false;

                    int num32 = num34;

                    if (num32 - num31 < 3)
                        continue;

                    int num35 = num33 - 20;
                    int num36 = num33 + 20;
                    int num37 = num32 - 10;
                    int num38 = num32 + 10;

                    for (int num39 = num35; num39 < num36; num39++) {

                        for (int num40 = num37; num40 < num38; num40++) {

                            if (Main.tile[num39, num40].active() && Main.tile[num39, num40].type == TileID.ClosedDoor) {

                                flag = false;
                                break;

                            }

                        }

                    }

                    if (flag) {

                        for (int num41 = num32 - 3; num41 < num32; num41++) {

                            for (int num42 = num33 - 3; num42 <= num33 + 3; num42++) {

                                if (Main.tile[num42, num41].active()) {

                                    flag = false;
                                    break;

                                }

                            }

                        }

                    }

                    if (flag && num32 - num31 < 20) {

                        bool flag2 = false;

                        if (DDoorPos[num26] == 0 && num32 - num31 < num29)
                            flag2 = true;

                        if (DDoorPos[num26] == -1 && num33 > num30)
                            flag2 = true;

                        if (DDoorPos[num26] == 1 && (num33 < num30 || num30 == 0))
                            flag2 = true;

                        if (flag2) {

                            num30 = num33;
                            num29 = num32 - num31;
                        }

                    }

                }

                if (num29 >= 20)
                    continue;

                int num43 = num30;
                int num44 = DDoorY[num26];
                int num45 = num44;

                for (; !Main.tile[num43, num44].active(); num44++)
                    Neo.SetActive(num43, num44, false, ref Undo);

                while (!Main.tile[num43, num45].active())
                    num45--;

                num44--;
                num45++;

                for (int num46 = num45; num46 < num44 - 2; num46++)
                    Neo.SetTile(num43, num46, TileType, ref Undo);

                int style = 13;

                if (genRand.Next(3) == 0) {

                    switch (WallType) {

                        case 7:
                            style = 16;
                            break;

                        case 8:
                            style = 17;
                            break;

                        case 9:
                            style = 18;
                            break;

                    }

                }

                PlaceTile(num43, num44, TileID.ClosedDoor, ref Undo, mute: true, forced: false, -1, style);

                num43--;
                int num47 = num44 - 3;

                while (!Main.tile[num43, num47].active())
                    num47--;

                if (num44 - num47 < num44 - num45 + 5 && Main.tileDungeon[Main.tile[num43, num47].type])
                    for (int num48 = num44 - 4 - genRand.Next(3); num48 > num47; num48--)
                        Neo.SetTile(num43, num48, TileType, ref Undo);

                num43 += 2;
                num47 = num44 - 3;

                while (!Main.tile[num43, num47].active())
                    num47--;

                if (num44 - num47 < num44 - num45 + 5 && Main.tileDungeon[Main.tile[num43, num47].type])
                    for (int num49 = num44 - 4 - genRand.Next(3); num49 > num47; num49--)
                        Neo.SetTile(num43, num49, TileType, ref Undo);

                num44++;
                num43--;

                Neo.SetTile(num43 - 1, num44, TileType, ref Undo);
                Neo.SetTile(num43 + 1, num44, TileType, ref Undo);

            }

        }

        public static void Part7() {

            UnifiedRandom genRand = WorldGen.genRand;

            StyleArray = new int[3];

            switch (WallType) {

                case 7:
                    StyleArray[0] = 7;
                    StyleArray[1] = 94;
                    StyleArray[2] = 95;
                    break;

                case 9:
                    StyleArray[0] = 9;
                    StyleArray[1] = 96;
                    StyleArray[2] = 97;
                    break;

                default:
                    StyleArray[0] = 8;
                    StyleArray[1] = 98;
                    StyleArray[2] = 99;
                    break;

            }

            for (int num50 = 0; num50 < 5; num50++) {

                for (int num51 = 0; num51 < 3; num51++) {

                    int num52 = genRand.Next(40, 240);
                    int num53 = genRand.Next(dMinX, dMaxX);
                    int num54 = genRand.Next(dMinY, dMaxY);

                    for (int num55 = num53 - num52; num55 < num53 + num52; num55++) {

                        for (int num56 = num54 - num52; num56 < num54 + num52; num56++) {

                            if (num56 > Main.worldSurface) {

                                float num57 = Math.Abs(num53 - num55);
                                float num58 = Math.Abs(num54 - num56);

                                if (Math.Sqrt(num57 * num57 + num58 * num58) < num52 * 0.4 && Main.wallDungeon[Main.tile[num55, num56].wall])
                                    WallDungeon(num55, num56, (ushort)StyleArray[num51], ref Undo);

                            }

                        }

                    }

                }

            }

        }

        public static void Part8() {

            for (int num59 = 0; num59 < numDPlats; num59++) {

                int num60 = DPlatX[num59];
                int num61 = DPlatY[num59];
                int num62 = Main.maxTilesX;
                int num63 = 10;

                if (num61 < Main.worldSurface + 50.0)
                    num63 = 20;

                for (int num64 = num61 - 5; num64 <= num61 + 5; num64++) {

                    int num65 = num60;
                    int num66 = num60;
                    bool flag3 = false;

                    if (Main.tile[num65, num64].active()) {

                        flag3 = true;

                    } else {

                        while (!Main.tile[num65, num64].active()) {

                            num65--;

                            if (!Main.tileDungeon[Main.tile[num65, num64].type] || num65 == 0) {

                                flag3 = true;
                                break;

                            }

                        }

                        while (!Main.tile[num66, num64].active()) {

                            num66++;

                            if (!Main.tileDungeon[Main.tile[num66, num64].type] || num66 == Main.maxTilesX - 1) {

                                flag3 = true;
                                break;

                            }

                        }

                    }

                    if (flag3 || num66 - num65 > num63)
                        continue;

                    bool flag4 = true;
                    int num67 = num60 - num63 / 2 - 2;
                    int num68 = num60 + num63 / 2 + 2;
                    int num69 = num64 - 5;
                    int num70 = num64 + 5;

                    for (int num71 = num67; num71 <= num68; num71++) {

                        for (int num72 = num69; num72 <= num70; num72++) {

                            if (Main.tile[num71, num72].active() && Main.tile[num71, num72].type == 19) {

                                flag4 = false;
                                break;

                            }

                        }

                    }

                    for (int num73 = num64 + 3; num73 >= num64 - 5; num73--) {

                        if (Main.tile[num60, num73].active()) {

                            flag4 = false;
                            break;

                        }

                    }

                    if (flag4) {

                        num62 = num64;
                        break;

                    }

                }

                if (num62 <= num61 - 10 || num62 >= num61 + 10)
                    continue;

                int num74 = num60;
                int num75 = num62;
                int num76 = num60 + 1;

                while (!Main.tile[num74, num75].active()) {

                    Neo.SetTile(num74, num75, 19, ref Undo);

                    switch (WallType) {

                        case 7: {
                                Main.tile[num74, num75].frameY = 108;
                                break;
                            }
                        case 8: {
                                Main.tile[num74, num75].frameY = 144;
                                break;
                            }
                        default: {
                                Main.tile[num74, num75].frameY = 126;
                                break;
                            }

                    }

                    WorldGen.TileFrame(num74, num75);

                    num74--;

                }

                for (; !Main.tile[num76, num75].active(); num76++) {

                    Neo.SetTile(num76, num75, 19, ref Undo);

                    switch (WallType) {

                        case 7: {
                                Main.tile[num76, num75].frameY = 108;
                                break;
                            }
                        case 8: {
                                Main.tile[num76, num75].frameY = 144;
                                break;
                            }
                        default: {
                                Main.tile[num76, num75].frameY = 126;
                                break;
                            }

                    }

                    WorldGen.TileFrame(num76, num75);

                }

            }

        }

        public static void Part9() {

            UnifiedRandom genRand = WorldGen.genRand;

            int limit = 5;

            if (DrunkWorldGen)
                limit = 6;

            for (int num77 = 0; num77 < limit; num77++) {

                if (num77 == 4) // TODO: Remove for v1.4
                    continue;

                bool flag5 = false;

                while (!flag5) {

                    int num78 = genRand.Next(dMinX, dMaxX);
                    int num79 = genRand.Next((int)Main.worldSurface, dMaxY);

                    if (!Main.wallDungeon[Main.tile[num78, num79].wall] || Main.tile[num78, num79].active())
                        continue;

                    ushort chestTileType = 21;
                    int contain = 0;
                    int style2 = 0;

                    switch (num77) {

                        case 0: {

                                style2 = 23;
                                contain = 1156;

                                break;
                            }
                        case 1: {

                                if (!crimson) {
                                    style2 = 24;
                                    contain = 1571;
                                }
                                else {
                                    style2 = 25;
                                    contain = 1569;
                                }

                                break;
                            }
                        case 2: {

                                style2 = 26;
                                contain = 1260;

                                break;
                            }
                        case 3: {

                                style2 = 27;
                                contain = 1572;

                                break;
                            }
                        case 4: { // TODO: Ignore until v1.4

                                chestTileType = 467;
                                style2 = 13;
                                contain = 4607;

                                break;
                            }
                        case 5: {

                                if (crimson) {

                                    style2 = 24;
                                    contain = 1571;

                                }
                                else {

                                    style2 = 25;
                                    contain = 1569;

                                }

                                break;
                            }

                    }

                    flag5 = AddBuriedChest(num78, num79, ref Undo, contain, notNearOtherChests: false, style2, trySlope: false, chestTileType);

                }

            }

        }

        public static void Part10() {

            UnifiedRandom genRand = WorldGen.genRand;

            int[] array2 = new int[] {
                genRand.Next(9, 13),
                genRand.Next(9, 13),
                0
            };

            while (array2[1] == array2[0])
                array2[1] = genRand.Next(9, 13);

            array2[2] = genRand.Next(9, 13);

            while (array2[2] == array2[0] || array2[2] == array2[1])
                array2[2] = genRand.Next(9, 13);

            int num13 = 0;
            int num14 = 1000;
            int num15 = 0;

            while (num15 < Main.maxTilesX / 20) {

                num13++;
                int num80 = genRand.Next(dMinX, dMaxX);
                int num81 = genRand.Next(dMinY, dMaxY);
                bool flag6 = true;

                if (Main.wallDungeon[Main.tile[num80, num81].wall] && !Main.tile[num80, num81].active()) {

                    int num82 = 1;

                    if (genRand.Next(2) == 0)
                        num82 = -1;

                    while (flag6 && !Main.tile[num80, num81].active()) {

                        num80 -= num82;

                        if (num80 < 5 || num80 > Main.maxTilesX - 5) {
                            flag6 = false;
                        } else if (Main.tile[num80, num81].active() && !Main.tileDungeon[Main.tile[num80, num81].type]) {
                            flag6 = false;
                        }

                    }

                    if (flag6 && Main.tile[num80, num81].active() && Main.tileDungeon[Main.tile[num80, num81].type] && Main.tile[num80, num81 - 1].active() && Main.tileDungeon[Main.tile[num80, num81 - 1].type] && Main.tile[num80, num81 + 1].active() && Main.tileDungeon[Main.tile[num80, num81 + 1].type]) {

                        num80 += num82;

                        for (int num83 = num80 - 3; num83 <= num80 + 3; num83++) {

                            for (int num84 = num81 - 3; num84 <= num81 + 3; num84++) {

                                if (Main.tile[num83, num84].active() && Main.tile[num83, num84].type == 19) {

                                    flag6 = false;
                                    break;

                                }

                            }

                        }

                        if (flag6 && (!Main.tile[num80, num81 - 1].active() & !Main.tile[num80, num81 - 2].active() & !Main.tile[num80, num81 - 3].active())) {

                            int num85 = num80;
                            int num86 = num80;

                            for (; num85 > dMinX && num85 < dMaxX && !Main.tile[num85, num81].active() && !Main.tile[num85, num81 - 1].active() && !Main.tile[num85, num81 + 1].active(); num85 += num82) { }

                            num85 = Math.Abs(num80 - num85);

                            bool flag7 = genRand.Next(2) == 0;

                            if (num85 > 5) {

                                for (int num87 = genRand.Next(1, 4); num87 > 0; num87--) {

                                    Neo.SetTile(num80, num81, 19, ref Undo);

                                    if (Main.tile[num80, num81].wall == StyleArray[0]) {
                                        Main.tile[num80, num81].frameY = (short)(18 * array2[0]);
                                    }
                                    else if (Main.tile[num80, num81].wall == StyleArray[1]) {
                                        Main.tile[num80, num81].frameY = (short)(18 * array2[1]);
                                    }
                                    else {
                                        Main.tile[num80, num81].frameY = (short)(18 * array2[2]);
                                    }

                                    WorldGen.TileFrame(num80, num81);

                                    if (flag7) {

                                        PlaceTile(num80, num81 - 1, 50, ref Undo, mute: true);

                                        if (genRand.Next(50) == 0 && num81 > (Main.worldSurface + Main.rockLayer) / 2.0 && Main.tile[num80, num81 - 1].type == 50) {

                                            Undo.Add(new ChangedTile(num80, num81 - 1));

                                            Main.tile[num80, num81 - 1].frameX = 90;
                                        }

                                    }

                                    num80 += num82;

                                }

                                num13 = 0;
                                num15++;

                                if (!flag7 && genRand.Next(2) == 0) {

                                    num80 = num86;
                                    num81--;
                                    int num88 = 0;

                                    if (genRand.Next(4) == 0)
                                        num88 = 1;

                                    switch (num88) {

                                        case 0:
                                            num88 = 13;
                                            break;

                                        case 1:
                                            num88 = 49;
                                            break;

                                    }

                                    PlaceTile(num80, num81, num88, ref Undo, mute: true);

                                    if (Main.tile[num80, num81].type == 13) {

                                        Undo.Add(new ChangedTile(num80, num81));

                                        if (genRand.Next(2) == 0) {
                                            Main.tile[num80, num81].frameX = 18;
                                        } else {
                                            Main.tile[num80, num81].frameX = 36;
                                        }

                                    }

                                }

                            }

                        }

                    }

                }

                if (num13 > num14) {

                    num13 = 0;
                    num15++;

                }

            }

        }

        public static void Part11() {

            UnifiedRandom genRand = WorldGen.genRand;

            int num89 = 1;

            for (int num90 = 0; num90 < numDRooms; num90++) {

                int num91 = 0;

                while (num91 < 1000) {

                    int num92 = (int)(dRoomSize[num90] * 0.4);
                    int i2 = dRoomX[num90] + genRand.Next(-num92, num92 + 1);
                    int num93 = dRoomY[num90] + genRand.Next(-num92, num92 + 1);
                    int style3 = 2;
                    int num94;

                    if (num89 == 1)
                        num89++;

                    switch (num89) {

                        case 2:
                            num94 = 155;
                            break;
                        case 3:
                            num94 = 156;
                            break;
                        case 4:
                            num94 = 157;
                            break;
                        case 5:
                            num94 = 163;
                            break;
                        case 6:
                            num94 = 113;
                            break;
                        case 7:
                            num94 = 3317;
                            break;
                        case 8:
                            num94 = 327;
                            style3 = 0;
                            break;
                        default:
                            num94 = 164;
                            num89 = 0;
                            break;

                    }

                    if (num93 < Main.worldSurface + 50.0) {

                        num94 = 327;
                        style3 = 0;

                    }

                    if (num94 == 0 && genRand.Next(2) == 0) {

                        num91 = 1000;
                        continue;

                    }

                    if (AddBuriedChest(i2, num93, ref Undo, num94, notNearOtherChests: false, style3, trySlope: false, 0)) {

                        num91 += 1000;
                        num89++;

                    }

                    num91++;

                }

            }

            dMinX -= 25;
            dMaxX += 25;
            dMinY -= 25;
            dMaxY += 25;

            if (dMinX < 0)
                dMinX = 0;

            if (dMaxX > Main.maxTilesX)
                dMaxX = Main.maxTilesX;

            if (dMinY < 0)
                dMinY = 0;

            if (dMaxY > Main.maxTilesY)
                dMaxY = Main.maxTilesY;

        }

        public static void Part12() {

            int num13 = 0;
            int num14 = 1000;
            int num15 = 0;

            MakeDungeon_Lights(TileType, ref num13, num14, ref num15, StyleArray, ref Undo);

        }

        public static void Part13() {

            int num13 = 0;
            int num14 = 1000;
            int num15 = 0;

            MakeDungeon_Traps(ref num13, num14, ref num15, ref Undo);

        }

        public static void Part14() {
            Count = MakeDungeon_GroundFurniture(WallType, ref Undo);
        }

        public static void Part15() {
            Count = MakeDungeon_Pictures(StyleArray, Count, ref Undo);
        }

        public static void Part16() {
            MakeDungeon_Banners(StyleArray, Count, ref Undo);
        }

        public static void Part17() {

            Undo.ResetFrames();

            FramesReset = true;

            DrunkWorldGen = false;
            GetGoodWorldGen = false;

        }

    }

}
