using Microsoft.Xna.Framework;
using NeoDraw.UI;
using static NeoDraw.WldGen.Place.TilePlacer;

namespace NeoDraw.WldGen.WldUtils { // v1.4 7/23/2020

	public class ActionStalagtite : GenAction {

		public override bool Apply(Point origin, int x, int y, params object[] args) {

			PlaceTight(x, y, ref DrawInterface.Undo);
			return UnitApply(origin, x, y, args);

		}

	}

}
