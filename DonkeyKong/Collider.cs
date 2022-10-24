using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DonkeyKong
{
    internal class Collider
    {
        public Vector2 size;
        public Vector2 position;
        public CollideInfo collideInfo = new CollideInfo();
        public Rectangle hitBox = new Rectangle();
        public bool showHitBox = false;
        public void UpdatePos()
        {
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            hitBox.Width = (int)size.X;
            hitBox.Height = (int)size.Y;

            if (showHitBox)
            {
                lineTop.start = new Vector2(hitBox.Left, hitBox.Top);
                lineTop.end = new Vector2(hitBox.Right, hitBox.Top);

                lineBottom.start = new Vector2(hitBox.Left, hitBox.Bottom);
                lineBottom.end = new Vector2(hitBox.Right, hitBox.Bottom);

                lineLeft.start = new Vector2(hitBox.Left, hitBox.Top);
                lineLeft.end = new Vector2(hitBox.Left, hitBox.Bottom);

                lineRight.start = new Vector2(hitBox.Right, hitBox.Top);
                lineRight.end = new Vector2(hitBox.Right, hitBox.Bottom);
            }
        }
        public void NewCollideInformation(GameObjectHandler gameObjectHandler, string objectsCategory, Rectangle? lookAhead)
        {
            CollideInfo collideInfo = new CollideInfo();
            foreach (GameObject gameObjectColliding in gameObjectHandler.gameObjects[objectsCategory])
            {
                if (gameObjectHandler.CollideWith((lookAhead == null)? hitBox: lookAhead.Value, gameObjectColliding.collider))
                {
                    Rectangle gameObjectHitBox = gameObjectColliding.collider.hitBox;
                    float left = MathF.Abs(hitBox.Left - gameObjectHitBox.Right);
                    float right = MathF.Abs(hitBox.Right - gameObjectHitBox.Left);
                    float top = MathF.Abs(hitBox.Top - gameObjectHitBox.Bottom);
                    float bottom = MathF.Abs(hitBox.Bottom - gameObjectHitBox.Top) -1;

                    if (bottom < right && bottom < top && bottom < left)
                    {
                        collideInfo.down = true;
                        collideInfo.otherDown = gameObjectHitBox;
                    }
                    if (left < right && left < top && left < bottom)
                    {
                        collideInfo.left = true;
                        collideInfo.otherLeft = gameObjectHitBox;
                    }
                    else if (right < left && right < top && right < bottom)
                    {
                        collideInfo.right = true;
                        collideInfo.otherRight = gameObjectHitBox;
                    }
                    else if (top < right && top < left && top < bottom)
                    {
                        collideInfo.up = true;
                        collideInfo.otherUp = gameObjectHitBox;
                    }
                }
            }
            this.collideInfo = collideInfo;
        }

        Line lineTop;
        Line lineBottom;
        Line lineLeft;
        Line lineRight;
        SpriteBatch spriteBatch;
        public Collider(SpriteBatch spriteBatch, Rectangle hitBox)
        {
            position = new Vector2(hitBox.Left, hitBox.Top);
            size = new Vector2(hitBox.Width, hitBox.Height);
            this.spriteBatch = spriteBatch;

            lineTop = new Line(spriteBatch, 1, new Vector2(hitBox.Left, hitBox.Top), new Vector2(hitBox.Right, hitBox.Top));
            lineBottom = new Line(spriteBatch, 1, new Vector2(hitBox.Left, hitBox.Bottom), new Vector2(hitBox.Right, hitBox.Bottom));
            lineLeft = new Line(spriteBatch, 1, new Vector2(hitBox.Left, hitBox.Top), new Vector2(hitBox.Left, hitBox.Bottom));
            lineRight = new Line(spriteBatch, 1, new Vector2(hitBox.Right, hitBox.Top), new Vector2(hitBox.Right, hitBox.Bottom));

            UpdatePos();
        }
        public void Draw()
        {
            if(showHitBox)
            {
                lineTop.Draw();
                lineBottom.Draw();
                lineLeft.Draw();
                lineRight.Draw();
            }
        }
    }
}