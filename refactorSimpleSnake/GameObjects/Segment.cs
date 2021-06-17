using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public class Segment:GameObject
    {
        public Segment nextSegment { get; set; }
        public Segment() { }
        public Segment(Vector2 pos):base(pos) { }
        public override void MoveTo(Vector2 to)
        {
            
        }
        public override bool IsHit(Vector2 pos)
        {
            var res = false;
            if (nextSegment != null)
            {
                var temp = nextSegment.IsHit(pos);
                if (temp) res = true;
            }
            if (position == pos) res = true;
            return res;
        }
        public void RemoveLast()
        {
            if (nextSegment.nextSegment == null) nextSegment = null;
        }
        public override List<GameObject> ToList()
        {
            var list = new List<GameObject>() { this };
            if (nextSegment != null) list.AddRange(nextSegment.ToList());
            return list;
        }
    }
}
