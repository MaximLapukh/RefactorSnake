using refactorSimpleSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.FactoryFoods
{
    public class EatToCreate : IFactoryFood
    {
        public List<Food> CreateFood(IGame game,GameSettings settings)
        {
            return game.GetCountFood() == 0 ? new List<Food>() { new Food(Rnd.GetRndPosInGameField(settings)) } : null;
        }
    }
}
