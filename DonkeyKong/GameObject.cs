using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DonkeyKong
{
    internal class GameObject
    {

        public Map.Key key;

        protected SpriteBatch spriteBatch;
        public Collider collider;
        public Rectangle hitBox;
        public Vector2 position;
        private Vector2 size;
        private Texture2D sprite;
        public GameObject() { }
        public GameObject(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            collider = new Collider(spriteBatch, hitBox);
            this.size = size;
            this.position = position;
            this.spriteBatch = spriteBatch;
            
            collider.UpdatePos();
        }
        public GameObject(Texture2D sprite, SpriteBatch spriteBatch, Vector2 position)
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, (int)sprite.Width, (int)sprite.Height);
            collider = new Collider(spriteBatch, hitBox);
            this.position = position;
            this.spriteBatch = spriteBatch;
            this.sprite = sprite;

            collider.UpdatePos();
        }
        public void UpdateCollider(bool onlySize = false)
        {
            if(position != collider.position || onlySize)
            {
                collider.position = position;
                collider.UpdatePos();
            }
        }
        public virtual void Draw()
        {
            if(sprite != null) spriteBatch.Draw(sprite, position, Color.White);
        }
    }
}
