using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace DonkeyKong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;

        GameObjectHandler gameObjectsHandler;

        Player player;
        Kong kong;
        EnemyManager enemyManager;

        UIHandler uiHandler = new UIHandler();

        UIDigit digit;
        Number bonusNumber;
        Number highScoreNumber;
        Number scoreNumber;

        Timer bonusTimer;

        AnimationManager screenAnimation;
        Texture2D endScreen;

        GameStates gameStates = GameStates.start;
        HighScore highScore;
        SpriteFont font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player = new Player(Content, _spriteBatch, new Vector2(9, 18), 40);
            kong = new Kong(Content, _spriteBatch, new Vector2(535, 336));

            gameObjectsHandler = new GameObjectHandler(_spriteBatch, Content);
            enemyManager = new EnemyManager(_spriteBatch, Content);

            screenAnimation = new AnimationManager();
            screenAnimation.LoadAnimations(Content, 1f);

            Map map = new Map(Content, _spriteBatch);
            map.Get(ref gameObjectsHandler, ref enemyManager, ref player, ref kong);

            UI bonusFrameUI = new UI(Content, "BonusFrame", new Vector2(800, 150), 4);
            UI highScoreFrame = new UI(Content, "HighScore", new Vector2(325, 50), 4);
            UI scoreFrame = new UI(Content, "1Up", new Vector2(50, 50), 4);


            uiHandler.UIs.Add(scoreFrame);
            uiHandler.UIs.Add(highScoreFrame);
            uiHandler.UIs.Add(bonusFrameUI);

            digit = new UIDigit(Content, new Vector2(100,100), 4);
            bonusNumber = new Number(_spriteBatch, digit, new Vector2(942, 195));
            bonusTimer = new Timer(1);

            highScoreNumber = new Number(_spriteBatch, digit, new Vector2(563, 96));
            scoreNumber = new Number(_spriteBatch, digit, new Vector2(224, 96));

            endScreen = Content.Load<Texture2D>("gameOver");

            highScore = new HighScore(new Vector2(1000, 200));
            font = Content.Load<SpriteFont>("File");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (gameStates)
            {
                #region start
                case GameStates.start:
                    screenAnimation.Play("startScreen", gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameStates = GameStates.play;
                        Reset();
                    }
                    break;
                #endregion
                #region play
                case GameStates.play:
                    #region Enemy
                    enemyManager.Spawn(gameTime, gameObjectsHandler);
                    enemyManager.Move(gameObjectsHandler, gameTime);
                    #endregion
                    #region Player
                    player.invinsible.Tick(gameTime);
                    player.CollidingLogic(ref gameObjectsHandler, ref enemyManager);
                    player.Move(Keyboard.GetState(), gameTime);
                    player.HammerHit(enemyManager, gameObjectsHandler);
                    player.UpdateHammer(gameTime);
                    player.CheckDeath(ref gameStates);
                    player.CheckWin(gameObjectsHandler, ref gameStates, ref highScore);
                    player.Animate(gameTime);
                    if (bonusTimer.Done())
                    {
                        player.bonus -= 100;
                        bonusTimer.Reset();
                    }
                    player.invinsible.Tick(gameTime);

                    if(player.win)
                    {
                        bonusTimer.stop = true;
                        kong.dead = true;
                    }
                    #endregion
                    bonusTimer.Tick(gameTime);
                    kong.Update(gameObjectsHandler, gameTime);
                    break;
                #endregion
                #region win
                case GameStates.win:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameStates = GameStates.play;
                        Reset();
                    }
                    screenAnimation.Play("winScreen", gameTime);
                    break;
                #endregion
                #region lose
                case GameStates.lose:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameStates = GameStates.play;
                        Reset();
                    }
                    screenAnimation.Play("gameOver", gameTime);
                    break;
                    #endregion
            }

            base.Update(gameTime);
        }

        Color backColor = Color.Black;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backColor);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            switch(gameStates)
            {
                #region Start
                case GameStates.start:
                    screenAnimation.Draw(_spriteBatch, new Vector2());
                    highScore.Draw(_spriteBatch, font);
                    break;
                #endregion
                #region Play
                case GameStates.play:
                    gameObjectsHandler.Draw(_spriteBatch);
                    kong.Draw(_spriteBatch);
                    player.Draw(_spriteBatch);
                    enemyManager.Draw();
                    uiHandler.Draw(_spriteBatch);
                    bonusNumber.Draw((player.bonus >= 0) ? player.bonus.ToString() : "0");
                    highScoreNumber.Draw(player.score.ToString());
                    scoreNumber.Draw(player.score.ToString());
                    break;
                #endregion
                #region Win
                case GameStates.win:
                    screenAnimation.Draw(_spriteBatch, new Vector2());
                    highScore.Draw(_spriteBatch, font);
                    break;
                #endregion
                #region Lose
                case GameStates.lose:
                    screenAnimation.Draw(_spriteBatch, new Vector2());
                    highScore.Draw(_spriteBatch, font);
                    break;
                    #endregion
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
        void Reset()
        {
            player.Reset();
            enemyManager.Reset();
            gameObjectsHandler.Reset();
            kong.Reset();

            Map map = new Map(Content, _spriteBatch);
            map.Get(ref gameObjectsHandler, ref enemyManager, ref player, ref kong);
            bonusTimer.stop = false;

        }
    }
}