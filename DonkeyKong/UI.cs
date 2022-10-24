using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class UI
    {
        public Texture2D texture2D;
        public Vector2 position;
        Vector2 origin;
        public float scale;
        public UI(ContentManager contentManager, string load, Vector2 position, float scale = 1)
        {
            texture2D = contentManager.Load<Texture2D>(load);
            origin = new Vector2(texture2D.Width / 2,texture2D.Height / 2);
            this.position = position + (origin * scale);
            this.scale = scale;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, position, null, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
