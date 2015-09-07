using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Buchheim
{
	public class BuchheimNode:IBuchheimNode
	{
		public int Order { get; }

		public INodeStructure InnerNode { get; }

		public List<IBuchheimNode> Children { get; }

		public IBuchheimNode Parent { get; }

		public IBuchheimNode Ancestor { get; set; }

		public IBuchheimNode Thread { get; set; }

		public float Preliminary { get; set; }

		public float Mod { get; set; }

		public float Change { get; set; }

		public float Shift { get; set; }

		public BuchheimNode (IBuchheimNode parent, INodeStructure root, int? order = null)
		{
			InnerNode = root;
			Order = order ?? InnerNode.Order;
			Children = new List<IBuchheimNode> ();
			Parent = parent;
			Mod = 0;
			Preliminary = 0;
			Shift = 0;
			Change = 0;
			Thread = null;
			Ancestor = this;
			Parent?.Children.Add (this);
		}

		public virtual IBuchheimNode AddChild (INodeStructure childInnerNode, int? order = null)
		{
			var child = new BuchheimNode (this, childInnerNode, order);
			return child;
		}

		public IBuchheimNode GetLeftSibling ()
		{
			return (Parent == null || Order == 0) ? null : Parent.Children [Order - 1];
		}

		public IBuchheimNode GetLeftMostChild ()
		{
			return Children.FirstOrDefault ();
		}

		public IBuchheimNode GetRightMostChild ()
		{
			return Children.LastOrDefault ();
		}

		public IBuchheimNode GetNextLeft ()
		{
			return Children.Count == 0 ? Thread : GetLeftMostChild ();
		}

		public IBuchheimNode GetNextRight ()
		{
			return Children.Count == 0 ? Thread : GetRightMostChild ();
		}
        public IBuchheimNode GetAncestor (IBuchheimNode vertex)
        {
            return (Ancestor.Parent == vertex.Parent) ? Ancestor : null;
        }
    }
}