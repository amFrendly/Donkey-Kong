using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using System.Windows.Forms;

namespace DonkeyKong
{
    internal class Map
    {
        public enum Key
        {
            bridge = 'b',
            ladder = 'l',
            bridgeLadder = 'k',
            button = 'n',
            enemy = 'e',
            enemySpawn = 'w',
            princess = 'p',
            player = 'm',
            bonus = 'z',
            hammer = 'h',
            kong = 'x',
        }
        float tileSize;
        ContentManager contentManager;
        SpriteBatch spriteBatch;
        string[] map = new string[30];
        public Map(ContentManager contentManager, SpriteBatch spriteBatch, float tileSize = 40)
        {
            this.spriteBatch = spriteBatch;
            this.contentManager = contentManager;
            this.tileSize = tileSize;
            LoadMap();
        }
        public void LoadMap()
        {
            StreamReader sr = new StreamReader(@"map.txt");
            for(int y = 0; y < 20; y++)
            {
                map[y] = sr.ReadLine();
            }

            sr.Close();
        }
        Random rng = new Random();
        public void Get(ref GameObjectHandler gameObjectHandler, ref EnemyManager enemyManager, ref Player player, ref Kong kong)
        {
            Texture2D floor = contentManager.Load<Texture2D>("Bridge");
            Texture2D ladder = contentManager.Load<Texture2D>("Ladder");
            Texture2D ladderBridge = contentManager.Load<Texture2D>("LadderBridge");
            Texture2D button = contentManager.Load<Texture2D>("Button");
            Texture2D fireBarrel = contentManager.Load<Texture2D>("fireBarrel");
            Texture2D princess = contentManager.Load<Texture2D>("Princess");
            Texture2D bonus1 = contentManager.Load<Texture2D>("Bonus1");
            Texture2D hammer = contentManager.Load<Texture2D>("Hammer");

            for (int y = 0; y < 20; y++)
            {
                for(int x = 0; x < 30; x++)
                {
                    switch (map[y][x])
                    {
                        case (char)Key.bridge:
                            gameObjectHandler.gameObjects["Floor"].Add(new GameObject(floor, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.ladder:
                            gameObjectHandler.gameObjects["Ladder"].Add(new GameObject(ladder, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.bridgeLadder:
                            gameObjectHandler.gameObjects["LadderBridge"].Add(new GameObject(ladderBridge, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.button:
                            gameObjectHandler.gameObjects["Button"].Add(new GameObject(button, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.enemySpawn:
                            gameObjectHandler.gameObjects["EnemySpawn"].Add(new GameObject(fireBarrel, spriteBatch, new Vector2(x * tileSize, (y * tileSize) + 10)));
                            break;
                        case (char)Key.enemy:
                            enemyManager.enemies.Add(new Enemy(spriteBatch, contentManager, new Vector2(x * tileSize, y * tileSize), rng.Next(10, 40) / 10f));
                            break;
                        case (char)Key.princess:
                            gameObjectHandler.gameObjects["Princess"].Add(new GameObject(princess, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.bonus:
                            gameObjectHandler.gameObjects["Bonus"].Add(new GameObject(bonus1, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.hammer:
                            gameObjectHandler.gameObjects["Hammer"].Add(new GameObject(hammer, spriteBatch, new Vector2(x * tileSize, y * tileSize)));
                            break;
                        case (char)Key.player:
                            player.gameObject.position = new Vector2(x * tileSize, y * tileSize);
                            player.tileX = x;
                            player.tileY = y;
                            player.gameObject.UpdateCollider();
                            player.gotoPosition.position = player.gameObject.position;
                            break;
                        case (char)Key.kong:
                            kong.gameObject.position = new Vector2((x * tileSize) - 25, (y * tileSize) - 24);
                            kong.gameObject.UpdateCollider();
                            break;
                    }
                }
            }

            gameObjectHandler.fallDownObjcts = GetFallDownObjects(gameObjectHandler);
        }

        public List<List<GameObject>> GetFallDownObjects(GameObjectHandler gameObjectHandler)
        {
            List<List<GameObject>> fallDownObjects = new List<List<GameObject>>();
            List<Vector2> buttonPos = new List<Vector2>();

            for (int i = 0; i < gameObjectHandler.gameObjects["Button"].Count / 2; i++) fallDownObjects.Add(new List<GameObject>());
            gameObjectHandler.gameObjects["Button"].ForEach(i => buttonPos.Add(i.position));

            int index = 0;
            for(int i = 0; i < buttonPos.Count -1; i+=2)
            {
                foreach(GameObject bridge in gameObjectHandler.gameObjects["Floor"])
                {
                    if(bridge.position.Y == buttonPos[i].Y)
                    {
                        if(bridge.position.X > buttonPos[i].X && bridge.position.X < buttonPos[i+1].X)
                        {
                            fallDownObjects[index].Add(bridge);
                        }
                    }
                }
                index++;
            }

            return fallDownObjects;
        }
    }
}
