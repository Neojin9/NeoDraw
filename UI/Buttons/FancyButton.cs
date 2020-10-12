using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using Terraria;

namespace NeoDraw.UI {

    public class FancyButton : Button {

        #region Variables

        #region Private Variables

        private int xPosition { get { return (int)Position.X + (int)Offset.X; } }
        private int yPosition { get { return (int)Position.Y + (int)Offset.Y; } }

        #endregion

        #region Public Variables

        public float Alpha;

        #endregion

        #endregion

        public FancyButton(Action<Button, int> clicked, Action<Button, SpriteBatch, int> drawCode, Action<Button> hover = null) :base(clicked, drawCode, hover) {

            Alpha = 1f;

        }

        #region Override Functions

        public override int ActualDraw(SpriteBatch sb, int block = -1) {

            if (new Rectangle(xPosition, yPosition, (int)Size.X, (int)Size.Y).Contains(Main.MouseScreen.ToPoint())) {

                Main.LocalPlayer.mouseInterface = true;

                if (Main.mouseLeft)
                    block = 0;

                if (Main.mouseRight)
                    block = 1;

            }

            Drawing.DrawBox(sb, xPosition, yPosition, (int)Size.X, (int)Size.Y, Alpha);
            DrawCode(this, sb, block);

            return block;

        }

        #endregion

        #region Public Functions

        public void Draw(SpriteBatch sb, bool draw, bool update, float alpha = 1f) {

            Alpha = alpha;
            Offset = default;
            base.Draw(sb, draw, update);

        }

        public void DrawWithOffset(SpriteBatch sb, bool draw, bool update, float alpha = 1f, int xOffset = 0, int yOffset = 0) {

            Alpha = alpha;
            Offset = new Vector2(xOffset, yOffset);
            base.Draw(sb, draw, update);

        }

        #endregion

    }

}
