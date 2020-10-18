using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace NeoDraw.UI {

    public class VerticalSlider {

        #region Variables

        public Action Click     = delegate { };
        public Action ClickHold = delegate { };
        public Action Update    = delegate { };

        public bool buttonLock   = true;
        public bool canMouseOver = true;
        public bool disabled;

        public int framesHeld;
        public int multislide = 1;
        public int segments;
        public int slider;
        public int slider_hover;

        public Vector2 anchor;
        public Vector2 offset;
        public Vector2 size;

        #endregion

        public Vector2 position => new Vector2(Main.screenWidth, Main.screenHeight) * anchor + offset;

        public VerticalSlider(Vector2 anchor, Vector2 offset, Action cl = null, Action clh = null) {

            size = new Vector2(200f, 40f);

            if (cl != null)
                Click = cl;

            if (clh != null)
                ClickHold = clh;

            this.anchor = anchor;
            this.offset = offset;

        }

        public void Draw(SpriteBatch sb, bool mouseOver) {

            if (segments == 0 || disabled)
                return;

            Texture2D colorBlipTexture = Main.colorBlipTexture;
            float     _scale           = 0.784313738f;

            int num  = -6;
            int num2 = -4;

            sb.Draw(
                colorBlipTexture,
                new Rectangle(
                    (int)position.X - num,
                    (int)position.Y - num2 + (int)(size.Y / segments * slider),
                    (int)size.X + num * 2,
                    (int)(size.Y / segments * multislide) + num2 * 2
                ),
                Color.LightGray * _scale
            );

            num  = -4;
            num2 = -6;

            sb.Draw(
                colorBlipTexture,
                new Rectangle(
                    (int)position.X - num,
                    (int)position.Y - num2 + (int)(size.Y / segments * slider),
                    (int)size.X + num * 2,
                    (int)(size.Y / segments * multislide) + num2 * 2
                ),
                Color.LightGray * _scale
            );

        }

        public bool MouseOver(Vector2 mouse) {

            slider_hover = -1;

            if (!(mouse.X >= position.X && mouse.X < position.X + size.X && mouse.Y >= position.Y && mouse.Y < position.Y + size.Y))
                return false;

            if (mouse.X >= size.X + position.X - 4f || mouse.X < position.X + 4f)
                return true;

            float num  = 0f;
            float num2 = size.Y - 8f;

            for (int i = 0; i < segments; i++) {

                float num3 = num2 / segments;

                if (mouse.Y >= num + position.Y + 4f && mouse.Y < num + position.Y + 4f + num3) {
                    slider_hover = i;
                    return true;
                }

                num += num3;

            }

            return true;

        }

        public VerticalSlider With(Action<VerticalSlider> action) {
            action(this);
            return this;
        }

        public VerticalSlider SetSize(Vector2 s) {
            size = s;
            return this;
        }

        public VerticalSlider SetSize(float width, float height) => SetSize(new Vector2(width, height));

    }

}
