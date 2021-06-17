using refactorSimpleSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake.FactoryFoods
{
    public interface IFactoryFood
    {
        public List<Food> CreateFood(IGame game, GameSettings settings);
      
    }
    public static class Rnd
    {
        public static Random rnd = new Random();
        public static Vector2 GetRndPosInGameField(GameSettings settings) =>
          new Vector2(rnd.Next(0, settings._width), rnd.Next(0, settings._height));
    }
}
