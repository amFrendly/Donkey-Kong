using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class Timer
    {
        public float count { get; private set;}
        float countTo;
        public bool stop = false;
        public Timer(float countTo = 0)
        {
            Reset();
            this.countTo = countTo;
        }
        public void CountTo(float countTo)
        {
            this.countTo = countTo;
        }
        public void Tick(GameTime gameTime)
        {
            if(!stop)
            count += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public bool Done()
        {
            if (count >= countTo)
            {
                return true;
            }
            return false;
        }
        public void Reset()
        {
            count = 0;
        }

    }
}
