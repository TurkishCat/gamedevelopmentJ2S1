using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame_Game.Engine.Animation;
using Monogame_Game.Engine.Animations;
using Monogame_Game.Engine.Input_Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Monogame_Game.Game_Code
{
    public class Joe : Entity
    {
        public bool IsGrounded { get; set; }

        private new float velocity = 200f;
        private float acceleration = 60f;
        private float deacceleration = 40f;
        private float jumpForce = 600f;
        private float gravityScale = 50f;
        private int attackDamage = 1;

        public event Action<int> OnPlayerAttack;
        public event Action OnPlayerDeath;

        public override int GetMaxHealth()
        {
            return 7;
        }

        private bool isAttacking;
        private bool isDead;

        private Animation attack;
        private Animation die;

        public Joe(Vector2 position, Texture2D texture) : base(position, texture)
        {
            isAttacking = false;
            isDead = false;

            List<AnimationFrame> frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.5f),
                new AnimationFrame(16, 16, 0.5f)
            };
            Animation idle = new Animation(frames, true, 0);

            frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f)
            };
            Animation walk = new Animation(frames, true, 16);

            frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f)
            };
            attack = new Animation(frames, false, 32);
            attack.OnAnimationFinished += AttackAnimationFinished;

            frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f),
                new AnimationFrame(16, 16, 0.1f)
            };
            die = new Animation(frames, false, 48);
            die.OnAnimationFinished += DeathAnimationFinished;

            animationManager.AddAnimation("idle", idle);
            animationManager.AddAnimation("walk", walk);
            animationManager.AddAnimation("attack", attack);
            animationManager.AddAnimation("die", die);

            animationManager.SetActiveAnimation("idle");
        }

        private void DeathAnimationFinished()
        {
            OnPlayerDeath?.Invoke();
        }

        private void AttackAnimationFinished()
        {
            isAttacking = false;

            if (Math.Abs(base.velocity.X) > 0.1f)
            {
                animationManager.SetActiveAnimation("walk");
            }
            else
            {
                animationManager.SetActiveAnimation("idle");
            }
        }

        public void ProcessPlayerInput()
        {
            if (isDead) return;

            
            if (InputReader.Instance.IsKeyDown(Keys.Right))
            {
                base.velocity.X += acceleration;
            }
            else if (InputReader.Instance.IsKeyDown(Keys.Left))
            {
                base.velocity.X -= acceleration;
            }
            if (InputReader.Instance.KeyPressed(Keys.Up) && IsGrounded)
            {
                base.velocity.Y = -jumpForce;
            }
        }

        public override void PhysicsUpdate()
        {
            
            base.velocity.Y += gravityScale;

            
            base.velocity.X += Math.Abs(base.velocity.X) > 0.01f ? -deacceleration * Math.Sign(base.velocity.X) : 0f;

            
            if (base.velocity.X > velocity) base.velocity.X = velocity;
            if (base.velocity.X < -velocity) base.velocity.X = -velocity;
            if (base.velocity.Y > jumpForce) base.velocity.Y = jumpForce;
            if (base.velocity.Y < -jumpForce) base.velocity.Y = -jumpForce;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isAttacking && !isDead)
            {
                if (Math.Abs(base.velocity.X) > 25f && animationManager.Animations["walk"] != animationManager.CurrentAnimation)
                {
                    animationManager.SetActiveAnimation("walk");
                }
                else if (Math.Abs(base.velocity.X) <= 25f && animationManager.Animations["idle"] != animationManager.CurrentAnimation)
                {
                    animationManager.SetActiveAnimation("idle");
                }

                if (InputReader.Instance.KeyPressed(Keys.Down))
                {
                    animationManager.SetActiveAnimation("attack");
                    Settings.PlaySoundEffect("attack");
                    OnPlayerAttack?.Invoke(attackDamage);
                    isAttacking = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void OnDeath()
        {
            animationManager.SetActiveAnimation("die");
            Settings.PlaySoundEffect("death");
            isDead = true;
        }

        protected override float GetInvolnurableTime()
        {
            return 0.5f;
        }

        public override bool ShouldColliderWithWorld()
        {
            return true;
        }
    }
}
