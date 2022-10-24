using DonkeyKong;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DonkeyKong
{
    internal class AnimationManager
    {
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        public float time = 0.1f;
        public float scale = 1;
        string playing;

        public void Play(string name, GameTime gameTime)
        {
            if (playing == null) playing = name;
            if (playing != name)
            {
                animations[playing].Reset();
                playing = name;
                animations[playing].ended = false;
            }

            animations[name].Play(gameTime);
        }
        public void ResetPlaying()
        {
            if (playing != null) animations[playing].Reset();
        }
        public bool IsAnimationNull()
        {
            return (playing == null)? true: false;
        }
        public void StopAnimation()
        {
            animations[playing].play = false;
        }
        public void StartAnimation()
        {
            animations[playing].play = true;
        }
        public string GetPlayingAnimation()
        {
            return playing;
        }
        public bool HasEnded()
        {
            if (playing != null)
                if (animations[playing].ended) return true;
            return false;
        }
        public void LoadAnimations(ContentManager contentManager, float speed)
        {
            #region Player
            Animation walk = new Animation(contentManager.Load<Texture2D>("walk"), speed, 3, "walk");
            animations.Add(walk.name, walk);
            Animation stay = new Animation(contentManager.Load<Texture2D>("stay"), speed, 1, "stay");
            animations.Add(stay.name, stay);
            Animation air = new Animation(contentManager.Load<Texture2D>("air"), speed, 1, "air");
            animations.Add(air.name, air);
            Animation climb = new Animation(contentManager.Load<Texture2D>("climb"), speed, 2, "climb");
            animations.Add(climb.name, climb);
            Animation stayLadder = new Animation(contentManager.Load<Texture2D>("stayLadder"), speed, 1, "stayLadder");
            animations.Add(stayLadder.name, stayLadder);
            Animation dead = new Animation(contentManager.Load<Texture2D>("dead"), speed * 2, 4, "dead");
            animations.Add(dead.name, dead);
            Animation hammer = new Animation(contentManager.Load<Texture2D>("killerHammerT"), speed, 2, "hammer");
            animations.Add(hammer.name, hammer);
            #endregion
            #region Flame
            Animation enemy = new Animation(contentManager.Load<Texture2D>("enemy"), speed, 2, "enemy");
            animations.Add(enemy.name, enemy);
            #endregion
            #region Kong
            Animation kongStay = new Animation(contentManager.Load<Texture2D>("KongStay"), speed, 1, "KongStay");
            animations.Add(kongStay.name, kongStay);
            Animation kongAngry = new Animation(contentManager.Load<Texture2D>("KongAngry"), speed, 2, "KongAngry");
            animations.Add(kongAngry.name, kongAngry);
            Animation kongFalling = new Animation(contentManager.Load<Texture2D>("KongFall"), speed, 1, "KongFall");
            animations.Add(kongFalling.name, kongFalling);
            Animation kongDefeated = new Animation(contentManager.Load<Texture2D>("KongDefeated"), speed, 1, "KongDefeated");
            animations.Add(kongDefeated.name, kongDefeated);
            #endregion
            #region Game Screens
            Animation startScreen = new Animation(contentManager.Load<Texture2D>("startScreen"), speed, 2, "startScreen");
            animations.Add(startScreen.name, startScreen);
            Animation gameOver = new Animation(contentManager.Load<Texture2D>("gameOver"), speed, 2, "gameOver");
            animations.Add(gameOver.name, gameOver);
            Animation winScreen = new Animation(contentManager.Load<Texture2D>("winScreen"), speed, 2, "winScreen");
            animations.Add(winScreen.name, winScreen);
            #endregion
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            if (playing != null) animations[playing].Draw(spriteBatch, position, scale);
        }
    }
}
