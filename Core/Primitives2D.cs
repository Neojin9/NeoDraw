using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeoDraw.Core {

    public static class Primitives2D {

        private static readonly Dictionary<string, List<Vector2>> CircleCache = new Dictionary<string, List<Vector2>>();
        private static Texture2D _pixel;

        private static List<Vector2> CreateCircle(double radius, int sides) {
            
            string key = radius + "x" + sides;
            
            if (CircleCache.ContainsKey(key))
                return CircleCache[key];
            
            List<Vector2> list = new List<Vector2>();
            double num = Math.PI * 2 / sides;
            
            for (double num2 = 0; num2 < Math.PI * 2; num2 += num)
                list.Add(new Vector2((float)(radius * Math.Cos(num2)), (float)(radius * Math.Sin(num2))));
            
            list.Add(new Vector2((float)(radius * Math.Cos(0.0)), (float)(radius * Math.Sin(0.0))));
            CircleCache.Add(key, list);
            
            return list;

        }

        private static List<Vector2> CreateArc(float radius, int sides, float startingAngle, float radians) {
            
            List<Vector2> list = new List<Vector2>();
            
            list.AddRange(CreateCircle(radius, sides));
            list.RemoveAt(list.Count - 1);
            
            double num = 0;
            double num2 = Math.PI * 2 / sides;
            
            while (num + num2 / 2.0 < startingAngle) {
                num += num2;
                list.Add(list[0]);
                list.RemoveAt(0);
            }

            list.Add(list[0]);
            int num3 = (int)(radians / num2 + 0.5);
            list.RemoveRange(num3 + 1, list.Count - num3 - 1);
            
            return list;

        }

        private static void CreateThePixel(GraphicsResource spriteBatch) {

            _pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            _pixel.SetData(new[] { Color.White });

        }

        public static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float radians, Color color) {
            spriteBatch.DrawArc(center, radius, sides, startingAngle, radians, color, 1f);
        }

        public static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float radians, Color color, float thickness) {

            List<Vector2> points = CreateArc(radius, sides, startingAngle, radians);
            DrawPoints(spriteBatch, center, points, color, thickness);

        }

        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color) {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), color, 1f);
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color, float thickness) {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), color, thickness);
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides, Color color) {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), color, 1f);
        }

        public static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides, Color color, float thickness) {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color) {
            spriteBatch.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, 1f);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color, float thickness) {
            spriteBatch.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color) {
            spriteBatch.DrawLine(point1, point2, color, 1f);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness) {

            float length = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2((point2.Y - point1.Y), (point2.X - point1.X));

            spriteBatch.DrawLine(point1, length, angle, color, thickness);

        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color) {
            spriteBatch.DrawLine(point, length, angle, color, 1f);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness) {

            if (_pixel == null)
                CreateThePixel(spriteBatch);

            spriteBatch.Draw(_pixel, point, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0f);

        }

        private static void DrawPoints(SpriteBatch spriteBatch, Vector2 position, IList<Vector2> points, Color color, float thickness) {

            if (points.Count < 2)
                return;

            for (int i = 1; i < points.Count; i++)
                spriteBatch.DrawLine(points[i - 1] + position, points[i] + position, color, thickness);

        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color) {
            spriteBatch.DrawRectangle(rect, color, 1f);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness) {

            spriteBatch.DrawLine(new Vector2(rect.X, rect.Y), new Vector2(rect.Right, rect.Y), color, thickness);
            spriteBatch.DrawLine(new Vector2(rect.X + 1f, rect.Y), new Vector2(rect.X + 1f, rect.Bottom + thickness), color, thickness);
            spriteBatch.DrawLine(new Vector2(rect.X, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, thickness);
            spriteBatch.DrawLine(new Vector2(rect.Right + 1f, rect.Y), new Vector2(rect.Right + 1f, rect.Bottom + thickness), color, thickness);

        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color) {
            spriteBatch.DrawRectangle(new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), color, 1f);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color, float thickness) {
            spriteBatch.DrawRectangle(new Rectangle((int)location.X, (int)location.Y, (int)size.X, (int)size.Y), color, thickness);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color) {

            if (_pixel == null)
                CreateThePixel(spriteBatch);

            spriteBatch.Draw(_pixel, rect, color);

        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float angle) {

            if (_pixel == null)
                CreateThePixel(spriteBatch);

            spriteBatch.Draw(_pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0f);

        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color) {
            spriteBatch.FillRectangle(location, size, color, 0f);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color, float angle) {

            if (_pixel == null)
                CreateThePixel(spriteBatch);

            spriteBatch.Draw(_pixel, location, null, color, angle, Vector2.Zero, size, SpriteEffects.None, 0f);

        }

        public static void FillRectangle(this SpriteBatch spriteBatch, float x, float y, float w, float h, Color color) {
            spriteBatch.FillRectangle(new Vector2(x, y), new Vector2(w, h), color, 0f);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, float x, float y, float w, float h, Color color, float angle) {
            spriteBatch.FillRectangle(new Vector2(x, y), new Vector2(w, h), color, angle);
        }

        public static void PutPixel(this SpriteBatch spriteBatch, float x, float y, Color color) {
            spriteBatch.PutPixel(new Vector2(x, y), color);
        }

        public static void PutPixel(this SpriteBatch spriteBatch, Vector2 position, Color color) {

            if (_pixel == null)
                CreateThePixel(spriteBatch);

            spriteBatch.Draw(_pixel, position, color);

        }

    }

}
