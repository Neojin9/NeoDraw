using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class WoodHouseBuilder : HouseBuilder {

		public WoodHouseBuilder(IEnumerable<Rectangle> rooms) : base(HouseType.Wood, rooms) {

			TileType = TileID.WoodBlock;
			WallType = WallID.Planked;
			BeamType = 124;
			PlatformStyle = 0;
			DoorStyle = 0;
			TableStyle = 0;
			WorkbenchStyle = 0;
			PianoStyle = 0;
			BookcaseStyle = 0;
			ChairStyle = 0;
			ChestStyle = 1;

		}

		protected override void AgeRoom(Rectangle room) {

			for (int i = 0; i < room.Width * room.Height / 16; i++) {

				int x = WorldGen.genRand.Next(1, room.Width - 1) + room.X;
				int y = WorldGen.genRand.Next(1, room.Height - 1) + room.Y;

				Gen(
					new Point(x, y),
					new Shapes.Rectangle(2, 2),
					Actions.Chain(
						new Modifiers.Dither(),
						new Modifiers.Blotches(2, 2.0),
						new Modifiers.IsEmpty(),
						new Actions.SetTile(51, setSelfFrames: true)
					)
				);

			}

			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.85),
					new Modifiers.Blotches(),
					new Modifiers.OnlyWalls(WallType),
					new Modifiers.SkipTiles(SkipTilesDuringWallAging),
					(room.Y > Main.worldSurface) ? ((GenAction)new Actions.ClearWall(frameNeighbors: true)) : ((GenAction)new Actions.PlaceWall(2))
				)
			);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.Dither(0.95),
					new Modifiers.OnlyTiles(30, 321, 158),
					new Actions.ClearTile(frameNeighbors: true)
				)
			);

		}

	}

}
