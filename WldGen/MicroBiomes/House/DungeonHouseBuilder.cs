using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class BlueDungeonHouseBuilder : HouseBuilder {

		public BlueDungeonHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Wood, rooms) {

			TileType = TileID.BlueDungeonBrick;
			WallType = WallID.BlueDungeon;
			BeamType = 124;
			PlatformStyle = 6;
			DoorStyle = 16;
			TableStyle = 10;
			WorkbenchStyle = 11;
			PianoStyle = 11;
			BookcaseStyle = 1;
			ChairStyle = 13;
			ChestStyle = 39;

		}

		protected override void AgeRoom(Rectangle room) { }

	}

}
