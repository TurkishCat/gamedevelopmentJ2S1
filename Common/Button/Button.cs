using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Game.Engine.Input_Manager;
using System;
using System.Diagnostics;

namespace Monogame_Game.Engine.UI
{
    public class Button : DrawableGameComponent
    {
        private Rectangle button;
        private SpriteBatch spriteBatch;

        private Texture2D texture;
        private SpriteFont font;
        private string text;
        private bool highlighted;

        public event Action OnPressed;

        public Button(Game game, Rectangle button, SpriteBatch spriteBatch) : base(game)
        {
            this.button = button;
            this.spriteBatch = spriteBatch;

            texture = null;
            font = null;
            text = string.Empty;
            highlighted = false;
        }

        // text van button
        public void SetText(string text, SpriteFont font)
        {
            this.font = font;
            this.text = text;
        }
        // button texture
        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // als muis over button hoverd = true
            if (button.Contains(InputReader.Instance.Mouse.X, InputReader.Instance.Mouse.Y))
                highlighted = true;
            else
                highlighted = false;

            if (highlighted)
                if (InputReader.Instance.MousePressed(MouseButton.Left))
                    OnPressed?.Invoke();

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();

            if (texture != null)
                spriteBatch.Draw(texture, new Vector2(button.X, button.Y), Color.White);

            if (font != null && text != string.Empty)
                spriteBatch.DrawString(font, text, ButtonMiddle.GetTextPosition(font, text, button.Center.ToVector2()), Color.White);

            spriteBatch.End();
        }
    }
}
