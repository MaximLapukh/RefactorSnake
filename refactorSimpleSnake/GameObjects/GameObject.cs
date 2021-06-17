using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactorSimpleSnake
{
    public abstract class GameObject
    {
        //enum food, wall or snake
        public Vector2 position { get; set; }
        public GameObject() { position = new Vector2(); }
        public GameObject(Vector2 pos) { position = pos; }
        public virtual void MoveTo(Vector2 to) { position = to; }
        public virtual List<GameObject> ToList() { return new List<GameObject>() { this }; }
        public virtual bool IsHit(Vector2 pos) { return position == pos; }
    }
}
