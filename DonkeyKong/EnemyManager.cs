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
    internal class EnemyManager
    {
        public List<Enemy> enemies = new List<Enemy>();

        Timer timer = new Timer(1);
        Random rng = new Random();
        SpriteBatch spriteBatch;
        ContentManager contentManager;
        public EnemyManager(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;
        }
        public void Spawn(GameTime gameTime, GameObjectHandler gameObjectHandler)
        {
            if (gameObjectHandler.gameObjects["EnemySpawn"].Count == 0) return;
            
                timer.Tick(gameTime);
            if(timer.Done())
            {
                Enemy enemy = new Enemy(spriteBatch, contentManager, new Vector2(0, 0), rng.Next(1, 40) / 10f);
                enemy.position = gameObjectHandler.gameObjects["EnemySpawn"][rng.Next(0, gameObjectHandler.gameObjects["EnemySpawn"].Count)].position;
                if (rng.Next(0, 2) == 0)
                {
                    enemy.GoLeft();
                }
                else
                {
                    enemy.GoRight();
                }
                enemies.Add(enemy);
                timer.Reset();
            }
        }
        public void Move(GameObjectHandler collision, GameTime gameTime)
        {
            foreach(Enemy enemy in enemies)
            {
                enemy.CollidingLogic(collision);
                enemy.Move(gameTime);
            }
        }
        public bool Collide(Player player)
        {
            foreach (Enemy enemy in enemies)
            {
                if (player.gameObject.collider.hitBox.Bottom >= enemy.collider.hitBox.Top && player.gameObject.collider.hitBox.Top <= enemy.collider.hitBox.Bottom)
                {
                    if (player.gameObject.collider.hitBox.Left <= enemy.collider.hitBox.Right && player.gameObject.collider.hitBox.Right >= enemy.collider.hitBox.Left)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Draw()
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }
        }
        public void Reset()
        {
            enemies.Clear();
        }
    }
}
