using Microsoft.Xna.Framework;
using NeoDraw.Core;
using Terraria;
using Terraria.ID;

namespace NeoDraw.WldGen {

    public partial class WldGen {

		public static Vector2 HurtTiles(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fireImmune = false) {

            int minX = (int)( Position.X           / 16f) - 1;
			int maxX = (int)((Position.X + Width)  / 16f) + 2;
			int minY = (int)( Position.Y           / 16f) - 1;
			int maxY = (int)((Position.Y + Height) / 16f) + 2;

			Neo.WorldRestrain(ref minX, ref maxX, ref minY, ref maxY);

			Vector2 worldPos = default;

			for (int i = minX; i < maxX; i++) {

				for (int j = minY; j < maxY; j++) {

					int tileType = Main.tile[i, j].type;

					if (Main.tile[i, j] == null || 
						Main.tile[i, j].slope() != 0 || 
						Main.tile[i, j].inActive() || 
						!Main.tile[i, j].active() || 
						(
							tileType != TileID.CorruptThorns && 
							tileType != TileID.Meteorite && 
							tileType != TileID.Spikes && 
							tileType != TileID.WoodenSpikes && 
							tileType != TileID.Sand && 
							tileType != TileID.Ash && 
							tileType != TileID.Hellstone && 
							tileType != TileID.JungleThorns && 
							tileType != TileID.HellstoneBrick && 
							tileType != TileID.Ebonsand && 
							tileType != TileID.Pearlsand && 
							tileType != TileID.Silt && 
							tileType != TileID.Slush && 
							tileType != TileID.Crimsand && 
							tileType != TileID.CrimtaneThorns && 
							tileType != 484
						)
					) {
						continue;
					}

					worldPos.X = i * 16;
					worldPos.Y = j * 16;

					int damage = 0;
					int yOffset = 16;

					if (Main.tile[i, j].halfBrick()) {
						worldPos.Y += 8f;
						yOffset -= 8;
					}

					if (tileType == TileID.CorruptThorns || tileType == TileID.JungleThorns || tileType == TileID.Cactus || tileType == TileID.CrimtaneThorns || (tileType == TileID.Cactus && Main.expertMode)) {

						if (!(Position.X + Width > worldPos.X) || !(Position.X < worldPos.X + 16f) || !(Position.Y + Height > worldPos.Y) || !(Position.Y < worldPos.Y + yOffset + 0.011f))
							continue;
						
						int direction = 1;

						if (Position.X + Width / 2 < worldPos.X + 8f)
							direction = -1;
						
						damage = 10;

						switch (tileType) {

							case TileID.JungleThorns: {
									damage = 17;
									break;
								}
							case TileID.Cactus: {
									damage = 6;
									break;
								}

						}

						if (tileType == TileID.CorruptThorns || tileType == TileID.JungleThorns || tileType == TileID.CrimtaneThorns) {

							WorldGen.KillTile(i, j);

							if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].active() && Main.netMode == NetmodeID.MultiplayerClient)
								NetMessage.SendData(MessageID.TileChange, -1, -1, null, 4, i, j);
							
						}

						return new Vector2(direction, damage);

					}

					if (tileType == TileID.Sand || tileType == TileID.Ebonsand || tileType == TileID.Pearlsand || tileType == TileID.Silt || tileType == TileID.Slush || tileType == TileID.Crimsand) {

						if (Position.X + Width - 2f >= worldPos.X && Position.X + 2f <= worldPos.X + 16f && Position.Y + Height - 2f >= worldPos.Y && Position.Y + 2f <= worldPos.Y + yOffset) {

							int direction = 1;

							if (Position.X + Width / 2 < worldPos.X + 8f)
								direction = -1;

							damage = 0; // 15 Changed to allow teleporting onto sand;

							return new Vector2(direction, damage);

						}

					}
					else if (Position.X + Width >= worldPos.X && Position.X <= worldPos.X + 16f && Position.Y + Height >= worldPos.Y && Position.Y <= worldPos.Y + yOffset + 0.011f) {

						int direction = 1;

						if (Position.X + Width / 2 < worldPos.X + 8f)
							direction = -1;

						if (!fireImmune && (tileType == TileID.Meteorite || tileType == TileID.Hellstone || tileType == TileID.HellstoneBrick))
							damage = 20;
						
						switch (tileType) {

							case TileID.Spikes: {
									damage = 60;
									break;
                                }
							case TileID.WoodenSpikes: {
									damage = 80;
									break;
                                }
							case 484: {
									damage = 25;
									break;
                                }

                        }

						return new Vector2(direction, damage);

					}

				}

			}

			return default;

		}

	}

}
