using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Game.Engine.UI
{
    public static class ButtonMiddle
    {
        // midden vinden om text te centreren
        public static Vector2 GetTextPosition(SpriteFont spriteFont, string text, Vector2 position, float scale = 1f)
        {
            var strLen = spriteFont.MeasureString(text);
            return position - new Vector2(strLen.X / 2, strLen.Y / 2) * scale;
        }
    }
}
