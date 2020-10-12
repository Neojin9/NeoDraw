using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/1/2020 Copy/Paste

		public static bool DungeonPitTrap(int i, int j, ushort tileType, ushort wallType, ref UndoStep undo) {

			UnifiedRandom genRand = WorldGen.genRand;

			int num  = 30;
			int num2 = j;
			int num3 = num2;
			int num4 = genRand.Next(8, 19);
			int num5 = genRand.Next(19, 46);
			int num6 = num4 + genRand.Next(6, 10);
			int num7 = num5 + genRand.Next(6, 10);

			if (!Main.wallDungeon[Main.tile[i, num2].wall])
				return false;
			
			if (Main.tile[i, num2].active())
				return false;
			
			for (int k = num2; k < Main.maxTilesY; k++) {

				if (k > Main.maxTilesY - 300)
					return false;
				
				if (Main.tile[i, k].active() && WorldGen.SolidTile(i, k)) {

					if (Main.tile[i, k].type == TileID.Spikes)
						return false;
					
					num2 = k;

					break;

				}

			}

			if (!Main.wallDungeon[Main.tile[i - num4, num2].wall] || !Main.wallDungeon[Main.tile[i + num4, num2].wall])
				return false;
			
			for (int l = num2; l < num2 + num; l++) {

				bool flag = true;

				for (int m = i - num4; m <= i + num4; m++)
                    if (Main.tile[m, l].active() && Main.tileDungeon[Main.tile[m, l].type])
						flag = false;
					
				if (flag) {

					num2 = l;

					break;

				}

			}

			for (int n = i - num4; n <= i + num4; n++)
				for (int num8 = num2; num8 <= num2 + num5; num8++)
                    if (Main.tile[n, num8].active() && (Main.tileDungeon[Main.tile[n, num8].type] || Main.tile[n, num8].type == CrackedType))
						return false;
					
			bool flag2 = false;

			if (dungeonLake) {
				flag2 = true;
				dungeonLake = false;
			}
			else if (genRand.Next(8) == 0) {
				flag2 = true;
			}

			for (int num9 = i - num4; num9 <= i + num4; num9++)
				for (int num10 = num3; num10 <= num2 + num5; num10++)
					if (Main.tileDungeon[Main.tile[num9, num10].type])
						Neo.SetTileWall(num9, num10, CrackedType, wallType, ref undo);

			for (int num11 = i - num6; num11 <= i + num6; num11++) {

				for (int num12 = num3; num12 <= num2 + num7; num12++) {

					Neo.SetLiquid(num11, num12, 0, ref undo, false);

					if (!Main.wallDungeon[Main.tile[num11, num12].wall] && Main.tile[num11, num12].type != CrackedType) {

						Neo.SetTile(num11, num12, tileType);

						if (num11 > i - num6 && num11 < i + num6 && num12 < num2 + num7)
							Neo.SetWall(num11, num12, wallType);

					}

				}

			}

			for (int num13 = i - num4; num13 <= i + num4; num13++) {

				for (int num14 = num3; num14 <= num2 + num5; num14++) {

					if (Main.tile[num13, num14].type != CrackedType) {

						undo.Add(new ChangedTile(num13, num14));

						if (flag2)
							Neo.SetLiquid(num13, num14, byte.MaxValue);

						if (num13 == i - num4 || num13 == i + num4 || num14 == num2 + num5) {
							Neo.SetTile(num13, num14, TileID.Spikes);
						}
						else if ((num13 == i - num4 + 1 && num14 % 2 == 0) || (num13 == i + num4 - 1 && num14 % 2 == 0) || (num14 == num2 + num5 - 1 && num13 % 2 == 0)) {
							Neo.SetTile(num13, num14, TileID.Spikes);
						}
						else {
							Neo.SetActive(num13, num14, false);
						}

					}

				}

			}

			return true;

		}

	}

}
