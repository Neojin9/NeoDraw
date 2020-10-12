using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using static NeoDraw.WldGen.Place.TilePlacer;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public class HouseBuilder {

		public static readonly HouseBuilder Invalid = new HouseBuilder();

		public readonly HouseType Type;

		public readonly bool BathroomCreated;
		public readonly bool IsValid;

		protected ushort[] SkipTilesDuringWallAging = new ushort[5] {
			TileID.Painting3X3,
			TileID.Painting4X3,
			TileID.Painting6X4,
			TileID.Painting2X3,
			TileID.Painting3X2
		};

		public bool PlaceSharpener;
		public bool PlaceExtractinator;

		public float ChestChance { get; set; }

		public bool UsesTables2 { get; protected set; }

		#region Construction Material

		public ushort TileType { get; protected set; }

		public ushort WallType { get; protected set; }

		public ushort BeamType { get; protected set; }

		#endregion

		#region Furniture Style

		public int BathtubStyle { get; protected set; }

		public int BedStyle { get; protected set; }

		public int BenchStyle { get; protected set; }

		public int BookcaseStyle { get; protected set; }

		public int CandelabraStyle { get; protected set; }

		public int CandleStyle { get; protected set; }

		public int ChairStyle { get; protected set; }

		public int ChandelierStyle { get; protected set; }

		public int ChestStyle { get; protected set; }

		public int ClockStyle { get; protected set; }

		public int DoorStyle { get; protected set; }

		public int DresserStyle { get; protected set; }

		public int LampStyle { get; protected set; }

		public int LanternStyle { get; protected set; }

		public int PianoStyle { get; protected set; }

		public int PlatformStyle { get; protected set; }

		public int SinkStyle { get; protected set; }

		public int SofaStyle { get; protected set; }

		public int TableStyle { get; protected set; }

		public int ToiletStyle { get; protected set; }

		public int WorkbenchStyle { get; protected set; }

        #endregion

		public ReadOnlyCollection<Rectangle> Rooms { get; private set; }

		public Rectangle TopRoom => Rooms.First();

		public Rectangle BottomRoom => Rooms.Last();

		private UnifiedRandom _random => WorldGen.genRand;

		private Tile[,] _tiles => Main.tile;

		private HouseBuilder() {
			IsValid = false;
		}

		protected HouseBuilder(HouseType type, IEnumerable<Rectangle> rooms) {

			Type = type;

			IsValid = true;

			List<Rectangle> list = rooms.ToList();

			list.Sort((Rectangle lhs, Rectangle rhs) => lhs.Top.CompareTo(rhs.Top));

			Rooms = list.AsReadOnly();

		}

		protected virtual void AgeRoom(Rectangle room) { }

		public void Place(HouseBuilderContext context, StructureMap structures) {

			PlaceEmptyRooms();

			foreach (Rectangle room in Rooms)
				structures.AddProtectedStructure(room, 8);

			PlaceStairs();
			PlaceDoors();
			PlacePlatforms();
			PlaceSupportBeams();
			FillRooms();

			foreach (Rectangle room2 in Rooms)
				AgeRoom(room2);
			
			PlaceChests();
			PlaceBiomeSpecificTool(context);

			structures.Reset();

		}

		private void PlaceEmptyRooms() {

			foreach (Rectangle room in Rooms) {

				Gen(
					new Point(room.X, room.Y),
					new Shapes.Rectangle(room.Width, room.Height),
					Actions.Chain(
						new Actions.SetTileKeepWall(TileType),
						new Actions.SetFrames(frameNeighbors: true)
					)
				);
				
				Gen(
					new Point(room.X + 1, room.Y + 1),
					new Shapes.Rectangle(room.Width - 2, room.Height - 2),
					Actions.Chain(
						new Actions.ClearTile(frameNeighbors: true),
						new Actions.PlaceWall(WallType)
					)
				);

			}

		}

		private void FillRooms() {

			int tableStyle = TileID.Tables;

			if (UsesTables2)
				tableStyle = TileID.Tables2;
			
			Point[] choices = new Point[7] {

				new Point(tableStyle, TableStyle),
				new Point(TileID.Anvils, 0),
				new Point(TileID.WorkBenches, WorkbenchStyle),
				new Point(TileID.Loom, 0),
				new Point(TileID.Pianos, PianoStyle),
				new Point(TileID.Kegs, 0),
				new Point(TileID.Bookcases, BookcaseStyle)

			};

			foreach (Rectangle room in Rooms) {

				int num  = room.Width / 8;
				int num2 = room.Width / (num + 1);
				int num3 = _random.Next(2);
				
				for (int i = 0; i < num; i++) {

					int num4 = (i + 1) * num2 + room.X;

					switch (i + num3 % 2) {

						case 0: {

								int num5 = room.Y + Math.Min(room.Height / 2, room.Height - 5);

								//Vector2 vector = (Type != HouseType.Desert) ? WorldGen.randHousePicture() : RandHousePictureDesert(); // TODO: Uncomment for v1.4
								Vector2 vector = WorldGen.randHousePicture();

								int type = (int)vector.X;
								int style = (int)vector.Y;
								
								PlaceTile(num4, num5, type, ref DrawInterface.Undo, mute: true, forced: false, -1, style);
								
								break;

							}
						case 1: {

								int num5 = room.Y + 1;

								PlaceTile(num4, num5, TileID.Chandeliers, ref DrawInterface.Undo, mute: true, forced: false, -1, _random.Next(6));
								
								for (int j = -1; j < 2; j++)
									for (int k = 0; k < 3; k++)
										_tiles[j + num4, k + num5].frameX += 54;
									
								break;

							}

					}

				}

				int roomCapacity = room.Width / 8 + 3;

				WorldGen.SetupStatueList();

				while (roomCapacity > 0) {

					int randomX = _random.Next(room.Width - 3) + 1 + room.X;
					int randomY = room.Y + room.Height - 2;

					switch (_random.Next(4)) {

						case 0: { // Place small pile.
								
								PlaceSmallPile(randomX, randomY, _random.Next(31, 34), 1, ref DrawInterface.Undo, TileID.SmallPiles);
								
								break;

							}
						case 1: { // Place Large Pile
								
								PlaceTile(randomX, randomY, TileID.LargePiles, ref DrawInterface.Undo, mute: true, forced: false, -1, _random.Next(22, 26));
								
								break;

							}
						case 2: { // Place statue.

								int num9 = _random.Next(2, WorldGen.statueList.Length);

								PlaceTile(randomX, randomY, WorldGen.statueList[num9].X, ref DrawInterface.Undo, mute: true, forced: false, -1, WorldGen.statueList[num9].Y);
								
								if (WorldGen.StatuesWithTraps.Contains(num9))
									PlaceStatueTrap(randomX, randomY, ref DrawInterface.Undo);
								
								break;

							}
						case 3: { // Place furniture.

								Point point = Utils.SelectRandom(_random, choices);
								PlaceTile(randomX, randomY, point.X, ref DrawInterface.Undo, mute: true, forced: false, -1, point.Y);
								
								break;

							}

					}

					roomCapacity--;

				}

			}

		}

		private void PlaceStairs() {

			foreach (Tuple<Point, Point> item3 in CreateStairsList()) {

				Point item  = item3.Item1;
				Point item2 = item3.Item2;

				int num = (item2.X > item.X) ? 1 : (-1);
				
				ShapeData shapeData = new ShapeData();
				
				for (int i = 0; i < item2.Y - item.Y; i++)
					shapeData.Add(num * (i + 1), i);
				
				Gen(
					item,
					new ModShapes.All(shapeData),
					Actions.Chain(
						new Actions.PlaceTile(TileID.Platforms, PlatformStyle),
						new Actions.SetSlope((num == 1) ? 1 : 2),
						new Actions.SetFrames(frameNeighbors: true)
					)
				);
				
				Gen(
					new Point(item.X + ((num == 1) ? 1 : (-4)), item.Y - 1),
					new Shapes.Rectangle(4, 1),
					Actions.Chain(
						new Actions.Clear(),
						new Actions.PlaceWall(WallType),
						new Actions.PlaceTile(TileID.Platforms, PlatformStyle),
						new Actions.SetFrames(frameNeighbors: true)
					)
				);

			}

		}

		private List<Tuple<Point, Point>> CreateStairsList() {

			List<Tuple<Point, Point>> list = new List<Tuple<Point, Point>>();

			for (int i = 1; i < Rooms.Count; i++) {

				Rectangle rectangle  = Rooms[i];
				Rectangle rectangle2 = Rooms[i - 1];
				
				int num  = rectangle2.X - rectangle.X;
				int num2 = rectangle.X + rectangle.Width - (rectangle2.X + rectangle2.Width);
				
				if (num > num2) {
					list.Add(new Tuple<Point, Point>(new Point(rectangle.X + rectangle.Width - 1, rectangle.Y + 1), new Point(rectangle.X + rectangle.Width - rectangle.Height + 1, rectangle.Y + rectangle.Height - 1)));
				}
				else {
					list.Add(new Tuple<Point, Point>(new Point(rectangle.X, rectangle.Y + 1), new Point(rectangle.X + rectangle.Height - 1, rectangle.Y + rectangle.Height - 1)));
				}

			}

			return list;

		}

		private void PlaceDoors() {

			foreach (Point item in CreateDoorList()) {

				Gen(
					item,
					new Shapes.Rectangle(1, 3),
					new Actions.ClearTile(frameNeighbors: true)
				);
				
				PlaceTile(item.X, item.Y, TileID.ClosedDoor, ref DrawInterface.Undo, mute: true, forced: true, -1, DoorStyle);

			}

		}

		private List<Point> CreateDoorList() {

			List<Point> list = new List<Point>();
			
			foreach (Rectangle room in Rooms) {

				if (FindSideExit(new Rectangle(room.X + room.Width, room.Y + 1, 1, room.Height - 2), isLeft: false, out int exitY))
					list.Add(new Point(room.X + room.Width - 1, exitY));
				
				if (FindSideExit(new Rectangle(room.X, room.Y + 1, 1, room.Height - 2), isLeft: true, out exitY))
					list.Add(new Point(room.X, exitY));
				
			}

			return list;

		}

		private void PlacePlatforms() {

			foreach (Point item in CreatePlatformsList()) {

				Gen(
					item,
					new Shapes.Rectangle(3, 1),
					Actions.Chain(
						new Actions.ClearMetadata(),
						new Actions.PlaceTile(TileID.Platforms, PlatformStyle),
						new Actions.SetFrames(frameNeighbors: true)
					)
				);

			}

		}

		private List<Point> CreatePlatformsList() {

			List<Point> list = new List<Point>();
			
			Rectangle topRoom    = TopRoom;
			Rectangle bottomRoom = BottomRoom;
			
			if (FindVerticalExit(new Rectangle(topRoom.X + 2, topRoom.Y, topRoom.Width - 4, 1), isUp: true, out int exitX))
				list.Add(new Point(exitX, topRoom.Y));
			
			if (FindVerticalExit(new Rectangle(bottomRoom.X + 2, bottomRoom.Y + bottomRoom.Height - 1, bottomRoom.Width - 4, 1), isUp: false, out exitX))
				list.Add(new Point(exitX, bottomRoom.Y + bottomRoom.Height - 1));
			
			return list;

		}

		private void PlaceSupportBeams() {

			foreach (Rectangle item in CreateSupportBeamList()) {

				if (item.Height > 1 && _tiles[item.X, item.Y - 1].type != TileID.Platforms) {

					Gen(
						new Point(item.X, item.Y),
						new Shapes.Rectangle(item.Width, item.Height),
						Actions.Chain(
							new Actions.SetTileKeepWall(BeamType),
							new Actions.SetFrames(frameNeighbors: true)
						)
					);

					_tiles[item.X, item.Y + item.Height].Clear(TileDataType.Slope);

					//Tile tile = _tiles[item.X, item.Y + item.Height];
					//
					//tile.slope(0);
					//tile.halfBrick(halfBrick: false);

				}

			}

		}

		private List<Rectangle> CreateSupportBeamList() {

			List<Rectangle> list = new List<Rectangle>();

			int num  = Rooms.Min((Rectangle room) => room.Left);
			int num2 = Rooms.Max((Rectangle room) => room.Right) - 1;
			int num3 = 6;
			
			while (num3 > 4 && (num2 - num) % num3 != 0)
				num3--;
			
			for (int i = num; i <= num2; i += num3) {

				for (int j = 0; j < Rooms.Count; j++) {

					Rectangle rectangle = Rooms[j];

					if (i < rectangle.X || i >= rectangle.X + rectangle.Width)
						continue;
					
					int num4 = rectangle.Y + rectangle.Height;
					int num5 = 50;

					for (int k = j + 1; k < Rooms.Count; k++)
						if (i >= Rooms[k].X && i < Rooms[k].X + Rooms[k].Width)
							num5 = Math.Min(num5, Rooms[k].Y - num4);
						
					if (num5 > 0) {

						Point result;
						
						bool flag = Find(
										new Point(i, num4),
										Searches.Chain(
											new Searches.Down(num5),
											new Conditions.IsSolid()
										),
										out result
									);
						
						if (num5 < 50) {
							flag = true;
							result = new Point(i, num4 + num5);
						}

						if (flag)
							list.Add(new Rectangle(i, num4, 1, result.Y - num4));
						
					}

				}

			}

			return list;

		}

		private static bool FindVerticalExit(Rectangle wall, bool isUp, out int exitX) {

			Point result;
			
			bool result2 = Find(
								new Point(wall.X + wall.Width - 3, wall.Y + (isUp ? (-5) : 0)),
								Searches.Chain(
									new Searches.Left(wall.Width - 3),
									new Conditions.IsSolid().Not().AreaOr(3, 5)
								),
								out result
							);
			
			exitX = result.X;
			
			return result2;

		}

		private static bool FindSideExit(Rectangle wall, bool isLeft, out int exitY) {

			Point result;

			bool result2 = Find(
								new Point(wall.X + (isLeft ? (-4) : 0), wall.Y + wall.Height - 3),
								Searches.Chain(
									new Searches.Up(wall.Height - 3),
									new Conditions.IsSolid().Not().AreaOr(4, 3)
								),
								out result
							);
			
			exitY = result.Y;
			
			return result2;

		}

		private void PlaceChests() {

			if (_random.NextFloat() > ChestChance)
				return;
			
			bool flag = false;

			foreach (Rectangle room in Rooms) {

				int num = room.Height - 1 + room.Y;
				int style = (num > (int)Main.worldSurface) ? ChestStyle : 0;
				
				for (int i = 0; i < 10; i++)
					if (flag = WldGen.AddBuriedChest(_random.Next(2, room.Width - 2) + room.X, num, ref DrawInterface.Undo, 0, notNearOtherChests: false, style, trySlope: false, 0))
						break;
					
				if (flag)
					break;
				
				for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
					if (flag = WldGen.AddBuriedChest(j, num, ref DrawInterface.Undo, 0, notNearOtherChests: false, style, trySlope: false, 0))
						break;
					
				if (flag)
					break;
				
			}

			if (!flag) {

				foreach (Rectangle room2 in Rooms) {

					int num2 = room2.Y - 1;
					int style2 = (num2 > (int)Main.worldSurface) ? ChestStyle : 0;
					
					for (int k = 0; k < 10; k++)
						if (flag = WldGen.AddBuriedChest(_random.Next(2, room2.Width - 2) + room2.X, num2, ref DrawInterface.Undo, 0, notNearOtherChests: false, style2, trySlope: false, 0))
							break;
						
					if (flag)
						break;
					
					for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
						if (flag = WldGen.AddBuriedChest(l, num2, ref DrawInterface.Undo, 0, notNearOtherChests: false, style2, trySlope: false, 0))
							break;
						
					if (flag)
						break;
					
				}

			}

			if (flag)
				return;
			
			for (int m = 0; m < 1000; m++) {

				int i2   = _random.Next(Rooms[0].X - 30, Rooms[0].X + 30);
				int num3 = _random.Next(Rooms[0].Y - 30, Rooms[0].Y + 30);

				int style3 = (num3 > (int)Main.worldSurface) ? ChestStyle : 0;

				if (flag = WldGen.AddBuriedChest(i2, num3, ref DrawInterface.Undo, 0, notNearOtherChests: false, style3, trySlope: false, 0))
					break;
				
			}

		}

		private void PlaceBiomeSpecificTool(HouseBuilderContext context) {

			if ((Type == HouseType.Jungle && _random.Next(5) == 0) || PlaceSharpener) {

				bool tilePlaced = false;

				foreach (Rectangle room in Rooms) {

					int floor = room.Height - 2 + room.Y;

					for (int attempts = 0; attempts < 10; attempts++) {

						int xPos = _random.Next(2, room.Width - 2) + room.X;

						PlaceTile(xPos, floor, TileID.SharpeningStation, ref DrawInterface.Undo, mute: true, forced: true);
						
						if (tilePlaced = (_tiles[xPos, floor].active() && _tiles[xPos, floor].type == 377))
							break;
						
					}

					if (tilePlaced)
						break;
					
					for (int xPos = room.X + 2; xPos <= room.X + room.Width - 2; xPos++)
						if (tilePlaced = PlaceTile(xPos, floor, TileID.SharpeningStation, ref DrawInterface.Undo, mute: true, forced: true))
							break;
						
					if (tilePlaced)
						break;
					
				}

			}

			if ((Type == HouseType.Desert && _random.Next(5) == 0) || PlaceExtractinator) {

				bool tilePlaced = false;

				foreach (Rectangle room in Rooms) {

					int floor = room.Height - 2 + room.Y;
					
					for (int attempts = 0; attempts < 10; attempts++) {

						int xPos = _random.Next(2, room.Width - 2) + room.X;
						
						PlaceTile(xPos, floor, TileID.Extractinator, ref DrawInterface.Undo, mute: true, forced: true);
						
						if (tilePlaced = (_tiles[xPos, floor].active() && _tiles[xPos, floor].type == 219))
							break;
						
					}

					if (tilePlaced)
						break;
					
					for (int xPos = room.X + 2; xPos <= room.X + room.Width - 2; xPos++)
						if (tilePlaced = PlaceTile(xPos, floor, TileID.Extractinator, ref DrawInterface.Undo, mute: true, forced: true))
							break;
						
					if (tilePlaced)
						break;
					
				}

			}

		}

		public void Reset() {
			PlaceExtractinator = false;
			PlaceSharpener = false;
		}

	}

}
