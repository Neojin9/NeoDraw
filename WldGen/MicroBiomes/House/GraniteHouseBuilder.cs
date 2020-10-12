using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class GraniteHouseBuilder : HouseBuilder {

		public GraniteHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Granite, rooms) {

			TileType = TileID.GraniteBlock;
			WallType = WallID.GraniteBlock;
			BeamType = 124; // TODO: After 1.4 update to 576;
			PlatformStyle = 28;
			DoorStyle = 34;
			TableStyle = 33;
			WorkbenchStyle = 29;
			PianoStyle = 28;
			BookcaseStyle = 30;
			ChairStyle = 34;
			ChestStyle = 50;

		}

		protected override void AgeRoom(Rectangle room) {

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.6),
					new Modifiers.Blotches(2, 0.6),
					new Modifiers.OnlyTiles(TileType),
					new Actions.SetTileKeepWall(368, setSelfFrames: true)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(0.8),
					new Modifiers.OnlyTiles(368),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y + room.Height - 1),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(0.8),
					new Modifiers.OnlyTiles(368),
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
					new Actions.PlaceWall(180)
				)
			);

		}

	}

}
