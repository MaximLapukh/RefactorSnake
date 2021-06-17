﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public class Snake:GameObject
    {
        private bool die = false;
        private Vector2 _direction;
        public Direction direction
        {
            get { return direction; }
            set {
                switch (value)
                {
                    case Direction.right:

                        _direction.x = 1;
                        _direction.y = 0;
                        break;

                    case Direction.left:
                        _direction.x = -1;
                        _direction.y = 0;
                        break;

                    case Direction.up:
                        _direction.x = 0;
                        _direction.y = -1;
                        break;

                    case Direction.down:
                        _direction.x = 0;
                        _direction.y = 1;
                        break;
                }
            }
        }
        public int score { get; private set; }
        private Game _game { get; }
        private Segment _segment;
        private int stomach = 2;
        public Snake(Game game,int slong)
        {
            if(slong > 0)
                stomach = slong;
            _direction = new Vector2();
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

            var nextpos = position + _direction;
            if(_game.TryGetGamObj(nextpos, out GameObject gObj))
            {
                if(gObj != null)
                {
                    if (gObj is Wall||gObj is Snake)
                    {
                        _game.GameOver();
                        die = true;
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
        public int GetScore()
        {
            return 0;
        }
    }
}