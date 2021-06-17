using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public enum Direction
    {
        left,up,right,down
    }
    public static class Dir
    {
        public static Vector2 GetVector2(this Direction dir)
        {
            Vector2 res = new Vector2();
            switch (dir)
            {
                case Direction.right:                    
                    res.x = 1;
                    res.y = 0;
                    break;

                case Direction.left:
                    res.x = -1;
                    res.y = 0;
                    break;

                case Direction.up:
                    res.x = 0;
                    res.y = 1;
                    break;

                case Direction.down:
                    res.x = 0;
                    res.y = -1;
                    break;
            }
            return res;
        }
    }
}
