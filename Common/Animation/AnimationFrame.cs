using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Game.Engine.Animation
{
    public class AnimationFrame
    {
        // width & height van frame
        public int W, H;
        // duration van frame
        public float Duration;
        // frame van de spritesheet
        public Rectangle Rectangle;


        public AnimationFrame(int width, int height, float duration)
        {

            W = width;
            H = height;


            Duration = duration;
        }
    }
}
