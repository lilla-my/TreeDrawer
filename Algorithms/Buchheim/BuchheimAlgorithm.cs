using System.Collections.Generic;

namespace Algorithm.Buchheim
{
	/// <summary>
	/// http://dirk.jivas.de/papers/buchheim02improving.pdf
	/// Improved version of Walker's algorithm to run in linear time.
	/// </summary>
	public class BuchheimAlgorithm : AlgorithmBase
	{
		private readonly float m_distance = 0.5f;
        private readonly TreeModifier m_treeModifier = new TreeModifier ();
        private IBuchheimNode m_rootNode;
		
		protected override void InitializeImpl (INodeStructure root)
		{
			if (root == null) {
				return;
			}

			m_rootNode = new BuchheimNode (null, root);
			InitializeRecursively (m_rootNode, root.Children);
		}

        /// <summary>
        /// Starts execution.
        /// </summary>
		protected override void ExecuteImpl ()
		{
			FirstWalk (m_rootNode);
		}

        /// <summary>
        /// Applies result of calculations to INodeStructure's coordinate. 
        /// </summary>
		protected override void ApplyCalculatedCoordinatesImpl ()
		{
			SecondWalk (m_rootNode, 0);
		}

        /// <summary>
        /// Initialize algorithm using passed INodeStructure, that will be wrapped into IBuchheimNode. 
        /// </summary>
        /// <param name="parent">Current IBuchheimNode parent.</param>
        /// <param name="innerChildren">Child to wrap and add.</param>
		private void InitializeRecursively (IBuchheimNode parent, List<INodeStructure> innerChildren)
		{
			innerChildren.ForEach (child => {
				var buchheimChild = parent.AddChild (child);
				if (!child.IsLeaf) {
					InitializeRecursively (buchheimChild, child.Children);
				}
			});
		}

        /// <summary>
        /// Here there be all the magic happening.
        /// Recursively adjust nodes from bottom to top. 
        /// </summary>
        /// <param name="vertex">Parent vertex.</param>
		private void FirstWalk (IBuchheimNode vertex)
		{
			if (vertex.Children.Count == 0) {
                // leafs of the tree.
				m_treeModifier.MoveAsideFromLeftSibling (vertex, m_distance);
			} else {
				var defaultAncestor = vertex.GetLeftMostChild ();
				vertex.Children.ForEach (child => {
					FirstWalk (child);
					defaultAncestor = Apportion (child, defaultAncestor);
				});

				m_treeModifier.ExecuteShifts (vertex);

				m_treeModifier.CenterVertexBasedOnChildren (vertex);

				m_treeModifier.MoveAsideFromLeftSibling (vertex);
			}
		}

        /// <summary>
        /// Sect. 3 and 5 (Apportion method). 
        /// </summary>
        /// <param name="vertex">Vertex being currently adjusting relatively its neighbors.</param>
        /// <param name="defaultAncestor">Variable to keep track of current default ancestor.</param>
        /// <returns></returns>
		private IBuchheimNode Apportion (IBuchheimNode vertex, IBuchheimNode defaultAncestor)
		{
			var leftSibling = vertex.GetLeftSibling ();
		    if (leftSibling == null)
		    {
		        return defaultAncestor;
		    }

		    var plusI = new EdgeWalker (vertex);
		    var minusI = new EdgeWalker (leftSibling);
		    var minus0 = new EdgeWalker (vertex.Parent.GetLeftMostChild ());
		    var plus0 = new EdgeWalker (vertex);

		    minusI.MoveRight ();
		    plusI.MoveLeft ();

		    while (!minusI.Finished && !plusI.Finished) {
		        minus0.MoveLeft ();
		        plus0.MoveRight ();

		        plus0.SetAncestor (vertex);

		        var shift = minusI.Offset - plusI.Offset + m_distance;
		        if (shift > 0) {
		            var ancestor = minusI.CurrentNode.GetAncestor (vertex) ?? defaultAncestor;
		            m_treeModifier.MoveSubtree (ancestor, vertex, shift);
		        }

		        minusI.UpdateSum ();
		        plusI.UpdateSum ();
		        plus0.UpdateSum ();
		        minus0.UpdateSum ();

		        minusI.MoveRight ();
		        plusI.MoveLeft ();
		    }

		    if ((!minusI.Finished) && plus0.IsTheLastOnRight()) {
		        plus0.SetThreadAndUpdateMod (minusI);
		    }

		    if (!plusI.Finished && minus0.IsTheLastOnLeft ()) {
		        minus0.SetThreadAndUpdateMod (plusI);
		        defaultAncestor = vertex;
		    }

		    return defaultAncestor;
		}

        /// <summary>
        /// Applies new X-coordinate to node, accumulating Mod-values of previous levels.
        /// </summary>
        /// <param name="vertex">Current vertex.</param>
        /// <param name="modSum">Sum of Mods.</param>
		private void SecondWalk (IBuchheimNode vertex, float modSum)
		{
			vertex.InnerNode.Point.X = modSum + vertex.Preliminary;
			vertex.Children.ForEach (w => {
				SecondWalk (w, modSum + vertex.Mod);
			});
		}
	}
}