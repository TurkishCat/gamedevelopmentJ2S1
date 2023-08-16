using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame_Game.Engine.Animation;
using Monogame_Game.Engine.Animations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Game.Game_Code
{
    public class FlyingEnemy : Entity
    {
        private new float velocity = 30;
        private float acceleration = 20;
        private float deacceleration = 10;
        private int maxHealth = 1;
        private float involnurableTime = 0.1f;
        private float distanceToPlayerToStartChase = 140f;

        private float stunnedTimeOnHit = 1f;

        private float stunnedTimer;


        public FlyingEnemy(Vector2 position, Texture2D texture) : base(position, texture)
        {
            stunnedTimer = 0;

            List<AnimationFrame> frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
            };
            Animation fly = new Animation(frames);

            animationManager.AddAnimation("fly", fly);
            animationManager.SetActiveAnimation("fly");
        }

        public override int GetMaxHealth()
        {
            return maxHealth;
        }
        protected override float GetInvolnurableTime()
        {
            return involnurableTime;
        }
        protected override void OnDeath()
        {
            active = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (stunnedTimer > 0) return;

            if (Vector2.Distance(Settings.Player.Position, position) <= distanceToPlayerToStartChase)
            {
                // follow player
                Vector2 dir = Settings.Player.Position - position;
                dir.Normalize();

                base.velocity += dir * acceleration;
            }

            
            base.velocity.X += Math.Abs(base.velocity.X) > 0.01f ? -deacceleration * Math.Sign(base.velocity.X) : 0f;
            base.velocity.Y += Math.Abs(base.velocity.Y) > 0.01f ? -deacceleration * Math.Sign(base.velocity.Y) : 0f;

            
            if (base.velocity.X > velocity) base.velocity.X = velocity;
            if (base.velocity.X < -velocity) base.velocity.X = -velocity;
            if (base.velocity.Y > velocity) base.velocity.Y = velocity;
            if (base.velocity.Y < -velocity) base.velocity.Y = -velocity;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            stunnedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void TakeDamage(int damage)
        {
            //stunnedTimer = stunnedTimeOnHit; Hp verandert naar 1 van alle enemies
            //base.velocity = Vector2.Zero;

            base.TakeDamage(damage);
        }

        public override bool ShouldColliderWithWorld()
        {
            return false;
        }
    }
}
