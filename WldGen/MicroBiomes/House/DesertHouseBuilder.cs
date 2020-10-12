using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class DesertHouseBuilder : HouseBuilder {

		public DesertHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Desert, rooms) {

			TileType = TileID.Sandstone;
			WallType = WallID.Sandstone;
			BeamType = 124; // TODO: After 1.4 update to 577;
			PlatformStyle = 0; // TODO: After 1.4 update to 42;
			DoorStyle = 0; // TODO: After 1.4 update to 43;
			TableStyle = 0; // TODO: After 1.4 update to 7;
			UsesTables2 = false; // TODO: After 1.4 update to true;
			WorkbenchStyle = 0; // TODO: After 1.4 update to 39;
			PianoStyle = 0; // TODO: After 1.4 update to 38;
			BookcaseStyle = 0; // TODO: After 1.4 update to 39;
			ChairStyle = 0; // TODO: After 1.4 update to 43;
			ChestStyle = 1; // TODO: After 1.4 update to 1;

		}

		protected override void AgeRoom(Rectangle room) {

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.8),
					new Modifiers.Blotches(2, 0.2),
					new Modifiers.OnlyTiles(TileType),
					new Actions.SetTileKeepWall(396, setSelfFrames: true),
					new Modifiers.Dither(),
					new Actions.SetTileKeepWall(397, setSelfFrames: true)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(),
					new Modifiers.OnlyTiles(397, 396),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y + room.Height - 1),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(),
					new Modifiers.OnlyTiles(397, 396),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.8),
					new Modifiers.Blotches(),
					new Modifiers.OnlyWalls(WallType),
					new Actions.PlaceWall(216)
				)
			);
			
		}

	}

}
