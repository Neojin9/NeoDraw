using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class CrimsonHouseBuilder : HouseBuilder {

		public CrimsonHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Crimson, rooms) {

			TileType = TileID.FleshBlock;
			WallType = WallID.Flesh;
			BeamType = 124;
			PlatformStyle = 34;
			DoorStyle = 5;
			TableStyle = 5;
			WorkbenchStyle = 6;
			PianoStyle = 6;
			BookcaseStyle = 8;
			ChairStyle = 8;
			ChestStyle = 43;

		}

		protected override void AgeRoom(Rectangle room) { }

	}

}
