using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithm.Walker
{

    /// <summary>
    /// Implementation of Walker's algorithm for tree representation. 
    /// </summary>
	public class WalkerAlgorithm: AlgorithmBase
	{
		private IWalkerNode m_rootNode;
		private readonly float m_offset = 0.5f;
		private readonly ContourCalculator m_contourCalculator = new ContourCalculator ();

		protected override void InitializeImpl (INodeStructure root)
		{
			m_rootNode = new WalkerNode (null, root);
			InitializeRecursively (m_rootNode, root.Children);
		}

		protected override void ExecuteImpl ()
		{
			CalculateX (m_rootNode);
			CheckAllChildrenOnScreen (m_rootNode);
		}

		protected override void ApplyCalculatedCoordinatesImpl ()
		{
			CalculateFinalPositions (m_rootNode, 0);
		}

		private void InitializeRecursively (IWalkerNode parent, List<INodeStructure> innerChildren)
		{
			innerChildren.ForEach (child => {
				var buchheimChild = parent.AddChild (child);
				if (!child.IsLeaf) {
					InitializeRecursively (buchheimChild, child.Children);
				}
			});
		}

        /// <summary>
        /// Calculate initial positions of nodes, starting from bottom to the top.
        /// </summary>
        /// <param name="node"></param>
		private void CalculateX (IWalkerNode node)
		{
			if (!node.Children.Any ()) {
				if (node.Order > 0) {
					node.X = node.GetLeftSibling ().X + m_offset;
				}
			} else {
				node.Children.ForEach (CalculateX);
				var leftChild = node.Children.FirstOrDefault ();
				var rightChild = node.Children.LastOrDefault ();
				var mid = (leftChild?.X + rightChild?.X) / 2 ?? 0f;

				if (node.Order == 0) {
					node.X = mid;
				} else {
					node.X = node.GetLeftSibling ().X + m_offset;
					node.Mod = node.X - mid;
					ResolveConflictsWhenFound (node);
				}
			}
		}

        /// <summary>
        /// Check that all nodes have X-coordinate exceeding 0. 
        /// Otherway shift everything to the right.
        /// </summary>
        /// <param name="node">Root node.</param>
		private void CheckAllChildrenOnScreen (IWalkerNode node)
		{
			var nodeContour = new Dictionary<int, float> ();
			m_contourCalculator.GetLeft (node, 0, nodeContour);
			var min = nodeContour.Values.Min ();
			if (min < 0) {
				node.X -= min;
				node.Mod -= min;
			}
		}

        /// <summary>
        /// Fix collisions for current node.
        /// </summary>
        /// <param name="node">Current node.</param>
        private void ResolveConflictsWhenFound (IWalkerNode node)
		{
			var rightContours = new List <Dictionary <int, float>> (node.Order);
            // collect all right contours before current node.
            node.Parent.Children
				.TakeWhile (sibling => sibling != node)
				.ToList ()
				.ForEach (x => {
				var contour = new Dictionary<int, float> ();
				m_contourCalculator.GetRight (x, 0, contour);
				rightContours.Add (contour);
			});	
					
			var leftContour = new Dictionary<int, float> ();
			m_contourCalculator.GetLeft (node, 0, leftContour);
            // compare each of them with left contour of current node and shift node when collision found.
			rightContours.ForEach (rightContour => {
				var commonLevel = Math.Min (rightContour.Keys.Max (), leftContour.Keys.Max ());
				var distance = leftContour [commonLevel] - rightContour [commonLevel];
                // Conflict resolving
                if (distance < m_offset) {
					node.X += m_offset - distance;
					node.Mod += m_offset - distance;
				}
			});
		}

        /// <summary>
        /// Apply result of calculations to INodeStructure.Position.
        /// </summary>
        /// <param name="node">Current node.</param>
        /// <param name="modSum">Summarized Mod so far.</param>
        private void CalculateFinalPositions (IWalkerNode node, float modSum)
		{
			node.X += modSum;
			modSum += node.Mod;
			node.InnerNode.Point.X = node.X;

			foreach (var child in node.Children) {
				CalculateFinalPositions (child, modSum);
			}
		}
	}
}