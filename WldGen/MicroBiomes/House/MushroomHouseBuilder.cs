using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class MushroomHouseBuilder : HouseBuilder {

		public MushroomHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Mushroom, rooms) {

			TileType = TileID.MushroomBlock;
			WallType = WallID.Mushroom;
			BeamType = 124; // TODO: After 1.4 update to 578;
			PlatformStyle = 18;
			DoorStyle = 6;
			TableStyle = 27;
			WorkbenchStyle = 7;
			PianoStyle = 22;
			BookcaseStyle = 24;
			ChairStyle = 9;
			ChestStyle = 32;

		}

		protected override void AgeRoom(Rectangle room) {

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.7),
					new Modifiers.Blotches(2, 0.5),
					new Modifiers.OnlyTiles(TileType),
					new Actions.SetTileKeepWall(70, setSelfFrames: true)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(0.6),
					new Modifiers.OnlyTiles(70),
					new Modifiers.Offset(0, -1),
					new Modifiers.IsEmpty(),
					new Actions.SetTile(71)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y + room.Height - 1),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(0.6),
					new Modifiers.OnlyTiles(70),
					new Modifiers.Offset(0, -1),
					new Modifiers.IsEmpty(),
					new Actions.SetTile(71)
				)
			);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.85),
					new Modifiers.Blotches(),
					new Actions.ClearWall()
				)
			);

		}

	}

}
