using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NeoDraw.WldGen.Place { // Updated Kindof 7/26/2020
    
    public partial class TilePlacer {

		public static bool PlaceTile(int i, int j, int type, ref UndoStep undo, bool mute = true, bool forced = true, int plr = -1, int style = 0) {

			bool result = false;

            if (type == -1)
                return result;

            if (!WorldGen.InWorld(i, j))
                return false;

            Tile tile = Main.tile[i, j];

            if (tile == null) {
                tile = new Tile();
                Main.tile[i, j] = tile;
            }

            if (!Main.tileFrameImportant[type] && type != TileID.WaterCandle && type != TileID.Vines && type != TileID.CrimsonVines && type != TileID.HallowedVines && type != TileID.JungleVines && type != TileID.VineFlowers
                && type != TileID.Cactus && type != TileID.Trees && type != TileID.MushroomTrees && type != TileID.PalmTree) {

                ChangedTile changedTile = new ChangedTile(i, j);

                undo.Add(changedTile);

                tile.active(active: true);
                tile.type = (ushort)type;

                WorldGen.SquareTileFrame(i, j);

                if (!Main.tile[i, j].active() || Main.tile[i, j].type != type) {
                    undo.Remove(changedTile);
                    return false;
                }

                tile.halfBrick(halfBrick: false);
                tile.frameY = 0;
                tile.frameX = 0;

            }

            // Check that tile above Drip is there.
            if ((type == TileID.WaterDrip || type == TileID.HoneyDrip || type == TileID.LavaDrip || type == TileID.SandDrip) && 
                (Main.tile[i, j - 1] == null || Main.tile[i, j - 1].bottomSlope()))
                return false;

            if (Main.tileAlch[type]) {
                result = PlantAlch(i, j, (ushort)type, style, ref undo);
            }
            else if (TileLoader.IsTorch(type)) {

                if (Main.tile[i, j].active())
                    return false;

                if (Main.tile[i - 1, j] == null)
                    Main.tile[i - 1, j] = new Tile();

                if (Main.tile[i + 1, j] == null)
                    Main.tile[i + 1, j] = new Tile();

                if (Main.tile[i, j + 1] == null)
                    Main.tile[i, j + 1] = new Tile();

                Tile tile2 = Main.tile[i - 1, j];
                Tile tile3 = Main.tile[i + 1, j];
                Tile tile4 = Main.tile[i, j + 1];

                if (tile.wall > 0 || (tile2.active() && (tile2.slope() == 0 || tile2.slope() % 2 != 1) && ((Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type] && !TileID.Sets.NotReallySolid[tile2.type]) || tile2.type == 124 || (tile2.type == 5 && Main.tile[i - 1, j - 1].type == 5 && Main.tile[i - 1, j + 1].type == 5))) || (tile3.active() && (tile3.slope() == 0 || tile3.slope() % 2 != 0) && ((Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.NotReallySolid[tile3.type]) || tile3.type == 124 || (tile3.type == 5 && Main.tile[i + 1, j - 1].type == 5 && Main.tile[i + 1, j + 1].type == 5))) || (tile4.active() && Main.tileSolid[tile4.type] && (!Main.tileSolidTop[tile4.type] || (TileID.Sets.Platforms[tile4.type] && tile4.slope() == 0)) && !TileID.Sets.NotReallySolid[tile4.type] && !tile4.halfBrick() && tile4.slope() == 0)) {

                    undo.Add(new ChangedTile(i, j));
                    tile.active(active: true);
                    tile.type = (ushort)type;
                    tile.frameY = (short)(22 * style);

                    WorldGen.SquareTileFrame(i, j);

                    if (Main.keyState.PressingAlt())
                        tile.frameX += 66;

                    result = true;

                }

            }
            else if (TileLoader.IsSapling(type)) {

                if (Main.tile[i, j + 1] == null)
                    Main.tile[i, j + 1] = new Tile();

                int type2 = Main.tile[i, j + 1].type;
                int saplingType = 20;
                int style3 = 0;

                if (Main.tile[i, j + 1].active() && (type2 == 2 || type2 == 109 || type2 == 147 || type2 == 60 || type2 == 23 || type2 == 199 || type2 == 53 || type2 == 234 || type2 == 116 || type2 == 112 || TileLoader.SaplingGrowthType(type2, ref saplingType, ref style3))) {
                    
                    result = Place1x2(i, j, (ushort)type, style, ref undo);
                    WorldGen.SquareTileFrame(i, j);

                }

            }
            else if (TileID.Sets.BasicChest[type]) {

                result = PlaceChest(i, j, ref undo, (ushort)type, false, style) > 0;
                WorldGen.SquareTileFrame(i, j);
                
            }

            switch (type) {

                case TileID.Trees: {

                        DrawInterface.AddClickDelay(200);
                        result = PlaceTree(i, j, ref undo);
                        break;

                    }
                case TileID.PalmTree: {

                        DrawInterface.AddClickDelay(200);
                        result = PlacePalmTree(i, j, ref undo);
                        break;

                    }
                case TileID.MushroomTrees: {

                        DrawInterface.AddClickDelay(200);
                        result = PlaceMushroomTree(i, j, ref undo);
                        break;

                    }
                case TileID.Cactus: {

                        DrawInterface.AddClickDelay(200);
                        result = PlaceCactus(i, j, ref undo);
                        break;

                    }
                case TileID.JunglePlants:
                case TileID.MushroomPlants: {

                        ushort grass = 0;

                        if (type == TileID.JunglePlants) {
                            grass = TileID.JungleGrass;
                        }
                        else if (type == TileID.MushroomPlants) {
                            grass = TileID.MushroomGrass;
                        }

                        if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].active() && Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick() && Main.tile[i, j + 1].type == grass) {

                            if (!Neo.TileCut(i, j))
                                return false;

                            undo.Add(new ChangedTile(i, j));
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(style * 18);

                            result = true;

                        }

                        break;

                    }
                case TileID.ExposedGems: {

                        if (!Main.tile[i, j].active() && (WorldGen.SolidTile(i - 1, j) || WorldGen.SolidTile(i + 1, j) || WorldGen.SolidTile(i, j - 1) || WorldGen.SolidTile(i, j + 1))) {

                            undo.Add(new ChangedTile(i, j));

                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(style * 18);
                            tile.frameY = (short)(WorldGen.genRand.Next(3) * 18);

                            result = true;

                        }

                        break;

                    }
                case TileID.LongMoss: {

                        if ((Main.tileMoss[Main.tile[i - 1, j].type] && WorldGen.SolidTile(i - 1, j)) || (Main.tileMoss[Main.tile[i + 1, j].type] && WorldGen.SolidTile(i + 1, j)) || (Main.tileMoss[Main.tile[i, j - 1].type] && WorldGen.SolidTile(i, j - 1)) || (Main.tileMoss[Main.tile[i, j + 1].type] && WorldGen.SolidTile(i, j + 1))) {

                            undo.Add(new ChangedTile(i, j));
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(style * 18);
                            tile.frameY = (short)(WorldGen.genRand.Next(3) * 18);

                            result = true;

                        }

                        break;

                    }
                case TileID.ChristmasTree: {

                        result = PlaceXmasTree(i, j, ref undo);
                        break;

                    }
                case TileID.Sunflower: {

                        result = PlaceSunflower(i, j, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.SmallPiles: {

                        int styleX = style;
                        int styleY = 0;

                        if (styleX > 71) {

                            styleX -= 72;
                            styleY = 1;

                            if (styleX > 52) {
                                styleX -= 53;
                                styleY = 3;
                            }

                        }

                        result = PlaceSmallPile(i, j, styleX, styleY, ref undo);
                        break;

                    }
                case TileID.PlantDetritus: {

                        int styleX = style;
                        int styleY = 0;

                        if (style > 8) {
                            styleX -= 9;
                            styleY = 1;
                        }

                        PlaceJunglePlant(i, j + 1, TileID.PlantDetritus, styleX, styleY, ref undo);

                        break;

                    }
                case TileID.Mannequin: case TileID.Womannequin: {

                        result = PlaceMan(i, j, (ushort)type, (Main.keyState.PressingAlt() ? 1 : 0), ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.MinecartTrack: {

                        undo.Add(new ChangedTile(i, j));

                        if (Main.tile[i, j].type == TileID.MinecartTrack)
                            WorldGen.KillTile(i, j, false, false, true);

                        Minecart.PlaceTrack(tile, style);

                        break;

                    } // TODO: Make into bool.
                case TileID.Stalactite: {

                        result = PlaceTight(i, j, ref undo, (ushort)type, false, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Pots: {

                        result = PlacePot(i, j, ref undo, 28, style * 3 + 1);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    } // TODO: Make Pot selection better.
                case TileID.Banners: {

                        result = PlaceBanner(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.ShadowOrbs: {

                        if (style == 0) {
                            WldGen.AddShadowOrb(i, j, ref undo, OrbType.ShadowOrb);
                        }
                        else {
                            WldGen.AddShadowOrb(i, j, ref undo, OrbType.CrimsonHeart);
                        }
                        
                        break;

                    }
                case TileID.Switches: {

                        if (Main.tile[i, j].active())
                            break;

                        if (Main.tile[i - 1, j] == null)
                            Main.tile[i - 1, j] = new Tile();

                        if (Main.tile[i + 1, j] == null)
                            Main.tile[i + 1, j] = new Tile();

                        if (Main.tile[i, j + 1] == null)
                            Main.tile[i, j + 1] = new Tile();

                        if (
                               (
                                   Main.tile[i - 1, j].nactive() &&
                                   !Main.tile[i - 1, j].halfBrick() &&
                                   !TileID.Sets.NotReallySolid[Main.tile[i - 1, j].type] &&
                                   Main.tile[i - 1, j].slope() == 0 &&
                                   (
                                       WorldGen.SolidTile(i - 1, j) || 
                                       Main.tile[i - 1, j].type == 124 || 
                                       (
                                           Main.tile[i - 1, j].type == 5 &&
                                           Main.tile[i - 1, j - 1].type == 5 &&
                                           Main.tile[i - 1, j + 1].type == 5
                                       )
                                   )
                               ) ||
                               (
                                   Main.tile[i + 1, j].nactive() &&
                                   !Main.tile[i + 1, j].halfBrick() &&
                                   !TileID.Sets.NotReallySolid[Main.tile[i + 1, j].type] &&
                                   Main.tile[i + 1, j].slope() == 0 &&
                                   (
                                       WorldGen.SolidTile(i + 1, j) ||
                                       Main.tile[i + 1, j].type == 124 ||
                                       (
                                           Main.tile[i + 1, j].type == 5 &&
                                           Main.tile[i + 1, j - 1].type == 5 &&
                                           Main.tile[i + 1, j + 1].type == 5
                                       )
                                   )
                               ) ||
                               (
                                   Main.tile[i, j + 1].nactive() &&
                                   !Main.tile[i, j + 1].halfBrick() &&
                                   WorldGen.SolidTile(i, j + 1) &&
                                   Main.tile[i, j + 1].slope() == 0
                               ) ||
                               tile.wall > 0
                           ) {

                            undo.Add(new ChangedTile(i, j));

                            tile.active(active: true);
                            tile.type = (ushort)type;
                            WorldGen.SquareTileFrame(i, j);

                        }

                        break;

                    }
                case TileID.Cannon: {

                        result = PlaceCannon(i, j, (ushort)type, ref undo, style);
                        break;

                    }
                case TileID.DyePlants: {

                        result = PlaceDye(i, j, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Teleporter: {

                        result = Place3x1(i, j, (ushort)type, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Painting2X3: {

                        result = Place2x3Wall(i, j, (ushort)type, style, ref undo);
                        break;

                    }
                case TileID.Painting3X2: {

                        result = Place3x2Wall(i, j, (ushort)type, style, ref undo);
                        break;

                    }
                case TileID.Painting6X4: {

                        result = Place6x4Wall(i, j, (ushort)type, style, ref undo);
                        break;

                    }
                case TileID.Painting4X3: {

                        result = Place4x3Wall(i, j, (ushort)type, style, ref undo);
                        break;

                    }
                case TileID.Chandeliers: case TileID.Pigronata: {

                        result = PlaceChand(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.LifeFruit: case TileID.PlanteraBulb: {

                        result = PlaceJunglePlant(i, j, (ushort)type, style, 0, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Beds: case TileID.Bathtubs: {

                        int direction = Main.keyState.PressingAlt() ? 1 : -1;

                        result = Place4x2(i, j, (ushort)type, ref undo, direction, style);
                        break;

                    }
                case TileID.WarTable: case TileID.ElderCrystalStand: {

                        result = Place5x4(i, j, (ushort)type, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Signs: case TileID.AnnouncementBox: {

                        result = PlaceSign(i, j, (ushort)type, ref undo, style);
                        break;

                    }
                case TileID.MusicBoxes: case TileID.Jackolanterns: {

                        result = PlaceMB(i, j, (ushort)type, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Pumpkins: case TileID.CookingPots: {

                        result = Place2x2Style(i, j, (ushort)type, ref undo, style);
                        break;

                    }
                case TileID.ClosedDoor: case TileID.OpenDoor: {

                        if (Main.tile[i, j - 1] == null)
                            Main.tile[i, j - 1] = new Tile();

                        if (Main.tile[i, j - 2] == null)
                            Main.tile[i, j - 2] = new Tile();

                        if (Main.tile[i, j - 3] == null)
                            Main.tile[i, j - 3] = new Tile();

                        if (Main.tile[i, j + 1] == null)
                            Main.tile[i, j + 1] = new Tile();

                        if (Main.tile[i, j + 2] == null)
                            Main.tile[i, j + 2] = new Tile();

                        if (Main.tile[i, j + 3] == null)
                            Main.tile[i, j + 3] = new Tile();

                        if (!Main.tile[i, j - 1].active() && !Main.tile[i, j - 2].active() && Main.tile[i, j - 3].active() && Main.tileSolid[Main.tile[i, j - 3].type]) {
                            result = PlaceDoor(i, j - 1, type, ref undo, style);
                            WorldGen.SquareTileFrame(i, j);
                            break;
                        }
                        else {

                            if (Main.tile[i, j + 1].active() || Main.tile[i, j + 2].active() || !Main.tile[i, j + 3].active() || !Main.tileSolid[Main.tile[i, j + 3].type])
                                return false;

                            result = PlaceDoor(i, j + 1, type, ref undo, style);
                            WorldGen.SquareTileFrame(i, j);

                        }

                        if (type == 11)
                            result = WldGen.OpenDoor(i, j + 1, ref undo, (Main.keyState.PressingAlt() ? -1 : 1));

                        break;

                    }
                case TileID.TallGateClosed: case TileID.TallGateOpen: {

                        result = PlaceTallGate(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Lampposts: case TileID.Lamps: case TileID.SillyBalloonTile: {

                        result = Place1xX(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Painting3X3: case TileID.GemLocks: case TileID.WeaponsRack: {

                        if (type == TileID.WeaponsRack)
                            if (style == -1)
                                style = 1;

                        result = Place3x3Wall(i, j, (ushort)type, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);

                        break;

                    }
                case TileID.Bookcases: case TileID.Thrones: case TileID.DefendersForge: {

                        result = Place3x4(i, j, (ushort)type, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.HangingLanterns: case TileID.FireflyinaBottle: case TileID.LightningBuginaBottle: {

                        result = Place1x2Top(i, j, (ushort)type, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Tombstones: case TileID.FishingCrate: case TileID.PartyPresent: {

                        result = Place2x2Horizontal(i, j, (ushort)type, ref undo, style);
                        break;

                    }
                case TileID.Plants2: case TileID.JunglePlants2: case TileID.HallowedPlants2: {

                        if (style == 8)
                            return false;

                        if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].active() && Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick() && ((Main.tile[i, j + 1].type == TileID.Grass && type == TileID.Plants2) || (Main.tile[i, j + 1].type == TileID.JungleGrass && type == TileID.JunglePlants2) || (Main.tile[i, j + 1].type == TileID.HallowedGrass && type == TileID.HallowedPlants2))) {

                            if (Main.tile[i, j].active() || Main.tile[i, j - 1].active())
                                break;

                            undo.Add(new ChangedTile(i, j));

                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(style * 18);
                            result = true;

                        }

                        break;

                    }
                case TileID.Plants: case TileID.CorruptPlants: case TileID.HallowedPlants: case TileID.FleshWeeds: {

                        if (j + 1 < Main.maxTilesY && Main.tile[i, j + 1].active() && Main.tile[i, j + 1].slope() == 0 && !Main.tile[i, j + 1].halfBrick() && ((Main.tile[i, j + 1].type == TileID.Grass && type == TileID.Plants) || (Main.tile[i, j + 1].type == TileID.CorruptGrass && type == TileID.CorruptPlants) || (Main.tile[i, j + 1].type == 199 && type == TileID.FleshWeeds) || ((Main.tile[i, j + 1].type == TileID.ClayPot || Main.tile[i, j + 1].type == TileID.PlanterBox) && type == TileID.Plants) || (Main.tile[i, j + 1].type == TileID.HallowedGrass && type == TileID.HallowedPlants))) {

                            if (Main.tile[i, j].active())
                                break;

                            undo.Add(new ChangedTile(i, j));
                            tile.active(active: true);
                            tile.type = (ushort)type;
                            tile.frameX = (short)(style * 18);
                            result = true;

                        }

                        break;

                    }
                case TileID.Chairs: case TileID.Firework: case TileID.FireworkFountain: case TileID.LavaLamp: {

                        if (Main.tile[i, j - 1] == null)
                            Main.tile[i, j - 1] = new Tile();

                        if (Main.tile[i, j] == null)
                            Main.tile[i, j] = new Tile();

                        result = Place1x2(i, j, (ushort)type, style, ref undo);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                
                case TileID.Bottles: case TileID.Candles: case TileID.PlatinumCandle: case TileID.PeaceCandle: case TileID.ClayPot: case TileID.WaterCandle: case TileID.Books: {

                        result = PlaceOnTable1x1(i, j, type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.LogicGateLamp: case TileID.LogicGate: case TileID.LogicSensor: case TileID.WirePipe: case TileID.WireBulb: case TileID.PixelBox: {

                        result = PlaceLogicTiles(i, j, type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                case TileID.Anvils: case TileID.WorkBenches: case TileID.PiggyBank: case TileID.Bowls: case TileID.MythrilAnvil: case TileID.DjinnLamp: case TileID.GeyserTrap: case TileID.TrapdoorClosed: {

                        result = Place2x1(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }

                case TileID.Traps: case TileID.PlanterBox: {

                        if (!Neo.TileCut(i, j))
                            return false;

                        undo.Add(new ChangedTile(i, j));

                        tile.active(true);
                        tile.type = (ushort)type;
                        tile.frameY = (short)(18 * style);

                        result = true;

                        break;

                    }

                // Place 1x1
                case TileID.Vines: case TileID.CrimsonVines: case TileID.HallowedVines: case TileID.JungleVines: case TileID.VineFlowers:
                case TileID.Crystals: case TileID.Coral: case TileID.Platforms: case TileID.TeamBlockBluePlatform: case TileID.TeamBlockGreenPlatform:
                case TileID.TeamBlockPinkPlatform: case TileID.TeamBlockRedPlatform: case TileID.TeamBlockWhitePlatform: case TileID.TeamBlockYellowPlatform:
                case TileID.Presents: case TileID.PressurePlates: case TileID.Explosives: case TileID.Timers: case TileID.LandMine: case TileID.MetalBars: case TileID.BeachPiles: 
                case TileID.HoneyDrip: case TileID.LavaDrip: case TileID.SandDrip: case TileID.WaterDrip: case TileID.WeightedPressurePlate: case TileID.ProjectilePressurePad: {

                        result = Place1x1(i, j, type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }
                
                // Place 2x2
                case TileID.Lever: case TileID.Boulder: case TileID.InletPump: case TileID.OutletPump: case TileID.FishBowl: case TileID.MonarchButterflyJar: case TileID.PurpleEmperorButterflyJar:
                case TileID.RedAdmiralButterflyJar: case TileID.UlyssesButterflyJar: case TileID.SulphurButterflyJar: case TileID.TreeNymphButterflyJar: case TileID.ZebraSwallowtailButterflyJar:
                case TileID.JuliaButterflyJar: case TileID.BlueJellyfishBowl: case TileID.GreenJellyfishBowl: case TileID.PinkJellyfishBowl: case TileID.ShipInABottle: case TileID.FireworksBox:
                case TileID.Kegs: case TileID.ChineseLanterns: case TileID.Safes: case TileID.SkullLanterns: case TileID.TrashCan: case TileID.Candelabras: case TileID.CrystalBall: case TileID.DiscoBall:
                case TileID.Sinks: case TileID.PlatinumCandelabra: case TileID.AmmoBox: case TileID.Detonator: case TileID.Heart: case TileID.GoldButterflyCage: case TileID.FakeContainers: case TileID.BeeHive:
                case TileID.FakeContainers2: case TileID.TrapdoorOpen: {

                        result = Place2x2(i, j, (ushort)type, style, ref undo);
                        break;

                    }

                // Place 2xX
                case TileID.GrandfatherClocks: case TileID.Statues: case TileID.WaterFountain: case TileID.SeaweedPlanter: case TileID.AlphabetStatues: case TileID.MushroomStatue: case TileID.Sundial:
                case TileID.TargetDummy: case TileID.LunarMonolith: case TileID.PartyBundleOfBalloonTile: case TileID.WarTableBanner: {

                        result = Place2xX(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }

                // Place 6x3
                case TileID.BunnyCage: case TileID.SquirrelCage: case TileID.MallardDuckCage: case TileID.DuckCage: case TileID.BirdCage: case TileID.BlueJay: case TileID.CardinalCage: case TileID.ScorpionCage:
                case TileID.BlackScorpionCage: case TileID.PenguinCage: case TileID.GoldBirdCage: case TileID.GoldBunnyCage: case TileID.SquirrelOrangeCage: case TileID.SquirrelGoldCage: {

                        result = Place6x3(i, j, (ushort)type, ref undo);
                        break;

                    }

                // Place 3x2
                case TileID.Furnaces: case TileID.Hellforge: case TileID.AdamantiteForge: case TileID.LihzahrdAltar: case TileID.BubbleMachine: case TileID.SnailCage: case TileID.GlowingSnailCage:
                case TileID.FrogCage: case TileID.MouseCage: case TileID.WormCage: case TileID.GrasshopperCage: case TileID.GoldFrogCage: case TileID.GoldGrasshopperCage: case TileID.GoldMouseCage:
                case TileID.GoldWormCage: case TileID.Campfire: case TileID.TinkerersWorkbench: case TileID.SharpeningStation: case TileID.Tables: case TileID.DemonAltar: case TileID.Loom: 
                case TileID.Pianos: case TileID.Dressers: case TileID.Benches: case TileID.LargePiles: case TileID.LargePiles2: case TileID.Blendomatic: case TileID.MeatGrinder: case TileID.CageBuggy:
                case TileID.CageGrubby: case TileID.CageSluggy: case TileID.CageEnchantedNightcrawler: case TileID.Fireplace: case TileID.Tables2: {
                        
                        result = Place3x2(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }

                // Place 3x3
                case TileID.Sawmill: case TileID.SnowballLauncher: case TileID.Extractinator: case TileID.Solidifier: case TileID.DyeVat: case TileID.Larva: case TileID.ImbuingStation: case TileID.Autohammer:
                case TileID.HeavyWorkBench: case TileID.BoneWelder: case TileID.FleshCloningVat: case TileID.GlassKiln: case TileID.LihzahrdFurnace: case TileID.LivingLoom: case TileID.SkyMill:
                case TileID.IceMachine: case TileID.SteampunkBoiler: case TileID.HoneyDispenser: case TileID.BewitchingTable: case TileID.AlchemyTable: case TileID.Chimney: case TileID.LunarCraftingStation:
                case TileID.SillyBalloonMachine: case TileID.PartyMonolith: {

                        result = Place3x3(i, j, (ushort)type, ref undo, style);
                        WorldGen.SquareTileFrame(i, j);
                        break;

                    }

            }

            if (type >= TileNames.OriginalTileCount && TileObjectData.GetTileData(type, style) != null) {
                DrawInterface.SetStatusBarTempMessage("ModTile place attempt.");
                result = PlaceObject(i, j, type, ref undo, mute, style);
            }

            return result;

		}

    }

}
