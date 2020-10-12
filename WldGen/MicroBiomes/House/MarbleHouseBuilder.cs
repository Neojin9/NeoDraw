using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class MarbleHouseBuilder : HouseBuilder {

		public MarbleHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Marble, rooms) {

			TileType = TileID.MarbleBlock;
			WallType = WallID.MarbleBlock;
			BeamType = 124; // TODO: After 1.4 update to 561;
			PlatformStyle = 29;
			DoorStyle = 35;
			TableStyle = 34;
			WorkbenchStyle = 30;
			PianoStyle = 29;
			BookcaseStyle = 31;
			ChairStyle = 35;
			ChestStyle = 51;

		}

		protected override void AgeRoom(Rectangle room) {

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.6),
					new Modifiers.Blotches(2, 0.6),
					new Modifiers.OnlyTiles(TileType),
					new Actions.SetTileKeepWall(367, setSelfFrames: true)
				));
			
			Gen(
				new Point(room.X + 1, room.Y),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(0.8),
					new Modifiers.OnlyTiles(367),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y + room.Height - 1),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(0.8),
					new Modifiers.OnlyTiles(367),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.85),
					new Modifiers.Blotches(),
					new Actions.PlaceWall(178)
				)
			);

		}

	}

}
