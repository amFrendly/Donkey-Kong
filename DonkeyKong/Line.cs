using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace DonkeyKong
{
    internal class Line
    {
        private Texture2D _texture;
        private SpriteBatch spriteBatch;
        public Color color;
        public float thickness;
        public Vector2 start;
        public Vector2 end;

        public Line(SpriteBatch spriteBatch, Color color, float thickness)
        {
            _texture = GetTexture(spriteBatch);
            this.spriteBatch = spriteBatch;
            this.color = color;
            this.thickness = thickness;
        }
        public Line(SpriteBatch spriteBatch, float thickness, Vector2 start, Vector2 end)
        {
            _texture = GetTexture(spriteBatch);
            this.spriteBatch = spriteBatch;
            this.color = Color.White;
            this.thickness = thickness;
            this.start = start;
            this.end = end;
        }
        public Line GetCopy()
        {
            return new Line(spriteBatch, color, thickness);
        }
        private Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }
        public void Draw(Vector2 point1, Vector2 point2)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(point1, distance, angle, color, thickness);
        }
        public void Draw(Vector2 point1, Vector2 point2, float distance)
        {
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(point1, distance, angle, color, thickness);
        }
        public void Draw()
        {
            var distance = Vector2.Distance(start, end);
            var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            DrawLine(start, distance, angle, color, thickness);
        }
        public void Draw(float distance)
        {
            var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            DrawLine(start, distance, 1000, color, thickness);
        }
        private void DrawLine(Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
    }
}
