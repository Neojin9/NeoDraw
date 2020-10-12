using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using Terraria;


namespace NeoDraw.UI {

    public class MenuButton {

        public Action Click = delegate { };
        public Action ClickHold = delegate { };
        public Action Update = delegate { };
        public Vector2 anchor;
        public int automaticPos = -1;
        public bool buttonLock = true;
        public bool canMouseOver = true;
        public Color colorFrame = new Color(63, 65, 151) * 0.784313738f;
        public Color colorMouseOver = Color.White;
        public Color colorText = Color.Silver;
        public Vector2? customPos = null;
        public string description = "";
        public bool disabled;
        public string displayText = "";
        public int fontType;
        public int framesHeld;
        public string leadsTo = "";
        public Vector2 offset;
        public float scale = 1f;
        public Vector2 size;
        public string tooltip = "";
        public int whoAmi;

        public MenuButton(int pageAnchor, string display, string to, string desc = "", Action cl = null, Action clh = null) {

            automaticPos = pageAnchor;
            size = new Vector2(200f, 40f);
            displayText = display;
            leadsTo = to;
            description = desc;
            
            if (cl == null)
                Click = () => Menu.MoveTo(leadsTo);
            else
                Click = cl;
            
            if (clh != null)
                ClickHold = clh;

        }

        public MenuButton(Vector2 anchor, Vector2 offset, string display, string to, string desc = "", Action cl = null, Action clh = null) : this(-1, display, to, desc, cl, clh) {
            this.anchor = anchor;
            this.offset = offset;
        }

        public Vector2 position {

            get {
                
                Vector2? vector = customPos;
                
                if (!vector.HasValue)
                    return new Vector2(Main.screenWidth, Main.screenHeight) * anchor + offset;
                
                return vector.GetValueOrDefault();

            }

            set { customPos = value; }

        }

        public MenuButton SetFontType(int t) {
            
            fontType = t;
            
            return this;

        }

        public MenuButton SetScale(float s) {
            
            scale = s;
            
            return this;

        }

        public MenuButton SetSize(Vector2 s) {
            
            size = s;
            
            return this;

        }

        public MenuButton SetSize(float s, float ss) {
            return SetSize(new Vector2(s, ss));
        }

        public MenuButton Disable(bool b) {
            
            disabled = b;
            
            return this;

        }

        public MenuButton CanMouseOver(bool b) {
            
            canMouseOver = b;
            
            return this;

        }

        public MenuButton With(Action<MenuButton> action) {
            
            action(this);

            return this;

        }

        public virtual void Draw(SpriteBatch SP, bool mouseOver) {
            
            if (disabled)
                return;
            
            Drawing.DrawBox(SP, position.X, position.Y, size.X, size.Y, colorFrame);
            
            if (mouseOver) {
                Menu.mouseOverText = description;
                Drawing.DString(SP, displayText, position + size / 2f, colorMouseOver, scale, 0.5f, 0.5f, fontType);
                return;
            }
            
            Drawing.DString(SP, displayText, position + size / 2f, colorText, scale - 0.05f, 0.5f, 0.5f, fontType);

        }

        public virtual bool MouseOver(Vector2 mouse) {
            return mouse.X >= position.X && mouse.X < position.X + size.X && mouse.Y >= position.Y && mouse.Y < position.Y + size.Y;
        }

        public void SetAutomaticPosition(MenuAnchor m, int index) {
            anchor = m.anchor;
            offset = m.offset + m.offset_button * index + new Vector2(-size.X / 2f, 0f);
        }

    }

}
