using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DonkeyKong.Player;

namespace DonkeyKong
{
    internal class Enemy : GameObject
    {
        AnimationManager animationManager;

        float gravity = 10;
        bool grounded = false;
        float gravityAdd = 10;
        public Vector2 velocity;
        float moveDirection = 1;
        public Enemy(SpriteBatch spriteBatch, ContentManager contentManager, Vector2 position, float speed = 3)
        {
            this.spriteBatch = spriteBatch;

            animationManager = new AnimationManager();
            animationManager.LoadAnimations(contentManager, 0.1f);

            hitBox = new Rectangle((int)position.X, (int)position.Y, 16, 16);
            collider = new Collider(spriteBatch, hitBox);
            this.position = position;
            velocity.X = speed;
            collider.UpdatePos();
        }
        public void CollidingLogic(GameObjectHandler collision)
        {
            Rectangle lookAhead = new Rectangle(collider.hitBox.X + (int)velocity.X, collider.hitBox.Y + (int)velocity.Y + (int)gravity, collider.hitBox.Width, collider.hitBox.Height);
            collider.NewCollideInformation(collision, "Floor", lookAhead);
            CollideInfo collideFloorInfo = collider.collideInfo;
            #region Floor Collision
            if (collideFloorInfo.down)
            {
                grounded = true;
                gravity = 0;
                position.Y = collideFloorInfo.otherDown.Top - collider.hitBox.Height;
            }
            else if(collision.CollideWith("Button", collider.hitBox))
            {
                GameObject ground = collision.CollideWith("Button", this);
                grounded = true;
                gravity = 0;
                position.Y = ground.collider.hitBox.Top - collider.hitBox.Height;
            }
            else if (collision.CollideWith("LadderBridge", collider.hitBox))
            {
                GameObject ground = collision.CollideWith("LadderBridge", this);
                grounded = true;
                gravity = 0;
                position.Y = ground.collider.hitBox.Top - collider.hitBox.Height;
            }
            else
            {
                moveDirection *= -1;
                grounded = false;
            }
            #endregion
        }
        private void Gravity(GameTime gameTime)
        {
            if(!grounded) gravity = gravity + gravityAdd * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += gravity;
        }
        public void Move(GameTime gameTime)
        {
            Gravity(gameTime);
            animationManager.Play("enemy", gameTime);
            position.X += velocity.X * moveDirection;
            UpdateCollider();
        }
        public void GoLeft()
        {
            moveDirection = -1;
        }
        public void GoRight()
        {
            moveDirection = 1;
        }
        public override void Draw()
        {
            animationManager.Draw(spriteBatch, position);
            collider.Draw();
        }
    }
}
