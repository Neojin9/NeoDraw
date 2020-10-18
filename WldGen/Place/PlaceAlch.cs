using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen.Place {

    public partial class TilePlacer { // Updated v1.4 8/4/2020 TileCut

		public static bool PlantAlch(int x, int y, ushort age, int style, ref UndoStep undo) {

			// 0 - Daybloom
			// 1 - Moonglow
			// 2 - Blinkroot
			// 3 - Deathweed
			// 4 - Waterleaf
			// 5 - Fireblossom
			// 6 - Shiverthorn

			if (!WorldGen.InWorld(x, y))
				return false;

			if (Main.tile[x, y] == null)
				Main.tile[x, y] = new Tile();

			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();
			
			if (!Main.tile[x, y + 1].nactive())
				return false;

			ushort groundType = Main.tile[x, y + 1].type;

			bool tilePlaced = false;

			switch (style) {

				case Herbs.Daybloom: {

						if (Main.tile[x, y].liquid > 0)
							break;

						if (groundType == TileID.Grass || groundType == TileID.ClayPot || groundType == TileID.HallowedGrass || groundType == TileID.PlanterBox || groundType == 477 || groundType == 492)
							tilePlaced = PlaceAlch(x, y, Herbs.Daybloom, age, ref undo);

						break;

                    }
				case Herbs.Moonglow: {

						if (Main.tile[x, y].liquid > 0)
							break;

						if (groundType == TileID.JungleGrass || groundType == TileID.ClayPot || groundType == TileID.PlanterBox)
							tilePlaced = PlaceAlch(x, y, Herbs.Moonglow, age, ref undo);

						break;

                    }
				case Herbs.Blinkroot: {

						if (Main.tile[x, y].liquid > 0)
							break;

						if (groundType == TileID.Dirt || groundType == TileID.Mud || groundType == TileID.ClayPot || groundType == TileID.PlanterBox)
							tilePlaced = PlaceAlch(x, y, Herbs.Blinkroot, age, ref undo);

						break;

                    }
				case Herbs.Deathweed: {

						if (Main.tile[x, y].liquid > 0)
							break;

						if (groundType == TileID.CorruptGrass || groundType == TileID.Ebonstone || groundType == TileID.ClayPot || groundType == TileID.FleshGrass || groundType == TileID.Crimstone || groundType == TileID.PlanterBox)
							tilePlaced = PlaceAlch(x, y, Herbs.Deathweed, age, ref undo);

						break;

                    }
				case Herbs.Waterleaf: {

						if (Main.tile[x, y].liquid > 0 && Main.tile[x, y].lava())
							break;

						if (groundType == TileID.Sand || groundType == TileID.ClayPot || groundType == TileID.Pearlsand || groundType == TileID.PlanterBox)
							tilePlaced = PlaceAlch(x, y, Herbs.Waterleaf, age, ref undo);

						break;

                    }
				case Herbs.Fireblossom: {

						if (Main.tile[x, y].liquid > 0 && !Main.tile[x, y].lava())
							break;

						if (groundType == TileID.Ash || groundType == TileID.ClayPot || groundType == TileID.PlanterBox)
							tilePlaced = PlaceAlch(x, y, Herbs.Fireblossom, age, ref undo);

						break;

                    }
				case Herbs.Shiverthorn: {

						if (Main.tile[x, y].liquid > 0 && Main.tile[x, y].lava())
							break;

						if (groundType == TileID.ClayPot || groundType == TileID.SnowBlock || groundType == TileID.IceBlock || groundType == TileID.CorruptIce || groundType == TileID.HallowedIce || groundType == TileID.FleshIce || groundType == TileID.PlanterBox)
							tilePlaced = PlaceAlch(x, y, Herbs.Shiverthorn, age, ref undo);

						break;

                    }

            }

			if (tilePlaced && Main.netMode == NetmodeID.Server)
				NetMessage.SendTileSquare(-1, x, y, 1);

			return tilePlaced;

		}

		public static bool PlaceAlch(int x, int y, int style, ushort age, ref UndoStep undo) {

			if (Main.tile[x, y] == null)
				Main.tile[x, y] = new Tile();
			
			if (Main.tile[x, y + 1] == null)
				Main.tile[x, y + 1] = new Tile();
			
			if (Main.tile[x, y + 1].nactive()) {

				switch (style) {

					case Herbs.Daybloom: {

							if (Main.tile[x, y].liquid > 0)
								return false;

							if (Main.tile[x, y + 1].type != 2 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 109 && Main.tile[x, y + 1].type != 380 && Main.tile[x, y + 1].type != 477 && Main.tile[x, y + 1].type != 492)
								return false;

							break;

						}
					case Herbs.Moonglow: {

							if (Main.tile[x, y].liquid > 0)
								return false;

							if (Main.tile[x, y + 1].type != 60 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 380)
								return false;

							break;

						}
					case Herbs.Blinkroot: {

							if (Main.tile[x, y].liquid > 0)
								return false;

							if (Main.tile[x, y + 1].type != 0 && Main.tile[x, y + 1].type != TileID.Mud && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 380)
								return false;

							break;

						}
					case Herbs.Deathweed: {

							if (Main.tile[x, y].liquid > 0)
								return false;

							if (Main.tile[x, y + 1].type != 203 && Main.tile[x, y + 1].type != 199 && Main.tile[x, y + 1].type != 23 && Main.tile[x, y + 1].type != 25 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 380)
								return false;

							break;

						}
					case Herbs.Waterleaf: {

							if (Main.tile[x, y].liquid > 0 && Main.tile[x, y].lava())
								return false;

							if (Main.tile[x, y + 1].type != 53 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 380 && Main.tile[x, y + 1].type != 116)
								return false;

							break;

						}
					case Herbs.Fireblossom: {

							if (Main.tile[x, y].liquid > 0 && !Main.tile[x, y].lava())
								return false;

							if (Main.tile[x, y + 1].type != 57 && Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 380)
								return false;

							break;

						}
					case Herbs.Shiverthorn: {

							if (Main.tile[x, y].liquid > 0 && Main.tile[x, y].lava())
								return false;

							if (Main.tile[x, y + 1].type != 78 && Main.tile[x, y + 1].type != 380 && Main.tile[x, y + 1].type != 147 && Main.tile[x, y + 1].type != 161 && Main.tile[x, y + 1].type != 163 && Main.tile[x, y + 1].type != 164 && Main.tile[x, y + 1].type != 200)
								return false;

							break;

                        }

				}

				if (!Neo.TileCut(x, y))
					return false;

				if (Main.tile[x, y + 1].halfBrick() || Main.tile[x, y + 1].slope() != 0) {
					undo.Add(new ChangedTile(x, y + 1));
					Main.tile[x, y + 1].Clear(TileDataType.Slope);
				}

				undo.Add(new ChangedTile(x, y));

				Main.tile[x, y].active(true);
				Main.tile[x, y].type = age;
				Main.tile[x, y].frameX = (short)(18 * style);
				Main.tile[x, y].frameY = 0;

				return true;

			}

			return false;

		}

		public static class Herbs {

			public const int Daybloom = 0;
			public const int Moonglow = 1;
			public const int Blinkroot = 2;
			public const int Deathweed = 3;
			public const int Waterleaf = 4;
			public const int Fireblossom = 5;
			public const int Shiverthorn = 6;

        }

	}

}
