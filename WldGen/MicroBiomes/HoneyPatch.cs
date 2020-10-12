using Microsoft.Xna.Framework;
using NeoDraw.Undo;
using NeoDraw.WldGen.WldUtils;
using static NeoDraw.WldGen.WldUtils.WldUtils;

namespace NeoDraw.WldGen.MicroBiomes {

    class HoneyPatch : MicroBiome {

		public override bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list) {

			Point result = origin;

			Gen(
				result,
				new Shapes.Circle(8),
				Actions.Chain(
					new Modifiers.RadialDither(0f, 10f),
					new Modifiers.IsSolid(),
					new Actions.SetTile(229, setSelfFrames: true)
				)
			);

			ShapeData data = new ShapeData();

			Gen(
				result,
				new Shapes.Circle(4, 3),
				Actions.Chain(
					new Modifiers.Blotches(),
					new Modifiers.IsSolid(),
					new Actions.ClearTile(frameNeighbors: true),
					new Modifiers.RectangleMask(-6, 6, 0, 3).Output(data),
					new Actions.SetLiquid(2)
				)
			);
			
			Gen(
				new Point(result.X, result.Y + 1),
				new ModShapes.InnerOutline(data),
				Actions.Chain(
					new Modifiers.IsEmpty(),
					new Modifiers.RectangleMask(-6, 6, 1, 3),
					new Actions.SetTile(59, setSelfFrames: true)
				)
			);
			
			return true;

		}

	}

}
