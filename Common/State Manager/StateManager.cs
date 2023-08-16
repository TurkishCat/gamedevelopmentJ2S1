using Microsoft.Xna.Framework;

namespace Monogame_Game.Engine.Game_State_Management
{
    public class StateManager : DrawableGameComponent
    {
        
        private SceneState state;

        public StateManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Draw(GameTime gt)
        {
            base.Draw(gt);
            if (!(state == null)) state.Draw(gt);

        }
        public override void Update(GameTime gt)
        {
            base.Update(gt);

            
            if (!Enabled) return;

           
            if (Enabled && state != null) state.Update(gt);
        }
        

        
        public void SwitchState(SceneState state)
        {
            
            this.state = state;
            
            this.state.Start();
        }
    }
}
