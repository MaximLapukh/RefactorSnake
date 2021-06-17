using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.FactoryWalls
{
    public abstract class BaseFactoryWalls
    {
        public abstract Dictionary<int, GameObject> InitWalls(GameSettings settings);
        public Dictionary<int,GameObject> CreateWall(Direction dir, Vector2 start, int wlong)
        {
            var walls = new Dictionary<int, GameObject>();
            var vec_dir = dir.GetVector2();
            for (int i = 0; i < wlong; i++)
            {
                GameObject block = new Wall(new Vector2((i * vec_dir.x) + start.x, (i * vec_dir.y) + start.y));
                walls.Add(block.GetHashCode(), block);
            }
            return walls;
        }
    }
}
