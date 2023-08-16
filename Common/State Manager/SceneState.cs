using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame_Game.Engine.Game_State_Management
{
    public abstract class SceneState
    {

        protected List<GameComponent> components;
        public List<GameComponent> Components { get { return components; } }

        protected  StateManager stateManager;

        protected  Game1 game;


        


        public SceneState(Game1 game, StateManager stateManager)
        {
            this.game = game;
            this.stateManager = stateManager;


            components = new List<GameComponent>();
        }

        public void Add(GameComponent component)
        {
            components.Add(component);
        }
        public void Remove(GameComponent component)
        {
            components?.Remove(component);
        }


        virtual public void Start()
        {
            
            components = new List<GameComponent>();
        }
        
        virtual public void Leave()
        {
            
        }

        
        public virtual void Update(GameTime gameTime)
        {
            
            foreach (var comp in components)
            {
                if (comp.Enabled) comp.Update(gameTime);
 
            }
        }
       
        public virtual void Draw(GameTime gameTime)
        {
            
            foreach (var comp in components)
            {
                if (comp is DrawableGameComponent component && component.Visible) component.Draw(gameTime);

            }
        }

        
        
    }
}
