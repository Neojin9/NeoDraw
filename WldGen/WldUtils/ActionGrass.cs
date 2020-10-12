using Microsoft.Xna.Framework;
using NeoDraw.UI;
using Terraria;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.WldUtils { // v1.4 7/23/2020

	public class ActionGrass : GenAction {

		public override bool Apply(Point origin, int x, int y, params object[] args) {

			if (_tiles[x, y].active() || _tiles[x, y - 1].active())
				return false;
			
			PlaceTile(x, y, Utils.SelectRandom(_random, new ushort[2] { 3, 73}), ref DrawInterface.Undo, mute: true);

			return UnitApply(origin, x, y, args);

		}

	}

}
