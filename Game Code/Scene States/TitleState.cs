using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Game.Engine.Game_State_Management;
using Monogame_Game.Engine.Input_Manager;
using Monogame_Game.Engine.UI;
using System;
using System.Diagnostics;

namespace Monogame_Game.Game_Code.Game_States
{
    public class TitleState : SceneState
    {
        private SpriteBatch spriteBatch;

        private Texture2D bg0;
        private Button playButton;
        private Button quitButton;

        public TitleState(Game1 game, StateManager stateManager) : base(game, stateManager)
        {
        }

        public override void Start()
        {
            base.Start();
            spriteBatch = Settings.SpriteBatch;
            bg0 = Settings.GetTexture("bg0");

            playButton = new Button(game, new Rectangle((game.GraphicsDevice.Viewport.Width / 2)
                - 100, 250, 200, 200), spriteBatch);
            playButton.SetText("Play", Settings.Font);

            quitButton = new Button(game, new Rectangle((game.GraphicsDevice.Viewport.Width / 2)
                - 100, 400, 200, 200), spriteBatch);
            quitButton.SetText("Quit", Settings.Font);


            playButton.OnPressed += PlayButtonPressed;
            quitButton.OnPressed += QuitButtonPressed;

            Add(playButton);
            Add(quitButton);
        }

        public override void Leave()
        {
            

            playButton.OnPressed -= PlayButtonPressed;
            quitButton.OnPressed -= QuitButtonPressed;
        }

        private void QuitButtonPressed()
        {
            game.Exit();
        }
        private void PlayButtonPressed()
        {
            stateManager.SwitchState(game.Level1);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg0, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 4.5f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(Settings.Font, "Joe The Little Boy",
                ButtonMiddle.GetTextPosition(Settings.Font, "Joe The Little Boy",
                new Vector2(game.GraphicsDevice.Viewport.Width / 2,
                100), 1), Color.White, 0f, Vector2.Zero, 1,
                SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
