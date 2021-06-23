using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.FactoryWalls
{
    public class FactoryWallsAround : BaseFactoryWalls
    {
        public override Dictionary<int, GameObject> InitWalls(GameSettings settings)
        {
            var wall = CreateWall(Direction.right, Vector2.Zero, settings._width);
            var wall2 = CreateWall(Direction.up, new Vector2(0, 1), settings._height - 2);            
            var wall3 = CreateWall(Direction.right, new Vector2(0, settings._height-1), settings._width);
            var wall4 = CreateWall(Direction.up, new Vector2(settings._width - 1, 1), settings._height-2);
            var res = wall.Union(wall2).Union(wall3).Union(wall4).ToDictionary(x => x.Key, x => x.Value);
            return res;
            
        }
    }

}
