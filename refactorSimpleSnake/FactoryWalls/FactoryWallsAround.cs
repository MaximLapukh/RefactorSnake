using System;
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
            var wall2 = CreateWall(Direction.up, Vector2.Zero, settings._height);
            var wall3 = CreateWall(Direction.right, new Vector2(0, settings._height), settings._width);
            var wall4 = CreateWall(Direction.up, new Vector2(settings._width - 1, 0), settings._height);
            var res = wall.Concat(wall2).Concat(wall3).Concat(wall4).ToDictionary(x => x.Key, x => x.Value);
            return res;
        }
    }
}
