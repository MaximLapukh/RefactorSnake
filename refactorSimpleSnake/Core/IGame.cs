using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public interface IGame
    {
        public bool isStop { get; set; }
        public event EventHandler<GameEventArgs> Update;
        public event EventHandler Stop;
        public bool TryGetGamObj(Vector2 vec, out GameObject gameObject);
        public void Start();
        public Snake AddSnake(Vector2 start,int slong,Direction dir);
        public void EatFood(object sender,Food food);
        public int GetCountFood();
        public List<GameObject> GetStaticObjs();
        public List<GameObject> GetDynamicObjs();
        public GameSettings GetSettings();
    }
}
