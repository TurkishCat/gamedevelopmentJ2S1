using Monogame_Game.Engine.Game_State_Management;
using Microsoft.Xna.Framework;
using Monogame_Game.Engine.Input_Manager;
using System.Runtime.InteropServices.ObjectiveC;
using System.Globalization;

namespace Monogame_Game.Game_Code.Game_States.Levels
{
    public class Level2 : PlayState
    {

        public Level2(Game1 game, StateManager stateManager, SceneState nextLevel) : base(game, stateManager, nextLevel)
        {
            levelMap = new string[]
            {
                "2GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG1",
                "2.....................................................................1G2",
                "2.....................................................................1G2",
                "2.....................................................................1G2",
                "2.....M...............................................................1G2",
                "1P........................M...........................................1G2",
                "1GG2......M.1G2....12.X..12...........................................1G2",
                "1GG2....1GGGGGGSSSSGGGGGGG2...........................M...............1G2",
                "1GG2....1GGGGGGGGGGGGGGGGG2.....M..........M..GGG2....................1G2",
                "1GG2SSSS2................G2..................GGGG2....................1G2",
                "1GG2GGGG2................GGGGGGGGGGGG1....2GGGGGG2....X.....1GGGGG...GGG2",
                "1GG2.................................1SSSS2......2GGGGGGGGGG1........GGG2",
                "1GG2.................................1GGGG2..........................GGG2",
                "1GG2..................................................M..............GGG2",
                "1GG2................................................GGGGG....C.......GGG2",
                "1GG2.....MMMMMM.....X..G.............G....M....X.G...................GGG2",
                "1GG2GGGGGGGGGGGGGGGGGGGGGGGGGGG2..1GGGGGGGGGGGGGGGGG1....2GGGGGGGGGGGGGG2",
                "1GG2...........................2..1.................1SSSS2..............2",
                "1GG2...........................2..1.......C.........1GGGG2..............2",
                "1GG2...........................2..........M........................M....2",
                "1GG2...........................2....................X...........X.......2",
                "1GG2...........................2GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG2",
                "1GG2....................................................................2",
                "1GG2....................................................................2",
                "1GG2....................................................................2",
                "1GG2....................................................................2",
            };
        }

        public override void Start()
        {
            
            Settings.Player.Position = GetStartPlayerPosition();

            base.Start();

            
            characters.Add(Settings.Player);

            
            SpawnEnemies();
        }
    }
}
