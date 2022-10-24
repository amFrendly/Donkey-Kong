using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class UIHandler
    {
        public List<UI> UIs = new List<UI>();

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(UI UI in UIs)
            {
                UI.Draw(spriteBatch);
            }
        }
    }
}
