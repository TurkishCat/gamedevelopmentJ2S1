using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame_Game.Engine;
using Monogame_Game.Engine.Animations;
using System;
using System.Diagnostics;

namespace Monogame_Game.Game_Code
{
    public abstract class Entity
    {
        protected Vector2 position;
        protected Vector2 velocity;
        protected int currentHealth;
        protected Texture2D texture;
        protected AnimationManager animationManager;
        protected bool active;
        protected float involnurableTimer;

        protected int facingDirection;

        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
        public int CurrentHealth { get { return currentHealth; } }
        public Texture2D Texture { get { return texture; } }
        public bool Active { get { return active; } }
        // Collision bool
        public bool HitWall { get; set; }

        protected abstract void OnDeath();
        protected abstract float GetInvolnurableTime();
        public abstract int GetMaxHealth();
        public abstract bool ShouldColliderWithWorld();

        protected Entity(Vector2 position, Texture2D texture)
        {
            this.position = position;
            velocity = Vector2.Zero;
            this.texture = texture;

            currentHealth = GetMaxHealth();

            facingDirection = 1; // Right

            active = true;

            animationManager = new AnimationManager();
        }

        public virtual void PhysicsUpdate()
        {
            if (!active) return;
        }
        public virtual void Update(GameTime gameTime)
        {
            involnurableTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Math.Abs(velocity.X) > 20f && Math.Sign(velocity.X) != Math.Sign(facingDirection))
                facingDirection *= -1; //draai

            animationManager.Update(gameTime);
        }
        public virtual void Draw()
        {
            Rectangle srcRect = animationManager.GetSourceRectangle();
            Settings.SpriteBatch.Draw(texture, position, srcRect, involnurableTimer > 0 ? Color.Black : Color.White, 0f, Vector2.Zero, 1f,
                facingDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f);
        }
        public virtual void TakeDamage(int damage)
        {
            if (involnurableTimer > 0 || !active) return;

            currentHealth -= damage;
            involnurableTimer = GetInvolnurableTime();

            Settings.PlaySoundEffect("damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath();
            }
        }
    }
}
