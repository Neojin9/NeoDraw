using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class IceHouseBuilder : HouseBuilder {

		public IceHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Ice, rooms) {

			TileType = TileID.BorealWood;
			WallType = WallID.BorealWood;
			BeamType = 124; // TODO: After 1.4 update to 574;
			DoorStyle = 30;
			PlatformStyle = 19;
			TableStyle = 28;
			WorkbenchStyle = 23;
			PianoStyle = 23;
			BookcaseStyle = 25;
			ChairStyle = 30;
			ChestStyle = 11;

		}

		protected override void AgeRoom(Rectangle room) {

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.6),
					new Modifiers.Blotches(2, 0.6),
					new Modifiers.OnlyTiles(TileType),
					new Actions.SetTileKeepWall(161, setSelfFrames: true),
					new Modifiers.Dither(0.8),
					new Actions.SetTileKeepWall(147, setSelfFrames: true)
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(),
					new Modifiers.OnlyTiles(161),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X + 1, room.Y + room.Height - 1),
				new Shapes.Rectangle(room.Width - 2, 1),
				Actions.Chain(
					new Modifiers.Dither(),
					new Modifiers.OnlyTiles(161),
					new Modifiers.Offset(0, 1),
					new ActionStalagtite()
				)
			);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.85),
					new Modifiers.Blotches(2, 0.8),
					new Modifiers.SkipTiles(SkipTilesDuringWallAging),
					(room.Y > Main.worldSurface) ? ((GenAction)new Actions.ClearWall(frameNeighbors: true)) : ((GenAction)new Actions.PlaceWall(40))
				)
			);

		}

	}

}
