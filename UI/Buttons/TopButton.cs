using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeoDraw.Core;
using Terraria;

namespace NeoDraw.UI {

	internal class TopButton {

        #region Variables

        #region Private Variables

        private DateTime _clickDelay = DateTime.MinValue;
        
        private float _alpha = 1;
        
        private int _clickedPos;
        private int _currentFrame;
        private int _frameDelayCounter;

        private int xPosition { get { return (int)Position.X + (int)Offset.X; } }
        private int yPosition { get { return (int)Position.Y + (int)Offset.Y; } }

        #endregion

        #region Func and Action

        public Func<bool> IsActive;
        public Func<float> GetAlpha;
        public new Action<TopButton> OnClick;
        public Action<TopButton> OnHover;
        public Func<TopButton, Texture2D, Texture2D> PreDrawBackground;

        #endregion

        #region Public Variables

        public Color BGActiveColor = Color.Green;
        public Color BGInactiveColor = Color.White;
        public Color BGHoveredColor = Color.Yellow;
        
        public int FrameCount;
        public int FrameDelay;

        public SpriteEffects Effects;

        public string Name;
        
        public Texture2D BGActiveTexture;
        public Texture2D BGHoveredTexture;
        public Texture2D BGInactiveTexture;
        public Texture2D Texture;
        
        public Vector2 Position;
        public Vector2 Offset;
        public Vector2 Size;
        public Vector2 TextureSize;
        public Vector2 TextureFrame;

        #endregion

        #endregion

        public TopButton(string name, Vector2 size, Vector2 position,
                         Texture2D texture, Vector2 textureSize, Vector2 textureFrame,
                         Texture2D bgActiveTexture, Texture2D bgInactiveTexture, Texture2D bgHoveredTexture,
                         Func<bool> isActive, Action<TopButton> onClick, Func<float> getAlpha, Action<TopButton> onHover = null,
                         int frameCount = 1, int frameDelay = 4, Func<TopButton, Texture2D, Texture2D> preDrawBackground = null) {
            
            Name = name;
            Size = size;
            Position = position;
            Texture = texture;
            TextureSize = textureSize;
            TextureFrame = textureFrame;
            BGActiveTexture = bgActiveTexture;
            BGInactiveTexture = bgInactiveTexture;
            BGHoveredTexture = bgHoveredTexture;
            IsActive = isActive;
            OnClick = onClick;
            GetAlpha = getAlpha;
            OnHover = onHover;
            Effects = SpriteEffects.None;
            FrameCount = frameCount;
            FrameDelay = frameDelay;
            PreDrawBackground = preDrawBackground;

        }

        public TopButton(string name, Vector2 size, Vector2 position,
                         Texture2D texture, Vector2 textureSize, Vector2 textureFrame,
                         Texture2D bgActiveTexture, Texture2D bgInactiveTexture, Texture2D bgHoveredTexture, SpriteEffects spriteEffects,
                         Func<bool> isActive, Action<TopButton> onClick, Func<float> getAlpha, Action<TopButton> onHover = null,
                         int frameCount = 1, int frameDelay = 4, Func<TopButton, Texture2D, Texture2D> preDrawBackground = null) {

            Name = name;
            Size = size;
            Position = position;
            Texture = texture;
            TextureSize = textureSize;
            TextureFrame = textureFrame;
            BGActiveTexture = bgActiveTexture;
            BGInactiveTexture = bgInactiveTexture;
            BGHoveredTexture = bgHoveredTexture;
            IsActive = isActive;
            OnClick = onClick;
            GetAlpha = getAlpha;
            OnHover = onHover;
            Effects = spriteEffects;
            FrameCount = frameCount;
            FrameDelay = frameDelay;
            PreDrawBackground = preDrawBackground;

        }

        #region Public Functions

        public void Draw(SpriteBatch sb) {

            Offset = default;
            DrawBackground(sb);
            DrawTexture(sb);

        }

