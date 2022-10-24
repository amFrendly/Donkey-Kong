using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class CollideInfo
    {
        public bool left = false;
        public bool right = false;
        public bool up = false;
        public bool down = false;
        public Rectangle otherDown;
        public Rectangle otherUp;
        public Rectangle otherLeft;
        public Rectangle otherRight;
    }
}
