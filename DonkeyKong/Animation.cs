using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class Animation
    {
        public string name;

        private Texture2D spriteSheet;
        public int spriteHeight;
        public int spriteWidth;

        public int frameCount;
        public int frame;
        public float frameSpeed;
        private float time = 0;
        public bool ended;
        public SpriteEffects spriteEffect = SpriteEffects.None;
        public bool play = true;

        public Animation(Texture2D spriteSheet, float frameSpeed, int frameCount, string name)
        {
            this.spriteSheet = spriteSheet;
            this.frameSpeed = frameSpeed;
            this.frameCount = frameCount;
            time = 0;
            ended = false;

            spriteHeight = spriteSheet.Height;
            spriteWidth = spriteSheet.Width / frameCount;

            this.name = name;
        }
        public void Play(GameTime gameTime)
        {
            if (!play) time = frameSpeed;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time >= frameSpeed && play)
            {
                if (frame < frameCount - 1)
                {
                    frame++;
                    time = 0;
                }
                else
                {
                    Reset();
                }
            }
        }
        public void Reset()
        {
            time = 0;
            frame = 0;
            ended = true;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            Rectangle viewFrame = new Rectangle(spriteWidth * frame, 0, spriteWidth, spriteHeight);
            spriteBatch.Draw(spriteSheet, position, viewFrame, Color.White, 0, new Vector2(0, 0), scale, spriteEffect, 0);
        }
    }
}
