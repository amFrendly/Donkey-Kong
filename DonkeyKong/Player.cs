using System;
using DonkeyKong.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;

namespace DonkeyKong
{
    internal class Player
    {
        public enum MoveState
        {
            left,
            right
        }
        public enum ClimbDirection
        {
            up,
            down,
            none
        }
        public enum AnimationState
        {
            walkLeft,
            walkRight,
            climb,
            stayClimb,
            air,
            stay,
            stayLadder,
            dead
        }

        #region Varibles
        Keys jumpKey = Keys.Space;
        Keys climbUpKey = Keys.W;
        Keys climbDownKey = Keys.S;
        Keys leftKey = Keys.A;
        Keys rightKey = Keys.D;
        AnimationState animationState = AnimationState.stay;

        const float gravityConst = 10;
        public float gravity;
        float gravityAdd = 40;
        public GameObject gameObject;
        bool grounded = false;
        private float tileSize;
        float jumpPower = 10;
        bool canClimb = false;
        bool climbing = false;
        MoveState moveState;
        ClimbDirection climbDirection = ClimbDirection.none;
        public int tileX = 0;
        public int tileY = 0;

        bool inputLeft = false;
        bool inputRight = false;
        bool stayClimb = false;
        public Collider gotoPosition;
        AnimationManager animationsManager;
        SpriteBatch spriteBatch;
        public Timer invinsible;
        LivesUI livesHUD;
        Timer hammerTimer = new Timer(5);
        public bool hammerMode = false;
        GameObject hammer;
        #endregion
        #region Variables that needs reset
        int bonusStart = 5000;
        int livesStart;
        float speedStart;

        public int bonus;
        public bool win = false;
        public int lives;
        public int score = 0;
        float speed;
        public Vector2 velocity = new Vector2(0, 0);
        #endregion

