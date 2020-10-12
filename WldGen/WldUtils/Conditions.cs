using Terraria;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/23/2020

	public static class Conditions {

		public class Continue : GenCondition {

			protected override bool CheckValidity(int x, int y) {
				return false;
			}

		}

		public class HasLava : GenCondition {

			protected override bool CheckValidity(int x, int y) {

				if (_tiles[x, y].liquid > 0)
					return _tiles[x, y].liquidType() == 1;

				return false;

			}

		}

		public class IsSolid : GenCondition {

			protected override bool CheckValidity(int x, int y) {

				if (!WorldGen.InWorld(x, y, 10))
					return false;

				if (_tiles[x, y].active())
					return Main.tileSolid[_tiles[x, y].type];
				
				return false;

			}

		}

		public class IsTile : GenCondition {

			private ushort[] _types;

			public IsTile(params ushort[] types) {
				_types = types;
			}

			protected override bool CheckValidity(int x, int y) {

				if (_tiles[x, y].active())
					for (int i = 0; i < _types.Length; i++)
						if (_tiles[x, y].type == _types[i])
							return true;

				return false;

			}

		}

		public class MysticSnake : GenCondition {

			protected override bool CheckValidity(int x, int y) {

				if (_tiles[x, y].active() && !Main.tileCut[_tiles[x, y].type])
					return _tiles[x, y].type != 504;
				
				return false;

			}

		}

		public class NotNull : GenCondition {

			protected override bool CheckValidity(int x, int y) {
				return _tiles[x, y] != null;
			}

		}

	}

}
