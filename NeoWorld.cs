using Terraria;
using Terraria.ModLoader;

namespace NeoDraw {

    public class NeoWorld : ModWorld {

        #region Override Functions

        public override void Initialize() => NeoDraw.Reset();

        public override void PostUpdate() {

            if (!NeoDraw.DrawMode)
                return;

            if (Main.slimeRain)
                Main.StopSlimeRain();

            Main.raining = false;

        }

        #endregion

    }

}
