using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DonkeyKong
{
    internal class GameObjectHandler
    {
        
        public Dictionary<string, List<GameObject>> gameObjects = new Dictionary<string, List<GameObject>>();
        public List<List<GameObject>> fallDownObjcts = new List<List<GameObject>>();
        public GameObjectHandler(SpriteBatch spriteBatch, ContentManager contentManager)
        {
            gameObjects.Add("Ladder", new List<GameObject>());
            gameObjects.Add("LadderBridge", new List<GameObject>());
            gameObjects.Add("Floor", new List<GameObject>());
            gameObjects.Add("Button", new List<GameObject>());
            gameObjects.Add("EnemySpawn", new List<GameObject>());
            gameObjects.Add("Princess", new List<GameObject>());
            gameObjects.Add("Bonus", new List<GameObject>());
            gameObjects.Add("Hammer", new List<GameObject>());
        }
        public GameObject CollideWith(string objectsCategory, GameObject gameObject)
        {
            foreach(GameObject gameObjectColliding in gameObjects[objectsCategory])
            {
                if (CollideWith(gameObject.collider, gameObjectColliding.collider))
                {
                    return gameObjectColliding;
                }
            }
            return null;
        }
        public bool CollideWith(Collider collider1, Collider collider2)
        {
            if(collider1.hitBox.Bottom >= collider2.hitBox.Top && collider1.hitBox.Top <= collider2.hitBox.Bottom)
            {
                if(collider1.hitBox.Left <= collider2.hitBox.Right && collider1.hitBox.Right >= collider2.hitBox.Left)
                {
                    return true;
                }
            }
            return false;
        }
        public bool CollideWith(string objectsCategory, Rectangle hitBox)
        {
            foreach (GameObject gameObjectColliding in gameObjects[objectsCategory])
            {
                if (CollideWith(hitBox, gameObjectColliding.collider))
                {
                    return true;
                }
            }
            return false;
        }
        public bool CollideWith(Rectangle hitBox, Collider collider)
        {
            if (hitBox.Bottom >= collider.hitBox.Top && hitBox.Top <= collider.hitBox.Bottom)
            {
                if (hitBox.Left < collider.hitBox.Right && hitBox.Right > collider.hitBox.Left)
                {
                    return true;
                }
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<string, List<GameObject>> iterate in gameObjects)
            {
                for(int i = 0; i < gameObjects[iterate.Key].Count; i++)
                {
                    gameObjects[iterate.Key][i].Draw();
                    gameObjects[iterate.Key][i].collider.Draw();

                }
            }
        }
        public void Reset()
        {
            gameObjects["Ladder"].Clear();
            gameObjects["LadderBridge"].Clear();
            gameObjects["Floor"].Clear();
            gameObjects["Button"].Clear();
            gameObjects["EnemySpawn"].Clear();
            gameObjects["Princess"].Clear();
            gameObjects["Bonus"].Clear();
            gameObjects["Hammer"].Clear();
        }
    }
}
