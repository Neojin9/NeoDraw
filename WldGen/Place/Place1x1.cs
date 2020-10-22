using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/3/2020 - TileCut

		public static bool Place1x1(int x, int y, int type, ref UndoStep undo, int style = 0) {

			if (!WorldGen.InWorld(x, y))
				return false;

			if (Main.tile[x, y] == null)
				Main.tile[x, y] = new Tile();

            Tile tile = Main.tile[x, y];

            if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();

            Tile tileBelow = Main.tile[x, y + 1];

            if (Main.tile[x, y - 1] == null)
                Main.tile[x, y - 1] = new Tile();

            Tile tileAbove = Main.tile[x, y - 1];

            if (Main.tile[x - 1, y] == null)
                Main.tile[x - 1, y] = new Tile();

            Tile tileLeft = Main.tile[x - 1, y];

            if (Main.tile[x + 1, y] == null)
                Main.tile[x + 1, y] = new Tile();

            Tile tileRight = Main.tile[x + 1, y];

            switch (type) {

                case TileID.BeachPiles: {

                        if (WorldGen.SolidTile2(x, y + 1) || tileBelow.nactive() && Main.tileTable[tileBelow.type]) {

                            if (!Neo.TileCut(x, y))
                                return false;

                            undo.Add(new ChangedTile(x, y));

                            int frameX = style;
                            int frameY = 0;

                            if (style > 5) {
                                frameX = 0;
                                frameY = style - 4;
                            }
                            else if (style > 2) {
                                frameX -= 3;
                                frameY = 1;
                            }

                            tile.type   = (ushort)type;
                            tile.frameX = (short)(22 * frameX);
                            tile.frameY = (short)(22 * frameY);

                            tile.active(true);

                            return true;

                        }

                        break;

                    }
                case TileID.HoneyDrip:
                case TileID.LavaDrip:
                case TileID.SandDrip:
                case TileID.WaterDrip: {

                        if (tileAbove.active() && WorldGen.SolidTile(x, y - 1)) {

                            if (!Neo.TileCut(x, y))
                                return false;

                            undo.Add(new ChangedTile(x, y));

                            Main.tile[x, y].type   = (ushort)type;
                            Main.tile[x, y].frameX = 0;
                            Main.tile[x, y].frameY = 0;

                            Main.tile[x, y].active(true);

                            return true;

                        }

                        break;

                    }
                case TileID.ProjectilePressurePad: {

                        short frameX = -1;

                        if (tileBelow.active() && WorldGen.SolidTile(x, y + 1)) {

                            frameX = 0;

                        }
                        else if (tileAbove.active() && WorldGen.SolidTile(x, y - 1)) {

                            frameX = 18;

                        }
                        else if (tileLeft.active() && WorldGen.SolidTile(x - 1, y)) {

                            frameX = 36;

                        }
                        else if (tileRight.active() && WorldGen.SolidTile(x + 1, y)) {

                            frameX = 54;

                        }

                        if (frameX > -1) {

                            if (!Neo.TileCut(x, y))
                                return false;

                            undo.Add(new ChangedTile(x, y));

                            Main.tile[x, y].type   = (ushort)type;
                            Main.tile[x, y].frameX = frameX;
                            Main.tile[x, y].frameY = 0;

                            Main.tile[x, y].active(true);

                            return true;

                        }

                        break;

                    }
                case TileID.Platforms:
                case TileID.TeamBlockBluePlatform:
                case TileID.TeamBlockGreenPlatform:
                case TileID.TeamBlockPinkPlatform:
                case TileID.TeamBlockRedPlatform:
                case TileID.TeamBlockWhitePlatform:
                case TileID.TeamBlockYellowPlatform: {

                        if (!Neo.TileCut(x, y))
                            return false;

                        undo.Add(new ChangedTile(x, y));

                        tile.type   = (ushort)type;
                        tile.frameY = (short)(style * 18);

                        tile.active(true);

                        return true;

                    }
                case TileID.Vines:
                case TileID.CrimsonVines:
                case TileID.HallowedVines:
                case TileID.JungleVines:
                case TileID.VineFlowers: {

                        if (
                            tileAbove.type == type ||
                            (
                                !tileAbove.bottomSlope() &&
                                (
                                    (type == TileID.Vines || type == TileID.VineFlowers) &&
                                    tileAbove.type == TileID.Grass
                                ) ||
                                (
                                    type == TileID.JungleVines &&
                                    tileAbove.type == TileID.JungleGrass
                                ) ||
                                (
                                    type == TileID.CrimsonVines &&
                                    tileAbove.type == TileID.FleshGrass
                                ) ||
                                (
                                    type == TileID.HallowedVines &&
                                    tileAbove.type == TileID.HallowedGrass
                                )
                            )
                            ) {

                            if (!Neo.TileCut(x, y))
                                return false;

                            undo.Add(new ChangedTile(x, y));

                            tile.type = (ushort)type;

                            tile.active(true);

                            return true;

                        }

                        break;

                    }
                case TileID.Coral: {

                        if (tileAbove.active())
                            return false;

                        if (!tileBelow.active() || !Main.tileSolid[tileBelow.type] || tileBelow.halfBrick() || tileBelow.slope() != 0)
                            return false;

                        if (!Neo.TileCut(x, y))
                            return false;

                        undo.Add(new ChangedTile(x, y));

                        tile.type   = (ushort)type;
                        tile.frameX = (short)(26 * style);
                        tile.frameY = 0;

                        tile.active(true);

                        return true;

                    }
                case TileID.Crystals: {

                        if (WorldGen.SolidTile(x - 1, y) || WorldGen.SolidTile(x + 1, y) || WorldGen.SolidTile(x, y - 1) || WorldGen.SolidTile(x, y + 1)) {

                            if (!Neo.TileCut(x, y))
                                return false;

                            undo.Add(new ChangedTile(x, y));

                            tile.type   = (ushort)type;
                            tile.frameX = (short)(style * 18);
                            tile.frameY = 0;

                            tile.active(true);

                            return true;

                        }

                        break;

                    }
                default: {

                        if (WorldGen.SolidTile2(x, y + 1)) {

                            if (!Neo.TileCut(x, y))
                                return false;

                            undo.Add(new ChangedTile(x, y));

                            tile.type = (ushort)type;

                            tile.active(true);

                            switch (type) {

                                case TileID.Presents:
                                case TileID.Timers:
                                case TileID.MetalBars: {

                                        tile.frameX = (short)(style * 18);
                                        tile.frameY = 0;

                                        break;

                                    }
                                default: {

                                        tile.frameX = 0;
                                        tile.frameY = (short)(style * 18);

                                        break;

                                    }

                            }

                            return true;

                        }

                        break;

                    }

            }

            return false;

		}

	}

}
