using System.Collections.Generic;

namespace Algorithm.Walker
{
    public class WalkerNode : IWalkerNode
    {
        public INodeStructure InnerNode { get; }
        public float X { get; set; }
        public int Y { get; }
        public float Mod { get; set; }
        public WalkerNode Parent { get; }
        public List<WalkerNode> Children { get; }
        public int Order => InnerNode.Order;

        /// <summary>
        /// Wrapping INodeStructure element for usage inside the algorithm.
        /// </summary>
        /// <param name="parent">Parent in wrapping structure.</param>
        /// <param name="node">Child to be wrapped.</param>
        public WalkerNode (WalkerNode parent, INodeStructure node)
        {
            InnerNode = node;
            Parent = parent;
            Children = new List<WalkerNode> ();
            X = 0;
            Y = (int)node.Point.Y;
            Mod = 0;
            // to keep structure of tree consistent
            parent?.Children.Add (this);
        }

        public WalkerNode AddChild (INodeStructure childInnerNode)
        {
            return new WalkerNode (this, childInnerNode);
        }

        public WalkerNode GetLeftSibling ()
        {
            return InnerNode.Order == 0 ? null : Parent.Children[InnerNode.Order - 1];
        }
    }
}
