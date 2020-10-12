using System;
using Microsoft.Xna.Framework;
using NeoDraw.Core;
using NeoDraw.UI;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using Terraria;
using static NeoDraw.WldGen.WldGen;

namespace NeoDraw.WldGen.MicroBiomes {

	
    public class NeonMoss : MicroBiome {

		public static int MaxY = 99999; // LavaLine

        public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			if (!WorldGen.InWorld(origin.X, origin.Y))
				return false;

			int neonMossType = _random.Next(179, 183);

			Vector2 vector = default;
			vector.X = origin.X;
			vector.Y = origin.Y;

			Vector2 vector2 = default;
			vector2.X = _random.NextFloat() * 4f - 2f;
			vector2.Y = _random.NextFloat() * 4f - 2f;

			if (vector2.X == 0f)
				vector2.X = 1f;
			
			while (vector2.Length() < 4f)
				vector2 *= 1.5f;

			double size = 20; // _random.Next(60, 80);
			double loops = _random.Next(30, 40);
			
			float  worldScale = Main.maxTilesX / 4200;

			if (GetGoodWorldGen)
				worldScale *= 1.5f;
			
			size  *= worldScale;
			loops *= worldScale;

			while (loops > 0.0) {

				size *= 0.98;
				loops -= 1.0;

				int tileLeft  = (int)(vector.X - size);
				int tileRight = (int)(vector.X + size);
				int tileAbove = (int)(vector.Y - size);
				int tileBelow = (int)(vector.Y + size);

				if (tileLeft < 1)
					tileLeft = 1;
				
				if (tileRight > Main.maxTilesX - 1)
					tileRight = Main.maxTilesX - 1;
				
				if (tileAbove < 1)
					tileAbove = 1;
				
				if (tileBelow > Main.maxTilesY - 1)
					tileBelow = Main.maxTilesY - 1;
				
				if (tileAbove < Main.rockLayer) {

					tileAbove = (int)Main.rockLayer;

					if (vector2.Y < 5f)
						vector2.Y = 5f;
					
				}

				if (tileBelow > MaxY) {

					tileBelow = MaxY;

					if (vector2.Y > -5f)
						vector2.Y = -5f;
					
				}

				double num8 = size * (1f + _random.NextFloat() * 0.4f - 0.2f);

				for (int k = tileLeft; k < tileRight; k++) {
					for (int l = tileAbove; l < tileBelow; l++) {
						if (new Vector2(Math.Abs(k - vector.X), Math.Abs(l - vector.Y)).Length() < num8 * 0.8 && Neo.TileType(k, l) == 1 && (!Main.tile[k - 1, l].active() || !Main.tile[k + 1, l].active() || !Main.tile[k, l - 1].active() || !Main.tile[k, l + 1].active())) {
							SpreadGrass(k - 1, l, ref undo, 1, neonMossType, repeat: true, 0);
						}
					}
				}

				vector += vector2;
				vector2.X += _random.NextFloat() * 4f - 2f;
				vector2.Y += _random.NextFloat() * 4f - 2f;
				vector2.Y = MathHelper.Clamp(vector2.Y, -10f, 10f);
				vector2.X = MathHelper.Clamp(vector2.X, -10f, 10f);

			}

			undo.ResetFrames();

			return true;

		}

    }

}
