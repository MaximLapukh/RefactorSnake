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
        private bool _isStop = false;

        public bool isStop
        {
            get { return _isStop; }
            set { _isStop = value; if (_isStop) Stop(this,null); }
        }

        private readonly Dictionary<int, GameObject> static_gObjs = new();
        private List<GameObject> dyn_gObjs = new();
        private List<GameObject> rem_gObjs = new();
        private readonly GameSettings _settings;

        private IFactoryFood factoryFood = new NoneFactoryFood();

        public IFactoryFood FactoryFood
        {
            get { return factoryFood; }
            set { if(value != null) factoryFood = value; }
        }
        private int countFoods = 0;

        public event EventHandler<GameEventArgs> Update;
        public event EventHandler Stop;

        public Game(GameSettings settings,BaseFactoryWalls factoryWalls = null,IFactoryFood factoryFood = null)
        {
            _settings = settings;
            FactoryFood = factoryFood;           
            static_gObjs = factoryWalls.InitWalls(_settings) ?? new();
        }
        //GetEmptySpace
        public Snake AddSnake(Vector2 start, int slong, Direction dir)
        {
            var snake = new Snake(this,slong);
            snake.direction = dir;
            snake.position = start;
            dyn_gObjs.Add(snake);
            return snake;
        }

        public int GetCountFood()
        {
            return countFoods;
        }

        public List<GameObject> GetDynamicObjs()
        {
            return dyn_gObjs;
        }

        public bool TryGetGamObj(Vector2 vec, out GameObject gameObject)
        {
            if (static_gObjs.TryGetValue(vec.GetHashCode(), out GameObject gObj))
            {
                gameObject = gObj;
                return true;
            }
            if (dyn_gObjs.Count > 0)
            {
                foreach (var obj in dyn_gObjs)
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
            return static_gObjs.Values.ToList();
        }

        public void Start()
        {            
            reset();
            Task.Factory.StartNew(() => {
                while (true)
                {
                    if (!isStop)
                    {
                        foreach (var gObj in dyn_gObjs)
                        {
                            gObj.MoveTo(gObj.position);
                        }
                        destroyRemObjs();

                        var nfoods = FactoryFood.CreateFood(this);
                        if (nfoods != null)
                        {
                            dyn_gObjs.AddRange(nfoods);
                            countFoods += nfoods.Count;
                        }

                        Update?.Invoke(this, new GameEventArgs(dyn_gObjs));
                        Task.Delay(_settings._speed).Wait();
                    }
                }               
            });
        }
        private void reset() {
            dyn_gObjs = new ();
            countFoods = 0;
            isStop = false;
        }
        private void destroyRemObjs()
        {
            if(rem_gObjs.Count > 0)
            {
                foreach (var obj in rem_gObjs)
                {
                    dyn_gObjs.Remove(obj);
                }
            }
        }
        public void EatFood(object sender,Food food)
        {
            if(sender is Snake && food != null && dyn_gObjs.Contains(food))
            {
                rem_gObjs.Add(food);
                countFoods--;
            }
        }

        public GameSettings GetSettings()
        {
            return _settings;
        }
    }
    public class GameEventArgs : EventArgs
    {
        public List<GameObject> dynObjs;
        public GameEventArgs(List<GameObject> dynObjs)
        {
            this.dynObjs = dynObjs;
        }
    }
}
