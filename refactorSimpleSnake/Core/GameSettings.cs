using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public struct GameSettings
    {
        public readonly int _width;
        public readonly int _height;
        public readonly int _speed;
        public GameSettings(int width, int height,int speed)
        {
            _width = width;
            _height = height;
            _speed = speed;
        }
    }
}
