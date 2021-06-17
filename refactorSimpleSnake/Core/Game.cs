using refactorSimpleSnake.Core;
using refactorSimpleSnake.FactoryFoods;
using refactorSimpleSnake.FactoryWalls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public sealed class Game:IGame
    {
        private bool IsGameOver { get; set; } = false;
        private Dictionary<int, GameObject> staticGameObjs;
        private List<GameObject> dynamicGameObjs;      
        private readonly GameSettings _settings;
        private IFactoryFood _factoryFood;
        private int countFoods=0;
        public Game(GameSettings settings,BaseFactoryWalls factoryWalls = null,IFactoryFood factoryFood = null)
        {            
            _settings = settings;
            _factoryFood = factoryFood;
            if (factoryWalls != null)
                staticGameObjs = factoryWalls.InitWalls(_settings);

        }

        public Snake AddSnake(Vector2 start, int slong, Direction dir)
        {
            throw new NotImplementedException();
        }

        public void GameOver()
        {
            IsGameOver = true;
        }

        public int GetCountFood()
        {
            return countFoods;
        }

        public List<GameObject> GetDynamicObjs()
        {
            return dynamicGameObjs;
        }

        public bool TryGetGamObj(Vector2 vec, out GameObject gameObject)
        {

            if (staticGameObjs.TryGetValue(vec.GetHashCode(), out GameObject gObj))
            {
                gameObject = gObj;
                return true;
            }
            if (dynamicGameObjs.Count > 0)
            {
                foreach (var obj in dynamicGameObjs)
                {
                    if (obj.IsHit(vec))
                    {
                        gameObject = obj;
                        return true;
                    }
                }
            }

            gameObject = null;
            return false;
        }

        public List<GameObject> GetStaticObjs()
        {
            return staticGameObjs.Values.ToList();
        }

        public void Start()
        {            
            reset();
            Task.Factory.StartNew(() => {
                
                if (IsGameOver) return; 
                var nfoods = _factoryFood.CreateFood(this, _settings);
                if (nfoods != null && nfoods.Count > 0)
                {
                    dynamicGameObjs.AddRange(nfoods);
                    countFoods += nfoods.Count;
                }
                foreach (var obj in dynamicGameObjs)
                {
                    obj.MoveTo(obj.position);
                }
                Task.Delay(500).Wait();
            });
            

        }
        private void reset() {
            dynamicGameObjs = new List<GameObject>();
            countFoods = 0;
            IsGameOver = false;
        }
        public void EatFood(object sender,Food food)
        {
            if(sender is Snake)
            {
                dynamicGameObjs.Remove(food);
                countFoods--;
            }
        }
    }
}
