using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.FactoryFoods
{
    public class NoneFactoryFood : IFactoryFood
    {
        public List<Food> CreateFood(IGame game, GameSettings settings)
        {
            return null;
        }
    }
}
