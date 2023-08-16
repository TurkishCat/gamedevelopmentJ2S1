using Monogame_Game.Engine.Game_State_Management;
using Microsoft.Xna.Framework.Graphics;
using Monogame_Game.Engine.Animations;
using Microsoft.Xna.Framework;
using Monogame_Game.Engine.Input_Manager;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Monogame_Game.Engine;
using System;
using System.Diagnostics;
using Monogame_Game.Engine.UI;
using Monogame_Game.Engine.Animation;

namespace Monogame_Game.Game_Code.Game_States
{
    public class PlayState : SceneState
    {
        protected SpriteBatch spriteBatch;
        protected Joe player;
        protected List<Entity> characters;

        
        protected string[] levelMap;
        // met 2 mappen werken wanneer coin word gepakt "verandert map ook".
        private string[] tmpLevelMap;

        protected int levelWidth => levelMap[0].Length;
        protected int levelHeight => levelMap.Length;

        private SceneState nextLevel;
        private Texture2D tilesTexture;
        private Texture2D bg0;
        private Texture2D bg1;
        private Texture2D bg2;

        private Vector2 lastPlayerGroundedPos;

        private AnimationManager heartAnimation;
        private AnimationManager coinAnimation;

        public PlayState(Game1 game, StateManager stateManager, SceneState nextLevel) : base(game, stateManager)
        {
            // Volgende level is verandering van scenestate
            this.nextLevel = nextLevel;

            heartAnimation = new AnimationManager();
            List<AnimationFrame> frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f)
            };
            heartAnimation.AddAnimation("beat", new Animation(frames));
            heartAnimation.SetActiveAnimation("beat");

