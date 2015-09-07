using System.Collections.Generic;

namespace Algorithm.Buchheim
{
	public interface IBuchheimNode
	{
		INodeStructure InnerNode { get; }

        /// <summary>
        /// Order of node underneath its parent (starts with 0).
        /// </summary>
		int Order { get; }

        /// <summary>
        /// List of children nodes.
        /// </summary>
		List<IBuchheimNode> Children { get; }

        /// <summary>
        /// Parent. If it's null then InnerNode is root.
        /// </summary>
		IBuchheimNode Parent { get; }

        /// <summary>
        /// Properties used in algorithm.
        /// </summary>
		IBuchheimNode Ancestor { get; set; }

		IBuchheimNode Thread { get; set; }

		float Preliminary { get; set; }

		float Mod { get; set; }

		float Change { get; set; }

		float Shift { get; set; }

		IBuchheimNode AddChild (INodeStructure childInnerNode, int? order = null);

	    IBuchheimNode GetAncestor (IBuchheimNode vertex);

        IBuchheimNode GetNextLeft ();

		IBuchheimNode GetNextRight ();

		IBuchheimNode GetLeftSibling ();

		IBuchheimNode GetLeftMostChild ();

		IBuchheimNode GetRightMostChild ();
	}
		
}