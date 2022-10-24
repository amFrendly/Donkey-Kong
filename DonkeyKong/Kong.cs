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
    internal class Kong
    {
        enum AnimationState
        {
            stay,
            angry,
            fall,
            dead
        }

        public bool dead = false;
        float gravity = 10;
        bool grounded = true;
        float gravityAdd = 10;

        AnimationManager animationManager;
        AnimationState animationState;
        Timer timer;
        public GameObject gameObject;
        public Kong(ContentManager contentManager, SpriteBatch spriteBatch, Vector2 position, float scale = 2)
        {
            animationManager = new AnimationManager();
            animationManager.LoadAnimations(contentManager, 0.5f);
            animationManager.scale = scale;

            animationState = AnimationState.stay;
            timer = new Timer(1);

            gameObject = new GameObject(contentManager.Load<Texture2D>("KongStay"), spriteBatch, position);
            gameObject.position = position;
            gameObject.collider.size = new Vector2(gameObject.collider.size.X * 2, gameObject.collider.size.Y * 2);
            gameObject.UpdateCollider(true);
        }
        public void CollidingLogic(GameObjectHandler collision)
        {
            Rectangle lookAhead = new Rectangle(gameObject.collider.hitBox.X, gameObject.collider.hitBox.Y + (int)gravity, gameObject.collider.hitBox.Width, gameObject.collider.hitBox.Height);
            gameObject.collider.NewCollideInformation(collision, "Floor", lookAhead);
            CollideInfo collideFloorInfo = gameObject.collider.collideInfo;
            if (collideFloorInfo.down && !dead)
            {
                grounded = true;
                gravity = 0;
                gameObject.position.Y = collideFloorInfo.otherDown.Top - gameObject.collider.hitBox.Height;
            }
            else if (!dead)
            {
                grounded = false;
            }


            if (!dead) return;

            #region Floor Collision
            if (collideFloorInfo.down)
            {
                grounded = true;
                gravity = 0;
                gameObject.position.Y = collideFloorInfo.otherDown.Top - gameObject.collider.hitBox.Height;
                animationState = AnimationState.dead;
            }
            else
            {
                grounded = false;
                animationState = AnimationState.fall;
            }
            #endregion
        }
        private void Gravity(GameTime gameTime)
        {
            if (!grounded) gravity = gravity + gravityAdd * (float)gameTime.ElapsedGameTime.TotalSeconds;
            gameObject.position.Y += gravity;
            gameObject.UpdateCollider();
        }
        public void Update(GameObjectHandler collision, GameTime gameTime)
        {
            if (timer.Done() && !dead)
            {
                animationState = AnimationState.angry;
                timer.Reset();
            }


            CollidingLogic(collision);
            Gravity(gameTime);



            Animate(animationState, gameTime);
            timer.Tick(gameTime);
        }
        void Animate(AnimationState animationState, GameTime gameTime)
        {
            switch(animationState)
            {
                case AnimationState.stay:
                    animationManager.Play("KongStay", gameTime);
                    break;
                case AnimationState.angry:
                    animationManager.Play("KongAngry", gameTime);
                    break;
                case AnimationState.fall:
                    animationManager.Play("KongFall", gameTime);
                    break;
                case AnimationState.dead:
                    animationManager.Play("KongDefeated", gameTime);
                    break;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            animationManager.Draw(spriteBatch, gameObject.position);
            gameObject.collider.Draw();
        }
        public void Reset()
        {
            dead = false;
        }

    }
}
