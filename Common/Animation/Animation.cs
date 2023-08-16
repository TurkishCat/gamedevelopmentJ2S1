using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Monogame_Game.Engine.Animation;

namespace Monogame_Game.Engine.Animations
{
    

    public class Animation
    {
        // Triggered als animation helemaal klaar is
        public event Action OnAnimationFinished;

        // Dictionary van frames, int is welke animation in loop
        private Dictionary<int, AnimationFrame> animation;

        // Y as van spritesheet
        private int yPosition;

        // index van huidige frame
        private int currentFrame;
        // Frame timer
        private float timer;

        // Is animation active?
        private bool active;
        // bijhouden of animation moet loopen
        private bool loop;


        public Animation(List<AnimationFrame> frames, bool loop = true, int yPosition = 0)
        {

            this.yPosition = yPosition;
            active = false;
            this.loop = loop;

            currentFrame = 0;
            timer = 0;

            animation = new Dictionary<int, AnimationFrame>();


            // alle frames toevoegen aan dictionary
            for (int i = 0; i < frames.Count; i++)
            {
                var frame = new AnimationFrame(frames[i].W, frames[i].H, frames[i].Duration);
                frame.Rectangle = new Rectangle(i * frames[i].W, this.yPosition, frames[i].W, frames[i].H);
                animation.Add(i, frame);
            }
        }

        // Stop animation
        public void Stop(bool shouldReset)
        {
            if (shouldReset) Reset();

            active = false;
        }

        // Start animation
        public void Start()
        {
            Reset();
            active = true;

            var frame = GetCurrentAnimationFrame();

            if (frame == null)
            {
                return;
            }
        }

        // Reset animation
        public void Reset()
        {
            currentFrame = 0;
            timer = 0;
        }

        // Update animation
        public void Update(GameTime gameTime)
        {

            if (!active) return;


            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var frame = GetCurrentAnimationFrame();


            if (frame == null)
            {
                return;
            }

            // Animation finish met timer
            if (timer > frame.Duration)
            {

                timer = 0;

                // Als animation klaar is en !loop
                if (currentFrame == animation.Count - 1 && !loop)
                {

                    OnAnimationFinished?.Invoke();


                    Stop(false);

                    return;
                }
                else
                {

                    currentFrame = (currentFrame + 1) % animation.Count;
                }
            }
        }

        // Rectangle pakken van active frame
        public Rectangle GetSourceRectangle()
        {
            var frame = GetCurrentAnimationFrame();

            if (frame != null)
            {
                return frame.Rectangle;
            }
            else
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }

        // huidige frame van dict
        public AnimationFrame GetCurrentAnimationFrame()
        {
            AnimationFrame frame = null;
            animation.TryGetValue(currentFrame, out frame);
            return frame;
        }
    }
}
