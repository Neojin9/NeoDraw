using Terraria;
using Terraria.ModLoader;

namespace NeoDraw {

    public class NeoItem : GlobalItem {

        #region Override Functions

        public override bool CanUseItem(Item item, Player player) => !NeoDraw.DrawMode;

        #endregion

    }

}
