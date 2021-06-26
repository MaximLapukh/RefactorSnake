using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public class Snake : GameObject
    {
        public bool die { get; private set; } = false;
        public event EventHandler Death;
        private Vector2 _direction = new();
        private Direction _dir;
        public Direction direction
        {
            get { return _dir; }
            set {
                if ((_dir.GetVector2() + value.GetVector2()) != Vector2.Zero) {
                    _dir = value;
                    _direction = value.GetVector2();
                }
            }
        }
        public int score { get; private set; } = 0;
        private readonly IGame _game;
        private Segment _segment { get; set; }
        private int stomach { get; set; } = 2;
        public Snake(IGame game,int slong = 2)
        {
            if(slong > 0)
                stomach = slong;            
            direction = Direction.right;
            _game = game;
        }
        public override List<GameObject> ToList()
        {
            var list = new List<GameObject>() { this };
            if (_segment != null) list.AddRange(_segment.ToList());
            return list;
        }
        public override void MoveTo(Vector2 to)
        {
            if (die) return;

            var nextpos = CorrectionPos(position + _direction);
            //nextpos.x % _game.GetSettings()._width;

            if (_game.TryGetGamObj(nextpos, out GameObject gObj))
            {
                if(gObj != null)
                {
                    if (gObj is Wall || gObj is Snake) { 
                        Die();
                    }
                    var food = gObj as Food;
                    if (food != null)
                    {
                        score++;
                        stomach++;
                        _game.EatFood(this,food);
                    }
                }               
            }
            var lastpos = position;
            position = nextpos;
            var segment = new Segment(lastpos);
            segment.nextSegment = _segment;
            _segment = segment;
            if (stomach <= 0)
            {
                _segment.RemoveLast();
            }
            else stomach--;
        }
        private Vector2 CorrectionPos(Vector2 pos) {
            if (pos.x > _game.GetSettings()._width)
                pos.x = 0;
            else if (pos.x < 0)
                pos.x = _game.GetSettings()._width;

            if (pos.y > _game.GetSettings()._height)
                pos.y = 0;
            else if (pos.y < 0)
                pos.y = _game.GetSettings()._height;
            return pos;
        }
        public override bool IsHit(Vector2 pos)
        {
            var res = false;
            if (_segment != null)
            {
                var temp = _segment.IsHit(pos);
                if (temp) res = true;
            }
            if (position == pos) res = true;
            return res;
        }
        public void Die()
        {
            die = true;
            Death?.Invoke(this, null);
            _game.isStop = true;
        }
    }
}
