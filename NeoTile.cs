using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using NeoDraw.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NeoDraw {

    public class NeoTile : GlobalTile {

        #region Variables

        #region Private Variables


        #endregion

        #region Public Variables

        public static bool CaptureTileFrames { get; set; }

        public static bool CaptureTileChanges { get; set; }

        #endregion

        #endregion

        #region Override Functions

        public override bool CreateDust(int i, int j, int type, ref int dustType) => !NeoDraw.DrawMode;

        public override void DrawEffects(int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex) {

            if (!NeoDraw.DrawMode)
                return;

            if (NeoDraw.BrightMode == 1) {

                drawColor.R = (byte)System.Math.Max(drawColor.R, 255 * DrawInterface.Brightness);
                drawColor.G = (byte)System.Math.Max(drawColor.G, 255 * DrawInterface.Brightness);
                drawColor.B = (byte)System.Math.Max(drawColor.B, 255 * DrawInterface.Brightness);

            }

        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem) {

            if (!NeoDraw.DrawMode || !CaptureTileChanges)
                return;

            CaptureTileFrames = true;

            fail       = false;
            effectOnly = false;
            noItem     = true;

            DrawInterface.AddChangedTile(i, j);

            if (TileID.Sets.BasicChest[type] || Main.tileContainer[type]) {

                Point topLeft = Neo.FindTopLeft(i, j);

                int chestIndex = Chest.FindChest(topLeft.X, topLeft.Y);

                if (chestIndex == -1)
                    chestIndex = Chest.FindChestByGuessing(topLeft.X, topLeft.Y);

                if (chestIndex != -1) {
                    DrawInterface.AddChangedTile(topLeft.X, topLeft.Y);
                    Chest.DestroyChestDirect(topLeft.X, topLeft.Y, chestIndex);
                }

            }

        }
        
        public override bool Drop(int i, int j, int type) => !NeoDraw.DrawMode;

        public override void DropCritterChance(int i, int j, int type, ref int wormChance, ref int grassHopperChance, ref int jungleGrubChance) {

            if (!NeoDraw.DrawMode)
                return;

            wormChance = grassHopperChance = jungleGrubChance = 0;

        }

        public override void PlaceInWorld(int i, int j, Item item) {

            if (!NeoDraw.DrawMode || !CaptureTileChanges)
                return;

            DrawInterface.AddChangedTile(i, j);

        }

        public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak) {

            if (!NeoDraw.DrawMode || (!CaptureTileChanges && !CaptureTileFrames))
                return true;

            if (Main.autoPause) {
                return false;
            }

            if (type == TileID.Trees || type == TileID.PalmTree || type == 571 || type == 596 || type == 616 || (type >= 583 && type <= 589))
                DrawInterface.AddChangedTile(i, j);

            return true;
            
        }

        #endregion

    }

}
