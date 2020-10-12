using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;

namespace NeoDraw.WldGen.MicroBiomes {

	public class CaveHouse : MicroBiome { // Updated v1.4 7/23/2020

		private readonly HouseBuilderContext _builderContext = new HouseBuilderContext();

		public float CorruptChestChance  = 1f;
		public float CrimsonChestChance  = 1f;
		public float DesertChestChance   = 1f;
		public float GoldChestChance     = 1f;
		public float GraniteChestChance  = 1f;
		public float IceChestChance      = 1f;
		public float JungleChestChance   = 1f;
		public float MarbleChestChance   = 1f;
		public float MushroomChestChance = 1f;

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			if (!WorldGen.InWorld(origin.X, origin.Y, 10))
				return false;

			HouseBuilder houseBuilder = HouseUtils.CreateBuilder(origin, structures);

			if (!houseBuilder.IsValid)
				return false;

			ApplyConfigurationToBuilder(houseBuilder);

			houseBuilder.Place(_builderContext, structures);

			return true;

		}

		private void ApplyConfigurationToBuilder(HouseBuilder builder) {

			switch (builder.Type) {

				case HouseType.Corrupt:
					builder.ChestChance = CorruptChestChance;
					break;
				case HouseType.Crimson:
					builder.ChestChance = CrimsonChestChance;
					break;
				case HouseType.Desert:
					builder.ChestChance = DesertChestChance;
					break;
				case HouseType.Granite:
					builder.ChestChance = GraniteChestChance;
					break;
				case HouseType.Ice:
					builder.ChestChance = IceChestChance;
					break;
				case HouseType.Jungle:
					builder.ChestChance = JungleChestChance;
					break;
				case HouseType.Marble:
					builder.ChestChance = MarbleChestChance;
					break;
				case HouseType.Mushroom:
					builder.ChestChance = MushroomChestChance;
					break;
				case HouseType.Wood:
					builder.ChestChance = GoldChestChance;
					break;

			}

		}

	}

}