        public void DrawWithOffset(SpriteBatch sb, int xOffset = 0, int yOffset = 0) {

            Offset = new Vector2(xOffset, yOffset);
            DrawBackground(sb);
            DrawTexture(sb);

        }

        public bool IsHovered() {
            return new Rectangle(xPosition + _clickedPos, yPosition + _clickedPos, (int)Size.X, (int)Size.Y).Contains(Main.MouseScreen.ToPoint());
        }

        public void Update(bool shouldCheckHover) {

            if (GetAlpha != null)
                _alpha = GetAlpha();

            if (FrameCount > 1) {

                _frameDelayCounter--;

                if (_frameDelayCounter <= 0) {

                    _frameDelayCounter = FrameDelay;

                    _currentFrame++;

                    if (_currentFrame >= FrameCount)
                        _currentFrame = 0;

                }

            }

            if (!shouldCheckHover || !IsHovered()) {
                _clickedPos = 0;
                return;
            }

            OnHover?.Invoke(this);

            if (Main.mouseLeft && (DrawInterface.ButtonWithFocus == null || DrawInterface.ButtonWithFocus == Name)) {
                DrawInterface.ButtonWithFocus = Name;
                _clickedPos = 2;
            }
            else {
                _clickedPos = 0;
            }
            
            if ((DrawInterface.LeftClicked || DrawInterface.LeftLongClick) && DrawInterface.ButtonWithFocus == Name && OnClick != null && DateTime.Now > _clickDelay) {

                OnClick(this);
                _clickDelay = DateTime.Now.AddMilliseconds(50);

            }

        }

        public void UpdatePos(Vector2 pos) {

            Position = pos;

        }

        #endregion

        #region Private Functions

        private void DrawBackground(SpriteBatch sb) {

            Color color;
            Texture2D texture;

            if (IsHovered()) {
                color = BGHoveredColor;
                texture = BGHoveredTexture;
            }
            else if (IsActive != null && IsActive()) {
                color = BGActiveColor;
                texture = BGActiveTexture;
            }
            else {
                color = BGInactiveColor;
                texture = BGInactiveTexture;
            }

            if (PreDrawBackground != null)
                texture = PreDrawBackground(this, texture);

            if (_clickedPos == 0) {

                if (texture == null) {

                    Drawing.DrawBox(sb, xPosition + 2, yPosition + 2, Size.X, Size.Y, Color.Black * 0.5f * _alpha);
                    Drawing.DrawBox(sb, xPosition + 1, yPosition + 1, Size.X, Size.Y, Color.Black * _alpha);

                }
                else {

                    sb.Draw(texture, new Vector2(xPosition + 2, yPosition + 2), Color.Black * 0.5f * _alpha);
                    sb.Draw(texture, new Vector2(xPosition + 1, yPosition + 1), Color.Black * _alpha);

                }

            }

            if (texture == null) {
                Drawing.DrawBox(sb, xPosition + _clickedPos, yPosition + _clickedPos, Size.X, Size.Y, color * _alpha);
            }
            else {
                sb.Draw(texture, new Vector2(xPosition + _clickedPos, yPosition + _clickedPos), Color.White * _alpha);
            }

        }

        private void DrawTexture(SpriteBatch sb) {

            if (Texture == null)
                return;

            float posX = (Size.X / 2f) + xPosition + _clickedPos;
            float posY = (Size.Y / 2f) + yPosition + _clickedPos;

            float targetSize = 24f;
            float largestSide = Math.Max(TextureSize.X, TextureSize.Y);
            float scale = targetSize / largestSide;

            sb.Draw(
                texture: Texture,
                position: new Vector2(posX, posY),
                sourceRectangle: new Rectangle((int)TextureFrame.X, (int)(TextureFrame.Y + (TextureSize.Y + 2) * _currentFrame), (int)TextureSize.X, (int)TextureSize.Y),
                color: Color.White * _alpha,
                rotation: 0f,
                origin: new Vector2(TextureSize.X / 2f, TextureSize.Y / 2f),
                scale: scale,
                effects: Effects,
                layerDepth: 1f
            );

        }

        #endregion

    }

}
