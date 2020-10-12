using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;

namespace NeoDraw.Core {

    public class Drawing {

        public static readonly int[][] shadowOffset;

        public static Texture2D box;

        static Drawing() {

            shadowOffset = new[] {
                new[] { -1, 0 },
                new[] { 1, 0 },
                new[] { 0, -1 },
                new[] { 0, 1 }
            };

            box = Main.inventoryBackTexture;

        }

        ///<summary>
        ///Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        ///of the specified color.
        ///</summary>
        ///<param name="sb">The SpriteBatch currently being used to draw to screen</param>
        ///<param name="borderTexture">The 1x1 Texture2D used to draw the border</param>
        ///<param name="rectangleToDraw">The border position and size</param>
        ///<param name="thicknessOfBorder">How thick in pixels the border should be</param>
        ///<param name="borderColor">What color the border should be</param>
        public void DrawBorder(SpriteBatch sb, Texture2D borderTexture, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor) {

            // Draw top line
            sb.Draw(borderTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            sb.Draw(borderTexture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            sb.Draw(borderTexture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);

            // Draw bottom line
            sb.Draw(borderTexture, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);

        }

        public static void DrawBox(SpriteBatch sb, float x, float y, float w, float h, Color c) {
            DrawBox(sb, (int)x, (int)y, (int)w, (int)h, c);
        }

        public static void DrawBox(SpriteBatch sb, float x, float y, float w, float h, float a = 0.785f) {
            DrawBox(sb, (int)x, (int)y, (int)w, (int)h, new Color(63, 65, 151) * a);
        }

        public static void DrawBox(SpriteBatch sb, int x, int y, int w, int h, Color c) {

            Texture2D texture2D = box;

            if (w < 20)
                w = 20;

            if (h < 20)
                h = 20;

            sb.Draw(texture2D, new Rectangle(x          , y          , 10     , 10     ), new Rectangle( 0                  , 0                    , 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x + 10     , y          , w - 20 , 10     ), new Rectangle(10                  , 0                    , 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x + w - 10 , y          , 10     , 10     ), new Rectangle(texture2D.Width - 10, 0                    , 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x          , y + 10     , 10     , h - 20 ), new Rectangle( 0                  , 10                   , 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x + 10     , y + 10     , w - 20 , h - 20 ), new Rectangle(10                  , 10                   , 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x + w - 10 , y + 10     , 10     , h - 20 ), new Rectangle(texture2D.Width - 10, 10                   , 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x          , y + h - 10 , 10     , 10     ), new Rectangle( 0                  , texture2D.Height - 10, 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x + 10     , y + h - 10 , w - 20 , 10     ), new Rectangle(10                  , texture2D.Height - 10, 10, 10), c);
            sb.Draw(texture2D, new Rectangle(x + w - 10 , y + h - 10 , 10     , 10     ), new Rectangle(texture2D.Width - 10, texture2D.Height - 10, 10, 10), c);

        }

        public static void DrawBox(SpriteBatch sb, int x, int y, int w, int h, float a) {

            sb.Draw(UI.DrawInterface.Dark   , new Rectangle(x        , y        , w    , 2    ), Color.White * a);
            sb.Draw(UI.DrawInterface.Dark   , new Rectangle(x        , y + h - 2, w    , 2    ), Color.White * a);
            sb.Draw(UI.DrawInterface.Dark   , new Rectangle(x        , y        , 2    , h    ), Color.White * a);
            sb.Draw(UI.DrawInterface.Dark   , new Rectangle(x + w - 2, y        , 2    , h    ), Color.White * a);
            
            sb.Draw(UI.DrawInterface.Lighter, new Rectangle(x + 2    , y + 2    , w - 4, 2    ), Color.White * a);

            sb.Draw(UI.DrawInterface.Light  , new Rectangle(x + 2    , y + 4    , w - 4, h - 6), Color.White * a);

        }

        public static void DrawStringShadow(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, int offset = 1) {
            DrawStringShadow(sb, font, text, pos, new Color(0f, 0f, 0f, 0.5f), 0f, default, 1f, offset);
        }

        public static void DrawStringShadow(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, Color color, float rotation = 0f, Vector2 origin = default, float scale = 1f, int offset = 1) {
            DrawStringShadow(sb, font, text, pos, color, rotation, origin, new Vector2(scale, scale), offset);
        }

        public static void DrawStringShadow(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, Color color, float rotation, Vector2 origin, Vector2 scale, int offset = 1) {

            color = new Color(color.R, color.G, color.B, ((byte)(Math.Pow((color.A / 255f), 2.0) * 255.0)));

            foreach (int[] t in shadowOffset)
                sb.DrawString(font, text, new Vector2(pos.X + (t[0] * offset), pos.Y + (t[1] * offset)), color, rotation, origin, scale, SpriteEffects.None, 0f);

        }

        public static Vector2 DString(SpriteBatch sb, string se, Vector2 ve, Color ce, float fe = 1f, float rex = 0f, float rey = 0f, int font = 0) {

            DynamicSpriteFont spriteFont = Main.fontMouseText;

            if (font == 1)
                spriteFont = Main.fontDeathText;

            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++)
                    sb.DrawString(spriteFont, se, ve + new Vector2(i, j) * 1f, Color.Black, 0f, new Vector2(rex, rey) * spriteFont.MeasureString(se), fe, SpriteEffects.None, 0f);
            }

            sb.DrawString(spriteFont, se, ve, ce, 0f, new Vector2(rex, rey) * spriteFont.MeasureString(se), fe, SpriteEffects.None, 0f);

            return spriteFont.MeasureString(se) * fe;

        }

        public static void StringShadowed(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos) {
            StringShadowed(sb, font, text, pos, Color.White);
        }

        public static void StringShadowed(SpriteBatch sb, DynamicSpriteFont font, string text, Vector2 pos, Color c, float scale = 1f, Vector2 origin = default) {
            DrawStringShadow(sb, font, text, pos, new Color(0, 0, 0, c.A), 0f, origin, scale);
            sb.DrawString(font, text, pos, c, 0f, origin, scale, SpriteEffects.None, 0f);
        }

    }

}
