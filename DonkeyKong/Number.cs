using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class Number
    {
        Vector2 position;
        UIDigit digitTemplate;
        SpriteBatch spriteBatch;
        public Number(SpriteBatch spriteBatch, UIDigit template, Vector2 position)
        {
            digitTemplate = template;
            this.position = position;
            this.spriteBatch = spriteBatch;
        }

        public void Draw(string number)
        {
            int digit = 0;
            for(int i = number.Length -1; i >= 0; i--)
            {
                digitTemplate.position = new Vector2(position.X - 8 * i * digitTemplate.scale, position.Y);
                digitTemplate.Draw(spriteBatch, number[digit]);
                digit++;
            }
        }
    }
}
