using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public class Wall:GameObject
    {
        public Wall(Vector2 pos) : base(pos) { }
        public override void MoveTo(Vector2 to)
        {
            throw new NotSupportedException();
        }
    }
}
