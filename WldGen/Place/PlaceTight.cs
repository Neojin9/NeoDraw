using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated 8/9/2020

		public static bool PlaceTight(int x, int y, ref UndoStep undo, ushort type = TileID.Stalactite, bool spiders = false, int height = Stalactitie.Default) {

			if (!WorldGen.InWorld(x, y))
				return false;

			UnifiedRandom genRand = WorldGen.genRand;

			int variation = genRand.Next(3);

			if (Main.tile[x, y - 1] == null)
				Main.tile[x, y - 1] = new Tile();
			
			if (Main.tile[x, y] == null)
				Main.tile[x, y] = new Tile();
			
			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();

			bool tilePlaced = false;

			if (WorldGen.SolidTile(x, y - 1) && !Main.tile[x, y].active() && !Main.tile[x, y + 1].active()) {

				if (spiders) {

					int frameX = 108 + variation * 18;

					undo.Add(new ChangedTile(x, y));
					undo.Add(new ChangedTile(x, y + 1));

					Main.tile[x, y].type = type;
					Main.tile[x, y].active(active: true);
					Main.tile[x, y].frameX = (short)frameX;
					Main.tile[x, y].frameY = 0;

					Main.tile[x, y + 1].type = type;
					Main.tile[x, y + 1].active(active: true);
					Main.tile[x, y + 1].frameX = (short)frameX;
					Main.tile[x, y + 1].frameY = 18;

					tilePlaced = true;

				}
				else {

					if (Main.tile[x, y - 1].type == TileID.SnowBlock || Main.tile[x, y - 1].type == TileID.IceBlock || Main.tile[x, y - 1].type == TileID.CorruptIce || Main.tile[x, y - 1].type == TileID.HallowedIce || Main.tile[x, y - 1].type == TileID.FleshIce) {

						int frameX = variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 72;

						}
						else {

							undo.Add(new ChangedTile(x, y));
							undo.Add(new ChangedTile(x, y + 1));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 0;

							Main.tile[x, y + 1].type = type;
							Main.tile[x, y + 1].active(active: true);
							Main.tile[x, y + 1].frameX = (short)frameX;
							Main.tile[x, y + 1].frameY = 18;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y - 1].type == TileID.Stone || Main.tileMoss[Main.tile[x, y - 1].type] || Main.tile[x, y - 1].type == TileID.Pearlstone || Main.tile[x, y - 1].type == TileID.Ebonstone || Main.tile[x, y - 1].type == TileID.Crimstone) {

						int frameX = 54 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 72;

						}
						else {

							undo.Add(new ChangedTile(x, y));
							undo.Add(new ChangedTile(x, y + 1));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 0;

							Main.tile[x, y + 1].type = type;
							Main.tile[x, y + 1].active(active: true);
							Main.tile[x, y + 1].frameX = (short)frameX;
							Main.tile[x, y + 1].frameY = 18;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y - 1].type == TileID.Hive) {

						int frameX = 162 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)frameX;
						Main.tile[x, y].frameY = 72;

						tilePlaced = true;

					}

					if (Main.tile[x, y - 1].type == TileID.Sandstone || Main.tile[x, y - 1].type == TileID.HardenedSand) {

						int frameX = 378 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 72;

						}
						else {

							undo.Add(new ChangedTile(x, y));
							undo.Add(new ChangedTile(x, y + 1));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 0;

							Main.tile[x, y + 1].type = type;
							Main.tile[x, y + 1].active(active: true);
							Main.tile[x, y + 1].frameX = (short)frameX;
							Main.tile[x, y + 1].frameY = 18;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y - 1].type == TileID.Granite) {

						int frameX = 432 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 72;

						}
						else {

							undo.Add(new ChangedTile(x, y));
							undo.Add(new ChangedTile(x, y + 1));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 0;

							Main.tile[x, y + 1].type = type;
							Main.tile[x, y + 1].active(active: true);
							Main.tile[x, y + 1].frameX = (short)frameX;
							Main.tile[x, y + 1].frameY = 18;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y - 1].type == TileID.Marble) {

						int frameX = 486 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 72;

						}
						else {

							undo.Add(new ChangedTile(x, y));
							undo.Add(new ChangedTile(x, y + 1));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 0;

							Main.tile[x, y + 1].type = type;
							Main.tile[x, y + 1].active(active: true);
							Main.tile[x, y + 1].frameX = (short)frameX;
							Main.tile[x, y + 1].frameY = 18;

						}

						tilePlaced = true;

					}

				}

			}
			else {

				if (spiders)
					return false;
				
				if (WorldGen.SolidTile(x, y + 1) && !Main.tile[x, y].active() && !Main.tile[x, y - 1].active()) {

					if (Main.tile[x, y + 1].type == TileID.Stone || Main.tileMoss[Main.tile[x, y + 1].type] || Main.tile[x, y - 1].type == TileID.Pearlstone || Main.tile[x, y - 1].type == TileID.Ebonstone || Main.tile[x, y - 1].type == TileID.Crimstone) {

						int frameX = 54 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 90;

						}
						else {

							undo.Add(new ChangedTile(x, y - 1));
							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y - 1].type = type;
							Main.tile[x, y - 1].active(active: true);
							Main.tile[x, y - 1].frameX = (short)frameX;
							Main.tile[x, y - 1].frameY = 36;

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 54;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y + 1].type == TileID.Hive) {

						int frameX = 162 + variation * 18;

						undo.Add(new ChangedTile(x, y));

						Main.tile[x, y].type = type;
						Main.tile[x, y].active(active: true);
						Main.tile[x, y].frameX = (short)frameX;
						Main.tile[x, y].frameY = 90;

						tilePlaced = true;

					}

					if (Main.tile[x, y + 1].type == TileID.Sandstone || Main.tile[x, y + 1].type == TileID.HardenedSand) {

						int frameX = 378 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 90;

						}
						else {

							undo.Add(new ChangedTile(x, y - 1));
							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y - 1].type = type;
							Main.tile[x, y - 1].active(active: true);
							Main.tile[x, y - 1].frameX = (short)frameX;
							Main.tile[x, y - 1].frameY = 36;

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 54;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y + 1].type == TileID.Granite) {

						int frameX = 432 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 90;

						}
						else {

							undo.Add(new ChangedTile(x, y - 1));
							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y - 1].type = type;
							Main.tile[x, y - 1].active(active: true);
							Main.tile[x, y - 1].frameX = (short)frameX;
							Main.tile[x, y - 1].frameY = 36;

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 54;

						}

						tilePlaced = true;

					}

					if (Main.tile[x, y + 1].type == TileID.Marble) {

						int frameX = 486 + variation * 18;

						if ((height == Stalactitie.Default && genRand.Next(2) == 0) || height == Stalactitie.Short) {

							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 90;

						}
						else {

							undo.Add(new ChangedTile(x, y - 1));
							undo.Add(new ChangedTile(x, y));

							Main.tile[x, y - 1].type = type;
							Main.tile[x, y - 1].active(active: true);
							Main.tile[x, y - 1].frameX = (short)frameX;
							Main.tile[x, y - 1].frameY = 36;

							Main.tile[x, y].type = type;
							Main.tile[x, y].active(active: true);
							Main.tile[x, y].frameX = (short)frameX;
							Main.tile[x, y].frameY = 54;

						}

						tilePlaced = true;

					}

				}

			}

			if (Main.tile[x, y].type == TileID.Stalactite)
				WorldGen.CheckTight(x, y);

			return tilePlaced;

		}

	}

	public struct Stalactitie {

		public const int Default = -1;
		public const int Tall = 0;
		public const int Short = 1;

    }

}
