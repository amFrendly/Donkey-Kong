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
    internal class UIDigit
    {
        Texture2D numbers;
        public Vector2 position;
        Vector2 origin;
        public float scale;

        public UIDigit(ContentManager contentManager, Vector2 position, float scale = 1)
        {
            this.scale = scale;
            this.position = position;
            numbers = contentManager.Load<Texture2D>("Numbers");
            origin = new Vector2(3.5f, 3.5f);
        }

        public void Draw(SpriteBatch spriteBatch, char digit)
        {
            int viewPos = int.Parse(digit.ToString()) * 7;
            Rectangle viewDigit = new Rectangle(viewPos, 0, 7, 7);
            spriteBatch.Draw(numbers, position, viewDigit, Color.White, 0, origin, scale, SpriteEffects.None, 0);
        }
    }
}
