using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace NeoDraw.UI {

    public class VerticalSlider : MenuButtonVerticalSlider {

        public VerticalSlider(int pageAnchor, string display, string to, string desc = "", Action cl = null, Action clh = null) : base(pageAnchor, display, to, desc, cl, clh) {}

        public VerticalSlider(Vector2 anchor, Vector2 offset, string display, string to, string desc = "", Action cl = null, Action clh = null) : this(-1, display, to, desc, cl, clh) {
            this.anchor = anchor;
            this.offset = offset;
        }

        public override void Draw(SpriteBatch sb, bool mouseOver) {

            ActualDraw(sb, mouseOver);

        }

        public void DrawWithOffset(SpriteBatch sb, bool mouseOver, int xOffset = 0, int yOffset = 0) {

            ActualDraw(sb, mouseOver);

        }

        public void ActualDraw(SpriteBatch sb, bool mouseOver) {

            if (segments == 0 || disabled)
                return;

            Texture2D colorBlipTexture = Main.colorBlipTexture;
            float _scale = 0.784313738f;

            int num = -6;
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

            num = -4;
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

        public VerticalSlider With(Action<VerticalSlider> action) {
            action(this);
            return this;
        }

        public new VerticalSlider SetSize(Vector2 s) {
            size = s;
            return this;
        }

        public new VerticalSlider SetSize(float s, float ss) {
            return SetSize(new Vector2(s, ss));
        }

    }

}
