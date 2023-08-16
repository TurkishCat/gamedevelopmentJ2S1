using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Game.Engine.Input_Manager
{
    public enum MouseButton { Right, Left, Middle };

    public class InputReader
    {
        //bij startup instance eenmalig null zetten
        private static InputReader instance = null;

        
        //Singleton -> constructor moet private zijn!
        private InputReader()
        {
            keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        
        //Singleton -> static instance, want 'Single' instantie
        public static InputReader Instance
        {
            get
            {
                //instance initialiseren hier pas want Singleton -> lazy initialization
                if (instance == null) instance = new InputReader();
 
                return instance;
            }
        }


        
        private KeyboardState keyboard;
       
        private KeyboardState prevKeyboardState;

        
        private MouseState mouse;
       
        private MouseState prevMouseState;

        
        public KeyboardState Keyboard { get { return keyboard; } }
        public KeyboardState PrevKeyboardState { get { return prevKeyboardState; } }

        
        public MouseState Mouse { get { return mouse; } }
        public MouseState PrevMouseState { get { return prevMouseState; } }


        
        public void UpdateInput()
        {
            
            prevKeyboardState = keyboard;
            prevMouseState = mouse;

            
            keyboard = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        //returnen allemaal enkel true als key BLIJFT ingehouden worden
        public bool IsKeyDown(Keys key)
        {
            return keyboard.IsKeyDown(key);
        }
        
        public bool WasKeyDown(Keys key)
        {
            return prevKeyboardState.IsKeyDown(key);
        }
        
        public bool KeyPressed(Keys key)
        {
            return keyboard.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }
        
        public bool KeyReleased(Keys key)
        {
            return keyboard.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key);
        }

        // Muis ingehouden in HUIDIGE frame -> true. Else false
        public bool IsMouseDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouse.LeftButton == ButtonState.Pressed,
                MouseButton.Right => mouse.RightButton == ButtonState.Pressed,
                MouseButton.Middle => mouse.MiddleButton == ButtonState.Pressed,
                _ => false,
            };
        }

        // Muis ingehouden in VORIGE frame -> true. Else false
        public bool WasMouseDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => prevMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => prevMouseState.RightButton == ButtonState.Pressed,
                MouseButton.Middle => prevMouseState.MiddleButton == ButtonState.Pressed,
                _ => false,
            };
        }

        // Muis gedrukt in huidige frame -> true. Else false
        public bool MousePressed(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouse.LeftButton == ButtonState.Pressed &&
               prevMouseState.LeftButton == ButtonState.Released,
                MouseButton.Right => mouse.RightButton == ButtonState.Pressed
               && prevMouseState.RightButton == ButtonState.Released,
                MouseButton.Middle => mouse.MiddleButton == ButtonState.Pressed
               && prevMouseState.MiddleButton == ButtonState.Released,
                _ => false,
            };
        }

        // Muis gereleased in huidige frame -> true. Else false
        public bool MouseReleased(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => mouse.LeftButton == ButtonState.Released &&
               prevMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => mouse.RightButton == ButtonState.Released
               && prevMouseState.RightButton == ButtonState.Pressed,
                MouseButton.Middle => mouse.MiddleButton == ButtonState.Released
               && prevMouseState.MiddleButton == ButtonState.Pressed,
                _ => false,
            };
        }
    }
}
