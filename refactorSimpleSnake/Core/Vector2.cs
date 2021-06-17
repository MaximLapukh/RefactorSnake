using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public class Vector2
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
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if (a.x == b.x && a.y == b.y) return true;
            return false;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            if (a.x == b.x && a.y == b.y) return false;
            return true;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            Vector2 res = new Vector2(a.x + b.x, a.y + b.y);
            return res;
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector2 vec)
                if (x == vec.x && y == vec.y) return true;
            return false;
        }
        public override int GetHashCode()
        {
            string temp = x.ToString() + y.ToString();
            return int.Parse(temp);
        }
    }
}
