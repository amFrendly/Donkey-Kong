using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong.Content
{
    internal class LivesUI
    {
        Vector2 position;
        UI template;
        public LivesUI(UI template)
        {
            this.template = template;
            position = template.position;
        }

        public void Draw(SpriteBatch spriteBatch, int lives)
        {
            for(int i = 0; i < lives; i++)
            {
                template.position = new Vector2(position.X + (template.texture2D.Width + 2) * i * template.scale, position.Y);
                template.Draw(spriteBatch);
            }
        }

    }
}
