using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.FactoryFoods
{
    public class EatToCreate : IFactoryFood
    {
        public List<Food> CreateFood(IGame game)
        {
            if (game.GetCountFood() <= 0)
            {
                var food = new Food(Rnd.GetRndPosInGameField(game.GetSettings()));
                for (int i = 0; i < 10; i++)
                {
                    if (!game.TryGetGamObj(food.position, out GameObject gObj)) return new List<Food>() { food };
                    else food = new Food(Rnd.GetRndPosInGameField(game.GetSettings()));
                }
            }
            return new();
        }
    }
}