        public Player(ContentManager content, SpriteBatch spriteBatch, Vector2 position, float tileSize, float speed = 3, int lives = 3, float invinsibleTime = 1) 
        {
            invinsible = new Timer(invinsibleTime);

            this.lives = lives;
            livesStart = lives;
            this.tileSize = tileSize;
            this.spriteBatch = spriteBatch;
            this.speed = speed;
            speedStart = speed;
            gameObject = new GameObject(spriteBatch, new Vector2(position.X * 40, position.Y * 40), new Vector2(tileSize, tileSize));
            tileX = (int)position.X;
            tileY = (int)position.Y;
            animationsManager = new AnimationManager();
            animationsManager.LoadAnimations(content, 0.1f);

            animationsManager.scale = tileSize / 16;
            gravity = gravityConst;
            jumpPower = 10;

            gotoPosition = new Collider(spriteBatch,  new Rectangle((int)gameObject.position.X, (int)gameObject.position.Y, (int)gameObject.collider.size.X, (int)gameObject.collider.size.Y));
            gotoPosition.showHitBox = false;

            livesHUD = new LivesUI(new UI(content, "lives", new Vector2(100,120), 3));
            hammer = new GameObject(spriteBatch, new Vector2(), new Vector2(40, 40));
            hammer.UpdateCollider();

            bonus = bonusStart;
        }
        public void SetKeys(Keys jumpKey, Keys climbUpKey, Keys climbDownKey, Keys leftKey, Keys rightKey)
        {
            this.jumpKey = jumpKey;
            this.climbUpKey = climbUpKey;
            this.climbDownKey = climbDownKey;
            this.leftKey = leftKey;
            this.rightKey = rightKey;
        }
        public void CollidingLogic(ref GameObjectHandler collision, ref EnemyManager enemyManager)
        {
            Rectangle lookAhead = new Rectangle(gameObject.collider.hitBox.X + (int)velocity.X, gameObject.collider.hitBox.Y + (int)velocity.Y + (int)gravity, gameObject.collider.hitBox.Width, gameObject.collider.hitBox.Height);
            gameObject.collider.NewCollideInformation(collision, "Floor", lookAhead);
            CollideInfo collideFloorInfo = gameObject.collider.collideInfo;
            #region Hammer Collision
            if (collision.CollideWith("Hammer", gameObject.collider.hitBox))
            {
                collision.gameObjects["Hammer"].Remove(collision.CollideWith("Hammer", gameObject));
                hammerTimer.Reset();
                hammerMode = true;
            }
            #endregion
            #region Bonus Collision
            if (collision.CollideWith("Bonus", gameObject.collider.hitBox))
            {
                collision.gameObjects["Bonus"].Remove(collision.CollideWith("Bonus", gameObject));
                score += 300;
                bonus += 300;
            }
            #endregion
            #region Floor Collision
            if (collideFloorInfo.down)
            {
                gravity = 0;
                grounded = true;
                climbing = false;
                gameObject.position.Y = collideFloorInfo.otherDown.Top - gameObject.collider.hitBox.Height;
            }
            else if(collision.CollideWith("LadderBridge", gameObject.collider.hitBox) == false)
            {
                grounded = false;
                animationState = AnimationState.air;
            }
            #endregion
            #region Ladder Collision
            if (collision.CollideWith("Ladder", gameObject.collider.hitBox) && !inputLeft && !inputRight)
            {

                if (climbDirection == ClimbDirection.up)
                {
                    climbing = true;
                }
                else if (climbDirection == ClimbDirection.none && grounded)
                {
                    canClimb = true;
                    climbing = false;
                }
            }
            else
            {
                canClimb = false;
            }
            #endregion
            #region LadderBridge Collision
            if (collision.CollideWith("LadderBridge", gameObject.collider.hitBox) && !inputLeft && !inputRight)
            {
                Collider ladderBridgeCollider = collision.CollideWith("LadderBridge", gameObject).collider;

                if(MathF.Abs(((int)gameObject.position.Y / (int)tileSize) - tileY) > 1) tileY = (int)gameObject.position.Y / (int)tileSize;

                canClimb = true;
                if(ladderBridgeCollider.hitBox.Top >= gameObject.collider.hitBox.Bottom - speed)
                {
                    if (climbDirection == ClimbDirection.down && grounded)
                    {
                        climbing = true;
                    }
                    else
                    {
                        gravity = 0;
                        grounded = true;
                        gameObject.position.Y = ladderBridgeCollider.hitBox.Top - gameObject.collider.hitBox.Height;
                        climbing = false;
                    }
                }
                else
                {
                    climbing = true;
                }
            }
            else 
            {
                if (collision.CollideWith("Ladder", gameObject.collider.hitBox) == false)
                {
                    climbing = false;
                }
            }
            #endregion
            #region Button Collision
            gameObject.collider.NewCollideInformation(collision, "Button", lookAhead);
            collideFloorInfo = gameObject.collider.collideInfo;
            if (collideFloorInfo.down)
            {
                GameObject ground = collision.CollideWith("Button", gameObject);
                grounded = true;
                gravity = 0;
                collision.gameObjects["Button"].Remove(ground);

                bonus += 100;
                score += 100;

                #region All Buttons Pressed
                if (collision.gameObjects["Button"].Count == 0 && !win)
                {
                    win = true;
                    score += 100;

                    enemyManager.enemies.Clear();
                    collision.gameObjects["EnemySpawn"].Clear();
                    collision.gameObjects["Bonus"].Clear();
                    collision.gameObjects["Hammer"].Clear();

                    int addPos = collision.fallDownObjcts.Count -1;
                    for (int index = 0; index < collision.fallDownObjcts.Count; index++)
                    {
                        for (int i = 0; i < collision.fallDownObjcts[index].Count; i++)
                        {
                            collision.fallDownObjcts[index][i].position = new Vector2(collision.fallDownObjcts[index][i].position.X, 720 - (addPos * 40));
                            collision.fallDownObjcts[index][i].UpdateCollider();
                        }
                        addPos--;
                    }
                }
                #endregion
            }
            #endregion
            #region Enemy Collision
            if (invinsible.Done() && enemyManager.Collide(this))
            {
                lives--;
                invinsible.Reset();
            }
            #endregion
            #region Out of bounds
            if(gameObject.position.Y >= 1000)
            {
                lives -= 1;
            }
            #endregion
        }
        #region Move
        public void Move(KeyboardState keyboardState, GameTime gameTime)
        {
            StickToTile();

            WalkInput(keyboardState);
            JumpInput(keyboardState);
            ClimbInput(keyboardState);
            CLimb();
            Walk();

            Gravity(gameTime);

            StickHammer();
            gameObject.UpdateCollider();
        }
        private void StickToTile()
        {
            if(climbing)
            {
                if ((Vector2.Distance(gameObject.position, new Vector2(gameObject.position.X, tileY * tileSize)) <= speed + speed))
                {
                    gameObject.position.Y = tileY * tileSize;
                    animationState = AnimationState.stayClimb;
                    stayClimb = true;
                }

                gotoPosition.position = new Vector2(gameObject.position.X, tileY * tileSize);
            }
            else
            {
                if ((Vector2.Distance(gameObject.position, new Vector2(tileX * tileSize, gameObject.position.Y)) <= speed + speed))
                {
                    gameObject.position.X = tileX * tileSize;
                    inputLeft = false;
                    inputRight = false;
                    if (canClimb)
                    {
                        stayClimb = true;
                        tileY = (int)gameObject.position.Y / (int)tileSize;
                        animationState = AnimationState.stayLadder;
                    }
                    else
                    {
                        animationState = AnimationState.stay;
                    }
                }

                gotoPosition.position = new Vector2(tileX * tileSize, gameObject.position.Y);
            }
            gotoPosition.UpdatePos();
        }
        private void StickHammer()
        {
            if (hammerMode && moveState == MoveState.left)
            {
                hammer.collider.position = new Vector2(gameObject.position.X - 40, gameObject.position.Y);
                hammer.collider.UpdatePos();
            }
            else if (hammerMode && moveState == MoveState.right)
            {
                hammer.collider.position = new Vector2(gameObject.position.X + 40, gameObject.position.Y);
                hammer.collider.UpdatePos();
            }

        }
        private void WalkInput(KeyboardState keyboardState)
        {
            if (climbing) return;

            if (keyboardState.IsKeyDown(rightKey) && !inputRight)
            {
                tileX++;
                inputRight = true;
                animationState = AnimationState.walkRight;
                moveState = MoveState.right;
            }
            if (keyboardState.IsKeyDown(leftKey) && !inputLeft)
            {
                tileX--;
                inputLeft = true;
                animationState = AnimationState.walkLeft;
                moveState = MoveState.left;
            }
        }
        private void JumpInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(jumpKey) && grounded)
            {
                gravity = -jumpPower;
            }
        }
        private void ClimbInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(climbDownKey))
            {
                climbDirection = ClimbDirection.down;
            }
            else if (keyboardState.IsKeyDown(climbUpKey))
            {
                climbDirection = ClimbDirection.up;
            }
            else
            {
                climbDirection = ClimbDirection.none;
            }

            if (!climbing) return;


            if (stayClimb && climbDirection == ClimbDirection.up)
            {
                tileY--;
                animationState = AnimationState.climb;
                stayClimb = false;
            }
            else if (stayClimb && climbDirection == ClimbDirection.down)
            {
                tileY++;
                animationState = AnimationState.climb;
                stayClimb = false;
            }
        }
        private void Walk()
        {
            if (gameObject.position.X < gotoPosition.position.X)
            {
                gameObject.position.X += speed;
            }
            else if (gameObject.position.X > gotoPosition.position.X)
            {
                gameObject.position.X -= speed;
            }
        }
        private void Gravity(GameTime gameTime)
        {
            if (!climbing)
            {
                if(!grounded) animationState = AnimationState.air;
                gravity = gravity + gravityAdd * (float)gameTime.ElapsedGameTime.TotalSeconds;
                gameObject.position.Y += gravity;
            }
        }
        private void CLimb()
        {
            if(climbing)
            {
                if (gameObject.position.Y < gotoPosition.position.Y)
                {
                    gameObject.position.Y += speed;
                    animationState = AnimationState.climb;
                }
                else if (gameObject.position.Y > gotoPosition.position.Y)
                {
                    gameObject.position.Y -= speed;
                    animationState = AnimationState.climb;
                }
            }
        }
        #endregion
        public void HammerHit(EnemyManager enemyManager, GameObjectHandler collision)
        {
            for(int i = enemyManager.enemies.Count -1; i >= 0; i--)
            {
                if (collision.CollideWith(enemyManager.enemies[i].collider, hammer.collider))
                {
                    bonus += 100;
                    score += 100;
                    enemyManager.enemies.RemoveAt(i);
                }
            }
        }
        public void UpdateHammer(GameTime gameTime)
        {
            if(hammerMode)
            {
                hammerTimer.Tick(gameTime);
                if(hammerTimer.Done())
                {
                    hammerMode = false;
                    hammerTimer.Reset();
                    hammer.collider.position = new Vector2(-40, -40);
                    hammer.collider.UpdatePos();
                }
            }

            if(lives <= 0 && bonus <= 0)
            {
                hammerMode = false;
                hammerTimer.Reset();
                hammer.collider.position = new Vector2(-40, -40);
                hammer.collider.UpdatePos();
            }
        }
        public void Animate(GameTime gameTime)
        {
            FlipAnimation(moveState);
            if (hammerMode)
            {
                animationsManager.Play("hammer", gameTime);
                return;
            }
            switch (animationState)
            {
                case AnimationState.stay:
                    animationsManager.Play("stay", gameTime);
                    break;
                case AnimationState.walkLeft:
                    animationsManager.Play("walk", gameTime);
                    break;
                case AnimationState.walkRight:
                    animationsManager.Play("walk", gameTime);
                    break;
                case AnimationState.air:
                    animationsManager.Play("air", gameTime);
                    break;
                case AnimationState.climb:
                    animationsManager.StartAnimation();
                    animationsManager.Play("climb", gameTime);
                    break;
                case AnimationState.stayClimb:
                    animationsManager.StopAnimation();
                    animationsManager.Play("climb", gameTime);
                    break;
                case AnimationState.stayLadder:
                    animationsManager.Play("stayLadder", gameTime);
                    break;
                case AnimationState.dead:
                    animationsManager.Play("dead", gameTime);
                    break;
            }
        }
        private void FlipAnimation(MoveState direction)
        {
            if (animationsManager.IsAnimationNull()) return;

            if (direction == MoveState.left)
            {
                animationsManager.animations[animationsManager.GetPlayingAnimation()].spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else if (direction == MoveState.right)
            {
                animationsManager.animations[animationsManager.GetPlayingAnimation()].spriteEffect = SpriteEffects.None;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            gameObject.collider.Draw();
            gotoPosition.Draw();
            if(hammerMode)
            {
                if(moveState == MoveState.left)
                {
                    animationsManager.Draw(spriteBatch, new Vector2(gameObject.position.X -26, gameObject.position.Y - 28));
                }
                else
                {
                    animationsManager.Draw(spriteBatch, new Vector2(gameObject.position.X , gameObject.position.Y - 28));
                }
                hammer.collider.Draw();
            }
            else
            {
                animationsManager.Draw(spriteBatch, gameObject.position);
            }

            livesHUD.Draw(spriteBatch, lives);
        }
        public void CheckDeath(ref GameStates gameStates)
        {
            if(lives <= 0 || bonus <= 0)
            {
                if (hammerMode) hammerMode = false;
                animationState = AnimationState.dead;
                if(animationsManager.HasEnded() && animationsManager.GetPlayingAnimation() == "dead")
                {
                    gameStates = GameStates.lose;
                }
            }
        }
        public void CheckWin(GameObjectHandler collision, ref GameStates gameStates, ref HighScore highScore)
        {
            if (collision.CollideWith("Princess", gameObject.collider.hitBox))
            {
                gameStates = GameStates.win;
                highScore.Add(this);
                highScore.Save();
            }
        }
        public void Reset()
        {
            bonus = bonusStart;
            win = false;
            lives = livesStart;
            score = 0;
            speed = speedStart;
            velocity = new Vector2(0, 0);
            gravity = 0;
        }
    }
}
