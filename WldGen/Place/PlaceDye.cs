using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated 8/6/2020 Heavily Modified TileCut

		public static bool PlaceDye(int x, int y, int style, ref UndoStep undo) {

			if (!WorldGen.InWorld(x, y))
				return false;

			switch (style) {

				case DyePlants.PinkPricklyPear: {

						if (!Main.tile[x, y + 1].nactive() || Main.tile[x, y + 1].type != TileID.Cactus)
							return false;

						if (!Neo.TileCut(new[] { new Point(x, y), new Point(x, y - 1), new Point(x - 1, y), new Point(x + 1, y) }))
							return false;

						break;

                    }
				case DyePlants.OrangeBloodroot: {

						if (!WorldGen.SolidTile(x, y - 1))
							return false;

						if (!Neo.TileCut(new[] { new Point(x, y), new Point(x, y + 1) }))
							return false;

						break;

                    }
				default: {

						if (!WorldGen.SolidTile(x, y + 1))
							return false;
						
						switch (style) {

							case DyePlants.LimeKelp: {

                                    if (Main.tile[x, y].liquid != byte.MaxValue)
                                        return false;

                                    break;

                                }
							case DyePlants.StrangePlantViolet:
							case DyePlants.StrangePlantOrange:
							case DyePlants.StrangePlantTeal:
							case DyePlants.StrangePlantRed: {

                                    break;

                                }
							default: {

                                    if (Main.tile[x, y].liquid != 0)
                                        return false;

                                    if (style == 3 || style == 4)
                                        if (Main.tile[x, y].wall != 0)
                                            return false;

                                    break;

                                }

						}

						if (!Neo.TileCut(new[] { new Point(x, y), new Point(x, y - 1) }))
							return false;

						break;

                    }

            }

			Neo.SetTile(x, y, 227, ref undo);
			Main.tile[x, y].frameX = (short)(34 * style);
			Main.tile[x, y].frameY = 0;

			return true;

		}

		public static class DyePlants {

			public const int TealMushroom        = 0;
			public const int GreenMushroom       = 1;
			public const int SkyBlueFlower       = 2;
			public const int YellowMarigold      = 3;
			public const int BlueBerries         = 4;
			public const int LimeKelp            = 5;
			public const int PinkPricklyPear     = 6;
			public const int OrangeBloodroot     = 7;
			public const int StrangePlantViolet  = 8;
			public const int StrangePlantOrange  = 9;
			public const int StrangePlantTeal    = 10;
			public const int StrangePlantRed     = 11;
			public const int PricklyPearCorrupt  = 12;
			public const int PricklyPearHallowed = 13;
			public const int PricklyPearCrimson  = 14;

		}

	}

}
