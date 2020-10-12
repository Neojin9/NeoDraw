using Microsoft.Xna.Framework;
using NeoDraw.Undo;

namespace NeoDraw.WldGen.WldUtils { // Updated v1.4 7/23/2020

	public abstract class GenStructure : GenBase {

		public abstract bool Place(Point origin, StructureMap structures, ref UndoStep undo, params object[] list);

	}

}
