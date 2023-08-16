using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame_Game.Engine.Game_State_Management;
using Monogame_Game.Engine.UI;
using System;

namespace Monogame_Game.Game_Code.Game_States
{
    public class EndState : SceneState
    {
        private SpriteBatch spriteBatch;

        private Texture2D bg0;
        private Button mainMenuButton;

        public EndState(Game1 game, StateManager stateManager) : base(game, stateManager)
        {
        }

        public override void Start()
        {
            base.Start();
            spriteBatch = Settings.SpriteBatch;
            bg0 = Settings.GetTexture("bg0");

            mainMenuButton = new Button(game, new Rectangle((game.GraphicsDevice.Viewport.Width / 2)
                - 100, 500, 200, 200), spriteBatch);
            mainMenuButton.SetText("Main Menu", Settings.Font);


            mainMenuButton.OnPressed += MainMenuButtonPressed;
            Add(mainMenuButton);
        }
        public override void Leave()
        {
            

            mainMenuButton.OnPressed -= MainMenuButtonPressed;
        }

        private void MainMenuButtonPressed()
        {
            stateManager.SwitchState(game.TitleState);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg0, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 4.5f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.DrawString(Settings.Font, "You Won!",
                ButtonMiddle.GetTextPosition(Settings.Font, "You Won!",
                new Vector2(game.GraphicsDevice.Viewport.Width / 2,
                100), 1), Color.White, 0f, Vector2.Zero, 1,
                SpriteEffects.None, 0f);

            spriteBatch.DrawString(Settings.Font, "Coins collected: " + Settings.Score,
                ButtonMiddle.GetTextPosition(Settings.Font, "Coins collected: " + Settings.Score,
                new Vector2(game.GraphicsDevice.Viewport.Width / 2,
                250), 1), Color.White, 0f, Vector2.Zero, 1,
                SpriteEffects.None, 0f);
            spriteBatch.End();
        }
    }
}
