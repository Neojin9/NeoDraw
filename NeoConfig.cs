using Terraria.ModLoader.Config;

namespace NeoDraw {

    public class NeoConfigServer : ModConfig {

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Activated")]
        [Tooltip("I acknowled that this is an experimental MOD that is still in progress. Must be ON for MOD to work.")]
        public bool Activated { get; set; }

        [Label("Backups Made")]
        [Tooltip("It is recommened to make backups of any world you intend to edit. Must be ON for MOD to work.")]
        public bool Backuped { get; set; }

        [Label("MOD Tile Compatibility")]
        [Tooltip("Allow placing tiles added from other MODs. May have undesireable results.")]
        public bool ModTilesAllowed { get; set; }

        [Label("Debug Mode")]
        [Tooltip("Glimpse under the hood.")]
        [ReloadRequired]
        public bool DebugMode { get; set; }

    }

}
