using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public record Vector2
    {
        public int x { get; set; }
        public int y { get; set; }
        public static Vector2 Zero = new Vector2();

        public Vector2() { }
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public override int GetHashCode()
        {
            string temp = 1 + Math.Abs(x).ToString() + 98 + Math.Abs(y).ToString();
            return int.Parse(temp);
        }
    }
}