            coinAnimation = new AnimationManager();
            frames = new List<AnimationFrame>()
            {
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f),
                new AnimationFrame(16, 16, 0.2f)
            };
            coinAnimation.AddAnimation("rotate", new Animation(frames));
            coinAnimation.SetActiveAnimation("rotate");
        }

        
        public override void Start()
        {
            base.Start();

            
            tmpLevelMap = (string[])levelMap.Clone();

            characters = new List<Entity>();
            Settings.CameraPosition = Vector2.Zero;
            player = Settings.Player;
            player.OnPlayerAttack += PlayerAttack;
            player.OnPlayerDeath += PlayerDeath;

            spriteBatch = Settings.SpriteBatch;

            tilesTexture = Settings.GetTexture("tiles");
            bg0 = Settings.GetTexture("bg0");
            bg1 = Settings.GetTexture("bg1");
            bg2 = Settings.GetTexture("bg2");
        }

        public override void Leave()
        {
            

            player.OnPlayerAttack -= PlayerAttack;
            player.OnPlayerDeath -= PlayerDeath;
        }

        private void PlayerAttack(int damage)
        {
            for (int i = 1; i < characters.Count; i++)
            {
                var character = characters[i];
                if (character.Active)
                {
                    if (Math.Abs(player.Position.X - character.Position.X) <= 22f &&
                        Math.Abs(character.Position.Y - player.Position.Y) <= 20f)
                    {
                        character.TakeDamage(damage);
                    }
                }
            }
        }

        private void PlayerDeath()
        {
            stateManager.SwitchState(game.GameOverState);
        }

        public char GetMapTile(int x, int y)
        {
            if (x >= 0 && x < levelWidth && y >= 0 && y < levelHeight)
            {
                return tmpLevelMap[y][x];
            }
            else
            {
                return ' ';
            }
        }
        public bool IsMapTileSolid(int x, int y)
        {
            return GetMapTile(x / Settings.TileWidth, y / Settings.TileHeight) == 'G' ||
                GetMapTile(x / Settings.TileWidth, y / Settings.TileHeight) == '1' ||
                GetMapTile(x / Settings.TileWidth, y / Settings.TileHeight) == '2';
        }
        public bool IsMapTileASpike(int x, int y)
        {
            return GetMapTile(x / Settings.TileWidth, y / Settings.TileHeight) == 'S';
        }
        public bool IsMapTileACoin(int x, int y)
        {
            return GetMapTile(x / Settings.TileWidth, y / Settings.TileHeight) == 'M';
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            
            heartAnimation.Update(gameTime);
            coinAnimation.Update(gameTime);

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Eerst input lezen dan update
            player.ProcessPlayerInput();

            
            player.PhysicsUpdate();

            // nieuwe positie calculation
            Vector2 newPlayerPos = player.Position + (player.Velocity * delta);

            // Alle collisions

            // Coin check
            if (IsMapTileACoin(GetPos(newPlayerPos.X), GetPos(newPlayerPos.Y)))
            {
                var line = tmpLevelMap[GetPos(newPlayerPos.Y) / Settings.TileHeight].ToCharArray();
                line[GetPos(newPlayerPos.X) / Settings.TileWidth] = '.';
                tmpLevelMap[GetPos(newPlayerPos.Y) / Settings.TileHeight] = new string(line);

                Settings.PlaySoundEffect("coin");
                Settings.Score += 1;
            }
            if (IsMapTileACoin(GetPos(newPlayerPos.X), GetPos(newPlayerPos.Y + TileHeightToPos(1))))
            {
                var line = tmpLevelMap[GetPos(newPlayerPos.Y + TileHeightToPos(1)) / Settings.TileHeight].ToCharArray();
                line[GetPos(newPlayerPos.X) / Settings.TileWidth] = '.';
                tmpLevelMap[GetPos(newPlayerPos.Y + TileHeightToPos(1)) / Settings.TileHeight] = new string(line);

                Settings.PlaySoundEffect("coin");
                Settings.Score += 1;
            }
            if (IsMapTileACoin(GetPos(newPlayerPos.X + TileWidthToPos(1)), GetPos(newPlayerPos.Y)))
            {
                var line = tmpLevelMap[GetPos(newPlayerPos.Y) / Settings.TileHeight].ToCharArray();
                line[GetPos(newPlayerPos.X + TileWidthToPos(1)) / Settings.TileWidth] = '.';
                tmpLevelMap[GetPos(newPlayerPos.Y) / Settings.TileHeight] = new string(line);

                Settings.PlaySoundEffect("coin");
                Settings.Score += 1;
            }
            if (IsMapTileACoin(GetPos(newPlayerPos.X + TileWidthToPos(1)), GetPos(newPlayerPos.Y + TileHeightToPos(1))))
            {
                var line = tmpLevelMap[GetPos(newPlayerPos.Y + TileHeightToPos(1)) / Settings.TileHeight].ToCharArray();
                line[GetPos(newPlayerPos.X + TileWidthToPos(1)) / Settings.TileWidth] = '.';
                tmpLevelMap[GetPos(newPlayerPos.Y + TileHeightToPos(1)) / Settings.TileHeight] = new string(line);

                Settings.PlaySoundEffect("coin");
                Settings.Score += 1;
            }


            // Spikes
            if (IsMapTileASpike(GetPos(newPlayerPos.X), GetPos(newPlayerPos.Y)) ||
                IsMapTileASpike(GetPos(newPlayerPos.X), GetPos(newPlayerPos.Y + TileHeightToPos(1))) ||
                IsMapTileASpike(GetPos(newPlayerPos.X + TileWidthToPos(1)), GetPos(newPlayerPos.Y)) ||
                IsMapTileASpike(GetPos(newPlayerPos.X + TileWidthToPos(1)), GetPos(newPlayerPos.Y + TileHeightToPos(1))))
            {
                
                player.TakeDamage(1);
                // Terug naar positie voor dat je damage krijgt
                newPlayerPos = lastPlayerGroundedPos;
            }

            // Out of map
            if (newPlayerPos.Y > (levelHeight + 5) * Settings.TileHeight)
            {
                // instakill
                player.TakeDamage(player.GetMaxHealth());
            }
                
            // Block collisions

            if (player.ShouldColliderWithWorld())
            {
                // X & links
                if (player.Velocity.X <= 0)
                {
                    if (IsMapTileSolid(GetPos(newPlayerPos.X), GetPos(player.Position.Y)) ||
                        IsMapTileSolid(GetPos(newPlayerPos.X), GetPos(player.Position.Y + TileWidthToPos(0.9f))))
                    {
                        newPlayerPos.X = GetPos(player.Position.X);
                        player.Velocity *= new Vector2(0f, 1f);
                    }
                }
                // rechts
                else
                {
                    if (IsMapTileSolid(GetPos(newPlayerPos.X + TileWidthToPos(1)), GetPos(player.Position.Y)) ||
                        IsMapTileSolid(GetPos(newPlayerPos.X + TileWidthToPos(1)), GetPos(player.Position.Y + TileWidthToPos(0.9f))))
                    {
                        newPlayerPos.X = player.Position.X;
                        player.Velocity *= new Vector2(0f, 1f);
                    }
                }

                // Y Boven
                if (player.Velocity.Y <= 0)
                {
                    player.IsGrounded = false;
                    if (IsMapTileSolid(GetPos(newPlayerPos.X), GetPos(newPlayerPos.Y)) ||
                        IsMapTileSolid(GetPos(newPlayerPos.X + TileHeightToPos(0.9f)), GetPos(newPlayerPos.Y)))
                    {
                        newPlayerPos.Y = GetPos(newPlayerPos.Y + 1f);
                        player.Velocity *= new Vector2(1f, 0f);
                    }
                }
                // beneden
                else
                {
                    if (IsMapTileSolid(GetPos(newPlayerPos.X), GetPos(newPlayerPos.Y + TileHeightToPos(1.0f))) ||
                        IsMapTileSolid(GetPos(newPlayerPos.X + TileHeightToPos(0.9f)), GetPos(newPlayerPos.Y + TileHeightToPos(1.0f))))
                    {
                        newPlayerPos.Y = player.Position.Y;
                        player.Velocity *= new Vector2(1f, 0f);

                        player.IsGrounded = true;
                        lastPlayerGroundedPos = newPlayerPos;
                    }
                }
            }

            // de stap
            player.Position = newPlayerPos;

            // next level
            if (player.Position.X >= (levelWidth - 2.5f) * Settings.TileWidth)
            {
                stateManager.SwitchState(nextLevel);
            }

            
            player.Update(gameTime);

            
            Settings.CameraPosition = player.Position;


            
            if (characters.Count <= 1) return;
            for (int i = 1; i < characters.Count; i++)
            {
                Entity character = characters[i];

                
                if (character.Active)
                {
                   
                    character.PhysicsUpdate();

                   
                    Vector2 newCharacterPos = character.Position + (character.Velocity * delta);

                    if (Math.Abs(player.Position.X - newCharacterPos.X) <= 10f &&
                        Math.Abs(newCharacterPos.Y - player.Position.Y) <= 5f)
                    {
                        player.TakeDamage(1);
                    }

                    if (character.ShouldColliderWithWorld())
                    {
                        
                        if (character.Velocity.X <= 0)
                        {
                            if (IsMapTileSolid(GetPos(newCharacterPos.X), GetPos(character.Position.Y)) ||
                                IsMapTileSolid(GetPos(newCharacterPos.X), GetPos(character.Position.Y + TileWidthToPos(0.9f))))
                            {
                                newCharacterPos.X = GetPos(character.Position.X);
                                character.Velocity *= new Vector2(0f, 1f);

                                character.HitWall = true;
                            }
                        }
                        
                        else
                        {
                            if (IsMapTileSolid(GetPos(newCharacterPos.X + TileWidthToPos(1)), GetPos(character.Position.Y)) ||
                                IsMapTileSolid(GetPos(newCharacterPos.X + TileWidthToPos(1)), GetPos(character.Position.Y + TileWidthToPos(0.9f))))
                            {
                                newCharacterPos.X = character.Position.X;
                                character.Velocity *= new Vector2(0f, 1f);

                                character.HitWall = true;
                            }
                        }

                        
                        if (character.Velocity.Y <= 0)
                        {
                            if (IsMapTileSolid(GetPos(newCharacterPos.X), GetPos(newCharacterPos.Y)) ||
                                IsMapTileSolid(GetPos(newCharacterPos.X + TileHeightToPos(0.9f)), GetPos(newCharacterPos.Y)))
                            {
                                newCharacterPos.Y = GetPos(newCharacterPos.Y + 1f);
                                character.Velocity *= new Vector2(1f, 0f);
                            }
                        }
                        
                        else
                        {
                            if (IsMapTileSolid(GetPos(newCharacterPos.X), GetPos(newCharacterPos.Y + TileHeightToPos(1.0f))) ||
                                IsMapTileSolid(GetPos(newCharacterPos.X + TileHeightToPos(0.9f)), GetPos(newCharacterPos.Y + TileHeightToPos(1.0f))))
                            {
                                newCharacterPos.Y = character.Position.Y;
                                character.Velocity *= new Vector2(1f, 0f);
                            }
                        }
                    }

                    
                    character.Position = newCharacterPos;

                    
                    character.Update(gameTime);

                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            /*
             * Draw volgorde
             * 1. Background
             * 2. tiles
             * 3. Characters laatst
             */
            base.Draw(gameTime);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, transformMatrix: Settings.GameDrawMatrix);

            
            Vector2 bgPosition = new Vector2(Settings.CameraPosition.X - (game.GraphicsDevice.Viewport.Width / 1.5f) / Settings.PixelScale,
                                                 Settings.CameraPosition.Y - (game.GraphicsDevice.Viewport.Height / 1.5f) / Settings.PixelScale);

            spriteBatch.Draw(bg0, bgPosition, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bg1, (bgPosition + new Vector2(10, -50f)) * 0.9f, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.Draw(bg2, (bgPosition + new Vector2(10, -50f)) * 0.85f, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    Vector2 position = new Vector2(x * Settings.TileWidth, y * Settings.TileHeight);

                    switch(GetMapTile(x, y))
                    {
                        case 'S':
                            spriteBatch.Draw(tilesTexture, position, new Rectangle(0, 0, Settings.TileWidth, Settings.TileHeight), Color.White);
                            break;
                        case 'G':
                            spriteBatch.Draw(tilesTexture, position, new Rectangle(16, 0, Settings.TileWidth, Settings.TileHeight), Color.White);
                            break;
                        case '1':
                            spriteBatch.Draw(tilesTexture, position, new Rectangle(32, 0, Settings.TileWidth, Settings.TileHeight), Color.White);
                            break;
                        case '2':
                            spriteBatch.Draw(tilesTexture, position, new Rectangle(48, 0, Settings.TileWidth, Settings.TileHeight), Color.White);
                            break;
                        case 'M':
                            spriteBatch.Draw(Settings.GetTexture("coin"), position,
                                coinAnimation.GetSourceRectangle(), Color.White, 0f, Vector2.Zero,
                                0.6f, SpriteEffects.None, 0f);
                            break;
                        default:
                            break;
                    }
                }
            }

            
            foreach (Entity character in characters)
            {
                if (character.Active)
                {
                    character.Draw();
                }
            }

            spriteBatch.End();

           
            spriteBatch.Begin();
            for (int i = 1; i < player.CurrentHealth + 1; i++)
            {
                spriteBatch.Draw(Settings.GetTexture("heart"), new Vector2(i * 32, 32),
                    heartAnimation.GetSourceRectangle(), Color.White, 0f, Vector2.Zero,
                    1.5f, SpriteEffects.None, 0f);
            }
            spriteBatch.DrawString(Settings.Font, "Score: " + Settings.Score,
                ButtonMiddle.GetTextPosition(Settings.Font, "Score: " + Settings.Score,
                new Vector2(game.GraphicsDevice.Viewport.Width / 2,
                40), 0.5f), Color.White, 0f, Vector2.Zero, 0.5f,
                SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        protected Vector2 GetStartPlayerPosition()
        {
            Vector2 position = new Vector2(0, 0);
            for (int i = 0; i < levelMap.Length; i++)
            {
                for (int j = 0; j < levelMap[i].Length; j++)
                {
                    if (levelMap[i][j] == 'P')
                    {
                        position = new Vector2(j * Settings.TileWidth, i * Settings.TileHeight);
                        break;
                    }
                }
            }
            return position;
        }
        protected void SpawnEnemies()
        {
            for (int i = 0; i < levelMap.Length; i++)
            {
                for (int j = 0; j < levelMap[i].Length; j++)
                {
                    if (levelMap[i][j] == 'X')
                    {
                        Vector2 position = new Vector2(j * Settings.TileWidth, i * Settings.TileHeight);
                        GroundEnemy enemy1 = new GroundEnemy(position, Settings.GetTexture("enemy"));
                        characters.Add(enemy1);
                    }
                    else if (levelMap[i][j] == 'C')
                    {
                        Vector2 position = new Vector2(j * Settings.TileWidth, i * Settings.TileHeight);
                        FlyingEnemy enemy2 = new FlyingEnemy(position, Settings.GetTexture("enemy2"));
                        characters.Add(enemy2);
                    }
                }
            }
        }

        private int GetPos(float f)
        {
            return (int)Math.Floor(f);
        }
        private float TileWidthToPos(float size)
        {
            return Settings.TileWidth * size;
        }
        private float TileHeightToPos(float size)
        {
            return Settings.TileHeight * size;
        }
    }
}
