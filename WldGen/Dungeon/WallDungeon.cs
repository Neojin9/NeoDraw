using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.Undo;
using Terraria;

namespace NeoDraw.WldGen.Dungeon {

    public partial class DungeonBuilder { // Updated v1.4 8/1/2020 Copy/Paste

		public static void WallDungeon(int x, int y, ushort wallType, ref UndoStep undo) {
			
			if (!WorldGen.InWorld(x, y))
				return;

            List<Point>    list    = new List<Point>();
			List<Point>    list2   = new List<Point>();
			HashSet<Point> hashSet = new HashSet<Point>();

			list2.Add(new Point(x, y));

			while (list2.Count > 0) {

				list.Clear();
				list.AddRange(list2);
				list2.Clear();

				while (list.Count > 0) {

					Point item = list[0];

					if (!WorldGen.InWorld(item.X, item.Y, 1)) {
						list.Remove(item);
						continue;
					}

					hashSet.Add(item);
					list.Remove(item);

					Tile tile = Main.tile[item.X, item.Y];

					if (!WorldGen.SolidTile(item.X, item.Y) && tile.wall != wallType && tile.wall > 0 && tile.wall != 244) {

						Neo.SetWall(item.X, item.Y, wallType, ref undo);

						Point item2 = new Point(item.X - 1, item.Y);
						
						if (!hashSet.Contains(item2))
							list2.Add(item2);
						
						item2 = new Point(item.X + 1, item.Y);
						
						if (!hashSet.Contains(item2))
							list2.Add(item2);
						
						item2 = new Point(item.X, item.Y - 1);
						
						if (!hashSet.Contains(item2))
							list2.Add(item2);
						
						item2 = new Point(item.X, item.Y + 1);
						
						if (!hashSet.Contains(item2))
							list2.Add(item2);
						
					}
					else if (tile.active()) {

						Neo.SetWall(item.X, item.Y, wallType, ref undo);

					}

				}

			}

		}

	}

}
