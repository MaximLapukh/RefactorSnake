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
        private readonly Dictionary<int, GameObject> staticGameObjs = new Dictionary<int, GameObject>();
        private List<GameObject> dynamicGameObjs = new List<GameObject>();
        private List<GameObject> remGameObjs = new List<GameObject>();
        private readonly GameSettings _settings;
        private readonly IFactoryFood _factoryFood;
        private int countFoods=0;

        public event EventHandler Update;

        public Game(GameSettings settings,BaseFactoryWalls factoryWalls = null,IFactoryFood factoryFood = null)
        {            
            _settings = settings;
            if (factoryFood != null)
                _factoryFood = factoryFood;
            else _factoryFood = new NoneFactoryFood();
            if (factoryWalls != null)
                staticGameObjs = factoryWalls.InitWalls(_settings);
        }

        public Snake AddSnake(Vector2 start, int slong, Direction dir)
        {
            var snake = new Snake(this,slong);
            snake.direction = dir;
            snake.position = start;
            dynamicGameObjs.Add(snake);
            return snake;
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
            var hash = vec.GetHashCode();
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
                while (true)
                {
                    if (IsGameOver) return;
                    foreach (var gObj in dynamicGameObjs)
                    {
                        gObj.MoveTo(gObj.position);
                    }
                    destroyRemObjs();
                    var nfoods = _factoryFood.CreateFood(this, _settings);
                    if (nfoods != null && nfoods.Count > 0)
                    {
                        dynamicGameObjs.AddRange(nfoods);
                        countFoods += nfoods.Count;
                    }
                    
                    Update?.Invoke(this, null);
                    Task.Delay(_settings._speed).Wait();
                }               
            });
        }
        private void reset() {
            dynamicGameObjs = new List<GameObject>();
            countFoods = 0;
            IsGameOver = false;
        }
        private void destroyRemObjs()
        {
            if(remGameObjs.Count > 0)
            {
                foreach (var obj in remGameObjs)
                {
                    dynamicGameObjs.Remove(obj);
                }
            }
        }
        public void EatFood(object sender,Food food)
        {
            if(sender is Snake && food != null && dynamicGameObjs.Contains(food))
            {
                remGameObjs.Add(food);
                countFoods--;
            }
        }

        public GameSettings GetSettings()
        {
            return _settings;
        }
    }
}
