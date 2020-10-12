using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;


namespace NeoDraw.UI {

    public class MenuButtonVerticalSlider : MenuButton {

        public int multislide = 1;
        public int segments;
        public int slider;
        public int slider_hover;

        public MenuButtonVerticalSlider(int pageAnchor, string display, string to, string desc = "", Action cl = null, Action clh = null) : base(pageAnchor, display, to, desc, cl, clh) {}

        public MenuButtonVerticalSlider(Vector2 anchor, Vector2 offset, string display, string to, string desc = "", Action cl = null, Action clh = null) : this(-1, display, to, desc, cl, clh) {
            this.anchor = anchor;
            this.offset = offset;
        }

        public new MenuButtonVerticalSlider SetFontType(int t) {
            fontType = t;
            return this;
        }

        public new MenuButtonVerticalSlider SetScale(float s) {
            scale = s;
            return this;
        }

        public new MenuButtonVerticalSlider SetSize(Vector2 s) {
            size = s;
            return this;
        }

        public new MenuButtonVerticalSlider SetSize(float s, float ss) {
            return SetSize(new Vector2(s, ss));
        }

        public new MenuButtonVerticalSlider Disable(bool b) {
            disabled = b;
            return this;
        }

        public MenuButtonVerticalSlider With(Action<MenuButtonVerticalSlider> action) {
            action(this);
            return this;
        }

        public override bool MouseOver(Vector2 mouse) {
            
            slider_hover = -1;
            
            if (!base.MouseOver(mouse))
                return false;
            
            if (mouse.X >= size.X + position.X - 4f || mouse.X < position.X + 4f)
                return true;
            
            float num = 0f;
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

        public override void Draw(SpriteBatch SP, bool mouseOver) {
            
            if (segments == 0 || disabled)
                return;
            
            Texture2D colorBlipTexture = Main.colorBlipTexture;
            float _scale = 0.784313738f;
            int num = -4;
            int num2 = 0;

            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2, (int)size.X + num * 2, (int)size.Y + num2 * 2), Color.Black * _scale);
            num = -2;
            num2 = -2;
            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2, (int)size.X + num * 2, (int)size.Y + num2 * 2), Color.Black * _scale);
            num = 0;
            num2 = -4;
            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2, (int)size.X + num * 2, (int)size.Y + num2 * 2), Color.Black * _scale);
            num = -2;
            num2 = -4;
            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2, (int)size.X + num * 2, (int)size.Y + num2 * 2), new Color(63, 65, 151) * _scale);
            num = -4;
            num2 = -2;
            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2, (int)size.X + num * 2, (int)size.Y + num2 * 2), new Color(63, 65, 151) * _scale);
            num = -6;
            num2 = -4;
            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2 + (int)(size.Y / segments * slider), (int)size.X + num * 2, (int)(size.Y / segments * multislide) + num2 * 2), Color.LightSteelBlue * _scale);
            num = -4;
            num2 = -6;
            SP.Draw(colorBlipTexture, new Rectangle((int)position.X - num, (int)position.Y - num2 + (int)(size.Y / segments * slider), (int)size.X + num * 2, (int)(size.Y / segments * multislide) + num2 * 2), Color.LightSteelBlue * _scale);
            
        }

    }

}
