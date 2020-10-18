using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using Terraria;
using Terraria.UI;

namespace NeoDraw.UI {

	public class Button : UIElement {

        #region Variables

        #region Private Variables

        private int xPosition => (int)Position.X + (int)Offset.X;
        private int yPosition => (int)Position.Y + (int)Offset.Y;

        #endregion

        #region Public Variables

        public bool Dragging = false;

		public Vector2 Position = default;
		public Vector2 Offset = default;
		public Vector2 Size = default;

		public readonly Action<Button, int> Clicked;
		public readonly Action<Button, SpriteBatch, int> DrawCode;
		public readonly Action<Button> Hover;

        #endregion

        #endregion

        public Button(Action<Button, int> clicked, Action<Button, SpriteBatch, int> drawCode, Action<Button> hover = null) {

			Clicked = clicked;
			DrawCode = drawCode;
			Hover = hover;

		}

        #region Virtual Functions

        public virtual int ActualDraw(SpriteBatch sb, int block = -1) {

			if (new Rectangle(xPosition, yPosition, (int)Size.X, (int)Size.Y).Contains(Main.MouseScreen.ToPoint())) {

				Main.LocalPlayer.mouseInterface = true;

				if (Main.mouseLeft)
					block = 0;

				if (Main.mouseRight)
					block = 1;

			}

			Drawing.DrawBox(sb, xPosition, yPosition, Size.X, Size.Y);
			DrawCode(this, sb, block);

			return block;

		}

		public virtual int Update() {

			int block = -1;

			if (!new Rectangle(xPosition, yPosition, (int)Size.X, (int)Size.Y).Contains(Main.MouseScreen.ToPoint()))
				return block;

			Main.LocalPlayer.mouseInterface = true;

            Hover?.Invoke(this);

            if (Main.mouseLeft) {

				if (Main.mouseLeftRelease)
					Clicked(this, 0);

				block = 0;

			}

			if (Main.mouseRight) {

				if (Main.mouseRightRelease)
					Clicked(this, 1);

				block = 1;

			}

			return block;

		}

        #endregion

        #region Public Functions

        public bool Draw(SpriteBatch sb, bool draw, bool update) {

			int block = -1;

			if (update)
				block = Update();

			if (draw)
				block = ActualDraw(sb, block);

			return block != -1;

		}

		#endregion

	}

}
