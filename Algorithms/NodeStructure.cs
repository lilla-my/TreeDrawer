using System.Collections.Generic;

namespace Algorithm
{
    public class NodeStructure<T>:INodeStructure
    {
        public T Content { get; }
        public List<INodeStructure> Children { get; }
        public INodeStructure Parent { get; }
        public int Order { get; }
        public bool IsLeaf => Children.Count == 0;
        public Position2D Point { get; set; }

        public NodeStructure (INodeStructure parent, T content)
        {
            Parent = parent;
			Order = parent?.Children.Count ?? 0;
            Content = content;
            Children = new List<INodeStructure> ();
            Point = new Position2D () {X = 0, Y = parent?.Point.Y + 1 ?? 0};

            parent?.Children.Add (this);
        }
       
        public NodeStructure<T> AddChild (T content)
        {
            var child = new NodeStructure<T> (this, content);
            return child;
        }

		public override string ToString(){
			return Content.ToString();
		}
    }
}
