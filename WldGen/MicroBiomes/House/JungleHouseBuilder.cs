using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class JungleHouseBuilder : HouseBuilder {

		public JungleHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Jungle, rooms) {

			TileType = TileID.RichMahogany;
			WallType = 42;
			BeamType = 124; // TODO: After 1.4 update to 575;
			PlatformStyle = 2;
			DoorStyle = 2;
			TableStyle = 2;
			WorkbenchStyle = 2;
			PianoStyle = 2;
			BookcaseStyle = 12;
			ChairStyle = 3;
			ChestStyle = 8;

		}

		protected override void AgeRoom(Rectangle room) {

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.6),
					new Modifiers.Blotches(2, 0.6),
					new Modifiers.OnlyTiles(TileType),
					new Actions.SetTileKeepWall(60, setSelfFrames: true),
					new Modifiers.Dither(0.8),
					new Actions.SetTileKeepWall(59, setSelfFrames: true)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(),
					new Modifiers.OnlyTiles(60),
					new Modifiers.Offset(0, 1),
					new Modifiers.IsEmpty(),
					new ActionVines(3, room.Height, 62)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y + room.Height - 1),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(),
					new Modifiers.OnlyTiles(60),
					new Modifiers.Offset(0, 1),
					new Modifiers.IsEmpty(),
					new ActionVines(3, room.Height, 62)
				)
			);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.85),
					new Modifiers.Blotches(),
					new Actions.PlaceWall(64)
				)
			);

		}
	}
}
