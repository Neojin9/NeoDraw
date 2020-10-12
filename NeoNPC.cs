using System.Collections.Generic;
using Terraria.ModLoader;

namespace NeoDraw {

    public class NeoNPC : GlobalNPC {

        #region Override Functions

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo) {

            if (!NeoDraw.DrawMode)
                return;

            pool.Clear();

        }

        #endregion

    }

}
