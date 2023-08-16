using Monogame_Game.Engine.Game_State_Management;
using Microsoft.Xna.Framework;
using Monogame_Game.Engine.Input_Manager;
using System.Runtime.InteropServices.ObjectiveC;
using System.Globalization;

namespace Monogame_Game.Game_Code.Game_States.Levels
{
    public class Level1 : PlayState
    {

        public Level1(Game1 game, StateManager stateManager, SceneState nextLevel) : base(game, stateManager, nextLevel)
        {
            levelMap = new string[]
            {
                "2GGGG....GGGGGGGGGGG...GGGGGGGGGGGGG.......GGGGGGGGGG1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2....................................................1",
                "2......................M.............................1",
                "2.....................GGG.......M........M...........1",
                "2..............1GG.....................GGGG..........1",
                "2..............1...M....X...GG....2..X.1.........M...1",
                "2P...M...X.1GGGGGGGGGGGGGGGGGGSSSSGGGGGG.......GGGGGG1",
                "2GGGGGGGGGG1GGGGGGGGGGGGGGGGGGGGGGGGGGGG.......GGGGGG1",
            };
        }

        public override void Start()
        {
            
            Settings.Player = new Joe(GetStartPlayerPosition(), Settings.GetTexture("player"));
            
            Settings.Score = 0;

            base.Start();

            
            characters.Add(Settings.Player);

           
            SpawnEnemies();
        }
    }
}
