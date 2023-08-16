using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Monogame_Game.Engine.Animations
{
    public class AnimationManager

    {

        /* Animation flow
         * 1 AnimationFrame's worden gemaakt
         * 2 Animation object aanmaken per animation en AnimationFrame's toevoegen aan de dict(animation behaviour ook in Animation class)
         * 3 AnimationManager class fungeert als centrale manager voor elke unieke entity haar animations (public prop Animations 'housed' alle Animation objecten)
         */

        // Om elke animatie met een object te assoscieren
        public Dictionary<object, Animation> Animations { get; private set; }

        
        private Animation currentAnimation;

       
        public Animation CurrentAnimation { get { return currentAnimation; } }

        
        public AnimationManager()
        {
            Animations = new Dictionary<object, Animation>();
        }

        
        public void AddAnimation(object key, Animation animation)
        {
            Animations.Add(key, animation);
        }

        
        public void SetActiveAnimation(object key)
        {
            // Als animatie al gaande is, stoppen
            if (currentAnimation != null)
            {
                currentAnimation.Stop(false);
            }

            // nieuwe animatie setten
            Animations.TryGetValue(key, out currentAnimation);

            // animatie die geset is, starten
            if (currentAnimation != null)
            {
                currentAnimation.Start();
            }
        }

        // update aanroepen op aninmatie object
        public void Update(GameTime gameTime)
        {
            if (currentAnimation != null)
            {
                currentAnimation.Update(gameTime);
            }
        }


        // geeft rectangle terug van animatie die NU speelt
        public Rectangle GetSourceRectangle()
        {
            Rectangle sourceRect = new Rectangle();

            if (currentAnimation != null)
            {
                sourceRect = currentAnimation.GetSourceRectangle();
            }

            return sourceRect;
        }
    }
}
