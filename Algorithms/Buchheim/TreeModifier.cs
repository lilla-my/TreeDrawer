namespace Algorithm.Buchheim
{
    public class TreeModifier
	{
		/// <summary>
		/// Executes the shifts for children of vertex.
		/// </summary>
		/// <param name="vertex">Vertex</param>
		public void ExecuteShifts (IBuchheimNode vertex)
		{
			float shift = 0;
			float change = 0;
			for (var i = vertex.Children.Count - 1; i >= 0; i--) {
				var child = vertex.Children [i];
				child.Preliminary += shift;
				child.Mod += shift;
				change += child.Change;
				shift += (child.Shift + change);
			}
		}

		/// <summary>
		/// Preparation for moving subtrees from left siblings to avoid intersections.
		/// </summary>
		/// <param name="left">Left node.</param>
		/// <param name="right">Right node.</param>
		/// <param name="shift">Difference to be applied between them.</param>
		public void MoveSubtree (IBuchheimNode left, IBuchheimNode right, float shift)
		{
			int subtrees = right.Order - left.Order;
			if (subtrees > 0) {
				left.Change += shift / subtrees;
				right.Change -= shift / subtrees;
				right.Shift += shift;
				right.Preliminary += shift;
				right.Mod += shift;
			}
		}

		/// <summary>
		/// Moves vertex to the right from left sibling based on left's Preliminary.
		/// </summary>
		/// <param name="vertex">Vertex.</param>
		/// <param name="offset">Offset.</param>
		public void MoveAsideFromLeftSibling (IBuchheimNode vertex, float offset = 0)
		{
			var leftSibling = vertex.GetLeftSibling ();
			if (leftSibling != null) {
				vertex.Preliminary += leftSibling.Preliminary + offset;
				vertex.Mod = leftSibling.Preliminary;
			}
		}

		/// <summary>
		/// Centers the vertex's Preliminary based on childrens Preliminary.
		/// </summary>
		/// <param name="vertex">Vertex.</param>
		public void CenterVertexBasedOnChildren (IBuchheimNode vertex)
		{
			if (vertex.Children.Count == 0) {
				return;
			}

			var first = vertex.GetLeftMostChild ();
			var last = vertex.GetRightMostChild ();

			vertex.Preliminary = .5f * (first.Preliminary + last.Preliminary);
		}
	}
}