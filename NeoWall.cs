using NeoDraw.UI;
using Terraria.ModLoader;

namespace NeoDraw {

    public class NeoWall : GlobalWall {

        #region Override Functions

        public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b) {

            if (!NeoDraw.DrawMode)
                return;
            
            if (NeoDraw.BrightMode == 2) {

                r = System.Math.Max(r, DrawInterface.Brightness);
                g = System.Math.Max(g, DrawInterface.Brightness);
                b = System.Math.Max(b, DrawInterface.Brightness);

            }

        }

        #endregion

    }

}
