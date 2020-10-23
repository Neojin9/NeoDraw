using System;
using Microsoft.Xna.Framework;
using NeoDraw.UI;
using Terraria;
using Terraria.ModLoader;

namespace NeoDraw.WldGen {

    public partial class WldGen { // Updated v1.4 7/25/2020

		public static bool EmptyTileCheck(int startX, int endX, int startY, int endY, int ignoreID = -1, bool ignoreCut = false) {
			
			if (!WorldGen.InWorld(startX, startY) || !WorldGen.InWorld(endX, endY))
				return false;

			for (int i = startX; i < endX + 1; i++) {

				for (int j = startY; j < endY + 1; j++) {
					DrawInterface.InvalidTiles.Add(new Tuple<Point, int>(new Point(i, j), 25));
					if (!Main.tile[i, j].active())
						continue;

					ushort type = Main.tile[i, j].type;

					if (ignoreCut && Main.tileCut[type])
						continue;

					if (ignoreID != -1 && type == ignoreID)
						continue;

					//if (!TileLoader.IsSapling(ignoreID))
					//	continue;
					
					if (!TileLoader.IsSapling(type)) {

						switch (type) {

							case   3:
							case  24:
							case  32:
							case  61:
							case  62:
							case  69:
							case  71:
							case  73:
							case  74:
							case  82:
							case  83:
							case  84:
							case 110:
							case 113:
							case 201:
							case 233:
							case 352:
							case 485:
							case 529:
							case 530:
								continue;

						}

						return false;

					}

				}

			}

			return true;

		}

	}

}
