using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using Terraria;

namespace NeoDraw.UI {

	internal class TopButton2 { // Not used

        #region Variables

        #region Private Variables

        private DateTime _clickDelay = DateTime.MinValue;
        
        private float _alpha = 1;
        
        private int _clickedPos;

        #endregion

        #region Func and Action

        public readonly Func<bool> IsActive;
        public Func<float> GetAlpha;
        public readonly Action<int> Clicked;
        public readonly Action Hover;

        #endregion

        #region Public Variables

        public bool Dragging = false;
        
        public Color BGActiveColor = Color.Green;
        public Color BGInactiveColor = Color.White;
        public Color BGHoveredColor = Color.Yellow;
        
        public string Name;
        
        public Texture2D BGActiveTexture;
        public Texture2D BGHoveredTexture;
        public Texture2D BGInactiveTexture;
        public Texture2D Texture;
        
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 TextureSize;
        public Vector2 TextureFrame;

        #endregion

        #endregion

        public TopButton2(string name, Action<int> clicked, Func<bool> isActive, Action hover = null) {

            Name = name;
            Clicked = clicked;
            IsActive = isActive;
            Hover = hover;

        }

        public TopButton2(string name, Vector2 size, Vector2 position,
                         Texture2D texture, Vector2 textureSize, Vector2 textureFrame,
                         Texture2D bgActiveTexture, Texture2D bgInactiveTexture, Texture2D bgHoveredTexture,
                         Action<int> clicked, Func<bool> isActive, Func<float> getAlpha, Action hover = null) {
            
            Name = name;
            Size = size;
            Position = position;

            Texture = texture;
            TextureSize = textureSize;
            TextureFrame = textureFrame;

            BGActiveTexture = bgActiveTexture;
            BGInactiveTexture = bgInactiveTexture;
            BGHoveredTexture = bgHoveredTexture;

            Clicked = clicked;
            IsActive = isActive;
            GetAlpha = getAlpha;
            Hover = hover;

        }

        #region Public Functions

        public void Draw(SpriteBatch sb, bool draw, bool update) {

            if (update)
                Update();

            if (draw)
                Draw(sb);

        }

        #endregion

        #region Private Functions

        private void Draw(SpriteBatch sb) {

            DrawBackground(sb);
            DrawTexture(sb);

        }

        private void DrawBackground(SpriteBatch sb) {

            Texture2D texture;
            Color color;

            if (IsHovered()) {
                color = BGHoveredColor;
                texture = BGHoveredTexture;
            } else if (IsActive != null && IsActive()) {
                color = BGActiveColor;
                texture = BGActiveTexture;
            } else {
                color = BGInactiveColor;
                texture = BGInactiveTexture;
            }

            if (_clickedPos == 0) {

                if (texture == null) {

                    Drawing.DrawBox(sb, Position.X + 2, Position.Y + 2, Size.X, Size.Y, Color.Black * 0.5f * _alpha);
                    Drawing.DrawBox(sb, Position.X + 1, Position.Y + 1, Size.X, Size.Y, Color.Black * _alpha);

                }
                else {

                    sb.Draw(texture, new Vector2(Position.X + 2, Position.Y + 2), Color.Black * 0.5f * _alpha);
                    sb.Draw(texture, new Vector2(Position.X + 1, Position.Y + 1), Color.Black * _alpha);

                }

            }

            if (texture == null) {
                Drawing.DrawBox(sb, Position.X + _clickedPos, Position.Y + _clickedPos, Size.X, Size.Y, color * _alpha);
            } else {
                sb.Draw(texture, new Vector2(Position.X + _clickedPos, Position.Y + _clickedPos), Color.White * _alpha);
            }

        }

        private void DrawTexture(SpriteBatch sb) {

            if (Texture == null)
                return;

            sb.Draw(Texture, new Vector2(Position.X + _clickedPos, Position.Y + _clickedPos), new Rectangle((int)TextureFrame.X, (int)TextureFrame.Y, (int)TextureSize.X, (int)TextureSize.Y), Color.White * _alpha);

        }

        private bool IsHovered() {
            return new Rectangle((int)Position.X + _clickedPos, (int)Position.Y + _clickedPos, (int)Size.X, (int)Size.Y).Contains(Main.MouseScreen.ToPoint());
        }

        private void Update() {

            if (new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y).Contains(Main.MouseScreen.ToPoint())) {

                Main.LocalPlayer.mouseInterface = true;

                Hover?.Invoke();

                if (Main.mouseLeft)
                    if (Main.mouseLeftRelease)
                        Clicked(0);

                if (Main.mouseRight)
                    if (Main.mouseRightRelease)
                        Clicked(1);

            }

        }

        #endregion

    }

}
