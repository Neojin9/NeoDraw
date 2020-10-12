using Microsoft.Xna.Framework;
using NeoDraw.Undo;

namespace NeoDraw.WldGen.WldUtils {

	public static class Biomes<T> where T : MicroBiome, new() {
		
		private static T _microBiome = CreateInstance();

        public static bool Place(int x, int y, StructureMap structures, ref UndoStep undo, params object[] list) => _microBiome.Place(new Point(x, y), structures, ref undo, list);

        private static T CreateInstance() {
			T val = new T();
			return val;
		}
		
	}

}
