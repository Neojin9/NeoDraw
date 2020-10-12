using Microsoft.Xna.Framework;
using NeoDraw.UI;
using Terraria;
using Terraria.DataStructures;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.WldUtils { // v1.4 7/23/2020

	public class ActionPlaceStatue : GenAction {

		private int _statueIndex;

		public ActionPlaceStatue(int index = -1) {
			_statueIndex = index;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args) {

			Point16 point = (_statueIndex != -1) ? WorldGen.statueList[_statueIndex] : WorldGen.statueList[_random.Next(2, WorldGen.statueList.Length)];

			PlaceTile(x, y, point.X, ref DrawInterface.Undo, mute: true, forced: false, -1, point.Y);

			return UnitApply(origin, x, y, args);

		}

	}

}
