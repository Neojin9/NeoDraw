using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.Utilities;

namespace NeoDraw.Core {
    public static class Extensions {

        #region Keys Extensions

        public static bool Pressed(this Keys key) => Main.keyState.GetPressedKeys().Contains(key);

        #endregion

        #region KeyState Extensions

        public static bool PressingAlt(this KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt);

		public static bool PressingCtrl(this KeyboardState keyboardState) => keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl);

		#endregion

		#region List<string> Extensions

		public static string Get(this List<string> listItem, int index) {
			return index < listItem.Count ? listItem[index] : "";
		}

		#endregion

		#region Tile Extensions

		public static void Clear(this Tile tile, TileDataType types) {

			if ((types & TileDataType.Tile) != 0) {
				tile.type = 0;
				tile.active(active: false);
				tile.frameX = 0;
				tile.frameY = 0;
			}

			if ((types & TileDataType.Wall) != 0) {
				tile.wall = 0;
				tile.wallFrameX(0);
				tile.wallFrameY(0);
			}

			if ((types & TileDataType.TilePaint) != 0) {
				tile.color(0);
			}

			if ((types & TileDataType.WallPaint) != 0) {
				tile.wallColor(0);
			}

			if ((types & TileDataType.Liquid) != 0) {
				tile.liquid = 0;
				tile.liquidType(0);
				tile.checkingLiquid(checkingLiquid: false);
			}

			if ((types & TileDataType.Slope) != 0) {
				tile.slope(0);
				tile.halfBrick(halfBrick: false);
			}

			if ((types & TileDataType.Wiring) != 0) {
				tile.wire(wire: false);
				tile.wire2(wire2: false);
				tile.wire3(wire3: false);
				tile.wire4(wire4: false);
			}

			if ((types & TileDataType.Actuator) != 0) {
				tile.actuator(actuator: false);
				tile.inActive(inActive: false);
			}

		}

		public static void ClearMetadata(this Tile tile) {
            tile.liquid = 0;
            tile.sTileHeader = 0;
            tile.bTileHeader = 0;
            tile.bTileHeader2 = 0;
            tile.bTileHeader3 = 0;
            tile.frameX = 0;
            tile.frameY = 0;
        }

        public static bool IsPlatform(this Tile tile) => Main.tileSolidTop[tile.type];

        public static bool IsNotActive(this Tile tile) => !tile.active();

        public static bool IsNotSolid(this Tile tile) => !Main.tileSolid[tile.type];

        public static bool IsSolid(this Tile tile) => Main.tileSolid[tile.type];

        #endregion

        #region Unified Random Extensions

        public static int Next(this UnifiedRandom random, IntRange range) => random.Next(range.Minimum, range.Maximum + 1);

        public static T NextFromList<T>(this UnifiedRandom random, params T[] objs) => objs[random.Next(objs.Length)];

        #endregion

    }

}
