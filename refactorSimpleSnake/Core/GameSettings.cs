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
        
        public GameSettings(int width, int height)
        {
            _width = width;
            _height = height;
        }
    }
}
