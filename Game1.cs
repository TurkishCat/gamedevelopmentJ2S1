using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Monogame_Game.Engine.Game_State_Management;
using Monogame_Game.Engine.Input_Manager;
using Monogame_Game.Game_Code;
using Monogame_Game.Game_Code.Game_States;
using Monogame_Game.Game_Code.Game_States.Levels;

namespace Monogame_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private StateManager stateManager;

        
        private Song backgroundMusic;

        public TitleState TitleState { get; private set; }
        public GameOverState GameOverState { get; private set; }
        public EndState YouWinState { get; private set; }
        public Level1 Level1 { get; private set; }
        public Level2 Level2 { get; private set; }
        public Level3 Level3 { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            IsMouseVisible = true;

            
            graphics.PreferredBackBufferWidth = Settings.ScreenWidth;
            graphics.PreferredBackBufferHeight = Settings.ScreenHeight;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Settings.SpriteBatch = spriteBatch;

            
            stateManager = new StateManager(this);
            
            Components.Add(stateManager);

            
            TitleState = new TitleState(this, stateManager);
            GameOverState = new GameOverState(this, stateManager);
            YouWinState = new EndState(this, stateManager);
            Level3 = new Level3(this, stateManager, YouWinState);
            Level2 = new Level2(this, stateManager, Level3);
            Level1 = new Level1(this, stateManager, Level2);

            
            stateManager.SwitchState(TitleState);
        }

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";
            Settings.Content = Content;

            
            backgroundMusic = Content.Load<Song>("backgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(backgroundMusic);

            
            Settings.LoadSfx("attack", "attack");
            Settings.LoadSfx("damage", "damage");
            Settings.LoadSfx("death", "death");
            Settings.LoadSfx("coin", "coinSfx");

            
            Settings.Font = Content.Load<SpriteFont>("font");

            
            Settings.LoadTexture("bg0", "background0");
            Settings.LoadTexture("bg1", "background1");
            Settings.LoadTexture("bg2", "background2");
            Settings.LoadTexture("tiles", "tiles");

            Settings.LoadTexture("player", "playerAnimations");
            Settings.LoadTexture("enemy", "enemy");
            Settings.LoadTexture("enemy2", "enemy2");
            Settings.LoadTexture("heart", "heart");
            Settings.LoadTexture("coin", "coin");
        }

        protected override void Update(GameTime gameTime)
        {
            
            InputReader.Instance.UpdateInput();

            
            Settings.GameDrawMatrix = Matrix.CreateTranslation(new Vector3(-Settings.CameraPosition.X, -Settings.CameraPosition.Y, 0))
                * Matrix.CreateScale(Settings.PixelScale) *
                Matrix.CreateTranslation(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f, 0);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}