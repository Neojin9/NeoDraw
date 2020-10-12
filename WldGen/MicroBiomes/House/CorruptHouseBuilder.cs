using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class CorruptHouseBuilder : HouseBuilder {

		public CorruptHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Corrupt, rooms) {

			TileType = TileID.Ebonwood;
			WallType = WallID.Ebonwood;
			BeamType = 124;
			PlatformStyle = 1;
			DoorStyle = 1;
			TableStyle = 1;
			WorkbenchStyle = 1;
			PianoStyle = 1;
			BookcaseStyle = 7;
			ChairStyle = 2;
			ChestStyle = 7;

		}

		protected override void AgeRoom(Rectangle room) { }

	}

}
