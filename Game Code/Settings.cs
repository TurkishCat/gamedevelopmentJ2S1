using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Monogame_Game.Game_Code
{
    public static class Settings
    {
        private static Dictionary<string, Texture2D> textures;

        public static Dictionary<string, Texture2D> Textures
        {
            get
            {
                if (textures == null) textures = new Dictionary<string, Texture2D>();

                return textures;
            }
        }

        private static Dictionary<string, SoundEffect> sfx;
        public static Dictionary<string, SoundEffect> Sfx
        {
            get
            {
                if (sfx == null) sfx = new Dictionary<string, SoundEffect>();
                return sfx;
            }
        }

        
        public const float PixelScale = 3.0f;
        
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        
        public const int TileWidth = 16;
        public const int TileHeight = 16;

        
        public static Vector2 CameraPosition { get; set; }
        public static Matrix GameDrawMatrix { get; set; }

        public static Joe Player { get; set; }
        public static int Score { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteFont Font { get; set; }

        public static void LoadTexture(string name, string path)
        {
            Textures.Add(name, Content.Load<Texture2D>(path));
        }
        public static void LoadSfx(string name, string path)
        {
            Sfx.Add(name, Content.Load<SoundEffect>(path));
        }

        public static Texture2D GetTexture(string name)
        {
            return Textures[name];
        }
        public static void PlaySoundEffect(string name)
        {
            Sfx[name].Play();
        }
    }
}
