using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using Terraria.ID;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes { // v1.4 7/23/2020

	public static class HouseUtils {

		public static bool NotTheBees;

		public static readonly bool[] BlacklistedTiles = TileID.Sets.Factory.CreateBoolSet(true,
			TileID.BlueDungeonBrick,
			TileID.GreenDungeonBrick,
			TileID.PinkDungeonBrick,
			TileID.SandstoneBrick,
			TileID.Hive,
			TileID.LihzahrdBrick
		);

		public static readonly bool[] BeelistedTiles = TileID.Sets.Factory.CreateBoolSet(true,
			TileID.BlueDungeonBrick,
			TileID.GreenDungeonBrick,
			TileID.PinkDungeonBrick,
			TileID.SandstoneBrick,
			TileID.LihzahrdBrick
		);

		public static HouseBuilder CreateBuilder(Point origin, StructureMap structures) {

			List<Rectangle> list = CreateRooms(origin);

			if (list.Count == 0 || !AreRoomLocationsValid(list))
				return HouseBuilder.Invalid;
			
			HouseType houseType = GetHouseType(list); // TODO: Force HouseType selection here.

			if (!AreRoomsValid(list, structures, houseType))
				return HouseBuilder.Invalid;
			
			switch (houseType) {

				case HouseType.Corrupt:
					return new CorruptHouseBuilder(list);
				case HouseType.Crimson:
					return new CrimsonHouseBuilder(list);
				case HouseType.Desert:
					return new DesertHouseBuilder(list);
				case HouseType.Granite:
					return new GraniteHouseBuilder(list);
				case HouseType.Ice:
					return new IceHouseBuilder(list);
				case HouseType.Jungle:
					return new JungleHouseBuilder(list);
				case HouseType.Marble:
					return new MarbleHouseBuilder(list);
				case HouseType.Mushroom:
					return new MushroomHouseBuilder(list);
				case HouseType.Wood:
					return new WoodHouseBuilder(list);
				default:
					return new WoodHouseBuilder(list);

			}

		}

		private static List<Rectangle> CreateRooms(Point origin) {

			if (!Find(
					origin,
					Searches.Chain(
						new Searches.Down(200),
						new Conditions.IsSolid()
					), out Point result
				) || result == origin) {

				return new List<Rectangle>();

			}

			Rectangle mainRoom = FindRoom(result);
			Rectangle subRoom1 = FindRoom(new Point(mainRoom.Center.X, mainRoom.Y + 1));
			Rectangle subRoom2 = FindRoom(new Point(mainRoom.Center.X, mainRoom.Y + mainRoom.Height + 10));

			subRoom2.Y = mainRoom.Y + mainRoom.Height - 1;

			float roomSolidPercentage  = GetRoomSolidPercentage(subRoom1);
			float roomSolidPercentage2 = GetRoomSolidPercentage(subRoom2);
			
			mainRoom.Y += 3;
			subRoom1.Y += 3;
			subRoom2.Y += 3;

			List<Rectangle> roomList = new List<Rectangle>();
			
			if (WorldGen.genRand.Next(3) != 0 || Main.keyState.PressingAlt()) // if (WorldGen.genRand.NextFloat() > roomSolidPrecentage + 0.2f)
				roomList.Add(subRoom1);
			
			roomList.Add(mainRoom);

			if (WorldGen.genRand.Next(2) == 0 || Main.keyState.PressingCtrl()) // if (WorldGen.genRand.NextFloat() > roomSolidPrecentage2 + 0.2f)
				roomList.Add(subRoom2);
			
			return roomList;

		}

		private static Rectangle FindRoom(Point origin) {


            bool foundSolidTileToTheLeft = Find(
                origin,
                Searches.Chain(
                    new Searches.Left(25),
                    new Conditions.IsSolid()
                ),
                out Point leftSide
            );

            bool foundSolidTileToTheRight = Find(
				origin,
				Searches.Chain(
					new Searches.Right(25),
					new Conditions.IsSolid()
				),
				out Point rightSide
			);
			
			if (!foundSolidTileToTheLeft)
				leftSide = new Point(origin.X - 25, origin.Y);
			
			if (!foundSolidTileToTheRight)
				rightSide = new Point(origin.X + 25, origin.Y);
			
			Rectangle room = new Rectangle(origin.X, origin.Y, 0, 0);

			if (origin.X - leftSide.X > rightSide.X - origin.X) {

				room.X = leftSide.X;
				room.Width = Utils.Clamp(rightSide.X - leftSide.X, 15, 30);

			}
			else {

				room.Width = Utils.Clamp(rightSide.X - leftSide.X, 15, 30);
				room.X = rightSide.X - room.Width;

			}


            bool foundSolidTopLeftTile = Find(
                leftSide,
                Searches.Chain(
                    new Searches.Up(10),
                    new Conditions.IsSolid()
                ),
                out Point ceilingLeft
            );

            bool foundSolidTopRightTile = Find(
				rightSide,
				Searches.Chain(
					new Searches.Up(10),
					new Conditions.IsSolid()
				),
				out Point ceilingRight
			);
			
			if (!foundSolidTopLeftTile)
				ceilingLeft = new Point(origin.X, origin.Y - 10);
			
			if (!foundSolidTopRightTile)
				ceilingRight = new Point(origin.X, origin.Y - 10);
			
			room.Height = Utils.Clamp(Math.Max(origin.Y - ceilingLeft.Y, origin.Y - ceilingRight.Y), 8, 12);
			room.Y -= room.Height;
			
			return room;

		}

		private static float GetRoomSolidPercentage(Rectangle room) {

			float roomArea = room.Width * room.Height;
			Ref<int> @ref = new Ref<int>(0);
			
			Gen(
				new Point(room.X, room.Y),
				new Shapes.Rectangle(room.Width, room.Height),
				Actions.Chain(
					new Modifiers.IsSolid(),
					new Actions.Count(@ref)
				)
			);
			
			return @ref.Value / roomArea;

		}

		private static int SortBiomeResults(Tuple<HouseType, int> item1, Tuple<HouseType, int> item2) {
			return item2.Item2.CompareTo(item1.Item2);
		}

		private static bool AreRoomLocationsValid(IEnumerable<Rectangle> rooms) {

			foreach (Rectangle room in rooms)
				if (room.Y + room.Height > Main.maxTilesY - 220)
					return false;
			
			return true;

		}

		private static HouseType GetHouseType(IEnumerable<Rectangle> rooms) {

			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();

			foreach (Rectangle room in rooms) {

				Gen(
					new Point(room.X - 10, room.Y - 10),
					new Shapes.Rectangle(room.Width + 20, room.Height + 20),
					new Actions.TileScanner(
						TileID.Dirt,
						TileID.Stone,
						TileID.Ebonstone,
						TileID.Sand,
						TileID.Mud,
						TileID.JungleGrass,
						TileID.MushroomGrass,
						TileID.SnowBlock,
						TileID.IceBlock,
						TileID.Crimstone,
						TileID.Marble,
						TileID.Granite,
						TileID.Sandstone,
						TileID.HardenedSand
					).Output(dictionary)
				);

			}

			List<Tuple<HouseType, int>> tileCount = new List<Tuple<HouseType, int>> {

				Tuple.Create(HouseType.Corrupt,  dictionary[TileID.Ebonstone]                                                                 ),
				Tuple.Create(HouseType.Crimson,  dictionary[TileID.Crimstone]                                                                 ),
				Tuple.Create(HouseType.Desert,   dictionary[TileID.Sand]      + dictionary[TileID.Sandstone] + dictionary[TileID.HardenedSand]),
				Tuple.Create(HouseType.Granite,  dictionary[TileID.Granite]                                                                   ),
				Tuple.Create(HouseType.Ice,      dictionary[TileID.SnowBlock] + dictionary[TileID.IceBlock]                                   ),
				Tuple.Create(HouseType.Jungle,   dictionary[TileID.Mud]       + dictionary[TileID.JungleGrass] * 10                           ),
				Tuple.Create(HouseType.Marble,   dictionary[TileID.Marble]                                                                    ),
				Tuple.Create(HouseType.Mushroom, dictionary[TileID.Mud]       + dictionary[TileID.MushroomGrass] * 10                         ),
				Tuple.Create(HouseType.Wood,     dictionary[TileID.Dirt]      + dictionary[TileID.Stone]                                      )

			};

			tileCount.Sort(SortBiomeResults);

			return tileCount[0].Item1;

		}

		private static bool AreRoomsValid(IEnumerable<Rectangle> rooms, StructureMap structures, HouseType style) {
			
			foreach (Rectangle room in rooms) {

				if (style != HouseType.Granite && Find(new Point(room.X - 2, room.Y - 2), Searches.Chain(new Searches.Rectangle(room.Width + 4, room.Height + 4).RequireAll(mode: false), new Conditions.HasLava()), out Point _))
					return false;
				
				if (NotTheBees) {
					if (!structures.CanPlace(room, BeelistedTiles, 5)) {
						return false;
					}
				}
				else if (!structures.CanPlace(room, BlacklistedTiles, 5)) {
					return false;
				}

			}

			return true;

		}

	}

}
