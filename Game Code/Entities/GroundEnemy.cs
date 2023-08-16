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
    public class GroundEnemy : Entity
    {
        private new float velocity = 30;
        private float acceleration = 20;
        private float deacceleration = 10;
        private float gravityScale = 100f;
        private float maxYVel = 700;
        private int maxHealth = 1;
        private float involnurableTime = 0.1f;

        private float stunnedTimeOnHit = 1f;

        private float stunnedTimer;

        // 1 rechts, -1 links
        private int moveDirection;

        public GroundEnemy(Vector2 position, Texture2D texture) : base(position, texture)
        {
            moveDirection = 1;
            stunnedTimer = 0;

            List<AnimationFrame> frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
            };
            Animation walk = new Animation(frames);

            animationManager.AddAnimation("walk", walk);
            animationManager.SetActiveAnimation("walk");
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

            
            base.velocity.X += moveDirection * acceleration;

            
            base.velocity.Y += gravityScale;

            
            base.velocity.X += Math.Abs(base.velocity.X) > 0.01f ? -deacceleration * Math.Sign(base.velocity.X) : 0f;

            
            if (base.velocity.X > velocity) base.velocity.X = velocity;
            if (base.velocity.X < -velocity) base.velocity.X = -velocity;
            if (base.velocity.Y > maxYVel) base.velocity.Y = maxYVel;
            if (base.velocity.Y < -maxYVel) base.velocity.Y = -maxYVel;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            stunnedTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (HitWall)
            {
                moveDirection *= -1;
                HitWall = false;
            }
        }

        public override void TakeDamage(int damage)
        {
            
            //stunnedTimer = stunnedTimeOnHit; Hp verandert naar 1 van alle enemies dus niet meer nodig
            //base.velocity = Vector2.Zero;

            base.TakeDamage(damage);
        }

        public override bool ShouldColliderWithWorld()
        {
            return true;
        }
    }
}
